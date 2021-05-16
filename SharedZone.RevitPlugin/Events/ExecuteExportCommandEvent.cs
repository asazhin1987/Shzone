using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using System.Linq;
using SharedZone.DTO;
using SharedZone.DTO.Infrastructure;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.Attributes;
using System.IO;
//using SharedZone.DTO.Interfaces;

namespace SharedZone.RevitPlugin.Events
{

	public partial class ExportEvent : IExternalEventHandler
	{
		public CollectionDTO Collection { get; set; }
		public JobLaunchDTO Launch { get; set; }

		public event EventHandler<SZExportResultEventArgs> OnExecuted;
		public event EventHandler<SZExportResultEventArgs> OnExecuting;

		UIApplication app;

		public void Execute(UIApplication _app) 
		{
			app = _app;
			//TaskDialog.Show("SharedZone", Collection.ToString());
			Launch.StartDateTime = DateTime.Now;
			OnExecuting?.Invoke(this, new SZExportResultEventArgs(Launch));

			StringBuilder log = new StringBuilder();
			
			AppendLog($"Start Collection {Collection.Id}");
			app.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(Application_FailuresProcessing);
			app.DialogBoxShowing += new EventHandler<DialogBoxShowingEventArgs>(App_DialogBoxShowing);

			foreach (var model in Collection.RevitModelsDTO)
			{

				try
				{
					ModelPath modelPath = ModelPathUtils.ConvertUserVisiblePathToModelPath(model.Path);
					AppendLog($"Start File: {model.Path}");
					AppendLog($"Get Open Options");
					using (OpenOptions options = GetOpenOptions(model.Path))
					{
						Document doc = null;
						try
						{
							AppendLog($"Set WorksetConfiguration");
							using (WorksetConfiguration workset = GetWorksetConfiguration(modelPath))
							{
								options.SetOpenWorksetsConfiguration(workset);
							}
							AppendLog($"Begin Open File");
							doc = OpenDocument(modelPath, options);

							AppendLog($"End Open File");
							AppendLog($"Begin Navis Jobs. Count: {Collection.NavisJobsDTO.Count()}");
							EvecuteJobs(Collection.NavisJobsDTO, doc, model);
							AppendLog($"End Navis Jobs");
							AppendLog($"Begin IFC Jobs. Count: {Collection.IFCJobsDTO.Count()}");
							EvecuteJobs(Collection.IFCJobsDTO, doc, model);
							AppendLog($"End IFC Jobs");
							AppendLog($"Begin Revit Jobs. Count: {Collection.RevitJobsDTO.Count()}");
							EvecuteJobs(Collection.RevitJobsDTO, doc, model);
							AppendLog($"End Revit Jobs");

							if (!doc.IsDetached)
								Synchronization(doc);

						}
						catch (Exception ex)
						{
							throw ex;
						}
						finally
						{
							try
							{
								if (doc != null)
								{
									AppendLog($"Document begin close");
									doc?.Close(false);
									AppendLog($"Document closed");
								}
							}
							catch { }
						}
					}
				}
				catch (Exception ex)
				{
					log.Append($"EXCEPTION: {ex.Message}");
					Launch.Success = false;
					Launch.Message = log.ToString();
				}
				
			}
			app.Application.FailuresProcessing -= Application_FailuresProcessing;
			app.DialogBoxShowing -= App_DialogBoxShowing;

			Launch.EndDateTime = DateTime.Now;
			OnExecuted?.Invoke(this, new SZExportResultEventArgs(Launch));



			//=============================HELPERS
			void EvecuteJobs(IEnumerable<JobDTO> jobs, Document doc, RevitModelDTO file)
			{
				foreach (var job in jobs)
				{
					CheckAndCreateFolder(job.Path);
					try
					{
						//DeleteOldFile(Path.Combine(job.Path, file.Name));
						if (job is RevitJobDTO rj)
							Export(doc, rj, file.Name);
						else if (job is NavisJobDTO nj)
							Export(doc, nj, file.Name);
						else if (job is IFCJobDTO ij)
							Export(doc, ij, file.Name);
						else
							throw new Exception($"Wrong Type IJobDTO: {job.GetType().Name}");
					}
					catch (Exception ex)
					{
						log.Append($"EXCEPTION: {ex.Message}");
						Launch.Success = false;
						continue;
					}
				}
			}

			void AppendLog(string msg)
			{
				log.Append($"\r\n{DateTime.Now}\t{msg}");
			}
		}

		private void Synchronization(Document doc)
		{
			SynchronizeWithCentralOptions o = new SynchronizeWithCentralOptions();
			o.SetRelinquishOptions(new RelinquishOptions(true)
			{
				StandardWorksets = true,
				ViewWorksets = true,
				FamilyWorksets = true,
				CheckedOutElements = true,
				UserWorksets = true
			});
			TransactWithCentralOptions to = new TransactWithCentralOptions();
			doc.SynchronizeWithCentral(to, o);
		}

		private OpenOptions GetOpenOptions(string VisiblePath)
		{
			OpenOptions options = new OpenOptions();

			if (IsWorkshared())
			{
				options.DetachFromCentralOption = Collection.Detach ?
					DetachFromCentralOption.DetachAndPreserveWorksets :
					DetachFromCentralOption.DoNotDetach;
				options.Audit = Collection.Audit;
				options.AllowOpeningLocalByWrongUser = true;
			}

			bool IsWorkshared()
			{

				if (VisiblePath.Contains("RSN://"))
					return true;
				BasicFileInfo basicFileInfo = BasicFileInfo.Extract(VisiblePath);
				return basicFileInfo.IsWorkshared;
			}

			return options;
		}

		public void CheckAndCreateFolder(string ExportFolder)
		{
			if (!Directory.Exists(ExportFolder))
				Directory.CreateDirectory(ExportFolder);
		}

		public void DeleteOldFile(string file)
		{
			if (File.Exists(file))
				File.Delete(file);
		}

		//public void CreateFolderAndDeleteFile(string ExportFolder, string file)
		//{
		//	try
		//	{
		//		if (!Directory.Exists(ExportFolder))
		//		{
		//			Directory.CreateDirectory(ExportFolder);
		//		}
		//		if (File.Exists(file))
		//		{
		//			File.Delete(file);
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		throw ex;
		//	}
		//}


		private void Application_FailuresProcessing(object sender, FailuresProcessingEventArgs e)
		{
			try
			{
				FailuresAccessor fA = e.GetFailuresAccessor();
				IList<FailureMessageAccessor> fmas = fA.GetFailureMessages();
				if (fmas.Count == 0)
				{
					e.SetProcessingResult(FailureProcessingResult.Continue);
				}
				else
				{
					try
					{
						fA.DeleteAllWarnings();
					}
					catch (Exception ex)
					{

					}
					finally
					{
						e.SetProcessingResult(FailureProcessingResult.ProceedWithCommit);
					}
					
					//foreach (FailureMessageAccessor fma in fmas)
					//{
					//	try
					//	{
					//		fA.DeleteWarning(fma);
					//	}
					//	catch (Exception ex)
					//	{
					//		fA.ResolveFailure(fma);
					//	}
					//	finally
					//	{
					//		e.SetProcessingResult(FailureProcessingResult.Continue);
					//	}
					//}
				}
			}
			catch (Exception ex)
			{

			}
		}

		private void App_DialogBoxShowing(object sender, DialogBoxShowingEventArgs e)
		{
			if (!e.OverrideResult(1))
			{
				if (!e.OverrideResult(5))
				{
					e.OverrideResult(2);
				}
			};
		}

		private Document OpenDocument(ModelPath path, OpenOptions options)
		{
			try
			{
				return app.Application.OpenDocumentFile(path, options);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			
		}

		private WorksetConfiguration GetWorksetConfiguration(ModelPath path)
		{
			WorksetConfiguration workset = new WorksetConfiguration(Collection.Worksets ?
					WorksetConfigurationOption.CloseAllWorksets :
					WorksetConfigurationOption.OpenAllWorksets);
		
			try
			{
				IEnumerable<WorksetPreview> list = WorksharingUtils.GetUserWorksetInfo(path);
				IEnumerable<string> filters = Collection.ExceptionWorksets.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
				IList<WorksetId> worksetIds = list.Where(x => filters.Contains(x.Name)).Select(x => x.Id).ToList();
				if (worksetIds.Count() > 0)
				{
					if (Collection.Worksets)
						workset.Open(worksetIds);
					else
						workset.Close(worksetIds);
				}
			}
			catch(Exception ex)
			{
				if (Collection.Worksets)
					throw ex;
			}
			

			return workset;
		}

		


		//private void AppendLog(string msg)
		//{
		//	log.Append($"\r{DateTime.Now}\t{msg}");
		//}

		public string GetName() => 
			GetType().Name;
	}
}
