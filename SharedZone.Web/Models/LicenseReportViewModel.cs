using SharedZone.DTO;
using System.Collections.Generic;

namespace SharedZone.Web.Models
{
	public class LicenseReportViewModel
	{
		public LicenseViewModel License { get; set; }
		public int AllQnt { get; set; }
		public int UsingQnt { get; set; }
		public int Last3MonthsQnt { get; set; }
		public IEnumerable<LicenseUsingDTO> Licenses { get; set; }
		public IEnumerable<LicenseMonthUsingDTO> LicenseUsings { get; set; }
	}
}