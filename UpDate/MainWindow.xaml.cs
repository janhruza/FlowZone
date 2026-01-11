using FZCore;
using FZCore.Extensions;
using FZCore.Windows;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

using UpDate.Core;
using UpDate.Pages;
using UpDate.Windows;

namespace UpDate;

/// <summary>
/// Representing the main application window class.
/// This class derives from the <see cref="Window"/> class.
/// </summary>
public partial class MainWindow : IconlessWindow
{
    /// <summary>
    /// Representing the maximum length of the displayed text of all channel items.
    /// </summary>
    private const int CHANNEL_MAX_SIZE = 25;

    /// <summary>
    /// Creates a new instance of the <see cref="MainWindow"/> class with default parameters.
    /// </summary>
    public MainWindow()
    {
        // init window
        InitializeComponent();

        // extend window
        WindowExtender wex = new WindowExtender(this);
        wex.AddSeparator(0x11);
        wex.AddMenuItem(0x12, "Refresh\tF5", async () => await ReloadFeedsAsync());
        wex.AddMenuItem(0x13, "View log\tF1", () => FZCore.Core.ViewLog());

        // activate all extensions
        wex.EmpowerWindow();

        // add hooks
        this.KeyDown += async (s, e) =>
        {
            if (e.Key == System.Windows.Input.Key.F2)
            {
                HandleSettings();
            }

            if (e.Key == System.Windows.Input.Key.F5)
            {
                await ReloadFeedsAsync();
            }
        };

        this.Loaded += async (s, e) =>
        {
            await ReloadFeedsAsync();
        };
    }

    private void ApplySettings(UpDateSettings? settings)
    {
        if (settings == null)
        {
            Log.Error("Selected settings object is null.", nameof(ApplySettings));
            return;
        }

        this.Title = settings.Title;
        this.Width = settings.WindowSize.Width;
        this.Height = settings.WindowSize.Height;

        FZCore.Core.SetApplicationTheme(settings.ThemeMode);
        return;
    }

    private async Task<bool> ReloadFeedsAsync()
    {
        try
        {
            if (UpDateSettings.Current == null)
            {
                UpDateSettings.Current = UpDateSettings.EnsureSettings();
            }

            // clear data
            trFeeds.Items.Clear();
            frmContent.Content = null;

            var feedSources = UpDateSettings.Current.Feeds;
            if (feedSources.Count == 0)
            {
                // draw info about no available feeds
                TreeViewItem item = new TreeViewItem
                {
                    Header = "No feeds available.",
                    Padding = new Thickness(0, 10, 10, 10)
                };

                trFeeds.Items.Add(item);
                return true;
            }

            foreach (string feedSource in feedSources)
            {
                RssReader reader = new RssReader();
                bool result = await reader.LoadDataAsync(feedSource);

                if (result == false)
                {
                    continue;
                }

                List<RssChannel> channels = await reader.ReadChannelsAsync();

                foreach (RssChannel channel in channels)
                {
                    // create channel item in feeds tree
                    TreeViewItem item = new TreeViewItem
                    {
                        Header = channel.Title.Trim().Reduce(CHANNEL_MAX_SIZE),
                        Uid = channel.Link,
                        ToolTip = new TextBlock
                        {
                            Inlines =
                            {
                                new Run
                                {
                                    Text = channel.Title.Trim(),
                                    FontWeight = FontWeights.SemiBold,
                                    FontSize = 16
                                },

                                new LineBreak(),

                                new Run
                                {
                                    Text = channel.Description.Trim()
                                }
                            },

                            TextWrapping = TextWrapping.Wrap,
                            TextTrimming = TextTrimming.CharacterEllipsis
                        },

                        Padding = new Thickness(0, 10, 10, 10)
                    };

                    item.Selected += (s, e) =>
                    {
                        // open RSS feed in a feed reader page
                        PgChannelView channelView = new PgChannelView(channel);
                        frmContent.Content = channelView;
                        this.Title = $"{channel.Title.Trim()} | {UpDateSettings.Current.Title}";
                    };

                    trFeeds.Items.Add(item);
                }

                if (trFeeds.Items.Count > 0)
                {
                    TreeViewItem? first = trFeeds.Items[0] as TreeViewItem;
                    if (first != null)
                    {
                        first.IsSelected = true;
                    }
                }
            }

            return true;
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(ReloadFeedsAsync));
            return false;
        }
    }

    private void miClose_Click(object sender, RoutedEventArgs e)
    {
        // close window
        this.Close();
    }

    private async void miAddFeed_Click(object sender, RoutedEventArgs e)
    {
        // add new feed item
        if (new WndAddFeed().ShowDialog() == true)
        {
            // redraw RSS feed sources
            await ReloadFeedsAsync();
        }
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        // check if settings exists
        if (UpDateSettings.Current != null)
        {
            UpDateSettings.Current.WindowSize = new Size(this.Width, this.Height);
        }
    }

    private void miResetSettings_Click(object sender, RoutedEventArgs e)
    {
        // get confirmation
        if (MessageBox.Show($"Do you want to restore the default settings? This will erase all saved feeds and other settings. This action is irreversible.",
                            $"Restore settings",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning,
                            MessageBoxResult.No) == MessageBoxResult.Yes)
        {
            // reset current settings object into the default state
            UpDateSettings.Current = new UpDateSettings();
            ApplySettings(UpDateSettings.Current);
        }
    }

    private void HandleSettings()
    {
        WndSettings settings = new WndSettings();
        _ = settings.ShowDialog();

        if (settings.Result == true)
        {
            ApplySettings(UpDateSettings.Current);
        }
    }

    private void miSettings_Click(object sender, RoutedEventArgs e)
    {
        HandleSettings();
    }

    private async void miRestoreFeed_Click(object sender, RoutedEventArgs e)
    {
        UpDateSettings.Current ??= UpDateSettings.EnsureSettings();
        UpDateSettings.Current.Feeds = UpDateSettings.GetDefaultFeeds();
        ApplySettings(UpDateSettings.Current);
        await ReloadFeedsAsync();
    }
}