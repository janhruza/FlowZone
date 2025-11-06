using Microsoft.Win32;

using PassFort.Core;

using System;
using System.Windows;
using System.Windows.Threading;

namespace PassFort.Windows;

/// <summary>
/// Representing a database creation window.
/// </summary>
public partial class WndNewDatabase : Window
{
    /// <summary>
    /// Creates a new instance of the <see cref="WndNewDatabase"/> class.
    /// </summary>
    public WndNewDatabase()
    {
        InitializeComponent();

        DispatcherTimer timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(20)
        };

        timer.Tick += (s, e) =>
        {
            btnCreate.IsEnabled = (txtName.Text.Length > 0 && txtPath.Text.Length > 0);
        };

        timer.Start();
    }

    private bool _isDialog;

    /// <summary>
    /// Standard Show method but it marks this window as non-dialog.
    /// </summary>
    public new void Show()
    {
        _isDialog = false;
        base.Show();
    }

    /// <summary>
    /// Standard ShowDialog method but it marks the window as dialog.
    /// </summary>
    /// <returns></returns>
    public new bool? ShowDialog()
    {
        _isDialog = true;
        return base.ShowDialog();
    }

    private readonly string _filter = App.DB_FILTER;

    private bool CreateDatabaseFile()
    {
        // get input
        string name = txtName.Text.Trim();
        string path = txtPath.Text.Trim();

        // check name
        if (string.IsNullOrEmpty(name) == true)
        {
            _ = MessageBox.Show("Please provide a valid database name.", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        // check path
        if (string.IsNullOrEmpty(path) == true)
        {
            _ = MessageBox.Show("Please specify save location for the new database.", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        // create database
        PasswordDatabase? db = PasswordDatabase.Create(name, path);
        if (db == null)
        {
            // can't create db
            App.CriticalError("Internal error. Check the log file for more information.", nameof(CreateDatabaseFile));
            return false;
        }

        return true;
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        if (_isDialog == true)
        {
            DialogResult = false;
        }

        this.Close();
    }

    private void btnCreate_Click(object sender, RoutedEventArgs e)
    {
        if (CreateDatabaseFile() == true)
        {
            if (_isDialog == true)
            {
                DialogResult = true;
            }

            this.Close();
        }
    }

    private void btnChoose_Click(object sender, RoutedEventArgs e)
    {
        SaveFileDialog sfd = new SaveFileDialog
        {
            Filter = _filter
        };

        if (sfd.ShowDialog() == true)
        {
            txtPath.Text = sfd.FileName;
        }
    }

    /// <summary>
    /// Shows a database creation window.
    /// </summary>
    /// <returns>True, if new database is created.</returns>
    public static bool CreateDatabase()
    {
        WndNewDatabase wnd = new WndNewDatabase();
        return wnd.ShowDialog() == true;
    }
}
