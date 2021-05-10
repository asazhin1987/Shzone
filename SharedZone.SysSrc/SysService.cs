using Bimacad.Sys;
using SharedZone.DTO;
using SharedZone.ISysService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
namespace SharedZone.SysSrc
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
	   InstanceContextMode = InstanceContextMode.Single,
	   IncludeExceptionDetailInFaults = true)]
	public class SysService<T> : ISysSrc where T : IService
	{

		private readonly string Url;

		public SysService(string host)
		{
			Url = $"http://{host}:80/SharedZoneSysService";
		}

		async Task<M> UseProxyClientAsync<M>(Func<ISysSrc, Task<M>> accessor)
		{
			using (ProxyFactory<T> proxy = new ProxyFactory<T>(Url))
			{
				try
				{
					return await accessor((ISysSrc)proxy.Service);
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

		private M UseProxyClientSync<M>(Func<ISysSrc, M> accessor)
		{
			using (ProxyFactory<T> proxy = new ProxyFactory<T>(Url))
			{
				try
				{
					return accessor((ISysSrc)proxy.Service);
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

		async Task UseProxyClientAsync(Func<ISysSrc, Task> accessor)
		{
			using (ProxyFactory<T> proxy = new ProxyFactory<T>(Url))
			{
				try
				{
					await accessor((ISysSrc)proxy.Service);
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

		

		public async Task ClearJobLogAsync(int days)
		{
			await UseProxyClientAsync(x => x.ClearJobLogAsync(days));
		}
			
		public async Task ClearServerLogAsync(int days) =>
			await UseProxyClientAsync(x => x.ClearServerLogAsync(days));

		public async Task<IEnumerable<ServerDTO>> GetAllDirectoriesAsync(string mask = "") =>
			await UseProxyClientAsync(x => x.GetAllDirectoriesAsync(mask));

		public async Task<IEnumerable<ServerDTO>> GetAllServersAsync(string mask = "") =>
			await UseProxyClientAsync(x => x.GetAllServersAsync(mask));

		public async Task<ServerLogDTO> MergeFilesAsync(ServerDTO server) =>
			await UseProxyClientAsync(x => x.MergeFilesAsync(server));

		public async Task UpdateDirectoriesAsync() =>
			await UseProxyClientAsync(x => x.UpdateDirectoriesAsync());

		public async Task UpdateRevitServersAsync() =>
			await UseProxyClientAsync(x => x.UpdateRevitServersAsync());

		public async Task UpdateServerAsync(int Id) =>
			await UseProxyClientAsync(x => x.UpdateServerAsync(Id));

		public async Task WriteServerLogAsync(ServerLogDTO serverLog) =>
			await UseProxyClientAsync(x => x.WriteServerLogAsync(serverLog));

		public string TestUpdateServer(int Id) =>
				UseProxyClientSync(x => x.TestUpdateServer(Id));

		public bool TestService() =>
				UseProxyClientSync(x => x.TestService());

	}
}
