//

using System.Net;

namespace Opaq.Network {
	public interface INetServer {
		void Start(IPAddress address, int port);
		void Stop();

		IPEndPoint? EndPoint {get;}

		event Action<INetServer, INetClient> OnClientConnected;
		event Action<INetServer, INetClient> OnClientDisconnected;
		event Action<INetServer, INetClient, byte[]> OnMessageReceived;
	}
}