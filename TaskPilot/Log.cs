using System;
using System.IO;
using System.Text;

namespace TaskPilot;

/// <summary>
/// Representing the logging class.
/// </summary>
public static class Log
{
    /// <summary>
    /// Representing the log file path.
    /// </summary>
    public static string Path => System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.log");

    private static void WriteEntry(string key, string value)
    {
        string line = $"{DateTime.Now.ToString()};{key};{value}{Environment.NewLine}";
        File.AppendAllText(Path, line, Encoding.UTF8);
        return;
    }

    /// <summary>
    /// Writes an error message into the log file.
    /// </summary>
    /// <param name="message"></param>
    public static void Error(string message)
    {
        WriteEntry("ERROR", message);
        return;
    }

    /// <summary>
    /// Writes an error message into the log file.
    /// </summary>
    /// <param name="ex"></param>
    public static void Error(Exception ex)
    {
        WriteEntry("ERROR", ex.ToString());
        return;
    }

    /// <summary>
    /// Writes a warning message into the log file.
    /// </summary>
    /// <param name="message"></param>
    public static void Warning(string message)
    {
        WriteEntry("WARNING", message);
        return;
    }

    /// <summary>
    /// Writes an informational message into the log file.
    /// </summary>
    /// <param name="message"></param>
    public static void Info(string message)
    {
        WriteEntry("INFO", message);
        return;
    }

    /// <summary>
    /// Writes a success message into the log file.
    /// </summary>
    /// <param name="message"></param>
    public static void Success(string message)
    {
        WriteEntry("OK", message);
        return;
    }
}
