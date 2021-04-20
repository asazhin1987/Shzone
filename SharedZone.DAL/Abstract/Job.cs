using System.ComponentModel.DataAnnotations;

namespace SharedZone.DAL.Abstract
{
	public abstract class Job : NamedUnit
	{
		[StringLength(255)]
		public string Path { get; set; }
	}
}
