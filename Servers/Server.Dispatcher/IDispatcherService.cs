using System.Net;
using Opaq.Network;

namespace Opaq.Server.Dispatcher;

public interface IDispatcherService:IDisposable {
	public Task<IPEndPoint?> GetServerForSession(SessionInfo session);
	//public Task<IPEndPoint?> CreateServer(SessionInfo session, uint settingsIndex);
}
