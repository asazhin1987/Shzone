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
		private void Export(Document doc, IFCJobDTO job, string name)
		{
			using (Transaction t = new Transaction(doc, string.Format("SharedZone Export ifc File ", name)))
			{
				try
				{
					IFCExportOptions options = GetOptions(job, doc);
					doc.Export(job.Path, name, options);
					t.Commit();
				}
				catch (Exception ex)
				{
					t.RollBack();
					throw ex;
				}
			}
		}

		IFCExportOptions GetOptions(IFCJobDTO job, Document doc)
		{
			IFCExportOptions options = new IFCExportOptions()
			{
				WallAndColumnSplitting = job.DivideWalls,
				ExportBaseQuantities = job.BasicValues
			};

			options.FileVersion = GetIFCVersion();
			SetBoundaryLevel();
			SetViewId();
			return options;


			IFCVersion GetIFCVersion()
			{
				switch (job.IFCFileTypeId)
				{
					case 2:
						{
							return IFCVersion.IFC2x2;
						}
					case 3:
						{
							return IFCVersion.IFC2x3;
						}
					case 4:
						{
							return IFCVersion.IFCBCA;
						}
					case 5:
						{
							return IFCVersion.IFC2x3CV2;
						}
					case 6:
						{
							return IFCVersion.IFCCOBIE;
						}
				}
				return IFCVersion.Default;
			}

			void SetViewId()
			{
				if (job.CurrentView && job.CurrentViewName.Length > 0)
				{
					using (FilteredElementCollector viewCollector = new FilteredElementCollector(doc))
					{
						var view = viewCollector.OfClass(typeof(View)).ToElements().Where(x => x.Name.ToLower() == job.CurrentViewName.ToLower()).FirstOrDefault();
						if (view != null)
							options.FilterViewId = view.Id;
					}
				}
			}

			void SetBoundaryLevel()
			{
				switch (job.IFCIncludeBoundatyId)
				{
					case 2:
						{
							options.SpaceBoundaryLevel = 1;
							break;
						}
					case 3:
						{
							options.SpaceBoundaryLevel = 2;
							break;
						}
				}
			}
		}


	}
}
