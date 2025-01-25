using System.Windows.Controls;
using Expando.Core;

namespace Expando.Pages;

/// <summary>
/// Representing a user profile page (not a profile selection page).
/// </summary>
public partial class PgMyProfile : Page, IExpandoPage
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgMyProfile"/> class.
    /// </summary>
    public PgMyProfile()
    {
        InitializeComponent();

        // load info on load
        this.Loaded += (s, e) =>
        {
            ReloadUI();
        };
    }

    /// <summary>
    /// Reloads the page UI.
    /// </summary>
    public void ReloadUI()
    {
        if (UserProfile.IsProfileLoaded() == false)
        {
            // no profile loaded
            // can't show any info
            Page pg = PgNoProfile.Instance;
            MainWindow.SetActivePage(ref pg);
            return;
        }

        // show profile info
        this.rUsername.Text = UserProfile.Current?.Username.ToUpper();
        this.rCreation.Text = $"{UserProfile.Current?.CreationDate.ToShortDateString()} {UserProfile.Current?.CreationDate.ToShortTimeString()}";
        this.rId.Text = UserProfile.Current?.Id.ToString();

        return;
    }

    #region Static code

    private static PgMyProfile? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="PgMyProfile"/> class.
    /// </summary>
    public static PgMyProfile Instance
    {
        get
        {
            _instance ??= new PgMyProfile();
            return _instance;
        }
    }

    #endregion
}
