using System.Windows;
using System.Windows.Controls.Primitives;
using System.Collections.Generic;

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

        this.Loaded += (s, e) =>
        {
            // construct the nav buttons list
            _navButtons = [btnHome, btnOverview, btnSettings];

            // navigate to home page
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

    private void btnHome_Click(object sender, RoutedEventArgs e)
    {
        NavUncheckAl(ref btnHome);
    }

    private void btnOverview_Click(object sender, RoutedEventArgs e)
    {
        NavUncheckAl(ref btnOverview);
    }

    private void btnSettings_Click(object sender, RoutedEventArgs e)
    {
        NavUncheckAl(ref btnSettings);
    }
}