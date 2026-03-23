using System.Net;
using Opaq.Games;
using Opaq.Network;

namespace Opaq.Server.Games;

public class GameServerWithPlayers {
	protected class PlayerData(IGame game, SessionInfo session) {
		public IGame game = game;
		public SessionInfo session = session;
	}

	protected Dictionary<uint, PlayerData> players = [];

	protected void OnPlayerDisconnected(BasePlayer player) {
		if (players.TryGetValue(player.session.id, out var data)) {
			data.game?.OnDisconnect(player);
		}
		//if players	
	}

	protected virtual async Task OnRPCRecieved(BasePlayer player, MessageRPC incomingMessage) {
		try {
			if (players.TryGetValue(player.session.id, out var data)) {
				_ = data.game.ProcessRPC(player, incomingMessage);
			} else if (TryAuthorize(player, incomingMessage)) {
				//new player authorized
				Logs.Log($"Player authorized");
				_ = players[player.session.id].game.OnJoin(player);
			} else {
				Logs.LogWarning($"Player must authorize first: {player}");
				player.client?.Disconnect();
			}
		} catch (Exception exc) {
			Logs.LogError($"GameServerWithPlayers:OnMessageRecieved error: {exc.Message}");
			player.client?.Disconnect();
		}
	}

	//Authorize new player (check session authCode and message.Code)
	private bool TryAuthorize(BasePlayer player, MessageRPC message) {
		if (message.fn == "auth") {
			if (uint.TryParse(message.data?["id"], out var id) && uint.TryParse(message.data?["code"], out var code)) {
				if (players.TryGetValue(id, out var data)) {
					if (data.session.authCode == code) {
						player.session = data.session;
						return true;
					} else {
						Logs.LogWarning($"Wrong auth code: {data.session.authCode} vs {code}");
					}
				} else {
					Logs.LogWarning($"No games found for player: {player}");
				}
			} else {
				Logs.LogError($"Cant parse auth data: {player}/{message}/{message.data}");
			}
		}
		return false;
	}
}
