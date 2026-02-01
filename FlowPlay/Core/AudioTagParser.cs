using System;
using System.IO;
using System.Text;

namespace FlowPlay.Core;

/// <summary>
/// Representing a simple audio tag parser class.
/// </summary>
public static class AudioTagParser
{
    /// <summary>
    /// Parses basic ID3v1 metadata from an MP3 file.
    /// </summary>
    /// <param name="filename">The full path to the MP3 file.</param>
    /// <param name="info">When this method returns, contains the parsed metadata if successful.</param>
    /// <returns>True if the file exists and contains valid ID3v1 tags; otherwise, false.</returns>
    public static bool ParseMp3File(string filename, out AudioTagInfo info)
    {
        // Default values
        info = new AudioTagInfo
        {
            Artist = "Unknown Artist",
            Title = "Unknown Title",
            Year = 0,
            Album = "Unknown Album"
        };

        if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
        {
            // File does not exist or invalid filename
            return false;
        }

        try
        {
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                // ID3v1 tags are stored in the last 128 bytes of the file
                if (fs.Length < 128) return false;

                byte[] buffer = new byte[128];
                fs.Seek(-128, SeekOrigin.End);
                fs.Read(buffer, 0, 128);

                // Check for the "TAG" identifier at the beginning of the 128-byte block
                string identifier = Encoding.ASCII.GetString(buffer, 0, 3);
                if (identifier == "TAG")
                {
                    // Extract and clean metadata strings
                    info.Title = ExtractString(buffer, 3, 30);
                    info.Artist = ExtractString(buffer, 33, 30);
                    info.Album = ExtractString(buffer, 63, 30);

                    // Try to parse the year (stored as a 4-character string)
                    string yearStr = ExtractString(buffer, 93, 4);
                    if (int.TryParse(yearStr, out int year))
                    {
                        info.Year = year;
                    }

                    return true;
                }
            }
        }
        catch (Exception)
        {
            // Return false if the file is locked or another I/O error occurs
            return false;
        }

        return false;
    }

    /// <summary>
    /// Parses basic metadata (LIST INFO chunk) from a WAV file.
    /// </summary>
    /// <param name="filename">The full path to the WAV file.</param>
    /// <param name="info">When this method returns, contains the parsed metadata if successful.</param>
    /// <returns>True if the file is a valid WAV and metadata was successfully read; otherwise, false.</returns>
    public static bool ParseWavFile(string filename, out AudioTagInfo info)
    {
        info = new AudioTagInfo
        {
            Artist = "Unknown Artist",
            Title = "Unknown Title",
            Year = 0,
            Album = "Unknown Album"
        };

        if (string.IsNullOrEmpty(filename) || !File.Exists(filename)) return false;

        try
        {
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new BinaryReader(fs))
            {
                // Check RIFF header
                if (Encoding.ASCII.GetString(reader.ReadBytes(4)) != "RIFF") return false;
                reader.ReadInt32(); // Skip file size
                if (Encoding.ASCII.GetString(reader.ReadBytes(4)) != "WAVE") return false;

                // Search for the "LIST" chunk
                while (fs.Position < fs.Length - 8)
                {
                    string chunkId = Encoding.ASCII.GetString(reader.ReadBytes(4));
                    int chunkSize = reader.ReadInt32();

                    if (chunkId == "LIST")
                    {
                        long endOfList = fs.Position + chunkSize;
                        string listType = Encoding.ASCII.GetString(reader.ReadBytes(4));

                        if (listType == "INFO")
                        {
                            while (fs.Position < endOfList)
                            {
                                string tagId = Encoding.ASCII.GetString(reader.ReadBytes(4));
                                int tagSize = reader.ReadInt32();
                                byte[] tagData = reader.ReadBytes(tagSize);

                                // WAV tags use 4-character codes (e.g., INAM = Title)
                                string value = Encoding.UTF8.GetString(tagData).TrimEnd('\0');

                                switch (tagId.ToUpper())
                                {
                                    case "INAM": info.Title = value; break;  // Name/Title
                                    case "IART": info.Artist = value; break; // Artist
                                    case "IPRD": info.Album = value; break;  // Product/Album
                                    case "ICRD":                             // Creation Date
                                        if (int.TryParse(value.Length >= 4 ? value[..4] : "", out int year))
                                            info.Year = year;
                                        break;
                                }

                                // Chunks must be aligned to even addresses
                                if (tagSize % 2 != 0 && fs.Position < fs.Length) fs.ReadByte();
                            }
                            return true;
                        }
                    }

                    // Skip to the next chunk
                    fs.Seek(chunkSize, SeekOrigin.Current);
                    if (chunkSize % 2 != 0 && fs.Position < fs.Length) fs.ReadByte();
                }
            }
        }
        catch { return false; }

        return false;
    }

    private static string ExtractString(byte[] buffer, int offset, int length)
    {
        // Use Default encoding to handle local characters (like Czech diacritics)
        // and trim trailing null characters (\0) and whitespace
        string content = Encoding.Default.GetString(buffer, offset, length);
        return content.TrimEnd('\0', ' ');
    }
}
