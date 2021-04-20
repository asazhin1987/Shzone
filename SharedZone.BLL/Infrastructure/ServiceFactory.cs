using Ninject;
using Quartz;
using SharedZone.DAL.Interfaces;
using SharedZone.DAL.Repositories;
using SharedZone.IWebService;
using SharedZone.ISysService;
using SharedZone.IRevitService;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SharedZone.BLL.Services;
using Bimacad.Sys;
using Quartz.Impl;
using SharedZone.BLL.Schedules;

namespace SharedZone.BLL.Infrastructure
{
	public class ServiceFactory : IDisposable
	{
		private readonly string connectionString;
		private readonly string providerName;
		private Dictionary<string, ServiceHost> ServiceHosts { get; set; }

		private IScheduler scheduler;
		readonly StandardKernel kerner;

		//services
		public event EventHandler<ServiceEventArgs> OnServicesDisposed;
		public event EventHandler<ServiceEventArgs> OnServiceDisposed;
		public event EventHandler<ServiceEventArgs> OnServiceRuning;
		public event EventHandler<ServiceEventArgs> OnServiceRunned;
		public event EventHandler<ServiceEventArgs> OnServicesRunned;
		public event EventHandler<ServiceEventArgs> OnServiceFailure;
		public event EventHandler<ServiceEventArgs> OnServicesFailure;

		//db migrate
		public event EventHandler<ServiceEventArgs> OnBeginMigrate;
		public event EventHandler<ServiceEventArgs> OnEndMigrate;
		public event EventHandler<ServiceEventArgs> OnFailureMigrate;

		//schedule
		public event EventHandler<ServiceEventArgs> OnScheduleRunned;
		public event EventHandler<ServiceEventArgs> OnScheduleFailure;

		public ServiceFactory(string connStr, string providerName)
		{
			connectionString = connStr;
			ServiceHosts = new Dictionary<string, ServiceHost>();
			kerner = new StandardKernel();
			kerner.Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(connectionString);
			this.providerName = providerName;
		}

		public void Migrate()
		{
			OnBeginMigrate?.Invoke(this, new ServiceEventArgs("Begin Migrate"));
			try
			{
				var config = new DAL.Migrations.Configuration
				{
					TargetDatabase = new System.Data.Entity.Infrastructure.DbConnectionInfo(connectionString, providerName)
				};
				var context = new DAL.EF.SharedZoneContext(connectionString);
				var migrator = new DbMigrator(config, context);
				if (context.Database.Exists())
				{
					migrator.Update();
				}
				OnEndMigrate?.Invoke(this, new ServiceEventArgs("End Migrate success"));
			}
			catch (Exception ex)
			{
				OnFailureMigrate?.Invoke(this, new ServiceEventArgs(ex.Message, false));
			}

		}

		public void RunServices()
		{
			try
			{
				kerner.Bind<IWebSrc>().To<WebService>().WithConstructorArgument(kerner.Get<IUnitOfWork>());
				kerner.Bind<IRevitSrc>().To<RevitService>().WithConstructorArgument(kerner.Get<IUnitOfWork>());
				kerner.Bind<ISysSrc>().To<SysService>().WithConstructorArgument(kerner.Get<IUnitOfWork>());


				CreateEndpoint(typeof(IWebSrc), kerner.Get<IWebSrc>(), "SharedZoneWebService");
				CreateEndpoint(typeof(IRevitSrc), kerner.Get<IRevitSrc>(), "SharedZoneRevitService");
				CreateEndpoint(typeof(ISysSrc), kerner.Get<ISysSrc>(), "SharedZoneSysService");


				OnServicesRunned?.Invoke(this, new ServiceEventArgs("All Services Runned"));
			}
			catch (Exception ex)
			{
				OnServicesFailure?.Invoke(this, new ServiceEventArgs(ex.Message, false));
			}
		}

		private void CreateEndpoint(Type iType, IService inst, string Name)
		{
			try
			{
				OnServiceRuning?.Invoke(inst, new ServiceEventArgs($"Service {Name} Running"));
				ServiceHost Host = new ServiceHost(inst, new Uri($"http://localhost:80/{Name}")) { };

				BasicHttpBinding binding = new BasicHttpBinding() { MaxReceivedMessageSize = 2147483647 };
				Host.AddServiceEndpoint(iType, binding, "");
				Host.Open();
				ServiceHosts.Add(Name, Host);
				OnServiceRunned?.Invoke(inst, new ServiceEventArgs($"Service {Name} Runned"));
			}
			catch (Exception ex)
			{
				OnServiceFailure?.Invoke(inst, new ServiceEventArgs($"Service {Name} - {ex.Message}"));
				throw ex;
			}
		}

		#region Schedule
		public async Task RunSchedule()
		{
			try
			{
				scheduler = await StdSchedulerFactory.GetDefaultScheduler();
				await scheduler.Start();

				await RunSchedule<ServerDirectoryExplorer>("ServerAndDirectoryExplorer", "Schedules");
				Thread.Sleep(1000);
				await RunSchedule<CleanerLog>("CleanerLog", "Schedules", 1440);

			}
			catch (Exception ex)
			{
				OnScheduleFailure?.Invoke(this, new ServiceEventArgs(ex.Message, false));
			}

		}

		private async Task RunSchedule<T>(string Name, string GroupName, int interval = 1) where T : IJob
		{
			try
			{
				IJobDetail job = JobBuilder.Create<T>()
					.UsingJobData(new JobDataMap
					(new Dictionary<string, ISysSrc>() { { "src", kerner.Get<ISysSrc>() } })).Build();

				ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
					.WithIdentity(Name, GroupName)     // идентифицируем триггер с именем и группой
					.StartNow()                            // запуск сразу после начала выполнения
					.WithSimpleSchedule(x => x            // настраиваем выполнение действия
						.WithIntervalInMinutes(interval)          // через {} мин
						.RepeatForever())                   // бесконечное повторение
					.Build();                               // создаем триггер

				await scheduler.ScheduleJob(job, trigger);
				OnScheduleRunned?.Invoke(this, new ServiceEventArgs($"{Name} Runned"));
			}
			catch (Exception ex)
			{
				OnScheduleFailure?.Invoke(this, new ServiceEventArgs(ex.Message, false));
			}

		}

		public void Pause()
		{
			if (scheduler.IsStarted)
				scheduler.PauseAll();
		}

		public void Run()
		{
			if (!scheduler.IsStarted)
				scheduler.Start();
		}

		
		#endregion Schedule

		

		public void Dispose()
		{
			foreach (var host in ServiceHosts.Values)
			{
				if (host != null && host.State == CommunicationState.Opened)
				{
					host.Close();
					OnServiceDisposed?.Invoke(this, new ServiceEventArgs($"{host.Description} Disposed"));
				}
			}
			ServiceHosts.Clear();
			kerner.Dispose();
			if (!scheduler.IsShutdown)
				scheduler.Shutdown(true);

			OnServicesDisposed?.Invoke(this, new ServiceEventArgs("All Services Disposed"));
		}
	}
}
