//тестовый сервер со всем локальным: диспетчером, лобби, игрой

using System.Diagnostics;
using System.Net;
using Opaq.Server.Dispatcher;
using Opaq.Server.Login;
using Opaq.Server.Games;
using Opaq.Games;

namespace Opaq.Server.Test;

public class TestServer {
	public static void Main(string[] args) {
		Logs.Log("Server is running.");
		//var config = Configs.LoadConfig<ServerConfig>(args.Length > 0 ? args[0] : null) ?? new();

		IDispatcherService dispatcher;
		//dispatcher = DispatchersFactory.CreateDispatcher(config.dispatchers);
		{
			var server = new Opaq.Network.WebSocket.Server();
			server.Start(IPAddress.Parse("127.0.0.1"), 81);
			var poker = new LocalGamesLink(
				server,
				Opaq.Games.Poker.PokerSettings.PredefinedSettings,
				(session, settings) => new Lobby(session, settings)
			);
			dispatcher = new LocalDispatcher(poker);
		}

		var websocket = new Opaq.Network.WebSocket.Server();
		//websocket.Start(config.server.ip, config.server.port);
		websocket.Start(IPAddress.Parse("127.0.0.1"), 80);

		using var login = new LoginServer(websocket, new AuthenticatorTest(), dispatcher);

		Process.GetCurrentProcess().WaitForExit();
		websocket.Stop();
	}

}

