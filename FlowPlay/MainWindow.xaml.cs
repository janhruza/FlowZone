using FlowPlay.Core;
using FlowPlay.Windows;

using FZCore;
using FZCore.Extensions;
using FZCore.Windows;

using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace FlowPlay;

/// <summary>
/// Representing the main application window.
/// </summary>
public partial class MainWindow : IconlessWindow
{
    #region Native calls

    [DllImport("dwmapi")]
    private static extern int DwmSetWindowAttribute(IntPtr hWnd, int dwAttribute, int[] pwAttribute, int cbAttribute);

    #endregion

    #region Media player controls

    private const double POSITION_STEP = 10_000;

    private void StopMedia()
    {
        cPlayer.Stop();
        Title = "FlowPlay";
    }

    private void SetPlaybackSpeed(double speed)
    {
        if (cPlayer.IsLoaded == false)
        {
            return;
        }

        cPlayer.SpeedRatio = speed;
        return;
    }

    private List<MenuItem> aSpeeds;
    private void SetupPlayerSpeeds()
    {
        foreach (MenuItem miSpeed in aSpeeds)
        {
            miSpeed.Click += (s, e) =>
            {
                foreach (MenuItem mi in aSpeeds)
                {
                    if (mi == miSpeed)
                    {
                        mi.IsCheckable = true;
                        mi.IsChecked = true;
                    }

                    else
                    {
                        mi.IsCheckable = false;
                        mi.IsChecked = false;
                    }
                }
            };
        }
    }

    private void VolumeUp()
    {
        if (cPlayer.Volume >= 0.95)
        {
            cPlayer.Volume = 1.0;
        }

        else
        {
            cPlayer.Volume += 0.05;
        }
    }

    private void VolumeDown()
    {
        if (cPlayer.Volume <= 0.05)
        {
            cPlayer.Volume = 0;
        }

        else
        {
            cPlayer.Volume -= 0.05;
        }
    }

    private void PlayerRewind()
    {
        double cur = cPlayer.Position.TotalMilliseconds;
        if (cur <= POSITION_STEP)
        {
            cPlayer.Position = TimeSpan.FromMilliseconds(0);
        }

        else
        {
            cPlayer.Position = TimeSpan.FromMilliseconds(cur - POSITION_STEP);
        }
    }

    private void PlayerForward()
    {
        if (cPlayer.NaturalDuration.HasTimeSpan == false)
        {
            return;
        }

        double max = cPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
        double cur = cPlayer.Position.TotalMilliseconds;

        if ((max - cur) < POSITION_STEP)
        {
            cPlayer.Position = TimeSpan.FromMilliseconds(max);
        }

        else
        {
            cPlayer.Position = TimeSpan.FromMilliseconds(cur + POSITION_STEP);
        }
    }

    private bool bPlaying = false;

    private void PlayerPause()
    {
        dTimer.Stop();
        cPlayer.Pause();
        bPlaying = false;
        btnPlayPause.Content = sPlay;
    }

    private void PlayerPlay()
    {
        cPlayer.Play();
        bPlaying = true;
        btnPlayPause.Content = sPause;
        dTimer.Start();
    }

    private void PlayPause()
    {
        if (bPlaying == true)
        {
            PlayerPause();
        }

        else
        {
            PlayerPlay();
        }
    }

    private void StartMedia()
    {
        dTimer.Stop();
        bPlaying = false;
        PlayPause();
        dTimer.Start();
    }

    private void OpenMedia()
    {
        OpenFileDialog ofd = new OpenFileDialog
        {
            Filter = "Video|*.wmv;*.mpeg;*.avi;*.mp4|Audio|*.mp3;*.wav|Other|*.*"
        };

        if (ofd.ShowDialog() == true)
        {
            StopMedia();

            SizeToContent = SizeToContent.WidthAndHeight;
            cPlayer.Source = new Uri(ofd.FileName);
            StartMedia();

            Title = $"{ofd.SafeFileName} | FlowPlay";
        }
    }

    private bool LoadAudioTrackInfo(string filename)
    {
        if (string.IsNullOrEmpty(filename) == true)
        {
            return false;
        }

        string ext = System.IO.Path.GetExtension(filename).ToLowerInvariant();
        AudioTagInfo tag = AudioTagInfo.Empty;
        bool result;

        switch (ext)
        {
            case ".mp3":
                result = AudioTagParser.ParseMp3File(filename, out tag);
                break;

            case ".wav":
                result = AudioTagParser.ParseWavFile(filename, out tag);
                break;

            default:
                result = false;
                break;
        }

        rArtist.Text = tag.Artist;
        rTitle.Text = tag.Title;

        return result;
    }

    private void FitMediaIntoWindow()
    {
        SizeToContent = SizeToContent.WidthAndHeight;
        cPlayer.Width = cPlayer.NaturalVideoWidth;
        cPlayer.Height = cPlayer.NaturalVideoHeight;
        return;
    }

    private void ToggleFullScreen()
    {
        if (WindowState == WindowState.Normal)
        {
            // fill window with player
            SizeToContent = SizeToContent.Manual;
            cPlayer.Width = double.NaN;
            cPlayer.Height = double.NaN;
        }

        else
        {
            // set window size to the size of the player
            if (cPlayer.HasVideo)
            {
                SizeToContent = SizeToContent.WidthAndHeight;
                cPlayer.Width = cPlayer.NaturalVideoWidth;
                cPlayer.Height = cPlayer.NaturalVideoHeight;
            }
        }

        // toggle full screen mode
        this.ToggleFullScreenMode();
    }

    #endregion

    #region Icon glyphs

    private string sPlay = "";
    private string sPause = "";

    #endregion

    private DispatcherTimer dTimer;

    /// <summary>
    /// Creates a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();

        aSpeeds = [miSpeed025, miSpeed050, miSpeed075, miSpeed100, miSpeed125, miSpeed150, miSpeed175, miSpeed200];
        SetupPlayerSpeeds();

        dTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(1)
        };

        dTimer.Tick += (s, e) =>
        {
            // adjust slider position
            slPosition.Value = cPlayer.Position.TotalMilliseconds;
        };

        // set transparent (acrylic) background
        //DwmSetWindowAttribute(this.GetHandle(), 38, [3], sizeof(int));
        //DwmSetWindowAttribute(this.GetHandle(), 38, [2], sizeof(int));
    }

    private void ShowLoadedVideoProperties()
    {
        // input checks
        if (cPlayer.HasVideo == false) return;
        if (cPlayer.Source == null) return;

        // only local files supported
        string path = cPlayer.Source.LocalPath;

        // get the video properties
        VideoProperties vp = new VideoProperties
        {
            Path = path,
            Height = cPlayer.NaturalVideoHeight,
            Width = cPlayer.NaturalVideoWidth,
            Duration = cPlayer.NaturalDuration.TimeSpan
        };

        // show the properties window
        _ = new WndVideoProperties(ref vp).ShowDialog();
    }

    private void RestoreDefaultSize()
    {
        Width = 300;
        Height = 300;
    }

    private void miOpenMedia_Click(object sender, RoutedEventArgs e)
    {
        OpenMedia();
    }

    private void cPlayer_MediaOpened(object sender, RoutedEventArgs e)
    {
        if (cPlayer.HasVideo == false)
        {
            // hide the player for audio files and show track info instead
            cPlayer.Visibility = Visibility.Collapsed;
            tbTrackInfo.Visibility = Visibility.Visible;

            slPosition.Minimum = 0;
            slPosition.Maximum = cPlayer.NaturalDuration.HasTimeSpan ? cPlayer.NaturalDuration.TimeSpan.TotalMilliseconds : 0;
            bdControls.Visibility = Visibility.Visible;

            if (cPlayer.Source != null || string.IsNullOrEmpty(cPlayer.Source!.LocalPath) == false)
            {
                string filename = cPlayer.Source!.LocalPath;
                if (LoadAudioTrackInfo(filename) == false)
                {
                    Log.Error("Failed to load audio track info for file: " + filename, nameof(cPlayer_MediaOpened));
                }
            }

            // restore default size
            RestoreDefaultSize();
            return;
        }

        // show the player and hide track info
        tbTrackInfo.Visibility = Visibility.Collapsed;
        cPlayer.Visibility = Visibility.Visible;

        int w = cPlayer.NaturalVideoWidth;
        int h = cPlayer.NaturalVideoHeight;

        cPlayer.Width = w;
        cPlayer.Height = h;

        slPosition.Minimum = 0;
        slPosition.Maximum = cPlayer.NaturalDuration.HasTimeSpan ? cPlayer.NaturalDuration.TimeSpan.TotalMilliseconds : 0;
        return;
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.F10)
        {
            ToggleFullScreen();
        }

        else if (e.Key == Key.F2)
        {
            FitMediaIntoWindow();
        }

        else if (e.Key == Key.F1)
        {
            OpenMedia();
        }

        else if (e.Key == Key.F11)
        {
            ToggleFullScreen();
        }

        else if (e.Key == Key.Down)
        {
            VolumeDown();
        }

        else if (e.Key == Key.Up)
        {
            VolumeUp();
        }

        else if (e.Key == Key.Space)
        {
            PlayPause();
        }

        else if (e.Key == Key.Left)
        {
            PlayerRewind();
        }

        else if (e.Key == Key.Right)
        {
            PlayerForward();
        }

        else if (e.Key == Key.Apps)
        {
            SystemCommands.ShowSystemMenu(this, PointToScreen(new Point(0, 0)));
        }

        else if (e.Key == Key.F9)
        {
            ShowLoadedVideoProperties();
        }
    }

    private void miFitVideo_Click(object sender, RoutedEventArgs e)
    {
        FitMediaIntoWindow();
    }

    private void miPlayPause_Click(object sender, RoutedEventArgs e)
    {
        PlayPause();
    }

    private void miVolumeUp_Click(object sender, RoutedEventArgs e)
    {
        VolumeUp();
    }

    private void miVolumeDown_Click(object sender, RoutedEventArgs e)
    {
        VolumeDown();
    }

    private void btnBack_Click(object sender, RoutedEventArgs e)
    {
        PlayerRewind();
    }

    private void btnPlayPause_Click(object sender, RoutedEventArgs e)
    {
        PlayPause();
    }

    private void btnNext_Click(object sender, RoutedEventArgs e)
    {
        PlayerForward();
    }

    private void cPlayer_MouseEnter(object sender, MouseEventArgs e)
    {
        bdControls.Visibility = Visibility.Visible;
    }

    private void cPlayer_MouseLeave(object sender, MouseEventArgs e)
    {
        if (bdControls.IsMouseOver)
        {
            return;
        }

        bdControls.Visibility = Visibility.Collapsed;
    }

    private void miFullScreen_Click(object sender, RoutedEventArgs e)
    {
        ToggleFullScreen();
    }

    private void slPosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (cPlayer.IsLoaded == false)
        {
            return;
        }

        if (slPosition.IsMouseOver == true && Mouse.LeftButton == MouseButtonState.Pressed)
        {
            PlayerPause();
            double value = slPosition.Value;
            cPlayer.Position = TimeSpan.FromMilliseconds(value);
            PlayerPlay();
        }

        return;
    }

    private void miFitSize_Click(object sender, RoutedEventArgs e)
    {
        // fill window with player
        SizeToContent = SizeToContent.Manual;
        cPlayer.Width = double.NaN;
        cPlayer.Height = double.NaN;
    }

    private void miSpeed025_Click(object sender, RoutedEventArgs e)
    {
        SetPlaybackSpeed(App.SPEED_025);
    }

    private void miSpeed050_Click(object sender, RoutedEventArgs e)
    {
        SetPlaybackSpeed(App.SPEED_050);
    }

    private void miSpeed075_Click(object sender, RoutedEventArgs e)
    {
        SetPlaybackSpeed(App.SPEED_075);
    }

    private void miSpeed100_Click(object sender, RoutedEventArgs e)
    {
        SetPlaybackSpeed(App.SPEED_NORMAL);
    }

    private void miSpeed125_Click(object sender, RoutedEventArgs e)
    {
        SetPlaybackSpeed(App.SPEED_125);
    }

    private void miSpeed150_Click(object sender, RoutedEventArgs e)
    {
        SetPlaybackSpeed(App.SPEED_150);
    }

    private void miSpeed175_Click(object sender, RoutedEventArgs e)
    {
        SetPlaybackSpeed(App.SPEED_175);
    }

    private void miSpeed200_Click(object sender, RoutedEventArgs e)
    {
        SetPlaybackSpeed(App.SPEED_200);
    }

    private void IconlessWindow_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        double newSize = e.NewSize.Width / 3;
        if (newSize != double.NaN)
        {
            tbNote.FontSize = newSize;
        }

        else
        {
            tbNote.FontSize = 100;
        }
    }

    private void miVideoProperties_Click(object sender, RoutedEventArgs e)
    {
        ShowLoadedVideoProperties();
    }
}