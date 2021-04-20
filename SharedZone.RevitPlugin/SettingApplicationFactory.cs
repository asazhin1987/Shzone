using System;
using SharedZone.Client.Applications;
using SharedZone.DTO.Infrastructure;
using SharedZone.DTO;

namespace SharedZone.RevitPlugin
{
	/// <summary>
	/// Мост между формой настройки и Revit
	/// </summary>
	internal class SettingApplicationFactory
	{
		private static readonly Lazy<SettingApplicationFactory> lazy =
			new Lazy<SettingApplicationFactory>(() => new SettingApplicationFactory());

		private SettingApplication Settings { get; set; }


		private SettingApplicationFactory()
		{
			Settings = new SettingApplication();
		}

		internal static SettingApplicationFactory GetInstance()
		{
			return lazy.Value;
		}

		internal void ResetSettings()
		{
			DisposeConnector();
			Settings = new SettingApplication();
		}

		internal void DisposeConnector()
		{
			Settings.CloseSettingsWindow();
			Settings.DisposeService();
		}

		internal void RunConnector(string version)
		{
			Settings.RunService(version);
		}

		internal void ResetExportEvent(EventHandler<SZExportEventArgs> handler, EventHandler<SZExportEventArgs> handlerManual)
		{
			Settings.ExportEvent += handler;
			Settings.ExportEventManual += handlerManual;
		}

		internal void ShowSettingsWindows()
		{
			Settings.ShowSettingsWindow();
		}

		internal void SetExecuteStart(JobLaunchDTO job)
		{
			Settings.ExportExecuteStart(job);
		}

		internal void SetExecuteEnd(JobLaunchDTO job)
		{
			Settings.ExportExecuteEnd(job);
		}
	}
}
