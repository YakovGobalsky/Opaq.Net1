using System.Net;
using Opaq.Network;

namespace Opaq.Games;

public interface IGameLinkService {
	public Task<IPEndPoint?> JoinServer(SessionInfo session);
	//public Task<IPEndPoint?> CreateServer(SessionInfo session, uint settingsIndex);
}
