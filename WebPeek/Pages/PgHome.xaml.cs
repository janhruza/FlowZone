using System.Windows;
using System.Windows.Controls;
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
        this.Loaded += (s, e) => RefreshAppsList();
    }

    private void RefreshAppsList()
    {
        // Logic to refresh the list of applications
        stpApps.Children.Clear();

        foreach (var app in AppManager.GetApps())
        {
            // TODO: add page item to the list
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
}
