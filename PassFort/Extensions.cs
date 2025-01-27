using System;
using System.IO;
using PassFort.Core;

namespace PassFort;

/// <summary>
/// Representing the various extensions to the standard classes tat can come handy.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Writes the given <paramref name="entry"/> into a file using the <paramref name="writer"/>.
    /// </summary>
    /// <param name="writer">An instance of a <see cref="BinaryWriter"/>.</param>
    /// <param name="entry">The entry to be written into the stream using the <paramref name="writer"/>.</param>
    public static void WritePasswordEntry(this BinaryWriter writer, PasswordEntry entry)
    {
        writer.Write(entry.Id.ToByteArray());
        writer.Write(entry.CreationTime.ToBinary());
        writer.Write(entry.ModificationTIme.ToBinary());
        writer.Write((byte)entry.Category);
        writer.Write(entry.Url);
        writer.Write(entry.Username);
        writer.Write(PasswordEntry.GetPasswordLength(entry));
        writer.Write(PasswordEntry.GetRawPasswordData(entry));
        writer.Write(entry.Notes);
        return;
    }

    /// <summary>
    /// Reads the <see cref="PasswordEntry"/> record from the stream using the <paramref name="reader"/>.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns>A new <see cref="PasswordEntry"/> object constructed with read data.</returns>
    public static PasswordEntry ReadPasswordEntry(this BinaryReader reader)
    {
        PasswordEntry entry = new PasswordEntry();
        entry.Id = new Guid(reader.ReadBytes(16));
        entry.CreationTime = DateTime.FromBinary(reader.ReadInt64());
        entry.ModificationTIme = DateTime.FromBinary(reader.ReadInt64());
        entry.Category = (PasswordCategory)reader.ReadByte();
        entry.Url = reader.ReadString();
        entry.Username = reader.ReadString();

        // reads the password data and assigns them to the entry
        int pLen = reader.ReadInt32();
        byte[] passwordData = reader.ReadBytes(pLen);
        PasswordEntry.SetPasswordData(entry, passwordData);

        entry.Notes = reader.ReadString();
        return entry;
    }
}
