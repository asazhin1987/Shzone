
using System.Collections.Generic;


namespace SharedZone.Web.Models
{
	public enum MenuItem {Collections, RevitServers, Directories, License, Schedule, Log }
	public class PageMenuViewModel
	{
		public IEnumerable<LinqMenuViewModel> Items { get; set; }
	}

	public class GroupMenuViewModel 
	{
		public string Title { get; set; }
		public ICollection<LinqMenuViewModel> Items { get; set; }
	}

	public class LinqMenuViewModel
	{
		public string Title { get; set; }
		public string Image { get; set; }
		public string ControllerName { get; set; }
		public string ActionName { get; set; }
		public MenuItem MenuItem { get; set; }
	}
}