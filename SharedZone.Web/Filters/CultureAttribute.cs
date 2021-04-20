using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using SharedZone.Web.Util;

namespace SharedZone.Web.Filters
{
	public class CultureAttribute : FilterAttribute, IActionFilter
	{
		

		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
            HttpCookie cultureCookie = filterContext.HttpContext.Request.Cookies["Lang"];
            string lang = "ru";
            if (cultureCookie != null)
                lang = cultureCookie.Value;
            if (!SharedZoneModule.Languages.Contains(lang))
				lang = "ru";


			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(lang);
        }


		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
			//
		}
	}
}