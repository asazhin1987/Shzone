using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedZone.DTO;

namespace SharedZone.DTO.Infrastructure
{
	public class SZEventArgs : EventArgs
	{

	}

	public class SZExportEventArgs : SZEventArgs
	{
		public CollectionDTO Collection { get; set; }
		public JobLaunchDTO JobLaunch { get; set; }

		public SZExportEventArgs(CollectionDTO collection, JobLaunchDTO launch)
		{
			Collection = collection;
			JobLaunch = launch;
		}
	}

	public class SZExportResultEventArgs : SZEventArgs
	{
		public JobLaunchDTO JobLaunch { get; set; }

		public SZExportResultEventArgs(JobLaunchDTO job)
		{
			JobLaunch = job;
		}
	}

	public class SZExceptionEventArgs : SZEventArgs
	{
		public Exception Exception { get; set; }
		public BimacadUnitDTO BimacadUnitDTO { get; set; }

		public SZExceptionEventArgs(Exception ex, BimacadUnitDTO unitDTO)
		{
			Exception = ex;
			BimacadUnitDTO = unitDTO;
		}
	}

	
}
