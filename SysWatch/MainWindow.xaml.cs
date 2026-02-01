using FZCore.Windows;

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SysWatch;

/// <summary>
/// Representing the main window.
/// </summary>
public partial class MainWindow : FreeWindow
{
    const string DefaultTitle = "System Watch";

    /// <summary>
    /// Creates a new <see cref="MainWindow"/> instance.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        ctlButtons.Target = this;
    }

    #region Public methods

    /// <summary>
    /// Attempts to activate the specified page by navigating to it.
    /// </summary>
    /// <param name="page">The page to activate. If <paramref name="page"/> is <see langword="null"/>, the method does not perform
    /// navigation and returns <see langword="false"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the page was
    /// successfully activated; otherwise, <see langword="false"/>.</returns>
    public async Task<bool> ActivatePage(Page? page)
    {
        if (frm is null) return false;
        if (page is null) return false;
        return frm.Navigate(page);
    }

    #endregion

    private void ShowHelp()
    {
        _ = FZCore.Core.AboutBox();
        return;
    }

    private void ShowLog()
    {
        _ = FZCore.Core.ViewLog(this);
        return;
    }

    private void ExtendSystemMenu()
    {
        WindowExtender wex = new WindowExtender(this);
        wex.AddSeparator(0x1000);
        wex.AddMenuItem(0x1001, new ExtendedMenuItem
        {
            Header = "About FlowZone\tF1",
            OnClick = () => ShowHelp()
        });

        wex.AddMenuItem(0x1002, new ExtendedMenuItem
        {
            Header = "View Log\tF2",
            OnClick = () => ShowLog()
        });
    }

    private void IconlessWindow_Loaded(object sender, RoutedEventArgs e)
    {
        ExtendSystemMenu();
        ctlButtons.Foreground = FZCore.Core.IsDarkModeEnabled() ? Brushes.White : Brushes.Black;

        // activate the default item
        tviDashboard.IsSelected = true;
    }

    private void IconlessWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.F1)
        {
            ShowLog();
        }

        else if (e.Key == Key.F2)
        {
            ShowLog();
        }
    }

    private void frm_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
    {
        // update the window title
        if (e.Content is Page pg)
        {
            this.Title = $"{pg.Title} - {DefaultTitle}";
        }
    }

    private void wnd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        this.DragMove();
    }

    private async void tviDashboard_Selected(object sender, RoutedEventArgs e)
    {
        await ActivatePage(App.PgDashboard);
    }

    private void tviCPU_Selected(object sender, RoutedEventArgs e)
    {
        return;
    }

    private void tviRAM_Selected(object sender, RoutedEventArgs e)
    {
        return;
    }

    private void tviTasks_Selected(object sender, RoutedEventArgs e)
    {
        return;
    }
}