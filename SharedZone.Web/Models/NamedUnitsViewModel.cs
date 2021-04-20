using SharedZone.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharedZone.Web.Models
{
	public class NamedUnitsViewModelBase
	{
		public IEnumerable<NamedUnitViewModel> Items { get; set; }

		public NamedUnitsViewModelBase()
		{
			Items = new List<NamedUnitViewModel>();
		}

		public NamedUnitsViewModelBase(IEnumerable<NamedUnitDTO> items)
		{
			Items = items.Select(x => (NamedUnitViewModel)x).ToList();
		}
	}

	public class NamedUnitsViewModel : NamedUnitsViewModelBase
	{
		public string FormName { get; set; }
		
		public NamedUnitsViewModel()
		{
		}

		public NamedUnitsViewModel(IEnumerable<NamedUnitViewModel> items, string formName)
		{
			FormName = formName;
			Items = items;
		}

		public NamedUnitsViewModel(IEnumerable<NamedUnitDTO> items, string formName) : base(items)
		{
			FormName = formName;
		}

	}
}