using System;
using System.Threading.Tasks;
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
            _ = OpenFolder(FolderName);
        }
    }

    /// <inheritdoc/>
    public event EventHandler FolderOpened = delegate { };

    /// <inheritdoc/>
    public async Task<bool> OpenFolder(string folderPath)
    {
        FolderChanged.Invoke(this, folderPath);
        return false;
    }

    /// <inheritdoc/>
    public object CreateParentFolderItem(string folderPath)
    {
        return new object();
    }

    /// <summary>
    /// Occurrs when the current folder changes.
    /// </summary>
    public event EventHandler<string> FolderChanged = delegate { };
}
