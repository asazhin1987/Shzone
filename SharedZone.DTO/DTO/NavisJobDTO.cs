//using SharedZone.DTO.Interfaces;
using System.Runtime.Serialization;


namespace SharedZone.DTO
{
	[DataContract]
	public partial class NavisJobDTO : JobDTO//, IJobDTO
	{
		[DataMember]
		public bool ConvertIdentif { get; set; }

		[DataMember]
		public bool TransformURL { get; set; }

		[DataMember]
		public bool TransformRoom { get; set; }

		[DataMember]
		public bool TransformElementsProperty { get; set; }

		[DataMember]
		public bool TransformLinkedFiles { get; set; }

		[DataMember]
		public bool TransformConstructionElements { get; set; }

		[DataMember]
		public bool DevideFiles { get; set; }

		[DataMember]
		public bool ExportGeometryRoom { get; set; }

		[DataMember]
		public int NavisConvertElementsPropertyId { get; set; }

		[DataMember]
		public int NavisCoordinateId { get; set; }

		[DataMember]
		public int NavisViewId { get; set; }

		[DataMember]
		public string ViewName { get; set; }

	
	}
}
