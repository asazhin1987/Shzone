using Bimacad.Sys;
using SharedZone.DTO;
using SharedZone.IWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SharedZone.WebSRC
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
	   InstanceContextMode = InstanceContextMode.Single,
	   IncludeExceptionDetailInFaults = true)]

	public class WebService<T> : IWebSrc where T : IService
	{
		private readonly string Url;

		public WebService(string host)
		{
			Url = $"http://{host}:80/SharedZoneWebService";
		}

		#region proxy
		private async Task<M> UseWebProxyClient<M>(Func<IWebSrc, Task<M>> accessor)
		{
			using (ProxyFactory<T> proxy = new ProxyFactory<T>(Url))
			{
				try
				{
					return await accessor((IWebSrc)proxy.Service);
				}
				catch (FaultException<NullKey>)
				{
					throw new NullKeyException();
				}
				catch (FaultException<WrongKey>)
				{
					throw new WrongKeyException();
				}
				catch (FaultException<ZeroQnt>)
				{
					throw new ZeroQntException();
				}
				catch (FaultException fkex)
				{
					throw fkex;
				}
				catch (CommunicationException e)
				{
					throw new ModelCheckerException(e.Message);
				}
				catch (Exception ex)
				{
					throw new ModelCheckerException(ex.Message);
				}
			}
		}

		private async Task UseWebProxyClient(Func<IWebSrc, Task> accessor)
		{
			using (ProxyFactory<T> proxy = new ProxyFactory<T>(Url))
			{
				try
				{
					await accessor((IWebSrc)proxy.Service);
				}
				catch (FaultException fkex)
				{
					throw fkex;
				}
				catch (CommunicationException e)
				{
					throw new ModelCheckerException(e.Message);
				}
				catch (Exception ex)
				{
					throw new ModelCheckerException(ex.Message);
				}
			}
		}

		private M UseWebProxyClient<M>(Func<IWebSrc, M> accessor)
		{
			using (ProxyFactory<T> proxy = new ProxyFactory<T>(Url))
			{
				try
				{
					return accessor((IWebSrc)proxy.Service);
				}
				
				catch (FaultException<NotFound>)
				{
					throw new NotFoundException();
				}
				catch (FaultException fkex)
				{
					throw fkex;
				}
				catch (CommunicationException e)
				{
					throw new ModelCheckerException(e.Message);
				}
				catch (Exception ex)
				{
					throw new ModelCheckerException(ex.Message);
				}
			}
		}

		#endregion proxy


		public bool CheckLicense()
		{
			return UseWebProxyClient(x => x.CheckLicense());
		}
			

		public async Task ClearModels(int Id) =>
			await UseWebProxyClient(x => x.ClearModels(Id));

		public async Task<int> CreateCollectionAsync(CollectionDTO item) =>
			await UseWebProxyClient(x => x.CreateCollectionAsync(item));

		public async Task CreateIFCJobAsync(IFCJobDTO item) =>
			await UseWebProxyClient(x => x.CreateIFCJobAsync(item));

		public async Task CreateNavisJobAsync(NavisJobDTO item) =>
			await UseWebProxyClient(x => x.CreateNavisJobAsync(item));

		public async Task CreateRevitJobAsync(RevitJobDTO item) =>
			await UseWebProxyClient(x => x.CreateRevitJobAsync(item));

		public async Task CreateServerAsync(ServerDTO item) =>
			await UseWebProxyClient(x => x.CreateServerAsync(item));

		public IEnumerable<ClientDTO> GetAllClients(string mask = "") =>
			UseWebProxyClient(x => x.GetAllClients(mask));

		public async Task<IEnumerable<ClientDTO>> GetAllClientsAsync(string mask = "") =>
			await UseWebProxyClient(x => x.GetAllClientsAsync(mask));

		public async Task<IEnumerable<CollectionDTO>> GetAllCollectionsAsync(string mask = "") =>
			await UseWebProxyClient(x => x.GetAllCollectionsAsync(mask));

		public async Task<IEnumerable<ServerDTO>> GetAllDirectoriesAsync(string mask = "") =>
			await UseWebProxyClient(x => x.GetAllDirectoriesAsync(mask));

		public async Task<IEnumerable<ServerDTO>> GetFilesAsync(int collectionId) =>
			await UseWebProxyClient(x => x.GetFilesAsync(collectionId));

		public IEnumerable<HourDTO> GetAllHours() =>
			UseWebProxyClient(x => x.GetAllHours());

		public async Task<IEnumerable<HourDTO>> GetAllHoursAsync() =>
			await UseWebProxyClient(x => x.GetAllHoursAsync());

		public IEnumerable<NamedUnitDTO> GetAllIFCFileTypes() =>
			UseWebProxyClient(x => x.GetAllIFCFileTypes());

		public async Task<IEnumerable<NamedUnitDTO>> GetAllIFCFileTypesAsync() =>
			await UseWebProxyClient(x => x.GetAllIFCFileTypesAsync());

		public IEnumerable<NamedUnitDTO> GetAllIFCIncludeBoundaties() =>
			UseWebProxyClient(x => x.GetAllIFCIncludeBoundaties());

		public async Task<IEnumerable<NamedUnitDTO>> GetAllIFCIncludeBoundatiesAsync() =>
			await UseWebProxyClient(x => x.GetAllIFCIncludeBoundatiesAsync());

		public IEnumerable<MinuteDTO> GetAllMinutes() =>
			UseWebProxyClient(x => x.GetAllMinutes());

		public async Task<IEnumerable<MinuteDTO>> GetAllMinutesAsync() =>
			await UseWebProxyClient(x => x.GetAllMinutesAsync());

		public IEnumerable<NamedUnitDTO> GetAllNavisConvertElementsProperties() =>
			UseWebProxyClient(x => x.GetAllNavisConvertElementsProperties());

		public async Task<IEnumerable<NamedUnitDTO>> GetAllNavisConvertElementsPropertiesAsync() =>
			await UseWebProxyClient(x => x.GetAllNavisConvertElementsPropertiesAsync());

		public IEnumerable<NamedUnitDTO> GetAllNavisCoordinates() =>
			UseWebProxyClient(x => x.GetAllNavisCoordinates());

		public async Task<IEnumerable<NamedUnitDTO>> GetAllNavisCoordinatesAsync() =>
			await UseWebProxyClient(x => x.GetAllNavisCoordinatesAsync());

		public IEnumerable<NamedUnitDTO> GetAllNavisViews() =>
			UseWebProxyClient(x => x.GetAllNavisViews());

		public async Task<IEnumerable<NamedUnitDTO>> GetAllNavisViewsAsync() =>
			await UseWebProxyClient(x => x.GetAllNavisViewsAsync());

		public IEnumerable<NamedUnitDTO> GetAllRevitVersions() =>
			UseWebProxyClient(x => x.GetAllRevitVersions());

		public async Task<IEnumerable<NamedUnitDTO>> GetAllRevitVersionsAsync() =>
			await UseWebProxyClient(x => x.GetAllRevitVersionsAsync());

		public async Task<IEnumerable<NamedUnitDTO>> GetNumericRevitVersionsAsync() =>
			await UseWebProxyClient(x => x.GetNumericRevitVersionsAsync());

		public IEnumerable<NamedUnitDTO> GetNumericRevitVersions() =>
			UseWebProxyClient(x => x.GetNumericRevitVersions());

		public async Task<IEnumerable<ServerDTO>> GetAllServersAsync(string mask = "") =>
			await UseWebProxyClient(x => x.GetAllServersAsync(mask));

		public async Task<IEnumerable<ServerDTO>> GetAllServersFullDataAsync(int collectionId) =>
			await UseWebProxyClient(x => x.GetAllServersFullDataAsync(collectionId));

		public IEnumerable<WeekDayDTO> GetAllWeekDays() =>
			UseWebProxyClient(x => x.GetAllWeekDays());

		public async Task<IEnumerable<WeekDayDTO>> GetAllWeekDaysAsync() =>
			await UseWebProxyClient(x => x.GetAllWeekDaysAsync());

		public async Task<ClientDTO> GetClientAsync(int Id) =>
			await UseWebProxyClient(x => x.GetClientAsync(Id));

		public async Task<CollectionDTO> GetCollectionAsync(int Id) =>
			await UseWebProxyClient(x => x.GetCollectionAsync(Id));

		public async Task<IFCJobDTO> GetIFCJob(int Id) =>
			await UseWebProxyClient(x => x.GetIFCJob(Id));

		public async Task<LicenseDTO> GetLicenseAsync() =>
			await UseWebProxyClient(x => x.GetLicenseAsync());


		public async Task<NavisJobDTO> GetNavisJob(int Id) =>
			await UseWebProxyClient(x => x.GetNavisJob(Id));

		public async Task<RevitJobDTO> GetRevitJob(int Id) =>
			await UseWebProxyClient(x => x.GetRevitJob(Id));

		public async Task<ServerDTO> GetServerAsync(int Id) =>
			await UseWebProxyClient(x => x.GetServerAsync(Id));

		public async Task MergeIFCJobAsync(IFCJobDTO item) =>
			await UseWebProxyClient(x => x.MergeIFCJobAsync(item));

		public async Task MergeNavisJobAsync(NavisJobDTO item) =>
			await UseWebProxyClient(x => x.MergeNavisJobAsync(item));

		public async Task MergeRevitJobAsync(RevitJobDTO item) =>
			await UseWebProxyClient(x => x.MergeRevitJobAsync(item));

		public async Task RemoveCollectionAsync(int Id) =>
			await UseWebProxyClient(x => x.RemoveCollectionAsync(Id));

		public async Task RemoveIFCJobAsync(int Id) =>
			await UseWebProxyClient(x => x.RemoveIFCJobAsync(Id));

		public async Task RemoveNavisJobAsync(int Id) =>
			await UseWebProxyClient(x => x.RemoveNavisJobAsync(Id));

		public async Task RemoveRevitJobAsync(int Id) =>
			await UseWebProxyClient(x => x.RemoveRevitJobAsync(Id));

		public async Task RemoveServerAsync(int Id) =>
			await UseWebProxyClient(x => x.RemoveServerAsync(Id));

		public async Task SetLicenseAsync(string key) =>
			await UseWebProxyClient(x => x.SetLicenseAsync(key));

		public async Task UpdateCollectionAsync(CollectionDTO item) =>
			await UseWebProxyClient(x => x.UpdateCollectionAsync(item));

		public async Task UpdateIFCJobAsync(IFCJobDTO item) =>
			await UseWebProxyClient(x => x.UpdateIFCJobAsync(item));

		public async Task UpdateModelsAsync(int collectionId, IEnumerable<int> files) =>
			await UseWebProxyClient(x => x.UpdateModelsAsync(collectionId, files));

		public async Task UpdateNavisJobAsync(NavisJobDTO item) =>
			await UseWebProxyClient(x => x.UpdateNavisJobAsync(item));

		public async Task UpdateRevitJobAsync(RevitJobDTO item) =>
			await UseWebProxyClient(x => x.UpdateRevitJobAsync(item));

		public async Task UpdateServerAsync(ServerDTO item) =>
			await UseWebProxyClient(x => x.UpdateServerAsync(item));

		public async Task<IEnumerable<JobLaunchDTO>> GetWeekJobLaunchesAsync() =>
			await UseWebProxyClient(x => x.GetWeekJobLaunchesAsync());

		public async Task<IEnumerable<JobLaunchDTO>> GetMonthJobLaunchesAsync() =>
			await UseWebProxyClient(x => x.GetMonthJobLaunchesAsync());

		public async Task<IEnumerable<JobLaunchDTO>> GetQuarterJobLaunchesAsync() =>
			await UseWebProxyClient(x => x.GetQuarterJobLaunchesAsync());

		public async Task<IEnumerable<JobLaunchDTO>> GetJobLaunchesAsync(DateTime sdate, DateTime edate) =>
			await UseWebProxyClient(x => x.GetJobLaunchesAsync(sdate, edate));

		public async Task<JobLaunchDTO> GetJobLaunchAsync(int Id) =>
			await UseWebProxyClient(x => x.GetJobLaunchAsync(Id));


		public async Task BreakLicenseAsync(int Id) =>
			await UseWebProxyClient(x => x.BreakLicenseAsync(Id));

		public async Task BreakLicensesAsync(IEnumerable<int> Ids) =>
			await UseWebProxyClient(x => x.BreakLicensesAsync(Ids));

		public async Task BreakAllLicensesAsync() =>
			await UseWebProxyClient(x => x.BreakAllLicensesAsync());

		public async Task<IEnumerable<LicenseUsingDTO>> GetAllLicenseUsingsAsync() =>
			await UseWebProxyClient(x => x.GetAllLicenseUsingsAsync());

		public async Task<IEnumerable<LicenseUsingDTO>> GetLicenseUseingAsync(int monthsQnt) =>
			await UseWebProxyClient(x => x.GetLicenseUseingAsync(monthsQnt));

		public async Task<IEnumerable<LicenseMonthUsingDTO>> GetLicenseMonthsUsingAsync(int nomthQnt) =>
			await UseWebProxyClient(x => x.GetLicenseMonthsUsingAsync(nomthQnt));

		public async Task<IEnumerable<LicenseCategogiesPercentDTO>> GetLicenseCategogiesPercentAsync(int monthsQnt) =>
			await UseWebProxyClient(x => x.GetLicenseCategogiesPercentAsync(monthsQnt));

		public async Task<int> GetLicenseQnt() =>
			await UseWebProxyClient(x => x.GetLicenseQnt());

		public async Task<int> GetLicenseUsedQnt(int monthsQnt) =>
			await UseWebProxyClient(x => x.GetLicenseUsedQnt(monthsQnt));

		public async Task<int> GetAllLicenseUsedQnt() =>
		await UseWebProxyClient(x => x.GetAllLicenseUsedQnt());

	}
}
