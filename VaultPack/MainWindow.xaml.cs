using System.Windows;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Windows.Controls;
using System.IO;
using System.Linq;

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
}