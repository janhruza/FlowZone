using FZCore;

using Microsoft.Win32;

using System.IO;
using System.Windows;
using System.Windows.Controls;

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
        return File.Exists(this.txtPath.Text);
    }

    private void ComputeHash(HashAlgorithm hash)
    {
        if (CanComputeHash() == false)
        {
            Log.Error("No file selected.", nameof(ComputeHash));
            this.txtResult.Text = "NO_FILE_OPENED";
            return;
        }

        byte[] data = File.ReadAllBytes(this.txtPath.Text);

        switch (hash)
        {
            default:
                this.txtResult.Text = "INVALID_HASH_TYPE";
                break;

            case HashAlgorithm.SHA1:
                this.txtResult.Text = App.SHA1Hash(data);
                break;

            case HashAlgorithm.SHA256:
                this.txtResult.Text = App.SHA256Hash(data);
                break;

            case HashAlgorithm.SHA384:
                this.txtResult.Text = App.SHA384Hash(data);
                break;

            case HashAlgorithm.SHA512:
                this.txtResult.Text = App.SHA512Hash(data);
                break;

            case HashAlgorithm.MD5:
                this.txtResult.Text = App.MD5Hash(data);
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
            this.txtPath.Text = ofd.FileName;
        }
    }

    private void btnCopy_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(this.txtResult.Text) == false)
        {
            Clipboard.SetText(this.txtResult.Text);
        }
    }
}
