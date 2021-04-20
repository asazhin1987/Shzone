//using SharedZone.DTO.Interfaces;
using System.Runtime.Serialization;


namespace SharedZone.DTO
{
	[DataContract]
	public partial class IFCJobDTO : JobDTO//, IJobDTO
	{
		[DataMember]
		public bool CurrentView { get; set; }

		[DataMember]
		public string CurrentViewName { get; set; }

		[DataMember]
		public bool DivideWalls { get; set; }

		[DataMember]
		public bool BasicValues { get; set; }

		[DataMember]
		public int IFCFileTypeId { get; set; }

		[DataMember]
		public int IFCIncludeBoundatyId { get; set; }

	}
}
