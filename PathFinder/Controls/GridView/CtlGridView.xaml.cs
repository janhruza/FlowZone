using FZCore;

using PathFinder.Data;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;

using static PathFinder.Core.ContextMenus;
using static PathFinder.Core.FileSystem;

namespace PathFinder.Controls.GridView;

/// <summary>
/// Representing the grid view control.
/// </summary>
public partial class CtlGridView : CtlFolderViewBase
{
    /// <inheritdoc/>
    public new object CreateParentFolderItem(string folderPath)
    {
        string parent = Directory.GetParent(folderPath)?.FullName ?? Directory.GetDirectoryRoot(folderPath);

        if (string.IsNullOrWhiteSpace(parent))
        {
            parent = "C:\\";
        }

        CtlGridViewItem item = new CtlGridViewItem
        {
            Uid = parent
        };

        item.MouseDoubleClick += async (s, e) =>
        {
            _ = await OpenFolder(parent);
        };

        return item;
    }

    private string _folderPath;

    /// <inheritdoc/>
    public new event EventHandler<string> FolderChanged = delegate { };

    /// <inheritdoc/>
    public new string FolderName => _folderPath;

    /// <inheritdoc/>
    public new async Task<bool> OpenFolder(string folderPath)
    {
        if (Directory.Exists(folderPath) == false)
        {
            Log.Error($"Folder \'{folderPath}\' not found.", nameof(OpenFolder));
            return false;
        }

        _folderPath = folderPath;
        FolderChanged.Invoke(this, folderPath);

        // clear grid
        uGrid.Children.Clear();

        // enum objects
        List<string> dirs = [];
        List<string> files = [];

        if (await FsFetchAllFoldersAsync(folderPath, true, dirs) == false)
        {
            Log.Error($"Unable to enumerate directories inside \'{folderPath}\'.", nameof(OpenFolder));
            return false;
        }

        if (await FsFetchAllFilesAsync(folderPath, true, files) == false)
        {
            Log.Error($"Unable to enumerate files inside \'{folderPath}\'.", nameof(OpenFolder));
            return false;
        }

        // add the parent folder item
        _ = uGrid.Children.Add((CtlGridViewItem)CreateParentFolderItem(folderPath));

        // handle all folders first
        foreach (string dir in dirs)
        {
            DirectoryInfo di = new DirectoryInfo(dir);
            FSObjectInfo obj = new FSObjectInfo
            {
                IsFile = false,
                Exists = di.Exists,
                Info = di,
                IsSpecial = false
            };

            // creates the folder item
            CtlGridViewItem item = new CtlGridViewItem(ref obj)
            {
                Uid = di.FullName
            };

            item.MouseDoubleClick += async (s, e) =>
            {
                _ = await OpenFolder(item.Uid);
            };

            if (CmFolderMenu(folderPath, out ContextMenu cm) == true)
            {
                item.ContextMenu = cm;
            }

            _ = uGrid.Children.Add(item);
        }

        // handle all files second
        foreach (string file in files)
        {
            FileInfo fi = new FileInfo(file);
            FSObjectInfo obj = new FSObjectInfo
            {
                IsFile = true,
                Exists = fi.Exists,
                Info = fi,
                IsSpecial = false
            };

            // creates the file item
            CtlGridViewItem item = new CtlGridViewItem(ref obj)
            {
                Uid = fi.FullName
            };

            item.MouseDoubleClick += (s, e) =>
            {
                try
                {
                    Process proc = new Process
                    {
                        StartInfo =
                            {
                                FileName = fi.Name,
                                WorkingDirectory = folderPath,
                                UseShellExecute = true,
                                CreateNoWindow = true,
                                WindowStyle = ProcessWindowStyle.Hidden
                            }
                    };

                    _ = proc.Start();
                }

                catch (Exception ex)
                {
                    Log.Error(ex);
                    FZCore.Core.ErrorBox(ex.Message, "Start Process");
                }
            };

            if (CmFileMenu(folderPath, out ContextMenu cm) == true)
            {
                item.ContextMenu = cm;
            }
            _ = uGrid.Children.Add(item);
        }

        return true;
    }

    /// <summary>
    /// Creates a new <see cref="CtlGridView"/> instance.
    /// </summary>
    public CtlGridView()
    {
        InitializeComponent();
        _folderPath = string.Empty;
        _ = OpenFolder(_folderPath);
    }

    /// <summary>
    /// Gets or sets the default size of the items.
    /// Default is 64.
    /// </summary>
    /// <remarks>All items are squares (the width and height is the same).</remarks>
    public int ItemSize { get; set; } = 64;

    /// <summary>
    /// Gets or sets the value of total columns in the display grid.
    /// Default is 8.
    /// </summary>
    public int Columns
    {
        get => uGrid.Columns;
        set => uGrid.Columns = value;
    }

    private async void miRefresh_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        _ = await OpenFolder(_folderPath);
    }

    private void miExplorer_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        _ = Process.Start("explorer", _folderPath);
    }
}
