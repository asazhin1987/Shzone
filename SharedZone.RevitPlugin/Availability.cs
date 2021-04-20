using Autodesk.Revit.UI;
using Autodesk.Revit.DB;


namespace SharedZone.RevitPlugin
{
	public class Availability
   : IExternalCommandAvailability
	{
		public bool IsCommandAvailable(
		  UIApplication a,
		  CategorySet b)
		{
			return true;
		}
	}
}
