//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System;
//using SharedZone.DAL.Abstract;

//namespace SharedZone.DAL.Entities
//{
//	public class LicenseUsing : BimacadUnit
//	{
//		public int Id { get; set; }

//		public int ClientId { get; set; }

//		public DateTime LastAccess { get; set; }

//		[Index(IsClustered = false, IsUnique = false)]
//		public DateTime LastAccessDate { get; set; }

//		public ICollection<LicenseUsersStatistic> LicenseUsersStatistics { get; set; }

//		public LicenseUsing()
//		{
//			LicenseUsersStatistics = new List<LicenseUsersStatistic>();
//		}

//	}
//}
