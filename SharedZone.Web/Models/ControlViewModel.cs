using Bimacad.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharedZone.Web.Models
{
	public class ControlViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string FormName { get; set; }
		public string DisplayText { get; set; }
		public string OnChangeAction { get; set; }

		public ControlViewModel(NamedUnitViewModel item)
		{
			Mapper.Map(this, item);
			DisplayText = item.Name;
		}

		public ControlViewModel() { }
	}

	public class CheckBoxControl : ControlViewModel
	{
		public int Value { get; set; }
		public bool Checked { get; set; }

		public CheckBoxControl(NamedUnitViewModel item) : base(item) { }
		public CheckBoxControl() : base() { }
	}

	public class RadioViewModel : CheckBoxControl
	{
		public RadioViewModel(NamedUnitViewModel item) : base(item) { }
		public RadioViewModel() : base() { }
	}

	public class CheckBoxViewModel : CheckBoxControl
	{
		public CheckBoxViewModel(NamedUnitViewModel item) : base(item) { }
		public CheckBoxViewModel() : base() { }
	}

	public class DropDownViewModel : ControlViewModel
	{
		public bool Selected { get; set; }
		public int Value { get; set; }
		public DropDownViewModel(NamedUnitViewModel item) : base(item) { }
		public DropDownViewModel() : base() { }
	}

	public class DropDownsViewModel
	{
		public string Name { get; set; }
		public IEnumerable<DropDownViewModel> Items { get; set; }

		public DropDownsViewModel(string name, IEnumerable<NamedUnitViewModel> items, int? selected = null)
		{
			Name = name;
			SetItems(items, selected);
		}

		public DropDownsViewModel(string name)
		{
			Name = name;
		}

		internal virtual void SetItems(IEnumerable<NamedUnitViewModel> items, int? selected = null)
		{
			int _selected = selected ?? 0;
			Items = items.Select(x => new DropDownViewModel(x)
			{
				Name = Name,
				Id = $"{Name}{x.Id}",
				Selected = x.Id == _selected,
				Value = x.Id
			});
		}

	}

	/*REVIT VERSIONS*/
	public class RevitVersionsDropDownViewModel : DropDownsViewModel
	{
		public RevitVersionsDropDownViewModel(IEnumerable<NamedUnitViewModel> items, int? selected)
			: base("RevitVersionId", items, selected){}

	}

	/*CLIENTS*/
	public class ClientsDropDownViewModel : DropDownsViewModel
	{
		public ClientsDropDownViewModel(IEnumerable<NamedUnitViewModel> items, int? selected)
			: base("ClientId", items, selected ?? 0) { }
	}

	/*HOURS*/
	public class HoursDropDownViewModel : DropDownsViewModel
	{
		public HoursDropDownViewModel(IEnumerable<NamedUnitViewModel> items, int? selected)
			: base("HourId", items, selected) { }
	}

	/*MINUTES*/
	public class MinutesDropDownViewModel : DropDownsViewModel
	{
		public MinutesDropDownViewModel(IEnumerable<NamedUnitViewModel> items, int? selected)
			: base("MinuteId", items, selected) { }
	}


	/*JOBS*/
	/*NAVIS CONVERT ELEMENT PROP*/
	public class NavisConvertElementsPropertiesViewModel : DropDownsViewModel
	{
		public NavisConvertElementsPropertiesViewModel(IEnumerable<NamedUnitViewModel> items, int? selected)
			: base("NavisConvertElementsPropertyId", items, selected) { }
	}

	/*NAVIS COORDINATES*/
	public class NavisCoordinatesViewModel : DropDownsViewModel
	{
		public NavisCoordinatesViewModel(IEnumerable<NamedUnitViewModel> items, int? selected)
			: base("NavisCoordinateId", items, selected) { }
	}

	/*NAVIS VIEW*/
	public class NavisViewsViewModel : DropDownsViewModel
	{
		public NavisViewsViewModel(IEnumerable<NamedUnitViewModel> items, int? selected)
			: base("NavisViewId", items, selected) { }
	}

	/*IFC INCLUDE*/
	public class IFCIncludeBoundatiesViewModel : DropDownsViewModel
	{
		public IFCIncludeBoundatiesViewModel(IEnumerable<NamedUnitViewModel> items, int? selected)
			: base("IFCIncludeBoundatyId", items, selected) { }
	}

	/*IFC TYPES*/
	public class IFCFileTypesViewModel : DropDownsViewModel
	{
		public IFCFileTypesViewModel(IEnumerable<NamedUnitViewModel> items, int? selected)
			: base("IFCFileTypeId", items, selected) { }
	}
}