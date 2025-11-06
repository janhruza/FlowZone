using FZCore;

using System;
using System.IO;

namespace PassFort.Core;

/// <summary>
/// Representing a single password entry item.
/// </summary>
public class PasswordEntry
{
    #region Static code

    /// <summary>
    /// Reads the password entry from the raw byte stream.
    /// </summary>
    /// <param name="data">Raw bytes stream containing the password entry.</param>
    /// <returns>The read password entry, otherwise null.</returns>
    public static PasswordEntry? ReadFromData(ReadOnlySpan<byte> data)
    {
        try
        {
            if (data.Length == 0)
            {
                Log.Error("The input contains no data.", nameof(ReadFromData));
                return null;
            }

            using (MemoryStream ms = new MemoryStream(data.ToArray()))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    PasswordEntry entry = new PasswordEntry
                    {
                        Id = new Guid(br.ReadBytes(16)),
                        CreationTime = DateTime.FromBinary(br.ReadInt64()),
                        Title = br.ReadString(),
                        Address = br.ReadString(),
                        Username = br.ReadString(),
                        Password = br.ReadString(),
                        Notes = br.ReadString()
                    };

                    return entry;
                }
            }
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(ReadFromData));
            return null;
        }
    }

    /// <summary>
    /// Writes the given <paramref name="entry"/> into a stream of bytes.
    /// </summary>
    /// <param name="entry">Target password entry to be sequenced.</param>
    /// <returns>A memory stream containing the sequenced entry data or null, if an error occurred.</returns>
    public static MemoryStream? WriteRawData(PasswordEntry? entry)
    {
        try
        {
            if (entry == null)
            {
                Log.Error($"No entry provided.", nameof(WriteRawData));
                return null;
            }

            MemoryStream ms;
            using (ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(entry.Id.ToByteArray());
                    bw.Write(entry.CreationTime.ToBinary());
                    bw.Write(entry.Title);
                    bw.Write(entry.Address);
                    bw.Write(entry.Username);
                    bw.Write(entry.Password);
                    bw.Write(entry.Notes);
                }
            }

            return ms;
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(WriteRawData));
            return null;
        }
    }

    #endregion

    /// <summary>
    /// Creates a new instance of the <see cref="PasswordEntry"/> class with default values.
    /// </summary>
    public PasswordEntry()
    {
        Id = Guid.CreateVersion7();
        CreationTime = DateTime.Now;
        Title = string.Empty;
        Address = string.Empty;
        Username = string.Empty;
        Password = string.Empty;
        Notes = string.Empty;
    }

    /// <summary>
    /// Representing the ID of the password entry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Representing the password entry creation time.
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// Representing the title of the password entry.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Representing the service web address (URL address).
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Representing the username for this password entry.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Representing the base64 encoded version of the encrypted password.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Representing the additional notes specified by the user.
    /// </summary>
    public string Notes { get; set; }
}
