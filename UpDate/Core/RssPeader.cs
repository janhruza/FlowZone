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
public class RssPeader
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

    private static async Task<string> RetrieveRssData(Uri source)
    {
        HttpClient client = new HttpClient
        {
            BaseAddress = source
        };

        HttpResponseMessage msg = await client.GetAsync(source);
        if (msg.IsSuccessStatusCode == false)
        {
            return string.Empty;
        }

        return await msg.Content.ReadAsStringAsync();
    }

    private static void ReadXmlData(XmlDocument doc, string data) => doc.LoadXml(data);

    #endregion

    #region Fields

    private string _data;
    private XmlDocument _doc = new XmlDocument();

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a new instance of the <see cref="RssPeader"/> class where the RSS source is a file (<paramref name="rssFile"/>).
    /// </summary>
    /// <param name="rssFile">Path to the file that contains RSS feed data. RSS file is a XML file.</param>
    public RssPeader(string rssFile)
    {
        _data = ReadRssFile(rssFile);
        ReadXmlData(_doc, _data);
    }

    #endregion

    #region Methods and properties

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
