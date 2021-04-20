using Bimacad.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SharedZone.DTO;

namespace SharedZone.IRevitService
{
	[ServiceContract]
	public interface IRevitSrc : IService
	{
		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		bool CheckConnection();

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		bool CheckLicense();

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		[FaultContract(typeof(NullKey))]
		[FaultContract(typeof(WrongKey))]
		[FaultContract(typeof(ZeroQnt))]
		bool TakeLicense(string clientName, string userName);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		IEnumerable<CollectionDTO> GetSchedule(string clientName, string version);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		IEnumerable<CollectionDTO> GetCollections(string clientName, string version);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		CollectionDTO GetCollection(int Id);


		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		bool SetStartResult(JobLaunchDTO launch);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		int CreateJobLaunch(JobLaunchDTO launch);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		bool SetEndResult(JobLaunchDTO launch);
		//[OperationContract]
		//[FaultContract(typeof(ModelCheck))]
		//void MergeClient(string clientName);
		//[OperationContract]
		//[FaultContract(typeof(ModelCheck))]
		//Task<IEnumerable<CollectionDTO>> GetScheduleAsync(string PCName);
	}
}
