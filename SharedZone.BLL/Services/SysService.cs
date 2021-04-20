using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SharedZone.DAL.Interfaces;
using SharedZone.DTO;
using SharedZone.ISysService;
using System.Data.Entity;
using SharedZone.DAL.Entities;
using Bimacad.Sys;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net;

namespace SharedZone.BLL.Services
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
	   InstanceContextMode = InstanceContextMode.Single,
	   IncludeExceptionDetailInFaults = true)]

	public class SysService : BaseService, ISysSrc
	{
		public SysService(IUnitOfWork uw) : base(uw) { }


		public string GetName()
		{
			return "SysService";
		}

		internal override async Task<IEnumerable<ServerDTO>> GetAllServersAsync(bool isDir, string mask = "")
		{
			return await db.RevitServers.GetAll()
				.Where(x => x.IsDirectory == isDir).Select(x =>
				new ServerDTO
				{
					Id = x.Id,
					Name = x.Name
				}).AsNoTracking().ToListAsync();
		}

		public string TestUpdateServer(int Id)
		{
			try
			{
				var srv = db.RevitServers.GetAll().Where(x => x.Id == Id).FirstOrDefault();
				if (srv == null)
					throw new NotFoundException();
				var _srv = new ServerDTO().Map(srv);
				var content = GetContent(_srv.Name, _srv.RevitVersionName, "|");
				var data = GetRevitServerFoldersAndModels(content, _srv, "|");

				return "OK!!!";
			}
			catch(Exception ex)
			{
				return ex.Message;
			}
		}


		public async Task UpdateServerAsync(int Id)
		{
			var srv = await db.RevitServers.GetAsync(Id);
			if (srv != null)
			{
				var _srv = new ServerDTO().Map(srv);
				if (srv.IsDirectory)
					await UpdateDirectory(_srv);
				else
					await UpdateRevitServer(_srv);
			}
		}

		public async Task<ServerLogDTO> MergeFilesAsync(ServerDTO server)
		{
			try
			{

				var newfilecollection = GetFlatTable(server);
				var oldfilecollection = await GetServerFlatTable(server.Id);
				//remove
				var fordeleteColl = GetCollectionForRemove(oldfilecollection, newfilecollection);
				int delm = await RemoveModels(fordeleteColl);
				//insert
				int ins = 0;
				if (GetCollectionForInsert(oldfilecollection, newfilecollection).Count() > 0)
				{
					ins = await CreateFiles(server.Files, null);
					ins += await CreateFolders(server.Folders, null);
				}
				return new ServerLogDTO() { Message = "OK", Success = true, RemovedQnt = delm, ServerId = server.Id, Odate = DateTime.Now, AddedQnt = ins };
			}
			catch (Exception ex)
			{
				return new ServerLogDTO(ex, server.Id);
			}
		}



		private async Task<int> CreateModels(ServerDTO server)
		{
			try
			{
				return await CreateFiles(server.Files, null);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



		private async Task<int> CreateFiles(IEnumerable<RevitModelDTO> items, int? parentId)
		{
			int result = 0;
			foreach (var item in items)
			{
				item.ParentId = parentId;
				result += await CreateFile(item);
			}
			return result;
		}


		private async Task<int> CreateFolders(IEnumerable<RevitModelDTO> items, int? parentId)
		{
			int result = 0;
			foreach (var item in items)
			{
				result += await CreateFolder(item, parentId);
			}
			return result;
		}

		private async Task<int> CreateFolder(RevitModelDTO item, int? parentId)
		{
			int result = 0;
			var _item = await db.RevitModels.GetAll()
				.Where(x => x.Path == item.Path && x.RevitServerId == item.RevitServerId).FirstOrDefaultAsync();
			if (_item == null)
			{
				_item = new RevitModel();
				Mapper.Map(_item, item);
				_item.ParentId = parentId;
				await db.RevitModels.CreateAsync(_item);
			}
			result += await CreateFolders(item.Folders, _item.Id);
			result += await CreateFiles(item.Files, _item.Id);
			return result;
		}


		private async Task<int> CreateFile(RevitModelDTO item)
		{
			var _item = await db.RevitModels.GetAll()
				.Where(x => x.Path == item.Path && x.RevitServerId == item.RevitServerId).FirstOrDefaultAsync();
			if (_item == null)
			{
				await db.RevitModels.CreateAsync(Mapper.Map(new RevitModel(), item));
				return 1;
			}
			return 0;
		}

		private async Task<int> RemoveModels(IEnumerable<RevitModelDTO> items)
		{
			try
			{
				if (items.Count() == 0)
					return 0;
				int result = items.Where(x => x.IsFolder == false).Count();
				foreach (var item in items)
					await db.RevitModels.DeleteAsync(item.Id);
				return result;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private IEnumerable<RevitModelDTO> GetCollectionForInsert(IEnumerable<RevitModelDTO> oldColl, IEnumerable<RevitModelDTO> newColl)
		{
			return newColl.Except(oldColl);
		}

		private IEnumerable<RevitModelDTO> GetCollectionForRemove(IEnumerable<RevitModelDTO> oldColl, IEnumerable<RevitModelDTO> newColl)
		{
			return oldColl.Except(newColl);
		}

		private async Task<IEnumerable<RevitModelDTO>> GetServerFlatTable(int serverId)
		{
			return from t1 in await db.RevitModels.GetAll()
				.Where(x => x.RevitServerId == serverId).ToListAsync()
				   select new RevitModelDTO() { Id = t1.Id, Path = t1.Path, RevitServerId = t1.RevitServerId };
		}

		private IEnumerable<RevitModelDTO> GetFlatTable(ServerDTO server)
		{
			return server.Files.Concat(GetFlatTable(server.Folders));
		}

		private IEnumerable<RevitModelDTO> GetFlatTable(IEnumerable<RevitModelDTO> folders)
		{
			return folders.Concat(folders.SelectMany(x => GetFlatTable(x)));
		}

		private IEnumerable<RevitModelDTO> GetFlatTable(RevitModelDTO folder)
		{
			return folder.Files.Concat(folder.Folders);
		}

		public async Task WriteServerLogAsync(ServerLogDTO serverLog)
		{
			try
			{
				await db.ServerLogs.CreateAsync(Mapper.Map(new ServerLog(), serverLog));
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public async Task ClearServerLogAsync(int days)
		{
			DateTime date = DateTime.Now.AddDays(-days);
			var logs = await db.ServerLogs.GetAll().Where(x => x.Odate <= date).ToListAsync();
			foreach (var log in logs)
				await db.ServerLogs.DeleteAsync(log);
		}

		public async Task ClearJobLogAsync(int days)
		{
			DateTime date = DateTime.Now.AddDays(-days);
			var logs = await db.JobLaunches.GetAll().Where(x => x.StartDateTime <= date).ToListAsync();
			foreach (var log in logs)
				await db.JobLaunches.DeleteAsync(log);
		}


		public async Task UpdateDirectoriesAsync()
		{
			var dirs = await GetAllDirectoriesAsync();
			foreach (var dir in dirs)
			{
				await UpdateDirectory(dir);
			}
		}

		private async Task UpdateDirectory(ServerDTO dir)
		{
			try
			{
				var server = new ServerDTO()
				{
					Id = dir.Id,
					Folders = GetFolders(dir.Name, dir.Id),
					Files = GetFiles(dir.Name, dir.Id)
				};
				var log = await MergeFilesAsync(server);
				await WriteServerLogAsync(log);

			}
			catch (Exception ex)
			{
				await WriteServerLogAsync(new ServerLogDTO(ex, dir.Id));
			}
		}

		private IEnumerable<RevitModelDTO> GetFolders(string path, int serverId)
		{
			if (Directory.Exists(path))
			{
				return Directory.GetDirectories(path).Select(x => new RevitModelDTO()
				{
					ParentName = path,
					Name = Path.GetFileName(x),
					IsFolder = true,
					Path = x,
					Files = GetFiles(x, serverId),
					Folders = GetFolders(x, serverId),
					RevitServerId = serverId
				});
			}
			else
			{
				throw new DirectoryNotFoundException();
			}
		}

		private IEnumerable<RevitModelDTO> GetFiles(string path, int serverId)
		{
			return Directory.GetFiles(path, "*.rvt", SearchOption.TopDirectoryOnly).Select(x => new RevitModelDTO()
			{
				ParentName = path,
				Name = Path.GetFileName(x),
				IsFolder = false,
				Path = x,
				RevitServerId = serverId
			});
		}


		#region revit server

		public async Task UpdateRevitServersAsync()
		{
			var srvs = (await GetAllServersAsync()).ToList();
			foreach (var srv in srvs)
			{
				await UpdateRevitServer(srv);
			}
		}

		

		private async Task UpdateRevitServer(ServerDTO srv)
		{
			try
			{
				var content = GetContent(srv.Name, srv.RevitVersionName, "|");
				var data = GetRevitServerFoldersAndModels(content, srv, "|");
				var server = new ServerDTO()
				{
					Id = srv.Id,
					Folders = data.Where(x => x.IsFolder == true),
					Files = data.Where(x => x.IsFolder == false),
				};
				var log = await MergeFilesAsync(server);
				await WriteServerLogAsync(log);
			}
			catch (Exception ex)
			{
				await WriteServerLogAsync(new ServerLogDTO(ex, srv.Id));
			}
		}

		private IEnumerable<RevitModelDTO> GetRevitServerFoldersAndModels(JObject obj, ServerDTO srv, string path)
		{
			try
			{
				ICollection<RevitModelDTO> result = new List<RevitModelDTO>();

				foreach (JToken item in obj["Folders"])
				{
					string currentPath = path.Equals("|") ? path + item["Name"].ToString() : path + "|" + item["Name"].ToString();
					string rsnpath = @"RSN://" + srv + @"/" + currentPath.Replace("|", "/").Remove(0, 1);
					JObject obj2 = GetContent(srv.Name, srv.RevitVersionName, currentPath);

					result.Add(new RevitModelDTO()
					{
						ParentName = path,
						Name = item["Name"].ToString(),
						Path = rsnpath,
						RevitServerId = srv.Id,
						IsFolder = true,
						Folders = GetRevitServerFoldersAndModels(obj2, srv, currentPath), 
						Files = GetFiles()
					});
				}

				return result;

				IEnumerable<RevitModelDTO> GetFiles()
				{
					return obj["Models"].Select(x => new RevitModelDTO
					{
						RevitServerId = srv.Id,
						IsFolder = false,
						Name = x["Name"].ToString(),
						Path = @"RSN://" + srv + @"/" + path.Replace("|", "/").Remove(0, 1) + "/" + x["Name"].ToString(), 
						ParentName = path
					});
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}


		private JObject GetContent(string srv, string vers, string currentPath)
		{
			//Uri uri = new Uri(string.Format("http://{0}/RevitServerAdminRESTService{1}/AdminRESTService.svc/{2}/Contents", srv, vers, currentPath));
			Uri uri = new Uri($"http://{srv}/RevitServerAdminRESTService{vers}/AdminRESTService.svc/{currentPath}/Contents");
			try
			{
				var resp = GetResponse(uri);
				return JObject.Parse(resp);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private string GetResponse(Uri uri, string method = "GET", long contentLength = 0)
		{
			try
			{
				//BTextWriter.WriteLog(uri.ToString());
				WebRequest wR = WebRequest.Create(uri);
				wR.Method = method;
				wR.Headers.Add("User-Name", "Environment.UserName"/*Environment.UserDomainName Environment.UserName*/);
				wR.Headers.Add("User-Machine-Name", "Environment.MachineName"/*Environment.MachineName*/);
				wR.Headers.Add("User-Domain-Name", "Environment.UserDomainName");
				wR.Headers.Add("Operation-GUID", Guid.NewGuid().ToString());
				if (method.Equals("POST")) wR.ContentLength = contentLength;

				WebResponse response = wR.GetResponse();

				Encoding encoding = Encoding.UTF8;
				string responseText = "";
				using (var reader = new StreamReader(response.GetResponseStream(), encoding))
				{
					responseText = reader.ReadToEnd();
				}
				return responseText;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message + ".\rURI: " + uri.ToString());
			}

		}

		#endregion revit server

	}
}
