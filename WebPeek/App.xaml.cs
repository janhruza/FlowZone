using FZCore;
using FZCore.Extra;

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

using WebPeek.Core;
using WebPeek.Pages;

namespace WebPeek;

/// <summary>
/// Representing the application class.
/// </summary>
public partial class App : BaseApplication
{
    internal const string FONT_DISPLAY_NAME = "Segoe Fluent Icons";

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        Log.AppStarted();

        if (e.Args.Length != 1)
        {
            // startup initialization
            _ = AppManager.ImportApps();

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

            // create content page
            PgWebView pg = new PgWebView(addr);

            mw.Loaded += (s, e) =>
            {
                _ = WebPeek.MainWindow.SetActivePage(pg);
            };

            // display main window with the web view page
            MainWindow = mw;
            MainWindow.Show();
        }
    }

    private static async Task ClearCookies()
    {
        try
        {
            // small hack to wait for the opened files to be closed
            await Task.Delay(100);

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
            FZCore.Core.ErrorBox($"An error occurred while clearing cookies. Please try again later.\n\nError message:\n{ex.Message}", "WebPeek");
            return;
        }
    }

    private async void Application_Exit(object sender, ExitEventArgs e)
    {
        // on app close cleanup
        _ = AppManager.ExportApps();

        // clear cookies
        await ClearCookies();

        Log.AppExited();
        return;
    }
}
