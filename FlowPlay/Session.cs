using System.IO;
using System.Linq;
using FlowPlay.Controls;
using FlowPlay.Core;

namespace FlowPlay;

/// <summary>
/// Representing a current player session.
/// </summary>
public static class Session
{
    static Session()
    {
        _player ??= new DoublePlayer();
    }

    private static DoublePlayer _player;

    /// <summary>
    /// Gets or sets the currently played track.
    /// </summary>
    public static string Track { get; set; } = string.Empty;

    /// <summary>
    /// Representing the player's volume.
    /// </summary>
    public static double Volume
    {
        get => _player.Volume;
        set => _player.Volume = value;
    }

    /// <summary>
    /// Reinitializes the media player and load the lastly played track (if any).
    /// </summary>
    public static void RestartPlayer()
    {
        _player = new DoublePlayer();
        
        if (File.Exists(Track) == true)
        {
            _player.LoadPlaylist(new MediaPlaylist
            {
                Tracks = [Track]
            });

            _player.OpenMedia(_player.Playlist.Tracks.First());
        }

        return;
    }
}
