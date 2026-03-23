using System.Text.Json;

namespace Opaq.Network;

public class MessageDict(Dictionary<string, string> dict) : IMessage<MessageDict> {
	private readonly Dictionary<string, string>? data = dict;

	public static byte MessageType => 0x0D;

	public static MessageDict Deserialize(byte[] data) {
		return new ([]);
	}

	public byte[] GetBytes() {
		throw new NotImplementedException();
	}

	public string GetJsonString() => JsonSerializer.Serialize(data);
}
