using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using UpDate.Core;

namespace UpDate.Pages;

/// <summary>
/// Representing the RSS channel view page.
/// </summary>
public partial class PgChannelView : Page
{
    #region Private fields

    private RssChannel _channel;

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a new instance of the <see cref="PgChannelView"/> class.
    /// </summary>
    public PgChannelView()
    {
        InitializeComponent();
        _channel = new RssChannel();
        ReloadUI();
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PgChannelView"/> class with the specified RSS <paramref name="channel"/> structure.
    /// </summary>
    /// <param name="channel">A RSS channel which will be used as data source.</param>
    public PgChannelView(RssChannel channel)
    {
        InitializeComponent();
        _channel = channel;
        ReloadUI();
    }

    #endregion

    #region Class methods

    private ListBoxItem NoFeedItemsItem()
    {
        return new ListBoxItem
        {
            Content = "No feed items available."
        };
    }

    private ListBoxItem GetFeedItem(FeedItem item)
    {
        ListBoxItem lbi = new ListBoxItem
        {
            Content = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                TextTrimming = TextTrimming.CharacterEllipsis,

                Inlines =
                {
                    new Run
                    {
                        Text = item.Title,
                        FontSize = 18,
                        FontWeight = FontWeights.SemiBold
                    },

                    new LineBreak(),

                    new Run
                    {
                        Text = item.Description
                    }
                }
            }
        };

        lbi.MouseDoubleClick += (s, e) =>
        {
            if (string.IsNullOrEmpty(item.Link) == false)
            {
                FZCore.Core.OpenWebPage(item.Link);
            }
        };

        return lbi;
    }

    private void ReloadUI()
    {
        if (RssChannel.IsValid(ref _channel) == false)
        {
            return;
        }

        // channel is valid, display its info
        rTitle.Text = _channel.Title;
        rDescription.Text = _channel.Description;

        // get channel items
        lbFeedItems.Items.Clear();

        if (_channel.Items.Count == 0)
        {
            // no feed items
            ListBoxItem item = NoFeedItemsItem();
            lbFeedItems.Items.Add(item);
            return;
        }

        foreach (FeedItem item in _channel.Items)
        {
            ListBoxItem lbi = GetFeedItem(item);
            lbFeedItems.Items.Add(lbi);
        }
    }

    #endregion
}
