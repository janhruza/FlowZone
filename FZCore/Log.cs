using System;
using System.IO;
using System.Text;

namespace FZCore;

/// <summary>
/// Representing the logging class.
/// </summary>
public static class Log
{
    /// <summary>
    /// Representing the log file path.
    /// </summary>
    public static string Path => System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.log");

    private static void WriteEntry(string key, string value, string? tag = null)
    {
        string line = $"{DateTime.Now.ToString()};{key};{(string.IsNullOrEmpty(tag) == false ? tag : "ALL")};{value}{Environment.NewLine}";
        File.AppendAllText(Path, line, Encoding.UTF8);

#if DEBUG
        Console.WriteLine(line.Trim());
#endif

        return;
    }

    /// <summary>
    /// Writes an error message into the log file.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="tag">A log message entry identifier.</param>
    public static void Error(string message, string? tag = null)
    {
        WriteEntry("ERROR", message, tag);
        return;
    }

    /// <summary>
    /// Writes an error message into the log file.
    /// </summary>
    /// <param name="ex"></param>
    /// /// <param name="tag">A log message entry identifier.</param>
    public static void Error(Exception ex, string? tag = null)
    {
        WriteEntry("ERROR", ex.ToString(), tag);
        return;
    }

    /// <summary>
    /// Writes a warning message into the log file.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="tag">A log message entry identifier.</param>
    public static void Warning(string message, string? tag = null)
    {
        WriteEntry("WARNING", message, tag);
        return;
    }

    /// <summary>
    /// Writes an informational message into the log file.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="tag">A log message entry identifier.</param>
    public static void Info(string message, string? tag = null)
    {
        WriteEntry("INFO", message, tag);
        return;
    }

    /// <summary>
    /// Writes a success message into the log file.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="tag">A log message entry identifier.</param>
    public static void Success(string message, string? tag = null)
    {
        WriteEntry("OK", message, tag);
        return;
    }

    #region Common method and log calls

    /// <summary>
    /// Logs an informational message that reads "Application has started.".
    /// </summary>
    public static void AppStarted()
    {
        Info("Application has started.", "Application_Startup");
        return;
    }

    /// <summary>
    /// Logs an informational message that reads "Application has exited.".
    /// </summary>
    public static void AppExited()
    {
        Info("Application has exited.", "Application_Exit");
        return;
    }

    /// <summary>
    /// Logs an error message and terminates the application.
    /// </summary>
    /// <param name="ex">Information about the error.</param>
    public static void Critical(Exception ex)
    {
        Error(ex);
        Environment.Exit(ex.HResult);
        return;
    }

    #endregion
}
