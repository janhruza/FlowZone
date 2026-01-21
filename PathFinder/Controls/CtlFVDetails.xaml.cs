using FZCore;

using PathFinder.Data;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using static PathFinder.Core.ContextMenus;
using static PathFinder.Core.FileSystem;

namespace PathFinder.Controls;

/// <summary>
/// Representing the details folder view control.
/// </summary>
public partial class CtlFVDetails : CtlFolderViewBase
{
    /// <summary>
    /// Creates a new <see cref="CtlFVDetails"/> instance.
    /// </summary>
    public CtlFVDetails()
    {
        InitializeComponent();
        SortFoldersFirst = false;

        Loaded += async (s, e) =>
        {
            _folderPath = $"C:\\";
            await OpenFolder(_folderPath);
        };
    }

    /// <inheritdoc/>
    public new object CreateParentFolderItem(string folderPath)
    {
        string parent = Directory.GetParent(folderPath)?.FullName ?? Directory.GetDirectoryRoot(folderPath);

        if (string.IsNullOrWhiteSpace(parent))
        {
            parent = "C:\\";
        }

        ListBoxItem lbi = new ListBoxItem
        {
            Content = "[Parent directory]",
            Uid = parent,
            FontSize = SystemFonts.StatusFontSize
        };

        lbi.MouseDoubleClick += async (s, e) =>
        {
            await OpenFolder(parent);
        };

        return lbi;
    }

    /// <inheritdoc/>
    public new async Task<bool> OpenFolder(string folderPath)
    {
        if (Directory.Exists(folderPath) == false)
        {
            Log.Error($"Folder \'{folderPath}\' not found.", nameof(OpenFolder));
            return false;
        }

        listBox.Items.Clear();
        listBox.Items.Add(new ListBoxItem
        {
            Content = new CtlItemDetailView()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                FontSize = SystemFonts.StatusFontSize + 2
            },

            HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch,
            IsEnabled = false
        });

        List<string> dirs = [];
        List<string> files = [];

        if (FsFetchAllFolders(folderPath, true, out dirs) == false)
        {
            return false;
        }

        if (FsFetchAllFiles(folderPath, true, out files) == false)
        {
            return false;
        }

        // folder exists
        _folderPath = folderPath;
        FolderChanged.Invoke(this, folderPath);

        // get parent folder item
        bool hasParent = Directory.GetParent(folderPath) != null;
        if (hasParent)
        {
            ListBoxItem lbiParent = (ListBoxItem)CreateParentFolderItem(folderPath);
            listBox.Items.Add(lbiParent);
        }

        if (hasParent && dirs.Count > 0)
        {
            listBox.Items.Add(new Separator());
        }

        foreach (string dir in dirs)
        {
            if (FsGetItemInfo(dir, out FSObjectInfo obj) == false) continue;
            if (obj.IsFile == true) continue; // file check

            DirectoryInfo di = (DirectoryInfo)obj.Info;
            ListBoxItem lbi = new ListBoxItem
            {
                Content = new CtlItemDetailView(ref obj)
                {
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
                },
                Tag = obj,
                HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch,
                Uid = di.FullName
            };

            // folder context menu
            if (CmFolderMenu(di.FullName, out ContextMenu cm) == true)
            {
                lbi.ContextMenu = cm;
            }

            lbi.MouseDoubleClick += async (s, e) =>
            {
                if (await OpenFolder(di.FullName) == false)
                {
                    FZCore.Core.ErrorBox($"Unable to open the given folder: {di.FullName}");
                }
            };
            listBox.Items.Add(lbi);
        }

        if (files.Count == 0)
        {
            return true;
        }

        listBox.Items.Add(new Separator());

        foreach (string file in files)
        {
            if (FsGetItemInfo(file, out FSObjectInfo obj) == false) continue;
            if (obj.IsFile == false) continue; // directory check

            FileInfo fi = (FileInfo)obj.Info;
            ListBoxItem lbi = new ListBoxItem
            {
                Content = new CtlItemDetailView(ref obj)
                {
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
                },
                Tag = obj,
                HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch,
                Uid = fi.FullName
            };

            // file context menu
            if (CmFileMenu(fi.FullName, out ContextMenu cm) == true)
            {
                lbi.ContextMenu = cm;
            }

            lbi.MouseDoubleClick += (s, e) =>
            {
                // open file
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

            listBox.Items.Add(lbi);
        }

        return true;
    }

    private string _folderPath = string.Empty;

    /// <inheritdoc/>
    public new event EventHandler<string> FolderChanged = delegate { };

    /// <inheritdoc/>
    public new string FolderName => _folderPath;

    private async void miRefresh_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        await OpenFolder(_folderPath);
    }

    private void miExplorer_Click(object sender, RoutedEventArgs e)
    {
        if (listBox.SelectedIndex != -1 && listBox.SelectedItem is ListBoxItem li)
        {
            string path = li.Uid;
            _ = Process.Start("explorer", $"/select, {path}");
        }

        else
        {
            // open folder
            _ = Process.Start("explorer", _folderPath);
        }
    }
}
