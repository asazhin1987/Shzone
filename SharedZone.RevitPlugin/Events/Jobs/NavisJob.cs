using System;
using Autodesk.Revit.UI;
using System.Linq;
using SharedZone.DTO;
using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace SharedZone.RevitPlugin.Events
{
	public partial class ExportEvent : IExternalEventHandler
	{
		private void Export(Document doc, NavisJobDTO job, string name)
		{
			using (NavisworksExportOptions opt = GetNavisworksExportOptions(doc, job))
			{
				try
				{
					doc.Export(job.Path, name, opt);
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
		}

		NavisworksExportOptions GetNavisworksExportOptions(Document doc, NavisJobDTO job)
		{
			NavisworksExportOptions optnwc = new NavisworksExportOptions()
			{
				ExportElementIds = job.TransformElementsProperty,
				ExportUrls = job.TransformURL,
				ExportRoomAsAttribute = job.TransformRoom,
				ConvertElementProperties = job.TransformElementsProperty,
				ExportLinks = job.TransformLinkedFiles,
				FindMissingMaterials = job.TransformConstructionElements,
				DivideFileIntoLevels = job.DevideFiles,
				ExportRoomGeometry = job.ExportGeometryRoom,
				Coordinates = job.NavisCoordinateId == 1 ? NavisworksCoordinates.Shared : NavisworksCoordinates.Internal,
			};
			optnwc.Parameters = GetNavisworksParameters();
			SetViewId();
			return optnwc;


			NavisworksParameters GetNavisworksParameters()
			{
				if (job.NavisConvertElementsPropertyId == 1)
				{
					return NavisworksParameters.All;
				}
				else if (job.NavisConvertElementsPropertyId == 3)
				{
					return NavisworksParameters.Elements;
				}
				else
				{
					return NavisworksParameters.None;
				}
			}

			void SetViewId()
			{
				if (job.NavisViewId == 2 && job.ViewName.Length > 0)
				{
					using (FilteredElementCollector viewCollector = new FilteredElementCollector(doc))
					{
						var view = viewCollector.OfClass(typeof(View)).ToElements().Where(x => x.Name.ToLower() == job.ViewName.ToLower()).FirstOrDefault();
						if (view != null)
						{
							optnwc.ExportScope = NavisworksExportScope.View;
							optnwc.ViewId = view.Id;
						}
					}
				}
			}
		}
	}
}
