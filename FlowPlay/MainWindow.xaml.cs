using System.Windows;

namespace FlowPlay;

/// <summary>
/// Representing the main application window.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// Creates a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();

        this.KeyDown += (s, e) =>
        {
            if (e.Key == System.Windows.Input.Key.Apps)
            {
                SystemCommands.ShowSystemMenu(this, this.PointToScreen(new Point(0, 0)));
            }
        };
    }
}