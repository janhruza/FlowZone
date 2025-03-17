using System.Windows;
using FZCore;

namespace HashHive;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        // log app startup
        Log.AppStarted();

        // create main window
        MainWindow mw = new MainWindow();
        MainWindow = mw;
        MainWindow.Show();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        // log app exit
        Log.AppExited();
    }
}

