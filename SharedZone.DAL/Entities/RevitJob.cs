using System;
using System.ComponentModel.DataAnnotations;
using SharedZone.DAL.Abstract;

namespace SharedZone.DAL.Entities
{
	public class RevitJob : Job
	{

		public bool TruncateView { get; set; }

		[StringLength(255)]
		public string ExceptionViews { get; set; }
		public bool AffixLinks { get; set; }

		[StringLength(255)]
		public string AffixesLinks { get; set; }
		public bool TruncateLinks { get; set; }
		public bool Purge { get; set; }

		public int CollectionId { get; set; }
		public Collection Collection { get; set; }
	}
}
