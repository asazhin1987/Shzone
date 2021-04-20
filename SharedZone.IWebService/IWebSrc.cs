using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using Bimacad.Sys;
using SharedZone.DTO;


namespace SharedZone.IWebService
{
	[ServiceContract]
	public interface IWebSrc : IService
	{
		/*GET ALL ASYNC*/
		[OperationContract(Name = "GetAllClients")]
		[FaultContract(typeof(NotFound))]
		IEnumerable<ClientDTO> GetAllClients(string mask = "");

		[OperationContract(Name = "GetAllClientsAsync")]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<ClientDTO>> GetAllClientsAsync(string mask = "");

		[OperationContract(Name = "GetAllCollectionsAsync")]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<CollectionDTO>> GetAllCollectionsAsync(string mask = "");

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<ServerDTO>> GetAllServersAsync(string mask = "");

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<ServerDTO>> GetAllDirectoriesAsync(string mask = "");

		[OperationContract(Name = "GetAllIFCFileTypes")]
		[FaultContract(typeof(NotFound))]
		IEnumerable<NamedUnitDTO> GetAllIFCFileTypes();

		[OperationContract(Name = "GetAllIFCFileTypesAsync")]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<NamedUnitDTO>> GetAllIFCFileTypesAsync();

		[OperationContract(Name = "GetAllIFCIncludeBoundaties")]
		[FaultContract(typeof(NotFound))]
		IEnumerable<NamedUnitDTO> GetAllIFCIncludeBoundaties();

		[OperationContract(Name = "GetAllIFCIncludeBoundatiesAsync")]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<NamedUnitDTO>> GetAllIFCIncludeBoundatiesAsync();

		[OperationContract(Name = "GetAllNavisConvertElementsProperties")]
		[FaultContract(typeof(NotFound))]
		IEnumerable<NamedUnitDTO> GetAllNavisConvertElementsProperties();

		[OperationContract(Name = "GetAllNavisConvertElementsPropertiesAsync")]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<NamedUnitDTO>> GetAllNavisConvertElementsPropertiesAsync();

		[OperationContract(Name = "GetAllNavisCoordinates")]
		[FaultContract(typeof(NotFound))]
		IEnumerable<NamedUnitDTO> GetAllNavisCoordinates();

		[OperationContract(Name = "GetAllNavisCoordinatesAsync")]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<NamedUnitDTO>> GetAllNavisCoordinatesAsync();

		[OperationContract(Name = "GetAllNavisViews")]
		[FaultContract(typeof(NotFound))]
		IEnumerable<NamedUnitDTO> GetAllNavisViews();

		[OperationContract(Name = "GetAllNavisViewsAsync")]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<NamedUnitDTO>> GetAllNavisViewsAsync();

		[OperationContract(Name = "GetAllHours")]
		[FaultContract(typeof(NotFound))]
		IEnumerable<HourDTO> GetAllHours();

		[OperationContract(Name = "GetAllHoursAsync")]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<HourDTO>> GetAllHoursAsync();

		[OperationContract(Name = "GetAllMinutes")]
		[FaultContract(typeof(NotFound))]
		IEnumerable<MinuteDTO> GetAllMinutes();

		[OperationContract(Name = "GetAllMinutesAsync")]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<MinuteDTO>> GetAllMinutesAsync();

		[OperationContract(Name = "GetAllWeekDays")]
		[FaultContract(typeof(NotFound))]
		IEnumerable<WeekDayDTO> GetAllWeekDays();

		[OperationContract(Name = "GetAllWeekDaysAsync")]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<WeekDayDTO>> GetAllWeekDaysAsync();

		[OperationContract(Name = "GetAllRevitVersions")]
		[FaultContract(typeof(NotFound))]
		IEnumerable<NamedUnitDTO> GetAllRevitVersions();

		[OperationContract(Name = "GetAllRevitVersionsAsync")]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<NamedUnitDTO>> GetAllRevitVersionsAsync();

		[OperationContract(Name = "GetNumericRevitVersionsAsync")]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<NamedUnitDTO>> GetNumericRevitVersionsAsync();

		[OperationContract(Name = "GetNumericRevitVersions")]
		[FaultContract(typeof(NotFound))]
		IEnumerable<NamedUnitDTO> GetNumericRevitVersions();

		/*GET ASYNC*/
		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<CollectionDTO> GetCollectionAsync(int Id);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<ServerDTO> GetServerAsync(int Id);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<ClientDTO> GetClientAsync(int Id);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<RevitJobDTO> GetRevitJob(int Id);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<NavisJobDTO> GetNavisJob(int Id);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<IFCJobDTO> GetIFCJob(int Id);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<ServerDTO>> GetAllServersFullDataAsync(int collectionId);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<ServerDTO>> GetFilesAsync(int collectionId);

		/*CREATE ASYNC*/
		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<int> CreateCollectionAsync(CollectionDTO item);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task CreateServerAsync(ServerDTO item);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task CreateRevitJobAsync(RevitJobDTO item);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task CreateNavisJobAsync(NavisJobDTO item);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task CreateIFCJobAsync(IFCJobDTO item);

		/*UPDATE ASYNC*/
		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task UpdateCollectionAsync(CollectionDTO item);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task UpdateServerAsync(ServerDTO item);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task UpdateRevitJobAsync(RevitJobDTO item);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task UpdateNavisJobAsync(NavisJobDTO item);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task UpdateIFCJobAsync(IFCJobDTO item);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task UpdateModelsAsync(int collectionId, IEnumerable<int> files);

		/*REMOVE ASYNC*/
		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task RemoveCollectionAsync(int Id);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task RemoveServerAsync(int Id);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task RemoveRevitJobAsync(int Id);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task RemoveNavisJobAsync(int Id);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task RemoveIFCJobAsync(int Id);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task ClearModels(int Id);

		/*LICENSE*/
		[OperationContract]
		[FaultContract(typeof(NullKey))]
		[FaultContract(typeof(WrongKey))]
		[FaultContract(typeof(ZeroQnt))]
		[FaultContract(typeof(NotFound))]
		Task<LicenseDTO> GetLicenseAsync();

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		bool CheckLicense();

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task SetLicenseAsync(string key);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task BreakLicenseAsync(int Id);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task BreakLicensesAsync(IEnumerable<int> Ids);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task BreakAllLicensesAsync();

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<LicenseUsingDTO>> GetAllLicenseUsingsAsync();

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<LicenseUsingDTO>> GetLicenseUseingAsync(int monthsQnt);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<LicenseMonthUsingDTO>> GetLicenseMonthsUsingAsync(int nomthQnt);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<LicenseCategogiesPercentDTO>> GetLicenseCategogiesPercentAsync(int monthsQnt);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<int> GetLicenseQnt();

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<int> GetLicenseUsedQnt(int monthsQnt);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<int> GetAllLicenseUsedQnt();


		/*MERGE*/
		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task MergeRevitJobAsync(RevitJobDTO item);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task MergeNavisJobAsync(NavisJobDTO item);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task MergeIFCJobAsync(IFCJobDTO item);



		/*JOB LAUNCHES*/
		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<JobLaunchDTO>> GetWeekJobLaunchesAsync();

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<JobLaunchDTO>> GetMonthJobLaunchesAsync();

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<JobLaunchDTO>> GetQuarterJobLaunchesAsync();

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<IEnumerable<JobLaunchDTO>> GetJobLaunchesAsync(DateTime sdate, DateTime edate);

		[OperationContract]
		[FaultContract(typeof(NotFound))]
		Task<JobLaunchDTO> GetJobLaunchAsync(int Id);

	}
}
