namespace FlowPlay.Core;

/// <summary>
/// Representing a simpe media tag info struct.
/// </summary>
public struct AudioTagInfo
{
    /// <summary>
    /// Representing the artist name.
    /// </summary>
    public string Artist;

    /// <summary>
    /// Representing the title name.
    /// </summary>
    public string Title;

    /// <summary>
    /// Representing the year of the media.
    /// </summary>
    public int Year;

    /// <summary>
    /// Representing the album name.
    /// </summary>
    public string Album;

    /// <summary>
    /// Representing an empty audio tag info.
    /// </summary>
    public static readonly AudioTagInfo Empty = new AudioTagInfo
    {
        Artist = "Unknown artist",
        Title = "Unknown title",
        Year = 0,
        Album = "Unknown album"
    };
}
