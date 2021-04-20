using System;

namespace Bimacad.Sys
{
	public static class Converter
	{
		public const double Fut = 304.8;
		public const double Metre = 3.28;

		public static double ToDouble(string strValue)
		{
			if (!double.TryParse(strValue, out double doubleValue))
				if (!double.TryParse(strValue.Replace('.', ','), out doubleValue))
					if (!double.TryParse(strValue.Replace(',', '.'), out doubleValue))
						return -999;
			return doubleValue;
		}


		public static int GetWeekDayId(this DateTime date)
		{
			return date.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)date.DayOfWeek;
		}

		public static int GetWeekNumId(this int Id)
		{
			return Id == 7 ? 0 : Id;
		}
	}
}
