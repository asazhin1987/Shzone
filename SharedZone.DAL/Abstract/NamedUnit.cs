using System.ComponentModel.DataAnnotations;

namespace SharedZone.DAL.Abstract
{
	public abstract class NamedUnit : BimacadUnit
	{
		public int Id { get; set; }

		[StringLength(125)]
		public string Name { get; set; }
	}
}
