using SharedZone.DAL.Abstract;


namespace SharedZone.DAL.Entities
{
	public class NavisJob : Job
	{

		public bool ConvertIdentif { get; set; }
		public bool TransformURL { get; set; }
		public bool TransformRoom { get; set; }
		public bool TransformElementsProperty { get; set; }
		public bool TransformLinkedFiles { get; set; }
		public bool TransformConstructionElements { get; set; }
		public bool DevideFiles { get; set; }
		public bool ExportGeometryRoom { get; set; }

		public int NavisConvertElementsPropertyId { get; set; }
		public NavisConvertElementsProperty NavisConvertElementsProperty { get; set; }

		public int NavisCoordinateId { get; set; }
		public NavisCoordinate NavisCoordinate { get; set; }

		public int NavisViewId { get; set; }
		public NavisView NavisView { get; set; }

		public string ViewName { get; set; }

		public int CollectionId { get; set; }
		public Collection Collection { get; set; }

	}
}
