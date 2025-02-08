using System.Windows;

namespace RipTide;

/// <summary>
/// Representing the main window class.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// Creates a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}