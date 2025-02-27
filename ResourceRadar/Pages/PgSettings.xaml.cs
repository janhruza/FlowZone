using System.Globalization;
using System.Threading.Tasks;
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
        InitializeComponent();

        this.Loaded += (s, e) =>
        {
            if (UserProfile.Current == null)
            {
                return;
            }

            // display cultures
            cbLocales.Items.Clear();

            foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                ComboBoxItem cbi = new ComboBoxItem
                {
                    Content = $"{culture.DisplayName} {(string.IsNullOrEmpty(culture.Name) == true ? string.Empty : $"({culture.Name.ToUpper()})")}"
                };

                cbi.Selected += async (s, e) =>
                {
                    // set the new culture
                    UserProfile.Current.Settings.CultureName = culture.Name;
                    CultureInfo cu = new CultureInfo(culture.Name);
                    CultureInfo.CurrentCulture = cu;
                    CultureInfo.CurrentUICulture = cu;
                    await UserProfile.SaveSettings(UserProfile.Current);
                };

                int index = cbLocales.Items.Add(cbi);
                if (culture.Name == UserProfile.Current.Settings.CultureName)
                {
                    cbLocales.SelectedIndex = index;
                }
            }

            // set checked theme item
            switch (UserProfile.Current.Settings.ThemeMode)
            {
                case FZThemeMode.None:
                    rbThemeNone.IsChecked = true;
                    break;

                case FZThemeMode.Light:
                    rbThemeLight.IsChecked = true;
                    break;

                case FZThemeMode.Dark:
                    rbThemeDark.IsChecked = true;
                    break;

                case FZThemeMode.System:
                    rbThemeSystem.IsChecked = true;
                    break;

                default:
                    return;
            }
        };
    }

    private static PgSettings? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="PgSettings"/> class.
    /// </summary>
    public static PgSettings Instance => _instance ??= new PgSettings();

    private async Task SetTheme(FZThemeMode theme)
    {
        if (UserProfile.Current == null)
        {
            Log.Warning($"No user profile loaded.", nameof(SetTheme));
        }

        else
        {
            UserProfile.Current.Settings.ThemeMode = theme;
            await UserProfile.SaveSettings(UserProfile.Current);
        }

        FZCore.Core.SetApplicationTheme(App.Current, theme);
        return;
    }

    private async void rbThemeSystem_Checked(object sender, RoutedEventArgs e)
    {
        await SetTheme(FZThemeMode.System);
    }

    private async void rbThemeDark_Checked(object sender, RoutedEventArgs e)
    {
        await SetTheme(FZThemeMode.Dark);
    }

    private async void rbThemeLight_Checked(object sender, RoutedEventArgs e)
    {
        await SetTheme(FZThemeMode.Light);
    }

    private async void rbThemeNone_Click(object sender, RoutedEventArgs e)
    {
        await SetTheme(FZThemeMode.None);
    }
}
