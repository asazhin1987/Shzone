using System.Runtime.Serialization;


namespace SharedZone.DTO
{

	[DataContract]
	public partial class VersionDTO  : BimacadUnitDTO
	{
		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public int VersionValue { get; set; }

	}
}
