//using SharedZone.DTO.Interfaces;
using System.Runtime.Serialization;


namespace SharedZone.DTO
{
	[DataContract]
	public partial class RevitJobDTO : JobDTO//, IJobDTO
	{
		[DataMember]
		public bool TruncateView { get; set; }

		[DataMember]
		public string ExceptionViews { get; set; }

		[DataMember]
		public bool AffixLinks { get; set; }

		[DataMember]
		public string AffixesLinks { get; set; }

		[DataMember]
		public bool TruncateLinks { get; set; }

		[DataMember]
		public bool Purge { get; set; }

		
	}
}
