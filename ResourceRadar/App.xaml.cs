using System;
using System.Windows;
using FZCore;
using ResourceRadar.Core.Authentication;
using ResourceRadar.Windows;

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

    /// <summary>
    /// Attempts to create a new user profile.
    /// </summary>
    /// <returns>True if new user profile was created.</returns>
    public static bool CreateNewProfile()
    {
        WndNewProfile wnd = new WndNewProfile();
        if (wnd.ShowDialog() != true)
        {
            Log.Info("Profile creation cancelled.", nameof(CreateNewProfile));
            return false;
        }

        return true;
    }
}

