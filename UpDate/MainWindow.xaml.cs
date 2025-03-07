using System.Windows;
using FZCore;
using FZCore.Windows;
using UpDate.Windows;

namespace UpDate;

/// <summary>
/// Representing the main application window class.
/// This class derives from the <see cref="Window"/> class.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// Creates a new instance of the <see cref="MainWindow"/> class with default parameters.
    /// </summary>
    public MainWindow()
    {
        // init window
        InitializeComponent();

        // extend window
        WindowExtender wex = new WindowExtender(this);
        wex.AddSeparator(0x11);
        wex.AddMenuItem(0x12, "View log\tF1", () => FZCore.Core.ViewLog());

        // activate all extensions
        wex.EmpowerWindow();

        // add hooks
        this.KeyDown += (s, e) =>
        {
            if (e.Key == System.Windows.Input.Key.F2)
            {
                HandleSettings();
            }
        };
    }

    private void ApplySettings(UpDateSettings? settings)
    {
        if (settings == null)
        {
            Log.Error("Selected settings object is null.", nameof(ApplySettings));
            return;
        }

        this.Title = settings.Title;
        this.Width = settings.WindowSize.Width;
        this.Height = settings.WindowSize.Height;

        FZCore.Core.SetApplicationTheme(settings.ThemeMode);
        return;
    }

    private void miClose_Click(object sender, RoutedEventArgs e)
    {
        // close window
        this.Close();
    }

    private void miAddFeed_Click(object sender, RoutedEventArgs e)
    {
        // add new feed item
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        // check if settings exists
        if (UpDateSettings.Current != null)
        {
            UpDateSettings.Current.WindowSize = new Size(this.Width, this.Height);
        }
    }

    private void miResetSettings_Click(object sender, RoutedEventArgs e)
    {
        // reset current settings object into the default state
        UpDateSettings.Current = new UpDateSettings();
        ApplySettings(UpDateSettings.Current);
    }

    private void HandleSettings()
    {
        WndSettings settings = new WndSettings();
        _ = settings.ShowDialog();

        if (settings.Result == true)
        {
            ApplySettings(UpDateSettings.Current);
        }
    }

    private void miSettings_Click(object sender, RoutedEventArgs e)
    {
        HandleSettings();
    }
}