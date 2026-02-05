using FZCore;

using PathFinder.Data;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PathFinder.Core;

/// <summary>
/// Representing core functions.
/// </summary>
public static class FileSystem
{
    private const string FILTER_ALL = "*";

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
    /// Retrieves all subfolders in the given <paramref name="folderPath"/>.
    /// </summary>
    /// <param name="folderPath">target folder path.</param>
    /// <param name="includeHidden">Determines whether to include hidden folders in the <paramref name="outputList"/>.</param>
    /// <param name="outputList">Output list object where all the entries will be stored.</param>
    /// <returns>Operation result.</returns>
    public static bool FsFetchAllFolders(string folderPath,
                                         bool includeHidden,
                                         out List<string> outputList)
    {
        outputList = new List<string>();

        if (Directory.Exists(folderPath) == false)
        {
            Log.Error($"Folder \'{folderPath}\' not found.", nameof(FsFetchAllFolders));
            return false;
        }

        try
        {
            if (includeHidden == true)
            {
                EnumerationOptions options = new EnumerationOptions
                {
                    IgnoreInaccessible = true
                };

                // return the entire list
                IEnumerable<string> dirs = Directory.EnumerateDirectories(folderPath, FILTER_ALL, options);
                outputList.AddRange(dirs);
                return true;
            }

            else
            {
                EnumerationOptions options = new EnumerationOptions
                {
                    AttributesToSkip = FileAttributes.Hidden | FileAttributes.System,
                    RecurseSubdirectories = false,
                    IgnoreInaccessible = true
                };

                IEnumerable<string> dirs = Directory.EnumerateDirectories(folderPath, FILTER_ALL, options);
                outputList.AddRange(dirs);
            }

            return true;
        }

        catch (Exception ex)
        {
            Log.Error(ex);
            return false;
        }
    }

    /// <summary>
    /// Retrieves all files in the given <paramref name="folderPath"/>.
    /// </summary>
    /// <param name="folderPath">target folder path.</param>
    /// <param name="includeHidden">Determines whether to include hidden files in the <paramref name="outputList"/>.</param>
    /// <param name="outputList">Output list object where all the entries will be stored.</param>
    /// <returns>Operation result.</returns>
    //public static bool FsFetchAllFiles(string folderPath,
    //                                     bool includeHidden,
    //                                     out List<string> outputList)
    //{
    //    outputList = new List<string>();

    //    if (Directory.Exists(folderPath) == false)
    //    {
    //        Log.Error($"Folder \'{folderPath}\' not found.", nameof(FsFetchAllFiles));
    //        return false;
    //    }

    //    try
    //    {
    //        if (includeHidden == true)
    //        {
    //            // return the entire list
    //            IEnumerable<string> files = Directory.EnumerateFiles(folderPath);
    //            outputList.AddRange(files);
    //            return true;
    //        }

    //        else
    //        {
    //            // filter out the hidden items
    //            EnumerationOptions options = new EnumerationOptions
    //            {
    //                AttributesToSkip = FileAttributes.Hidden | FileAttributes.System,
    //                RecurseSubdirectories = false
    //            };

    //            IEnumerable<string> files = Directory.EnumerateFiles(folderPath, FILTER_ALL, options);
    //            outputList.AddRange(files);
    //        }

    //        return true;
    //    }

    //    catch (Exception ex)
    //    {
    //        Log.Error(ex);
    //        return false;
    //    }
    //}

    public static bool FsFetchAllFiles(string folderPath, bool includeHidden, out List<string> outputList)
    {
        outputList = new List<string>();

        // Ošetření Long Path prefixu
        string preparedPath = folderPath;
        if (!preparedPath.StartsWith(@"\\?\") && Path.IsPathRooted(preparedPath))
        {
            preparedPath = @"\\?\" + preparedPath;
        }

        try
        {
            EnumerationOptions options = new EnumerationOptions
            {
                AttributesToSkip = includeHidden ? 0 : FileAttributes.Hidden,
                IgnoreInaccessible = true,
                RecurseSubdirectories = false
            };

            // Directory.EnumerateFiles s prefixem \\?\ zvládne i extrémně dlouhé názvy
            var files = Directory.EnumerateFiles(preparedPath, FILTER_ALL, options);

            // Při ukládání do seznamu můžete prefix zase odstranit, aby s tím zbytek aplikace mohl pracovat
            outputList.AddRange(files);

            return true;
        }

        catch (Exception ex)
        {
            Log.Error(ex);
            return false;
        }
    }

    /// <summary>
    /// Retrieves all subfolders in the given <paramref name="folderPath"/> asynchronously.
    /// </summary>
    /// <param name="folderPath">target folder path.</param>
    /// <param name="includeHidden">Determines whether to include hidden folders in the <paramref name="outputList"/>.</param>
    /// <param name="outputList">Output list object where all the entries will be stored.</param>
    /// <returns>Operation result.</returns>
    public static async Task<bool> FsFetchAllFoldersAsync(string folderPath,
                                                          bool includeHidden,
                                                          List<string> outputList)
    {
        return FsFetchAllFolders(folderPath, includeHidden, out outputList);
    }

    /// <summary>
    /// Retrieves all files in the given <paramref name="folderPath"/> asynchronously.
    /// </summary>
    /// <param name="folderPath">target folder path.</param>
    /// <param name="includeHidden">Determines whether to include hidden files in the <paramref name="outputList"/>.</param>
    /// <param name="outputList">Output list object where all the entries will be stored.</param>
    /// <returns>Operation result.</returns>
    public static async Task<bool> FsFetchAllFilesAsync(string folderPath,
                                                        bool includeHidden,
                                                        List<string> outputList)
    {
        return FsFetchAllFiles(folderPath, includeHidden, out outputList);
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
