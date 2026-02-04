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

        Loaded += (s, e) =>
        {
            SystemSounds.Beep.Play();
        };
    }

    private string _parameter = string.Empty;

    /// <summary>
    /// Representing the user-specified paramteter.
    /// </summary>
    public string ParameterValue => this._parameter;

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        this._parameter = string.Empty;
        DialogResult = false;
        Close();
    }

    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
        this._parameter = this.txtParam.Text.Trim();

        if (string.IsNullOrEmpty(this._parameter) == false)
        {
            DialogResult = true;
            Close();
        }

        return;
    }

    private void txtParam_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        this.btnAdd.IsEnabled = this.txtParam.Text.Trim().Length > 0;
    }

    private void rSupportedSites_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        this.rSupportedSites.Foreground = SystemColors.AccentColorBrush;
    }

    private void rSupportedSites_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        this.rSupportedSites.Foreground = SystemColors.AccentColorLight2Brush;
    }

    private void rSupportedSites_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        App.OpenWebPage(VideoDownloader.Help);
    }
}
