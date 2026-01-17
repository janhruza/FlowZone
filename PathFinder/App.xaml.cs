using FZCore.Extra;

namespace PathFinder;

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
    }

    private void BaseApplication_Exit(object sender, System.Windows.ExitEventArgs e)
    {
        // exit cleanup
        return;
    }
}