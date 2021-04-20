using SharedZone.DTO;
using System;
using System.Collections.Generic;
using System.Web.Razor.Editor;
using System.ComponentModel.DataAnnotations;
using Bimacad.Sys;

namespace SharedZone.Web.Models
{
	public class ServerViewModel : NamedUnitViewModel
	{
		[Display(Name = "Version", ResourceType = typeof(Resources.Setting))]
		public int RevitVersionId { get; set; }

		[ScaffoldColumn(false)]
		public bool IsDirectory { get; set; }

		[ScaffoldColumn(false)]
		public string NameCaption { get { return IsDirectory ? Resources.Setting.Directory : Resources.Setting.RevitServerName; } }

		public ServerViewModel(ServerDTO item)
		{
			Mapper.Map(this, item);
		}

		public ServerViewModel()
		{

		}
	}
}