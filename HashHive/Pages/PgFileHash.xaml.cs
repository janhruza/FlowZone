using System.IO;
using System.Windows;
using System.Windows.Controls;
using FZCore;
using Microsoft.Win32;

namespace HashHive.Pages;

/// <summary>
/// Representing the file hashing page.
/// This class derives directly from the <see cref="Page"/> class.
/// </summary>
public partial class PgFileHash : Page
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgFileHash"/> class.
    /// </summary>
    public PgFileHash()
    {
        // loads the page
        InitializeComponent();
    }

    private static PgFileHash? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="PgFileHash"/> object.
    /// </summary>
    public static PgFileHash Instance => _instance ??= new PgFileHash();

    private bool CanComputeHash()
    {
        return File.Exists(txtPath.Text);
    }

    private void ComputeHash(HashAlgorithm hash)
    {
        if (CanComputeHash() == false)
        {
            Log.Error("No file selected.", nameof(ComputeHash));
            return;
        }

        byte[] data = File.ReadAllBytes(txtPath.Text);

        switch (hash)
        {
            default:
                txtResult.Text = "INVALID_HASH_TYPE";
                break;

            case HashAlgorithm.SHA1:
                txtResult.Text = App.SHA1Hash(data);
                break;

            case HashAlgorithm.SHA256:
                txtResult.Text = App.SHA256Hash(data);
                break;

            case HashAlgorithm.SHA384:
                txtResult.Text = App.SHA384Hash(data);
                break;

            case HashAlgorithm.SHA512:
                txtResult.Text= App.SHA512Hash(data);
                break;

            case HashAlgorithm.MD5:
                txtResult.Text = App.MD5Hash(data);
                break;
        }

        return;
    }

    private void btnSha1_Click(object sender, RoutedEventArgs e)
    {
        ComputeHash(HashAlgorithm.SHA1);
    }

    private void btnSha256_Click(object sender, RoutedEventArgs e)
    {
        ComputeHash(HashAlgorithm.SHA256);
    }

    private void btnSha384_Click(object sender, RoutedEventArgs e)
    {
        ComputeHash(HashAlgorithm.SHA384);
    }

    private void btnSha512_Click(object sender, RoutedEventArgs e)
    {
        ComputeHash(HashAlgorithm.SHA512);
    }

    private void btnMd5_Click(object sender, RoutedEventArgs e)
    {
        ComputeHash(HashAlgorithm.MD5);
    }

    private void btnChooseFile_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog
        {
            Filter = "Any File|*.*"
        };

        if (ofd.ShowDialog() == true)
        {
            txtPath.Text = ofd.FileName;
        }
    }
}
