using System.Runtime.Serialization;
using System.Collections.Generic;

namespace SharedZone.DTO
{
	[DataContract]
	public partial class RevitModelDTO : ModelRepositoryDTO
	{
		[DataMember]
		public string Path { get; set; }

		[DataMember]
		public int RevitServerId { get; set; }

		[DataMember]
		public string RevitServer { get; set; }

		[DataMember]
		public bool IsFolder { get; set; }

		[DataMember]
		public int? ParentId { get; set; }

		[DataMember]
		public string ParentName { get; set; }

		[DataMember]
		public bool Checked { get; set; }

		public override bool Equals(object obj) =>
			obj != null && Equals(obj as RevitModelDTO);


		public bool Equals(RevitModelDTO dto) =>
			Path == dto.Path && RevitServerId == dto.RevitServerId;

		public override int GetHashCode() =>
			(RevitServerId, Path).GetHashCode();

		public RevitModelDTO()
		{
			Folders = new List<RevitModelDTO>();
			Files = new List<RevitModelDTO>();

		}
	}
}
