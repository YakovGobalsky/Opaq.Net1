namespace Opaq.Network;

//2do: переписать без c#11 или заменить на flatbuffer/или ещё чего готовое
public interface IMessage<T> where T : IMessage<T> {
	static abstract byte MessageType {get;}
    static abstract T Deserialize(byte[] data);
	byte[] GetBytes();
	string GetJsonString();
}
