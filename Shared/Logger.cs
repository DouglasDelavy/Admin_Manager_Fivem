using System;
#if CLIENT
using CitizenFX.Core;
#endif

public static class Logger
{
    public static void LogDebug(string message)
    {
        LoggerWriteLine("Info", message, ConsoleColor.Green);
    }

    public static void LogInformation(string message)
    {
        LoggerWriteLine("Info", message, ConsoleColor.Blue);
    }

    public static void LogWarning(string message)
    {
        LoggerWriteLine("Warn", message, ConsoleColor.Yellow);
    }

    public static void LogError(string message)
    {
        LoggerWriteLine("Error", message, ConsoleColor.Red);
    }

    public static void LogError(Exception exception, string message = null)
    {
        LoggerWriteLine("Error", $"{message}\r\n{exception}", ConsoleColor.Red);
    }

    private static void LoggerWriteLine(string title, string message, ConsoleColor color)
    {
        var log = $"[{title}] {message}";
#if SERVER
        Console.ForegroundColor = color;
        Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} {log}");
        Console.ResetColor();
#else
				Debug.WriteLine(log);
#endif
    }
}