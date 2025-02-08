using System.Media;
using System.Windows;

namespace RipTide.Windows;

/// <summary>
/// Representing the about RipTide dialog window.
/// </summary>
public partial class WndAbout : Window
{
    /// <summary>
    /// Create a new instance of the <see cref="WndAbout"/> class.
    /// </summary>
    public WndAbout()
    {
        InitializeComponent();

        this.Loaded += (s, e) =>
        {
            SystemSounds.Beep.Play();
        };
    }

    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
