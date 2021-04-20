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
	public class ServerController : SharedZoneController
	{
		public ServerController(IWebSrc service) : base(service) { }

		[HttpGet]
		[ActiveItem(MenuItem.RevitServers)]
		public ActionResult Servers()
		{
			return View(new RevitServersViewModel(null));
		}

		[HttpGet]
		[ActiveItem(MenuItem.Directories)]
		public ActionResult Directories()
		{
			return View("Servers", new DirectoriessViewModel(null));
		}

		[HttpGet]
		public async Task<PartialViewResult> ServerRows(string Search = "")
		{
			return PartialView("_ServerRows", await src.GetAllServersAsync(Search));
		}

		[HttpGet]
		public async Task<PartialViewResult> DirectoryRows(string Search = "")
		{
			return PartialView("_ServerRows", await src.GetAllDirectoriesAsync(Search));
		}

		[HttpGet]
		public PartialViewResult CreateServer()
		{
			return PartialView("_ServerEdit", new ServerViewModel() { IsDirectory = false });
		}

		[HttpGet]
		public PartialViewResult CreateDirectory()
		{
			return PartialView("_ServerEdit", new ServerViewModel() {IsDirectory = true });
		}

		[HttpGet]
		public async Task<PartialViewResult> EditServer(int Id)
		{
			var srv = await src.GetServerAsync(Id);
			return PartialView("_ServerEdit", new ServerViewModel(srv));
		}

		[HttpGet]
		public async Task<PartialViewResult> CloneServer(int Id)
		{
			var srv = await src.GetServerAsync(Id);
			var model = new ServerViewModel()
			{
				Id = 0,
				IsDirectory = srv.IsDirectory,
				Name = srv.Name,
				RevitVersionId = srv.RevitVersionId
			};

			return PartialView("_ServerEdit", model);
		}

		[HttpPost]
		public async Task<PartialViewResult> MergeServer(ServerViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					ServerDTO _model = Mapper.Map(new ServerDTO(), model) as ServerDTO;
					if (model.Id > 0)
						await src.UpdateServerAsync(_model);
					else
						await src.CreateServerAsync(_model);
					return PartialView("_Success", new SucceedResultActionViewModel(updateScript, Resources.Global.SuccessMsg));
				}
				catch (Exception ex)
				{
					return PartialView("_Success", new FailureResultActionViewModel("", ex.Message));
				}
			}
			else
			{
				return PartialView("_ServerEdit", model);
			}
		}


		[HttpPost]
		public async Task<string> RemoveServer(int Id)
		{
			try
			{
				await src.RemoveServerAsync(Id);
				return Resources.Global.SuccessRemoveMsg;
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		public PartialViewResult RevitVersions(int? selected)
		{
			var model = new RevitVersionsDropDownViewModel(src.GetNumericRevitVersions().Select(x => (NamedUnitViewModel)x), selected);
			return PartialView("_DropDown", model);
		}

	}
}