using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FZCore;

namespace ResourceRadar.Pages;

/// <summary>
/// Representing the settings page.
/// </summary>
public partial class PgSettings : Page
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgSettings"/> class.
    /// </summary>
    public PgSettings()
    {
        _instance = this;
        InitializeComponent();
    }

    private static PgSettings? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="PgSettings"/> class.
    /// </summary>
    public static PgSettings Instance => _instance ??= new PgSettings();

    private void rbThemeSystem_Checked(object sender, RoutedEventArgs e)
    {
        FZCore.Core.SetApplicationTheme(App.Current, FZThemeMode.System);
    }

    private void rbThemeDark_Checked(object sender, RoutedEventArgs e)
    {
        FZCore.Core.SetApplicationTheme(App.Current, FZThemeMode.Dark);
    }

    private void rbThemeLight_Checked(object sender, RoutedEventArgs e)
    {
        FZCore.Core.SetApplicationTheme(App.Current, FZThemeMode.Light);
    }

    private void rbThemeNone_Click(object sender, RoutedEventArgs e)
    {
        FZCore.Core.SetApplicationTheme(App.Current, FZThemeMode.None);
    }
}
