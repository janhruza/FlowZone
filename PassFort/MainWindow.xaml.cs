using FZCore.Windows;

using Microsoft.Win32;

using PassFort.Core;
using PassFort.Pages;
using PassFort.Windows;

using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;

namespace PassFort;

/// <summary>
/// Repreenting the main application window.
/// </summary>
public partial class MainWindow : IconlessWindow
{
    /// <summary>
    /// Ctreates a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();

        _affectedItems = [miSaveDatabase, miCloseDatabase, miDbAddEntry];

        int idx = 0x10;
        WindowExtender wex = new WindowExtender(this);
        wex.AddSeparator(idx++);
        wex.AddMenuItem(idx++, new ExtendedMenuItem
        {
            Header = "View Log\tF1",
            OnClick = () => FZCore.Core.ViewLog()
        });

        wex.AddMenuItem(idx++, new ExtendedMenuItem
        {
            Header = "Refresh\tF5",
            OnClick = () => RedrawUIElements()
        });

        wex.EmpowerWindow();

        this.Loaded += (s, e) =>
        {
            NavToPage(new PgDatabase());
        };
    }

    private void NavToPage(Page? pg)
    {
        if (pg == null) return;

        frmContent.Content = pg;
        this.Title = $"{pg.Title} | PassFort";
    }

    private List<MenuItem> _affectedItems;

    #region Static code

    private static MainWindow? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public static MainWindow Instance => _instance ??= new MainWindow();

    #endregion

    /// <summary>
    /// Performs a check whether a databse is opened and if so, enables all associated elements, otherwise disables those elements.
    /// </summary>
    private void RedrawUIElements()
    {
        if (PasswordDatabase.Current == null)
        {
            foreach (MenuItem mi in _affectedItems)
            {
                mi.IsEnabled = false;
            }

            this.Title = "PassFort";
            return;
        }

        bool value = true;
        foreach (MenuItem mi in _affectedItems)
        {
            mi.IsEnabled = value;
        }

        this.Title = value == true ? $"{PasswordDatabase.Current.Name} - PassFort" : "PassFort";
    }

    private void SaveDatabase()
    {
        if (PasswordDatabase.Current != null)
        {
            PasswordDatabase.Current.Save();
        }

        else
        {
            _ = MessageBox.Show(Messages.NO_DB_OPENED, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
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

    private bool OpenDatabase(string filename)
    {
        PasswordDatabase db = new PasswordDatabase(filename);
        return db.OpenArchive() == true && db.ReadMetadata() == true && db.ReadPasswordEntries(out _) == true;
    }

    private void miOpenDatabase_Click(object sender, RoutedEventArgs e)
    {
        // open a database file
        OpenFileDialog ofd = new OpenFileDialog
        {
            Filter = App.DB_FILTER
        };

        if (ofd.ShowDialog() == true)
        {
            if (OpenDatabase(ofd.FileName) == true)
            {
                this.Refresh();
            }
        }

        return;
    }

    private void miNewDatabase_Click(object sender, RoutedEventArgs e)
    {
        // TODO open create new database window
        if (WndNewDatabase.CreateDatabase() == false)
        {
            App.CriticalError("Unable to create a new database.", "DB_CREATE_FAIL");
        }
        return;
    }

    private void miSaveDatabase_Click(object sender, RoutedEventArgs e)
    {
        // saves opened database
        SaveDatabase();
        return;
    }

    private void miCloseDatabase_Click(object sender, RoutedEventArgs e)
    {
        // closes the opened database
        if (PasswordDatabase.Current != null)
        {
            PasswordDatabase.Current.CloseArchive();
        }

        this.Refresh();
    }

    private void CreateNewPassword()
    {
        // show new entry window
        if (PasswordDatabase.Current != null)
        {
            // add new password
            if (WndNewEntry.CreateEntry() == true)
            {
                // TODO refresh UI password list
                this.Refresh();
            }
        }
    }

    private void miDbAddEntry_Click(object sender, RoutedEventArgs e)
    {
        CreateNewPassword();
    }

    /// <summary>
    /// Refreshes the display of the content area, updating all visible UI elements to reflect the current state.
    /// </summary>
    /// <remarks>Call this method to ensure that any changes to the underlying data or UI controls are
    /// immediately rendered. This method is typically used after modifying content or layout to force a visual
    /// update.</remarks>
    public void Refresh()
    {
        RedrawUIElements();
        this.frmContent.Refresh();
        return;
    }

    private void miFrameRefresh_Click(object sender, RoutedEventArgs e)
    {
        this.Refresh();
    }
}