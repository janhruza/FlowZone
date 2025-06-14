using System.Windows;
using FZCore;

namespace WebPeek;

/// <summary>
/// Representing the application class.
/// </summary>
public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        // startup initialization
        Core.SetApplicationTheme(FZThemeMode.System);

        // show main window
        MainWindow mw = new MainWindow();
        MainWindow = mw;
        MainWindow.Show();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        // on app close cleanup
        return;
    }
}
