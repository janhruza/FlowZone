using FZCore;
using FZCore.Extra;

using System.Windows;

namespace UpDate;

/// <summary>
/// Representing the main application class.
/// This class contains the application entry point - the <see cref="Application_Startup(object, StartupEventArgs)"/> method.
/// </summary>
public partial class App : BaseApplication
{
    /// <summary>
    /// Representing the pure main window title.
    /// </summary>
    public const string AppTitle = "UpDate";

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        // loads the settings
        UpDateSettings.Current = UpDateSettings.EnsureSettings();

        // set application theme
        FZCore.Core.SetApplicationTheme(UpDateSettings.Current.ThemeMode);

        // create main window with the properties
        // set according to the settings values
        MainWindow mw = new()
        {
            Title = UpDateSettings.Current.Title,
            Width = UpDateSettings.Current.WindowSize.Width,
            Height = UpDateSettings.Current.WindowSize.Height
        };

        MainWindow = mw;
        MainWindow.Show();

        Log.Info("App started.", nameof(Application_Startup));
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        // save loaded settings
        UpDateSettings.Current?.SaveAsDefault();

        Log.Info("App exited.", nameof(Application_Exit));
        return;
    }
}

