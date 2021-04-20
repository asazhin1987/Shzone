using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SharedZone.DTO;

namespace SharedZone.Web.Models
{
	/// <summary>
	/// Представление календаря
	/// </summary>
	public class CollectionCalendarViewModel
	{
		public IEnumerable<HourViewModel> Hours { get; set; }
		public IEnumerable<WeekDayViewModel> WeekDays { get; set; }
		public IEnumerable<CollectionDTO> Collections { get; set; }
		public IEnumerable<MinuteViewModel> Minutes { get; set; }

		public CollectionCalendarViewModel() { }

		
	}

	

	
}