using Opaq.Network;

namespace Opaq.Server.Login;

public class LoginAuthenticator {
	public async Task<bool> ProcessMessage(BasePlayer player, MessageRPC msg) {
		if (msg.fn == "auth" && await TryAuthorize(player, msg.data)) {
			Logs.Log($"Auth success {player}");
			return true;
		} else {
			Logs.LogWarning($"Auth failed at fn:{msg.fn}");
			return false;
		}
	}

	protected virtual async Task<bool> TryAuthorize(BasePlayer player, Dictionary<string, string>? data) {
		return false;
	}

}
