//using Bimacad.Sys;
//using SharedZone.IRevitService;
//using SharedZone.RevitSRC;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using winapp = System.Windows;

//namespace SharedZone.Client.Infrastructure
//{
//	public abstract class Client
//	{
//		internal IRevitSrc Src { get; }
//		internal string HostName { get { return GetHostName(); } }
//		internal bool IsLicensed { get; }
//		public Client()
//		{

//		}

//		public Client(bool checkConnection, bool checklic)
//		{
//			//if (checkConnection)
//			//{
//			//	try
//			//	{
//			//		Src = new RevitService<IRevitSrc>(HostName);
//			//		bool b = Src.CheckConnection();
//			//	}
//			//	catch (Exception)
//			//	{
//			//		new SettingFactory().Create();
//			//	}
//			//}
//			//if (checklic)
//			//	IsLicensed = CheckLicense();
//		}


//		private string GetHostName() =>
//			ClaerStrinng(BTextWriter.ReadCurrentFile("host.txt"));

//		internal string ClaerStrinng(string _str) =>
//		_str.Replace("\r", "").Replace("\n", "").Trim();

//		internal void Inform(string msg)
//		{
//			winapp.MessageBox.Show(msg);
//		}


//		internal bool CheckLicense()
//		{
//			try
//			{
//				return Src.TakeLicense(Environment.MachineName, Environment.UserName);
//			}
//			catch (WrongKeyException)
//			{
//				Inform(Resources.Global.LicenseWrongKeyMsg);
//				return false;
//			}
//			catch (NullKeyException)
//			{
//				Inform(Resources.Global.LicenseNullKeyMsg);
//				return false;
//			}
//			catch (ZeroQntException)
//			{
//				Inform(Resources.Global.LicenseZeroQnt);
//				return false;
//			}
//			catch (Exception ex)
//			{
//				Inform(ex.Message);
//				return false;
//			}
//		}
//	}
//}
