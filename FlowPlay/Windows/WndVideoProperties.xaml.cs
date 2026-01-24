using FlowPlay.Core;

using FZCore.Windows;

using System.Threading.Tasks;

namespace FlowPlay.Windows;

/// <summary>
/// Representing a video properties window.
/// </summary>
public partial class WndVideoProperties : IconlessWindow
{
    private VideoProperties _properties;

    private async Task LoadProperties(VideoProperties properties)
    {
        rFilename.Text = properties.Path;
        rWidth.Text = properties.Width.ToString();
        rHeight.Text = properties.Height.ToString();
        rDuration.Text = $"{properties.Duration.Hours:D2}:{properties.Duration.Minutes:D2}:{properties.Duration.Seconds:D2}";
        return;
    }

    /// <summary>
    /// Creates a new <see cref="WndVideoProperties"/> instance with the given video <paramref name="properties"/> to show.
    /// </summary>
    /// <param name="properties">Target video properties.</param>
    public WndVideoProperties(ref VideoProperties properties)
    {
        InitializeComponent();
        _properties = properties;
    }

    private async void IconlessWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        await LoadProperties(_properties);
    }

    private void miCancel_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        this.Close();
    }
}
