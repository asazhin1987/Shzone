using SharedZone.DTO;
using System;
using System.Globalization;
using Bimacad.Sys;
using System.Collections.Generic;

namespace SharedZone.Web.Models
{
	/// <summary>
	/// Модель представления "день недели"
	/// </summary>
	public class WeekDayViewModel : NamedUnitViewModel
	{
		/// <summary>
		/// Номер дня в неделе
		/// </summary>
		public int DayNum { get; set; }

		/// <summary>
		/// Отображаемое длинное имя в соответствии с культурой
		/// </summary>
		public string LongName { get; set; }

		/// <summary>
		/// Отображаемое короткое имя в соответствии с культурой
		/// </summary>
		public string ShortName { get; set; }

		/// <summary>
		/// День недели
		/// </summary>
		public DayOfWeek DayOfWeek { get; set; }

		/// <summary>
		/// Признак сегодня
		/// </summary>
		public bool IsToday { get; set; }

		/// <summary>
		/// Признак прошлого
		/// </summary>
		public bool IsPast { get; set; }

		/// <summary>
		/// Дата
		/// </summary>
		public DateTime Date { get; set; }

		public WeekDayViewModel(WeekDayDTO wd) : this(wd.Id){}

		public WeekDayViewModel(int wdId)
		{
			CultureInfo culture = CultureInfo.CurrentCulture;
			int nowId = DateTime.Now.GetWeekDayId();
			Id = wdId;

			IsToday = Id == nowId;
			IsPast = Id < nowId;
			DayNum = wdId.GetWeekNumId();
			DayOfWeek = (DayOfWeek)DayNum;
			ShortName = culture.DateTimeFormat.GetShortestDayName(DayOfWeek);
			LongName = culture.DateTimeFormat.GetDayName(DayOfWeek);
			Name = ShortName;
			Date = DateTime.Now.AddDays(wdId - nowId);
		}

		public WeekDayViewModel(DateTime date) : this(date.GetWeekDayId())
		{
			Date = date;
		}

		public override string ToString()
		{
			return Id.ToString();
		}
		public override bool Equals(object obj)
		{
			if (obj is WeekDayViewModel _obj)
				return _obj.ToString() == ToString();
			return false;
		}

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

	}

	//public class WeekDayLaunchViewModel : WeekDayViewModel
	//{
	//	//public IEnumerable<JobLaunchDTO> Lauches { get; set; }

	//	public WeekDayLaunchViewModel(DateTime date) : base(date.GetWeekDayId())
	//	{
	//		Date = date;
	//		//Lauches = new List<JobLaunchDTO>();
	//	}
	//}
}