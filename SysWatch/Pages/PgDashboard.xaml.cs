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

    private void CPUCounter_ValueObtained(object? sender, float e)
    {
        Dispatcher.Invoke(() =>
        {
            rCPU.Text = MathF.Round(e, 2).ToString();
        });
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        App.CPUCounter.ValueObtained += CPUCounter_ValueObtained;
        return;
    }

    private void Page_Unloaded(object sender, RoutedEventArgs e)
    {
        App.CPUCounter.ValueObtained -= CPUCounter_ValueObtained;
        return;
    }
}
