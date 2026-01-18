using FZCore;

using PathFinder.Data;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
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
            Uid = parent,
            FontSize = SystemFonts.StatusFontSize
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

        List<string> paths = [];
        if (FsGetAllEntries(folderPath, SortFoldersFirst, out paths) == false)
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

        if (paths.Count <= 1) // including the separator
        {
            // folder is empty
            return true;
        }

        if (hasParent)
        {
            listBox.Items.Add(new Separator());
        }

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
                        Content = new CtlItemDetailView(ref obj)
                        {
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
                        },
                        Tag = obj,
                        HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch
                    };

                    lbi.MouseDoubleClick += (s, e) =>
                    {
                        // open file
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
                    };
                }

                else
                {
                    // Check if it's special (separator) AND not at the boundaries
                    if (obj.IsSpecial == true)
                    {
                        // object is a separator and not first or last in the list
                        // (first item is always after a separator from the parent folder item)
                        listBox.Items.Add(new Separator());
                        continue;
                    }

                    DirectoryInfo di = (DirectoryInfo)obj.Info;
                    lbi = new ListBoxItem
                    {
                        Content = new CtlItemDetailView(ref obj)
                        {
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
                        },
                        Tag = obj,
                        HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch
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

        return true;
    }

    private string _folderPath = string.Empty;

    /// <inheritdoc/>
    public new event EventHandler<string> FolderChanged = delegate { };

    /// <inheritdoc/>
    public new string FolderName => _folderPath;

    private void miRefresh_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        OpenFolder(_folderPath);
    }
}
