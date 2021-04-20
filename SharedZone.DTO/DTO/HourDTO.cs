using System.Runtime.Serialization;

namespace SharedZone.DTO
{
	[DataContract]
	public class HourDTO : NamedUnitDTO
	{
		[DataMember]
		public int HourValue { get; set; }
	}
}
