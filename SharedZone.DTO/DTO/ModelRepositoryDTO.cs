using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;

namespace SharedZone.DTO
{
	[DataContract]
	public class ModelRepositoryDTO : NamedUnitDTO
	{
		[DataMember]
		public int RevitVersionId { get; set; }

		[DataMember]
		public IEnumerable<RevitModelDTO> Folders { get; set; }

		[DataMember]
		public IEnumerable<RevitModelDTO> Files { get; set; }

		public int FilesCount { get { return Files.Count() + Folders.Sum(x => x.FilesCount); } }

		public int SelectedFilesCount { get { return Files.Where(x => x.Checked).Count() + Folders.Sum(x => x.SelectedFilesCount); } }
	}
}
