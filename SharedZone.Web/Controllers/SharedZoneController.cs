using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SharedZone.Web.Models;
using SharedZone.Web.Filters;
using SharedZone.IWebService;
using SharedZone.Web.Util;
using SharedZone.ISysService;
using System.Threading.Tasks;

namespace SharedZone.Web.Controllers
{
	[SharedZoneException]
	[Culture]
	public class SharedZoneController : Controller
    {
		protected readonly IWebSrc src;
		protected readonly ISysSrc sysSrc;
		protected readonly string updateScript = "UpdateResult()";

		public SharedZoneController(IWebSrc service, ISysSrc ssrc = null)
		{
			src = service;
			sysSrc = ssrc;
		}

		public ActionResult Index() => RedirectToAction("Collections", "Collection");


		[HttpGet]
		public ActionResult PageMenu() =>
		PartialView(new PageMenuViewModel()
		{
			Items = new List<LinqMenuViewModel>
			{
					new LinqMenuViewModel(){Title = Resources.Site.Collections, MenuItem = MenuItem.Collections, ActionName = "Collections", ControllerName = "Collection", Image = "settings-32.png" },
					new LinqMenuViewModel(){Title = Resources.Site.Servers, MenuItem = MenuItem.RevitServers, ActionName = "Servers", ControllerName = "Server", Image = "geography-32.png" },
					new LinqMenuViewModel(){Title = Resources.Site.Directories, MenuItem = MenuItem.Directories, ActionName = "Directories", ControllerName = "Server", Image = "folder-32.png" },
					new LinqMenuViewModel(){Title = Resources.Site.Log, MenuItem = MenuItem.Log, ActionName = "Logs", ControllerName = "Log", Image = "textfile-32.png" },
					new LinqMenuViewModel(){Title = Resources.Site.License, MenuItem = MenuItem.License, ActionName = "License", ControllerName = "License", Image = "lock-32.png" },
			}
		});

		[ActiveItem(MenuItem.Collections)]
		public ActionResult Test()
		{
			return View();
		}

		public string Test2(int Id)
		{
			return sysSrc.TestUpdateServer(Id);

		}

		private string GetCookieLanguage()
		{
			return Request.Cookies["Lang"]?.Value ?? "ru";
		}

		[HttpPost]
		public ActionResult SetLanguage(string language)
		{
			string returnUrl = Request.UrlReferrer.AbsolutePath;
			if (!SharedZoneModule.Languages.Contains(language))
				language = "ru";

			HttpCookie cookie = Request.Cookies["Lang"];
			if (cookie != null)
			{
				Response.Cookies["Lang"].Value = language;
			}
			else
			{
				cookie = new HttpCookie("Lang")
				{
					HttpOnly = false,
					Value = language,
					Expires = DateTime.Now.AddYears(1)
				};
				Response.Cookies.Add(cookie);
			}
			return Redirect(returnUrl);
		}

		[HttpGet]
		public PartialViewResult GetLanguage()
		{
			var lang = GetCookieLanguage();
			ViewBag.ChangeLang = lang == "en" ? "ru" : "en";
			return PartialView("_Language", lang);
		}

		#region admin


		public async Task<string> UpdateDirectories()
		{
			try
			{
				await sysSrc.UpdateDirectoriesAsync();
				return $"OK {DateTime.Now}";
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		public async Task<string> UpdateServer(int Id)
		{
			try
			{
				await sysSrc.UpdateServerAsync(Id);
				return $"OK {DateTime.Now}";
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		public async Task<string> UpdateServers()
		{
			try
			{
				await sysSrc.UpdateRevitServersAsync();
				return $"OK {DateTime.Now}";
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		#endregion admin
	}
}