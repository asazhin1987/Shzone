using SharedZone.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bimacad.Sys;
using System.Globalization;
using SharedZone.Web.Models;

namespace SharedZone.Web.Util
{
	public static class HelperModule
	{
		public static string GetWdStrings(IEnumerable<WeekDayDTO> wds)
		{
			if (wds == null)
				return string.Empty;
			StringBuilder result = new StringBuilder();
			
			byte b = 0;
			foreach (var wd in wds)
			{
				b++;
				result.Append(new WeekDayViewModel(wd).ShortName);
				if(b != wds.Count())
					result.Append(", ");
			}

			return result.ToString();
		}

		public static string GetWdShortName(WeekDayDTO wd) =>
			CultureInfo.CurrentCulture.DateTimeFormat.GetShortestDayName((DayOfWeek)wd.Id.GetWeekNumId());

		public static string GetWdLongName(WeekDayDTO wd) =>
			CultureInfo.CurrentCulture.DateTimeFormat.GetDayName((DayOfWeek)wd.Id.GetWeekNumId());

		public static string GetCollectionBgClass(CollectionDTO item, WeekDayViewModel day, HourViewModel hour)
		{
			StringBuilder result = new StringBuilder();
			//if (item.LastJobLaunch != null)
			//{
			//	if (item.LastJobLaunch.Success == false)
			//		result.Append("bg-danger");
			//	else if (item.LastJobLaunch.IsAtWork)
			//		result.Append("bg-light-green");
			//	else
			//		result.Append("bg-primary-green");
			//}
			//else
			//{
			//	result.Append("bg-primary-green");
			//}
			result.Append("bg-primary-green");
			if (day.IsPast || day.IsToday && hour.IsPast)
				result.Append(" text-white-50");
			else
				result.Append(" text-white");
			return result.ToString();
		}

		
		public static string GetCollectionToolTip(CollectionDTO item)
		{
			StringBuilder result = new StringBuilder();

			result.Append($"<h6>{Resources.Setting.Id}: {item.Id}</h6>");
			result.Append($"<p>{item.Name}</p>");
			result.Append($"<p>{Resources.Setting.Client}: {item.ClientName}</p>");

			if (item.LastJobLaunch != null)
			{
				var launch = item.LastJobLaunch;
				result.Append($"<p>{Resources.Setting.LastStart}: {launch.StartDateTime}</p>");
				if (launch.IsAtWork)
				{
					result.Append($"<p>{Resources.Setting.AtWork}</p>");
				}
				else
				{
					result.Append($"<p>{Resources.Setting.Completed}: {launch.EndDateTime}</p>");
					if (item.LastJobLaunch.Success)
					{
						result.Append($"<p>{Resources.Setting.Result}: {Resources.Setting.Success}</p>");
					}
					else
					{
						result.Append($"<p>{Resources.Setting.Result}: {launch.Message}</p>");
					}
				}

			}
			else
			{
				result.Append($"<p>{Resources.Setting.LastStart}: - </p>");
			}

			return result.ToString();
		}
	}
}