using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Bimacad.Sys;
using SharedZone.DAL.Entities;
using SharedZone.DAL.Interfaces;
using SharedZone.DTO;
using SharedZone.IRevitService;
using System.Data.Entity;

namespace SharedZone.BLL.Services
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
	   InstanceContextMode = InstanceContextMode.Single,
	   IncludeExceptionDetailInFaults = true)]

	public class RevitService : BaseService, IRevitSrc
	{
		public RevitService(IUnitOfWork uw) : base(uw) { }

		public bool CheckConnection()
		{
			return true;
		}

		internal override Task<IEnumerable<ServerDTO>> GetAllServersAsync(bool isDir, string mask = "")
		{
			throw new NotImplementedException();
		}

		public IEnumerable<CollectionDTO> GetCollections(string clientName, string version)
		{
			var result = GetCollections(db.Collections.GetAll(), clientName, version);
			return GetCollections(result);

		}

		public IEnumerable<CollectionDTO> GetSchedule(string clientName, string version)
		{
			int dayId = DateTime.Now.GetWeekDayId();
			var now = DateTime.Now;
			var result = GetCollections(db.Collections.GetAll(), clientName, version);
			//фильтр по дню недели
			result = result.Where(x => x.WeekDays.Select(d => d.Id).Contains(dayId));
			//далее - по логам
			result = result.Where(x => x.JobLaunches.Where(l => l.HourId == x.HourId && l.MinuteId == x.MinuteId).Count() == 0);
			//теперь по времени
			result = result.Where(x =>
			x.Hour.HourValue < now.Hour
			|| (x.Hour.HourValue == now.Hour && x.Minute.MinuteValue <= now.Minute));
			return GetCollections(result);
		}

		

		private IEnumerable<CollectionDTO> GetCollections(IQueryable<Collection> qwuery)
		{
			return qwuery.Select(x => new CollectionDTO()
			{
				Id = x.Id,
				Detach = x.Detach,
				Audit = x.Audit,
				ClientId = x.ClientId.Value,
				ExceptionWorksets = x.ExceptionWorksets,
				HourId = x.HourId,
				MinuteId = x.MinuteId,
				Worksets = x.Worksets,
				RevitVersionId = x.RevitVersionId,
				Name = x.Name, 
				ModelsCount = x.RevitModels.Count()
				//RevitModelsDTO = x.RevitModels.Select(m => new RevitModelDTO()
				//{
				//	Id = m.Id,
				//	//Name = m.Name,
				//	//Path = m.Path,
				//}),
				//RevitJobsDTO = x.RevitJobs.Select(j => new RevitJobDTO()
				//{
				//	Id = j.Id,
				//	//Name = j.Name,
				//	//Path = j.Path,
					
				//}),
				//NavisJobsDTO = x.NavisJobs.Select(j => new NavisJobDTO()
				//{
				//	Id = j.Id,
				//	//Name = j.Name,
				//	//Path = j.Path,
				//}),
				//IFCJobsDTO = x.IFCJobs.Select(j => new IFCJobDTO()
				//{
				//	Id = j.Id,
				//	//Name = j.Name,
				//	//Path = j.Path,
				//})
			}).ToList();
		}

		public CollectionDTO GetCollection(int Id)
		{
			Task<Collection> t = Task.Run(() => db.Collections.GetAsync(Id));
			var item = t.Result;
			if(item == null)
				throw new FaultException<NotFound>(new NotFound());
			//db.RevitJobs.GetWithInclude(x => x.Collection.Equals(item), j => j.Collection);
			//db.NavisJobs.GetWithInclude(x => x.Collection.Equals(item), j => j.Collection);
			//db.IFCJobs.GetWithInclude(x => x.Collection.Equals(item), j => j.Collection);
			db.RevitJobs.GetAll().Where(x => x.CollectionId == Id).Load();
			db.NavisJobs.GetAll().Where(x => x.CollectionId == Id).Load();
			db.IFCJobs.GetAll().Where(x => x.CollectionId == Id).Load();

			//db.RevitModels.GetWithInclude(x => x.Collections.Contains(item), m => m.Collections);

			return (new CollectionDTO()
			{
				RevitModelsDTO = item.RevitModels.Select(x => new RevitModelDTO().Map(x)), 
				IFCJobsDTO = item.IFCJobs.Select(x => new IFCJobDTO().Map(x)),
				NavisJobsDTO = item.NavisJobs.Select(x => new NavisJobDTO().Map(x)),
				RevitJobsDTO = item.RevitJobs.Select(x => new RevitJobDTO().Map(x)),
			}).Map(item);
		}

		private IQueryable<Collection> GetCollections(IQueryable<Collection> qwuery, string clientName, string version)
		{
			var cliemt = db.Clients.GetAll().Where(x => x.Name == clientName).FirstOrDefault();
			var _version = db.RevitVersions.GetAll().Where(x => x.Name == version).FirstOrDefault();
			if (cliemt == null || _version == null)
				return new List<Collection>().AsQueryable();

			return qwuery.Where(x => x.ClientId == cliemt.Id
			&& (x.RevitVersionId == _version.Id || (x.RevitVersionId == 1 && x.RevitModels.Select(m => m.RevitServer.RevitVersionId == _version.Id).Count() > 0)));
		}

		public int CreateJobLaunch(JobLaunchDTO launch)
		{
			var _launch = new JobLaunch().Map<JobLaunch>(launch);
			try
			{
				Task t = Task.Run(() => db.JobLaunches.CreateAsync(_launch));
				t.Wait();
				return _launch.Id;
			}
			catch (Exception ex)
			{
				return 0;
			}
		}

		public bool SetStartResult(JobLaunchDTO launch)
		{
			try
			{
				Task<JobLaunch> _launchT = Task.Run(() => db.JobLaunches.GetAsync(launch.Id));
				_launchT.Wait();
				var _launch = _launchT.Result;
				if (_launch != null)
				{
					_launch.StartDateTime = launch.StartDateTime;
					Task t = Task.Run(() => db.JobLaunches.UpdateAsync(_launch));
					t.Wait();
					return true;
				}
				else
				{
					return false;
				}
			}
			catch(Exception ex)
			{
				return false;
			}
		}

		public bool SetEndResult(JobLaunchDTO launch)
		{

			Task<JobLaunch> _launchT = Task.Run(() => db.JobLaunches.GetAsync(launch.Id));
			var _launch = _launchT.Result;
			if (_launch != null)
			{
				try
				{
					_launch.EndDateTime = launch.EndDateTime;
					_launch.Success = launch.Success;
					_launch.Message = launch.Message;
					_launch.Duration = launch.EndDateTime.Value.Subtract(launch.StartDateTime.Value).Seconds;

					Task t = Task.Run(() => db.JobLaunches.UpdateAsync(_launch));
					t.Wait();
					return true;
				}
				catch(Exception ex)
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		public bool TakeLicense(string clientName, string userName)
		{
			//проверяем есть ли лицензия
			var lic = db.Licenses.Get(0);
			if (lic == null)
				throw new FaultException<NullKey>(new NullKey());

			(DateTime? date, int qnt) = GetLicenseParamsAsync(lic.Key);
			if (date == null || qnt == 0)
				throw new FaultException<WrongKey>(new WrongKey());
			//сверяем количество
			int usqnt = qnt - db.Clients.GetAll().Count();
			Client licpc = db.Clients.GetAll().Where(x => x.Name == clientName).FirstOrDefault();
			                                                                                                                 
			if (usqnt <= 0 && licpc == null)
				throw new FaultException<ZeroQnt>(new ZeroQnt());

			//если новый пк - регистрируем
			DateTime today = DateTime.Now;
			bool unic;
			if (licpc == null)
			{
				unic = true;
				licpc = new Client()
				{
					Name = clientName,
					UserName = userName,
					LastAccess = today,
					LastAccessDate = today.Date
				};
				Task t = Task.Run(() => db.Clients.CreateAsync(licpc));
				t.Wait();
				//db.Clients.CreateAsync(licpc).GetAwaiter().GetResult();
			}
			else
			{
				unic = today.Date != licpc.LastAccessDate;
				licpc.LastAccess = today;
				licpc.LastAccessDate = today.Date;
				licpc.UserName = userName;
				Task t = Task.Run(() => db.Clients.UpdateAsync(licpc));
				t.Wait();
				//db.Clients.UpdateAsync(licpc).GetAwaiter().GetResult();
			}
			//пишем статистику
			LicenseUsersStatistic stat = db.LicenseStatistics.GetAll().Where(x => x.Date == today.Date && x.ClientId == licpc.Id).FirstOrDefault();
			if (stat == null)
			{
				stat = new LicenseUsersStatistic()
				{
					Date = today.Date,
					MonthNum = DateTime.Now.Month,
					YearNum = DateTime.Now.Year,
					Qnt = 1,
					UniqQnt = 1,
					ClientId = licpc.Id
				};
				Task t = Task.Run(() => db.LicenseStatistics.CreateAsync(stat));
				t.Wait();
				//db.LicenseStatistics.CreateAsync(new LicenseUsersStatistic()
				//{
				//	Date = today.Date,
				//	MonthNum = DateTime.Now.Month,
				//	YearNum = DateTime.Now.Year,
				//	Qnt = 1,
				//	UniqQnt = 1,
				//	ClientId = licpc.Id
				//}).GetAwaiter().GetResult();
			}
			else
			{
				stat.Qnt += 1;
				Task t = Task.Run(() => db.LicenseStatistics.UpdateAsync(stat));
				t.Wait();
				//if (unic)
				//	stat.UniqQnt += 1;
				//db.LicenseStatistics.UpdateAsync(stat).GetAwaiter().GetResult();
			}
			return true;
			
		}
	}
}
