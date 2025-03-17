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
        InitializeComponent();
    }

    private static PgTextHash? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="PgTextHash"/> class.
    /// </summary>
    public static PgTextHash Instance => _instance ??= new PgTextHash();

    private void btnSha256_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        txtResult.Text = App.SHA256Hash(txtInput.Text);
    }

    private void btnSha512_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        txtResult.Text = App.SHA512Hash(txtInput.Text);
    }

    private void btnMD5_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        txtResult.Text = App.MD5Hash(txtInput.Text);
    }
}
