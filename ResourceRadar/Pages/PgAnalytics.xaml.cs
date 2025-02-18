using System.Windows.Controls;

namespace ResourceRadar.Pages;

/// <summary>
/// Representing the analytics page.
/// </summary>
public partial class PgAnalytics : Page
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgAnalytics"/> page.
    /// </summary>
    public PgAnalytics()
    {
        InitializeComponent();
    }

    #region Static code

    private static PgAnalytics? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="PgAnalytics"/> class.
    /// </summary>
    public static PgAnalytics Instance => _instance ??= new PgAnalytics();

    #endregion
}
