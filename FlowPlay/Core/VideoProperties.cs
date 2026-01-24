using System;

namespace FlowPlay.Core;

/// <summary>
/// Representing a struct with information about a video file.
/// </summary>
public struct VideoProperties
{
    /// <summary>
    /// Representing the path to the file.
    /// </summary>
    public string Path;

    /// <summary>
    /// Gets the video width in pixels.
    /// </summary>
    public int Width;

    /// <summary>
    /// Gets the video height in pixels.
    /// </summary>
    public int Height;

    /// <summary>
    /// Gets the length of the video.
    /// </summary>
    public TimeSpan Duration;
}
