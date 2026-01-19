using FZCore;

using System.Collections.Generic;
using System.IO;

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

        /// <summary>
        /// Adds a track into the playlist.
        /// </summary>
        /// <param name="fileName">Path to the media file.</param>
        /// <returns>True, if item was added, otherwise false.</returns>
        public bool Add(string fileName)
        {
            if (File.Exists(fileName) == false)
            {
                Log.Error($"Target file \'{fileName}\' was not found.", nameof(Add));
                return false;
            }

            Tracks.Add(fileName);
            return true;
        }

        /// <summary>
        /// Removes a track from the playlist.
        /// </summary>
        /// <param name="fileName">Path to the media file.</param>
        /// <returns>True, if media file is removed from the playlist, otherwise false.</returns>
        public bool Remove(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) == true)
            {
                Log.Error("Target file is null.", nameof(Remove));
                return false;
            }

            if (Tracks.Contains(fileName) == false)
            {
                Log.Error($"Target file \'{fileName}\' is not present in the playlist.", nameof(Remove));
                return false;
            }

            Tracks.Remove(fileName);
            return true;
        }
    }
}
