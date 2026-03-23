//

using Opaq.Network;

namespace Opaq.Games.Poker;

public class PokerSettings: IGameSettings {
	public static PokerSettings[] PredefinedSettings = [
		new() {
			Title = "8",
			Descr = "8p",
			MaxPlayers = 8
		},
		new() {
			Title = "6",
			Descr = "6p",
			MaxPlayers = 6
		},
	];

	public string Title {get; private set;} = "";

	public string Descr {get;private set;} = "";

	public int MaxPlayers {get; private set;} = 1;

	public IGame StartGame(IList<SessionInfo> players) {
		throw new NotImplementedException();
	}
}
