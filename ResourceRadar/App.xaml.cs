using System;
using System.Windows;
using ResourceRadar.Core.Authentication;

namespace ResourceRadar;

/// <summary>
/// Representing the main application class.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Representing the base window title.
    /// </summary>
    public const string Title = "Resource Radar";

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        MainWindow mw = new MainWindow();
        MainWindow = mw;
        MainWindow.Show();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        // Post exit cleanup
        return;
    }
}

