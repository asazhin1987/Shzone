using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace SharedZone.DTO
{
	[DataContract]
	public partial class ServerDTO : ModelRepositoryDTO
	{
		
		[DataMember]
		public string RevitVersionName { get; set; }

		[DataMember]
		public bool IsDirectory { get; set; }

		[DataMember]
		public DateTime? LastUpdate { get; set; }

		[DataMember]
		public int? AddedQnt { get; set; }

		[DataMember]
		public int? RemovedQnt { get; set; }

		[DataMember]
		public bool? Success { get; set; }

		[DataMember]
		public int ModelQnt { get; set; }

		[DataMember]
		public string Description { get; set; }

		//[DataMember]
		//public IEnumerable<ServerLogDTO> ServerLogs { get; set; }

		//public ServerLogDTO LastLog { get { return ServerLogs?.LastOrDefault(); } }

	}
}
