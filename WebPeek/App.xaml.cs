using System.Windows;
using FZCore;
using WebPeek.Core;

namespace WebPeek;

/// <summary>
/// Representing the application class.
/// </summary>
public partial class App : Application
{
    internal const string FONT_DISPLAY_NAME = "Segoe Fluent Icons";
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        // startup initialization
        AppManager.ImportApps();
        FZCore.Core.SetApplicationTheme(FZThemeMode.System);

        // show main window
        MainWindow mw = new MainWindow();
        MainWindow = mw;
        MainWindow.Show();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        // on app close cleanup
        AppManager.ExportApps();
        return;
    }
}
