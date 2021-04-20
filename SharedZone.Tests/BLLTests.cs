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
using SharedZone.IWebService;
using SharedZone.BLL.Services;
using SharedZone.DTO;
using SharedZone.IRevitService;

namespace SharedZone.Tests
{
	[TestClass]
	public class BaseSrcTests
	{
		protected SharedZoneContext db;
		protected IUnitOfWork uow;


		[TestInitialize]
		public void ContextInitialize()
		{
			DbConnection connection = DbConnectionFactory.CreateTransient();
			db = new SharedZoneContext(connection);
			uow = new EFUnitOfWork(db);
		}

	}

	[TestClass]
	public class WebServiceTests : BaseSrcTests
	{
		private IWebSrc src;

		[TestInitialize]
		public void Service_Initialize()
		{
			src = new WebService(uow);
		}


		[TestMethod]
		public async Task GetAllClientsAsync()
		{
			//Arrage
			var clients = new List<Client>() {
				new Client() { Name = "FirstUser", Id = 1, UserName = "FirstName" },
				new Client() { Name = "SecondtUser", Id = 2, UserName = "SecondName" },
				new Client() { Name = "ThirdUser", Id = 3, UserName = "ThirdName" } };
			db.Clients.AddRange(clients);
			await db.SaveChangesAsync();
			//Act
			var result1 = await src.GetAllClientsAsync();
			var result2 = await src.GetAllClientsAsync("Firstuser");
			var result3 = await src.GetAllClientsAsync("Secondname");
			//Assert
			Assert.IsNotNull(result1);
			Assert.IsNotNull(result2);
			Assert.IsNotNull(result3);
			Assert.AreEqual(result1.Count(), 3);
			Assert.AreEqual(result2.Count(), 1);
			Assert.AreEqual(result3.Count(), 1);
		}

		[TestMethod]
		public async Task GetAllCollectionsAsync()
		{
			//Arrage
			Client client = new Client() { Name = "Item1", UserName = "UserName 1" };

			var cols = new List<Collection>() {
			new Collection(){ Id = 1, Name = "Item1", Description = "", RevitVersionId = 1, ClientId = 1, HourId = 1, MinuteId = 1  },
			new Collection(){ Id = 2, Name = "Item2", Description = "Description", RevitVersionId = 1, ClientId = 1, HourId = 1, MinuteId = 1  },
			new Collection(){ Id = 3, Name = "Item3", Description = "Description Test", RevitVersionId = 1, ClientId = 1, HourId = 1, MinuteId = 1  }};
			db.Clients.Add(client);
			await db.SaveChangesAsync();

			db.Collections.AddRange(cols);
			await db.SaveChangesAsync();
			//Act
			var result1 = await src.GetAllCollectionsAsync();
			var result2 = await src.GetAllCollectionsAsync("Item2");
			var result3 = await src.GetAllCollectionsAsync("Description");
			//Assert
			Assert.IsNotNull(result1);
			Assert.IsNotNull(result2);
			Assert.IsNotNull(result3);
			Assert.AreEqual(result1.Count(), 3);
			Assert.AreEqual(result2.Count(), 1);
			Assert.AreEqual(result3.Count(), 2);
		}

		[TestMethod]
		public async Task GetAllServersAsync()
		{
			//Arrage
			var servers = new List<RevitServer>() { new RevitServer() { IsDirectory = false, RevitVersionId = 1, Id = 1, Name = "SRV1" },
			new RevitServer() { IsDirectory = false, RevitVersionId = 1, Id = 2, Name = "SERVER" },
			new RevitServer() { IsDirectory = false, RevitVersionId = 1, Id = 3, Name = "REVIRSRV" },
			new RevitServer() { IsDirectory = true, RevitVersionId = 1, Id = 4, Name = "Dir" }};
			db.RevitServers.AddRange(servers);
			await db.SaveChangesAsync();
			//Act
			var result1 = await src.GetAllServersAsync();
			var result2 = await src.GetAllServersAsync("srv");
			var result3 = await src.GetAllServersAsync("aaa");
			var result4 = await src.GetAllServersAsync("Dir");
			//Assert
			Assert.IsNotNull(result1);
			Assert.IsNotNull(result2);
			Assert.IsNotNull(result3);
			Assert.IsNotNull(result4);
			Assert.AreEqual(result1.Count(), 3);
			Assert.AreEqual(result2.Count(), 2);
			Assert.AreEqual(result3.Count(), 0);
			Assert.AreEqual(result4.Count(), 0);
		}

		[TestMethod]
		public async Task GetAllDirectoriesAsync()
		{
			//Arrage
			var servers = new List<RevitServer>() { new RevitServer() { IsDirectory = true, RevitVersionId = 1, Id = 1, Name = "SRV1" },
			new RevitServer() { IsDirectory = true, RevitVersionId = 1, Id = 2, Name = "SERVER" },
			new RevitServer() { IsDirectory = true, RevitVersionId = 1, Id = 3, Name = "REVIRSRV" },
			new RevitServer() { IsDirectory = false, RevitVersionId = 1, Id = 4, Name = "NotDir" }};
			db.RevitServers.AddRange(servers);
			await db.SaveChangesAsync();
			//Act
			var result1 = await src.GetAllDirectoriesAsync();
			var result2 = await src.GetAllDirectoriesAsync("srv");
			var result3 = await src.GetAllDirectoriesAsync("aaa");
			var result4 = await src.GetAllDirectoriesAsync("NotDir");
			//Assert
			Assert.IsNotNull(result1);
			Assert.IsNotNull(result2);
			Assert.IsNotNull(result3);
			Assert.IsNotNull(result4);
			Assert.AreEqual(result1.Count(), 3);
			Assert.AreEqual(result2.Count(), 2);
			Assert.AreEqual(result3.Count(), 0);
			Assert.AreEqual(result4.Count(), 0);
		}

		[TestMethod]
		public void GetAllIFCFileTypes()
		{
			//Act
			var result = src.GetAllIFCFileTypes();
			//Assert
			Assert.AreEqual(result.Count(), db.IFCFileTypes.Count());
			Assert.IsInstanceOfType(result, typeof(IEnumerable<NamedUnitDTO>));
		}

		[TestMethod]
		public async Task GetAllIFCFileTypesAsync()
		{
			//Act
			var result = await src.GetAllIFCFileTypesAsync();
			//Assert
			Assert.AreEqual(result.Count(), db.IFCFileTypes.Count());
			Assert.IsInstanceOfType(result, typeof(IEnumerable<NamedUnitDTO>));
		}

		[TestMethod]
		public void GetAllIFCIncludeBoundaties()
		{
			//Act
			var result = src.GetAllIFCIncludeBoundaties();
			//Assert
			Assert.AreEqual(result.Count(), db.IFCIncludeBoundaties.Count());
			Assert.IsInstanceOfType(result, typeof(IEnumerable<NamedUnitDTO>));
		}

		[TestMethod]
		public async Task GetAllIFCIncludeBoundatiesAsync()
		{
			//Act
			var result = await src.GetAllIFCIncludeBoundatiesAsync();
			//Assert
			Assert.AreEqual(result.Count(), db.IFCIncludeBoundaties.Count());
			Assert.IsInstanceOfType(result, typeof(IEnumerable<NamedUnitDTO>));
		}

		[TestMethod]
		public void GetAllNavisConvertElementsProperties()
		{
			//Act
			var result = src.GetAllNavisConvertElementsProperties();
			//Assert
			Assert.AreEqual(result.Count(), db.NavisConvertElementsProperties.Count());
			Assert.IsInstanceOfType(result, typeof(IEnumerable<NamedUnitDTO>));
		}

		[TestMethod]
		public async Task GetAllNavisConvertElementsPropertiesAsync()
		{
			//Act
			var result = await src.GetAllNavisConvertElementsPropertiesAsync();
			//Assert
			Assert.AreEqual(result.Count(), db.NavisConvertElementsProperties.Count());
			Assert.IsInstanceOfType(result, typeof(IEnumerable<NamedUnitDTO>));
		}

		[TestMethod]
		public void GetAllNavisCoordinates()
		{
			//Act
			var result = src.GetAllNavisCoordinates();
			//Assert
			Assert.AreEqual(result.Count(), db.NavisCoordinates.Count());
			Assert.IsInstanceOfType(result, typeof(IEnumerable<NamedUnitDTO>));
		}

		[TestMethod]
		public async Task GetAllNavisCoordinatesAsync()
		{
			//Act
			var result = await src.GetAllNavisCoordinatesAsync();
			//Assert
			Assert.AreEqual(result.Count(), db.NavisCoordinates.Count());
			Assert.IsInstanceOfType(result, typeof(IEnumerable<NamedUnitDTO>));
		}

		[TestMethod]
		public void GetAllNavisViews()
		{
			//Act
			var result = src.GetAllNavisViews();
			//Assert
			Assert.AreEqual(result.Count(), db.NavisViews.Count());
			Assert.IsInstanceOfType(result, typeof(IEnumerable<NamedUnitDTO>));
		}

		[TestMethod]
		public async Task GetAllNavisViewsAsync()
		{
			//Act
			var result = await src.GetAllNavisViewsAsync();
			//Assert
			Assert.AreEqual(result.Count(), db.NavisViews.Count());
			Assert.IsInstanceOfType(result, typeof(IEnumerable<NamedUnitDTO>));
		}

		[TestMethod]
		public void GetAllHours()
		{
			//Act
			var result = src.GetAllHours();
			//Assert
			Assert.AreEqual(result.Count(), db.Hours.Count());
			Assert.IsInstanceOfType(result, typeof(IEnumerable<NamedUnitDTO>));
		}

		[TestMethod]
		public async Task GetAllHoursAsync()
		{
			//Act
			var result = await src.GetAllHoursAsync();
			//Assert
			Assert.AreEqual(result.Count(), db.Hours.Count());
			Assert.IsInstanceOfType(result, typeof(IEnumerable<NamedUnitDTO>));
		}

		[TestMethod]
		public void GetAllMinutes()
		{
			//Act
			var result = src.GetAllMinutes();
			//Assert
			Assert.AreEqual(result.Count(), db.Minutes.Count());
			Assert.IsInstanceOfType(result, typeof(IEnumerable<NamedUnitDTO>));
		}

		[TestMethod]
		public async Task GetAllMinutesAsync()
		{
			//Act
			var result = await src.GetAllMinutesAsync();
			//Assert
			Assert.AreEqual(result.Count(), db.Minutes.Count());
			Assert.IsInstanceOfType(result, typeof(IEnumerable<NamedUnitDTO>));
		}

		[TestMethod]
		public void GetAllWeekDays()
		{
			//Act
			var result = src.GetAllWeekDays();
			//Assert
			Assert.AreEqual(result.Count(), db.WeekDays.Count());
			Assert.IsInstanceOfType(result, typeof(IEnumerable<NamedUnitDTO>));
		}

		[TestMethod]
		public async Task GetAllWeekDaysAsync()
		{
			//Act
			var result = await src.GetAllWeekDaysAsync();
			//Assert
			Assert.AreEqual(result.Count(), db.WeekDays.Count());
			Assert.IsInstanceOfType(result, typeof(IEnumerable<NamedUnitDTO>));
		}

		[TestMethod]
		public void GetAllRevitVersions()
		{
			//Act
			var result = src.GetAllRevitVersions();
			//Assert
			Assert.AreEqual(result.Count(), db.RevitVersions.Count());
			Assert.IsInstanceOfType(result, typeof(IEnumerable<NamedUnitDTO>));
		}

		[TestMethod]
		public async Task GetAllRevitVersionsAsync()
		{
			//Act
			var result = await src.GetAllRevitVersionsAsync();
			//Assert
			Assert.AreEqual(result.Count(), db.RevitVersions.Count());
			Assert.IsInstanceOfType(result, typeof(IEnumerable<NamedUnitDTO>));
		}

		//	/*GET ASYNC*/
		[TestMethod]
		public async Task GetCollectionAsync()
		{
			//Arrage
			int Id = 1;
			Client client = new Client() { Name = "Item1", UserName = "UserName 1" };
			Collection item = new Collection() { Name = "Item1", RevitVersionId = 1, ClientId = 1, HourId = 1, MinuteId = 1, ScheduleTime = new TimeSpan(0, 0, 0), Id = Id };
			NavisJob nJob = new NavisJob() { CollectionId = 1, NavisConvertElementsPropertyId = 1, NavisCoordinateId = 1, NavisViewId = 1 };
			RevitJob rJob = new RevitJob() { CollectionId = 1 };
			IFCJob iJob = new IFCJob() { CollectionId = 1, IFCFileTypeId = 1, IFCIncludeBoundatyId = 1 };
			IFCJob iJob2 = new IFCJob() { CollectionId = 1, IFCFileTypeId = 1, IFCIncludeBoundatyId = 1 };
			db.Clients.Add(client);
			await db.SaveChangesAsync();
			db.Collections.Add(item);
			await db.SaveChangesAsync();
			db.IFCJobs.Add(iJob);
			db.IFCJobs.Add(iJob2);
			db.RevitJobs.Add(rJob);
			db.NavisJobs.Add(nJob);

			await db.SaveChangesAsync();
			//Act
			var result = await src.GetCollectionAsync(Id);
			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(result.Name, item.Name);
			Assert.AreEqual(result.RevitVersionId, item.RevitVersionId);
			Assert.AreEqual(result.ClientId, item.ClientId);
			Assert.AreEqual(result.HourId, item.HourId);
			Assert.AreEqual(result.MinuteId, item.MinuteId);
			Assert.AreEqual(result.Worksets, item.Worksets);
			Assert.AreEqual(result.HourName, item.Hour.Name);
			Assert.AreEqual(result.MinuteName, item.Minute.Name);

			Assert.IsNotNull(result.IFCJobsDTO);
			Assert.IsNotNull(result.NavisJobsDTO);
			Assert.IsNotNull(result.RevitJobsDTO);

			Assert.AreEqual(result.IFCJobsDTO.Count(), 2);
			Assert.AreEqual(result.RevitJobsDTO.Count(), 1);
			Assert.AreEqual(result.NavisJobsDTO.Count(), 1);
		}

		[TestMethod]
		public async Task GetServerAsync()
		{
			//Arrage
			int Id = 1;
			var item = new ServerDTO() { Name = "Server1", RevitVersionId = 1, Id = Id };
			await src.CreateServerAsync(item);
			//Act
			var _item = await src.GetServerAsync(Id);
			//Assert
			Assert.IsNotNull(_item);
			Assert.IsInstanceOfType(_item, typeof(ServerDTO));
			Assert.IsTrue(_item.Name == "Server1");
			Assert.IsTrue(_item.RevitVersionId == 1);
		}

		[TestMethod]
		public async Task GetClientAsync()
		{
			//Arrage
			int Id = 1;
			Client client = new Client() { Name = "Item1", UserName = "UserName 1", Id = Id };
			await uow.Clients.CreateAsync(client);
			//Act
			var _item = await src.GetClientAsync(Id);
			//Assert
			Assert.IsNotNull(_item);
		}

		[TestMethod]
		public async Task GetRevitJob()
		{
			await CreateCollectionAsync();
			//Arrage
			var job = new RevitJob()
			{
				CollectionId = 1,
				Id = 1,
				Name = "Name",
				Path = "Path",
				ExceptionViews = "ExceptionViews",
				AffixesLinks = "AffixesLinks",
				AffixLinks = true,
				Purge = true,
				TruncateLinks = true,
				TruncateView = true
			};
			db.RevitJobs.Add(job);
			db.SaveChanges();
			//Act
			var result = await src.GetRevitJob(1);
			//Assert
			Assert.IsNotNull(result);
			Assert.IsNull(result.CollectionName);
			Assert.AreEqual(result.Name, job.Name);
			Assert.AreEqual(result.CollectionId, job.CollectionId);
			Assert.AreEqual(result.AffixesLinks, job.AffixesLinks);
			Assert.AreEqual(result.AffixLinks, job.AffixLinks);
			Assert.AreEqual(result.ExceptionViews, job.ExceptionViews);
			Assert.AreEqual(result.Path, job.Path);
			Assert.AreEqual(result.Purge, job.Purge);
			Assert.AreEqual(result.TruncateLinks, job.TruncateLinks);
			Assert.AreEqual(result.TruncateView, job.TruncateView);

		}

		[TestMethod]
		public async Task GetNavisJob()
		{
			//Arrage
			await CreateCollectionAsync();
			var newjob = new NavisJob()
			{
				Id = 1,
				CollectionId = 1,
				ConvertIdentif = true,
				DevideFiles = true,
				ExportGeometryRoom = true,
				Name = "Name",
				NavisConvertElementsPropertyId = 2,
				NavisCoordinateId = 2,
				NavisViewId = 2,
				Path = "path",
				TransformConstructionElements = true,
				TransformElementsProperty = true,
				TransformLinkedFiles = true,
				TransformRoom = true,
				TransformURL = true,
				ViewName = "view name"
			};
			db.NavisJobs.Add(newjob);
			db.SaveChanges();
			//Act
			var result = await src.GetNavisJob(1);
			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(result.Name, newjob.Name);
			Assert.AreEqual(result.ConvertIdentif, newjob.ConvertIdentif);
			Assert.AreEqual(result.DevideFiles, newjob.DevideFiles);
			Assert.AreEqual(result.ExportGeometryRoom, newjob.ExportGeometryRoom);
			Assert.AreEqual(result.Path, newjob.Path);
			Assert.AreEqual(result.NavisConvertElementsPropertyId, newjob.NavisConvertElementsPropertyId);
			Assert.AreEqual(result.NavisCoordinateId, newjob.NavisCoordinateId);
			Assert.AreEqual(result.TransformConstructionElements, newjob.TransformConstructionElements);
			Assert.AreEqual(result.TransformElementsProperty, newjob.TransformElementsProperty);
			Assert.AreEqual(result.TransformLinkedFiles, newjob.TransformLinkedFiles);
			Assert.AreEqual(result.TransformRoom, newjob.TransformRoom);
			Assert.AreEqual(result.TransformURL, newjob.TransformURL);
			Assert.AreEqual(result.ViewName, newjob.ViewName);
		}

		[TestMethod]
		public async Task GetIFCJob()
		{
			await CreateCollectionAsync();
			var newjob = new IFCJob()
			{
				Id = 1,
				CollectionId = 1,
				BasicValues = true,
				CurrentView = true,
				CurrentViewName = "new view name",
				DivideWalls = true,
				IFCFileTypeId = 2,
				IFCIncludeBoundatyId = 2,
				Path = "new path",
				Name = "new Name"
			};
			db.IFCJobs.Add(newjob);
			db.SaveChanges();
			//Act
			var result = await src.GetIFCJob(1);
			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(result.BasicValues, newjob.BasicValues);
			Assert.AreEqual(result.CurrentView, newjob.CurrentView);
			Assert.AreEqual(result.CurrentViewName, newjob.CurrentViewName);
			Assert.AreEqual(result.DivideWalls, newjob.DivideWalls);
			Assert.AreEqual(result.IFCFileTypeId, newjob.IFCFileTypeId);
			Assert.AreEqual(result.IFCIncludeBoundatyId, newjob.IFCIncludeBoundatyId);
			Assert.AreEqual(result.Path, newjob.Path);
			Assert.AreEqual(result.Name, newjob.Name);

		}

		[TestMethod]
		public async Task GetAllServersFullDataAsync()
		{
			//Arrage
			RevitServer server = new RevitServer() { Name = "Item1", RevitVersionId = 1 };
			var item1 = new RevitModel() { RevitServerId = 1, Path = @"C:\\Temp", IsFolder = true, Name = "Temp", Id = 1 };
			var item2 = new RevitModel() { RevitServerId = 1, Path = @"C:\\Temp\Model1", IsFolder = false, Name = "Model1", ParentId = 1 };
			var item3 = new RevitModel() { RevitServerId = 1, Path = @"C:\\Temp\Model2", IsFolder = false, Name = "Model2", ParentId = 1 };
			db.RevitServers.Add(server);
			db.SaveChanges();
			db.RevitModels.Add(item1);
			db.RevitModels.Add(item2);
			db.RevitModels.Add(item3);
			db.SaveChanges();
			var coll = new Collection() { Id = 1, Name = "name", RevitModels = new List<RevitModel> { item2 } };
			db.Collections.Add(coll);
			//Act
			var result = await src.GetAllServersFullDataAsync(1);
			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(result.First().Name, server.Name);
			Assert.AreEqual(result.First().RevitVersionName, server.RevitVersion.Name);
			Assert.AreEqual(result.First().Folders.Count(), 1);
			Assert.AreEqual(result.First().Files.Count(), 0);
			Assert.AreEqual(result.First().Folders.First().Name, item1.Name);
		}

		[TestMethod]
		public async Task GetFilesAsync()
		{
			//Arrage
			int Id = 1;
			await CreateCollectionAsync();
			var col = db.Collections.Find(Id);
			RevitServer server = new RevitServer() { Name = "Item1", RevitVersionId = 1 };
			var item1 = new RevitModel() { RevitServerId = 1, Path = @"C:\\Temp", IsFolder = true, Name = "Temp", Id = 1 };
			var item2 = new RevitModel() { RevitServerId = 1, Path = @"C:\\Temp\Model1", IsFolder = false, Name = "Model1", ParentId = 1, Collections = new List<Collection> { col } };
			var item3 = new RevitModel() { RevitServerId = 1, Path = @"C:\\Temp\Model2", IsFolder = false, Name = "Model2", ParentId = 1, Collections = new List<Collection> { col } };
			db.RevitServers.Add(server);
			db.SaveChanges();
			db.RevitModels.Add(item1);
			db.RevitModels.Add(item2);
			db.RevitModels.Add(item3);
			db.SaveChanges();

			//Act
			var result = await src.GetFilesAsync(Id);
			//Assert
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Count() == 1);
			Assert.IsTrue(result.First().Files.Count() == 2);
		}

		//	/*CREATE ASYNC*/
		[TestMethod]
		public async Task CreateCollectionAsync()
		{
			//Arrage
			int Id = 1;
			Client client = new Client() { Name = "Item1", UserName = "UserName 1" };
			db.Clients.Add(client);
			await db.SaveChangesAsync();

			CollectionDTO item = new CollectionDTO() { Id = Id, Name = "Item1", RevitVersionId = 1, ClientId = 1, HourId = 1, MinuteId = 1 };
			//Act
			await src.CreateCollectionAsync(item);
			var result = db.Collections.Find(Id);
			//Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task CreateServerAsync()
		{
			//Arrage
			var item = new ServerDTO() { Name = "Server1", RevitVersionId = 1 };
			//Act
			await src.CreateServerAsync(item);
			//Assert
			Assert.IsTrue(db.RevitServers.Count() > 0);
		}

		[TestMethod]
		public async Task CreateRevitJobAsync()
		{
			int Id = 1;
			await CreateCollectionAsync();
			//Arrage
			var job = new RevitJobDTO() { CollectionId = 1, Name = "Job1" };
			//Act
			await src.CreateRevitJobAsync(job);
			var result = db.RevitJobs.Find(Id);
			//Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task CreateNavisJobAsync()
		{
			int Id = 1;
			await CreateCollectionAsync();
			//Arrage
			var job = new NavisJobDTO() { CollectionId = 1, Name = "Job1", NavisConvertElementsPropertyId = 1, NavisCoordinateId = 1, NavisViewId = 1 };
			//Act
			await src.CreateNavisJobAsync(job);
			var result = db.NavisJobs.Find(Id);
			//Assert
			Assert.IsNotNull(result);

		}

		[TestMethod]
		public async Task CreateIFCJobAsync()
		{
			int Id = 1;
			await CreateCollectionAsync();
			//Arrage
			var job = new IFCJobDTO() { CollectionId = 1, Name = "Job1", IFCFileTypeId = 1, IFCIncludeBoundatyId = 1 };
			//Act
			await src.CreateIFCJobAsync(job);
			var result = db.IFCJobs.Find(Id);
			//Assert
			Assert.IsNotNull(result);
		}

		//	/*UPDATE ASYNC*/
		[TestMethod]
		public async Task UpdateCollectionAsync()
		{
			//Arrage
			int Id = 1;
			Client client = new Client() { Name = "Item1", UserName = "UserName 1" };
			Client client2 = new Client() { Name = "Item2", UserName = "UserName 2" };
			db.Clients.Add(client);
			db.Clients.Add(client2);
			await db.SaveChangesAsync();

			var oldItem = new Collection() { Id = Id, ClientId = 1, Name = "Name", Description = "Desc", Detach = false, Audit = false, HourId = 1, MinuteId = 1, RevitVersionId = 1, Worksets = false };
			db.Collections.Add(oldItem);
			await db.SaveChangesAsync();

			var newItem = new CollectionDTO() { Id = Id, ClientId = 2, Name = "NewName", Description = "NewDesc", Detach = true, Audit = true, HourId = 2, MinuteId = 2, RevitVersionId = 2, Worksets = true };
			//Act
			await src.UpdateCollectionAsync(newItem);
			var result = await db.Collections.FindAsync(Id);
			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(result.Name, newItem.Name);
			Assert.AreEqual(result.ClientId, newItem.ClientId);
			Assert.AreEqual(result.Description, newItem.Description);
			Assert.AreEqual(result.Detach, newItem.Detach);
			Assert.AreEqual(result.Audit, newItem.Audit);
			Assert.AreEqual(result.HourId, newItem.HourId);
			Assert.AreEqual(result.MinuteId, newItem.MinuteId);
			Assert.AreEqual(result.RevitVersionId, newItem.RevitVersionId);
			Assert.AreEqual(result.Worksets, newItem.Worksets);
		}

		[TestMethod]
		public async Task UpdateServerAsync()
		{
			//Arrage
			await CreateServerAsync();
			var newserver = new ServerDTO() { Id = 1, RevitVersionId = 2, Name = "New Name" };
			//Act
			await src.UpdateServerAsync(newserver);
			var result = db.RevitServers.Find(1);
			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(result.RevitVersionId, newserver.RevitVersionId);
			Assert.AreEqual(result.Name, newserver.Name);
		}

		[TestMethod]
		public async Task UpdateRevitJobAsync()
		{
			//Arrage
			await CreateRevitJobAsync();
			var newjob = new RevitJobDTO() {
				Id = 1,
				AffixesLinks = "Links",
				AffixLinks = true,
				Name = "New Name",
				ExceptionViews = "ExceptionViews",
				Path = "New Path",
				Purge = true,
				TruncateLinks = true,
				TruncateView = true,
				CollectionId = 1
			};
			//Act
			await src.UpdateRevitJobAsync(newjob);
			var result = db.RevitJobs.Find(1);
			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(result.Name, newjob.Name);
			Assert.AreEqual(result.AffixesLinks, newjob.AffixesLinks);
			Assert.AreEqual(result.AffixLinks, newjob.AffixLinks);
			Assert.AreEqual(result.ExceptionViews, newjob.ExceptionViews);
			Assert.AreEqual(result.Path, newjob.Path);
			Assert.AreEqual(result.Purge, newjob.Purge);
			Assert.AreEqual(result.TruncateLinks, newjob.TruncateLinks);
			Assert.AreEqual(result.TruncateView, newjob.TruncateView);

		}

		[TestMethod]
		public async Task UpdateNavisJobAsync()
		{
			//Arrage
			await CreateNavisJobAsync();
			var newjob = new NavisJobDTO() {
				Id = 1,
				CollectionId = 1,
				ConvertIdentif = true,
				DevideFiles = true,
				ExportGeometryRoom = true,
				Name = "new Name",
				NavisConvertElementsPropertyId = 2,
				NavisCoordinateId = 2,
				NavisViewId = 2,
				Path = "new path",
				TransformConstructionElements = true,
				TransformElementsProperty = true,
				TransformLinkedFiles = true,
				TransformRoom = true,
				TransformURL = true,
				ViewName = "new view name"
			};
			//Act
			await src.UpdateNavisJobAsync(newjob);
			var result = db.NavisJobs.Find(1);
			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(result.Name, newjob.Name);
			Assert.AreEqual(result.ConvertIdentif, newjob.ConvertIdentif);
			Assert.AreEqual(result.DevideFiles, newjob.DevideFiles);
			Assert.AreEqual(result.ExportGeometryRoom, newjob.ExportGeometryRoom);
			Assert.AreEqual(result.Path, newjob.Path);
			Assert.AreEqual(result.NavisConvertElementsPropertyId, newjob.NavisConvertElementsPropertyId);
			Assert.AreEqual(result.NavisCoordinateId, newjob.NavisCoordinateId);
			Assert.AreEqual(result.TransformConstructionElements, newjob.TransformConstructionElements);
			Assert.AreEqual(result.TransformElementsProperty, newjob.TransformElementsProperty);
			Assert.AreEqual(result.TransformLinkedFiles, newjob.TransformLinkedFiles);
			Assert.AreEqual(result.TransformRoom, newjob.TransformRoom);
			Assert.AreEqual(result.TransformURL, newjob.TransformURL);
			Assert.AreEqual(result.ViewName, newjob.ViewName);
		}

		[TestMethod]
		public async Task UpdateIFCJobAsync()
		{
			//Arrage
			await CreateIFCJobAsync();
			var newjob = new IFCJobDTO()
			{
				Id = 1,
				CollectionId = 1,
				BasicValues = true,
				CurrentView = true,
				CurrentViewName = "new view name",
				DivideWalls = true,
				IFCFileTypeId = 2,
				IFCIncludeBoundatyId = 2,
				Path = "new path",
				Name = "new Name"
			};
			//Act
			await src.UpdateIFCJobAsync(newjob);
			var result = db.IFCJobs.Find(1);
			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(result.BasicValues, newjob.BasicValues);
			Assert.AreEqual(result.CurrentView, newjob.CurrentView);
			Assert.AreEqual(result.CurrentViewName, newjob.CurrentViewName);
			Assert.AreEqual(result.DivideWalls, newjob.DivideWalls);
			Assert.AreEqual(result.IFCFileTypeId, newjob.IFCFileTypeId);
			Assert.AreEqual(result.IFCIncludeBoundatyId, newjob.IFCIncludeBoundatyId);
			Assert.AreEqual(result.Path, newjob.Path);
			Assert.AreEqual(result.Name, newjob.Name);
		}



		/*REMOVE ASYNC*/
		[TestMethod]
		public async Task RemoveCollectionAsync()
		{
			//Arrage
			int Id = 1;
			await CreateCollectionAsync();

			var njob = new NavisJob() { CollectionId = 1, Name = "Job1", NavisConvertElementsPropertyId = 1, NavisCoordinateId = 1, NavisViewId = 1 };
			var ijob = new IFCJob() { CollectionId = 1, Name = "Job1", IFCFileTypeId = 1, IFCIncludeBoundatyId = 1 };
			var rjob = new RevitJob() { CollectionId = 1, Name = "Job1" };
			var client = new Client() { };
			JobLaunch launch = new JobLaunch() { ClientId = 1, CollectionId = 1, Message = "MSG", StartDateTime = DateTime.Now, WeekDayId = 1, Success = true };
			db.RevitJobs.Add(rjob);
			db.NavisJobs.Add(njob);
			db.IFCJobs.Add(ijob);
			db.Clients.Add(client);
			db.SaveChanges();
			db.JobLaunches.Add(launch);
			db.SaveChanges();
			//Act
			await src.RemoveCollectionAsync(Id);
			var nresults = db.NavisJobs.Count();
			var rresults = db.RevitJobs.Count();
			var iresults = db.IFCJobs.Count();
			var lresults = db.JobLaunches.Count();
			//Assert
			Assert.AreEqual(nresults, 0);
			Assert.AreEqual(rresults, 0);
			Assert.AreEqual(iresults, 0);
			Assert.AreEqual(lresults, 0);
		}

		[TestMethod]
		public async Task RemoveServerAsync()
		{
			//Arrage
			int Id = 1;
			await CreateCollectionAsync();
			var col = db.Collections.First();
			RevitServer server = new RevitServer() { Name = "Item1", RevitVersionId = 1, Id = Id };
			var item1 = new RevitModel() { RevitServerId = 1, Path = @"C:\\Temp", IsFolder = true, Name = "Temp", Id = 1 };
			var item2 = new RevitModel() { RevitServerId = 1, Path = @"C:\\Temp\Model1", IsFolder = false, Name = "Model1", ParentId = 1, Collections = new List<Collection> { col } };
			var item3 = new RevitModel() { RevitServerId = 1, Path = @"C:\\Temp\Model2", IsFolder = false, Name = "Model2", ParentId = 1, Collections = new List<Collection> { col } };
			db.RevitServers.Add(server);
			db.SaveChanges();
			db.RevitModels.Add(item1);
			db.RevitModels.Add(item2);
			db.RevitModels.Add(item3);
			db.SaveChanges();
			//Act
			await src.RemoveServerAsync(Id);
			var resultsmodel = db.RevitModels.Where(x => x.RevitServerId == Id).Count();
			var resultcol = db.Collections.First().RevitModels.Count();
			//Assert
			Assert.AreEqual(resultsmodel, 0);
			Assert.AreEqual(resultcol, 0);
		}

		[TestMethod]
		public async Task RemoveRevitJobAsync()
		{
			//Arrage
			int Id = 1;
			await CreateCollectionAsync();
			RevitJob job = new RevitJob() { Id = 1, CollectionId = 1, Name = "Name" };
			db.RevitJobs.Add(job);
			db.SaveChanges();
			//Act
			await src.RemoveRevitJobAsync(Id);
			var result = db.RevitJobs.Find(Id);
			//Assert
			Assert.IsNull(result);
		}

		[TestMethod]
		public async Task RemoveNavisJobAsync()
		{
			//Arrage
			int Id = 1;
			await CreateCollectionAsync();
			NavisJob job = new NavisJob() { Id = 1, CollectionId = 1, Name = "Name", NavisConvertElementsPropertyId = 1, NavisCoordinateId = 1, NavisViewId = 1 };
			db.NavisJobs.Add(job);
			db.SaveChanges();
			//Act
			await src.RemoveNavisJobAsync(Id);
			var result = db.NavisJobs.Find(Id);
			//Assert
			Assert.IsNull(result);
		}

		[TestMethod]
		public async Task RemoveIFCJobAsync()
		{
			//Arrage
			int Id = 1;
			await CreateCollectionAsync();
			IFCJob job = new IFCJob() { Id = 1, CollectionId = 1, Name = "Name", IFCIncludeBoundatyId = 1, IFCFileTypeId = 1 };
			db.IFCJobs.Add(job);
			db.SaveChanges();
			//Act
			await src.RemoveIFCJobAsync(Id);
			var result = db.IFCJobs.Find(Id);
			//Assert
			Assert.IsNull(result);
		}

		[TestMethod]
		public async Task ClearModels()
		{
			//Arrage
			int Id = 1;
			await CreateCollectionAsync();
			var col = db.Collections.Find(Id);
			RevitServer server = new RevitServer() { Name = "Item1", RevitVersionId = 1 };
			var item1 = new RevitModel() { RevitServerId = 1, Path = @"C:\\Temp", IsFolder = true, Name = "Temp", Id = 1 };
			var item2 = new RevitModel() { RevitServerId = 1, Path = @"C:\\Temp\Model1", IsFolder = false, Name = "Model1", ParentId = 1, Collections = new List<Collection> { col } };
			var item3 = new RevitModel() { RevitServerId = 1, Path = @"C:\\Temp\Model2", IsFolder = false, Name = "Model2", ParentId = 1, Collections = new List<Collection> { col } };
			db.RevitServers.Add(server);
			db.SaveChanges();
			db.RevitModels.Add(item1);
			db.RevitModels.Add(item2);
			db.RevitModels.Add(item3);
			db.SaveChanges();
			//Act
			await src.ClearModels(Id);
			var results = db.RevitModels.Where(x => x.Collections.Select(s => s.Id).Contains(Id)).ToList();
			//Assert
			Assert.IsTrue(results.Count() == 0);
		}


		[TestMethod]
		public async Task UpdateModelsAsync()
		{
			//Arrage
			//Arrage
			int Id = 1;
			await CreateCollectionAsync();
			var col = db.Collections.Find(Id);
			RevitServer server = new RevitServer() { Name = "Item1", RevitVersionId = 1 };
			var item1 = new RevitModel() { RevitServerId = 1, Path = @"C:\\Temp", IsFolder = true, Name = "Temp", Id = 1 };
			var item2 = new RevitModel() { Id = 2, RevitServerId = 1, Path = @"C:\\Temp\Model1", IsFolder = false, Name = "Model1", ParentId = 1, Collections = new List<Collection> { col } };
			var item3 = new RevitModel() { Id = 3, RevitServerId = 1, Path = @"C:\\Temp\Model2", IsFolder = false, Name = "Model2", ParentId = 1 };
			db.RevitServers.Add(server);
			db.SaveChanges();
			db.RevitModels.Add(item1);
			db.RevitModels.Add(item2);
			db.RevitModels.Add(item3);
			db.SaveChanges();
			//Act
			var resultsBefore = db.RevitModels.Where(x => x.Collections.Select(s => s.Id).Contains(Id)).ToList();
			await src.UpdateModelsAsync(Id, new List<int> { 2, 3 });
			var results = db.RevitModels.Where(x => x.Collections.Select(s => s.Id).Contains(Id)).ToList();
			//Assert
			Assert.IsTrue(resultsBefore.Count() == 1);
			Assert.IsTrue(results.Count() == 2);
		}

		///*LICENSE*/
		//[TestMethod]
		//	Task<LicenseDTO> GetLicenseAsync()
		//{
		//	//Arrage

		//	//Act

		//	//Assert

		//}

		//[TestMethod]
		//	bool CheckLicense();

		//	[TestMethod]
		//	Task SetLicenseAsync(string key)
		//{
		//	//Arrage

		//	//Act

		//	//Assert

		//}


	}

	[TestClass]
	public class RevitServiceTests : BaseSrcTests
	{
		private IRevitSrc src;

		[TestInitialize]
		public void Service_Initialize()
		{
			src = new RevitService(uow);
		}

		[TestMethod]
		public void GetSchedule()
		{
			//Arrage

			//Act

			//Assert
		}


		[TestMethod]
		public void SetResult()
		{
			//Arrage

			//Act

			//Assert
		}

		[TestMethod]
		public void GetCollections()
		{
			//Arrage

			//Act

			//Assert
		}

		[TestMethod]
		public void GetCollectiuon()
		{
			//Arrage
			db.Clients.Add(new Client() { Name = "Item1", UserName = "UserName 1" });
			db.SaveChanges();
			db.Collections.Add(new Collection() { Id = 1, Name = "Item1", Description = "", RevitVersionId = 1, ClientId = 1, HourId = 1, MinuteId = 1 });
			db.SaveChanges();
			//Act
			var result = src.GetCollection(1);
			//Assert
			Assert.IsNotNull(result);
		}
	}
}
