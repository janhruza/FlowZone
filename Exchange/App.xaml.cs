using FZCore;
using FZCore.Extra;

namespace Exchange;

/// <summary>
/// Representing the main application class.
/// </summary>
public partial class App : BaseApplication
{
    private void BaseApplication_Startup(object sender, System.Windows.StartupEventArgs e)
    {
        MainWindow mw = new MainWindow();
        MainWindow = mw;
        MainWindow.Show();

        Log.AppStarted();
    }

    private void BaseApplication_Exit(object sender, System.Windows.ExitEventArgs e)
    {
        Log.AppExited();
    }
}

