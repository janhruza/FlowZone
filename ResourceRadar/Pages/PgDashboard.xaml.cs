using System.Windows.Controls;

namespace ResourceRadar.Pages;

/// <summary>
/// Representing the dashboard page.
/// </summary>
public partial class PgDashboard : Page
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgDashboard"/> class.
    /// </summary>
    public PgDashboard()
    {
        InitializeComponent();
    }

    #region Static code

    private static PgDashboard? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="PgDashboard"/> class.
    /// </summary>
    public static PgDashboard Instance => _instance ??= new PgDashboard();

    #endregion
}
