using System.Diagnostics;
using System.Media;

namespace FZCore.Windows.Dialogs;

/// <summary>
/// Representing the about dialog box.
/// </summary>
public partial class DlgAbout : IconlessWindow
{
    /// <summary>
    /// Creates a new <see cref="DlgAbout"/> instance.
    /// </summary>
    public DlgAbout()
    {
        InitializeComponent();
    }

    private void TransientWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        SystemSounds.Beep.Play();
    }

    private void link_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        ProcessStartInfo psInfo = new ProcessStartInfo
        {
            FileName = FZData.Website,
            UseShellExecute = true
        };

        _ = Process.Start(psInfo);
    }
}
