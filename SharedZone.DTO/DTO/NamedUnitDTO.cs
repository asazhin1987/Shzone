using System.Runtime.Serialization;


namespace SharedZone.DTO
{
	[DataContract]
	public partial class NamedUnitDTO : BimacadUnitDTO
	{
		[DataMember(Name = "Id")]
		public int Id { get; set; }
		[DataMember(Name = "Name")]
		public string Name { get; set; }

	}
}
