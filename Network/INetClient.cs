//

namespace Opaq.Network {
	public interface INetClient {
		void Send(byte[] message);
		void Disconnect();


		event Action<INetClient, byte[]>? OnMessageRecieved;
		event Action<INetClient>? OnDisconnected;
	
	}
}