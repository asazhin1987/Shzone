using System;
using System.Collections.Generic;
using SharedZone.DAL.Abstract;

namespace SharedZone.DAL.Entities
{
	public class RevitVersion : NamedUnit
	{
		public bool IsDefault { get; set; }
		public ICollection<RevitServer> Servers { get; set; }

		public RevitVersion()
		{
			Servers = new List<RevitServer>();
		}
	}
}
