using System.Windows;

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
}
