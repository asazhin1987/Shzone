
using System.Collections.Generic;
using SharedZone.DAL.Abstract;
using System.ComponentModel.DataAnnotations;

namespace SharedZone.DAL.Entities
{
	public class WeekDay : NamedUnit
	{

		public virtual ICollection<Collection> Collections { get; set; }

		public WeekDay()
		{
			Collections = new List<Collection>();
		}
	}
}
