using Expando.Core;

using FZCore;
using FZCore.Windows.Extra;

using System;
using System.Windows;
using System.Windows.Controls;

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

        Loaded += (s, e) =>
        {
            // reloads the UI
            ReloadUI();

            // adds listeners to the events
            // change user page
            this.rChangeUser.MouseEnter += (s, e) => this.rChangeUser.Foreground = SystemColors.AccentColorLight2Brush;
            this.rChangeUser.MouseLeave += (s, e) => this.rChangeUser.Foreground = SystemColors.AccentColorBrush;

            // view log window
            this.rLog.MouseEnter += (s, e) => this.rLog.Foreground = SystemColors.AccentColorLight2Brush;
            this.rLog.MouseLeave += (s, e) => this.rLog.Foreground = SystemColors.AccentColorBrush;
        };
    }

    /// <summary>
    /// Reloads the page UI.
    /// </summary>
    public void ReloadUI()
    {
        if (IsLoaded == false) return;

        this.rName.Text = UserProfile.Current?.Username ?? "Anonymous";
        this.rDate.Text = DateTime.Now.ToLongDateString();
    }

    private void rChangeUser_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        Page pg = PgProfiles.Instance;
        _ = MainWindow.SetActivePage(ref pg);
    }

    private void rLog_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        LogViewer lvWindow = new LogViewer(Log.Path);
        _ = lvWindow.ShowDialog();
    }

    #region Static code

    private static PgHome? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="PgHome"/> class.
    /// </summary>
    public static PgHome Instance
    {
        get
        {
            return _instance ??= new PgHome();
        }
    }

    #endregion
}
