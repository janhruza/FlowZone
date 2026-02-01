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

    /// <summary>
    /// Gets a shared instance of the RAMCounter used to monitor system memory usage.
    /// </summary>
    /// <remarks>This property provides a singleton RAMCounter that can be used throughout the application to
    /// retrieve memory statistics. The instance is thread-safe and intended for global access.</remarks>
    public static RAMCounter RAMCounter { get; } = new RAMCounter();

    /// <summary>
    /// Gets a static counter that provides disk usage statistics for the system drive.
    /// </summary>
    public static DiskUsageCounter DriveCounter { get; } = new DiskUsageCounter();

    /// <summary>
    /// Gets the global GPU counter instance used to monitor GPU-related metrics.
    /// </summary>
    /// <remarks>This property provides access to a shared GPUCounter object for tracking GPU performance
    /// statistics across the application. The returned instance is thread-safe and intended for use throughout the
    /// application's lifetime.</remarks>
    public static GPUCounter GPUCounter { get; } = new GPUCounter();

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
        ICounter[] counters = [CPUCounter, RAMCounter, DriveCounter, GPUCounter];
        foreach (ICounter counter in counters)
        {
            counter.Start();
        }

        // show the main window
        MainWindow.Show();
        Log.AppStarted();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        Log.AppExited();
    }
}