
using Opaq.Network;
using Opaq.Games;
using System.Net;

namespace Opaq.Server.Dispatcher;

public class LocalDispatcher(IGameLinkService game) : IDispatcherService {
	public void Dispose() {
	}

	public async Task<IPEndPoint?> GetServerForSession(SessionInfo session) {
		IPEndPoint? server = null;
		try {
			server = await game.JoinServer(session);
		} catch (Exception exc) {
			Logs.LogError(exc.Message);
		}
		return server;
	}

	// public async Task<IPEndPoint?> CreateServer(SessionInfo session, uint settingsIndex) {
	// 	IPEndPoint? server = null;
	// 	try {
	// 		server = await game.CreateServer(session, settingsIndex);
	// 	} catch (Exception exc) {
	// 		Logs.LogError(exc.Message);
	// 	}
	// 	return server;
	// }

}

/*
// public class CommandChannelClient(ServerInfo serverInfo) : IDisposable {
// 	private EndPoint endPoint = serverInfo.GetEndPoint();
// 	private Socket socket = new(SocketType.Stream, ProtocolType.Tcp);

// 	public void Dispose() {
// 		socket?.Shutdown(SocketShutdown.Both);
// 		socket?.Dispose();
// 		GC.SuppressFinalize(this);
// 	}

// 	public async Task<byte[]> SendCommand(MessageBinary message) {
// 		byte[] result = [];
// 		try {
// 			await socket.ConnectAsync(endPoint);
// 			await socket.SendAsync(message.GetBytes(), SocketFlags.None);

// 			byte[] buffer = new byte[1024];
// 			var received = await socket.ReceiveAsync(buffer, SocketFlags.None);
// 			result = buffer;
// 		} catch (Exception exc) {
// 			Logs.Log(exc.ToString());
// 		}
// 		return result;
// 	}
// }

public class LocalDispatcher : IDispatcherService {
	//private ServerInfo gameServer;
	private ServerInfo cmdServer;

//	private Dictionary<uint, bool> gamesList = [];

	public LocalDispatcher(ServerInfo game, ServerInfo commands, ServerInfo hqAddr) {
		gameServer = game;
		//cmdServer = commands;
		hqNode = new(hqAddr, this);
		game = hqNode.CreateLink(commands);
	}

	public void Dispose() {
		GC.SuppressFinalize(this);
	}

	public async Task<ServerInfo?> JoinServer(uint inviteCode, SessionInfo session) {
		var isJoined = await game.SendJoinRequestToGame(session);
		//await Task.Delay(1000);
		return isJoined ? gameServer : null;
	}

	public async Task<bool> PrecreateLobby(SessionInfo session) {
		var result = await game.SendCommand(new MessageBinary(0x01, session));
		return result?[0] == 1;
	}

	private async Task<bool> SendJoinRequestToGame(SessionInfo session) {
		var result = await game.SendCommand(new MessageBinary(0x01, session));
		return result?[0] == 1;
	}
}


*/