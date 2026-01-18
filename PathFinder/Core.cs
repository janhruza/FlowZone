using FZCore;

using PathFinder.Data;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace PathFinder;

/// <summary>
/// Representing core functions.
/// </summary>
public static class Core
{
    /// <summary>
    /// Representing a text of a separator item placeholder.
    /// </summary>
    public static readonly string TEXT_SEPARATOR = new string('-', 80);

    /// <summary>
    /// Gets all filesystem entries from the given directory.
    /// </summary>
    /// <param name="folderPath">Target folder path.</param>
    /// <param name="sortFoldersFirst">Determines whether to sort directories before files.</param>
    /// <param name="list">Output list object.</param>
    /// <returns>Operation result.</returns>
    public static bool FsGetAllEntries(string folderPath,
                                       bool sortFoldersFirst,
                                       out List<string> list)
    {
        // create new list
        list = new List<string>();

        if (Directory.Exists(folderPath) == false)
        {
            Log.Error($"Directory \'{folderPath}\' not found.", nameof(FsGetAllEntries));
            return false;
        }

        if (sortFoldersFirst == false)
        {
            try
            {
                list = Directory.GetFileSystemEntries(folderPath).ToList();
            }
            
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        else
        {
            try
            {
                // get directories first
                string[] dirs = Directory.GetDirectories(folderPath);
                list.AddRange(dirs);

                // add separator
                list.Add(TEXT_SEPARATOR);

                // get files after directories
                string[] files = Directory.GetFiles(folderPath);
                list.AddRange(files);
            }

            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Gets information about the given filesystem object.
    /// Must be file or directory.
    /// </summary>
    /// <param name="path">Object path.</param>
    /// <param name="info">Output object.</param>
    /// <returns>Operation result.</returns>
    public static bool FsGetItemInfo(string path, [NotNull] out FSObjectInfo info)
    {
        if (path == TEXT_SEPARATOR)
        {
            info = new FSObjectInfo
            {
                IsFile = false,
                Info = new object(),
                Exists = false,
                IsSpecial = true
            };

            return true;
        }

        if (Path.Exists(path) == false)
        {
            info = new FSObjectInfo
            {
                Exists = false,
                IsSpecial = false
            };

            return false;
        }

        if (File.Exists(path) == true)
        {
            info = new FSObjectInfo
            {
                Exists = true,
                Info = new FileInfo(path),
                IsFile = true,
                IsSpecial = false
            };

            return info.Exists;
        }

        else if (Directory.Exists(path) == true)
        {
            info = new FSObjectInfo
            {
                Exists = true,
                Info = new DirectoryInfo(path),
                IsFile = false,
                IsSpecial = false
            };

            return info.Exists;
        }

        else
        {
            info = new FSObjectInfo
            {
                Exists = false
            };

            return false;
        }
    }
}
