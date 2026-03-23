using System.Diagnostics;

namespace Opaq.Network;

public class TCPServerProcessor<P> : IDisposable {
	public delegate Task MessageHandler<M>(P player, M message) where M : IMessage<M>;
	public delegate P? PlayersFactory(INetServer server, INetClient client);

	private PlayersFactory playersFactory;

	private readonly Dictionary<INetClient, P> players = [];

	private INetServer server;
	private Dictionary<int, Action<P, byte[]>> fns = [];

	public event Action<P>? OnPlayerDisconnected;

	public TCPServerProcessor(INetServer server, PlayersFactory playersFactory) {
		this.server = server;
		this.playersFactory = playersFactory;

		server.OnClientConnected += OnClientConnected;
		server.OnClientDisconnected += OnClientDisconnected;
		server.OnMessageReceived += OnMessageRecieved;
	}

	public void Dispose() {
		server.OnClientConnected -= OnClientConnected;
		server.OnClientDisconnected -= OnClientDisconnected;
		server.OnMessageReceived -= OnMessageRecieved;
		GC.SuppressFinalize(this);
	}

	public IDisposable AddMessageHandler<M>(MessageHandler<M> handler) where M:IMessage<M> {
		fns.TryAdd(M.MessageType, (player, bytes) => {
			handler(player, M.Deserialize(bytes));
		});
		return this;
	}

	private void OnMessageRecieved(INetServer server, INetClient client, byte[] message) {
		 if (message.Length > 0 && fns.ContainsKey(message[0])) {
		 	fns[message[0]](players[client], message);
		 } else {
		 	//throw new NotImplementedException();
		 	//Logs.LogWarning($"MessageType {message.type} not implemented");
		 	Logs.LogWarning($"MessageType {(message.Length > 0 ? message[0].ToString() : "<EMPTY>")} not implemented");
		 }
	}

	public void OnClientConnected(INetServer server, INetClient client) {
		Logs.Log($"TCPServerProcessor::Client connected. Clients before connect: {players.Count}");
		var player = playersFactory(server, client);
		if (player != null) {
			players.Add(client, player);
		} else {
			client?.Disconnect();
		}
	}

	public void OnClientDisconnected(INetServer server, INetClient client) {
		if (players.Remove(client, out var player)) {
			Logs.Log($"TCPServerProcessor::Client {player} disconnected");
			OnPlayerDisconnected?.Invoke(player);
			//player?.Dispose()
		} else {
			//Logs.Log($"Error! Client disconnected, but not found in players list! {new StackTrace()}");
		}
	}

}