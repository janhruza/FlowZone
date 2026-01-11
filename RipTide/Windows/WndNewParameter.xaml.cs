using FZCore.Windows;

using RipTide.Core;

using System.Media;
using System.Windows;

namespace RipTide.Windows;

/// <summary>
/// Representing a window responsible for adding new user-specified parameters.
/// </summary>
public partial class WndNewParameter : IconlessWindow
{
    /// <summary>
    /// Creates a new instance of the <see cref="WndNewParameter"/> class.
    /// </summary>
    public WndNewParameter()
    {
        InitializeComponent();

        this.Loaded += (s, e) =>
        {
            SystemSounds.Beep.Play();
        };
    }

    private string _parameter = string.Empty;

    /// <summary>
    /// Representing the user-specified paramteter.
    /// </summary>
    public string ParameterValue => _parameter;

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        _parameter = string.Empty;
        this.DialogResult = false;
        this.Close();
    }

    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
        _parameter = txtParam.Text.Trim();

        if (string.IsNullOrEmpty(_parameter) == false)
        {
            this.DialogResult = true;
            this.Close();
        }

        return;
    }

    private void txtParam_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        btnAdd.IsEnabled = txtParam.Text.Trim().Length > 0;
    }

    private void rSupportedSites_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        rSupportedSites.Foreground = SystemColors.AccentColorBrush;
    }

    private void rSupportedSites_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        rSupportedSites.Foreground = SystemColors.AccentColorLight2Brush;
    }

    private void rSupportedSites_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        App.OpenWebPage(VideoDownloader.Help);
    }
}
