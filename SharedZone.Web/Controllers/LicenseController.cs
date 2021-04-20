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

	public class LicenseController : SharedZoneController
	{
		public LicenseController(IWebSrc service) : base(service) { }


		[ActiveItem(MenuItem.License)]
		public async Task<ActionResult> License(LicenseViewModel model)
		{
			return View(await GetLicenseReportViewModel());
		}


		public async Task<PartialViewResult> LicensePartial()
		{
			return PartialView("_License", await GetLicenseReportViewModel());
		}

		[NonAction]
		public async Task<LicenseReportViewModel> GetLicenseReportViewModel()
		{
			try
			{
				return new LicenseReportViewModel()
				{
					License = await GetLicenseVM(),
					AllQnt = await src.GetLicenseQnt(),
					Last3MonthsQnt = await src.GetLicenseUsedQnt(3),
					LicenseUsings = await src.GetLicenseMonthsUsingAsync(12),
					Licenses = await src.GetAllLicenseUsingsAsync(),
					UsingQnt = await src.GetAllLicenseUsedQnt()
				};
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		[HttpGet]
		public PartialViewResult EditLicense()
		{
			return PartialView("_EditLicense");
		}

		[HttpGet]
		public async Task<PartialViewResult> LicenseResult()
		{
			return PartialView("_LicenseResult", await GetLicenseVM());
		}

		[HttpPost]
		public async Task<PartialViewResult> EditLicense(LicenseViewModel model)
		{
			if (ModelState.IsValid)
			{
				await src.SetLicenseAsync(model.Key);
				SetLicenseCookie(src.CheckLicense());
				return PartialView("_Success", new SucceedResultActionViewModel(updateScript, Resources.Global.SuccessMsg));
			}
			return PartialView("_EditLicense", model);
		}

		public async Task<PartialViewResult> GetTableUse()
		{
			return PartialView("_TableUse", await src.GetAllLicenseUsingsAsync());
		}

		[HttpPost]
		public async Task<PartialViewResult> DeleteLicenses(IEnumerable<int> lics)
		{
			await src.BreakLicensesAsync(lics);
			return PartialView("_License", await GetLicenseReportViewModel());
		}


		[NonAction]
		public async Task<LicenseViewModel> GetLicenseVM()
		{
			try
			{
				var lic = await src.GetLicenseAsync();
				return new LicenseViewModel(item: lic);
			}
			catch (WrongKeyException)
			{
				return new LicenseViewModel(false, Resources.Global.LicenseWrongKeyMsg);
			}
			catch (NullKeyException)
			{
				return new LicenseViewModel(false, Resources.Global.LicenseNullKeyMsg);
			}
			catch (ZeroQntException)
			{
				return new LicenseViewModel(false, Resources.Global.LicenseZeroQnt);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpGet]
		public ActionResult CheckLicense(string previewurl)
		{
			try
			{
				bool lic = src.CheckLicense();
				SetLicenseCookie(lic);
				if (lic == true)
					return Redirect(previewurl);
				else
					return RedirectToAction("License");
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		private void SetLicenseCookie(bool lic)
		{
			Session["License"] = lic;
		}


		#region Dashboard
		public PartialViewResult GetPipe()
		{
			return PartialView("_Pipe");
		}

		public PartialViewResult GetAllQnt()
		{
			return PartialView("_AllQnt");
		}

		public PartialViewResult GetUsesQnt()
		{
			return PartialView("_UsesQnt");
		}

		#endregion Dashboard
		/*public async Task<ActionResult> License(LicenseViewModel model)
		{
			return View(await GetLicenseVM());
		}

		public async Task<PartialViewResult> LicensePartial()
		{
			return PartialView("_License", await GetLicenseVM());
		}


		[HttpGet]
		public PartialViewResult EditLicense()
		{
			return PartialView("_EditLicense");
		}

		[HttpPost]
		public async Task<PartialViewResult> EditLicense(LicenseViewModel model)
		{
			if (ModelState.IsValid)
			{
				await src.SetLicenseAsync(model.Key);
				return PartialView("_License", await GetLicenseVM());
			}
			return PartialView("_EditLicense", model);
		}


		[NonAction]
		public async Task<LicenseViewModel> GetLicenseVM()
		{
			try
			{
				var lic = await src.GetLicenseAsync();
				return new LicenseViewModel(item: lic);
			}
			catch (WrongKeyException)
			{
				return new LicenseViewModel(false, Resources.Global.LicenseWrongKeyMsg);
			}
			catch (NullKeyException)
			{
				return new LicenseViewModel(false, Resources.Global.LicenseNullKeyMsg);
			}
			catch (ZeroQntException)
			{
				return new LicenseViewModel(false, Resources.Global.LicenseZeroQnt);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpGet]
		public ActionResult CheckLicense(string previewurl)
		{
			try
			{
				bool lic = src.CheckLicense();
				SetLicenseCookie(lic);
				if (lic == true)
					return Redirect(previewurl);
				else
					return RedirectToAction("License");
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		private void SetLicenseCookie(bool lic)
		{
			Session["License"] = lic;
		}*/
	}
}