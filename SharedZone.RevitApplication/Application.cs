using Autodesk.Revit.UI;
using System;
using System.IO;
using System.Reflection;


namespace SharedZone.RevitApplication
{
	public class Application : IExternalApplication
	{
		public Result OnStartup(UIControlledApplication application)
		{
			try
			{
				//подключаем бибилиотеку с плагином
				Assembly asm = Assembly.LoadFrom(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"SharedZone.RevitPlugin.dll"));
				Assembly.LoadFrom(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"WpfStyles.dll"));

				Type panelFactoryType = asm.GetType("SharedZone.RevitPlugin.PanelFactory", true, true);

				//создаем экземпляр фабрики
				var panelFactory = Activator.CreateInstance(panelFactoryType, new object[] { application });

				return Result.Succeeded;
			}
			catch (Exception ex)
			{
				TaskDialog.Show("SharedZone", ex.Message);
				return Result.Failed;
			}
			
		}

		public Result OnShutdown(UIControlledApplication application) =>
			Result.Succeeded;

	}
}
