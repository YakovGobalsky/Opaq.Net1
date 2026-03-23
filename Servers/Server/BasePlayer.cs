using Opaq.Network;

namespace Opaq.Server;

//2do: упразднить Opaq.Server. Перенести куда-нить. Или сюда притащить SessionInfo
public class BasePlayer {
	public SessionInfo session;
	public INetClient? client;

	public BasePlayer(INetClient source) {
		session = new();
		client = source;
	}

	public override string ToString() => $"{session}";
}
