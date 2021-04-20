using System;
using SharedZone.DAL.Abstract;
using System.ComponentModel.DataAnnotations;

namespace SharedZone.DAL.Entities
{
	public class ServerLog : BimacadUnit
	{
		public int Id { get; set; }
		public int ServerId { get; set; }
		public RevitServer Server { get; set; }
		public DateTime Odate { get; set; }
		public int AddedQnt { get; set; }
		public int RemovedQnt { get; set; }
		public bool Success { get; set; }

		[StringLength(500)]
		public string Message { get; set; }
	}
}
