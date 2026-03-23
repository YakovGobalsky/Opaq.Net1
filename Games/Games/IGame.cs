using System;
using Opaq.Network;
using Opaq.Server;

namespace Opaq.Games;

public interface IGame {
	uint InviteCode {get;}
	Task ProcessRPC(BasePlayer player, MessageRPC msg);
	Task OnDisconnect(BasePlayer player);
	Task OnJoin(BasePlayer player);

	event Action<IGame, BasePlayer> OnPlayerLeft;
	event Action<IGame> OnGameFinished;
}
