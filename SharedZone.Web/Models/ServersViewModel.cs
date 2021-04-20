using SharedZone.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SharedZone.Web.Models
{

	public abstract class ServersViewModel
	{

		public string Caption { get; set; }
		public bool IsDirectory { get; set; }
		public IEnumerable<ServerDTO> Items { get; set; }
		public string CreateAction { get; set; }
		public string UpdateAction { get; set; }
		public bool IsNotEmpty { get { return Items != null && Items.Count() > 0; } }

		public ServersViewModel(IEnumerable<ServerDTO> items)
		{
			Items = items;
		}

	}

	public class AllServersViewModel
	{
		public RevitServersViewModel RevitServers { get; set; }
		public DirectoriessViewModel Directories { get; set; }

		public AllServersViewModel(IEnumerable<ServerDTO> items)
		{
			RevitServers = new RevitServersViewModel(items.Where(x => x.IsDirectory == false));
			Directories = new DirectoriessViewModel(items.Where(x => x.IsDirectory == true));
		}

		
	}

	public class RevitServersViewModel : ServersViewModel
	{
		public RevitServersViewModel(IEnumerable<ServerDTO> items) : base(items)
		{
			Caption = Resources.Setting.ServerCaption;
			IsDirectory = false;
			CreateAction = "CreateServer";
			UpdateAction = "ServerRows";
		}


	}


	public class DirectoriessViewModel : ServersViewModel
	{
		public DirectoriessViewModel(IEnumerable<ServerDTO> items) : base(items)
		{
			Caption = Resources.Setting.DirectoryCaptions;
			IsDirectory = true;
			CreateAction = "CreateDirectory";
			UpdateAction = "DirectoryRows";
		}
	}

}