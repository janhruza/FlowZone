using System;
using System.Collections.Generic;
using System.IO;
using FZCore;

namespace PassFort.Core;

/*
 * DbFile data structure
 * 
 * PROPERTY         SIZE        DESCRIPTION
 * Header           8           Description header. Determines that the file is a valid DbFile.
 * Version          1           Version of the file format as byte.
 * Key              32          AES encryption key
 * Name             VARIABLE    User-defined name of the database.
 * PasswordCount    4           Number of stored passwords as Int32.
 * PasswordEntries  VARIABLE    All stored password entries. Number of them is described in the previous 4 bytes (Int32)
 */

/// <summary>
/// Representing the password database file handling class.
/// </summary>
public class DbFile
{
    /// <summary>
    /// Representing the file header.
    /// </summary>
    public const string HEADER = "PassFort";

    /// <summary>
    /// Representing the key size.
    /// </summary>
    public const int KEY_SIZE = 32;

    /// <summary>
    /// Creates a new instance of the <see cref="DbFile"/> class with default values.
    /// </summary>
    /// <param name="filePath">Database file path - used only when creating a new DB file.</param>
    public DbFile(string? filePath = null)
    {
        _filePath = filePath ?? string.Empty;
        _formatVersion = 1;
        _entries = [];
        _name = string.Empty;

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
    private List<PasswordEntry> _entries;
    private string _name;

    /// <summary>
    /// Representing the user name of the database.
    /// </summary>
    public string Name
    {
        get => _name;
        set => _name = value;
    }

    #region Static code

    private static List<byte> _loadedKey = [];

    /// <summary>
    /// Representing the currently loaded password key. This value is file-based.
    /// </summary>
    public static List<byte> LoadedKey => _loadedKey;

    /// <summary>
    /// Representing currently opened database file.
    /// </summary>
    public static DbFile? Current {  get; set; }

    /// <summary>
    /// Creaes a new instance of the <see cref="DbFile"/> class.
    /// </summary>
    /// <returns></returns>
    public static DbFile CreateNew()
    {
        return new DbFile();
    }

    /// <summary>
    /// Attempts to open the given database file located at <paramref name="filePath"/>.
    /// </summary>
    /// <param name="filePath">Path to the database file.</param>
    /// <param name="file">A location where the file data will be loaded.</param>
    /// <returns>True if the opening was successful, otherwise false.</returns>
    public static bool Open(string filePath, out DbFile file)
    {
        file = new DbFile();
        file._filePath = filePath;

        try
        {
            using (FileStream fs = File.OpenRead(filePath))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    // invalid file check
                    if (br.ReadString() != HEADER) return false;

                    file._formatVersion = br.ReadByte();
                    file._key = br.ReadBytes(KEY_SIZE);
                    file._name = br.ReadString();
                    int entriesCount = br.ReadInt32();

                    for (int x = 0; x < entriesCount; x++)
                    {
                        PasswordEntry entry = br.ReadPasswordEntry();
                        file._entries.Add(entry);
                    }
                }
            }
            return true;
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(DbFile.Open));
            return false;
        }
    }

    /// <summary>
    /// Attempts to save the database <paramref name="file"/>.
    /// </summary>
    /// <param name="file">Target database file structure.</param>
    /// <returns>True if saving was successful, otherwise false.</returns>
    public static bool Save(DbFile file)
    {
        try
        {
            using (FileStream fs = File.Create(file._filePath))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(DbFile.HEADER);
                    bw.Write(file._formatVersion);
                    bw.Write(file._key);
                    bw.Write(file._name);
                    bw.Write(file._entries.Count);

                    for (int x = 0; x < file._entries.Count; x++)
                    {
                        bw.WritePasswordEntry(file._entries[x]);
                    }
                }
            }

            return true;
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(DbFile.Save));
            return false;
        }
    }

    #endregion
}
