using FlowPlay.Core;

using FZCore;

using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FlowPlay.Controls;

/// <summary>
/// Representing the double-buffered media player.
/// It holds 2 media elements for smooth transitions between songs.
/// This control derives directly from the <see cref="UIElement"/> class.
/// </summary>
public class DoublePlayer : Control
{
    #region Fields

    private int playlistIndex;
    private MediaElement _pCurrent;
    private MediaElement _pNext;

    private bool _canNext => (Playlist.Tracks.Count - 1) > playlistIndex;
    private bool _canPrev => playlistIndex >= 0;

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
            LoadedBehavior = MediaState.Manual,
            Visibility = Visibility.Visible
        };

        _pCurrent.MediaEnded += (s, e) =>
        {
            // swap players and play the next song
            if (_canNext)
            {
                PlayNext();
            }
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
    public void LoadPlaylist(MediaPlaylist playlist)
    {
        ValidatePlaylist(playlist);
        Playlist = playlist;
        return;
    }

    /// <summary>
    /// Opens a single media file and begins playing it.
    /// </summary>
    /// <param name="fileName">Path to the media file.</param>
    /// <returns>A new task as this method awaits the <see cref="ValidatePlaylist(MediaPlaylist)"/> method.</returns>
    public void PlaySingleTrack(string fileName)
    {
        if (File.Exists(fileName) == false)
        {
            Log.Error($"Target media file \'{fileName}\' was not found.", nameof(PlaySingleTrack));
            return;
        }

        MediaPlaylist playlist = new MediaPlaylist
        {
            Tracks = [fileName]
        };

        Playlist = playlist;
        OpenMedia(fileName);
        Play();
        return;
    }

    private void SwapPlayers()
    {
        // reset next (new current) player
        _pNext.Position = TimeSpan.Zero;

        // tuple swap
        (_pNext, _pCurrent) = (_pCurrent, _pNext);
        return;
    }

    private void ValidatePlaylist(MediaPlaylist playlist)
    {
        foreach (string track in playlist.Tracks)
        {
            if (File.Exists(track) == false)
            {
                playlist.Tracks.Remove(track);
            }
        }
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
    /// This method will also resets the current playlist index position to 0.
    /// </summary>
    /// <param name="playlistItem">Path to the media file.</param>
    public void OpenMedia(string playlistItem)
    {
        _pCurrent.Source = new Uri(playlistItem, UriKind.RelativeOrAbsolute);
        _pCurrent.Position = TimeSpan.Zero;
        Session.Track = playlistItem;
        playlistIndex = 0;
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
        get
        {
            if (_pCurrent == null) return TimeSpan.Zero;
            return _pCurrent.Position;
        }

        set
        {
            if (_pCurrent == null) return;
            _pCurrent.Position = value;
        }
    }

    /// <summary>
    /// Determines whether the player can play previous track.
    /// Previous track means a track in the playlist in front of the current one.
    /// </summary>
    public bool CanPrevious => _canPrev;

    /// <summary>
    /// Determines whether player can play next track.
    /// Next track means a track in the playlist after the current one.
    /// </summary>
    public bool CanNext => _canNext;

    /// <summary>
    /// Plays the next track in the loaded <see cref="Playlist"/> if any.
    /// </summary>
    public void PlayNext()
    {
        if (_canNext)
        {
            SwapPlayers();
            _pCurrent.Play();
            Session.Track = _pCurrent.Source.AbsolutePath;
        }

        return;
    }

    /// <summary>
    /// Plays the previous track in the <see cref="Playlist"/> if any.
    /// </summary>
    public void PlayPrev()
    {
        if (_canPrev)
        {
            playlistIndex--;
            QueueNext(Playlist.Tracks[playlistIndex]);
            SwapPlayers();
            _pCurrent.Play();
            Session.Track = _pCurrent.Source.AbsolutePath;
        }

        return;
    }

    /// <summary>
    /// Attempts to rewind current media by the specified number of <paramref name="milliseconds"/>.
    /// </summary>
    /// <param name="milliseconds">
    /// Number of milliseconds to rewind.
    /// If this number is lower than current millisecond position of the loaded media,
    /// the media position is set to the beginning.
    /// </param>
    public void Rewind(double milliseconds)
    {
        double value = Position.TotalMilliseconds - milliseconds;
        if (value > 0)
        {
            Position = TimeSpan.FromMilliseconds(value);
        }

        else
        {
            Position = TimeSpan.Zero;
        }
    }

    /// <summary>
    /// Attempts to skip current media by specified number of <paramref name="milliseconds"/>.
    /// If the currently loaded media duration has no <see cref="TimeSpan"/>, it returns without any action.
    /// </summary>
    /// <param name="milliseconds">
    /// Number of milliseconds from the current player position to skip.
    /// If this number is greater than total number of milliseconds of the media file,
    /// the next song is played.
    /// </param>
    public void Skip(double milliseconds)
    {
        if (_pCurrent.NaturalDuration.HasTimeSpan == false)
        {
            Log.Error($"Current player has no timespan.", nameof(Skip));
            return;
        }

        double value = Position.TotalMilliseconds + milliseconds;
        if (value >= _pCurrent.NaturalDuration.TimeSpan.TotalMilliseconds)
        {
            PlayNext();
        }

        else
        {
            Position = TimeSpan.FromMicroseconds(value);
        }

        return;
    }

    /// <summary>
    /// Gets or sets the media volume. The valid values are between 0 and 1. Default value is 0.5.
    /// </summary>
    public double Volume
    {
        get => _pCurrent.Volume;
        set
        {
            _pCurrent.Volume = value;
            _pNext.Volume = value;
        }
    }
}