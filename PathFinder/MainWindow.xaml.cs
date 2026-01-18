using FZCore.Win32;
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
        CtlFVDetails ctl = new CtlFVDetails
        {
            BorderThickness = new Thickness(0)
        };

        ctl.SortFoldersFirst = true;
        ctl.FolderChanged += (s, folderPath) =>
        {
            SetStatusMessage(folderPath);
        };

        ctlView = ctl;
        gdContent.Children.Add(ctlView);
        Grid.SetColumn(ctlView, 1);
    }

    private CtlFolderViewBase ctlView;

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

    private async Task SetFavoriteFolders()
    {
        tvFavorites.Items.Clear();
        foreach (var entry in GetFavoriteFolders())
        {
            TreeViewItem tvi = new TreeViewItem
            {
                Header = entry.Key,
                Tag = entry.Value
            };

            tvi.Selected += (s, e) =>
            {
                CtlFVDetails ctl = (CtlFVDetails)ctlView;
                ctl.OpenFolder(entry.Value);
            };

            tvFavorites.Items.Add(tvi);
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
            sbStatusText.Content = string.Empty;
            statusBar.Visibility = Visibility.Collapsed;
            return;
        }

        else
        {
            sbStatusText.Content = message;
            statusBar.Visibility= Visibility.Visible;
            return;
        }
    }

    private async Task LoadDrives()
    {
        tvDrives.Items.Clear();
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

                ti.Selected += (s, e) =>
                {
                    CtlFVDetails ctl = (CtlFVDetails)ctlView;
                    ctl.OpenFolder(di.Name);
                };

                tvDrives.Items.Add(ti);
            }
        }
    }

    private async void IconlessWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        await SetStatusMessage(string.Empty);
        await LoadDrives();
        await SetFavoriteFolders();
    }

    bool consoleOpen = false;
    private void IconlessWindow_DevToolsKeyPressed(object sender, EventArgs e)
    {
        if (consoleOpen == false)
        {
            WinAPI.AllocConsole();
            Console.Title = this.Title;
            consoleOpen = true;
        }

        else
        {
            WinAPI.FreeConsole();
            consoleOpen = false;
        }
    }
}