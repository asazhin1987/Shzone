using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Optimization;
using System.Web.Routing;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using SharedZone.Web.Util;
using Bimacad.Sys;

namespace SharedZone.Web
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			DependencyResolver.SetResolver(
				new NinjectDependencyResolver(
					new StandardKernel(new SharedZoneModule(GetServerAddress()))));


			string GetServerAddress()
			{
				string result = BTextWriter.ReadCurrentFile("host.txt");
				if (result != "")
					return result;
				BTextWriter.WriteCurrentFile("localhost", "host.txt");
				return GetServerAddress();
			}
		}
	}
}
