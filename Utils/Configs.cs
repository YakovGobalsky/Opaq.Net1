using System.Text.Json;

namespace Opaq;

public static class Configs {
	public static T? LoadConfig<T>(string? fileName) where T : new() {
		if (!string.IsNullOrEmpty(fileName)) {
			try {
				return JsonSerializer.Deserialize<T>(File.ReadAllText(fileName));
			} catch (Exception exc) {
				Console.WriteLine(exc.ToString());
			}
		}
		return default;
	}
}