using FZCore;
using FZCore.Extra;

using System.Windows;

namespace SysWatch;

/// <summary>
/// Representing the main application class.
/// </summary>
public partial class App : BaseApplication
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        MainWindow = new MainWindow();
        MainWindow.Show();

        Log.AppStarted();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        Log.AppExited();
    }
}

