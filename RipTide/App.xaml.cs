using RipTide.Core;
using RipTide.Windows;

using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace RipTide;

/// <summary>
/// Representing the main application class.
/// </summary>
public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        // load saved settings
        if (RTSettings.LoadCurrent() == true)
        {
            // check if custom downloader path is set
            if (string.IsNullOrEmpty(RTSettings.Current.CustomDownloaderPath) == false)
            {
                // load downloader path from the setting file
                VideoDownloader.YT_DLP_CUSTOM_PATH = RTSettings.Current.CustomDownloaderPath;
            }
        }

        // creates the main window
        MainWindow mw = new MainWindow
        {
            Title = $"{App.Title} [{VideoDownloader.GetVersion()}]"
        };

        MainWindow = mw;
        MainWindow.Show();
    }

    /// <summary>
    /// Representing the main window title
    /// </summary>
    public const string Title = "RipTide";

    /// <summary>
    /// Representing the interval (in ms) of the main window timer that is used to verify input data.
    /// </summary>
    public const int TimerInterval = 250;

    /// <summary>
    /// Shows the OK message box.
    /// </summary>
    /// <param name="message">Message to be displayed within the message box.</param>
    /// <param name="image">Specified image associated to the message box.</param>
    public static void ShowMessage(string message, DialogIcon image)
    {
        _ = new WndDialog(message, App.Title, image).ShowDialog();
        return;
    }

    /// <summary>
    /// Opens the given web page in the associated web browser.
    /// </summary>
    /// <param name="url">Target URL where the page is located at.</param>
    public static void OpenWebPage(string url)
    {
        Process proc = new Process
        {
            StartInfo =
            {
                UseShellExecute = true,
                FileName = url.Trim()
            }
        };

        proc.Start();
        return;
    }

    /// <summary>
    /// Representing the friendly names of all the supported cookies browsers.
    /// </summary>
    public static Dictionary<CookiesBrowser, string> NameByBrowser => new Dictionary<CookiesBrowser, string>
    {
        { CookiesBrowser.None, "None" },
        { CookiesBrowser.Brave, "Brave" },
        { CookiesBrowser.Chrome, "Google Chrome" },
        { CookiesBrowser.Chromium, "Google Chromium" },
        { CookiesBrowser.Edge, "Microsoft Edge" },
        { CookiesBrowser.Firefox, "Mozilla Firefox" },
        { CookiesBrowser.Opera, "Opera" },
        { CookiesBrowser.Safari, "Apple Safari" },
        { CookiesBrowser.Vivaldi, "Vivaldi" },
        { CookiesBrowser.Whale, "Whale" },
    };

#pragma warning disable WPF0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    /// <summary>
    /// Representing a dictionary of identifiers and their associated theme modes.
    /// </summary>
    public static Dictionary<RipTide.Core.ThemeMode, System.Windows.ThemeMode> ThemeById => new Dictionary<RipTide.Core.ThemeMode, System.Windows.ThemeMode>
    {
        { RipTide.Core.ThemeMode.System, System.Windows.ThemeMode.System },
        { RipTide.Core.ThemeMode.Light, System.Windows.ThemeMode.Light },
        { RipTide.Core.ThemeMode.Dark, System.Windows.ThemeMode.Dark }
    };
#pragma warning restore WPF0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

    /// <summary>
    /// Shows the about dialog.
    /// </summary>
    public static void ShowAboutDialog()
    {
        _ = new WndAbout().ShowDialog();
        return;
    }

    /// <summary>
    /// Shows the missing downloader dialog.
    /// </summary>
    public static void ShowMissingDownloaderDialog()
    {
        _ = new WndMissingDownloader().ShowDialog();
        return;
    }

    /// <summary>
    /// Handles settings specific for the whole application.
    /// </summary>
    public static void HandleAppSettings()
    {
        if (RTSettings.Current == null)
        {
            return;
        }

        // set theme mode
#pragma warning disable WPF0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        App.Current.ThemeMode = ThemeById[RTSettings.Current.ThemeMode];
#pragma warning restore WPF0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    }

    /// <summary>
    /// Handles settings specific for the <see cref="Window"/> class only.
    /// </summary>
    /// <param name="window">Target window where to apply the settings.</param>
    public static void HandleWindowSettings(Window window)
    {
        if (RTSettings.Current != null)
        {
            window.Left = RTSettings.Current.WindowPosition.X;
            window.Top = RTSettings.Current.WindowPosition.Y;
            window.Topmost = RTSettings.Current.AlwaysOnTop;
        }

        return;
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        RTSettings.SaveCurrent();
    }
}
