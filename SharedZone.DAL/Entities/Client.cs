using System;
using System.Collections.Generic;
using SharedZone.DAL.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedZone.DAL.Entities
{
	public class Client : BimacadUnit
	{
		public int Id { get; set; }

		[StringLength(125)]
		[Index(IsClustered = false, IsUnique = false)]
		public string Name { get; set; }

		[StringLength(125)]
		public string UserName { get; set; }

		public bool OnLine { get; set; }

		public ICollection<Collection> Collections { get; set; }

		public DateTime LastAccess { get; set; }

		[Index(IsClustered = false, IsUnique = false)]
		public DateTime LastAccessDate { get; set; }

		public ICollection<LicenseUsersStatistic> LicenseUsersStatistics { get; set; }


		public Client()
		{
			Collections = new List<Collection>();
			LicenseUsersStatistics = new List<LicenseUsersStatistic>();
		}
	}
}
