using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using SharedZone.Client.Commands;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using SharedZone.DTO;
using SharedZone.DTO.Infrastructure;
using SharedZone.Client.Xaml;
using SharedZone.IRevitService;
using SharedZone.RevitSRC;
using winapp = System.Windows;
using Bimacad.Sys;
using System.Threading;
using System.Linq;

namespace SharedZone.Client.Applications
{
	/// <summary>
	/// Настройка
	/// </summary>s
	public class SettingApplication : INotifyPropertyChanged 
	{
		/// <summary>
		/// Событие экспорта
		/// </summary>
		public event EventHandler<SZExportEventArgs> ExportEvent;

		/// <summary>
		/// Событие ручного запуска экспорта
		/// </summary>
		public event EventHandler<SZExportEventArgs> ExportEventManual;

		/// <summary>
		/// События выброса исключения
		/// </summary>
		private event EventHandler<SZExceptionEventArgs> OnExceptionThrown;
		/// <summary>
		/// События обновления списка коллекций
		/// </summary>
		private event EventHandler<SZEventArgs> OnUpdateListTiming;
		/// <summary>
		/// Событие проверки расписания
		/// </summary>
		private event EventHandler<SZEventArgs> OnCheckScheduleTiming;

		

		/// <summary>
		/// Форма настройки 
		/// </summary>
		private SettingsForm SettingsForm { get; set; }

		private Thread UpdateThread { get; set; }

		private IRevitSrc src;
		/// <summary>
		/// Сервис взаимодействия с бд
		/// </summary>
		private IRevitSrc Src
		{
			get {
				if (src == null)
					src = new RevitService<IRevitSrc>(Host);
				return src;
			}
		}

		

		/// <summary>
		/// Версия
		/// </summary>
		public string Version { get; set; }

		private string _host;
		/// <summary>
		/// Адрес подключения
		/// </summary>
		public string Host { 
			get { return _host ??  GetHostName(); }
			set { _host = value; }
		}


		public SettingApplication()
		{
			ClientName = Environment.MachineName;
			UserName = Environment.UserName;

			OnCheckScheduleTiming += SettingApplication_OnCheckScheduleTiming;
			OnUpdateListTiming += SettingApplication_OnUpdateListTiming;
		}

		internal bool CheckLicense()
		{
			try
			{
				lock (Src)
				{
					return Src.TakeLicense(Environment.MachineName, Environment.UserName);
				}
				
			}
			catch (WrongKeyException)
			{
				Inform(Resources.Global.LicenseWrongKeyMsg);
				return false;
			}
			catch (NullKeyException)
			{
				Inform(Resources.Global.LicenseNullKeyMsg);
				return false;
			}
			catch (ZeroQntException)
			{
				Inform(Resources.Global.LicenseZeroQnt);
				return false;
			}
			catch (Exception ex)
			{
				Inform(ex.Message);
				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="version"></param>
		public void RunService(string version)
		{
			if (CheckLicense())
			{
				Version = version;
				ApplicationIsActive = true;
				UpdateThread = new Thread(UpdateData);
				UpdateThread.Start();
			}
		}

		public void DisposeService()
		{
			ApplicationIsActive = false;
			if (UpdateThread != null)
			{
				UpdateThread.Join();
				UpdateThread.Abort();
			}
		}

		

		private void SettingApplication_OnCheckScheduleTiming(object sender, SZEventArgs e)
		{

			var schedule = GetSchedule();
			if (schedule.Count() > 0)
				RunExport(schedule.First());
			//foreach (var sch in schedule)
			//	RunExport(sch);
		}

		private void SettingApplication_OnUpdateListTiming(object sender, SZEventArgs e)
		{
			var items = GetCollections();
			Profiles = new ObservableCollection<CollectionDTO>(items);
			OnPropertyChanged(nameof(Profiles));
			ConnectMessage = Resources.Global.ConnectionSuccess;
		}

		private void SettingApplication_OnExceptionThrown(object sender, SZExceptionEventArgs e)
		{
			WriteLog(e.Exception, e.BimacadUnitDTO);
		}

		private void WriteLog(Exception exception, BimacadUnitDTO item)
		{
			BTextWriter.WriteLog($"{item.ToString()}. {exception.Message}");
		}


		/// <summary>
		/// Признак установленного подключения подключения
		/// </summary>
		public bool Connected { get; set; }

		/// <summary>
		/// Имя ПК
		/// </summary>
		public string ClientName { get; private set; }

		/// <summary>
		/// Имя пользователя
		/// </summary>
		public string UserName { get;  private set; } 

		private string connectMessage;
		/// <summary>
		/// Сообщение подключения
		/// </summary>
		public string ConnectMessage { get { return connectMessage; } set { connectMessage = value; OnPropertyChanged(); } }
		private string testMessage;

		/// <summary>
		/// Тест
		/// </summary>
		public string TestMessage { get { return testMessage; } set { testMessage = value; OnPropertyChanged(); } }
		
		/// <summary>
		/// Признак активной выгрузки
		/// </summary>
		public bool ExportInDeed { get; set; }


		/// <summary>
		/// Признак включенного плагина
		/// </summary>
		public bool ApplicationIsActive { get; set; }

		/// <summary>
		/// Коллекция профилей
		/// </summary>
		public ObservableCollection<CollectionDTO> Profiles { get; set; }
		private CollectionDTO activeProfile;

		/// <summary>
		/// Активный профиль
		/// </summary>
		public CollectionDTO ActiveProfile { get { return activeProfile; } set { activeProfile = value; OnPropertyChanged(); } }

		private RelayCommand seveHostCommand;
		private RelayCommand checkHostCommand;
		private RelayCommand startCommand;

		/// <summary>
		/// Сохранить строку подключения
		/// </summary>
		public RelayCommand SeveHostCommand
		{
			get
			{
				return seveHostCommand ?? (seveHostCommand = new RelayCommand(obj =>
				{
					SaveHost();
				}));
			}
		}

		/// <summary>
		/// Запустить профиль
		/// </summary>
		public RelayCommand StartCommand
		{
			get
			{
				return startCommand ??
					(startCommand = new RelayCommand(obj =>
					{
						RunManualExport(activeProfile);
					}));
			}
		}

		/// <summary>
		/// Проверить строку подключения
		/// </summary>
		public RelayCommand CheckHostCommand
		{
			get
			{
				return checkHostCommand ??
					(checkHostCommand = new RelayCommand(obj =>
					{
						CheckHost();
					}));
			}
		}


		private void RunManualExport(CollectionDTO col)
		{
			try
			{
				CollectionDTO colFullData;
				lock (Src)
				{
					colFullData = Src.GetCollection(col.Id);
				}
				if (colFullData != null)
				{
					//execute
					ExportEventManual?.Invoke(this, new SZExportEventArgs(colFullData, new JobLaunchDTO()));
				}
			}
			catch (Exception ex)
			{
				OnExceptionThrown?.Invoke(this, new SZExceptionEventArgs(ex, col));
			}
}

		private void RunExport(CollectionDTO col)
		{
			try
			{
				CollectionDTO colFullData;
				lock (Src)
				{
					colFullData = Src.GetCollection(col.Id);
				}
				if (colFullData != null)
				{
					JobLaunchDTO launch = new JobLaunchDTO()
					{
						ClientId = col.ClientId,
						CollectionId = col.Id,
						WeekDayId = DateTime.Now.GetWeekDayId(),
						MinuteId = col.MinuteId,
						HourId = col.HourId,
						Odate = DateTime.Now
					};
					//старт лог
					int Id = 0;
					lock (Src)
					{
						Id = Src.CreateJobLaunch(launch);
					}

					if (Id > 0)
					{
						launch.Id = Id;
						//execute
						ExportEvent?.Invoke(this, new SZExportEventArgs(colFullData, launch));
						launch.Success = true;
					}
				}
			}
			catch (Exception ex)
			{
				OnExceptionThrown?.Invoke(this, new SZExceptionEventArgs(ex, col));
			}
		}

		private void CheckHost()
		{
			try
			{
				string _host = ClearStrinng(Host);
				var _src = new RevitService<IRevitSrc>(_host);

				if (_src.CheckConnection())
					Inform(Resources.Global.ConnectionSuccess);
			}
			catch
			{
				throw new Exception(Resources.Global.ConnectionError);
			}
		}

		
		//private bool dispose;
		private void UpdateData()
		{
			while (ApplicationIsActive)
			{
				try
				{
					TestMessage = $"{Src.CheckConnection()} - {DateTime.Now.ToLongTimeString()}";
					if (!ExportInDeed)
					{
						OnCheckScheduleTiming?.Invoke(this, new SZEventArgs());
						OnUpdateListTiming?.Invoke(this, new SZEventArgs());
					}
				}
				catch (Exception ex)
				{
					ConnectMessage = ex.Message;
				}
				Thread.Sleep(2000);
			}
		}

		private IEnumerable<CollectionDTO> GetSchedule()
		{
			lock (Src)
			{
				return Src.GetSchedule(ClientName, Version);
			}
		}

		private IEnumerable<CollectionDTO> GetCollections()
		{
			lock (Src)
			{
				return Src.GetCollections(ClientName, Version);
			}
		}

		public void ExportExecuteStart(JobLaunchDTO job)
		{
			ExportInDeed = true;
			if (job.Id > 0)
				lock (Src)
				{
					Src.SetStartResult(job);
				}
		}


		public void ExportExecuteEnd(JobLaunchDTO job)
		{
			if (job.Id > 0)
				lock (Src)
				{
					Src.SetEndResult(job);
				}
			ExportInDeed = false;
		}

		/// <summary>
		/// Закрытие окна настройки
		/// </summary>
		public void CloseSettingsWindow()
		{
			if (SettingsForm != null)
				SettingsForm.Close();
		}

		/// <summary>
		/// Запуск окна настройки
		/// </summary>
		public void ShowSettingsWindow()
		{
			if (SettingsForm == null)
			{
				SettingsForm = new SettingsForm(this);
				SettingsForm.Closed += delegate (object o, EventArgs e) { SettingsForm = null; };
				
				SettingsForm.Show();
			}
			else
			{
				SettingsForm.WindowState = winapp.WindowState.Normal;
				SettingsForm.Activate();
			}
		}

		//public void WriteExportStartLog(JobLaunchDTO launchDTO)
		//{

		//}

		///// <summary>
		///// Запись результатов выгрузки
		///// </summary>
		//public void WriteExportResult(JobLaunchDTO launchDTO)
		//{
		//	try
		//	{
		//		Src.SetResult(launchDTO);
		//	}
		//	catch(Exception ex)
		//	{
		//		OnExceptionThrown?.Invoke(this, new SZExceptionEventArgs(ex, launchDTO));
		//	}
			
		//}

		private void SaveHost()
		{
			try
			{
				BTextWriter.WriteCurrentFile(ClearStrinng(Host), "host.txt");
				Inform(Resources.Global.SuccessMsg);
			}

			catch (Exception ex)
			{
				Inform(ex.Message);
			}
		}

		private string GetHostName() =>
			ClearStrinng(BTextWriter.ReadCurrentFile("host.txt"));

		internal string ClearStrinng(string _str) =>
		_str.Replace("\r", "").Replace("\n", "").Trim();

		internal void Inform(string msg)
		{
			winapp.MessageBox.Show(msg);
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
