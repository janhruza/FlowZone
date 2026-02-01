using FZCore;
using FZCore.Extra;

using System;
using System.Threading.Tasks;
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
    public static PgDashboard PgDashboard { get; private set; }

    #endregion

    #region Performance counters

    /// <summary>
    /// Gets a new instance of the CPU performance counter.
    /// </summary>
    /// <remarks>Use this property to access CPU usage metrics for performance monitoring purposes. Each call
    /// returns a new instance; reuse the instance if you require consistent measurements over time.</remarks>
    public static CPUCounter CPUCounter { get; private set; }

    /// <summary>
    /// Gets a shared instance of the RAMCounter used to monitor system memory usage.
    /// </summary>
    /// <remarks>This property provides a singleton RAMCounter that can be used throughout the application to
    /// retrieve memory statistics. The instance is thread-safe and intended for global access.</remarks>
    public static RAMCounter RAMCounter { get; private set; }

    /// <summary>
    /// Gets a static counter that provides disk usage statistics for the system drive.
    /// </summary>
    public static DiskUsageCounter DriveCounter { get; private set; }

    /// <summary>
    /// Gets the global GPU counter instance used to monitor GPU-related metrics.
    /// </summary>
    /// <remarks>This property provides access to a shared GPUCounter object for tracking GPU performance
    /// statistics across the application. The returned instance is thread-safe and intended for use throughout the
    /// application's lifetime.</remarks>
    public static GPUCounter GPUCounter { get; private set; }

    #endregion

    /// <inheritdoc />
    public new MainWindow MainWindow { get; set; }

    private async void Application_Startup(object sender, StartupEventArgs e)
    {
        DevConsole.OpenConsole();

        // 1. Nejdřív vytvoříme čítače na pozadí (těch 15s)
        await Task.Run(() =>
        {
            CPUCounter = new CPUCounter();
            RAMCounter = new RAMCounter();
            GPUCounter = new GPUCounter();
            DriveCounter = new DiskUsageCounter();
        });

        MonitorService.Register(CPUCounter);
        MonitorService.Register(RAMCounter);
        MonitorService.Register(GPUCounter);
        MonitorService.Register(DriveCounter);

        // 2. TEĎ jsme zpět v UI vlákně (STA). Vytvoříme stránku a okno.
        PgDashboard = new PgDashboard();
        MainWindow = new MainWindow();

        MainWindow.Show();
        MonitorService.Start();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        MonitorService.Stop();
        DevConsole.CloseConsole();
    }
}