using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using FZCore;
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

    private string DocumentMeta(string styleCss)
    {
        return $"<head><meta charset='UTF-8'><meta name='viewport' content='width=device-width, initial-scale=1'><style>{styleCss}</style></head>";
    }

    private string GetDarkStyles()
    {
        return @"* {
    background-color: #000000;
    color: #FFFFFF;
    font-family: sans-serif;
}

p {
    color: #FFFFFF;
    font-size: 14pt;
}

a {
    color: darkorange;
}

a:hover {
    color: orange;
}

h1, h2, h3, h4, h5, h6 {
    color: darkorange;
}

::selection {
    background-color: darkorange;
    color: #FFFFFF;
}";
    }

    private string GetLightStyles()
    {
        return @"* {
    background-color: #FFFFFF;
    color: #000000;
    font-family: sans-serif;
}

p {
    color: #000000;
    font-size: 14pt;
}

a {
    color: orange;
}

a:hover {
    color: darkorange;
}

h1, h2, h3, h4, h5, h6 {
    color: orange;
}

::selection {
    background-color: orange;
    color: #000000;
}";
    }

    private string GetStyleSheet()
    {
        if (UpDateSettings.Current == null)
        {
            UpDateSettings.Current = UpDateSettings.EnsureSettings();
        }

        switch (UpDateSettings.Current.ThemeMode)
        {
            case FZThemeMode.Light: return GetLightStyles();
            case FZThemeMode.Dark: return GetDarkStyles();
            case FZThemeMode.None: return GetLightStyles();

            default:
            case FZThemeMode.System: return (FZCore.Core.IsDarkModeEnabled() == true ? GetDarkStyles() : GetLightStyles());
        }
    }

    private string GetHTMLText(string title, string body)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<html>");
        sb.Append(DocumentMeta(GetStyleSheet()));
        sb.Append("<body>");
        sb.Append("<h1>");
        sb.Append(title);
        sb.Append("</h1>");
        sb.Append(body);
        sb.Append("</body>");
        sb.Append("</html>");
        return sb.ToString();
    }

    private ListBoxItem NoFeedItemsItem()
    {
        return new ListBoxItem
        {
            Content = "No feed items available."
        };
    }

    private ListBoxItem GetFeedItem(FeedItem item)
    {
        ListBoxItem lbi = new ListBoxItem();

        if (FeedItem.IsHTMLBody(ref item) == false)
        {
            string dateText;
            if (DateTime.TryParse(item.PublicationDate, out DateTime dt) == true)
            {
                dateText = dt.ToString("f");
            }

            else
            {
                dateText = item.PublicationDate;
            }

            string description = item.Description.Trim();
            if (string.IsNullOrEmpty(description) == true)
            {
                description = "No description.";
            }

            lbi.Content = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                TextTrimming = TextTrimming.CharacterEllipsis,

                Inlines =
                {
                    new Run
                    {
                        Text = item.Title.Trim(),
                        FontSize = 18,
                        FontWeight = FontWeights.SemiBold
                    },

                    new LineBreak(),

                    new Run
                    {
                        Text = dateText
                    },

                    new LineBreak(),

                    new Run
                    {
                        Text = description
                    }
                }
            };
        }

        else
        {
            // has HTML body
            lbi.Content = item.Title;
        }

        lbi.MouseDoubleClick += (s, e) =>
        {
            if (string.IsNullOrEmpty(item.Link) == false)
            {
                FZCore.Core.OpenWebPage(item.Link);
            }

            else if (FeedItem.IsHTMLBody(ref item) == true)
            {
                if (UpDateSettings.Current == null)
                {
                    UpDateSettings.Current = UpDateSettings.EnsureSettings();
                }

                // open description as web
                WebBrowser wb = new WebBrowser();

                wb.NavigateToString(GetHTMLText(item.Title, item.Description));

                Window window = new Window
                {
                    Content = wb,
                    Width = SystemParameters.PrimaryScreenWidth - 400,
                    Height = SystemParameters.PrimaryScreenHeight - 200,
                    Title = $"{item.Title} - {UpDateSettings.Current.Title}",
                    Background = Brushes.Transparent
                };

                window.Show();
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
        rTitle.Text = _channel.Title.Trim();
        rDescription.Text = _channel.Description.Trim();

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
