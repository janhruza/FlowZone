using FZCore.Windows;

using System.Windows;

using WebPeek.Core;

namespace WebPeek.Windows;

/// <summary>
/// Representing the window for adding a new application.
/// </summary>
public partial class WndNewApp : IconlessWindow
{
    /// <summary>
    /// Creates a new instance of the <see cref="WndNewApp"/> class.
    /// </summary>
    public WndNewApp()
    {
        InitializeComponent();
        webApp = new WebApplication();
        Result = webApp;
    }

    private bool bDialog;

    private WebApplication webApp;

    /// <summary>
    /// Representing the result of the application registration.
    /// </summary>
    public WebApplication Result { get; private set; }

    /// <summary>
    /// Ovverrides the Show method to set the dialog state.
    /// </summary>
    public new void Show()
    {
        bDialog = false;
        return;
    }

    /// <summary>
    /// Ovverrides the ShowDialog method to set the dialog state and return a nullable boolean.
    /// </summary>
    /// <returns>The window's dialog result.</returns>
    public new bool? ShowDialog()
    {
        bDialog = true;
        return base.ShowDialog();
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        if (bDialog)
        {
            DialogResult = false;
        }

        Close();
    }

    private bool ExportApp()
    {
        webApp = new WebApplication(txtName.Text, txtUrl.Text);
        return AppManager.RegisterApp(webApp);
    }

    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
        if (ExportApp() == false)
        {
            _ = MessageBox.Show("Failed to register the application. Please check the name and URL.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        Result = webApp;

        if (bDialog)
        {
            DialogResult = true;
        }

        Close();
    }
}
