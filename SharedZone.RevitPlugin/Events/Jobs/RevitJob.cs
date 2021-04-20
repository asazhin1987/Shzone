using System;
using Autodesk.Revit.UI;
using System.Linq;
using SharedZone.DTO;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace SharedZone.RevitPlugin.Events
{
	public partial class ExportEvent : IExternalEventHandler
	{
		//RevitJobDTO job;
		private void Export(Document doc, RevitJobDTO job, string name)
		{
			//job = _job;
			try
			{
				using (SaveAsOptions options = new SaveAsOptions()
				{
					OverwriteExistingFile = true,
					Compact = true, 
				})
				{
					WorksharingSaveAsOptions ws = new WorksharingSaveAsOptions
					{
						SaveAsCentral = true
					};
					options.SetWorksharingOptions(ws);

					//purges
					if (job.TruncateView)
						try
						{
							TruncateView(doc, GetExceptionViews(job.ExceptionViews));
						}
						catch { }
					if (job.Purge)
						try
						{
							Purge(doc);
						}
						catch { }
					if (job.AffixLinks)
						try
						{
							AffixLinks(doc, GetExceptionViews(job.AffixesLinks));
						}
						catch { }
					if (job.TruncateLinks)
						try
						{
							TruncateLinks(doc);
						}
						catch { }
					doc.SaveAs(Path.Combine(job.Path, name), options);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



		#region
		private List<string> GetExceptionViews(string sArr)
		{
			return sArr.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
		}

		private void TruncateView(Document doc, List<string> ExceptionViews)
		{
			try
			{
				//get views
				using (FilteredElementCollector col = new FilteredElementCollector(doc))
				{
					List<Element> views = col.OfClass(typeof(View)).Cast<View>().Where(x => x.ViewType != ViewType.ProjectBrowser && x.ViewType != ViewType.SystemBrowser).Cast<Element>().ToList();

					foreach (string f in ExceptionViews)
					{
						List<Element> viewsTostay = views.Where(x => x.Name.Contains(f)).ToList();
						foreach (Element item in viewsTostay)
						{
							views.Remove(item);
						}
					}
					//Delete views
					Delete(doc, views, "BimacadMonitor Purge views");
				}

				//create 3d view
				using (Transaction tran = new Transaction(doc, "NewView3D - BimacadMonitor"))
				{
					tran.Start();
					using (FilteredElementCollector col = new FilteredElementCollector(doc))
					{
						List<Element> listvt = col.OfClass(typeof(ViewFamilyType)).ToElements().ToList();
						foreach (Element el in listvt)
						{
							ViewFamilyType vt = el as ViewFamilyType;
							if (vt.ViewFamily == ViewFamily.ThreeDimensional)
							{
								using (View3D.CreateIsometric(doc, vt.Id))
								{
									break;
								}
							}

						}
					}
					tran.Commit();
				}

			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		private void AffixLinks(Document doc, List<string> Links)
		{
			try
			{
				using (FilteredElementCollector col = new FilteredElementCollector(doc))
				{
					List<RevitLinkType> links = col.OfClass(typeof(RevitLinkType)).Cast<RevitLinkType>().ToList();
					List<RevitLinkType> linksToPin = new List<RevitLinkType>();
					foreach (RevitLinkType link in links)
					{
						foreach (string filter in Links)
						{
							if (link.Name.Contains(filter)) linksToPin.Add(link);
						}
					}
					using (Transaction t = new Transaction(doc, "PinLinks"))
					{
						t.Start();

						foreach (RevitLinkType link in linksToPin)
						{
							link.AttachmentType = AttachmentType.Attachment;
						}
						_ = t.Commit();
					}

					using (Transaction t = new Transaction(doc, "RelativeLinks"))
					{
						t.Start();

						foreach (RevitLinkType link in linksToPin)
						{
							try
							{
								link.PathType = PathType.Relative;
							}
							catch
							{
								continue;
							}

						}

						t.Commit();
					}
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void TruncateLinks(Document doc)
		{
			try
			{
				List<ElementId> files = ExternalFileUtils.GetAllExternalFileReferences(doc).ToList();
				List<Element> le = new List<Element>();
				files.ForEach(x => le.Add(doc.GetElement(x)));

				List<Element> links = new List<Element>();

				foreach (Element item in le)
				{
					string type = item.ToString();
					if (type.Equals("Autodesk.Revit.DB.CADLinkType") |
						type.Equals("Autodesk.Revit.DB.RevitLinkType") |
						type.Equals("Autodesk.Revit.DB.PointCloudType"))
					{
						if (item is RevitLinkType rvtLinkType && rvtLinkType.AttachmentType != AttachmentType.Attachment) links.Add(item);
						else continue;

					}
				}

				Delete(doc, links, "Purge links");
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void Purge(Document doc)
		{
			try
			{
				MethodInfo GetUnusedAppearances = doc.GetType().GetMethod("GetUnusedAppearances", BindingFlags.NonPublic | BindingFlags.Instance);
				MethodInfo GetUnusedMaterials = doc.GetType().GetMethod("GetUnusedMaterials", BindingFlags.NonPublic | BindingFlags.Instance);
				MethodInfo GetUnusedFamilies = doc.GetType().GetMethod("GetUnusedFamilies", BindingFlags.NonPublic | BindingFlags.Instance);
				MethodInfo GetUnusedImportCategories = doc.GetType().GetMethod("GetUnusedImportCategories", BindingFlags.NonPublic | BindingFlags.Instance);
				MethodInfo GetUnusedStructures = doc.GetType().GetMethod("GetUnusedStructures", BindingFlags.NonPublic | BindingFlags.Instance);
				MethodInfo GetUnusedSymbols = doc.GetType().GetMethod("GetUnusedSymbols", BindingFlags.NonPublic | BindingFlags.Instance);
				MethodInfo GetUnusedThermals = doc.GetType().GetMethod("GetUnusedThermals", BindingFlags.NonPublic | BindingFlags.Instance);
				MethodInfo GetNonDeletableUnusedElements = doc.GetType().GetMethod("GetNonDeletableUnusedElements", BindingFlags.NonPublic | BindingFlags.Instance);

				int num = 0;
				while (true)
				{
					HashSet<ElementId> hashSet = new HashSet<ElementId>();

					ICollection<ElementId> unusedAppearances = GetUnusedAppearances.Invoke(doc, null) as ICollection<ElementId>;
					foreach (ElementId current in unusedAppearances)
					{
						hashSet.Add(current);
					}

					ICollection<ElementId> unusedMaterials = GetUnusedMaterials.Invoke(doc, null) as ICollection<ElementId>;
					foreach (ElementId current2 in unusedMaterials)
					{
						hashSet.Add(current2);
					}

					ICollection<ElementId> unusedFamilies = GetUnusedFamilies.Invoke(doc, null) as ICollection<ElementId>;
					foreach (ElementId current3 in unusedFamilies)
					{
						hashSet.Add(current3);
					}

					ICollection<ElementId> unusedImportCategories = GetUnusedImportCategories.Invoke(doc, null) as ICollection<ElementId>;
					foreach (ElementId current4 in unusedImportCategories)
					{
						hashSet.Add(current4);
					}

					ICollection<ElementId> unusedStructures = GetUnusedStructures.Invoke(doc, null) as ICollection<ElementId>;
					foreach (ElementId current5 in unusedStructures)
					{
						hashSet.Add(current5);
					}

					ICollection<ElementId> unusedSymbols = GetUnusedSymbols.Invoke(doc, null) as ICollection<ElementId>;
					foreach (ElementId current6 in unusedSymbols)
					{
						hashSet.Add(current6);
					}

					var unusedThermals = GetUnusedThermals.Invoke(doc, null) as ICollection<ElementId>;
					foreach (ElementId current7 in unusedThermals)
					{
						hashSet.Add(current7);
					}

					ICollection<ElementId> nonDeletableUnusedElements = GetNonDeletableUnusedElements.Invoke(doc, null) as ICollection<ElementId>;
					foreach (ElementId current8 in nonDeletableUnusedElements)
					{
						hashSet.Remove(current8);
					}


					if (hashSet.Count != num && hashSet.Count != 0)
					{
						num = hashSet.Count;
						using (Transaction transaction = new Transaction(doc, "purge unused"))
						{
							transaction.Start();
							doc.Delete(hashSet);
							transaction.Commit();
							continue;
						}
					}
					break;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void Delete(Document doc, List<Element> list, string transactionName)
		{
			using (Transaction t = new Transaction(doc, transactionName))
			{
				t.Start();
				List<ElementId> ids = list.Select(x => x.Id).ToList();
				foreach (ElementId id in ids)
				{
					try
					{
						Element el = doc.GetElement(id);
						if (el.Pinned) el.Pinned = false;
					}
					catch (Exception ex)
					{
						throw ex;
					}
				}
				doc.Delete(ids);
				t.Commit();
			}
		}
		#endregion
	}
}
