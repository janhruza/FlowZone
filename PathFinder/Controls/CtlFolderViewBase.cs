using System;
using System.Windows.Controls;

namespace PathFinder.Controls;

/// <summary>
/// Representing a base class for all folder view controls.
/// </summary>
public class CtlFolderViewBase : UserControl, IFolderView
{
    /// <inheritdoc/>
    public string FolderName { get; } = string.Empty;

    /// <inheritdoc/>
    public bool SortFoldersFirst
    {
        get => field;
        set
        {
            field = value;
            OpenFolder(FolderName);
        }
    }

    /// <inheritdoc/>
    public event EventHandler FolderOpened = delegate { };

    /// <inheritdoc/>
    public bool OpenFolder(string folderPath)
    {
        return false;
    }

    /// <inheritdoc/>
    public object CreateParentFolderItem(string folderPath)
    {
        return new object();
    }
}
