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
        this.webApp = new WebApplication();
        Result = this.webApp;
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
        this.bDialog = false;
        return;
    }

    /// <summary>
    /// Ovverrides the ShowDialog method to set the dialog state and return a nullable boolean.
    /// </summary>
    /// <returns>The window's dialog result.</returns>
    public new bool? ShowDialog()
    {
        this.bDialog = true;
        return base.ShowDialog();
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        if (this.bDialog)
        {
            DialogResult = false;
        }

        Close();
    }

    private bool ExportApp()
    {
        this.webApp = new WebApplication(this.txtName.Text, this.txtUrl.Text);
        return AppManager.RegisterApp(this.webApp);
    }

    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
        if (ExportApp() == false)
        {
            FZCore.Core.ErrorBox("Failed to register the application. Please check the name and URL.", "Error");
            return;
        }

        Result = this.webApp;

        if (this.bDialog)
        {
            DialogResult = true;
        }

        Close();
    }
}
