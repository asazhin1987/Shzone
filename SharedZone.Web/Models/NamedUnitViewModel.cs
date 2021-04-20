using SharedZone.DTO;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;

namespace SharedZone.Web.Models
{
	public class NamedUnitViewModel
	{
		[Display(Name = "Id", ResourceType = typeof(Resources.Setting))]
		public int Id { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources.Required),
				  ErrorMessageResourceName = "Name")]
		[Display(Name = "Name", ResourceType = typeof(Resources.Setting))]
		[StringLength(125)]
		public string Name { get; set; }


		public NamedUnitViewModel()
		{

		}

		public NamedUnitViewModel(NamedUnitDTO item)
		{
			try
			{
				ResourceManager rm = new ResourceManager("Bimacad.Sys.Resources.Setting", typeof(Resources.Setting).Assembly);
				Name = rm.GetString(item.Name) ?? item.Name;
			}
			catch
			{
				Name =  item.Name;
			}
			Id = item.Id;
		}

		public static implicit operator NamedUnitViewModel(NamedUnitDTO item)
		{
			return new NamedUnitViewModel(item);
		}
	}
}