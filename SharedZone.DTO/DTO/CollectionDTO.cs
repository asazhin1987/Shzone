using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Bimacad.Sys;

namespace SharedZone.DTO
{
	[DataContract]
	public partial class CollectionDTO : NamedUnitDTO
	{
		[DataMember]
		public string Description { get; set; }

		/*SCHEDULE*/
		//[DataMember]
		//public DateTime? LastDateTime { get; set; }

		

		[DataMember]
		[NotMaping]
		public IEnumerable<JobLaunchDTO> JobLaunchDTOs { get; set; }

		//[DataMember]
		[NotMaping]
		public JobLaunchDTO LastJobLaunch
		{
			get { return JobLaunchDTOs?.LastOrDefault(); } 
		}


		[DataMember]
		[NotMaping]
		public IEnumerable<WeekDayDTO> WeekDayDTOs { get; set; }

		//[DataMember]
		//public DateTime? NextDateTime { get; set; }

		[DataMember]
		public int HourId { get; set; }

		[DataMember]
		[NotMaping]
		public string HourName { get; set; }

		[DataMember]
		[NotMaping]
		public int HourValue{ get; set; }

		[DataMember]
		public int MinuteId { get; set; }

		[DataMember]
		[NotMaping]
		public string MinuteName { get; set; }

		[DataMember]
		[NotMaping]
		public int MinuteValue { get; set; }

		//[DataMember]
		[NotMaping]
		public TimeSpan StartTime { get { return new TimeSpan(HourValue, MinuteValue, 0); } }

		/*OPEN PARAMS*/
		[DataMember]
		public bool Detach { get; set; }

		[DataMember]
		public int? RevitVersionId { get; set; }

		[DataMember]
		public bool Worksets { get; set; }

		[DataMember]
		public bool Audit { get; set; }

		[DataMember]
		public string ExceptionWorksets { get; set; }

		/*CLIENTS*/
		[DataMember]
		public int ClientId { get; set; }

		[DataMember]
		public string ClientName { get; set; }

		[DataMember]
		public bool DefaultClient { get; set; }

		[DataMember]
		[NotMaping]
		public IEnumerable<int> WeekDays { get; set; }

		[DataMember]
		[NotMaping]
		public IEnumerable<RevitJobDTO> RevitJobsDTO { get; set; }

		[DataMember]
		[NotMaping]
		public IEnumerable<NavisJobDTO> NavisJobsDTO { get; set; }

		[DataMember]
		[NotMaping]
		public IEnumerable<IFCJobDTO> IFCJobsDTO { get; set; }

		[DataMember]
		[NotMaping]
		public IEnumerable<ServerDTO> ServersDTO { get; set; }

		[DataMember]
		[NotMaping]
		public IEnumerable<RevitModelDTO> RevitModelsDTO { get; set; }

		[DataMember]
		public int ModelsCount { get; set; }

		[DataMember]
		public IEnumerable<DateTime> Schedule{ get; set; }

		[DataMember]
		[NotMaping]
		public IEnumerable<int> RevitModelIds { get; set; }

		public CollectionDTO() : base()
		{
			RevitModelsDTO = new List<RevitModelDTO>();
			ServersDTO = new List<ServerDTO>();
			IFCJobsDTO = new List<IFCJobDTO>();
			NavisJobsDTO = new List<NavisJobDTO>();
			RevitJobsDTO = new List<RevitJobDTO>();
			WeekDays = new List<int>();
			RevitModelIds = new List<int>();
		}

		public override string ToString()
		{
			return $"{Id} {Name}";
		}
	}
}
