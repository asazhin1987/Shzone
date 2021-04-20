using SharedZone.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static SharedZone.Web.Application.UI;

namespace SharedZone.Web.Models
{
	public class CollectionsViewModel
	{
		[Display(Name = "CollectionsCaption", ResourceType = typeof(Resources.Setting))]
		public string Caption { get; set; }

		public IEnumerable<CollectionDTO> Items { get; set; }

		[Display(Name = "Id", ResourceType = typeof(Resources.Setting))]
		public string ColumnId { get; set; } = "Id";

		[Display(Name = "Name", ResourceType = typeof(Resources.Setting))]
		public string ColumnName { get; set; } = "Name";

		[Display(Name = "Client", ResourceType = typeof(Resources.Setting))]
		public string ColumnClient { get; set; } = "Client";

		[Display(Name = "LastStart", ResourceType = typeof(Resources.Setting))]
		public string ColumnLastStart { get; set; } = "LastDateTime";

		[Display(Name = "NextStart", ResourceType = typeof(Resources.Setting))]
		public string ColumnNextStart { get; set; } = "NextDateTime";

		[Display(Name = "Schedule", ResourceType = typeof(Resources.Setting))]
		public string ColumnSchedule{ get; set; } = "Schedule";

		[Display(Name = "Description", ResourceType = typeof(Resources.Setting))]
		public string ColumnDescription { get; set; } = "Description";

		public string Sort { get; set; } = "Id";
		public string Search { get; set; } = "";

		public ViewType ViewType { get; set; }


		public CollectionsViewModel(IEnumerable<CollectionDTO> items)
		{
			Items = items;
		}

		public CollectionsViewModel()
		{
			
		}
	}
}