using System;
using System.Windows;
using FZCore;

namespace PassFort;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Representing a filter used when manipulating with file dialogs.
    /// </summary>
    public const string DB_FILTER = "PassFort Database File|*.pfd|Other|*.*";

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        Log.Info("Application was executed.", nameof(Application_Startup));

        // set system style
        // including drawing of system-rendered controls
        FZCore.Core.SetApplicationTheme(FZThemeMode.System);

        MainWindow mw = new MainWindow();
        MainWindow = mw;
        MainWindow.Show();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        Log.Info("Application was terminated.", nameof(Application_Exit));
        return;
    }

    /// <summary>
    /// Handles a critical error that has been caught.
    /// </summary>
    /// <param name="message">A message that will be displayed to the user.</param>
    /// <param name="tag">Log tag attribute. Use the name of the method which failed.</param>
    public static void CriticalError(string message, string? tag)
    {
        // log event
        Log.Error(message, tag);

        // show error info
        if (MessageBox.Show($"Critical error has occurred. Message: {message} Do you want to terminate the process and report this error to Microsoft?", tag ?? "Critical Error", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.No) == MessageBoxResult.Yes)
        {
            Environment.FailFast(message);
        }

        return;
    }
}

