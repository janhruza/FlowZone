using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Expando.Pages;
using System.Reflection.Metadata.Ecma335;

namespace Expando;

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
            // contructs the _navButtons list
            _navButtons = [btnHome, btnOverview, btnIncomes, btnExpanses, btnProfile];

            Page home = PgHome.Instance;
            NavSetPage(ref home);
            btnHome.IsChecked = true;
        };
    }

    private List<ToggleButton> _navButtons = [];

    private void NavUncheckAll(ref ToggleButton tbException)
    {
        foreach (var tButton in _navButtons)
        {
            if (tbException == tButton)
            {
                tButton.IsChecked = true;
                continue;
            }

            tButton.IsChecked = false;
        }

        return;
    }

    private bool NavSetPage(ref Page page)
    {
        if (page == null) return false;
        if (frmContent == null) return false;

        frmContent.Content = page;
        this.Title = $"{page.Title} - {Messages.AppTitle}";

        return true;
    }

    private void btnHome_Click(object sender, RoutedEventArgs e)
    {
        NavUncheckAll(ref btnHome);
        Page pg = PgHome.Instance;
        NavSetPage(ref pg);
    }

    private void btnOverview_Click(object sender, RoutedEventArgs e)
    {
        NavUncheckAll(ref btnOverview);
    }

    private void btnIncomes_Click(object sender, RoutedEventArgs e)
    {
        NavUncheckAll(ref btnIncomes);
    }

    private void btnExpanses_Click(object sender, RoutedEventArgs e)
    {
        NavUncheckAll(ref btnExpanses);
    }

    private void btnProfile_Click(object sender, RoutedEventArgs e)
    {
        NavUncheckAll(ref btnProfile);
        Page pg = PgProfiles.Instance;
        NavSetPage(ref pg);
    }

    #region Static code

    private static MainWindow? _instance;

    /// <summary>
    /// Sets the content of the window to the selected <paramref name="page"/>.
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public static bool SetActivePage(ref Page page)
    {
        if (_instance == null)
        {
            return false;
        }

        return _instance.NavSetPage(ref page);
    }

    /// <summary>
    /// Navigates to the home page.
    /// </summary>
    /// <returns></returns>
    public static bool SetHomePage()
    {
        if (_instance == null)
        {
            return false;
        }

        Page pgHome = PgHome.Instance;
        _instance.NavUncheckAll(ref _instance.btnHome);
        return _instance.NavSetPage(ref pgHome);
    }

    #endregion
}