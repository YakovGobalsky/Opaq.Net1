namespace Opaq;

//2do: replace with ILogger/Opentelemetry
//read: https://github.com/open-telemetry/opentelemetry-dotnet#getting-started
public static class Logs {
	public static void Log(string message) {
		Console.BackgroundColor = ConsoleColor.Black;
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine(message);
	}
	public static void LogError(string message) {
		Console.BackgroundColor = ConsoleColor.Red;
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine($"Error: {message}");
		Console.WriteLine(new System.Diagnostics.StackTrace().ToString());
	}
	public static void LogWarning(string message) {
		Console.BackgroundColor = ConsoleColor.Black;
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine($"Warning: {message}");
	}
}