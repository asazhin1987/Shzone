using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedZone.DAL.EF;
using SharedZone.DAL.Interfaces;
using SharedZone.DAL.Repositories;
using SharedZone.DAL.Entities;
using System.Data.Common;
using Effort;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SharedZone.Tests
{
	[TestClass]
	public class DALTests
	{
		SharedZoneContext db;
		IUnitOfWork uow;

		[TestInitialize]
		public void ContextInitialize()
		{
			DbConnection connection = DbConnectionFactory.CreateTransient();
			db = new SharedZoneContext(connection);
			uow = new EFUnitOfWork(db);
		}

		#region Data


		#endregion Data

		#region Database

		[TestMethod]
		public void DataBase_Initialised()
		{
			Assert.IsNotNull(db);
			Assert.IsNotNull(db.Clients);
			Assert.IsNotNull(db.Collections);
			Assert.IsNotNull(db.Hours);
			Assert.IsNotNull(db.IFCFileTypes);
			Assert.IsNotNull(db.IFCIncludeBoundaties);
			Assert.IsNotNull(db.IFCJobs);
			Assert.IsNotNull(db.JobLaunches);
			Assert.IsNotNull(db.Licenses);
			Assert.IsNotNull(db.Minutes);
			Assert.IsNotNull(db.NavisConvertElementsProperties);
			Assert.IsNotNull(db.NavisCoordinates);
			Assert.IsNotNull(db.NavisJobs);
			Assert.IsNotNull(db.NavisViews);
			Assert.IsNotNull(db.RevitJobs);
			Assert.IsNotNull(db.RevitModels);
			Assert.IsNotNull(db.RevitServers);
			Assert.IsNotNull(db.RevitVersions);
			Assert.IsNotNull(db.ServerLogs);
			Assert.IsNotNull(db.WeekDays);
		}

		[TestMethod]
		public void DataBase_Seed()
		{
			Assert.AreEqual(db.Hours.Count(), 24);
			Assert.AreEqual(db.Minutes.Count(), 4);
			Assert.AreEqual(db.IFCFileTypes.Count(), 6);
			Assert.AreEqual(db.IFCIncludeBoundaties.Count(), 3);
			Assert.AreEqual(db.NavisConvertElementsProperties.Count(), 3);
			Assert.AreEqual(db.NavisCoordinates.Count(), 2);
			Assert.AreEqual(db.RevitVersions.Count(), 8);
			Assert.AreEqual(db.WeekDays.Count(), 7);
		}

		#region CRUD

		[TestMethod]
		public async Task Add_ServerAsync()
		{
			//Arrage
			RevitServer item = new RevitServer() { Name = "Item1", RevitVersionId = 1 };
			//Act
			db.RevitServers.Add(item);
			await db.SaveChangesAsync();
			//Assert
			Assert.AreEqual(await db.RevitServers.CountAsync(), 1);
		}

		[TestMethod]
		public async Task Add_ClientAsync()
		{
			//Arrage
			Client item = new Client() { Name = "Item1", UserName = "UserName 1"};
			//Act
			db.Clients.Add(item);
			await db.SaveChangesAsync();
			//Assert
			Assert.AreEqual(await db.Clients.CountAsync(), 1);
			//Assert.AreEqual(db.Clients.First().Name, "Item1");
		}

		private async Task Add_Collection()
		{
			Client client = new Client() { Name = "Item1", UserName = "UserName 1" };
			Collection item = new Collection() { Name = "Item1", RevitVersionId = 1, ClientId = 1, HourId = 1, MinuteId = 1, ScheduleTime = new TimeSpan(0, 0, 0) };
			db.Clients.Add(client);
			await db.SaveChangesAsync();
			db.Collections.Add(item);
			await db.SaveChangesAsync();
		}

		[TestMethod]
		public async Task Add_CollectionAsync()
		{
			//Act
			await Add_Collection();
			//Assert
			Assert.AreEqual(await db.Collections.CountAsync(), 1);
		}

		[TestMethod]
		public async Task Add_IFCJobAsync()
		{
			//Arrage
			await Add_Collection();
			var item = new IFCJob() { CollectionId = 1, IFCFileTypeId = 1, IFCIncludeBoundatyId = 1 };
			//Act
			db.IFCJobs.Add(item);
			await db.SaveChangesAsync();
			//Assert
			Assert.AreEqual(await db.IFCJobs.CountAsync(), 1);
		}

		[TestMethod]
		public async Task Add_NavisJobAsync()
		{
			//Arrage
			await Add_Collection();
			var item = new NavisJob() { CollectionId = 1, NavisConvertElementsPropertyId = 1, NavisCoordinateId = 1, NavisViewId = 1 };
			//Act
			db.NavisJobs.Add(item);
			await db.SaveChangesAsync();
			//Assert
			Assert.AreEqual(await db.NavisJobs.CountAsync(), 1);
		}

		[TestMethod]
		public async Task Add_RevitJobAsync()
		{
			//Arrage
			await Add_Collection();
			var item = new RevitJob() { CollectionId = 1};
			//Act
			db.RevitJobs.Add(item);
			await db.SaveChangesAsync();
			//Assert
			Assert.AreEqual(await db.RevitJobs.CountAsync(), 1);
		}



		[TestMethod]
		public async Task Add_ModelsAsync()
		{
			//Arrage
			RevitServer server = new RevitServer() { Name = "Item1", RevitVersionId = 1 };
			var item1 = new RevitModel() { RevitServerId = 1, Path = @"C:\\Temp", IsFolder = true, Name = "Temp", Id = 1  };
			var item2 = new RevitModel() { RevitServerId = 1, Path = @"C:\\Temp\Model1", IsFolder = false, Name = "Model1", ParentId = 1 };
			var item3 = new RevitModel() { RevitServerId = 1, Path = @"C:\\Temp\Model2", IsFolder = false, Name = "Model2", ParentId = 1 };
			//Act

			db.RevitServers.Add(server);
			await db.SaveChangesAsync();
			db.RevitModels.AddRange(new List<RevitModel> {item1, item2, item3 });
			await db.SaveChangesAsync();
			//Assert
			Assert.AreEqual(await db.RevitModels.CountAsync(), 3);
			Assert.AreEqual(await db.RevitModels.Where(x => x.IsFolder).CountAsync(), 1);
			Assert.AreEqual(await db.RevitModels.Where(x => !x.IsFolder).CountAsync(), 2);
			Assert.AreEqual(await db.RevitModels.Where(x => x.ParentId == 1).CountAsync(), 2);
		}

		[TestMethod]
		public async Task Add_ServerLogsAsync()
		{
			RevitServer server = new RevitServer() { Name = "Item1", RevitVersionId = 1 };
			//Arrage
			ServerLog item = new ServerLog() { ServerId = 1, Odate = DateTime.Now, Message = "Message", Success = true };
			//Act
			db.RevitServers.Add(server);
			await db.SaveChangesAsync();
			db.ServerLogs.Add(item);
			await db.SaveChangesAsync();
			//Assert
			Assert.AreEqual(await db.ServerLogs.CountAsync(), 1);
		}

		[TestMethod]
		public async Task Add_LaunchesAsync()
		{
			await Add_Collection();

			//Arrage
			JobLaunch item = new JobLaunch() { ClientId = 1, CollectionId = 1, Message = "MSG", StartDateTime = DateTime.Now, WeekDayId = 1, Success = true };
			//Act
			db.JobLaunches.Add(item);
			await db.SaveChangesAsync();
			//Assert
			Assert.AreEqual(await db.JobLaunches.CountAsync(), 1);
		}

		//Каскадное удаление
		[TestMethod]
		public async Task RevitServer_Remove()
		{
			//Arrage
			await Add_ModelsAsync();
			await Add_ServerLogsAsync();
			//Act
			db.RevitServers.RemoveRange(db.RevitServers.ToList());
			await db.SaveChangesAsync();
			//Assert
			Assert.AreEqual(db.RevitServers.Count(), 0);
			Assert.AreEqual(db.RevitModels.Count(), 0);
			Assert.AreEqual(db.ServerLogs.Count(), 0);

		}

		[TestMethod]
		public async Task Collection_Remove()
		{
			//Arrage
			await Add_CollectionAsync();
			await Add_IFCJobAsync();
			await Add_NavisJobAsync();
			await Add_RevitJobAsync();
			await Add_LaunchesAsync();
			//Act
			db.Collections.RemoveRange(db.Collections.ToList());
			await db.SaveChangesAsync();
			//Assert
			Assert.AreEqual(db.Collections.Count(), 0);
			Assert.AreEqual(db.IFCJobs.Count(), 0);
			Assert.AreEqual(db.NavisJobs.Count(), 0);
			Assert.AreEqual(db.RevitJobs.Count(), 0);
			Assert.AreEqual(db.JobLaunches.Count(), 0);

		}


		#endregion CRUD

		#endregion

		#region UOW

		[TestMethod]
		public void UnitOfWork_Initialised()
		{
			//Assert
			Assert.IsNotNull(uow);
			Assert.IsNotNull(uow.Clients);
			Assert.IsNotNull(uow.Collections);
			Assert.IsNotNull(uow.Hours);
			Assert.IsNotNull(uow.IFCFileTypes);
			Assert.IsNotNull(uow.IFCIncludeBoundaties);
			Assert.IsNotNull(uow.IFCJobs);
			Assert.IsNotNull(uow.JobLaunches);
			Assert.IsNotNull(uow.Licenses);
			Assert.IsNotNull(uow.Minutes);
			Assert.IsNotNull(uow.NavisConvertElementsProperties);
			Assert.IsNotNull(uow.NavisCoordinates);
			Assert.IsNotNull(uow.NavisJobs);
			Assert.IsNotNull(uow.NavisViews);
			Assert.IsNotNull(uow.RevitJobs);
			Assert.IsNotNull(uow.RevitModels);
			Assert.IsNotNull(uow.RevitServers);
			Assert.IsNotNull(uow.RevitVersions);
			Assert.IsNotNull(uow.ServerLogs);
			Assert.IsNotNull(uow.WeekDays);
		}

		[TestMethod]
		public async Task Create_ServerAsync()
		{
			//Arrage
			RevitServer item = new RevitServer() { Name = "Item1", RevitVersionId = 1 };
			//Act
			await uow.RevitServers.CreateAsync(item);
			//Assert
			Assert.AreEqual(await uow.RevitServers.GetAll().CountAsync(), 1);
		}

		[TestMethod]
		public async Task Create_ClientAsync()
		{
			//Arrage
			Client item = new Client() { Name = "Item1", UserName = "UserName 1" };
			//Act
			await uow.Clients.CreateAsync(item);
			//Assert
			Assert.AreEqual(await uow.Clients.GetAll().CountAsync(),1);
		}



		[TestMethod]
		public async Task Create_CollectionAsync()
		{
			Client client = new Client() { Name = "Item1", UserName = "UserName 1" };
			Collection item = new Collection() { Name = "Item1", RevitVersionId = 1, ClientId = 1, HourId = 1, MinuteId = 1, ScheduleTime = new TimeSpan(0, 0, 0) };
			//Act
			await uow.Clients.CreateAsync(client);
			await uow.Collections.CreateAsync(item);
			//Assert
			Assert.AreEqual(await db.Collections.CountAsync(), 1);
		}

		[TestMethod]
		public async Task Create_IFCJobAsync()
		{
			//Arrage
			await Add_Collection();
			var item = new IFCJob() { CollectionId = 1, IFCFileTypeId = 1, IFCIncludeBoundatyId = 1 };
			//Act
			await uow.IFCJobs.CreateAsync(item);
			//Assert
			Assert.AreEqual(await uow.IFCJobs.GetAll().CountAsync(), 1);
		}

		[TestMethod]
		public async Task Create_NavisJobAsync()
		{
			//Arrage
			await Add_Collection();
			var item = new NavisJob() { CollectionId = 1, NavisConvertElementsPropertyId = 1, NavisCoordinateId = 1, NavisViewId = 1 };
			//Act
			await uow.NavisJobs.CreateAsync(item);
			//Assert
			Assert.AreEqual(await uow.NavisJobs.GetAll().CountAsync(), 1);
		}

		[TestMethod]
		public async Task Create_RevitJobAsync()
		{
			//Arrage
			await Add_Collection();
			var item = new RevitJob() { CollectionId = 1 };
			//Act
			await uow.RevitJobs.CreateAsync(item);
			//Assert
			Assert.AreEqual(await uow.RevitJobs.GetAll().CountAsync(), 1);
		}



		[TestMethod]
		public async Task Create_ModelsAsync()
		{
			//Arrage
			RevitServer server = new RevitServer() { Name = "Item1", RevitVersionId = 1 };
			var item1 = new RevitModel() { RevitServerId = 1, Path = @"C:\\Temp", IsFolder = true, Name = "Temp", Id = 1 };
			var item2 = new RevitModel() { RevitServerId = 1, Path = @"C:\\Temp\Model1", IsFolder = false, Name = "Model1", ParentId = 1 };
			var item3 = new RevitModel() { RevitServerId = 1, Path = @"C:\\Temp\Model2", IsFolder = false, Name = "Model2", ParentId = 1 };
			//Act

			await uow.RevitServers.CreateAsync(server);
			await uow.RevitModels.CreateAsync(item1);
			await uow.RevitModels.CreateAsync(item2);
			await uow.RevitModels.CreateAsync(item3);

			//Assert
			Assert.AreEqual(await uow.RevitModels.GetAll().CountAsync(), 3);
			Assert.AreEqual(await uow.RevitModels.GetAll().Where(x => x.IsFolder).CountAsync(), 1);
			Assert.AreEqual(await uow.RevitModels.GetAll().Where(x => !x.IsFolder).CountAsync(), 2);
			Assert.AreEqual(await uow.RevitModels.GetAll().Where(x => x.ParentId == 1).CountAsync(), 2);
		}

		[TestMethod]
		public async Task Create_ServerLogsAsync()
		{
			RevitServer server = new RevitServer() { Name = "Item1", RevitVersionId = 1 };
			//Arrage
			ServerLog item = new ServerLog() { ServerId = 1, Odate = DateTime.Now, Message = "Message", Success = true };
			//Act
			await uow.RevitServers.CreateAsync(server);
			await uow.ServerLogs.CreateAsync(item);
			//Assert
			Assert.AreEqual(await uow.ServerLogs.GetAll().CountAsync(), 1);
		}

		[TestMethod]
		public async Task Create_LaunchesAsync()
		{
			await Add_Collection();

			//Arrage
			JobLaunch item = new JobLaunch() { ClientId = 1, CollectionId = 1, Message = "MSG", StartDateTime = DateTime.Now, WeekDayId = 1, Success = true };
			//Act
			await uow.JobLaunches.CreateAsync(item);
			//Assert
			Assert.AreEqual(await uow.JobLaunches.GetAll().CountAsync(), 1);
		}

		//Каскадное удаление
		[TestMethod]
		public async Task UOW_RevitServer_Remove()
		{
			//Arrage
			await Add_ModelsAsync();
			await Add_ServerLogsAsync();
			//Act
			foreach (var item in await uow.RevitServers.GetAllAsync())
			{
				await uow.RevitServers.DeleteAsync(item);
			}
		
			//Assert
			Assert.AreEqual(await uow.RevitServers.GetAll().CountAsync(), 0);
			Assert.AreEqual(await uow.RevitModels.GetAll().CountAsync(), 0);
			Assert.AreEqual(await uow.ServerLogs.GetAll().CountAsync(), 0);

		}

		[TestMethod]
		public async Task UOW_Collection_Remove()
		{
			//Arrage
			await Add_CollectionAsync();
			await Add_IFCJobAsync();
			await Add_NavisJobAsync();
			await Add_RevitJobAsync();
			await Add_LaunchesAsync();
			//Act
			foreach (var item in await uow.Collections.GetAllAsync())
			{
				await uow.Collections.DeleteAsync(item);
			}
			//Assert
			Assert.AreEqual(await uow.Collections.GetAll().CountAsync(), 0);
			Assert.AreEqual(await uow.IFCJobs.GetAll().CountAsync(), 0);
			Assert.AreEqual(await uow.NavisJobs.GetAll().CountAsync(), 0);
			Assert.AreEqual(await uow.RevitJobs.GetAll().CountAsync(), 0);
			Assert.AreEqual(await uow.JobLaunches.GetAll().CountAsync(), 0);

		}

		#endregion UOW


	}
}
