using Bimacad.Sys;
using System.ServiceModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharedZone.DTO;

namespace SharedZone.ISysService
{
	[ServiceContract]
	public interface ISysSrc : IService
	{

		//async
		[OperationContract]
		Task<IEnumerable<ServerDTO>> GetAllDirectoriesAsync(string mask = "");

		[OperationContract]
		Task<IEnumerable<ServerDTO>> GetAllServersAsync(string mask = "");

		[OperationContract]
		Task<ServerLogDTO> MergeFilesAsync(ServerDTO server);

		[OperationContract]
		Task WriteServerLogAsync(ServerLogDTO serverLog);

		[OperationContract]
		Task ClearServerLogAsync(int days);

		[OperationContract]
		Task ClearJobLogAsync(int days);

		[OperationContract]
		Task UpdateDirectoriesAsync();

		[OperationContract]
		Task UpdateRevitServersAsync();

		[OperationContract]
		Task UpdateServerAsync(int Id);

		[OperationContract]
		string TestUpdateServer(int Id);

		////sync
		//[OperationContract]
		//IEnumerable<ServerDTO> GetAllDirectories(string mask = "");

		//[OperationContract]
		//IEnumerable<ServerDTO> GetAllServers(string mask = "");

		//[OperationContract]
		//ServerLogDTO MergeFiles(ServerDTO server);

		//[OperationContract]
		//void WriteServerLog(ServerLogDTO serverLog);

		//[OperationContract]
		//void UpdateDirectories();

		//[OperationContract]
		//void UpdateRevitServers();

		//[OperationContract]
		//void UpdateServer(int Id);
	}
}
