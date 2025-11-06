using FZCore.Windows;

using Microsoft.Win32;

using PassFort.Core;
using PassFort.Pages;
using PassFort.Windows;

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PassFort;

/// <summary>
/// Repreenting the main application window.
/// </summary>
public partial class MainWindow : Window
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
            Header = "View Log",
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

    private void miOpenDatabase_Click(object sender, RoutedEventArgs e)
    {
        // open a database file
        OpenFileDialog ofd = new OpenFileDialog
        {
            Filter = App.DB_FILTER
        };

        if (ofd.ShowDialog() == true)
        {
            PasswordDatabase db = new PasswordDatabase(ofd.FileName);
            if (db.OpenArchive() == true && db.ReadMetadata() == true)
            {
                RedrawUIElements();
            }
        }

        return;
    }

    private void miNewDatabase_Click(object sender, RoutedEventArgs e)
    {
        // TODO open create new database window
        WndNewDatabase.CreateDatabase();
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

        RedrawUIElements();
    }

    private void miDbAddEntry_Click(object sender, RoutedEventArgs e)
    {
        // show new entry window
        if (PasswordDatabase.Current != null)
        {
            // add new password
            if (WndNewEntry.CreateEntry() == true)
            {
                // TODO refresh UI password list
            }
        }
    }

    private void miFrameRefresh_Click(object sender, RoutedEventArgs e)
    {
        frmContent.Refresh();
    }
}