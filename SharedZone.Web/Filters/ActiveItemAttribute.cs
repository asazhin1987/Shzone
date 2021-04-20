using SharedZone.Web.Models;
using System.Web.Mvc;

namespace SharedZone.Web.Filters
{
	public class ActiveItemAttribute : FilterAttribute, IResultFilter
	{
		public MenuItem Item { get; }

		public ActiveItemAttribute(MenuItem item)
		{
			this.Item = item;
		}

		public void OnResultExecuting(ResultExecutingContext filterContext) => 
			filterContext.Controller.TempData["ActiveItem"] = Item;


		public void OnResultExecuted(ResultExecutedContext filterContext)
		{
			
		}
	}
}