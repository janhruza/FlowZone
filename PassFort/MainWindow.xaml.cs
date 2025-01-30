using System.Windows;
using System.Windows.Controls.Primitives;
using System.Collections.Generic;
using System.Windows.Controls;
using FZCore;
using PassFort.Pages;

namespace PassFort;

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
        _instance = this;

        this.Loaded += (s, e) =>
        {
            // construct the nav buttons list
            _navButtons = [btnHome, btnOverview, btnSettings];

            // navigate to home page
            NavSetPage(PgHome.Instance);
            NavUncheckAl(ref btnHome);
        };
    }

    private List<ToggleButton> _navButtons = [];

    private void NavUncheckAl(ref ToggleButton btnException)
    {
        foreach (ToggleButton button in _navButtons)
        {
            if (button == btnException)
            {
                button.IsChecked = true;
            }

            else
            {
                button.IsChecked = false;
            }
        }
    }

    private bool NavSetPage(Page? page)
    {
        if (page == null)
        {
            Log.Error("Invalid content. Page was null.", nameof(NavSetPage));
            return false;
        }

        // set page content
        frmContent.Content = page;
        this.Title = $"{page.Title} - {App.Title}";
        return true;
    }

    private void btnHome_Click(object sender, RoutedEventArgs e)
    {
        NavSetPage(PgHome.Instance);
        NavUncheckAl(ref btnHome);
    }

    private void btnOverview_Click(object sender, RoutedEventArgs e)
    {
        NavSetPage(PgOverview.Instance);
        NavUncheckAl(ref btnOverview);
    }

    private void btnSettings_Click(object sender, RoutedEventArgs e)
    {
        NavUncheckAl(ref btnSettings);
    }

    private static MainWindow? _instance;

    /// <summary>
    /// Attempts to set home page as the content page.
    /// </summary>
    public static void SetHomePage()
    {
        if (_instance == null) return;

        _instance.NavSetPage(PgHome.Instance);
        _instance.NavUncheckAl(ref _instance.btnHome);
        return;
    }

    /// <summary>
    /// Representing the current instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public static MainWindow Instance => _instance ??= new MainWindow();

    /// <summary>
    /// Attempts to set a content <paramref name="page"/> as the current main window content.
    /// </summary>
    /// <param name="page">Target page with content.</param>
    /// <param name="assocButton">Associated toggle button.</param>
    /// <returns></returns>
    public static bool SetContentPage(Page? page, ref ToggleButton? assocButton)
    {
        if (page == null)
        {
            return false;
        }

        if (_instance == null)
        {
            return false;
        }

        _instance.NavSetPage(page);

        if (assocButton != null)
        {
            _instance.NavUncheckAl(ref assocButton);
        }

        return true;
    }
}