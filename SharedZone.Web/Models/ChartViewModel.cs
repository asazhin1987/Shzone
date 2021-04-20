using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharedZone.Web.Models
{
	public class ChartViewModel
	{
		public IEnumerable<WeekDayViewModel> WeekDays { get; set; }
	}
}