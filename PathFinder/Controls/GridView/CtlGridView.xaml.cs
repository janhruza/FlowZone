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
    public override object CreateParentFolderItem(string folderPath)
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

    /// <inheritdoc />
    public override event EventHandler FolderOpened = delegate { };

    /// <inheritdoc />
    public override event EventHandler<string> FolderChanged = delegate { };

    /// <inheritdoc/>
    public new string FolderName => this._folderPath;

    /// <inheritdoc/>
    public override async Task<bool> OpenFolder(string folderPath)
    {
        if (Directory.Exists(folderPath) == false)
        {
            Log.Error($"Folder \'{folderPath}\' not found.", nameof(OpenFolder));
            return false;
        }

        // clear grid
        this.uGrid.Children.Clear();

        // enum objects
        List<string> dirs = [];
        List<string> files = [];

        if (FsFetchAllFolders(folderPath, true, out dirs) == false)
        {
            Log.Error($"Unable to enumerate directories inside \'{folderPath}\'.", nameof(OpenFolder));
            return false;
        }

        if (FsFetchAllFiles(folderPath, true, out files) == false)
        {
            Log.Error($"Unable to enumerate files inside \'{folderPath}\'.", nameof(OpenFolder));
            return false;
        }

        this._folderPath = folderPath;
        FolderChanged.Invoke(this, folderPath);

        // add the parent folder item
        _ = this.uGrid.Children.Add((CtlGridViewItem)CreateParentFolderItem(folderPath));

        // handle all folders first
        foreach (string dir in dirs)
        {
            if (FsGetItemInfo(dir, out FSObjectInfo obj) == false) continue;
            if (obj.IsFile == true) continue; // file check

            DirectoryInfo di = (DirectoryInfo)obj.Info;

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

            _ = this.uGrid.Children.Add(item);
        }

        // handle all files second
        foreach (string file in files)
        {
            if (FsGetItemInfo(file, out FSObjectInfo obj) == false) continue;
            if (obj.IsFile == false) continue; // file check

            FileInfo fi = (FileInfo)obj.Info;

            // creates the file item
            CtlGridViewItem item = new CtlGridViewItem(ref obj)
            {
                Uid = fi.Name
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
                                UseShellExecute = true
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
            _ = this.uGrid.Children.Add(item);
        }

        this.FolderOpened.Invoke(this, EventArgs.Empty);

        return true;
    }

    /// <summary>
    /// Creates a new <see cref="CtlGridView"/> instance.
    /// </summary>
    public CtlGridView()
    {
        InitializeComponent();
        this._folderPath = string.Empty;
        _ = OpenFolder(this._folderPath);
    }

    /// <summary>
    /// Gets or sets the default size of the items.
    /// Default is 64.
    /// </summary>
    /// <remarks>All items are squares (the width and height is the same).</remarks>
    public int ItemSize { get; set; } = 128;

    /// <summary>
    /// Gets or sets the value of total columns in the display grid.
    /// Default is 8.
    /// </summary>
    public int Columns
    {
        get => this.uGrid.Columns;
        set => this.uGrid.Columns = value;
    }

    private async void miRefresh_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        _ = await OpenFolder(this._folderPath);
    }

    private void miExplorer_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        _ = Process.Start("explorer", this._folderPath);
    }
}
