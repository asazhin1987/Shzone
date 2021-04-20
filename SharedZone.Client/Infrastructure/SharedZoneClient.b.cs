//using Bimacad.Sys;
//using SharedZone.Client.Applications;
//using SharedZone.Client.Xaml;
//using SharedZone.DTO.Infrastructure;
//using SharedZone.IRevitService;
//using SharedZone.RevitSRC;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Threading;
//using System.Collections.ObjectModel;
//using SharedZone.DTO;
//using System.Reflection;
////using winapp = System.Windows;

//namespace SharedZone.Client.Infrastructure
//{
//	/// <summary>
//	/// 
//	/// </summary>
//	public class SharedZoneClient: IDisposable
//	{

//		public event EventHandler<SZExportEventArgs> ExportEvent;

//		private IRevitSrc _src;
//		private IRevitSrc src;
//		//private IRevitService src { 
//		//	get { 
//		//		if (_src == null) 
//		//			_src = new RevitSRC(; }
//		private string HostName { get { return GetHostName(); } }
//		private string ClientName { get; set; }
//		private SettingApplication app;

//		/// <summary>
//		/// Форма взаимодействия с приложением
//		/// </summary>
//		private SettingsForm SettingsForm { get; set; }
//		private Thread updateThread;
//		private bool disposed = false;

//		private static readonly Lazy<SharedZoneClient> lazy =
//			new Lazy<SharedZoneClient>(() => new SharedZoneClient());

//		private SharedZoneClient()
//		{
//			InitializeComponent();
//		}

//		public static SharedZoneClient ServiceInstance()
//		{
//			return lazy.Value;
//		}

//		public void ShowDialog()
//		{
//			if (SettingsForm == null)
//			{
//				SettingsForm = new SettingsForm(app);
//				SettingsForm.Closed += delegate (object o, EventArgs e) { SettingsForm = null; };
//				SettingsForm.Show();
//			}
//			else
//			{
//				SettingsForm.WindowState = System.Windows.WindowState.Normal;
//				SettingsForm.Activate();
//			}
			
//		}

//		private void InitializeComponent()
//		{
//			app = new SettingApplication();
//			app.CheckHost += SettingModel_CheckHost;
//			app.SaveHost += SettingModel_SaveHost;
//			app.StartProfile += App_StartProfile;
//			app.Host = HostName;
//			app.ExportIsActive = true;
//			ClientName = Environment.MachineName;
//			//src.TakeLicense(ClientName, Environment.UserName);
//			updateThread = new Thread(UpdateData);
//			updateThread.Start();
//		}

//		private void App_StartProfile(object sender, SZExportEventArgs e)
//		{
//			ExportEvent?.Invoke(this, e);
//		}

//		private void UpdateData()
//		{
//			while (disposed == false)
//			{
//				try
//				{
//					if (src == null)
//						SettingModel_CheckHost(this, null);
//					app.TestMessage = $"{src.CheckConnection()} - {DateTime.Now.ToLongTimeString()}";
//					src.TakeLicense(ClientName, Environment.UserName);
//					//check schedule

//					//run job

//					//write log

//					//update ui
//					var items = src.GetSchedule(ClientName);
//					app.SetProfiles(items);

//				}
//				catch (Exception ex)
//				{
//					Inform(ex.Message);
//				}
//				Thread.Sleep(1000);
//			}
//		}

//		public void Dispose()
//		{
//			disposed = true;
//			updateThread.Join();
//		}




//		private string GetHostName() =>
//			ClaerStrinng(BTextWriter.ReadCurrentFile("host.txt"));

//		private string ClaerStrinng(string _str) =>
//		_str.Replace("\r", "").Replace("\n", "").Trim();

//		private void Inform(string msg)
//		{
//			app.ConnectMessage = msg;
//			//winapp.MessageBox.Show(msg);
//		}

//		private void SettingModel_CheckHost(object sender, EventArgs e)
//		{
//			try
//			{
//				string _host = ClaerStrinng(app.Host);
//				var _src = new RevitService<IRevitSrc>(_host);

//				if (_src.CheckConnection())
//				{
//					Inform(Resources.Global.ConnectionSuccess);
//					if (src != null)
//					{
//						lock (src)
//						{
//							src = _src;
//						}
//					}
//					else
//					{
//						src = _src;
//					}
					
//				}
//			}
//			catch
//			{
//				throw new Exception(Resources.Global.ConnectionError);
//			}
//		}

//		private void SettingModel_SaveHost(object sender, EventArgs e)
//		{
//			try
//			{
//				BTextWriter.WriteCurrentFile(ClaerStrinng(app.Host), "host.txt");
//				Inform(Resources.Global.SuccessMsg);
//				SettingModel_CheckHost(sender, e);
//			}

//			catch (Exception ex)
//			{
//				Inform(ex.Message);
//			}
//		}

		

//		//private bool CheckLicense()
//		//{
//		//	try
//		//	{
//		//		return src.TakeLicense(Environment.MachineName, Environment.UserName);
//		//	}
//		//	catch (WrongKeyException)
//		//	{
//		//		Inform(Resources.Global.LicenseWrongKeyMsg);
//		//		return false;
//		//	}
//		//	catch (NullKeyException)
//		//	{
//		//		Inform(Resources.Global.LicenseNullKeyMsg);
//		//		return false;
//		//	}
//		//	catch (ZeroQntException)
//		//	{
//		//		Inform(Resources.Global.LicenseZeroQnt);
//		//		return false;
//		//	}
//		//	catch (Exception ex)
//		//	{
//		//		Inform(ex.Message);
//		//		return false;
//		//	}
//		//}

//		//public void Create()
//		//{
//		//try
//		//{

//		//	settingModel = new SettingApplication()
//		//	{
//		//		Host = HostName
//		//	};
//		//	settingModel.SaveHost += SettingModel_SaveHost;
//		//	settingModel.CheckHost += SettingModel_CheckHost;
//		//	SettingsForm sf = new SettingsForm(settingModel);

//		//	sf.ShowDialog();
//		//}
//		//catch (Exception ex)
//		//{
//		//	Inform(ex.Message);
//		//}
//		//}

//		//public void Dispose()
//		//{

//		//}
//	}
//}
