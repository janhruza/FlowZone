using System;
using System.Windows;
using System.Windows.Controls;

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

    private void SubscribeCounters()
    {
        App.CPUCounter.ValueObtained += CPUCounter_ValueObtained;
        App.RAMCounter.ValueObtained += RAMCounter_ValueObtained;
        App.DriveCounter.ValueObtained += DriveCounter_ValueObtained;
        App.GPUCounter.ValueObtained += GPUCounter_ValueObtained;
    }

    private void GPUCounter_ValueObtained(object? sender, float e)
    {
        Dispatcher.Invoke(() =>
        {
            rGPU.Text = MathF.Round(e, 2).ToString();
        });
    }

    private void DriveCounter_ValueObtained(object? sender, float e)
    {
        Dispatcher.Invoke(() =>
        {
            rIO.Text = MathF.Round(e, 2).ToString();
        });
    }

    private void RAMCounter_ValueObtained(object? sender, float e)
    {
        Dispatcher.Invoke(() =>
        {
            rRAM.Text = MathF.Round(e, 2).ToString();
        });
    }

    private void UnsubscribeCounters()
    {
        App.CPUCounter.ValueObtained -= CPUCounter_ValueObtained;
        App.RAMCounter.ValueObtained -= RAMCounter_ValueObtained;
        App.DriveCounter.ValueObtained -= DriveCounter_ValueObtained;
        App.GPUCounter.ValueObtained -= GPUCounter_ValueObtained;
    }

    private void CPUCounter_ValueObtained(object? sender, float e)
    {
        Dispatcher.Invoke(() =>
        {
            rCPU.Text = MathF.Round(e, 2).ToString();
        });
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        this.SubscribeCounters();
        return;
    }

    private void Page_Unloaded(object sender, RoutedEventArgs e)
    {
        this.UnsubscribeCounters();
        return;
    }
}
