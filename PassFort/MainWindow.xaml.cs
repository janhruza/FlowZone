using System.Windows;

namespace PassFort;

/// <summary>
/// Repreenting the main application window.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// Ctreates a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }

    #region Static code

    private static MainWindow? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public static MainWindow Instance => _instance ??= new MainWindow();

    #endregion

    private void miClose_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}