using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Win32;
using RipTide.Core;
using RipTide.Windows;

namespace RipTide;

/// <summary>
/// Representing the main window class.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// Creates a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(App.TimerInterval)
        };

        _timer.Tick += (s, e) =>
        {
            btnDownload.IsEnabled = VerifyFields();
        };

        _timer.Start();

        // creates an instance of a video downloader
        _downloader = new VideoDownloader();

        this.Loaded += (s, e) =>
        {
            ResetFields();
        };
    }

    private DispatcherTimer _timer;
    private VideoDownloader _downloader;

    private void ResetFields()
    {
        txtUrl.Text = string.Empty;
        txtLocation.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

        // reload available cookies browsers
        cbCookiesBrowsers.Items.Clear();

        foreach (CookiesBrowser value in Enum.GetValues(typeof(CookiesBrowser)))
        {
            ComboBoxItem cbi = new ComboBoxItem
            {
                Tag = value,
                Content = App.NameByBrowser[value]
            };

            cbi.Selected += (s, e) =>
            {
                // set cookies browser to the downloader instance
                _downloader.Cookies = value;
            };

            cbCookiesBrowsers.Items.Add(cbi);
        }

        if (cbCookiesBrowsers.Items.Count > 0)
        {
            cbCookiesBrowsers.SelectedIndex = 0;
        }
    }

    /// <summary>
    /// Verifies whether all the required fields contains valid data and if they are valid, it assigns them into the current
    /// instance of the <see cref="VideoDownloader"/> class (<see cref="_downloader"/>).
    /// </summary>
    /// <returns>True, if all required input data are valid, otherwise false.</returns>
    private bool VerifyFields()
    {
        string address = txtUrl.Text.Trim();
        string location = txtLocation.Text.Trim();

        if (string.IsNullOrEmpty(address))
        {
            // invalid URL
            return false;
        }

        if (string.IsNullOrEmpty(location))
        {
            // invalid save folder
            return false;
        }

        _downloader.Address = address;
        _downloader.Location = location;
        return true;
    }

    private void HandleVideoDownload()
    {
        if (VerifyFields() == false)
        {
            // incomplete input data
            App.ShowMessage(Messages.UnableToVerifyFields, MessageBoxImage.Error);
            return;
        }

        // rebuild additional params list
        _downloader.AdditionalParameters.Clear();
        foreach (string param in lbExtraParams.Items)
        {
            _downloader.AdditionalParameters.Add(param);
        }

        Process? pDownload;
        if (_downloader.Download(out pDownload) == false)
        {
            // unable to start download
            App.ShowMessage(Messages.UnableToStartDownload, MessageBoxImage.Error);
            return;
        }

        this.Visibility = Visibility.Hidden;
        pDownload?.WaitForExit();
        this.Visibility = Visibility.Visible;

        return;
    }

    private void SelectFolder()
    {
        OpenFolderDialog openFolderDialog = new OpenFolderDialog
        {
            Title = Messages.SelecOutputFolder,
            Multiselect = false
        };

        if (openFolderDialog.ShowDialog() == true)
        {
            txtLocation.Text = openFolderDialog.FolderName;
        }

        return;
    }

    private void AddNewParameter()
    {
        WndNewParameter wnd = new WndNewParameter();
        if (wnd.ShowDialog() == true)
        {
            // add parameter
            string param = wnd.ParameterValue;
            lbExtraParams.Items.Add(param);
        }

        return;
    }

    private void RemoveSelectedParameter()
    {
        if (lbExtraParams.Items.Count == 0) return;
        if (lbExtraParams.SelectedIndex == -1) return;

        lbExtraParams.Items.RemoveAt(lbExtraParams.SelectedIndex);
        return;
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void btnDownload_Click(object sender, RoutedEventArgs e)
    {
        HandleVideoDownload();
    }

    private void miClose_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void miSupportedSites_Click(object sender, RoutedEventArgs e)
    {
        // navigate to the web page with all the supported domains
        App.OpenWebPage(VideoDownloader.SupportedSites);
    }

    private void miMissingDownloader_Click(object sender, RoutedEventArgs e)
    {
        // show the help page
        App.ShowMissingDownloaderDialog();
    }

    private void miAbout_Click(object sender, RoutedEventArgs e)
    {
        // show the about product dialog
        App.ShowAboutDialog();
    }

    private void btnChooseLocation_Click(object sender, RoutedEventArgs e)
    {
        SelectFolder();
    }

    private void miResetFields_Click(object sender, RoutedEventArgs e)
    {
        ResetFields();
    }

    private void btnAddParam_Click(object sender, RoutedEventArgs e)
    {
        AddNewParameter();
    }

    private void btnRemoveParam_Click(object sender, RoutedEventArgs e)
    {
        RemoveSelectedParameter();
    }

    private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.F1)
        {
            App.ShowAboutDialog();
        }

        else if (e.Key == System.Windows.Input.Key.F5)
        {
            ResetFields();
        }
    }
}