using System.Windows;
using System.Windows.Controls;
using PassFort.Core;
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
        ReloadHistory();
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

    private void OpenHistoryItem(string historyItem)
    {
        if (DbFile.Open(historyItem, out DbFile file) == true)
        {
            // file opened successfully
            DbFile.SetCurrent(file);
            MainWindow.SetContentPage(PgOverview.Instance, ref MainWindow.Instance.btnOverview);
        }

        else
        {
            _ = MessageBox.Show(Messages.CantOpenDbFile, "Unable to open database", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
    }

    private void ReloadHistory()
    {
        if (_isLocked == true)
        {
            // page has been locked
            return;
        }

        lbHistory.Items.Clear();
        foreach (string historyItem in App.History)
        {
            // item menu items
            MenuItem miOpen = new MenuItem
            {
                Header = "Open"
            };

            miOpen.Click += (s, e) =>
            {
                // open database file
                OpenHistoryItem(historyItem);
            };

            MenuItem miRemove = new MenuItem
            {
                Header = "Remove"
            };

            miRemove.Click += (s, e) =>
            {
                App.History.Remove(historyItem);
                ReloadHistory();
            };

            ListBoxItem lbi = new ListBoxItem
            {
                Content = historyItem,
                Uid = historyItem,
                ContextMenu = new ContextMenu
                {
                    Items =
                    {
                        miOpen,
                        new Separator(),
                        miRemove
                    }
                }
            };

            lbi.MouseDoubleClick += (s, e) =>
            {
                OpenHistoryItem(historyItem);
            };

            lbHistory.Items.Add(lbi);
        }

        return;
    }

    private void CreateDatabase()
    {
        if (_isLocked == true)
        {
            // page is locked
            return;
        }

        if (WndCreateDatabase.CreateNewDatabase() == true)
        {
            ReloadHistory();
        }

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
