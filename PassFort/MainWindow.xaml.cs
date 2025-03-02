using System.Windows;
using FZCore.Windows;

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

        int idx = 0x10;
        WindowExtender wex = new WindowExtender(this);
        wex.AddSeparator(idx++);
        wex.AddMenuItem(idx++, new ExtendedMenuItem
        {
            Header = "View Log",
            OnClick = () => FZCore.Core.ViewLog()
        });
    }

    #region Static code

    private static MainWindow? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public static MainWindow Instance => _instance ??= new MainWindow();

    #endregion

    private void SaveDatabase()
    {

    }

    private void miClose_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void miABoutPassFort_Click(object sender, RoutedEventArgs e)
    {
        // TODO show about box
        return;
    }

    private void miAboutFlowZone_Click(object sender, RoutedEventArgs e)
    {
        // open project repository
        FZCore.Core.OpenWebPage(FZCore.Core.FlowZoneUrl);
        return;
    }

    private void miOpenDatabase_Click(object sender, RoutedEventArgs e)
    {
        // TODO open database file
        return;
    }

    private void miNewDatabase_Click(object sender, RoutedEventArgs e)
    {
        // TODO open create new database window
        return;
    }

    private void miSaveDatabase_Click(object sender, RoutedEventArgs e)
    {
        // TODO save opened database
        SaveDatabase();
        return;
    }
}