using Quartz;
using System;
using System.Threading.Tasks;
using SharedZone.ISysService;
using Bimacad.Sys;

namespace SharedZone.BLL.Schedules
{
	[DisallowConcurrentExecution]
	public class ServerDirectoryExplorer : IJob
	{

		public async Task Execute(IJobExecutionContext context)
		{
			try
			{
				JobDataMap dataMap = context.JobDetail.JobDataMap;
				ISysSrc src = (ISysSrc)dataMap["src"];
				lock (src)
				{
					Task t = Task.Run(() => src.UpdateDirectoriesAsync());
					t.Wait();
					Task t2 = Task.Run(() => src.UpdateRevitServersAsync());
					t2.Wait();
				}

				//await src.UpdateDirectories();
				//await src.UpdateRevitServers();
			}
			catch (Exception ex)
			{
				BTextWriter.WriteLog($"DirectoryExplorer EXCEPTION: {ex.Message}");
			}
		}
	}
}
