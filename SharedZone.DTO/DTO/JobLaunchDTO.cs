using Bimacad.Sys;
using System;
using System.Runtime.Serialization;

namespace SharedZone.DTO
{
	[DataContract]
	public partial class JobLaunchDTO : BimacadUnitDTO
	{
		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public int CollectionId { get; set; }

		[DataMember]
		public string CollectionName { get; set; }

		[DataMember]
		public DateTime Odate { get; set; }

		[DataMember]
		public DateTime? StartDateTime { get; set; }

		[DataMember]
		public DateTime? EndDateTime { get; set; }

		[NotMaping]
		public bool IsAtWork { get { return StartDateTime.HasValue && !EndDateTime.HasValue; } }

		[DataMember]
		public int? Duration { get; set; }

		[DataMember]
		public int ClientId { get; set; }

		[DataMember]
		public string ClientName { get; set; }

		[DataMember]
		public bool Success { get; set; }

		[DataMember]
		public string Message { get; set; }

		[DataMember]
		public int WeekDayId { get; set; }

		[DataMember]
		public int? HourId { get; set; }

		[DataMember]
		public int? HourValue { get; set; }

		[DataMember]
		public int? MinuteId { get; set; }

		[DataMember]
		public int? MinuteValue { get; set; }

		public override string ToString()
		{
			return $"{Id}-{CollectionId}_{ClientName} {HourValue}:{MinuteValue}";
		}
	}
}
