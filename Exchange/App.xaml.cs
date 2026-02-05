using FZCore;
using FZCore.Extra;

using System.Globalization;

namespace Exchange;

/// <summary>
/// Representing the main application class.
/// </summary>
public partial class App : BaseApplication
{
    /// <summary>
    /// Initializes the <see cref="App"/> class.
    /// </summary>
    static App()
    {
        MainWindow = new MainWindow();
        MainWindow.ThemeMode = FZThemeMode.System;
    }

    /// <summary>
    /// Representing the input data culture.
    /// </summary>
    public static CultureInfo InputCulture => new CultureInfo("cs-CZ");

    /// <summary>
    /// Representing the window title.
    /// </summary>
    public const string Title = "Currency Exchange";

    /// <summary>
    /// Representing the main application window.
    /// </summary>
    public static MainWindow MainWindow { get; set; }

    private void BaseApplication_Startup(object sender, System.Windows.StartupEventArgs e)
    {
        MainWindow.Show();
        Log.AppStarted();
    }

    private void BaseApplication_Exit(object sender, System.Windows.ExitEventArgs e)
    {
        Log.AppExited();
    }
}

