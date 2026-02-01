using FZCore;
using FZCore.Extra;

using System.Windows;

using SysWatch.Counters;
using SysWatch.Pages;

namespace SysWatch;

/// <summary>
/// Representing the main application class.
/// </summary>
public partial class App : BaseApplication
{
    #region Page definitions

    /// <summary>
    /// Gets a new instance of the PgDashboard page definition.
    /// </summary>
    public static PgDashboard PgDashboard => new PgDashboard();

    #endregion

    #region Performance counters

    /// <summary>
    /// Gets a new instance of the CPU performance counter.
    /// </summary>
    /// <remarks>Use this property to access CPU usage metrics for performance monitoring purposes. Each call
    /// returns a new instance; reuse the instance if you require consistent measurements over time.</remarks>
    public static CPUCounter CPUCounter { get; } = new CPUCounter();

    #endregion

    /// <summary>
    /// Initializes a new instance of the App class and sets the application's main window.
    /// </summary>
    /// <remarks>This constructor is typically called by the application framework during startup. It creates
    /// and assigns the main window for the application, which serves as the primary user interface.</remarks>
    public App()
    {
        MainWindow = new MainWindow();
    }

    /// <inheritdoc />
    public new MainWindow MainWindow { get; set; }
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        // start the counters
        App.CPUCounter.Start();

        // show the main window
        MainWindow.Show();
        Log.AppStarted();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        Log.AppExited();
    }
}