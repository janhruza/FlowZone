using FZCore;

using PathFinder.Data;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Controls;

using static PathFinder.Core;

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

        this.Loaded += (s, e) =>
        {
            _folderPath = $"C:\\";
            OpenFolder(_folderPath);
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
            Uid = parent
        };

        lbi.MouseDoubleClick += (s, e) =>
        {
            OpenFolder(parent);
        };

        return lbi;
    }

    /// <inheritdoc/>
    public new bool OpenFolder(string folderPath)
    {
        if (Directory.Exists(folderPath) == false)
        {
            Log.Error($"Folder \'{folderPath}\' not found.", nameof(OpenFolder));
            return false;
        }

        listBox.Items.Clear();
        // get parent folder item
        ListBoxItem lbiParent = (ListBoxItem)CreateParentFolderItem(folderPath);
        listBox.Items.Add(lbiParent);

        List<string> paths = [];
        if (FsGetAllEntries(folderPath, SortFoldersFirst, out paths) == false)
        {
            return false;
        }

        if (paths.Count == 0)
        {
            // folder is empty
            return true;
        }

        listBox.Items.Add(new Separator());
        foreach (string path in paths)
        {
            Console.WriteLine(path);

            if (FsGetItemInfo(path, out FSObjectInfo obj) == true)
            {
                ListBoxItem lbi;
                if (obj.IsFile == true)
                {
                    FileInfo fi = (FileInfo)obj.Info;
                    lbi = new ListBoxItem
                    {
                        Content = fi.Name,
                        Uid = fi.FullName
                    };

                    lbi.MouseDoubleClick += (s, e) =>
                    {
                        // open file
                        Process proc = new Process
                        {
                            StartInfo =
                            {
                                FileName = "cmd.exe",
                                Arguments = $"/C {fi.FullName}",
                                WorkingDirectory = folderPath,
                                UseShellExecute = true,
                                CreateNoWindow = true,
                                WindowStyle = ProcessWindowStyle.Hidden
                            }
                        };

                        _ = proc.Start();
                    };
                }

                else
                {
                    DirectoryInfo di = (DirectoryInfo)obj.Info;
                    lbi = new ListBoxItem
                    {
                        Content = di.Name,
                        Uid = di.FullName
                    };

                    lbi.MouseDoubleClick += (s, e) =>
                    {
                        if (OpenFolder(di.FullName) == false)
                        {
                            FZCore.Core.ErrorBox($"Unable to open the given folder: {di.FullName}");
                        }
                    };
                }

                listBox.Items.Add(lbi);
            }
        }

        _folderPath = folderPath;
        return true;
    }

    private string _folderPath = string.Empty;

    /// <inheritdoc/>
    public new string FolderName => _folderPath;

    private void miRefresh_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        OpenFolder(_folderPath);
    }
}
