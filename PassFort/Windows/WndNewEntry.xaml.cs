using System.Windows;
using FZCore;
using FZCore.Extensions;
using PassFort.Core;

namespace PassFort.Windows;

/// <summary>
/// Representing a window for creating new password entries.
/// </summary>
public partial class WndNewEntry : Window
{
    private bool isDialog;

    /// <summary>
    /// Creates a new instance of the <see cref="WndNewEntry"/> class.
    /// </summary>
    public WndNewEntry()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Standard <see cref="Show"/> method but also marks this window as non-dialog.
    /// </summary>
    public new void Show()
    {
        isDialog = false;
        base.Show();
    }

    /// <summary>
    /// Standard <see cref="ShowDialog"/> method but also marks this window as dialog.
    /// </summary>
    /// <returns></returns>
    public new bool? ShowDialog()
    {
        isDialog = true;
        return base.ShowDialog();
    }

    private bool HandleEntry()
    {
        string title = txtTitle.Text.Trim();            // required
        string address = txtAddress.Text.Trim();        // optional
        string username = txtUsername.Text.Trim();      // optional
        string password = txtPassword.Password.Trim();  // required
        string notes = txtNotes.Text.Trim();            // optional

        string[] requiredFields = [title, password];

        foreach (string required in requiredFields)
        {
            if (string.IsNullOrEmpty(required) == true)
            {
                // required field is null
                this.ShowMessage("Not all required fields contain valid data. Please fill the required boxes.", MessageBoxImage.Error);
                return false;
            }
        }

        PasswordEntry entry = new PasswordEntry
        {
            Title = title,
            Address = address,
            Username = username,
            Password = password,
            Notes = notes
        };

        // check before writing
        if (PasswordDatabase.Current == null)
        {
            // no DB is opened
            Log.Error(Messages.NO_DB_OPENED, nameof(HandleEntry));
            this.ShowMessage(Messages.NO_DB_OPENED, MessageBoxImage.Error);
            return false;
        }

        // adds the entry to the list
        PasswordDatabase.Current.Entries.Add(entry);
        Log.Info("New password entry was created.", nameof(HandleEntry));
        return true;
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        if (isDialog)
        {
            DialogResult = false;
        }

        this.Close();
    }

    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
        if (HandleEntry() == true)
        {
            if (isDialog)
            {
                DialogResult = true;
            }

            this.Close();
        }
    }

    /// <summary>
    /// SHows the new entry dialog box.
    /// </summary>
    /// <returns>True, if new entry is added to loaded database, otherwise false.</returns>
    public static bool CreateEntry()
    {
        return new WndNewEntry().ShowDialog() == true;
    }
}
