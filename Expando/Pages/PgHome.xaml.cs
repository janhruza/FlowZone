using System;
using System.Windows;
using System.Windows.Controls;
using Expando.Core;

namespace Expando.Pages;

/// <summary>
/// Representing the home page.
/// </summary>
public partial class PgHome : Page, IExpandoPage
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgHome"/> class.
    /// </summary>
    public PgHome()
    {
        InitializeComponent();

        this.Loaded += (s, e) =>
        {
            // reloads the UI
            ReloadUI();

            // adds listeners to the events
            rChangeUser.MouseEnter += (s, e) => rChangeUser.Foreground = SystemColors.AccentColorLight2Brush;
            rChangeUser.MouseLeave += (s, e) => rChangeUser.Foreground = SystemColors.AccentColorBrush;
        };
    }

    /// <summary>
    /// Reloads the page UI.
    /// </summary>
    public void ReloadUI()
    {
        if (this.IsLoaded == false) return;

        rName.Text = UserProfile.Current?.Username ?? "Anonymous";
        rDate.Text = DateTime.Now.ToLongDateString();
    }

    private void rChangeUser_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        Page pg = PgProfiles.Instance;
        MainWindow.SetActivePage(ref pg);
    }

    #region Static code

    private static PgHome _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="PgHome"/> class.
    /// </summary>
    public static PgHome Instance
    {
        get
        {
            if (_instance != null) return _instance;
            
            _instance = new PgHome();
            return _instance;
        }
    }

    #endregion
}
