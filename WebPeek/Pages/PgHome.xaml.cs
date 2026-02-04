using FZCore.Windows;

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using WebPeek.Core;
using WebPeek.Windows;

namespace WebPeek.Pages;

/// <summary>
/// Representing the home page class.
/// </summary>
public partial class PgHome : Page
{
    /// <summary>
    /// Constructs a new instance of the <see cref="PgHome"/> class.
    /// </summary>
    public PgHome()
    {
        InitializeComponent();
        Loaded += (s, e) => RefreshAppsList();
    }

    private void OpenWebApp(WebApplication? app)
    {
        // Logic to open the web application, e.g., in a new window or tab
        if (app == null) return;

        PgWebView pgWebView = new PgWebView(app.Link)
        {
            Title = app.Name
        };

        // Assuming MainWindow is the main application window that can host pages
        _ = MainWindow.SetActivePage(pgWebView);
        return;
    }

    private bool RemoveApp(WebApplication? webApp)
    {
        // check if the web application is null
        if (webApp == null) return false;

        // show confirmation dialog
        if (DialogWindow.ShowDialog($"Are you sure you want to remove this web application? This action is irreversable.", $"Remove \'{webApp.Name}\'", caption: $"Remove \'{webApp.Name}\'", DWImage.WARNING, DWButton.YES | DWButton.NO) != TDReturn.IDYES) return false;

        if (AppManager.UnregisterApp(webApp) == true)
        {
            RefreshAppsList();
            return true;
        }

        return false;
    }

    private void RefreshAppsList()
    {
        // Logic to refresh the list of applications
        this.lbxApps.Items.Clear();

        var apps = AppManager.GetApps().OrderBy(x => x.Name);

        if (apps.Count() == 0)
        {
            // No applications registered, show a message
            ListBoxItem lbi = new ListBoxItem
            {
                Content = "No web applications registered. Click the 'Add Application' button to add a new application."
            };

            _ = this.lbxApps.Items.Add(lbi);
            return;
        }

        foreach (var app in apps)
        {
            // TODO: add page item to the list
            Border bd = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Transparent
            };

            bd.KeyDown += (s, e) =>
            {
                if (bd.IsFocused == true && e.Key == System.Windows.Input.Key.Enter)
                {
                    // click event for the button
                    OpenWebApp(app);
                }
            };

            bd.MouseLeftButtonDown += (s, e) =>
            {
                if (e.ClickCount == 2)
                {
                    // open on mouse double click as well
                    OpenWebApp(app);
                }
            };

            Grid g = new Grid();
            g.ColumnDefinitions.Add(new ColumnDefinition());
            g.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            g.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            // label
            Label lbl = new Label
            {
                Content = app.Name,
                Margin = new Thickness(0, 0, 5, 0),
                VerticalAlignment = VerticalAlignment.Center
            };

            // delete button
            Button btnDelete = new Button
            {
                Content = $"",
                FontFamily = new System.Windows.Media.FontFamily(App.FONT_DISPLAY_NAME),
                Margin = new Thickness(5, 0, 5, 0)
            };

            btnDelete.Click += (s, e) =>
            {
                // Logic to delete the application
                _ = RemoveApp(app);
            };

            // run button
            Button btnStart = new Button
            {
                Content = $"",
                FontFamily = new System.Windows.Media.FontFamily(App.FONT_DISPLAY_NAME)
            };

            btnStart.Click += (s, e) =>
            {
                OpenWebApp(app);
            };

            _ = g.Children.Add(lbl);
            _ = g.Children.Add(btnDelete);
            _ = g.Children.Add(btnStart);

            Grid.SetColumn(btnDelete, 1);
            Grid.SetColumn(btnStart, 2);

            bd.Child = g;

            ListBoxItem lbi = new ListBoxItem
            {
                Content = bd,
                HorizontalContentAlignment = HorizontalAlignment.Stretch
            };

            _ = this.lbxApps.Items.Add(lbi);
        }

        return;
    }

    private void AddNewApp()
    {
        WndNewApp wna = new WndNewApp();
        if (wna.ShowDialog() == true)
        {
            // Logic to handle the new app addition
            RefreshAppsList();
        }
    }

    private void btnRefresh_Click(object sender, RoutedEventArgs e)
    {
        RefreshAppsList();
    }

    private void btnAddApp_Click(object sender, RoutedEventArgs e)
    {
        AddNewApp();
    }

    private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.F5)
        {
            RefreshAppsList();
        }
    }
}
