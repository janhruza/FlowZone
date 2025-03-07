using System.Windows;
using FZCore;

namespace UpDate;

/// <summary>
/// Representing the main application class.
/// This class contains the application entry point - the <see cref="Application_Startup(object, StartupEventArgs)"/> method.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Representing the pure main window title.
    /// </summary>
    public const string AppTitle = "UpDate";

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        // set application theme
        FZCore.Core.SetApplicationTheme(FZThemeMode.System);

        // create main window
        MainWindow mw = new MainWindow();
        MainWindow = mw;
        MainWindow.Show();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        // clear data (if needed)
        return;
    }
}

