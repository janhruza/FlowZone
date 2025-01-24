using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Expando.Pages;

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
            if (tbException == tButton) continue;

            tButton.IsChecked = false;
        }

        return;
    }

    private void NavSetPage(ref Page page)
    {
        if (page == null) return;
        if (frmContent == null) return;

        frmContent.Content = page;
        this.Title = $"{page.Title} - {Messages.AppTitle}";

        return;
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
}