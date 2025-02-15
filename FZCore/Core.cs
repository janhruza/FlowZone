using System.IO;
using System.Windows;
using FZCore.Windows;

namespace FZCore;

/// <summary>
/// Representing the functions shared across various applications.
/// </summary>
public static class Core
{
    /// <summary>
    /// Shows the popup dialog box with the specified <paramref name="message"/> and window <paramref name="caption"/>.
    /// </summary>
    /// <param name="message">Text of the message box.</param>
    /// <param name="caption">Caption of the message box window.</param>
    public static void InfoBox(string message, string caption = "FZCore")
    {
        _ = MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        return;
    }

    /// <summary>
    /// Opens a new <see cref="LogViewer"/> window and displays the application log.
    /// </summary>
    /// <returns>True, if the log window is opened, otherwise false.</returns>
    public static bool ViewLog()
    {
        if (File.Exists(Log.Path) == false)
        {
            return false;
        }

        _ = new LogViewer(Log.Path).ShowDialog();
        return true;
    }
}
