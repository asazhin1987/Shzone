using System;
using System.Threading;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SharedZone.DTO.Infrastructure;
using SharedZone.RevitPlugin.Events;

namespace SharedZone.RevitPlugin.Commands
{
	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]
	public class TurnOnCommand : IExternalCommand
	{
		private ExportEvent export;
		private ExternalEvent exportEvent;


		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			try
			{
				export = new ExportEvent();
				exportEvent = ExternalEvent.Create(export);

				var src = SettingApplicationFactory.GetInstance();
				src.ResetSettings();
				src.ResetExportEvent(Settings_ExportEventHidden, Settings_ExportEventManual);
				src.RunConnector(commandData.Application.Application.VersionNumber);

				PanelFactory.Instance.TurnOnOffButton.ToggleButton_On();
				return Result.Succeeded;
			}
			catch (Exception ex)
			{
				TaskDialog.Show(Resources.Global.ApplicationName, ex.Message);
				return Result.Failed;
			}
		}

		private void Settings_ExportEventManual(object sender, SZExportEventArgs e)
		{
			if (exportEvent.IsPending)  
				TaskDialog.Show(Resources.Global.ApplicationName, Resources.Global.RevitBookedMsg);
			else
				Settings_ExportEvent(sender, e);
		}

		private void Settings_ExportEventHidden(object sender, SZExportEventArgs e)
		{
			if (exportEvent.IsPending)
				throw new Exception(Resources.Global.RevitBookedMsg);
			else
				Settings_ExportEvent(sender, e);
		}


		private void Settings_ExportEvent(object sender, SZExportEventArgs e)
		{
			try
			{
				export.Collection = e.Collection;
				export.Launch = e.JobLaunch;
				export.OnExecuted += Export_OnExecuted;
				export.OnExecuting += Export_OnExecuting;
				var request = exportEvent.Raise();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void Export_OnExecuting(object sender, SZExportResultEventArgs e)
		{
			var src = SettingApplicationFactory.GetInstance();
			src.SetExecuteStart(e.JobLaunch);
		}

		private void Export_OnExecuted(object sender, SZExportResultEventArgs e)
		{
			var src = SettingApplicationFactory.GetInstance();
			src.SetExecuteEnd(e.JobLaunch);
		}
	}


	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]
	public class TurnOffCommand : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			SettingApplicationFactory.GetInstance().DisposeConnector();
			PanelFactory.Instance.TurnOnOffButton.ToggleButton_Off();
			return Result.Succeeded;
		}
	}


	
}
