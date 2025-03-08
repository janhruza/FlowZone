using System.Collections.Generic;

namespace UpDate.Core;

/// <summary>
/// Representing a single RSS channel item.
/// </summary>
public struct RssChannel
{
    #region Static methods

    /// <summary>
    /// Determines whether the given ´<paramref name="channel"/> is valid or default.
    /// </summary>
    /// <param name="channel">Reference to the target RSS channel item.</param>
    /// <returns>True if the channel is not default, otherwise false.</returns>
    public static bool IsValid(ref RssChannel channel)
    {
        return (string.IsNullOrEmpty(channel.Title) == false &&
                string.IsNullOrEmpty(channel.Link) == false &&
                string.IsNullOrEmpty(channel.Description) == false);
    }

    #endregion

    /// <summary>
    /// Creates a new, empty <see cref="RssChannel"/> item.
    /// </summary>
    public RssChannel()
    {
        Title = string.Empty;
        Link = string.Empty;
        Description = string.Empty;
        Items = [];
        Category = string.Empty;
        Copyright = string.Empty;
    }

    /// <summary>
    /// Representing the title of the RSS channel.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Representing the source of the RSS channel.
    /// </summary>
    public string Link { get; set; }

    /// <summary>
    /// Representing the description of the RSS channel.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Representing the list of all RSS channel items.
    /// </summary>
    public List<FeedItem> Items { get; set; }

    /// <summary>
    /// Representing the category of the RSS channel.
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// Representing the copyright of the RSS channel.
    /// </summary>
    public string Copyright { get; set; }
}
