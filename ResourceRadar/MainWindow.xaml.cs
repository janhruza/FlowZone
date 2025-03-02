using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using FZCore;
using ResourceRadar.Core.Authentication;
using ResourceRadar.Pages;

namespace ResourceRadar;

/// <summary>
/// Representing the main window class.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// Creates a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        _instance = this;
        InitializeComponent();

        // Initialize navigation buttons
        navButons = [btnDashboard, btnInventory, btnAnalytics, btnSettings];

        // Initialize window properties
        this.Loaded += (s, e) => InitWindow();
    }

    private ToggleButton[] navButons;

    private void InitWindow()
    {
        this.MinWidth = this.ActualWidth;
        this.MinHeight = this.ActualHeight;

        NavSetPage(PgProfileSelector.Instance, null);
    }

    private void NavUncheckAll(ToggleButton? exception)
    {
        foreach (ToggleButton navButton in navButons)
        {
            if (navButton == exception)
            {
                navButton.IsChecked = true;
            }

            else
            {
                navButton.IsChecked = false;
            }
        }
    }

    private bool NavSetPage(Page? page, ToggleButton? toggle)
    {
        if (page == null)
        {
            Log.Error("Navigation failed because the target page is null.", nameof(NavSetPage));
            return false;
        }

        if (page.GetType() == typeof(PgProfileSelector))
        {
            gNavBar.Visibility = Visibility.Collapsed;
            FZCore.Core.SetApplicationTheme(App.Current, FZThemeMode.System);
        }

        else
        {
            gNavBar.Visibility = Visibility.Visible;
        }

        frmContent.Content = page;
        this.Title = $"{page.Title} - {App.Title}";
        NavUncheckAll(toggle);
        return true;
    }

    private void btnDashboard_Click(object sender, RoutedEventArgs e)
    {
        NavSetPage(PgDashboard.Instance, btnDashboard);
    }

    private void btnInventory_Click(object sender, RoutedEventArgs e)
    {
        NavSetPage(PgInventory.Instance, btnInventory);
    }

    private void btnAnalytics_Click(object sender, RoutedEventArgs e)
    {
        NavSetPage(PgAnalytics.Instance, btnAnalytics);
    }

    private void btnSettings_Click(object sender, RoutedEventArgs e)
    {
        NavSetPage(PgSettings.Instance, btnSettings);
    }

    private void btnLogout_Click(object sender, RoutedEventArgs e)
    {
        UserProfile.Logout();
        NavSetPage(PgProfileSelector.Instance, null);
    }

    #region Static code

    private static MainWindow? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public static MainWindow Instance => _instance ??= new MainWindow();

    /// <summary>
    /// Navigates the <see cref="Instance"/> window to the given <paramref name="page"/> and sets the active <paramref name="assocButton"/> as checked (if any).
    /// </summary>
    /// <param name="page">New page content.</param>
    /// <param name="assocButton">Associated toggle button (optional).</param>
    /// <returns>True, if navigation succeeded, otherwise false.</returns>
    public static bool SetActivePage(Page? page, ToggleButton? assocButton)
    {
        if (_instance == null)
        {
            Log.Error("Window is null.", nameof(SetActivePage));
            return false;
        }

        return _instance.NavSetPage(page, assocButton);
    }

    #endregion
}