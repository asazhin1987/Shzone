
using System;
using SharedZone.DTO;
using SharedZone.DTO.Infrastructure;
using System.Windows;
using SharedZone.Client.Applications;
using Bimacad.Sys;

namespace SharedZone.Win.Test
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		SettingApplicationFactory client;
		public MainWindow()
		{
			InitializeComponent();
			
			//client = SettingApplication.CreateInstance("2020");
			//client.ExportEvent += Client_ExportEvent;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			client = SettingApplicationFactory.GetInstance();
			client.RunConnector("2019");
			client.ShowSettingsWindows();

		}

		private void Client_ExportEvent(object sender, SZExportEventArgs e)
		{
			//var col = e.Collection;
			//JobLaunchDTO launch = new JobLaunchDTO()
			//{
			//	ClientId = col.ClientId,
			//	CollectionId = col.Id,
			//	WeekDayId = DateTime.Now.GetWeekDayId(),
			//	MinuteId = col.MinuteId,
			//	HourId = col.HourId
			//};

			try
			{
				//выполнение экспорта
				MessageBox.Show(e.Collection.Name);
				//launch.Success = true;

			}
			catch (Exception ex)
			{
				throw ex;
				//launch.Success = false;
				//launch.Message = ex.Message;
			}
			//launch.EndDateTime = DateTime.Now;
			//client.WriteExportResult(launch);
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{


		}

		//private void Export(object sender, SZExportEventArgs e)
		//{
		//	var coll = e.Collection;
		//	MessageBox.Show(coll.Name);
		//}
	}

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
