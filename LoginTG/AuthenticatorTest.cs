using Opaq.Network;
using Opaq.Server.Login;

namespace Opaq.Server.Test;

public class AuthenticatorTest : LoginAuthenticator {
	private readonly Random random = new();

	protected override async Task<bool> TryAuthorize(BasePlayer player, Dictionary<string, string>? data) {
		if (data != null && data.ContainsKey("id") && uint.TryParse(data["id"], out uint id)) {
			if (data.TryGetValue("invite", out string? inviteText) && uint.TryParse(inviteText, out uint invite)) {
			} else {
				invite = id;
			}
			player.session = new SessionInfo(
				id: id,
				name: data["name"],
				inviteCode: invite,
				authCode: (uint)random.Next() //Returns a non-negative random integer.
			);
			return true;
		} else {
			Logs.Log($"Auth fail: {data != null}/{data?["id"]??"null"}");
			return false;
		}
	}
}
