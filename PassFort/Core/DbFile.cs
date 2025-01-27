using System;
using System.Collections.Generic;
using System.Linq;

namespace PassFort.Core;

/// <summary>
/// Representing the password database file handling class.
/// </summary>
public class DbFile
{
    /// <summary>
    /// Creates a new instance of the <see cref="DbFile"/> class with default values.
    /// </summary>
    public DbFile()
    {
        _filePath = string.Empty;
        _formatVersion = 1;

        // generate key
        byte[] buffer = new byte[32];
        Random.Shared.NextBytes(buffer);

        // assign the key as the loaded one
        _key = buffer;
        _loadedKey = [.. buffer];
    }

    private string _filePath;
    private byte _formatVersion;
    private byte[] _key;

    #region Static code

    private static List<byte> _loadedKey = [];

    /// <summary>
    /// Representing the currently loaded password key. This value is file-based.
    /// </summary>
    public static List<byte> LoadedKey => _loadedKey;

    #endregion
}
