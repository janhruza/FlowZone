using System.Collections.Generic;

namespace FlowPlay.Core
{
    /// <summary>
    /// Representing a media playlist class.
    /// </summary>
    public class MediaPlaylist
    {
        /// <summary>
        /// Creates a new instance of the <see cref="MediaPlaylist"/> class with default settings.
        /// </summary>
        public MediaPlaylist()
        {
            Name = string.Empty;
            Tracks = [];
        }

        /// <summary>
        /// Representing the name of the playlist.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Representing the list of all files in this playlist.
        /// </summary>
        public List<string> Tracks { get; set; }
    }
}
