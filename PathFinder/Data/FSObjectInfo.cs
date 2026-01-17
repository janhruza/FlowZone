using System.IO;

namespace PathFinder.Data;

/// <summary>
/// Representing a filesystem object info struct.
/// </summary>
public struct FSObjectInfo
{
    /// <summary>
    /// Determines whether the object exists.
    /// </summary>
    public bool Exists;

    /// <summary>
    /// Representing the data object.
    /// This can be either <see cref="FileInfo"/> or <see cref="DirectoryInfo"/> class.
    /// </summary>
    public object Info;

    /// <summary>
    /// Determines whether the object is a file (<see langword="true"/>) or a directory (<see langword="false"/>).
    /// </summary>
    public bool IsFile;
}
