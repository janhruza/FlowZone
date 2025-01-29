using System.Windows;

namespace PassFort;

/// <summary>
/// Representing the main application class.
/// </summary>
public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        MainWindow mw = new MainWindow();
        MainWindow = mw;
        MainWindow.Show();
    }

    /// <summary>
    /// Representing the app window title.
    /// </summary>
    public const string Title = "PassFort";
}
