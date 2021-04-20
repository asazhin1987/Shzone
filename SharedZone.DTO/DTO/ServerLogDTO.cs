using System.Runtime.Serialization;
using System;
using System.Collections.Generic;

namespace SharedZone.DTO
{
	[DataContract]
	public partial class ServerLogDTO : BimacadUnitDTO
	{
		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public int ServerId { get; set; }

		[DataMember]
		public string ServerName { get; set; }

		[DataMember]
		public DateTime Odate { get; set; }

		[DataMember]
		public int AddedQnt { get; set; }

		[DataMember]
		public int RemovedQnt { get; set; }

		[DataMember]
		public bool Success { get; set; }

		[DataMember]
		public string Message { get; set; }


		public ServerLogDTO() : base()
		{

		}

		public ServerLogDTO(Exception ex, int serverId) : base()
		{
			Success = false;
			Odate = DateTime.Now;
			Message = ex.Message;
			ServerId = serverId;
		}


	}
}
