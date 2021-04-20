using System;
using SharedZone.DAL.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedZone.DAL.Entities
{
	public class JobLaunch : BimacadUnit
	{
		public int Id { get; set; }
		public int CollectionId { get; set; }
		public Collection Collection { get; set; }

		public DateTime Odate { get; set; }
		public DateTime? StartDateTime { get; set; }
		public DateTime? EndDateTime { get; set; }

		public int? Duration { get; set; }

		public int ClientId { get; set; }
		public Client Client { get; set; }

		public bool Success { get; set; }

		//[StringLength(500)]
		public string Message { get; set; }

		public int WeekDayId { get; set; }
		public WeekDay WeekDay { get; set; }

		public int? HourId { get; set; }
		public Hour Hour { get; set; }

		public int? MinuteId { get; set; }
		public Minute Minute { get; set; }

	}
}
