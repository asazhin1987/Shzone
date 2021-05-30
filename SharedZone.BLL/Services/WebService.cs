using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SharedZone.DAL.Interfaces;
using SharedZone.DTO;
using SharedZone.IWebService;
using System.Data.Entity;
using SharedZone.DAL.Entities;
using Bimacad.Sys;
using SharedZone.BLL.Extensions;
using System.Globalization;

namespace SharedZone.BLL.Services
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
	   InstanceContextMode = InstanceContextMode.Single,
	   IncludeExceptionDetailInFaults = true)]

	public class WebService : BaseService, IWebSrc
	{
		public WebService(IUnitOfWork uw) : base(uw) { }


		#region Collections

		public async Task<IEnumerable<CollectionDTO>> GetAllCollectionsAsync(string mask = "")
		{

			var iq = db.Collections.GetAll();
			if (mask != null && mask.Length > 0)
				iq = iq.Where(x => x.Name.ToLower().Contains(mask.ToLower()) || x.Description.ToLower().Contains(mask.ToLower()));
			return await iq.Select(x => new CollectionDTO()
			{
				Id = x.Id,
				Name = x.Name,
				Description = x.Description,
				HourValue = x.Hour.HourValue,
				MinuteValue = x.Minute.MinuteValue,
				ClientName = x.Client.Name,
				HourId = x.HourId,
				MinuteId = x.MinuteId,
				RevitJobsDTO = x.RevitJobs.Select(j => new RevitJobDTO
				{
					Id = j.Id
				}),
				NavisJobsDTO = x.NavisJobs.Select(j => new NavisJobDTO
				{
					Id = j.Id
				}),
				IFCJobsDTO = x.IFCJobs.Select(j => new IFCJobDTO
				{
					Id = j.Id
				}),
				JobLaunchDTOs = x.JobLaunches.Select(j => new
					JobLaunchDTO
					{
						Id = j.Id,
						HourId = j.HourId,
						MinuteId = j.MinuteId,
						StartDateTime = j.StartDateTime,
						EndDateTime = j.EndDateTime,
						Success = j.Success,
						Message = j.Message
					}),
				WeekDayDTOs = x.WeekDays.Select(w =>
					new WeekDayDTO
					{
						Id = w.Id,
						Name = w.Name,
					}),
				WeekDays = x.WeekDays.Select(w => w.Id)
			}).ToListAsync();
		}

		public async Task<int> CreateCollectionAsync(CollectionDTO item)
		{
			try
			{
				var model = new Collection()
				{
					Name = item.Name,
					Description = item.Description,
					HourId = 1,
					MinuteId = 1,
					Detach = true,
					Worksets = false,
					IsDefaultVersion = true
				};
				await db.Collections.CreateAsync(model);

				return model.Id;
			}
			catch (Exception ex)
			{
				throw new FaultException<ModelCheck>(new ModelCheck(ex));
			}
		}


		public async Task<CollectionDTO> GetCollectionAsync(int Id)
		{

			var coll = await db.Collections.GetAsync(Id);
			if (coll == null)
				throw new FaultException<NotFound>(new NotFound());

			await db.Hours.GetAll().LoadAsync();
			await db.Minutes.GetAll().LoadAsync();
			await db.RevitJobs.GetAll().Where(x => x.CollectionId == Id).LoadAsync();
			await db.NavisJobs.GetAll().Where(x => x.CollectionId == Id).LoadAsync();
			await db.IFCJobs.GetAll().Where(x => x.CollectionId == Id).LoadAsync();

			return Mapper.Map(new CollectionDTO()
			{
				HourValue = coll.Hour.HourValue,
				MinuteValue = coll.Minute.MinuteValue,
				ClientName = coll.Client?.Name,
				RevitJobsDTO = coll.RevitJobs.Select(x => new RevitJobDTO() { Id = x.Id, Name = x.Name }),
				NavisJobsDTO = coll.NavisJobs.Select(x => new NavisJobDTO() { Id = x.Id, Name = x.Name }),
				IFCJobsDTO = coll.IFCJobs.Select(x => new IFCJobDTO() { Id = x.Id, Name = x.Name }),
				ServersDTO = await GetFilesAsync(Id),
				WeekDays = coll.WeekDays.Select(w => w.Id).ToList(),
				WeekDayDTOs = coll.WeekDays.Select(w => new WeekDayDTO
				{
					Id = w.Id,
					Name = w.Name,
				}).ToList(),
				RevitModelsDTO = new List<RevitModelDTO>()
			}, coll);
		}

		public async Task RemoveCollectionAsync(int Id)
		{
			await db.Collections.DeleteAsync(Id);
		}

		public async Task UpdateCollectionAsync(CollectionDTO item)
		{
			var coll = await db.Collections.GetAsync(item.Id);
			if (coll == null)
				throw new FaultException<NotFound>(new NotFound());
			Mapper.Map(coll, item);
			try
			{
				await db.Collections.UpdateAsync(coll);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			await UpdateWeekDaysAsync(item.Id, item.WeekDays);
		}

		private async Task UpdateWeekDaysAsync(int Id, IEnumerable<int> wdays)
		{
			var item = await db.Collections.GetAsync(Id);

			item.WeekDays?.Clear();
			if (wdays != null)
			{
				foreach (int dayId in wdays)
				{
					try
					{
						item.WeekDays.Add(await db.WeekDays.GetAsync(dayId));
					}
					catch { continue; }
				}
			}
			await db.Collections.UpdateAsync(item);
		}

		public async Task UpdateModelsAsync(int collectionId, IEnumerable<int> files)
		{
			var item = await db.Collections.GetAsync(collectionId);
			if (item == null)
				throw new FaultException<NotFound>(new NotFound());

			await db.SqlInjector.ClearModelsAsync(collectionId);
			if (files != null)
				await db.SqlInjector.InsertModelsAsync(collectionId, files);

			//await db.RevitModels.GetAll().Where(x => x.Collections.Select(s => s.Id).Contains(collectionId)).AsNoTracking().LoadAsync();

			//item.RevitModels?.Clear();
			//try
			//{
			//	await db.Collections.UpdateAsync(item);
			//}
			//catch (Exception ex)
			//{

			//}
			//if (files != null)
			//{
			//	foreach (int fileId in files)
			//	{
			//		try
			//		{
			//			item.RevitModels.Add(await db.RevitModels.GetAsync(fileId));
			//		}
			//		catch { continue; }
			//	}
			//	try
			//	{
			//		await db.Collections.UpdateAsync(item);
			//	}
			//	catch (Exception ex)
			//	{

			//	}
			//}


		}


		public async Task ClearModels(int Id)
		{
			var item = await db.Collections.GetAsync(Id);
			if (item == null)
				throw new FaultException<NotFound>(new NotFound());
			await db.SqlInjector.ClearModelsAsync(Id);
			//item.RevitModels.Clear();
			//await db.Collections.UpdateAsync(item);
		}
		#endregion Collections

		#region Clients
		public IEnumerable<ClientDTO> GetAllClients(string mask = "") 
		{
			Task<IEnumerable<ClientDTO>> t = Task.Run(() => GetAllClientsAsync(mask));
			return t.Result;
		}

		public async Task<IEnumerable<ClientDTO>> GetAllClientsAsync(string mask = "")
		{
			var result = db.Clients.GetAll();
			if (mask != null && mask != "")
				result = result.Where(x => x.Name.ToLower().Contains(mask.ToLower()) || x.UserName.ToLower().Contains(mask.ToLower()));
			return await result.Select(x => new ClientDTO() { Id = x.Id, Name = x.Name }).ToListAsync();
		}
			 

		public async Task<ClientDTO> GetClientAsync(int Id)
		{
			var item = await db.Clients.GetAsync(Id);
			if (item == null)
				throw new NotFoundException();
			return new ClientDTO().Map(item);
		}

		#endregion Clients

		/*LICENSE*/
		#region Lic

		public async Task BreakLicenseAsync(int Id)
		{
			var lic = await db.Clients.GetAsync(Id);
			if (lic != null)
			{
				//await db.Collections.GetAll().Where(x => x.ClientId == Id).LoadAsync();
				await db.Clients.DeleteAsync(lic);
			}
				
		}

		public async Task BreakLicensesAsync(IEnumerable<int> Ids)
		{
			foreach (int id in Ids)
				await BreakLicenseAsync(id);
		}

		public async Task BreakAllLicensesAsync()
		{
			foreach (var lic in await db.Clients.GetAllAsync())
				await db.Clients.DeleteAsync(lic);
		}


		public async Task<LicenseDTO> GetLicenseAsync()
		{
			var lic = await db.Licenses.GetAsync(0);
			if (lic == null)
				throw new FaultException<NullKey>(new NullKey());

			(DateTime? date, int qnt) = GetLicenseParamsAsync(lic.Key);
			if (date == null || qnt == 0)
				throw new FaultException<WrongKey>(new WrongKey());

			return new LicenseDTO()
			{
				EndLicDate = date.Value,
				Success = date >= DateTime.Now.Date && qnt > 0,
				Key = lic.Key,
				LicensseQnt = qnt
			};
		}


		public async Task SetLicenseAsync(string key)
		{
			var lic = await db.Licenses.GetAsync(0);
			if (lic != null)
				await db.Licenses.DeleteAsync(0);
			await db.Licenses.CreateAsync(new License() { Key = key });

		}

		public async Task<IEnumerable<LicenseUsingDTO>> GetAllLicenseUsingsAsync()
		{
			return await GetLicenseUsingAsync(db.LicenseStatistics.GetAll());
		}


		public async Task<IEnumerable<LicenseUsingDTO>> GetLicenseUseingAsync(int monthsQnt)
		{
			DateTime _date = DateTime.Now.AddMonths(-monthsQnt);
			return await GetLicenseUsingAsync(db.LicenseStatistics.GetAll().Where(l => l.Date >= _date));
		}

		private async Task<IEnumerable<LicenseUsingDTO>> GetLicenseUsingAsync(IQueryable<LicenseUsersStatistic> iqstat)
		{
			return await db.Clients.GetAll().GroupJoin(iqstat,
			u => u.Id,
			us => us.ClientId,
			(lu, lus) =>
			new LicenseUsingDTO()
			{
				Id = lu.Id,
				StantionName = lu.Name,
				UserName = lu.UserName,
				LastAccess = lu.LastAccess,
				LastAllUsingQnt = lus.Sum(x => x.Qnt),
				LastUniqUsingQnt = lus.Sum(x => x.UniqQnt)
			}).ToListAsync();
		}

		public async Task<IEnumerable<LicenseMonthUsingDTO>> GetLicenseMonthsUsingAsync(int monthsQnt)
		{
			DateTime _date = DateTime.Now.AddMonths(-monthsQnt);
			DateTime date = new DateTime(_date.Year, _date.Month, 1);
			ICollection<LicenseMonthUsingDTO> result = new List<LicenseMonthUsingDTO>();
			for (int i = 1; i <= monthsQnt; i++)
			{
				DateTime _d = date.AddMonths(i);
				var qw = db.LicenseStatistics.GetAll().Where(x => x.YearNum == _d.Year && x.MonthNum == _d.Month);
				result.Add(new LicenseMonthUsingDTO()
				{
					Year = _d.Year,
					MonthNum = _d.Month,
					AllQnt = qw.Count() > 0 ? await qw.Select(s => s.Qnt).SumAsync() : 0,
					UniqQnt = qw.Count() > 0 ? await qw.Select(s => s.UniqQnt).SumAsync() : 0
				});
			}
			return result;

		}

		public async Task<IEnumerable<LicenseCategogiesPercentDTO>> GetLicenseCategogiesPercentAsync(int monthsQnt)
		{
			DateTime _date = DateTime.Now.Date.AddMonths(-monthsQnt);
			var iqw = db.LicenseStatistics.GetAll()
				.Where(x => x.Date >= _date);
			int qnt = await db.Clients.GetAll().CountAsync();

			return new List<LicenseCategogiesPercentDTO>()
			{
				new LicenseCategogiesPercentDTO(){ CategoryName = "firstcat" },
				new LicenseCategogiesPercentDTO(){ CategoryName = "secondcat" },
				new LicenseCategogiesPercentDTO(){ CategoryName = "thirdcat" },
			};

		}

		public async Task<int> GetLicenseQnt()
		{
			try
			{
				return (GetLicenseParamsAsync((await db.Licenses.GetAsync(0)).Key)).qnt;
			}
			catch
			{
				return 0;
			}

		}

		public async Task<int> GetLicenseUsedQnt(int monthsQnt)
		{
			try
			{
				DateTime _date = DateTime.Now.Date.AddMonths(-monthsQnt);
				return await db.Clients.GetAll().Where(x => x.LastAccess >= _date).CountAsync();
			}
			catch
			{
				return 0;
			}

		}

		public async Task<int> GetAllLicenseUsedQnt()
		{
			return await db.Clients.GetAll().CountAsync();
		}

		#endregion Lic

		#region Servers

		public async Task<IEnumerable<ServerDTO>> GetFilesAsync(int collectionId)
		{
			return await db.RevitModels.GetAll().Where(x => x.Collections.Select(s => s.Id).Contains(collectionId))
				.GroupBy(gr => new { Id= gr.RevitServerId, gr.RevitServer.Name, gr.RevitServer.IsDirectory, Version = gr.RevitServer.RevitVersion.Name }).Select(x => new ServerDTO()
				{
					Id = x.Key.Id,
					Name = x.Key.Name,
					RevitVersionName = x.Key.Version,
					IsDirectory = x.Key.IsDirectory,
					Files = x.Select(f => new RevitModelDTO()
					{
						Id = f.Id, Name = f.Name, Path = f.Path
					})
				}).AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<ServerDTO>> GetAllServersFullDataAsync(int collectionId)
		{
			await db.RevitVersions.GetAll().ToListAsync();
			var models = await db.RevitModels.GetAll().AsNoTracking().ToListAsync();
			var servers = await db.RevitServers.GetAll().AsNoTracking().ToListAsync();
			//await db.RevitModels.GetAll().Where(x => x.Collections.Select(s => s.Id).Contains(collectionId)).LoadAsync();

			

			return servers.Select(x =>
				   new ServerDTO()
				   {
					   Id = x.Id,
					   Name = x.Name,
					   RevitVersionId = x.RevitVersionId,
					   RevitVersionName = x.RevitVersion?.Name,
					   IsDirectory = x.IsDirectory,
					   Folders = GetFolders(x.Id, null),
					   Files = GetFiles(x.Id, null)
				   }).ToList();

			IEnumerable<RevitModelDTO> GetFolders(int serverId, int? parentId)
			{
				return models
					   .Where(x => x.RevitServerId == serverId && x.ParentId == parentId && x.IsFolder == true).Select(x =>
					   new RevitModelDTO()
					   {
						   Id = x.Id,
						   Name = x.Name,
						   Folders = GetFolders(serverId, x.Id),
						   Files = GetFiles(serverId, x.Id)
					   });
			}

			IEnumerable<RevitModelDTO> GetFiles(int serverId, int? parentId)
			{
				return models
					   .Where(x => x.RevitServerId == serverId && x.ParentId == parentId && x.IsFolder == false).Select(x =>
					   new RevitModelDTO()
					   {
						   Id = x.Id,
						   Name = x.Name,
						   RevitServerId = x.RevitServerId,
						   Checked = !x.IsFolder && x.Collections != null && x.Collections.Select(s=> s.Id).Contains(collectionId)
					   });
			}
		}

		public async Task CreateServerAsync(ServerDTO item)
		{
			await db.RevitServers.CreateAsync(new RevitServer() { Name = item.Name, RevitVersionId = item.RevitVersionId, IsDirectory = item.IsDirectory });
		}


		internal override async Task<IEnumerable<ServerDTO>> GetAllServersAsync(bool isDir, string mask = "")
		{
			try
			{
				var iq = db.RevitServers.GetAll().Where(x => x.IsDirectory == isDir);
				if (mask != null && mask.Length > 0)
					iq = iq.Where(x => x.Name.ToLower().Contains(mask.ToLower()));
				return await iq.Select(x => new ServerDTO()
				{
					Id = x.Id,
					Name = x.Name,
					RevitVersionId = x.RevitVersionId,
					RevitVersionName = x.RevitVersion.Name,
					ModelQnt = x.RevitModels.Count(), 
					LastUpdate = x.ServerLogs.Max(m => m.Odate), 
					AddedQnt = x.ServerLogs.OrderByDescending(o => o.Id).FirstOrDefault().AddedQnt,
					RemovedQnt = x.ServerLogs.OrderByDescending(o => o.Id).FirstOrDefault().RemovedQnt,
					Success = x.ServerLogs.OrderByDescending(o => o.Id).FirstOrDefault().Success,
					Description = x.ServerLogs.OrderByDescending(o => o.Id).FirstOrDefault().Message
				}).ToListAsync();
				
			}
			catch (Exception ex)
			{
				throw new FaultException<ModelCheck>(new ModelCheck(ex));
			}
		}

		public async Task<ServerDTO> GetServerAsync(int Id)
		{
			var item = await db.RevitServers.GetAsync(Id);
			if (item == null)
				throw new FaultException<NotFound>(new NotFound());
			return Mapper.Map(new ServerDTO(), item);
		}


		public async Task RemoveServerAsync(int Id)
		{
			await db.RevitServers.DeleteAsync(Id);
		}


		public async Task UpdateServerAsync(ServerDTO item)
		{
			var _item = await db.RevitServers.GetAsync(item.Id);
			if (_item == null)
				throw new FaultException<NotFound>(new NotFound());
			_item.Name = item.Name;
			_item.RevitVersionId = item.RevitVersionId;
			await db.RevitServers.UpdateAsync(_item);
		}


		#endregion Servers

		#region Sys
		public IEnumerable<NamedUnitDTO> GetNumericRevitVersions()
		{
			var t = Task.Run(() => GetNumericRevitVersionsAsync());
			return t.Result;
		}

		public async Task<IEnumerable<NamedUnitDTO>> GetNumericRevitVersionsAsync()
		{
			return await db.RevitVersions.GetAll().Where(x => x.IsDefault == false).Select(x => new NamedUnitDTO()
			{
				Id = x.Id,
				Name = x.Name
			}).OrderByDescending(x => x.Name).ToListAsync();
		}

		public async Task<IEnumerable<NamedUnitDTO>> GetAllRevitVersionsAsync()
		{
			return await db.RevitVersions.GetAll().Select(x => new NamedUnitDTO()
			{
				Id = x.Id, Name = x.Name
			}).OrderByDescending(x => x.Name).ToListAsync();
		}

		public IEnumerable<NamedUnitDTO> GetAllRevitVersions()
		{
			return db.RevitVersions.GetAll().Select(x => new NamedUnitDTO()
			{
				Id = x.Id,
				Name = x.Name
			}).OrderByDescending(x => x.Name).ToList();
		}

		public IEnumerable<WeekDayDTO> GetAllWeekDays() =>
			db.WeekDays.GetAll().Select(x => new WeekDayDTO()
			{
				Id = x.Id,
				Name = x.Name
			});

		public async Task<IEnumerable<WeekDayDTO>> GetAllWeekDaysAsync() =>
			await db.WeekDays.GetAll().Select(x => new WeekDayDTO()
			{
				Id = x.Id,
				Name = x.Name
			}).ToListAsync();

		public IEnumerable<HourDTO> GetAllHours()
		{
			Task<IEnumerable<HourDTO>> t = Task.Run(() => GetAllHoursAsync());
			return t.Result;
		}
		
		public async Task<IEnumerable<HourDTO>> GetAllHoursAsync()
		{
			return await db.Hours.GetAll().Select(x => new HourDTO
			{
				Id = x.Id, Name = x.Name, HourValue = x.HourValue
			}).ToListAsync();
		}

		public IEnumerable<MinuteDTO> GetAllMinutes()
		{
			Task<IEnumerable<MinuteDTO>> t = Task.Run(() => GetAllMinutesAsync());
			return t.Result;
		}

		public async Task<IEnumerable<MinuteDTO>> GetAllMinutesAsync() =>
			await db.Minutes.GetAll().Select(x => new MinuteDTO
			{
				Id = x.Id,
				Name = x.Name,
				MinuteValue = x.MinuteValue
			}).ToListAsync();

		#endregion Sys

		#region Job

		public async Task CreateIFCJobAsync(IFCJobDTO item)
		{
			IFCJob job = Mapper.Map(new IFCJob(), item);
			await db.IFCJobs.CreateAsync(job);
		}


		public async Task CreateRevitJobAsync(RevitJobDTO item)
		{
			RevitJob job = Mapper.Map(new RevitJob(), item);
			await db.RevitJobs.CreateAsync(job);
		}

		public async Task CreateNavisJobAsync(NavisJobDTO item)
		{
			NavisJob job = Mapper.Map(new NavisJob(), item);
			await db.NavisJobs.CreateAsync(job);
		}


		public IEnumerable<NamedUnitDTO> GetAllIFCFileTypes() =>
			 db.IFCFileTypes.GetAll().GetNamedUnit();

		public async Task<IEnumerable<NamedUnitDTO>> GetAllIFCFileTypesAsync() =>
			 await db.IFCFileTypes.GetAll().GetNamedUnitAsync();

		public IEnumerable<NamedUnitDTO> GetAllIFCIncludeBoundaties() =>
			 db.IFCIncludeBoundaties.GetAll().GetNamedUnit();

		public async Task<IEnumerable<NamedUnitDTO>> GetAllIFCIncludeBoundatiesAsync() =>
			 await db.IFCIncludeBoundaties.GetAll().GetNamedUnitAsync();

		
		public IEnumerable<NamedUnitDTO> GetAllNavisConvertElementsProperties() =>
			 db.NavisConvertElementsProperties.GetAll().GetNamedUnit();

		public async Task<IEnumerable<NamedUnitDTO>> GetAllNavisConvertElementsPropertiesAsync() =>
			 await db.NavisConvertElementsProperties.GetAll().GetNamedUnitAsync();

		public IEnumerable<NamedUnitDTO> GetAllNavisCoordinates() =>
			 db.NavisCoordinates.GetAll().GetNamedUnit();

		public async Task<IEnumerable<NamedUnitDTO>> GetAllNavisCoordinatesAsync() =>
			await db.NavisCoordinates.GetAll().GetNamedUnitAsync();

		public IEnumerable<NamedUnitDTO> GetAllNavisViews() =>
			 db.NavisViews.GetAll().GetNamedUnit();

		public async Task<IEnumerable<NamedUnitDTO>> GetAllNavisViewsAsync() =>
			await db.NavisViews.GetAll().GetNamedUnitAsync();


		public async Task<RevitJobDTO> GetRevitJob(int Id)
		{
			var item = await db.RevitJobs.GetAsync(Id);
			if (item == null)
				throw new FaultException<NotFound>(new NotFound());
			return Mapper.Map(new RevitJobDTO(), item);
		}

		public async Task<NavisJobDTO> GetNavisJob(int Id)
		{
			var item = await db.NavisJobs.GetAsync(Id);
			if (item == null)
				throw new FaultException<NotFound>(new NotFound());
			return Mapper.Map(new NavisJobDTO(), item);
		}

		public async Task<IFCJobDTO> GetIFCJob(int Id)
		{
			var item = await db.IFCJobs.GetAsync(Id);
			if (item == null)
				throw new FaultException<NotFound>(new NotFound());
			return Mapper.Map(new IFCJobDTO(), item);
		}

		public async Task MergeRevitJobAsync(RevitJobDTO item)
		{
			if (item.Id == 0)
				await CreateRevitJobAsync(item);
			else
				await UpdateRevitJobAsync(item);
		}

		public async Task MergeNavisJobAsync(NavisJobDTO item)
		{
			if (item.Id == 0)
				await CreateNavisJobAsync(item);
			else
				await UpdateNavisJobAsync(item);
		}

		public async Task MergeIFCJobAsync(IFCJobDTO item)
		{
			if (item.Id == 0)
				await CreateIFCJobAsync(item);
			else
				await UpdateIFCJobAsync(item);
		}

		public async Task RemoveRevitJobAsync(int Id)
		{
			await db.RevitJobs.DeleteAsync(Id);
		}

		public async Task RemoveNavisJobAsync(int Id)
		{
			await db.NavisJobs.DeleteAsync(Id);
		}

		public async Task RemoveIFCJobAsync(int Id)
		{
			await db.IFCJobs.DeleteAsync(Id);
		}

		public async Task UpdateRevitJobAsync(RevitJobDTO item)
		{
			var job = await db.RevitJobs.GetAsync(item.Id);
			if (job == null)
				throw new FaultException<NotFound>(new NotFound());
			Mapper.Map(job, item);
			await db.RevitJobs.UpdateAsync(job);
		}

		public async Task UpdateNavisJobAsync(NavisJobDTO item)
		{
			var job = await db.NavisJobs.GetAsync(item.Id);
			if (job == null)
				throw new FaultException<NotFound>(new NotFound());
			Mapper.Map(job, item);
			await db.NavisJobs.UpdateAsync(job);
		}

		public async Task UpdateIFCJobAsync(IFCJobDTO item)
		{
			var job = await db.IFCJobs.GetAsync(item.Id);
			if (job == null)
				throw new FaultException<NotFound>(new NotFound());
			Mapper.Map(job, item);
			await db.IFCJobs.UpdateAsync(job);
		}

		#endregion Job

		#region logs

		public async Task<IEnumerable<JobLaunchDTO>> GetWeekJobLaunchesAsync()
		{
			return await GetJobLaunchesAsync(DateTime.Now.Date.AddDays(-7), DateTime.Now.Date);
		}

		public async Task<IEnumerable<JobLaunchDTO>> GetMonthJobLaunchesAsync()
		{
			return await GetJobLaunchesAsync(DateTime.Now.Date.AddMonths(-1), DateTime.Now.Date.AddDays(1));
		}

		public async Task<IEnumerable<JobLaunchDTO>> GetQuarterJobLaunchesAsync()
		{
			return await GetJobLaunchesAsync(DateTime.Now.Date.AddMonths(-3), DateTime.Now.Date);
		}

		public async Task<IEnumerable<JobLaunchDTO>> GetJobLaunchesAsync(DateTime sdate, DateTime edate)
		{
			return await GetJobLaunchesAsync(db.JobLaunches.GetAll().Where(x => x.Odate <= edate && x.Odate >= sdate));
		}

		private async Task<IEnumerable<JobLaunchDTO>> GetJobLaunchesAsync(IQueryable<JobLaunch> data)
		{
			return await data.Select(x => new JobLaunchDTO
			{
				Id = x.Id,
				ClientName = x.Client.Name,
				CollectionName = x.Collection.Name,
				CollectionId = x.CollectionId,
				Odate = x.Odate,
				Success = x.Success,
				Message = x.Message,
				StartDateTime = x.StartDateTime,
				EndDateTime = x.EndDateTime
			}).OrderByDescending(o => o.Odate).AsNoTracking().ToListAsync();
		}

		public async Task<JobLaunchDTO> GetJobLaunchAsync(int Id)
		{
			var log = await GetJobLaunchesAsync(db.JobLaunches.GetAll().Where(x => x.Id == Id));
			if(log.Count() == 0)
				throw new FaultException<NotFound>(new NotFound());
			return log.First();
		}

		#endregion logs

	}
}
