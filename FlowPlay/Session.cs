using FlowPlay.Controls;

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

    private static void SetTrack(string value)
    {
        _player.OpenMedia(value);
    }

    /// <summary>
    /// Gets or sets the currently played track.
    /// </summary>
    public static string Track
    {
        get => Track;
        set
        {
            Track = value;
            SetTrack(value);
        }
    }
}
