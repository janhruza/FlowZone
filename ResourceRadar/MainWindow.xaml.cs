using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.TextFormatting;
using FZCore;
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
        NavUncheckAll(btnDashboard);
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

        frmContent.Content = page;
        this.Title = $"{page.Title} - {App.Title}";
        NavUncheckAll(toggle);
        return true;
    }

    private void btnDashboard_Click(object sender, RoutedEventArgs e)
    {
        NavUncheckAll(btnDashboard);
    }

    private void btnInventory_Click(object sender, RoutedEventArgs e)
    {
        NavUncheckAll(btnInventory);
    }

    private void btnAnalytics_Click(object sender, RoutedEventArgs e)
    {
        NavUncheckAll(btnAnalytics);
    }

    private void btnSettings_Click(object sender, RoutedEventArgs e)
    {
        NavSetPage(PgSettings.Instance, btnSettings);
    }
}