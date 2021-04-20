using Bimacad.Sys;
using SharedZone.DTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SharedZone.Web.Models
{
	public class CollectionViewModel
	{

		public ParamsCollectionViewModel Params { get; }
		public JobsViewModel Jobs { get; }
		public CollectionFilesViewModel Files { get; }

		public CollectionViewModel(CollectionDTO item)
		{
			Params = new ParamsCollectionViewModel(item);
			Jobs = new JobsViewModel(item);
			Files = new CollectionFilesViewModel(item);
		}
	}

	public class CollectionFilesViewModel
	{
		public int CollectionId { get; set; }
		public AllServersViewModel Servers { get; set; }
		public bool IsNotEmpty { get; set; }

		

		public CollectionFilesViewModel(int Id, IEnumerable<ServerDTO> servers)
		{
			CollectionId = Id;
			Servers = new AllServersViewModel(servers);
			IsNotEmpty = servers.Count() > 0;
		}

		public CollectionFilesViewModel(CollectionDTO item) : this(item.Id, item.ServersDTO)
		{
			//CollectionId = item.Id;
			//Servers = new AllServersViewModel(item.ServersDTO);
			//IsEmpty = item.ServersDTO.Count() > 0;
		}
	}

	public class JobsViewModel
	{
		public int CollectionId { get; set; }
		public IEnumerable<JobViewModel> RevitJobs { get; set; }
		public IEnumerable<JobViewModel> NavisJobs { get; set; }
		public IEnumerable<JobViewModel> IFCJobs { get; set; }

		public IEnumerable<JobViewModel> Jobs { get { return RevitJobs.Concat(NavisJobs).Concat(IFCJobs); } }

		public JobsViewModel(CollectionDTO item)
		{
			CollectionId = item.Id;
			RevitJobs = item.RevitJobsDTO.AsParallel().AsOrdered().Select(x => new RevitJobViewModel(x));
			NavisJobs = item.NavisJobsDTO.AsParallel().AsOrdered().Select(x => new NavisJobViewModel(x));
			IFCJobs = item.IFCJobsDTO.AsParallel().AsOrdered().Select(x => new IFCJobViewModel(x));
		}
	}

	public class ParamsCollectionViewModel
	{
		public CollectionDTO Collection { get; set; }
		//public int Id { get; set; }
		//public string Time { get; }
		//public string WorkSets { get; }
		//public string RevitVersion { get; set; }
		//public string Detach { get; set; }
		//public string WeekDays { get; }

		public ParamsCollectionViewModel(CollectionDTO item)
		{
			Collection = item;
			//Id = item.Id;
			//Time = $"{item.HourName} : {item.MinuteName}";
		}
	}



	public class EditCollectionViewModel : NamedUnitViewModel
	{
		[Display(Name = "Description", ResourceType = typeof(Resources.Setting))]
		[StringLength(500, MinimumLength = 0, ErrorMessageResourceType = typeof(Resources.Required),
				  ErrorMessageResourceName = "Description")]
		public string Description { get; set; }

		[Display(Name = "Detach", ResourceType = typeof(Resources.Setting))]
		public bool Detach { get; set; }

		[Display(Name = "OpenWorksets", ResourceType = typeof(Resources.Setting))]
		public bool OpenWorksets { get; set; }

		[Display(Name = "CloseWorksets", ResourceType = typeof(Resources.Setting))]
		public bool CloseWorksets { get; set; }

		[ScaffoldColumn(false)]
		public int Worksets { get; set; }

		[Display(Name = "ExceptionsWorksets", ResourceType = typeof(Resources.Setting))]
		public string ExceptionWorksets { get; set; }

		[Display(Name = "Audit", ResourceType = typeof(Resources.Setting))]
		public bool Audit { get; set; }

		[Display(Name = "DefaultClient", ResourceType = typeof(Resources.Setting))]
		public bool DefaultClient { get; set; }

		[Display(Name = "Client", ResourceType = typeof(Resources.Setting))]
		public int ClientId { get; set; }

		[Display(Name = "Client", ResourceType = typeof(Resources.Setting))]
		public string ClientName { get; set; }

		[Display(Name = "Version", ResourceType = typeof(Resources.Setting))]
		public int RevitVersionId { get; set; }

		public IEnumerable<int> WeekDays { get; set; }

		public int HourId { get; set; }

		public int MinuteId { get; set; }


		public EditCollectionViewModel(CollectionDTO item)
		{
			Mapper.Map(this, item);
			OpenWorksets = item.Worksets == true;
			CloseWorksets = item.Worksets == false;
			WeekDays = item.WeekDays;
		}

		public EditCollectionViewModel()
		{

		}
	}
}