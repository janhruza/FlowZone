using System.Windows.Controls;

namespace Expando.Pages;

/// <summary>
/// Representing a no profile loaded page.
/// </summary>
public class PgNoProfile : Page, IExpandoPage
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgNoProfile"/> class.
    /// </summary>
    public PgNoProfile()
    {
        // set page properties
        Title = "No profile loaded";

        // reload page UI
        ReloadUI();
    }

    /// <summary>
    /// Reloads the page UI.
    /// </summary>
    public void ReloadUI()
    {
        // construct the UI

        // container parent
        Border bd = new Border
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Center,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center
        };

        // elements
        Label lMessage = new Label
        {
            Content = new TextBlock
            {
                Text = Messages.NoProfileLoaded,
                TextWrapping = System.Windows.TextWrapping.Wrap,
                FontSize = 16
            }
        };

        Button btnProfiles = new Button
        {
            Content = "Go to Profiles",
            VerticalAlignment = System.Windows.VerticalAlignment.Center,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Right
        };

        btnProfiles.Click += (s, e) =>
        {
            Page pgSelection = PgProfiles.Instance;
            _ = MainWindow.SetActivePage(ref pgSelection);
        };

        // elements container
        StackPanel sp = new StackPanel
        {
            Children =
            {
                lMessage,
                btnProfiles,
            }
        };

        bd.Child = sp;
        Content = bd;
        return;
    }

    #region Static code

    private static PgNoProfile? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="PgNoProfile"/> class.
    /// </summary>
    public static PgNoProfile Instance
    {
        get
        {
            _instance ??= new PgNoProfile();
            return _instance;
        }
    }

    #endregion
}
