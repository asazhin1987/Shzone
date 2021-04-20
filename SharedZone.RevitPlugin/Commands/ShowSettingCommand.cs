using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Threading.Tasks;

namespace SharedZone.RevitPlugin.Commands
{
	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]

	public class ShowSettingCommand : IExternalCommand
	{
		

		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			var src = SettingApplicationFactory.GetInstance();
			src.ShowSettingsWindows();

			return Result.Succeeded;
		}

	}
}
