using System;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FlowPlay.Core;

namespace FlowPlay.Controls;

/// <summary>
/// Representing the double-buffered media player.
/// It holds 2 media elements for smooth transitions between songs.
/// This control derives directly from the <see cref="UIElement"/> class.
/// </summary>
public class DoublePlayer : UIElement
{
    #region Fields

    private int playlistIndex;
    private MediaElement _pCurrent;
    private MediaElement _pNext;

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a new instance of the <see cref="DoublePlayer"/> class.
    /// </summary>
    public DoublePlayer()
    {
        playlistIndex = 0;
        Playlist = new MediaPlaylist();
        Position = TimeSpan.Zero;

        this._pCurrent = new MediaElement
        {
            LoadedBehavior = MediaState.Manual
        };

        _pCurrent.MediaEnded += (s, e) =>
        {
            // swap players and play the next song
            SwapPlayers();
            _pCurrent.Play();
        };

        _pCurrent.MediaOpened += (s, e) =>
        {
            // load buffer with the first player simultaneously
            if (QueueNext(Playlist.Tracks[playlistIndex]) == true)
            {
                // advance the current index
                playlistIndex++;
            }

            else
            {
                // playlist ended, reset index to default
                playlistIndex = 0;
            }
        };

        this._pNext = new MediaElement
        {
            LoadedBehavior = MediaState.Manual
        };
    }

    #endregion

    /// <summary>
    /// Representing the media playlist.
    /// </summary>
    public MediaPlaylist Playlist { get; private set; }

    /// <summary>
    /// Loads the given <paramref name="playlist"/> and validates it.
    /// </summary>
    /// <param name="playlist">A playlist to be loaded and validated. Validation will remove unavailable tracks.</param>
    /// <returns>A task as the validation is asynchronous.</returns>
    public async Task LoadPlaylist(MediaPlaylist playlist)
    {
        await ValidatePlaylist(playlist);
        Playlist = playlist;
        return;
    }

    private void SwapPlayers()
    {
        // tuple swap
        (_pNext, _pCurrent) = (_pCurrent, _pNext);
        return;
    }

    private Task ValidatePlaylist(MediaPlaylist playlist)
    {
        return Task.Run(() =>
        {
            foreach (string track in playlist.Tracks)
            {
                if (File.Exists(track) == false)
                {
                    playlist.Tracks.Remove(track);
                }
            }
        });
    }

    /// <summary>
    /// Loads a file (<paramref name="playlistItem"/>) into the second media player (buffer).
    /// </summary>
    /// <param name="playlistItem">Path to the media file (playlist item).</param>
    public bool QueueNext(string playlistItem)
    {
        if (Playlist.Tracks.Any() == false)
        {
            // playlist ended
            return false;
        }

        _pNext.Source = new Uri(playlistItem, UriKind.RelativeOrAbsolute);
        _pNext.Position = TimeSpan.Zero;
        return true;
    }

    /// <summary>
    /// Loads a file (<paramref name="playlistItem"/>) into the first media player.
    /// </summary>
    /// <param name="playlistItem"></param>
    public void OpenMedia(string playlistItem)
    {
        _pCurrent.Source = new Uri(playlistItem, UriKind.RelativeOrAbsolute);
        _pCurrent.Position = TimeSpan.Zero;
        return;
    }

    /// <summary>
    /// Plays current media from current position.
    /// </summary>
    public void Play() => _pCurrent.Play();

    /// <summary>
    /// Pauses current media at current position.
    /// </summary>
    public void Pause() => _pCurrent.Pause();

    /// <summary>
    /// Stops and resets current media to be played from the beginning.
    /// </summary>
    public void Stop() => _pCurrent.Stop();

    /// <summary>
    /// Gets or sets the current position of progress through the current media's playback time.
    /// </summary>
    public TimeSpan Position
    {
        get => _pCurrent.Position;
        set => _pCurrent.Position = value;
    }
}
