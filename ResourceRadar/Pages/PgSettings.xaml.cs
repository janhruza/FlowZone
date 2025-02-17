using System.Windows;
using System.Windows.Controls;
using FZCore;
using ResourceRadar.Core.Authentication;

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

    private void SetTheme(FZThemeMode theme)
    {
        if (UserProfile.Current == null)
        {
            Log.Warning($"No user profile loaded.", nameof(SetTheme));
        }

        else
        {
            UserProfile.Current.Settings.ThemeMode = theme;
        }

        FZCore.Core.SetApplicationTheme(App.Current, theme);
        return;
    }

    private void rbThemeSystem_Checked(object sender, RoutedEventArgs e)
    {
        SetTheme(FZThemeMode.System);
    }

    private void rbThemeDark_Checked(object sender, RoutedEventArgs e)
    {
        SetTheme(FZThemeMode.Dark);
    }

    private void rbThemeLight_Checked(object sender, RoutedEventArgs e)
    {
        SetTheme(FZThemeMode.Light);
    }

    private void rbThemeNone_Click(object sender, RoutedEventArgs e)
    {
        SetTheme(FZThemeMode.None);
    }
}
