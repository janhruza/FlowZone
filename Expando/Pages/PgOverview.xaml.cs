using System.Windows.Controls;

namespace Expando.Pages;

/// <summary>
/// Representing the overview page.
/// </summary>
public partial class PgOverview : Page
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgOverview"/> class.
    /// </summary>
    public PgOverview()
    {
        InitializeComponent();
    }

    #region Static code

    private static PgOverview? _instance;

    /// <summary>
    /// Representing the working instance of the <see cref="PgOverview"/> class.
    /// </summary>
    public static PgOverview Instance
    {
        get
        {
            _instance ??= new PgOverview();
            return _instance;
        }
    }

    #endregion
}
