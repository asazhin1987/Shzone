using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using SharedZone.DAL.Abstract;

namespace SharedZone.DAL.Entities
{
	public class Collection : NamedUnit
	{

		[StringLength(500)]
		public string Description { get; set; }
		
		/*SCHEDULE*/
		public int HourId { get; set; }
		public Hour Hour { get; set; }
		public int MinuteId { get; set; }
		public Minute Minute { get; set; }

		[Index(IsClustered =false, IsUnique = false)]
		public TimeSpan ScheduleTime { get; set; }

		public virtual ICollection<WeekDay> WeekDays { get; set; }

		/*MODEL*/
		public virtual ICollection<RevitModel> RevitModels { get; set; }


		/*JOBS*/
		public ICollection<RevitJob> RevitJobs { get; set; }
		public ICollection<NavisJob> NavisJobs { get; set; }
		public ICollection<IFCJob> IFCJobs { get; set; }


		/*OPEN PARAMS*/
		public bool Detach { get; set; }
		public int? RevitVersionId { get; set; }
		public RevitVersion RevitVersion { get; set; }
		public bool IsDefaultVersion { get; set; }
		public bool Worksets { get; set; }
		public bool Audit { get; set; }
		[StringLength(500)]
		public string ExceptionWorksets { get; set; }

		/*CLIENTS*/
		public int? ClientId { get; set; }
		public virtual Client Client { get; set; }

		/*LOG*/
		public ICollection<JobLaunch> JobLaunches { get; set; }


		public Collection()
		{
			WeekDays = new List<WeekDay>();
			RevitModels = new List<RevitModel>();

			RevitJobs = new List<RevitJob>();
			NavisJobs = new List<NavisJob>();
			IFCJobs = new List<IFCJob>();

			JobLaunches = new List<JobLaunch>();
		}

		public static implicit operator Collection(int id) => 
			new Collection() {Id = id };

		public override bool Equals(object obj) =>
			Id == ((Collection)obj).Id;

		public override int GetHashCode() =>
			(Id).GetHashCode();
	}
}
