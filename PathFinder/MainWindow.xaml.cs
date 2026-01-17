using FZCore.Win32;
using FZCore.Windows;

using PathFinder.Controls;

using System;
using System.IO;
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
        ctlView = new CtlFVDetails();
        ctlView.OpenFolder("C:\\");

        gdContent.Children.Add(ctlView);
        Grid.SetColumn(ctlView, 1);
    }

    private CtlFolderViewBase ctlView;

    /// <summary>
    /// Sets the status message.
    /// </summary>
    /// <param name="message">New status message.</param>
    /// <remarks>
    /// If the <paramref name="message"/> is set to <see cref="string.Empty"/>, it hides the status bar entirely.
    /// </remarks>
    public void SetStatusMessage(string message)
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

    private void LoadDrives()
    {
        tvDrives.Items.Clear();
        foreach (string drive in Environment.GetLogicalDrives())
        {
            DriveInfo di = new DriveInfo(drive);
            if (di.IsReady)
            {
                TreeViewItem ti = new TreeViewItem
                {
                    Header = $"{(string.IsNullOrWhiteSpace(di.VolumeLabel) == false ? di.VolumeLabel : "Drive")} ({di.Name})"
                };

                ti.MouseDoubleClick += (s, e) =>
                {
                    CtlFVDetails ctl = (CtlFVDetails)ctlView;
                    ctl.OpenFolder(di.Name);
                };

                tvDrives.Items.Add(ti);
            }
        }
    }

    private void IconlessWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        SetStatusMessage(string.Empty);
        LoadDrives();
    }

    bool consoleOpen = false;
    private void IconlessWindow_DevToolsKeyPressed(object sender, EventArgs e)
    {
        if (consoleOpen == false)
        {
            WinAPI.AllocConsole();
            consoleOpen = true;
        }

        else
        {
            WinAPI.FreeConsole();
            consoleOpen = false;
        }
    }
}