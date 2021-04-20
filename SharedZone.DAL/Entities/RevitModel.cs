
using System.Collections.Generic;
using SharedZone.DAL.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SharedZone.DAL.Entities
{
	public class RevitModel : NamedUnit
	{
		[StringLength(255)]
		public string Path { get; set; }

		public int RevitServerId { get; set; }
		public RevitServer RevitServer { get; set; }

		public bool IsFolder { get; set; }
		public int? ParentId { get; set; }
		public RevitModel Parent { get; set; }

		public virtual ICollection<RevitModel> RevitModels { get; set; }

		public virtual ICollection<Collection> Collections { get; set; }

		public RevitModel()
		{
			Collections = new List<Collection>();
			RevitModels = new List<RevitModel>();
		}
	}
}
