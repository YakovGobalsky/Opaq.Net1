using System.Text.Json;
using Opaq.Network;
using Opaq.Server;

namespace Opaq.Games;

public class Lobby : IGame {
	private uint ownerId = 0;
	private Dictionary<uint, BasePlayer> players = [];
	private HashSet<uint> onlinePlayers = [];
	private IGameSettings settings;

	public uint InviteCode {get; protected set;} = 0;

	public event Action<IGame>? OnGameFinished;
	public event Action<IGame, BasePlayer>? OnPlayerLeft;

	public Lobby(SessionInfo session, IGameSettings gameSettings) {
		ownerId = session.id;
		InviteCode = session.inviteCode;
		settings = gameSettings;
	}

	public async Task ProcessRPC(BasePlayer player, MessageRPC msg) {
		if (msg.fn == "left") {
			KickPlayer(player);
		}
		Logs.Log($"Lobby::ProcessMessage {player}/{msg} {this}");
	}

	private void KickPlayer(BasePlayer player) {
		bool needUpdate = true;
		onlinePlayers.Remove(player.session.id);
		players.Remove(player.session.id);

		OnPlayerLeft?.Invoke(this, player);

		if (player.session.id == ownerId) {
			if (players.Count == 0) {
				needUpdate = false;
				OnGameFinished?.Invoke(this);
			} else {
				if (onlinePlayers.Count > 0) {
					ownerId = onlinePlayers.GetEnumerator().Current;
				} else {
					ownerId = players.GetEnumerator().Current.Value.session.id;
				}
			}
		}

		player.client?.Disconnect();
		if (needUpdate) {
			SendState();
		}
	}

	public async Task OnDisconnect(BasePlayer player) {
		onlinePlayers.Remove(player.session.id);

		SendState();
		Logs.Log($"Lobby::OnDisconnect {player} {this}");
	}

	public async Task OnJoin(BasePlayer player) {
		players[player.session.id] = player;
		onlinePlayers.Add(player.session.id);

		SendState();
		Logs.Log($"Lobby::OnJoin {player} {this}");
	}

	//2do: заменить на кастомное сообщение, убрать словари и json
	private void SendState() {
		Dictionary<string, string> state = [];
		state["fn"] = "state";
		state["count"] = $"{players.Count}";

		var plist = new List<string>();
		foreach (var player in players) {
			var info = new Dictionary<string, string> {
				["name"] = player.Value.session.name,
				["online"] = onlinePlayers.Contains(player.Key) ? "1" : "0"
			};
			plist.Add(JsonSerializer.Serialize(info));
		}
		state["players"] = JsonSerializer.Serialize(plist);

		MessageDict msg = new(state);
		foreach (var player in players.Values) {
			player.client?.Send(msg.GetBytes());
		}

		Logs.Log("Lobby::SendState");
		Logs.Log(msg.GetJsonString());
	}

	public override string ToString() {
		return $"Lobby Invite: {InviteCode}. Owner: {ownerId}. Players: {players.Count}";
	}
}
