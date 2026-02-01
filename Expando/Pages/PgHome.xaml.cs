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
            rChangeUser.MouseEnter += (s, e) => rChangeUser.Foreground = SystemColors.AccentColorLight2Brush;
            rChangeUser.MouseLeave += (s, e) => rChangeUser.Foreground = SystemColors.AccentColorBrush;

            // view log window
            rLog.MouseEnter += (s, e) => rLog.Foreground = SystemColors.AccentColorLight2Brush;
            rLog.MouseLeave += (s, e) => rLog.Foreground = SystemColors.AccentColorBrush;
        };
    }

    /// <summary>
    /// Reloads the page UI.
    /// </summary>
    public void ReloadUI()
    {
        if (IsLoaded == false) return;

        rName.Text = UserProfile.Current?.Username ?? "Anonymous";
        rDate.Text = DateTime.Now.ToLongDateString();
    }

    private void rChangeUser_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        Page pg = PgProfiles.Instance;
        _ = MainWindow.SetActivePage(ref pg);
    }

    private void rLog_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        LogViewer lvWindow = new LogViewer(Log.Path);
        lvWindow.ShowDialog();
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
