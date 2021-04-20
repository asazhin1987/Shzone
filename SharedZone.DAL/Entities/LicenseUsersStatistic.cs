using System.ComponentModel.DataAnnotations.Schema;
using System;
using SharedZone.DAL.Abstract;

namespace SharedZone.DAL.Entities
{
	public class LicenseUsersStatistic : BimacadUnit
	{
		public int Id { get; set; }
		public int ClientId { get; set; }
		public Client Client { get; set; }

		[Index(IsClustered = false, IsUnique = false)]
		public DateTime Date { get; set; }

		[Index(IsClustered = false, IsUnique = false)]
		public int MonthNum { get; set; }

		[Index(IsClustered = false, IsUnique = false)]
		public int YearNum { get; set; }

		public int Qnt { get; set; }
		public int UniqQnt { get; set; }
	}
}
