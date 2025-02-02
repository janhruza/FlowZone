using System;
using System.Collections.Generic;
using System.Linq;

namespace PassFort.Core;

/*
 * PasswordEntry file data structure
 * 
 * PROPERTY                 SIZE        DESCRIPTION
 * Id                       16          Guid encoded as 16 bytes array
 * CreationTime             8           Creation time encoded as binary DateTime
 * ModificationDate         8           Last modificationd ate encoded as binary DateTime
 * Category                 1           Password category as a byte
 * Url                      VARIABLE    A string with the associated URL address
 * Username                 VARIABLE    A string with the user name
 * PasswordLength           4           Password length as int
 * Password                 VARIABLE    A byte array with the password and IV data. Size of those data is defined with the previous 4 bytes.
 * Notes                    VARIABLE    A string with additional password entry notes
 */

/// <summary>
/// Representing the structure of a single password entry.
/// </summary>
public struct PasswordEntry
{
    /// <summary>
    /// Creates a new empty <see cref="PasswordEntry"/> with default parameters.
    /// </summary>
    public PasswordEntry()
    {
        // private fields
        _password = [];

        // public fields
        Id = Guid.NewGuid();
        Name = string.Empty;
        CreationTime = DateTime.Now;
        ModificationTIme = DateTime.Now;
        Category = PasswordCategory.None;
        Url = string.Empty;
        Username = string.Empty;
        Notes = string.Empty;
    }

    private List<byte> _password;

    /// <summary>
    /// Representing the unique identifier for this password entry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Representing the user given entry name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Representing the date and time of when the entry was created.
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// Representing the date and time of the last entry modification.
    /// </summary>
    public DateTime ModificationTIme { get; set; }

    /// <summary>
    /// Representing the category of the password entry.
    /// </summary>
    public PasswordCategory Category { get; set; }

    /// <summary>
    /// Representing the associated service URL address.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Representing the username for this password entry.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Representing the password.
    /// </summary>
    public string Password
    {
        get => DecryptPassword(_password.ToArray(), DbFile.LoadedKey.ToArray());
        set => _password = EncryptPassword(value, DbFile.LoadedKey.ToArray()).ToList();
    }

    /// <summary>
    /// Representing additional entry notes, or any other user-defined text.
    /// </summary>
    public string Notes { get; set; }

    #region Static code

    private static byte[] EncryptPassword(string password, byte[] key)
    {
        return AesHelper.EncryptPassword(password, key);
    }

    private static string DecryptPassword(byte[] password, byte[] key)
    {
        return AesHelper.DecryptPassword(password, key);
    }

    /// <summary>
    /// Gets the encrypted password data length.
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    public static int GetPasswordLength(PasswordEntry entry)
    {
        return entry._password.Count;
    }

    /// <summary>
    /// Gets the encrypted password data in a raw form.
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    public static byte[] GetRawPasswordData(PasswordEntry entry)
    {
        return entry._password.ToArray();
    }

    /// <summary>
    /// Directly sets the password <paramref name="data"/> into the given password <paramref name="entry"/>.
    /// </summary>
    /// <param name="entry"></param>
    /// <param name="data"></param>
    public static void SetPasswordData(PasswordEntry entry, byte[] data)
    {
        entry._password = data.ToList();
        return;
    }

    /// <summary>
    /// Determines whether the given <paramref name="entry"/> is valid or not.
    /// </summary>
    /// <param name="entry">Target entry to be checked.</param>
    /// <returns>True, if the <paramref name="entry"/> is valid, otherwise false.</returns>
    public static bool IsEntryValid(PasswordEntry entry)
    {
        if (string.IsNullOrEmpty(entry.Name.Trim()) == true) return false;
        if (string.IsNullOrEmpty(entry.Username.Trim()) == true) return false;
        if (string.IsNullOrEmpty(entry.Password.Trim()) == true) return false;

        return true;
    }

    /// <summary>
    /// Filters target entries from the given list of <paramref name="entries"/> those who have the target <paramref name="category"/>.
    /// </summary>
    /// <param name="entries">List of all entries to filter from.</param>
    /// <param name="category">A target password entry category.</param>
    /// <returns></returns>
    public static List<PasswordEntry> FilterEntries(List<PasswordEntry> entries, PasswordCategory category)
    {
        if (entries.Count == 0)
        {
            // no entries to filter from
            return new List<PasswordEntry>();
        }

        return entries.Where(x => x.Category == category).ToList();

    }

    #endregion
}
