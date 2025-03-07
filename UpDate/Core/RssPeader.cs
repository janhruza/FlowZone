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

    /// <summary>
    /// Creates a new instance of the <see cref="RssPeader"/> class where the RSS source is a URL address where the RSS feed data are located.
    /// </summary>
    /// <param name="rssSource">URL address where the RSS data can be retrieved from.</param>
    public RssPeader(Uri rssSource)
    {
        var task = RetrieveRssData(rssSource);
        Task.WaitAny(task);
        _data = task.Result;
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
            List<RssChannel> channels = [];

            var rssNodes = _doc.SelectNodes("rss");
            if (rssNodes == null || rssNodes.Count == 0)
            {
                Log.Warning("Selected document has no RSS nodes.", nameof(ReadChannels));
                return [];
            }

            // read channels (from all RSS nodes (if multiple for some reason)
            foreach (XmlNode node in rssNodes)
            {
                var channelNodes = node.SelectNodes("channel");
                if (channelNodes == null || channelNodes.Count == 0)
                {
                    // no channels, return
                    Log.Warning("No channel nodes found.", nameof(ReadChannels));
                    return channels;
                }

                foreach (XmlNode channelNode in channelNodes)
                {
                    if (channelNode == null)
                    {
                        // just to be sure
                        continue;
                    }

                    // get channel properties
                    string? channelTitle = channelNode?.SelectSingleNode("title")?.Value;
                    string? channelLink = channelNode?.SelectSingleNode("link")?.Value;
                    string? channelDesc = channelNode?.SelectSingleNode("description")?.Value;
                    string? channelCategory = channelNode?.SelectSingleNode("category")?.Value;
                    string? channelCopyright = channelNode?.SelectSingleNode("copyright")?.Value;

                    // create channel item
                    RssChannel channel = new RssChannel
                    {
                        Title = channelTitle ?? string.Empty,
                        Link = channelLink ?? string.Empty,
                        Description = channelDesc ?? string.Empty,
                        Category = channelCategory ?? string.Empty,
                        Copyright = channelCopyright ?? string.Empty
                    };

                    // read channel items
                    var itemNodes = channelNode?.SelectNodes("item");
                    if (itemNodes == null || itemNodes.Count == 0)
                    {
                        Log.Warning("Channel contains no items.", nameof(ReadChannels));
                        continue;
                    }

                    foreach (XmlNode itemNode in itemNodes)
                    {
                        // gets a single channel item
                        FeedItem item = new FeedItem
                        {
                            Title = itemNode?.SelectSingleNode("title")?.Value ?? string.Empty,
                            Link = itemNode?.SelectSingleNode("link")?.Value ?? string.Empty,
                            Description = itemNode?.SelectSingleNode("description")?.Value ?? string.Empty,
                            Author = itemNode?.SelectSingleNode("author")?.Value ?? string.Empty,
                            Category = itemNode?.SelectSingleNode("category")?.Value ?? string.Empty,
                            Comments = itemNode?.SelectSingleNode("comments")?.Value ?? string.Empty,
                            Guid = itemNode?.SelectSingleNode("guid")?.Value ?? string.Empty,
                            PublicationDate = itemNode?.SelectSingleNode("pubDate")?.Value ?? string.Empty
                        };

                        // adds item to the channel
                        channel.Items.Add(item);
                    }

                    // add channel to return list
                    channels.Add(channel);
                }
            }

            return channels;
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(ReadChannels));
            return [];
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
