using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using FZCore;
using PassFort.Core;

namespace PassFort.Windows;

/// <summary>
/// Representing a password creation window.
/// </summary>
public partial class WndCreatePassword : Window
{
    /// <summary>
    /// Creates a new instance of the <see cref="WndCreatePassword"/> class.
    /// </summary>
    public WndCreatePassword()
    {
        InitializeComponent();

        this.Loaded += (s, e) =>
        {
            name = string.Empty;
            username = string.Empty;
            password = string.Empty;
            url = string.Empty;
            notes = string.Empty;
            category = PasswordCategory.None;

            // reloads UI on every load
            ReloadUI();
        };
    }

    private string name;
    private string username;
    private string password;
    private string url;
    private string notes;
    private PasswordCategory category;

    private void ReloadUI()
    {
        // add password categories
        {
            cbCategory.Items.Clear();
            foreach (KeyValuePair<PasswordCategory, string> kvp in App.NameByPasswordCategory)
            {
                // construct the category item
                ComboBoxItem cbiCategory = new ComboBoxItem
                {
                    Tag = kvp.Key,
                    Content = kvp.Value
                };

                cbiCategory.Selected += (s, e) =>
                {
                    // sets the category
                    category = kvp.Key;
                };

                // add the category item to the list of them
                cbCategory.Items.Add(cbiCategory);
            }

            if (cbCategory.Items.Count > 0)
            {
                cbCategory.SelectedIndex = 0;
            }
        }
    }

    private bool ValidateInputData()
    {
        name = txtName.Text.Trim();                             // mandatory
        username = txtUsername.Text.Trim();                     // mandatory
        password = txtPassword.Password.Trim();                 // mandatory
        string confirm = txtConfirmPassword.Password.Trim();    // mandatory
        url = txtUrl.Text.Trim();                               // optional
        notes = txtNotes.Text.Trim();                           // optional

        if (string.IsNullOrEmpty(name) == true) return false;
        if (string.IsNullOrEmpty(username) == true) return false;
        if (string.IsNullOrEmpty(password) == true) return false;
        if (string.Equals(password, confirm) == false) return false;

        return true;
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
        this.Close();
        return;
    }

    private void btnCreate_Click(object sender, RoutedEventArgs e)
    {
        if (DbFile.Current == null)
        {
            // no database file opened
            _ = MessageBox.Show(Messages.NoDatabaseLoaded, "No database", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // check if input data is valid
        if (ValidateInputData() == false)
        {
            // unable to validate password entry
            _ = MessageBox.Show(Messages.CanValidatePassword, "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // data is valid, create a new entry
        PasswordEntry entry = new PasswordEntry
        {
            Name = name,
            Url = url,
            Username = username,
            Password = password,
            Notes = notes,
            Category = category
        };

        // additional validation check
        if (PasswordEntry.IsEntryValid(entry) == false)
        {
            // data is somehow invalid
            Log.Error($"Password entry is not valid. Source: {PasswordEntry.IsEntryValid}", nameof(btnCreate_Click));
            _ = MessageBox.Show(Messages.CanValidatePassword, "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // add created password entry into the
        // database password entries list
        DbFile.Current.Entries.Add(entry);

        // save the passwords file
        DbFile.Save(DbFile.Current);

        // close window with success
        this.DialogResult = true;
        this.Close();
        return;
    }

    #region Static code

    /// <summary>
    /// Shows a pop-up dialog for a new password creation.
    /// </summary>
    /// <returns>True, if a new password for oaded database is created, otherwise false.</returns>
    public static bool CreateNewPassword()
    {
        try
        {
            // creates a new window instance
            WndCreatePassword wnd = new WndCreatePassword();

            // processes user input and validates it
            // all the magic is happening here
            return wnd.ShowDialog() == true;
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(WndCreatePassword.CreateNewPassword));
            return false;
        }
    }

    #endregion
}
