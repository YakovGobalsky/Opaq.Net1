using System.Net;
using System.Text.Json;

namespace Opaq.Network;

[System.Serializable]
public class MessageRedirect(IPEndPoint server, SessionInfo session) : IMessage<MessageRedirect> {
	public static byte MessageType => 0x1D;

	public string fn { get; set; } = "redirect";
	public string ip { get; set; } = server.Address.ToString();
	public int port {get; set;} = server.Port;
	public uint code {get; set;} = session.authCode;
	public uint invite {get; set;} = session.inviteCode;

	public static MessageRedirect Deserialize(byte[] data) {
		throw new NotImplementedException();
	}

	public byte[] GetBytes() {
		throw new NotImplementedException();
	}

	public string GetJsonString() => JsonSerializer.Serialize(this);
}
