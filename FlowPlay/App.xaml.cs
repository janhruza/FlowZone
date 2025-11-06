using FZCore;

using System.Windows;

namespace FlowPlay;

/// <summary>
/// Representing the main application class.
/// </summary>
public partial class App : Application
{
    internal const double SPEED_025 = 0.25;
    internal const double SPEED_050 = 0.50;
    internal const double SPEED_075 = 0.75;
    internal const double SPEED_NORMAL = 1.00;
    internal const double SPEED_125 = 1.25;
    internal const double SPEED_150 = 1.50;
    internal const double SPEED_175 = 1.75;
    internal const double SPEED_200 = 1.75;
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        // startup log
        Log.AppStarted();

        // set app theme
        FZCore.Core.SetApplicationTheme(FZThemeMode.System);

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