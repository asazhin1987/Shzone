using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using SharedZone.DAL.Abstract;

namespace SharedZone.DAL.Entities
{
	public class IFCJob : Job
	{
		public bool CurrentView { get; set; }
		public string CurrentViewName { get; set; }
		public bool DivideWalls { get; set; }
		public bool BasicValues { get; set; }
		public int IFCFileTypeId { get; set; }
		public IFCFileType IFCFileType { get; set; }
		public int IFCIncludeBoundatyId { get; set; }
		public IFCIncludeBoundaty IFCIncludeBoundaty { get; set; }

		public int CollectionId { get; set; }
		public Collection Collection { get; set; }

	}
}
