using Expando.Core;
using Expando.Pages;

using FZCore;
using FZCore.Windows;

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Expando;

/// <summary>
/// Representing the main window class.
/// </summary>
public partial class MainWindow : IconlessWindow
{
    /// <summary>
    /// Creates a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();

        _instance = this;

        Loaded += (s, e) =>
        {
            // contructs the _navButtons list
            this._navButtons = [this.btnHome, this.btnOverview, this.btnIncomes, this.btnExpanses, this.btnProfile];

            ToggleButton? tb = null;
            _ = ChangePage(PgProfiles.Instance, ref tb);
        };
    }

    private List<ToggleButton> _navButtons = [];

    private void NavUncheckAll(ref ToggleButton? tbException)
    {
        foreach (var tButton in this._navButtons)
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
        if (page == null)
        {
            Log.Error("Desired page is null.", nameof(NavSetPage));
            return false;
        }

        if (this.frmContent == null)
        {
            Log.Error("Frame (page viever) is null.", nameof(NavSetPage));
            return false;
        }

        this.frmContent.Content = page;
        Title = $"{page.Title}{(UserProfile.IsProfileLoaded() ? $" ({UserProfile.Current?.Username})" : string.Empty)} - {Messages.AppTitle}";
        Log.Info($"Navigation changed to: {page.Title}", nameof(NavSetPage));
        return true;
    }

    private bool ChangePage(Page page, ref ToggleButton? assocButton)
    {
        NavUncheckAll(ref assocButton);
        return NavSetPage(ref page);
    }

    private void btnHome_Click(object sender, RoutedEventArgs e)
    {
        _ = ChangePage(PgHome.Instance, ref this.btnHome);
    }

    private void btnOverview_Click(object sender, RoutedEventArgs e)
    {
        _ = ChangePage(PgOverview.Instance, ref this.btnOverview);
    }

    private void btnIncomes_Click(object sender, RoutedEventArgs e)
    {
        _ = ChangePage(PgIncomes.Instance, ref this.btnIncomes);
    }

    private void btnExpanses_Click(object sender, RoutedEventArgs e)
    {
        _ = ChangePage(PgExpanses.Instance, ref this.btnExpanses);
    }

    private void btnProfile_Click(object sender, RoutedEventArgs e)
    {
        _ = ChangePage(PgMyProfile.Instance, ref this.btnProfile);
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
            Log.Error($"Static window instance is null.", nameof(SetActivePage));
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
            Log.Error("Static window instance is null.", nameof(SetHomePage));
            return false;
        }

        Page pgHome = PgHome.Instance;
        _instance.NavUncheckAll(ref _instance.btnHome);
        return _instance.NavSetPage(ref pgHome);
    }

    #endregion
}