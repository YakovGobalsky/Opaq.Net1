namespace Opaq.Network;

public static class MessageRPCPopup {
	public enum PopupType : int {
		INFO = 0,
		WARNING = 1,
		ERROR = 2
	}

	public static MessageRPC CreatePopupMessage(PopupType popupType, string message) {
		var msg = new MessageRPC() {
			fn = "text",
			data = new() {
				{"title", ((int)popupType).ToString()},
				{"text", message}
			}
		};
		return msg;
	}
}