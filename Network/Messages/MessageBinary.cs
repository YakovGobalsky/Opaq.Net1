using System;

namespace Opaq.Network;

public class MessageBinary : IMessage<MessageBinary> {
	public static byte MessageType => 0x0B;

	private readonly byte[] data;

	public MessageBinary(byte[] array) => data = array;

	public static MessageBinary Deserialize(byte[] data) => new(data);

	public byte[] GetBytes() => data;

	public string GetJsonString() {
		throw new NotImplementedException();
	}
}
