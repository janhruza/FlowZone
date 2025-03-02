using System.Windows;
using FZCore;

namespace PassFort;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        Log.Info("Application was executed.", nameof(Application_Startup));

        // set system style
        // including drawing of system-rendered controls
        FZCore.Core.SetApplicationTheme(FZThemeMode.System);

        MainWindow mw = new MainWindow();
        MainWindow = mw;
        MainWindow.Show();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        Log.Info("Application was terminated.", nameof(Application_Exit));
        return;
    }
}

