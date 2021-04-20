//using Quartz;
//using System;
//using System.Threading.Tasks;
//using SharedZone.ISysService;
//using Bimacad.Sys;


//namespace SharedZone.BLL.Schedules
//{
//	[DisallowConcurrentExecution]
//	public class RevitServerExplorer : IJob
//	{
//		public async Task Execute(IJobExecutionContext context)
//		{
//			try
//			{
//				JobDataMap dataMap = context.JobDetail.JobDataMap;
//				ISysSrc src = (ISysSrc)dataMap["src"];
//				await src.UpdateRevitServers();
//			}
//			catch (Exception ex)
//			{
//				BTextWriter.WriteLog($"RevitServerExplorer EXCEPTION: {ex.Message}");
//			}
//		}
//	}
//}
