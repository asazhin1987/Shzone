//using SharedZone.DTO.Interfaces;
using System.Runtime.Serialization;


namespace SharedZone.DTO
{
	[DataContract]
	public abstract class JobDTO : NamedUnitDTO//, IJobDTO
	{
		[DataMember]
		public string Path { get; set; }

		[DataMember]
		public int CollectionId { get; set; }

		[DataMember]
		public string CollectionName { get; set; }

	}
}
