using System.Windows;
using System.Windows.Controls;
using PassFort.Windows;

namespace PassFort.Pages;

/// <summary>
/// Representing the home page.
/// </summary>
public partial class PgHome : Page, IPassFortPage
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgHome"/> class.
    /// </summary>
    public PgHome()
    {
        InitializeComponent();

        this.Loaded += (s, e) =>
        {
            // load the page UI
            this.ReloadUI();
        };
    }

    private bool _isLocked = false;

    /// <summary>
    /// Determines whethwer the page is locked.
    /// </summary>
    public bool IsLocked => _isLocked;

    /// <summary>
    /// Locks the UI.
    /// </summary>
    public void Lock()
    {
        // nothing, home page is always open
        _isLocked = true;
        return;
    }

    /// <summary>
    /// Reloads the UI.
    /// </summary>
    public void ReloadUI()
    {
        return;
    }

    /// <summary>
    /// Unlocks the UI.
    /// </summary>
    public void Unlock()
    {
        // nothing, the home page is always open
        _isLocked = false;
        return;
    }

    private void ReloadHistory()
    {
        if (_isLocked == true)
        {
            // page has been locked
            return;
        }

        lbHistory.Items.Clear();
        return;
    }

    private void CreateDatabase()
    {
        if (_isLocked == true)
        {
            // page is locked
            return;
        }

        WndCreateDatabase.CreateNewDatabase();
        return;
    }

    private void btnNewDatabase_Click(object sender, RoutedEventArgs e)
    {
        CreateDatabase();
    }

    private void btnReload_Click(object sender, RoutedEventArgs e)
    {
        ReloadHistory();
    }

    #region Static code

    private static PgHome? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="PgHome"/> class.
    /// </summary>
    public static PgHome Instance => _instance ??= new PgHome();

    #endregion
}
