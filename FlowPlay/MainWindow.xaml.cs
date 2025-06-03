using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;

namespace FlowPlay;

/// <summary>
/// Representing the main application window.
/// </summary>
public partial class MainWindow : Window
{
    #region Media player controls

    private void StopMedia()
    {
        cPlayer.Stop();
        this.Title = "FlowPlay";
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

    private bool bPlaying = false;

    private void PlayPause()
    {
        if (bPlaying == true)
        {
            dTimer.Stop();
            cPlayer.Pause();
            bPlaying = false;
            btnPlayPause.Content = sPlay;
        }

        else
        {
            cPlayer.Play();
            bPlaying = true;
            btnPlayPause.Content = sPause;
            dTimer.Start();
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
        return;
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
        if (e.Key == Key.F2)
        {
            FitMediaIntoWindow();
        }

        if (e.Key == Key.F1)
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

    }

    private void btnPlayPause_Click(object sender, RoutedEventArgs e)
    {
        PlayPause();
    }

    private void btnNext_Click(object sender, RoutedEventArgs e)
    {

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
}