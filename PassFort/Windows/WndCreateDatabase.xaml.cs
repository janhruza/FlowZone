using System.Windows;
using Microsoft.Win32;
using PassFort.Core;

namespace PassFort.Windows;

/// <summary>
/// Representing the new database creation window.
/// </summary>
public partial class WndCreateDatabase : Window
{
    /// <summary>
    /// Creates a new instance of <see cref="WndCreateDatabase"/> class.
    /// </summary>
    public WndCreateDatabase()
    {
        InitializeComponent();
    }

    private string dbName = string.Empty;
    private string dbPath = string.Empty;

    private bool ProcessInputData()
    {
        dbName = txtDbName.Text.Trim();
        dbPath = txtPath.Text.Trim();

        if (string.IsNullOrEmpty(dbName) == true) return false;
        if (string.IsNullOrEmpty(dbPath) == true) return false;

        DbFile dbFile = new DbFile(dbPath)
        {
            Name = dbName
        };

        // attempt to save the new file
        return DbFile.Save(dbFile);
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
        this.Close();
    }

    private void btnCreate_Click(object sender, RoutedEventArgs e)
    {
        if (ProcessInputData() == true)
        {
            // check iif history item already exists
            if (App.History.Contains(dbPath) == false)
            {
                App.History.Add(dbPath);
            }

            this.DialogResult = true;
            this.Close();
        }

        else
        {
            _ = MessageBox.Show(Messages.CantCreateDatabase, "Unable to create database", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return;
    }

    private void btnChoose_Click(object sender, RoutedEventArgs e)
    {
        SaveFileDialog sfd = new SaveFileDialog
        {
            Filter = "PassFort Database File|*.pfd|Other|*.*"
        };

        if (sfd.ShowDialog() == true)
        {
            txtPath.Text = sfd.FileName;
        }
    }

    #region Static code

    /// <summary>
    /// Shows a new database dialog.
    /// </summary>
    /// <returns>True, if new database was created, otherwise false.</returns>
    public static bool CreateNewDatabase()
    {
        WndCreateDatabase wnd = new WndCreateDatabase();
        return wnd.ShowDialog() == true;
    }

    #endregion
}
