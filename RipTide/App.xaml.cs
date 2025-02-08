using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using RipTide.Core;
using RipTide.Windows;

namespace RipTide;

/// <summary>
/// Representing the main application class.
/// </summary>
public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
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
    public static void ShowMessage(string message, MessageBoxImage image)
    {
        _ = MessageBox.Show(message, Title, MessageBoxButton.OK, image);
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

    /// <summary>
    /// SHows the about dialog.
    /// </summary>
    public static void ShowAboutDialog()
    {
        _ = new WndAbout().ShowDialog();
    }
}
