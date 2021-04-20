using SharedZone.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SharedZone.Web.Models
{
	public class LicenseViewModel
	{
		[Display(Name = "LicenseKey", ResourceType = typeof(Resources.Global))]
		[Required(AllowEmptyStrings = true, ErrorMessageResourceType = typeof(Resources.Required),
				  ErrorMessageResourceName = "Key")]
		[StringLength(125)]
		public string Key { get; set; }
		public bool Success { get; set; }
		public string Message { get; set; }
		public string Caption { get { return Resources.Site.License; } }
		public int LicensseQnt { get; }

		public LicenseViewModel()
		{
		}

		public LicenseViewModel(LicenseDTO item)
		{
			Success = item.Success;
			Key = item.Key;
			LicensseQnt = item.LicensseQnt;
			if (Success)
				Message = $"{Resources.Global.LicenseSuccessMsg} {item.EndLicDate}";
			else
				Message = $"{Resources.Global.LicenseTimeOutMsg} {item.EndLicDate}";
			//EndLicDate = item.EndLicDate;
		}

		public LicenseViewModel(bool succes, string msg)
		{
			Success = succes;
			Message = msg;
		}

	}
}