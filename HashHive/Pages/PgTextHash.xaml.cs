using System.Windows;
using System.Windows.Controls;

namespace HashHive.Pages;

/// <summary>
/// Representing the hash text page.
/// This class derives directly from the <see cref="Page"/> class.
/// </summary>
public partial class PgTextHash : Page
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgTextHash"/> class.
    /// </summary>
    public PgTextHash()
    {
        // loads the page
        InitializeComponent();
    }

    private static PgTextHash? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="PgTextHash"/> class.
    /// </summary>
    public static PgTextHash Instance => _instance ??= new PgTextHash();

    private void btnSha256_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        this.txtResult.Text = App.SHA256Hash(this.txtInput.Text);
    }

    private void btnSha512_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        this.txtResult.Text = App.SHA512Hash(this.txtInput.Text);
    }

    private void btnMD5_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        this.txtResult.Text = App.MD5Hash(this.txtInput.Text);
    }

    private void btnSha1_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        this.txtResult.Text = App.SHA1Hash(this.txtInput.Text);
    }

    private void btnSha384_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        this.txtResult.Text = App.SHA384Hash(this.txtInput.Text);
    }

    private void btnCopyHash_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(this.txtResult.Text) == false)
        {
            Clipboard.SetText(this.txtResult.Text);
        }
    }
}
