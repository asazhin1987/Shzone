using Bimacad.Sys;
using SharedZone.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SharedZone.Web.Models
{
	public abstract class JobViewModel : NamedUnitViewModel
	{
		[Display(Name = "Path", ResourceType = typeof(Resources.Setting))]
		[Required(ErrorMessageResourceType = typeof(Resources.Required),
				  ErrorMessageResourceName = "Path")]
		[StringLength(255)]
		public string Path { get; set; }

		[Display(Name = "CollectionName", ResourceType = typeof(Resources.Setting))]
		public int CollectionId { get; set; }

		[Display(Name = "CollectionName", ResourceType = typeof(Resources.Setting))]
		public string CollectionName { get; set; }

		public virtual string RefName { get; }

		public string Icon { get { return $"{RefName}.png"; } }

		public string ActionName { get { return $"Edit{RefName}Job"; } }

		public string PartialViewName { get { return $"_{RefName}JobEdit"; } }


	}


	public class RevitJobViewModel : JobViewModel
	{
		[ScaffoldColumn(false)]
		public override string RefName { get { return "Revit"; } }

		[Display(Name = "TruncateView", ResourceType = typeof(Resources.Setting))]
		public bool TruncateView { get; set; }

		[Display(Name = "ExceptionViews", ResourceType = typeof(Resources.Setting))]
		public string ExceptionViews { get; set; }

		[Display(Name = "AffixLinks", ResourceType = typeof(Resources.Setting))]
		public bool AffixLinks { get; set; }

		[Display(Name = "AffixesLinks", ResourceType = typeof(Resources.Setting))]
		public string AffixesLinks { get; set; }

		[Display(Name = "TruncateLinks", ResourceType = typeof(Resources.Setting))]
		public bool TruncateLinks { get; set; }

		[Display(Name = "Purge", ResourceType = typeof(Resources.Setting))]
		public bool Purge { get; set; }



		public RevitJobViewModel(RevitJobDTO item)
		{
			Mapper.Map(this, item);
		}

		public RevitJobViewModel() { }

		public static implicit operator RevitJobViewModel(RevitJobDTO item) =>
			new RevitJobViewModel(item);
	}

	public class NavisJobViewModel : JobViewModel
	{
		[ScaffoldColumn(false)]
		public override string RefName { get { return "Navis"; } }

		[Display(Name = "ConvertIdentif", ResourceType = typeof(Resources.Setting))]
		public bool ConvertIdentif { get; set; }

		[Display(Name = "TransformURL", ResourceType = typeof(Resources.Setting))]
		public bool TransformURL { get; set; }

		[Display(Name = "TransformRoom", ResourceType = typeof(Resources.Setting))]
		public bool TransformRoom { get; set; }

		[Display(Name = "TransformElementsProperty", ResourceType = typeof(Resources.Setting))]
		public bool TransformElementsProperty { get; set; }

		[Display(Name = "TransformLinkedFiles", ResourceType = typeof(Resources.Setting))]
		public bool TransformLinkedFiles { get; set; }

		[Display(Name = "TransformConstructionElements", ResourceType = typeof(Resources.Setting))]
		public bool TransformConstructionElements { get; set; }

		[Display(Name = "DevideFiles", ResourceType = typeof(Resources.Setting))]
		public bool DevideFiles { get; set; }

		[Display(Name = "ExportGeometryRoom", ResourceType = typeof(Resources.Setting))]
		public bool ExportGeometryRoom { get; set; }

		[Display(Name = "NavisConvertElementsProperty", ResourceType = typeof(Resources.Setting))]
		public int NavisConvertElementsPropertyId { get; set; }

		[Display(Name = "NavisCoordinate", ResourceType = typeof(Resources.Setting))]
		public int NavisCoordinateId { get; set; }

		[Display(Name = "NavisView", ResourceType = typeof(Resources.Setting))]
		public int NavisViewId { get; set; }

		[Display(Name = "ViewName", ResourceType = typeof(Resources.Setting))]
		public string ViewName { get; set; }

		public NavisJobViewModel(NavisJobDTO item)
		{
			Mapper.Map(this, item);
		}

		public NavisJobViewModel() { }

		public static implicit operator NavisJobViewModel(NavisJobDTO item) =>
			new NavisJobViewModel(item);
	}

	public class IFCJobViewModel : JobViewModel
	{
		[ScaffoldColumn(false)]
		public override string RefName { get { return "IFC"; } }

		[Display(Name = "CurrentView", ResourceType = typeof(Resources.Setting))]
		public bool CurrentView { get; set; }

		[Display(Name = "CurrentViewName", ResourceType = typeof(Resources.Setting))]
		public string CurrentViewName { get; set; }

		[Display(Name = "DivideWalls", ResourceType = typeof(Resources.Setting))]
		public bool DivideWalls { get; set; }

		[Display(Name = "BasicValues", ResourceType = typeof(Resources.Setting))]
		public bool BasicValues { get; set; }

		[Display(Name = "IFCFileType", ResourceType = typeof(Resources.Setting))]
		public int IFCFileTypeId { get; set; }

		[Display(Name = "IFCIncludeBoundaty", ResourceType = typeof(Resources.Setting))]
		public int IFCIncludeBoundatyId { get; set; }

		public IFCJobViewModel(IFCJobDTO item)
		{
			Mapper.Map(this, item);
		}

		public IFCJobViewModel() { }

		public static implicit operator IFCJobViewModel(IFCJobDTO item) =>
			new IFCJobViewModel(item);
	}
}