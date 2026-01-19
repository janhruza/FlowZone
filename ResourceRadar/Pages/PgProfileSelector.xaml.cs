using ResourceRadar.Core.Authentication;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ResourceRadar.Pages;

/// <summary>
/// Representing a profile selection page.
/// </summary>
public partial class PgProfileSelector : Page
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgProfileSelector"/> class.
    /// </summary>
    public PgProfileSelector()
    {
        _instance = this;
        InitializeComponent();

        Loaded += PgProfileSelector_Loaded;
    }

    private async void PgProfileSelector_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        await RefreshProfiles();
    }

    private ListBoxItem CreateProfileItem()
    {
        ListBoxItem lbi = new ListBoxItem
        {
            Content = "Create a new profile"
        };

        lbi.MouseDoubleClick += CreateUser_MouseDoubleClick;

        return lbi;
    }

    private async void CreateUser_MouseDoubleClick(object? sender, MouseButtonEventArgs e)
    {
        // open new user dialog
        if (App.CreateNewProfile() == true)
        {
            // open dashboard for the newly created user
            await RefreshProfiles();
        }
    }

    private async Task RefreshProfiles()
    {
        lbProfiles.Items.Clear();
        lbProfiles.Items.Add(CreateProfileItem());

        // no profiles, return
        if (UserProfile.GetProfilesCount() == 0) return;

        List<UserProfile> profiles = await UserProfile.GetProfilesAsync();
        if (profiles.Count == 0)
        {
            return;
        }

        lbProfiles.Items.Add(new Separator());

        foreach (UserProfile profile in profiles)
        {
            ListBoxItem lbi = new ListBoxItem
            {
                Tag = profile,
                Content = $"{profile.Name}"
            };

            lbi.MouseDoubleClick += (s, e) =>
            {
                // select user profile
                UserProfile.SetCurrent(profile);

                // nav to dashboard
                MainWindow.SetActivePage(PgDashboard.Instance, MainWindow.Instance.btnDashboard);
            };

            lbProfiles.Items.Add(lbi);
        }
    }

    #region Static code

    private static PgProfileSelector? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="PgProfileSelector"/> class.
    /// </summary>
    public static PgProfileSelector? Instance => _instance ??= new PgProfileSelector();

    #endregion
}
