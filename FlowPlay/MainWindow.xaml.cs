using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using FZCore.Extensions;
using Microsoft.Win32;

namespace FlowPlay;

/// <summary>
/// Representing the main application window.
/// </summary>
public partial class MainWindow : Window
{
    #region Native calls

    [DllImport("dwmapi")]
    static extern int DwmSetWindowAttribute(IntPtr hWnd, int dwAttribute, int[] pwAttribute, int cbAttribute);

    #endregion

    #region Media player controls

    const double POSITION_STEP = 10_000;

    private void StopMedia()
    {
        cPlayer.Stop();
        this.Title = "FlowPlay";
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

    List<MenuItem> aSpeeds;
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
        bPlaying = false;
        PlayPause();
    }

    private void OpenMedia()
    {
        OpenFileDialog ofd = new OpenFileDialog
        {
            Filter = "All files|*.*"
        };

        if (ofd.ShowDialog() == true)
        {
            this.SizeToContent = SizeToContent.WidthAndHeight;
            cPlayer.Source = new Uri(ofd.FileName);
            StartMedia();

            this.Title = $"{ofd.SafeFileName} | FlowPlay";
        }
    }

    private void FitMediaIntoWindow()
    {
        this.SizeToContent = SizeToContent.WidthAndHeight;
        cPlayer.Width = cPlayer.NaturalVideoWidth;
        cPlayer.Height = cPlayer.NaturalVideoHeight;
        return;
    }

    private void ToggleFullScreen()
    {
        if (WindowState == WindowState.Normal)
        {
            // fill window with player
            this.SizeToContent = SizeToContent.Manual;
            cPlayer.Width = double.NaN;
            cPlayer.Height = double.NaN;
        }

        else
        {
            // set window size to the size of the player
            if (cPlayer.HasVideo)
            {
                this.SizeToContent = SizeToContent.WidthAndHeight;
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

        this.KeyDown += (s, e) =>
        {
            if (e.Key == System.Windows.Input.Key.Apps)
            {
                SystemCommands.ShowSystemMenu(this, this.PointToScreen(new Point(0, 0)));
            }
        };

        // set transparent (acrylic) background
        DwmSetWindowAttribute(this.GetHandle(), 38, [3], sizeof(int));
    }

    private void miOpenMedia_Click(object sender, RoutedEventArgs e)
    {
        OpenMedia();
    }

    private void cPlayer_MediaOpened(object sender, RoutedEventArgs e)
    {
        if (cPlayer.HasVideo == false)
        {
            return;
        }

        int w = cPlayer.NaturalVideoWidth;
        int h = cPlayer.NaturalVideoHeight;

        cPlayer.Width = w;
        cPlayer.Height = h;

        slPosition.Minimum = 0;
        slPosition.Maximum = cPlayer.NaturalDuration.HasTimeSpan ? cPlayer.NaturalDuration.TimeSpan.TotalMilliseconds : 0;
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
        this.SizeToContent = SizeToContent.Manual;
        cPlayer.Width = double.NaN;
        cPlayer.Height = double.NaN;
    }

    private void miSpeed025_Click(object sender, RoutedEventArgs e)
    {
        SetPlaybackSpeed(0.25);
    }

    private void miSpeed050_Click(object sender, RoutedEventArgs e)
    {
        SetPlaybackSpeed(0.50);
    }

    private void miSpeed075_Click(object sender, RoutedEventArgs e)
    {
        SetPlaybackSpeed(0.75);
    }

    private void miSpeed100_Click(object sender, RoutedEventArgs e)
    {
        SetPlaybackSpeed(1.00);
    }

    private void miSpeed125_Click(object sender, RoutedEventArgs e)
    {
        SetPlaybackSpeed(1.25);
    }

    private void miSpeed150_Click(object sender, RoutedEventArgs e)
    {
        SetPlaybackSpeed(1.50);
    }

    private void miSpeed175_Click(object sender, RoutedEventArgs e)
    {
        SetPlaybackSpeed(1.75);
    }

    private void miSpeed200_Click(object sender, RoutedEventArgs e)
    {
        SetPlaybackSpeed(2.00);
    }
}