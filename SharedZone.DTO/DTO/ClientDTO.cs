using System.Runtime.Serialization;


namespace SharedZone.DTO
{
	[DataContract]
	public partial class ClientDTO : NamedUnitDTO
	{
		[DataMember]
		public string DefaultPath { get; set; }

		public ClientDTO() : base()
		{

		}

	}
}
