using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using FZCore;
using PassFort.Core;
using PassFort.Windows;

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

        _username = string.Empty;
        _password = string.Empty;

        this.Loaded += (s, e) =>
        {
            ReloadUI();
        };
    }

    // loaded username
    private string _username;

    // loaded password
    private string _password;

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
        lbEntries.Items.Clear();
        trFolders.IsEnabled = false;
        _isLocked = true;
        return;
    }

    /// <summary>
    /// Reloads the page UI.
    /// </summary>
    public void ReloadUI()
    {
        lbEntries.Items.Clear();
        trDbAll.IsSelected = true;
    }

    /// <summary>
    /// Unlocks the page UI.
    /// </summary>
    public void Unlock()
    {
        lbEntries.Items.Clear();
        trFolders.IsEnabled = true;
        trDbAll.Header = DbFile.Current?.Name;
        _isLocked = false;
        return;
    }

    private bool FilterPasswords(PasswordCategory category)
    {
        // display all saved user password entries from
        // the given category

        // clear entries list
        lbEntries.Items.Clear();

        if (DbFile.Current == null)
        {
            // no file loaded
            Log.Error("No database file loaded.", nameof(FilterPasswords));
            return false;
        }

        List<PasswordEntry> entries = [];

        if (category == PasswordCategory.All || category == PasswordCategory.None)
        {
            // get all entries (all categories)
            entries = DbFile.Current.Entries;
        }

        else
        {
            // get only specific entries (based on filtered category)
            entries = PasswordEntry.FilterEntries(DbFile.Current.Entries, category);
        }

        if (entries.Count == 0)
        {
            // no entries
            Log.Error($"No password entries in this category: {App.NameByPasswordCategory[category]}", nameof(FilterPasswords));
            return false;
        }

        // select only from the filtered entries list
        foreach (PasswordEntry entry in entries)
        {
            // draw basic menu item
            // no custom style or template

            ListBoxItem lbi = new ListBoxItem
            {
                Tag = entry.Id,
                Content = $"{entry.Name} ({App.NameByPasswordCategory[entry.Category]})"
            };

            lbi.Selected += (s, e) =>
            {
                // set field data
                _username = entry.Username;
                _password = entry.Password;
            };

            lbEntries.Items.Add(lbi);
        }

        return true;
    }

    private void btnCopyUsername_Click(object sender, RoutedEventArgs e)
    {
        Clipboard.SetText(_username);
        return;
    }

    private void btnCopyPassword_Click(object sender, RoutedEventArgs e)
    {
        Clipboard.SetText(_password);
        return;
    }

    private void btnNewPassword_Click(object sender, RoutedEventArgs e)
    {
        if (WndCreatePassword.CreateNewPassword() == true)
        {
            ReloadUI();
        }

        return;
    }

    private void trSocialMedia_Selected(object sender, RoutedEventArgs e)
    {
        FilterPasswords(PasswordCategory.SocialMedia);
    }

    private void trEmail_Selected(object sender, RoutedEventArgs e)
    {
        FilterPasswords(PasswordCategory.Email);
    }

    private void trFinance_Selected(object sender, RoutedEventArgs e)
    {
        FilterPasswords(PasswordCategory.Finance);
    }

    private void trShopping_Selected(object sender, RoutedEventArgs e)
    {
        FilterPasswords(PasswordCategory.Shopping);
    }

    private void trGaming_Selected(object sender, RoutedEventArgs e)
    {
        FilterPasswords(PasswordCategory.Gaming);
    }

    private void trWork_Selected(object sender, RoutedEventArgs e)
    {
        FilterPasswords(PasswordCategory.Work);
    }

    private void trSchool_Selected(object sender, RoutedEventArgs e)
    {
        FilterPasswords(PasswordCategory.School);
    }

    private void trUtilities_Selected(object sender, RoutedEventArgs e)
    {
        FilterPasswords(PasswordCategory.Utilities);
    }

    private void trOther_Selected(object sender, RoutedEventArgs e)
    {
        FilterPasswords(PasswordCategory.Other);
    }

    #region Static code

    private static PgOverview? _instance;

    /// <summary>
    /// Representing the current instance of <see cref="PgOverview"/> class.
    /// </summary>
    public static PgOverview Instance => _instance ??= new PgOverview();

    #endregion

    private void trFolders_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (e.NewValue == trDbAll)
        {
            // only if the parent node is selected
            // display all password entries
            FilterPasswords(PasswordCategory.All);
        }
    }

    private void lbEntries_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (lbEntries.SelectedItem == null)
        {
            btnCopyPassword.IsEnabled = false;
            btnCopyUsername.IsEnabled = false;
        }

        else
        {
            btnCopyPassword.IsEnabled = true;
            btnCopyUsername.IsEnabled = true;
        }
    }
}
