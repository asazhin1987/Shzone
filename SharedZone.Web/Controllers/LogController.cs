using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SharedZone.Web.Models;
using SharedZone.Web.Filters;
using SharedZone.DTO;
using System.Web.UI.WebControls.WebParts;
using SharedZone.IWebService;
using Bimacad.Sys;


namespace SharedZone.Web.Controllers
{
	[Licensing]
	public class LogController : SharedZoneController
	{
		public LogController(IWebSrc service) : base(service) { }

		[HttpGet]
		[ActiveItem(MenuItem.Log)]
		public async Task<ActionResult> Logs()
        {
            return View(new LogsDateViewModel(await src.GetMonthJobLaunchesAsync()));
        }


		[HttpGet]
		public async Task<PartialViewResult> GetLog(int Id)
		{
			return PartialView("_LogDetails", await src.GetJobLaunchAsync(Id));
		}
	}
}