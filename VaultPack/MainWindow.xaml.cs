using System.Windows;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Windows.Controls;
using System.IO;
using System.Linq;
using System.Xml;

namespace VaultPack;

/// <summary>
/// Representing the main application window.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// Creates a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }

    #region Create new archive code

    private List<string> _filesToAdd = [];

    private void btnCreate_Click(object sender, RoutedEventArgs e)
    {
        // conditions check
        string path = txtCreatePath.Text.Trim();

        if (string.IsNullOrEmpty(path) == true)
        {
            return;
        }

        if (_filesToAdd.Count == 0)
        {
            return;
        }

        // write data to file as stream
        using (FileStream fs = File.Create(path))
        {
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                // write data
                bw.Write(_filesToAdd.Count);

                foreach (string file in _filesToAdd)
                {
                    FileInfo fi = new FileInfo(file);
                    bw.Write(fi.Name);
                    bw.Write(fi.Length);
                    bw.Write(File.ReadAllBytes(file));
                }
            }
        }

        return;
    }

    private void AddCreateItem(string path)
    {
        path = path.Trim();
        if (_filesToAdd.Contains(path) == true)
        {
            return;
        }

        _filesToAdd.Add(path);

        // create to UI
        ListBoxItem lbi = new ListBoxItem
        {
            Uid = path,
            Content = Path.GetFileName(path)
        };

        lbFilesToAdd.Items.Add(lbi);

        return;
    }

    private void RemoveCreateSelected()
    {
        if (lbFilesToAdd.SelectedIndex >= 0)
        {
            List<ListBoxItem> selectedItems = lbFilesToAdd.SelectedItems.Cast<ListBoxItem>().ToList();

            foreach (ListBoxItem selected in selectedItems)
            {
                lbFilesToAdd.Items.Remove(selected);
            }
        }

        return;
    }

    private void btnCreateChoose_Click(object sender, RoutedEventArgs e)
    {
        SaveFileDialog sfd = new SaveFileDialog
        {
            Filter = "Flow Zone Archive|*.fza|Other|*.*"
        };

        if (sfd.ShowDialog() == true)
        {
            txtCreatePath.Text = sfd.FileName;
        }
    }

    private void btnCreateRemove_Click(object sender, RoutedEventArgs e)
    {
        RemoveCreateSelected();
    }

    private void btnCreateAddFile_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog
        {
            Multiselect = true,
            Filter = "All Files|*.*"
        };

        if (ofd.ShowDialog() == true)
        {
            foreach (string file in ofd.FileNames)
            {
                AddCreateItem(file);
            }
        }
    }

    private void ClearCreatePage()
    {
        _filesToAdd.Clear();
        lbFilesToAdd.Items.Clear();
        txtCreatePath.Clear();
    }

    private void btnClearCreate_Click(object sender, RoutedEventArgs e)
    {
        ClearCreatePage();
    }

    #endregion

    #region Extract existing archive code
    string e_ArchivePath = string.Empty;
    string e_FolderPath = string.Empty;

    private void btnExtractClear_Click(object sender, RoutedEventArgs e)
    {
        txtArchivePath.Clear();
        txtExtractFolder.Clear();
    }

    private bool ValidateExtractionData()
    {
        e_ArchivePath = txtArchivePath.Text.Trim();
        e_FolderPath = txtExtractFolder.Text.Trim();

        if (File.Exists(e_ArchivePath) == false) return false;
        if (Directory.Exists(e_FolderPath) == false) return false;

        return true;
    }

    private void btnExtract_Click(object sender, RoutedEventArgs e)
    {
        // Data check
        if (ValidateExtractionData() == false)
        {
            return;
        }

        // extract archive
        using (FileStream fs = File.OpenRead(e_ArchivePath))
        {
            using (BinaryReader br = new BinaryReader(fs))
            {
                // get number of entries
                int count = br.ReadInt32();

                for (int x = 0; x <count; x++)
                {
                    string name = br.ReadString();
                    long size = br.ReadInt64();

                    byte[] buffer = new byte[size];

                    for (int i = 0; i < size; i++)
                    {
                        buffer[i] = br.ReadByte();
                    }

                    string path = Path.Combine(e_FolderPath, name);
                    File.WriteAllBytes(path, buffer);
                }
            }
        }
    }

    private void btnChooseArchive_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog
        {
            Filter = "Flow Zone Archive|*.fza|Other|*.*"
        };

        if (ofd.ShowDialog() == true)
        {
            txtArchivePath.Text = ofd.FileName;
        }
    }

    private void btnChooseExtractFolder_Click(object sender, RoutedEventArgs e)
    {
        OpenFolderDialog ofd = new OpenFolderDialog
        {
            Title = "Select extraction folder"
        };

        if (ofd.ShowDialog() == true)
        {
            txtExtractFolder.Text = ofd.FolderName;
        }
    }

    #endregion
}