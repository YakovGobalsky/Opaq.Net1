using System.Net;
using Opaq.Games;
using Opaq.Network;

namespace Opaq.Server.Games;

public class LocalGamesLink : GameServerWithPlayers, IGameLinkService, IDisposable {
	private Dictionary<uint, IGame> games = [];
	private IGameSettings[] gameSettings;

	private INetServer server;
	private TCPServerProcessor<BasePlayer> subscriptions;
	private Func<SessionInfo, IGameSettings, IGame> gamesFactory;

	public LocalGamesLink(INetServer listenServer, IGameSettings[] settings, Func<SessionInfo, IGameSettings, IGame> gamesFactory) {
		server = listenServer;
		gameSettings = settings;
		this.gamesFactory = gamesFactory;

		subscriptions = new(
			server: listenServer,
			playersFactory: (_, player) => new(player)
		);
		subscriptions.AddMessageHandler<MessageRPC>(OnRPCRecieved);
		subscriptions.OnPlayerDisconnected += OnPlayerDisconnected;
	}

	public void Dispose() {
		subscriptions?.Dispose();
		GC.SuppressFinalize(this);
	}

	private void OutDbgInfo() {
		Logs.Log($"Players count: {players.Count}. Games count: {games.Count}");
		foreach (var player in players.Values) {
			Logs.Log($"p {player.session}:{player.game}");
		}
	}

	public async Task<IPEndPoint?> JoinServer(SessionInfo session) {
		Logs.Log($"Join server for {session}.");
		OutDbgInfo();

		if (players.TryGetValue(session.id, out var data)) {
			//player allready joined to game and still online
			//2do: disconnect player or prev player?

			//reset authorization
			data.session.SetAuthCode(session.authCode);
		} else if (games.TryGetValue(session.inviteCode, out var game)) {
			//redirect player to existing game
			players.Add(session.id, new PlayerData(game, session));
		} else {
			//new player, no games found. Create server
			return await CreateServer(session, 0);
		}
		return server.EndPoint;
	}

	public async Task<IPEndPoint?> CreateServer(SessionInfo session, uint settingsIndex) {
		Logs.Log($"Create server for {session}");
		if (games.ContainsKey(session.inviteCode)) {
			Logs.LogError($"Create game:game exist {session.inviteCode}");
			return null;
		} else {
			if (settingsIndex >= 0 && settingsIndex < gameSettings.Length) {
				return CreateGame(session, gameSettings[settingsIndex]);
			} else {
				Logs.LogError($"Unknown settings index {settingsIndex}");
				return null;
			}
		}
	}

	private IPEndPoint? CreateGame(SessionInfo session, IGameSettings settings) {
		var game = gamesFactory(session, settings);
		players.Add(session.id, new PlayerData(game, session));
		games.Add(session.inviteCode, game);

		game.OnGameFinished += OnGameFinished;
		game.OnPlayerLeft += OnPlayerLeft;

		return server.EndPoint;
	}

	private void OnPlayerLeft(IGame game, BasePlayer player) {
		Logs.Log($"Game {game.InviteCode} player {player} left");
	}

	private void OnGameFinished(IGame game) {
		Logs.Log($"Game {game.InviteCode} closed");
		games.Remove(game.InviteCode);
		//delete game
	}
}
