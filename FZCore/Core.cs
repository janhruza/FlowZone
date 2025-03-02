using System.Collections.Generic;
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

    /// <summary>
    /// Shows the license for all the products inside of the FlowZone suite.
    /// </summary>
    /// <returns></returns>
    public static bool ShowLicense()
    {
        InfoBox(FZData.License, FZData.Solution);
        return true;
    }

#pragma warning disable WPF0001
    private static Dictionary<FZThemeMode, ThemeMode> ThemesByNames => new Dictionary<FZThemeMode, ThemeMode>
    {
        {FZThemeMode.None, ThemeMode.None },
        {FZThemeMode.Light, ThemeMode.Light },
        {FZThemeMode.Dark, ThemeMode.Dark },
        {FZThemeMode.System, ThemeMode.System }
    };

    /// <summary>
    /// Sets the desired <paramref name="theme"/> to the desired <paramref name="window"/> (<see cref="Window"/> only scope).
    /// </summary>
    /// <param name="window">Target window.</param>
    /// <param name="theme">Desired window theme.</param>
    public static void SetWindowTheme(Window? window, FZThemeMode theme)
    {
        if (window == null)
        {
            return;
        }

        window.ThemeMode = ThemesByNames[theme];
        
        return;
    }

    /// <summary>
    /// Sets the desired <paramref name="theme"/> for the entire <paramref name="application"/> (full <see cref="Application"/> scope).
    /// </summary>
    /// <param name="application">Target application.</param>
    /// <param name="theme">Desired application theme.</param>
    public static void SetApplicationTheme(Application? application, FZThemeMode theme)
    {
        if (application == null)
        {
            return;
        }

        application.ThemeMode = ThemesByNames[theme];

        if (theme == FZThemeMode.Light)
        {
            WindowExtender.EnableLightMode();
        }

        else if (theme == FZThemeMode.Dark)
        {
            WindowExtender.EnableDarkMode();
        }

        else
        {
            WindowExtender.EnableSystemMode();
        }

        return;
    }

#pragma warning restore WPF0001
}
