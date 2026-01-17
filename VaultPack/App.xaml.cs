using FZCore.Extra;

using System.Windows;

namespace VaultPack;

/// <summary>
/// Representing the main application class.
/// </summary>
public partial class App : BaseApplication
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        MainWindow mw = new MainWindow();
        MainWindow = mw;
        MainWindow.Show();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        return;
    }
}

