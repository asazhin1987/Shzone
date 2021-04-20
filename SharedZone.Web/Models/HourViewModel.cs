using SharedZone.DTO;
using System;
using System.Threading;

namespace SharedZone.Web.Models
{
	public class HourViewModel : NamedUnitViewModel
	{
		public TimeSpan Time { get; set; }
		public bool IsCurrent { get; set; }
		public bool IsPast { get; set; }

		public HourViewModel(HourDTO dto)
		{
			Id = dto.Id;
			Time = new TimeSpan(dto.HourValue, 0, 0);
			Name = new DateTime(Time.Ticks).ToShortTimeString();
			IsCurrent = DateTime.Now.Hour == Time.Hours;
			IsPast = DateTime.Now.Hour > Time.Hours;
		}

		public static implicit operator HourViewModel(HourDTO dto) =>
			new HourViewModel(dto);
	}

	public class MinuteViewModel
	{
		public int Id { get; set; }
		public int MinuteValue { get; set; }


		public MinuteViewModel(MinuteDTO dto)
		{
			Id = dto.Id;
			MinuteValue = dto.MinuteValue;
		}

		public static implicit operator MinuteViewModel(MinuteDTO dto) =>
			new MinuteViewModel(dto);
	}
	
}