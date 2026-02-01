using FZCore.Windows;

namespace Exchange;

/// <summary>
/// Representing the main application window.
/// </summary>
public partial class MainWindow : IconlessWindow
{
    /// <summary>
    /// Creates a new <see cref="MainWindow"/> instance.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Sets the status bar text.
    /// </summary>
    /// <param name="message">New text or <see langword="null"/>.</param>
    /// <returns>Value determining whether the status bar is visible or not.</returns>
    /// <remarks>
    /// Setting <paramref name="message"/> to <see langword="null"/> hides the status bar.
    /// </remarks>
    public bool SetStatusMessage(string? message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            sbiMessage.Content = string.Empty;
            statusBar.Visibility = System.Windows.Visibility.Collapsed;
            return false;
        }

        else
        {
            sbiMessage.Content = message;
            statusBar.Visibility = System.Windows.Visibility.Visible;
            return true;
        }
    }

    private void IconlessWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        _ = SetStatusMessage("Window is ready.");
    }
}