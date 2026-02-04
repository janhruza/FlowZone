using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using SysWatch.Counters;

namespace SysWatch.Pages;

/// <summary>
/// Representing the dashboard page.
/// </summary>
public partial class PgDashboard : Page
{
    /// <summary>
    /// Initializes a new instance of the PgDashboard class.
    /// </summary>
    public PgDashboard()
    {
        InitializeComponent();
    }

    private async Task SubscribeCounters()
    {
    start:
        ICounter[] counters = [App.CPUCounter, App.RAMCounter, App.DriveCounter, App.GPUCounter];
        foreach (ICounter counter in counters)
        {
            if (counter is null)
            {
                await Task.Delay(1000);
                goto start;
            }
        }

        App.CPUCounter.ValueObtained += CPUCounter_ValueObtained;
        App.RAMCounter.ValueObtained += RAMCounter_ValueObtained;
        App.DriveCounter.ValueObtained += DriveCounter_ValueObtained;
        App.GPUCounter.ValueObtained += GPUCounter_ValueObtained;
    }

    private void GPUCounter_ValueObtained(object? sender, float e)
    {
        Dispatcher.Invoke(() =>
        {
            this.rGPU.Text = MathF.Round(e, 2).ToString();
        });
    }

    private void DriveCounter_ValueObtained(object? sender, float e)
    {
        Dispatcher.Invoke(() =>
        {
            this.rIO.Text = MathF.Round(e, 2).ToString();
        });
    }

    private void RAMCounter_ValueObtained(object? sender, float e)
    {
        Dispatcher.Invoke(() =>
        {
            this.rRAM.Text = MathF.Round(e, 2).ToString();
        });
    }

    private async Task UnsubscribeCounters()
    {
        App.CPUCounter.ValueObtained -= CPUCounter_ValueObtained;
        App.RAMCounter.ValueObtained -= RAMCounter_ValueObtained;
        App.DriveCounter.ValueObtained -= DriveCounter_ValueObtained;
        App.GPUCounter.ValueObtained -= GPUCounter_ValueObtained;
    }

    private async Task RegisterCounters()
    {
        MonitorService.Register(App.CPUCounter);
        MonitorService.Register(App.RAMCounter);
        MonitorService.Register(App.GPUCounter);
        MonitorService.Register(App.DriveCounter);
    }

    private async Task UnregisterCounters()
    {
        MonitorService.Unregister(App.CPUCounter);
        MonitorService.Unregister(App.RAMCounter);
        MonitorService.Unregister(App.GPUCounter);
        MonitorService.Unregister(App.DriveCounter);
    }

    private void CPUCounter_ValueObtained(object? sender, float e)
    {
        Dispatcher.Invoke(() =>
        {
            this.rCPU.Text = MathF.Round(e, 2).ToString();
        });
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        await RegisterCounters();

        await SubscribeCounters();
        MonitorService.Start();
        return;
    }

    private async void Page_Unloaded(object sender, RoutedEventArgs e)
    {
        await UnregisterCounters();
        await UnsubscribeCounters();
        return;
    }
}
