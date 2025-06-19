using System.Windows;
using System.Windows.Controls;
using FZCore.Windows;
using WebPeek.Pages;

namespace WebPeek;

/// <summary>
/// Representing the main window class.
/// </summary>
public partial class MainWindow : Window
{
    private PgHome? _homePage;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();

        // expand the system menu
        WindowExtender we = new WindowExtender(this);
        we.AddMenuItem(0x1001, new ExtendedMenuItem
        {
            Header = "Home Page",
            OnClick = () =>
            {
                GoToHomePage();
            }
        });

        this.Loaded += (s, e) =>
        {
            this.frmContent.Content = _homePage;
        };

        _homePage = new PgHome();
        _instance = this;
    }

    private static MainWindow? _instance;

    private static bool GoToHomePage()
    {
        if (_instance == null) return false;
        if (_instance._homePage == null) return false;

        // check if current page is already home page
        if (_instance.frmContent.Content == _instance._homePage) return true;

        // dispose current web view if it exists
        if (_instance.frmContent.Content is PgWebView pgWebView)
        {
            pgWebView.WebView?.Dispose();
        }

        // set the home page as active
        SetActivePage(_instance._homePage, 640, 450);
        return true;
    }

    private static void CenterWindow()
    {
        if (_instance == null) return;

        // Center the window on the screen
        _instance.Left = (SystemParameters.PrimaryScreenWidth - _instance.Width) / 2;
        _instance.Top = (SystemParameters.PrimaryScreenHeight - _instance.Height) / 2;
        return;
    }

    internal static bool SetActivePage(Page? pg, double dWidth = 1280, double dHeight = 720)
    {
        if (pg == null) return false;
        if (_instance == null) return false;

        // set window size
        _instance.Width = dWidth;
        _instance.Height = dHeight;

        CenterWindow();

        _instance.frmContent.Content = pg;
        return true;
    }
}