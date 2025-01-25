using System.Windows;
using System.Windows.Controls;
using Expando.Core;
using Expando.Windows;

namespace Expando.Pages;

/// <summary>
/// Representing a profile selection page.
/// </summary>
public partial class PgProfiles : Page, IExpandoPage
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgProfiles"/> class.
    /// </summary>
    public PgProfiles()
    {
        InitializeComponent();

        this.Loaded += (s, e) =>
        {
            ReloadUI();
        };

        this.KeyDown += (s, e) =>
        {
            if (e.Key == System.Windows.Input.Key.F5)
            {
                ReloadUI();
            }
        };
    }

    private void CreateNewUser()
    {
        if (WndNewProfile.CreateUser() == true)
        {
            ReloadUI();
        }
    }

    private ListBoxItem NewProfileItem()
    {
        Border bdContent = new Border();

        Label lblHeader = new Label
        {
            Content = "Create a new profile",
            FontSize = 18
        };

        Label lblDescription = new Label
        {
            Content = "New profile will be created in a new window.",
            FontSize = 14
        };

        Button btnCreate = new Button
        {
            Content = "Create profile"
        };

        btnCreate.Click += (s, e) =>
        {
            CreateNewUser();
        };

        StackPanel sp = new StackPanel
        {
            Children =
                {
                    lblHeader,
                    lblDescription,
                    btnCreate
                }
        };

        bdContent.Child = sp;

        ListBoxItem lbi = new ListBoxItem
        {
            Content = bdContent
        };

        return lbi;
    }

    private ListBoxItem ProfileMenuItem(UserProfile profile)
    {
        Border bdContent = new Border();

        Label lUsername = new Label
        {
            Content = profile.Username,
            FontWeight = FontWeights.Bold,
            FontSize = 18
        };

        Label lCreated = new Label
        {
            Content = profile.CreationDate.ToShortDateString(),
            FontSize = 14
        };

        Label lProfileId = new Label
        {
            Content = "ID: " + ((ulong)profile.Id).ToString(),
            FontSize = 12
        };

        StackPanel sp = new StackPanel
        {
            Children =
                {
                    lUsername,
                    lCreated,
                    lProfileId
                }
        };

        bdContent.Child = sp;

        ListBoxItem item = new ListBoxItem
        {
            Content = bdContent,
            Tag = profile.Id
        };

        item.MouseDoubleClick += (s, e) =>
        {
            // confirm profile selection
            SelectProfile(ref profile);
        };

        return item;
    }

    /// <summary>
    /// Reloads the page UI.
    /// </summary>
    public void ReloadUI()
    {
        lbProfiles.Items.Clear();

        if (UserProfile.Profiles.Count == 0)
        {
            // no profiles found
            // draw an item that redirects user to a create new profile page

            lbProfiles.Items.Add(NewProfileItem());
            return;
        }

        lbProfiles.Items.Add(NewProfileItem());
        lbProfiles.Items.Add(new Separator());

        foreach (UserProfile profile in UserProfile.Profiles)
        {
            // add an item with associated profile
            lbProfiles.Items.Add(ProfileMenuItem(profile));
        }

        return;
    }

    #region Static data

    private static PgProfiles? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="PgProfiles"/> class.
    /// </summary>
    public static PgProfiles Instance
    {
        get
        {
            _instance ??= new PgProfiles();
            return _instance;
        }
    }

    private static void SelectProfile(ref UserProfile profile)
    {
        // selects a single user profile and
        // navigates to the home page with new
        // user profile loaded

        UserProfile.Current = profile;
        MainWindow.SetHomePage();
        return;
    }

    #endregion

    private void miRefresh_Click(object sender, RoutedEventArgs e)
    {
        ReloadUI();
    }
}
