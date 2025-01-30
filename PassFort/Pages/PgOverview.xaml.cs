using System.Windows;
using System.Windows.Controls;
using PassFort.Core;

namespace PassFort.Pages;

/// <summary>
/// Representing the password overview page
/// </summary>
public partial class PgOverview : Page, IPassFortPage
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgOverview"/> class.
    /// </summary>
    public PgOverview()
    {
        InitializeComponent();

        this.Loaded += (s, e) =>
        {
            ReloadUI();
        };
    }

    private bool _isLocked = false;

    /// <summary>
    /// Determines whether the page is locked or not.
    /// </summary>
    public bool IsLocked => _isLocked;

    /// <summary>
    /// Locks the page UI.
    /// </summary>
    public void Lock()
    {
        trFolders.IsEnabled = false;
        _isLocked = true;
        return;
    }

    /// <summary>
    /// Reloads the page UI.
    /// </summary>
    public void ReloadUI()
    {
        if (DbFile.Current == null)
        {
            Lock();
        }

        else
        {
            Unlock();
        }
    }

    /// <summary>
    /// Unlocks the page UI.
    /// </summary>
    public void Unlock()
    {
        trFolders.IsEnabled = true;
        trDbAll.Header = DbFile.Current?.Name;


        _isLocked = false;
        return;
    }

    private void btnCopyUsername_Click(object sender, RoutedEventArgs e)
    {
        return;
    }

    private void btnCopyPassword_Click(object sender, RoutedEventArgs e)
    {
        return;
    }

    private void btnNewPassword_Click(object sender, RoutedEventArgs e)
    {
        return;
    }

    #region Static code

    private static PgOverview? _instance;

    /// <summary>
    /// Representing the current instance of <see cref="PgOverview"/> class.
    /// </summary>
    public static PgOverview Instance => _instance ??= new PgOverview();

    #endregion
}
