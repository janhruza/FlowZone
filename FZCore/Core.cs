using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using FZCore.Windows;
using Microsoft.Win32;

namespace FZCore;

/// <summary>
/// Representing the functions shared across various applications.
/// </summary>
public static class Core
{
    /// <summary>
    /// Representing the URL of the FlowZone project.
    /// </summary>
    public const string FlowZoneUrl = $"https://www.github.com/janhruza/FlowZone";

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
    /// Shows the popup dialog box with the specified <paramref name="message"/> and window <paramref name="caption"/>.
    /// </summary>
    /// <param name="message">Text of the message box.</param>
    /// <param name="caption">Caption of the message box window.</param>
    public static void ErrorBox(string message, string caption = "FZCore")
    {
        _ = MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
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

    /// <summary>
    /// Determines whether the user's Windows customization is set to dark mode.
    /// </summary>
    /// <returns>True, if the dark mode is enabled in settings, otherwise false. Also returns false if an exception is raised.</returns>
    public static bool IsDarkModeEnabled()
    {
        const string registryKey = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        const string valueName = "AppsUseLightTheme";

        try
        {
            using (var key = Registry.CurrentUser.OpenSubKey(registryKey))
            {
                if (key != null && key.GetValue(valueName) is int value)
                {
                    return value == 0; // 0 means dark mode
                }
            }
        }
        catch
        {
            // Handle exceptions (e.g., lack of permissions)
        }

        // Default to light mode if the value cannot be determined
        return false;
    }

    /// <summary>
    /// Opens the specified <paramref name="url"/> using the default web browser.
    /// </summary>
    /// <param name="url">Target URL address of the site to visit.</param>
    public static void OpenWebPage(string url)
    {
        _ = Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true // Must be true to open the URL with the default web browser
        });

        return;
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
    /// Also draws the system elements (menu, etc.) using the corresponding theme.
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

        else if (theme == FZThemeMode.System)
        {
            if (IsDarkModeEnabled() == true)
            {
                WindowExtender.EnableDarkMode();
            }

            else
            {
                WindowExtender.EnableLightMode();
            }
        }

        else
        {
            WindowExtender.EnableSystemMode();
        }
        return;
    }

    /// <summary>
    /// Sets the desired <paramref name="theme"/> for the entire applcation (full <see cref="Application"/> scope).
    /// Also draws the system elements (menu, etc.) using the corresponding theme.
    /// </summary>
    /// <param name="theme">Desired application theme.</param>
    public static void SetApplicationTheme(FZThemeMode theme)
    {
        SetApplicationTheme(Application.Current, theme);
        return;
    }

#pragma warning restore WPF0001

    internal static BitmapImage GetImageSource(string imageName)
    {
        var uri = new Uri($"pack://application:,,,/FZCore;component/Resources/{imageName}", UriKind.Absolute);
        return new BitmapImage(uri);
    }

    /// <summary>
    /// Returns a path to the desired <paramref name="resource"/>.
    /// </summary>
    /// <param name="resource">Name of the resource inside of the 'Resources' folder.</param>
    /// <returns>Full path to the desired resource as <see cref="string"/>.</returns>
    public static string GetResource(string resource)
    {
        return $"pack://application:,,,/FZCore;component/Resources/{resource}";
    }

    /// <summary>
    /// Sets both <see cref="CultureInfo.CurrentCulture"/> and <see cref="CultureInfo.CurrentUICulture"/> from the specified <paramref name="cultureName"/> parameter.
    /// </summary>
    /// <param name="cultureName">Name of the culture. You can use the <see cref="CultureInfo.Name"/> property to get the name of the desired culture.</param>
    public static void SetCulture(string cultureName)
    {
        CultureInfo cu = new CultureInfo(cultureName);
        CultureInfo.CurrentCulture = cu;
        CultureInfo.CurrentUICulture = cu;
        return;
    }
}
