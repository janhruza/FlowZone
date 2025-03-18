using System.Windows;
using System.Windows.Input;
using FZCore;

namespace FlowPlay;

/// <summary>
/// Representing the main application class.
/// </summary>
public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        // startup log
        Log.AppStarted();

        // set app theme
        FZCore.Core.SetApplicationTheme(FZCore.FZThemeMode.System);

        // create and show the main window
        MainWindow mw = new MainWindow();
        MainWindow = mw;
        MainWindow.Show();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        // exit log
        Log.AppExited();
    }
}