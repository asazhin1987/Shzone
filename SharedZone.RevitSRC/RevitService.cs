using Bimacad.Sys;
using SharedZone.DTO;
using SharedZone.IRevitService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SharedZone.RevitSRC
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
	   InstanceContextMode = InstanceContextMode.Single,
	   IncludeExceptionDetailInFaults = true)]

	public class RevitService<T> : IRevitSrc where T : IService
	{
		private readonly string Url;

		public RevitService(string host)
		{
			Url = $"http://{host}:80/SharedZoneRevitService";
		}

		
		#region proxy
		private M UseProxyClient<M>(Func<IRevitSrc, M> accessor) 
		{
			using (ProxyFactory<T> proxy = new ProxyFactory<T>(Url))
			{
				try
				{
					return accessor((IRevitSrc)proxy.Service);
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

		
		#endregion proxy


		public bool CheckConnection() =>
			UseProxyClient(x => x.CheckConnection());

		public bool CheckLicense() =>
			UseProxyClient(x => x.CheckLicense());

		public bool TakeLicense(string clientName, string userName) =>
			UseProxyClient(x => x.TakeLicense(clientName, userName));

		public IEnumerable<CollectionDTO> GetSchedule(string clientName, string version) =>
			UseProxyClient(x => x.GetSchedule(clientName, version));

		public IEnumerable<CollectionDTO> GetCollections(string clientName, string version) =>
			UseProxyClient(x => x.GetCollections(clientName, version));


		public bool SetStartResult(JobLaunchDTO launch) =>
			UseProxyClient(x => x.SetStartResult(launch));

		public int CreateJobLaunch(JobLaunchDTO launch) =>
			UseProxyClient(x => x.CreateJobLaunch(launch));

		public bool SetEndResult(JobLaunchDTO launch) =>
			UseProxyClient(x => x.SetEndResult(launch));

		public CollectionDTO GetCollection(int Id) =>
			UseProxyClient(x => x.GetCollection(Id));
	}
}
