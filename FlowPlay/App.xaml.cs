using System.Windows;

namespace FlowPlay;

/// <summary>
/// Representing the main application class.
/// </summary>
public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        // set app theme
        FZCore.Core.SetApplicationTheme(FZCore.FZThemeMode.System);

        // create and show the main window
        MainWindow mw = new MainWindow();
        MainWindow = mw;
        MainWindow.Show();
    }
}