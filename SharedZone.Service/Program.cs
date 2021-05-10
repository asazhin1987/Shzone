using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Runtime.InteropServices;
using SharedZone.BLL.Infrastructure;
using System.Threading;

namespace SharedZone.Service
{
	public class Program
	{
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();

		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		const int SW_HIDE = 0;


		static async Task Main(string[] args)
		{
			ServiceFactory service = new ServiceFactory(
				ConfigurationManager.ConnectionStrings["SharedZoneConnection"].ConnectionString, 
				ConfigurationManager.ConnectionStrings["SharedZoneConnection"].ProviderName);

			service.OnServiceRuning += ServiceEventLog;
			service.OnServiceRunned += ServiceEventLog;
			service.OnServicesDisposed += ServiceEventLog;
			service.OnServiceDisposed += ServiceEventLog;
			service.OnBeginMigrate += ServiceEventLog;
			service.OnEndMigrate += ServiceEventLog;
			service.OnFailureMigrate += OnServiceFailureLog;
			service.OnServiceFailure += OnServiceFailureLog;
			service.OnServicesFailure += OnServiceFailureLog;
			service.OnScheduleRunned += ServiceEventLog;
			service.OnScheduleFailure += OnServiceFailureLog;

			service.Migrate();
			//Thread.Sleep(10000);
			service.RunServices();
			//Thread.Sleep(500);
			await service.RunSchedule();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Press any key to hidde console");
			Console.ReadLine();
			var handle = GetConsoleWindow();
			ShowWindow(handle, SW_HIDE);
			Console.ReadLine();
			service.Dispose();

			Console.ReadLine();
		}


		private static void ServiceEventLog(object sender, ServiceEventArgs e)
		{
			Console.ForegroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(e.Message);
		}

		private static void OnServiceFailureLog(object sender, ServiceEventArgs e)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(e.Message);
		}
	}
}
