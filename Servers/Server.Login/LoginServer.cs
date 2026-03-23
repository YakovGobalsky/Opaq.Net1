using Opaq.Network;
using Opaq.Server.Dispatcher;

namespace Opaq.Server.Login;

public class LoginServer : IDisposable {
	private LoginAuthenticator authenticator;
	private IDispatcherService dispatcher;

	//private IDisposable subscriptions;
	private TCPServerProcessor<BasePlayer> subscriptions;

	public LoginServer(INetServer listenServer, LoginAuthenticator authManager, IDispatcherService dispatcherServer) {
		authenticator = authManager;
		dispatcher = dispatcherServer;

		subscriptions = new(
			server: listenServer,
			playersFactory: CreatePlayer
		);
		subscriptions.AddMessageHandler<MessageRPC>(OnRPCRecieved);
	}

	public void Dispose() {
		subscriptions?.Dispose();
		GC.SuppressFinalize(this);
	}

	private BasePlayer CreatePlayer(INetServer server, INetClient client) => new(client);

	private async Task OnRPCRecieved(BasePlayer player, MessageRPC incomingMessage) {
		try {
			if (await authenticator.ProcessMessage(player, incomingMessage)) {
				var serverInfo = await dispatcher.GetServerForSession(player.session);
				//serverInfo ??= await dispatcher.CreateServer(player.session, 0);
				if (serverInfo == null) {
					player.client?.Send(MessageRPCPopup.CreatePopupMessage(MessageRPCPopup.PopupType.ERROR, "No servers found").GetBytes());
				} else {
					Logs.Log($"Redirect server: {serverInfo}");
					player.client?.Send(new MessageRedirect(serverInfo, player.session).GetBytes());
				}
			} else {
				player.client?.Send(MessageRPCPopup.CreatePopupMessage(MessageRPCPopup.PopupType.ERROR, "Auth failed").GetBytes());
			}
		} catch (Exception exc) {
			Logs.LogWarning($"LobbyServer:OnMessageRecieved error: {exc.Message}");
		}
		await Task.Delay(5000);//2do: remove
		player.client?.Disconnect();
	}
}