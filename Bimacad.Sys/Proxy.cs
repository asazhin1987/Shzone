using System;
using System.ServiceModel;

namespace Bimacad.Sys
{
	public class ProxyFactory<T> : IDisposable where T : IService
	{
		private ChannelFactory<T> ChannelFactory { get; }
		public T Service { get; }

		public ProxyFactory(string URL, TimeSpan? closeTimeout = null)
		{

			try
			{
				BasicHttpBinding binding = new BasicHttpBinding()
				{
					MaxReceivedMessageSize = 2147483647,
					CloseTimeout = closeTimeout ?? new TimeSpan(0, 2, 0)
				};

				ChannelFactory = new ChannelFactory<T>(binding, new EndpointAddress(new Uri(URL)));
				Service = ChannelFactory.CreateChannel();
			}
			catch (Exception ex)
			{
				throw new Exception($"Не удалось создать канал связи. Exception {ex.Message}");
			}
		}


		public void Dispose()
		{
			if (ChannelFactory.State == CommunicationState.Opened)
				ChannelFactory.Close();
			GC.SuppressFinalize(this);
		}
	}
}
