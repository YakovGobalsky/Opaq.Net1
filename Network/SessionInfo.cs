using System;

namespace Opaq.Network;

//2do: навести порядок. Почему SessionInfo в Network? а BasePlayer в Opaq.Server
//2do: Убрать сессию из network!
public struct SessionInfo(uint id = 0, string name = "", uint inviteCode = 0, uint authCode = 0) {
	public uint id {get; private set;} = id;
	public string name {get; private set;} = name;
	public uint inviteCode {get; private set;} = inviteCode;
	public uint authCode {get; private set;} = authCode;

	public void SetAuthCode(uint code) => authCode = code;

	public override string ToString() => $"{name}:{id}:{inviteCode}";
}
