using System.Windows;

namespace RipTide;

/// <summary>
/// Representing the main application class.
/// </summary>
public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        MainWindow mw = new MainWindow
        {
            Title = App.Title
        };

        MainWindow = mw;
        MainWindow.Show();
    }

    /// <summary>
    /// Representing the main window title
    /// </summary>
    public const string Title = "RipTide";
}
