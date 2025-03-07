namespace UpDate.Core
{
    /// <summary>
    /// Representing a single RSS feed item.
    /// </summary>
    public struct FeedItem
    {
        /// <summary>
        /// Creates a new RSS feed item.
        /// </summary>
        public FeedItem()
        {
            Title = string.Empty;
            Link = string.Empty;
            Description = string.Empty;
            Author = string.Empty;
            Category = string.Empty;
            Comments = string.Empty;
            Guid = string.Empty;
            PublicationDate = string.Empty;
        }

        /// <summary>
        /// Representing the title of the RSS feed item.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Representing the link to the RSS feed item.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Representing the description of the RSS feed item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Representing the RSS feed item author (author of the news headline, story, etc.).
        /// This parameter is optional.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Representing the category of the RSS feed item.
        /// This parameter is optional.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Representing a link to a comments section of the RSS feed item.
        /// This parameter is optional.
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Representing the GUID of the RSS feed item.
        /// This parameter is optional.
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// Representing the publication date of the RSS feed item.
        /// This parameter is optional.
        /// </summary>
        public string PublicationDate { get; set; }
    }
}
