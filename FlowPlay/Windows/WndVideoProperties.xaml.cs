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
        this.rFilename.Text = properties.Path;
        this.rWidth.Text = properties.Width.ToString();
        this.rHeight.Text = properties.Height.ToString();
        this.rDuration.Text = $"{properties.Duration.Hours:D2}:{properties.Duration.Minutes:D2}:{properties.Duration.Seconds:D2}";
        return;
    }

    /// <summary>
    /// Creates a new <see cref="WndVideoProperties"/> instance with the given video <paramref name="properties"/> to show.
    /// </summary>
    /// <param name="properties">Target video properties.</param>
    public WndVideoProperties(ref VideoProperties properties)
    {
        InitializeComponent();
        this._properties = properties;
    }

    private async void IconlessWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        await LoadProperties(this._properties);
    }

    private void miCancel_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        Close();
    }
}
