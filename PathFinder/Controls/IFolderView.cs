using System;

namespace PathFinder.Controls;

/// <summary>
/// Representing a common interface for all folder view controls.
/// </summary>
public interface IFolderView
{
    /// <summary>
    /// Attempts to open the given <paramref name="folderPath"/> and list its contents.
    /// </summary>
    /// <param name="folderPath">Path to the target folder.</param>
    /// <returns>Operation result.</returns>
    /// <remarks>
    /// This method should also set the value of the <see cref="FolderName"/> property.
    /// </remarks>
    bool OpenFolder(string folderPath);

    /// <summary>
    /// Gets the currently opened folder name.
    /// </summary>
    string FolderName { get; }

    /// <summary>
    /// Occurrs when a folder is opened using the <see cref="OpenFolder(string)"/> method.
    /// </summary>
    event EventHandler FolderOpened;

    /// <summary>
    /// Determines whether to sort all directories before files.
    /// </summary>
    bool SortFoldersFirst { get; set; }

    /// <summary>
    /// Creates an item in the view that represents the parent directory.
    /// </summary>
    /// <param name="folerPath">Current folder path.</param>
    /// <returns>New parent directory item. The type depends on the view type and must be handled by the programmer.</returns>
    object CreateParentFolderItem(string folerPath);
}