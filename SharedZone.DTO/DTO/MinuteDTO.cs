using System.Runtime.Serialization;

namespace SharedZone.DTO
{
	[DataContract]
	public class MinuteDTO : NamedUnitDTO
	{
		[DataMember]
		public int MinuteValue { get; set; }
	}
}
