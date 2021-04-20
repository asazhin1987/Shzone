using System.Runtime.Serialization;

namespace SharedZone.DTO
{
	[DataContract]
	public class LicenseCategogiesPercentDTO
	{
		[DataMember]
		public string CategoryName { get; set; }

		[DataMember]
		public decimal Percent { get; set; }
	}
}
