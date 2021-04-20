using System;
using System.Collections.Generic;
using SharedZone.DAL.Abstract;


namespace SharedZone.DAL.Entities
{
	public class RevitServer : NamedUnit
	{
		public int RevitVersionId { get; set; }
		public RevitVersion RevitVersion { get; set; }
		public bool IsDirectory { get; set; }

		public ICollection<RevitModel> RevitModels { get; set; }
		public ICollection<ServerLog> ServerLogs { get; set; }

		public RevitServer()
		{
			RevitModels = new List<RevitModel>();
			ServerLogs = new List<ServerLog>();
		}


	}
}
