using System;

namespace SharedZone.BLL.Infrastructure
{
	public class SharedZoneEventArgs : EventArgs
	{
		public readonly string Message;
		public bool Success { get; set; }

		public SharedZoneEventArgs(string message, bool success = true)
		{
			Message = message;
			Success = success;
		}
	}


	public class ServiceEventArgs : SharedZoneEventArgs
	{
		public ServiceEventArgs(string message, bool success = true) : base(message, success) { }
	}
}
