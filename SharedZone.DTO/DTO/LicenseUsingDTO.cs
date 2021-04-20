using System.Runtime.Serialization;
using System;

namespace SharedZone.DTO
{
	[DataContract]
	public class LicenseUsingDTO : BimacadUnitDTO
	{
		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public string StantionName { get; set; }

		[DataMember]
		public string UserName { get; set; }

		[DataMember]
		public DateTime LastAccess { get; set; }

		[DataMember]
		public int? LastAllUsingQnt { get; set; }

		[DataMember]
		public int? LastUniqUsingQnt { get; set; }
	}
}
