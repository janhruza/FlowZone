using FZCore.Win32;
using FZCore.Windows;

using Microsoft.Win32;

using RipTide.Core;
using RipTide.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace RipTide;

/// <summary>
/// Representing the main window class.
/// </summary>
public partial class MainWindow : IconlessWindow
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

        Loaded += (s, e) =>
        {
            // handle window settings
            App.HandleWindowSettings(this);

            // assign settings for this window (MainWindow specific)
            miAoT.IsChecked = RTSettings.Current.AlwaysOnTop;
            SetThemeMode(RTSettings.Current.ThemeMode, ThemeItemById[RTSettings.Current.ThemeMode]);

            // refresh all fields to their defaults
            ResetFields();
        };
    }

    private DispatcherTimer _timer;
    private VideoDownloader _downloader;

    private void ResetFields()
    {
        // reset window title
        Title = $"{App.Title} [{VideoDownloader.GetVersion()}]";

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
            App.ShowMessage(Messages.UnableToVerifyFields, DialogIcon.Error);
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
            App.ShowMessage(Messages.UnableToStartDownload, DialogIcon.Error);
            return;
        }

        Visibility = Visibility.Hidden;
        pDownload?.WaitForExit();
        Visibility = Visibility.Visible;

        return;
    }

    private async Task HandleVideoDownloadAsync()
    {
        if (VerifyFields() == false)
        {
            // incomplete input data
            App.ShowMessage(Messages.UnableToVerifyFields, DialogIcon.Error);
            return;
        }

        // rebuild additional params list
        _downloader.AdditionalParameters.Clear();
        foreach (string param in lbExtraParams.Items)
        {
            _downloader.AdditionalParameters.Add(param);
        }

        WinAPI.AllocConsole();
        await _downloader.DownloadAsync();
        WinAPI.FreeConsole();
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

    private void ResetDownloader()
    {
        VideoDownloader.YT_DLP_CUSTOM_PATH = string.Empty;
        RTSettings.Current.CustomDownloaderPath = string.Empty;
        RTSettings.SaveCurrent();
        ResetFields();
        return;
    }

    private void AddNewParameter()
    {
        WndNewParameter wnd = new WndNewParameter
        {
            Owner = this
        };

        if (wnd.ShowDialog() == true)
        {
            // add parameter
            string param = wnd.ParameterValue;
            lbExtraParams.Items.Add(param);
        }

        return;
    }

    private void SelectDownloader()
    {
        OpenFileDialog ofd = new OpenFileDialog
        {
            FileName = "yt-dlp.exe",
            Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*"
        };

        if (ofd.ShowDialog() == true)
        {
            VideoDownloader.YT_DLP_CUSTOM_PATH = ofd.FileName;
            RTSettings.Current.CustomDownloaderPath = ofd.FileName;
            RTSettings.SaveCurrent();
            ResetFields();
            VerifyFields();
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
        Close();
    }

    private async void btnDownload_Click(object sender, RoutedEventArgs e)
    {
        await HandleVideoDownloadAsync();
    }

    private void miClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void miSupportedSites_Click(object sender, RoutedEventArgs e)
    {
        // navigate to the web page with all the supported domains
        App.OpenWebPage(VideoDownloader.SupportedSites);
    }

    private void miMissingDownloader_Click(object sender, RoutedEventArgs e)
    {
        // show the help page
        App.ShowMissingDownloaderDialog(this);
    }

    private void miAbout_Click(object sender, RoutedEventArgs e)
    {
        // show the about product dialog
        App.ShowAboutDialog(this);
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

    private Dictionary<Core.ThemeMode, MenuItem> ThemeItemById => new Dictionary<Core.ThemeMode, MenuItem>()
    {
        { Core.ThemeMode.System, miThemeSystem },
        { Core.ThemeMode.Light, miThemeLight },
        { Core.ThemeMode.Dark, miThemeDark },
        { Core.ThemeMode.None, miThemeLegacy }
    };

    private void SetThemeMode(Core.ThemeMode mode, MenuItem? toggleOn)
    {
#pragma warning disable WPF0001
        // disable all theme-related check boxes (on menu items)
        miThemeLight.IsChecked = false;
        miThemeDark.IsChecked = false;
        miThemeSystem.IsChecked = false;
        miThemeLegacy.IsChecked = false;

        // set mode to settings
        RTSettings.Current.ThemeMode = mode;

        switch (mode)
        {
            case Core.ThemeMode.Dark:
                FZCore.Core.SetApplicationTheme(FZCore.FZThemeMode.Dark);
                break;

            case Core.ThemeMode.Light:
                FZCore.Core.SetApplicationTheme(FZCore.FZThemeMode.Light);
                break;

            case Core.ThemeMode.System:
                FZCore.Core.SetApplicationTheme(FZCore.FZThemeMode.System);
                break;

            case Core.ThemeMode.None:
                FZCore.Core.SetApplicationTheme(FZCore.FZThemeMode.None);
                break;

            default:
                break;
        }

        // check the right one
        if (toggleOn != null)
        {
            toggleOn.IsChecked = true;
        }
#pragma warning restore WPF0001
    }

    private void miAoT_Checked(object sender, RoutedEventArgs e)
    {
        RTSettings.Current.AlwaysOnTop = true;
        Topmost = true;
    }

    private void miAoT_Unchecked(object sender, RoutedEventArgs e)
    {
        RTSettings.Current.AlwaysOnTop = false;
        Topmost = false;
    }

    private void miThemeLight_Click(object sender, RoutedEventArgs e)
    {
        SetThemeMode(Core.ThemeMode.Light, miThemeLight);
    }

    private void miThemeDark_Click(object sender, RoutedEventArgs e)
    {
        SetThemeMode(Core.ThemeMode.Dark, miThemeDark);
    }

    private void miThemeSystem_Click(object sender, RoutedEventArgs e)
    {
        SetThemeMode(Core.ThemeMode.System, miThemeSystem);
    }

    private void miThemeLegacy_Click(object sender, RoutedEventArgs e)
    {
        SetThemeMode(Core.ThemeMode.None, miThemeLegacy);
    }

    private void miCustomPath_Click(object sender, RoutedEventArgs e)
    {
        SelectDownloader();
    }

    private void miCustomPathReset_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show(Messages.ResetCustomDownloader, "Reset Downloader", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            ResetDownloader();
        }
    }

    private async void miCheckForUpdates_Click(object sender, RoutedEventArgs e)
    {
        // check for yt-dlp updates
        WinAPI.AllocConsole();

        if (FZCore.Core.StartProcess(VideoDownloader.GetDownloader(), "--update", out Process proc) == true)
        {
            await proc.WaitForExitAsync();
        }

        WinAPI.FreeConsole();

        if (proc.ExitCode == 0)
        {
            ResetFields();
        }
    }
}