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

    private bool FilterPasswords(PasswordCategory category)
    {
        // display all saved user password entries from
        // the given category

        if (DbFile.Current == null)
        {
            // no file loaded
            return false;
        }

        lbEntries.Items.Clear();
        foreach (PasswordEntry entry in DbFile.Current.Entries)
        {
            // draw basic menu item
            // no custom style or template

            ListBoxItem lbi = new ListBoxItem
            {
                Tag = entry.Id,
                Content = $"{entry.Name}"
            };

            lbEntries.Items.Add(lbi);
        }

        return true;
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

    private void trDbAll_Selected(object sender, RoutedEventArgs e)
    {

    }

    private void trSocialMedia_Selected(object sender, RoutedEventArgs e)
    {

    }

    private void trEmail_Selected(object sender, RoutedEventArgs e)
    {

    }

    private void trFinance_Selected(object sender, RoutedEventArgs e)
    {

    }

    private void trShopping_Selected(object sender, RoutedEventArgs e)
    {

    }

    private void trGaming_Selected(object sender, RoutedEventArgs e)
    {

    }

    private void trWork_Selected(object sender, RoutedEventArgs e)
    {

    }

    private void trSchool_Selected(object sender, RoutedEventArgs e)
    {

    }

    private void trUtilities_Selected(object sender, RoutedEventArgs e)
    {

    }

    private void trOther_Selected(object sender, RoutedEventArgs e)
    {

    }

    #region Static code

    private static PgOverview? _instance;

    /// <summary>
    /// Representing the current instance of <see cref="PgOverview"/> class.
    /// </summary>
    public static PgOverview Instance => _instance ??= new PgOverview();

    #endregion
}
