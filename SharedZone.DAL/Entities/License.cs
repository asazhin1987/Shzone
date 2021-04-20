using SharedZone.DAL.Abstract;
using System.ComponentModel.DataAnnotations;

namespace SharedZone.DAL.Entities
{
	public class License : BimacadUnit
	{
		[StringLength(125)]
		[Key]
		public string Key { get; set; }
	}
}
