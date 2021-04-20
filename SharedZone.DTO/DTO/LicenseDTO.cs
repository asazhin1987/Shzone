using System;
using System.Runtime.Serialization;


namespace SharedZone.DTO
{
	[DataContract]
	public partial class LicenseDTO
	{
		[DataMember]
		public bool Success { get; set; }
		[DataMember]
		public DateTime EndLicDate { get; set; }
		[DataMember]
		public string Key { get; set; }
		[DataMember]
		public int LicensseQnt { get; set; }
	}
}
