using SharedZone.IWebService;
using SharedZone.ISysService;
using Ninject.Modules;
using SharedZone.WebSRC;
using SharedZone.SysSrc;
using System.Collections;
using System.Collections.Generic;

namespace SharedZone.Web.Util
{
	public class SharedZoneModule : NinjectModule
	{
		public static IEnumerable<string> Languages = new List<string> { "ru", "en" };
		private readonly string url;
		public SharedZoneModule(string url)
		{
			this.url = url;
		}
		public override void Load()
		{
			Bind<IWebSrc>().To<WebService<IWebSrc>>().WithConstructorArgument(url);
			Bind<ISysSrc>().To<SysService<ISysSrc>>().WithConstructorArgument(url);
		}
	}
}