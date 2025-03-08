using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices.Marshalling;
using System.Threading.Tasks;
using System.Xml;
using FZCore;

namespace UpDate.Core;

/// <summary>
/// Representing a basic RSS parser.
/// </summary>
public class RssReader
{
    #region Static methods

    private static string ReadRssFile(string path)
    {
        if (File.Exists(path) == false)
        {
            Log.Error($"File \'{path}\' was not found.", nameof(ReadRssFile));
            return string.Empty;
        }

        string data = File.ReadAllText(path);
        return data;
    }

    internal static async Task<string> RetrieveRssData(Uri? source)
    {
        if (source == null)
        {
            return string.Empty;
        }

        try
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(10); // Set timeout
                HttpResponseMessage msg = await client.GetAsync(source);

                msg.EnsureSuccessStatusCode();

                if (msg.IsSuccessStatusCode)
                {
                    return await msg.Content.ReadAsStringAsync();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(RetrieveRssData));
            return string.Empty;
        }
    }

    private static bool ReadXmlData(XmlDocument doc, string data)
    {
        if (string.IsNullOrEmpty(data) == true)
        {
            return false;
        }

        doc.LoadXml(data);
        return true;
    }

    #endregion

    #region Fields

    private string _data;
    private XmlDocument _doc = new XmlDocument();

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a default nstace of the <see cref="RssReader"/> class.
    /// </summary>
    public RssReader()
    {
        _data = string.Empty;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="RssReader"/> class where the RSS source is a file (<paramref name="rssFile"/>).
    /// </summary>
    /// <param name="rssFile">Path to the file that contains RSS feed data. RSS file is a XML file.</param>
    public RssReader(string rssFile)
    {
        _data = ReadRssFile(rssFile);
        ReadXmlData(_doc, _data);
    }

    #endregion

    #region Methods and properties

    /// <summary>
    /// Loads the source data from the specified <paramref name="url"/> address.
    /// </summary>
    /// <param name="url">Target URL address.</param>
    /// <returns>True, if data were downloaded, otherwise false.</returns>
    public async Task<bool> LoadDataAsync(string url)
    {
        _data = await RetrieveRssData(new Uri(url, UriKind.RelativeOrAbsolute));
        if (string.IsNullOrEmpty(_data) == false)
        {
            return ReadXmlData(_doc, _data);
        }

        return false;
    }

    /// <summary>
    /// Reads all RSS channels from the RSS feed.
    /// </summary>
    /// <returns>List of all parsed channels.</returns>
    public List<RssChannel> ReadChannels()
    {
        try
        {
            List<RssChannel> channels = new List<RssChannel>();

            var rssNodes = _doc.SelectNodes("//rss/channel");
            if (rssNodes == null || rssNodes.Count == 0)
            {
                Log.Warning("Selected document has no RSS channels.", nameof(ReadChannels));
                return channels;
            }

            foreach (XmlNode channelNode in rssNodes)
            {
                if (channelNode == null)
                {
                    continue;
                }

                RssChannel channel = new RssChannel
                {
                    Title = channelNode.SelectSingleNode("title")?.InnerText ?? string.Empty,
                    Link = channelNode.SelectSingleNode("link")?.InnerText ?? string.Empty,
                    Description = channelNode.SelectSingleNode("description")?.InnerText ?? string.Empty,
                    Category = channelNode.SelectSingleNode("category")?.InnerText ?? string.Empty,
                    Copyright = channelNode.SelectSingleNode("copyright")?.InnerText ?? string.Empty
                };

                var itemNodes = channelNode.SelectNodes("item");
                if (itemNodes != null)
                {
                    foreach (XmlNode itemNode in itemNodes)
                    {
                        FeedItem item = new FeedItem
                        {
                            Title = itemNode.SelectSingleNode("title")?.InnerText ?? string.Empty,
                            Link = itemNode.SelectSingleNode("link")?.InnerText ?? string.Empty,
                            Description = itemNode.SelectSingleNode("description")?.InnerText ?? string.Empty,
                            Author = itemNode.SelectSingleNode("author")?.InnerText ?? string.Empty,
                            Category = itemNode.SelectSingleNode("category")?.InnerText ?? string.Empty,
                            Comments = itemNode.SelectSingleNode("comments")?.InnerText ?? string.Empty,
                            Guid = itemNode.SelectSingleNode("guid")?.InnerText ?? string.Empty,
                            PublicationDate = itemNode.SelectSingleNode("pubDate")?.InnerText ?? string.Empty
                        };

                        channel.Items.Add(item);
                    }
                }

                channels.Add(channel);
            }

            return channels;
        }
        catch (Exception ex)
        {
            Log.Error(ex, nameof(ReadChannels));
            return new List<RssChannel>();
        }
    }

    /// <summary>
    /// Reads all RSS channels from the RSS feed asynchronously.
    /// </summary>
    /// <returns>List of all parsed channels.</returns>
    public Task<List<RssChannel>> ReadChannelsAsync()
    {
        return Task<List<RssChannel>>.Run(() =>
        {
            return ReadChannels();
        });
    }

    #endregion
}
