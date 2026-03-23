using System.Text.Json;

namespace Opaq.Network;

[System.Serializable]
public class MessageRPC : IMessage<MessageRPC> {
	public static byte MessageType => 0xEC;

	public string? fn { get; set; }
	public Dictionary<string, string>? data { get; set; }

	public static MessageRPC Deserialize(byte[] data) => new() { fn = "", data = [] };

	public string GetJsonString() => JsonSerializer.Serialize(this);

	public byte[] GetBytes() {
		throw new NotImplementedException();
	}

}
