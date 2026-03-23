using System;
using Opaq.Network;

namespace Opaq.Games;

public interface IGameSettings {
	public string Title { get; }
	public string Descr { get; }
	public int MaxPlayers { get; }

	public IGame StartGame(IList<SessionInfo> players);
}
