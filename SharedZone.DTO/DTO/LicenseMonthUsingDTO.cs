using System.Runtime.Serialization;

namespace SharedZone.DTO
{
	[DataContract]
	public class LicenseMonthUsingDTO : BimacadUnitDTO
	{
		[DataMember]
		public int Year { get; set; }

		[DataMember]
		public int MonthNum { get; set; }

		[DataMember]
		public int AllQnt { get; set; }

		[DataMember]
		public int UniqQnt { get; set; }
	}
}
