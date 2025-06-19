using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using FZCore;
using WebPeek.Core;
using WebPeek.Pages;

namespace WebPeek;

/// <summary>
/// Representing the application class.
/// </summary>
public partial class App : Application
{
    internal const string FONT_DISPLAY_NAME = "Segoe Fluent Icons";
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        FZCore.Log.AppStarted();

        if (e.Args.Length != 1)
        {
            // startup initialization
            AppManager.ImportApps();
            FZCore.Core.SetApplicationTheme(FZThemeMode.System);

            // show main window
            MainWindow mw = new MainWindow();
            MainWindow = mw;
            MainWindow.Show();
        }

        else
        {
            // command line argument handling
            // cargument is expected to be a URL address of the site to visit
            string addr = e.Args.First().Trim();

            // create main window
            MainWindow mw = new MainWindow();
            FZCore.Core.SetApplicationTheme(FZThemeMode.System);

            // create content page
            PgWebView pg = new PgWebView(addr);

            mw.Loaded += (s, e) =>
            {
                WebPeek.MainWindow.SetActivePage(pg);
            };

            // display main window with the web view page
            MainWindow = mw;
            MainWindow.Show();
        }
    }

    private static void ClearCookies()
    {
        try
        {
            // clear stored cookies
            // get cookies folder path
            string path = $"{Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly()?.Location)}.exe.WebView2";

            if (Directory.Exists(path) == true)
            {
                Directory.Delete(path, true);
                Log.Info($"Cleared cookies from {path}", nameof(ClearCookies));
            }

            return;
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(ClearCookies));
            FZCore.Core.InfoBox("An error occurred while clearing cookies. Please try again later.", "WebPeek");
            return;
        }
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        // on app close cleanup
        AppManager.ExportApps();

        // clear cookies
        ClearCookies();

        Log.AppExited();
        return;
    }
}
