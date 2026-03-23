// using System;
// using Opaq.Network;

// namespace Opaq.Server.Dispatcher;

// public class DispatchersFactory {
// 	public static IDispatcherService CreateDispatcher(ServerInfo[]? servers) {
// 		IDispatcherService dispatcher;
// 		if (servers == null) {
// 			dispatcher = new LocalDispatcher(
// 				game: new("127.0.0.1", 81),
// 				commands: new("127.0.0.1", 82)
// 			);
// 		} else {
// 			dispatcher = new LocalDispatcher(
// 				game: new("127.0.0.1", 81),
// 				commands: new("127.0.0.1", 82)
// 			);
// 			foreach (var server in servers) {
// 				//dispatcher.AddDispatchers(dispatcher);
// 			}
// 		}
// 		return dispatcher;
// 	}
// }
