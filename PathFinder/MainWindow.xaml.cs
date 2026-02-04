using FZCore;
using FZCore.Windows;

using PathFinder.Controls;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PathFinder;

/// <summary>
/// Representing the main application window.
/// </summary>
public partial class MainWindow : IconlessWindow
{
    /// <summary>
    /// Creates a new <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();

        // init the theme items array
        this._themeItems = [this.miThemeDefault, this.miThemeLight, this.miThemeDark, this.miThemeSystem];

        CtlFVDetails ctl = new CtlFVDetails
        {
            BorderThickness = new Thickness(0)
        };

        ctl.SortFoldersFirst = true;
        ctl.FolderChanged += async (s, folderPath) =>
        {
            await SetStatusMessage(folderPath);
        };

        this.ctlView = ctl;
        _ = this.gdContent.Children.Add(this.ctlView);
        Grid.SetColumn(this.ctlView, 1);
    }

    private CtlFolderViewBase ctlView;

    private MenuItem[] _themeItems;

    private Dictionary<string, string> GetFavoriteFolders()
    {
        return new Dictionary<string, string>
        {
            { "Desktop", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) },
            { "Downloads", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads") },
            { "Documents", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) },
            { "Pictures", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) },
            { "Music", Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) },
            { "Videos", Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) }
        };
    }

    private void DeselectDriveItems()
    {
        foreach (object obj in this.tvDrives.Items)
        {
            if (obj is TreeViewItem ti)
            {
                ti.IsSelected = false;
            }
        }
    }

    private void DeselectFavoriteItems()
    {
        foreach (object obj in this.tvFavorites.Items)
        {
            if (obj is TreeViewItem ti)
            {
                ti.IsSelected = false;
            }
        }
    }

    private async Task SetFavoriteFolders()
    {
        this.tvFavorites.Items.Clear();
        foreach (var entry in GetFavoriteFolders())
        {
            TreeViewItem tvi = new TreeViewItem
            {
                Header = entry.Key,
                Tag = entry.Value,
                FontSize = SystemFonts.StatusFontSize
            };

            tvi.Selected += async (s, e) =>
            {
                CtlFVDetails ctl = (CtlFVDetails)this.ctlView;
                _ = await ctl.OpenFolder(entry.Value);

                // deselect all drive items
                DeselectDriveItems();
            };

            _ = this.tvFavorites.Items.Add(tvi);
        }
    }

    /// <summary>
    /// Sets the status message.
    /// </summary>
    /// <param name="message">New status message.</param>
    /// <remarks>
    /// If the <paramref name="message"/> is set to <see cref="string.Empty"/>, it hides the status bar entirely.
    /// </remarks>
    public async Task SetStatusMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            this.sbStatusText.Content = string.Empty;
            this.statusBar.Visibility = Visibility.Collapsed;
            this.miStatusPanel.IsChecked = false;
            return;
        }

        else
        {
            this.sbStatusText.Content = message;
            this.statusBar.Visibility = Visibility.Visible;
            this.miStatusPanel.IsChecked = true;
            return;
        }
    }

    private async Task LoadDrives()
    {
        this.tvDrives.Items.Clear();
        foreach (string drive in Environment.GetLogicalDrives())
        {
            DriveInfo di = new DriveInfo(drive);
            if (di.IsReady)
            {
                TreeViewItem ti = new TreeViewItem
                {
                    Header = $"{(string.IsNullOrWhiteSpace(di.VolumeLabel) == false ? di.VolumeLabel : "Drive")} ({di.Name})",
                    FontSize = SystemFonts.StatusFontSize
                };

                ti.Selected += async (s, e) =>
                {
                    CtlFVDetails ctl = (CtlFVDetails)this.ctlView;
                    _ = await ctl.OpenFolder(di.Name);

                    // deselect favorite items
                    DeselectFavoriteItems();
                };

                _ = this.tvDrives.Items.Add(ti);
            }
        }
    }

    private async void IconlessWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        await SetStatusMessage(string.Empty);
        await LoadDrives();
        await SetFavoriteFolders();
    }

    private void IconlessWindow_DevToolsKeyPressed(object sender, EventArgs e)
    {
        if (DevConsole.IsActive)
        {
            DevConsole.CloseConsole();
        }

        else
        {
            DevConsole.OpenConsole();
        }
    }

    private void UncheckAllThemeButtons()
    {
        if (this._themeItems == null) return;

        foreach (MenuItem mi in this._themeItems)
        {
            mi.IsChecked = false;
        }

        return;
    }

    private void SetAppThemeUIWrapper(FZThemeMode themeMode)
    {
        // TODO change toggle state of the theme menu items
        FZCore.Core.SetApplicationTheme(themeMode);
        UncheckAllThemeButtons();

        switch (themeMode)
        {
            case FZThemeMode.None:
                this.miThemeDefault.IsChecked = true;
                break;

            case FZThemeMode.Light:
                this.miThemeLight.IsChecked = true;
                break;

            case FZThemeMode.Dark:
                this.miThemeDark.IsChecked = true;
                break;

            case FZThemeMode.System:
                this.miThemeSystem.IsChecked = true;
                break;
        }

        return;
    }

    private void miClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void miAbout_Click(object sender, RoutedEventArgs e)
    {
        _ = FZCore.Core.AboutBox();
    }

    private void miThemeDark_Click(object sender, RoutedEventArgs e)
    {
        SetAppThemeUIWrapper(FZThemeMode.Dark);
    }

    private void miThemeSystem_Click(object sender, RoutedEventArgs e)
    {
        SetAppThemeUIWrapper(FZThemeMode.System);
    }

    private void miThemeDefault_Click(object sender, RoutedEventArgs e)
    {
        SetAppThemeUIWrapper(FZThemeMode.None);
    }

    private void miThemeLight_Click(object sender, RoutedEventArgs e)
    {
        SetAppThemeUIWrapper(FZThemeMode.Light);
    }

    private void miStatusPanel_Checked(object sender, RoutedEventArgs e)
    {
        this.statusBar.Visibility = Visibility.Visible;
    }

    private void miStatusPanel_Unchecked(object sender, RoutedEventArgs e)
    {
        this.statusBar.Visibility = Visibility.Collapsed;
    }
}