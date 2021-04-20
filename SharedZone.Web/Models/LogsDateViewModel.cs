using System;
using System.Collections.Generic;
using System.Linq;
using SharedZone.DTO;

namespace SharedZone.Web.Models
{
	public class LogsDateViewModel
	{
		public IEnumerable<IGrouping<WeekDayViewModel, JobLaunchDTO>> Data { get; set; }

		public LogsDateViewModel(IEnumerable<JobLaunchDTO> data)
		{
			Data = data.GroupBy(g => new WeekDayViewModel(g.Odate.Date)).OrderBy(o => o.Key.Date) ;
		}
	}
}