using Quartz;
using System;
using System.Threading.Tasks;
using SharedZone.ISysService;
using Bimacad.Sys;

namespace SharedZone.BLL.Schedules
{
	[DisallowConcurrentExecution]
	public class CleanerLog : IJob
	{
		public async Task Execute(IJobExecutionContext context)
		{
			try
			{
				JobDataMap dataMap = context.JobDetail.JobDataMap;
				ISysSrc src = (ISysSrc)dataMap["src"];
				lock (src)
				{
					Task t = Task.Run(() => src.ClearJobLogAsync(90));
					t.Wait();
					Task t2 = Task.Run(() => src.ClearServerLogAsync(90));
					t2.Wait();
				}
			}
			catch (Exception ex)
			{
				BTextWriter.WriteLog($"DirectoryExplorer EXCEPTION: {ex.Message}");
			}
		}
	}
}
