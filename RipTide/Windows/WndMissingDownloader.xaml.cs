using FZCore.Windows;

using RipTide.Core;

using System.Media;
using System.Windows;

namespace RipTide.Windows;

/// <summary>
/// Representing the missing downloader dialog window.
/// </summary>
public partial class WndMissingDownloader : IconlessWindow
{
    /// <summary>
    /// Creates a new instance of the <see cref="WndMissingDownloader"/> class.
    /// </summary>
    public WndMissingDownloader()
    {
        InitializeComponent();

        Loaded += (s, e) =>
        {
            SystemSounds.Beep.Play();
        };
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void rDownloadLink_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        this.rDownloadLink.Foreground = FZCore.Core.IsDarkModeEnabled() ? SystemColors.AccentColorLight2Brush : SystemColors.AccentColorDark2Brush;
    }

    private void rDownloadLink_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        this.rDownloadLink.Foreground = SystemColors.AccentColorBrush;
    }

    private void rDownloadLink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        App.OpenWebPage(VideoDownloader.LinkRelease);
    }
}
