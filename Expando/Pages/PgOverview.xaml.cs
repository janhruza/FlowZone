using Expando.Core;

using System.Linq;
using System.Windows.Controls;

namespace Expando.Pages;

/// <summary>
/// Representing the overview page.
/// </summary>
public partial class PgOverview : Page, IExpandoPage
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgOverview"/> class.
    /// </summary>
    public PgOverview()
    {
        InitializeComponent();

        this.Loaded += (s, e) =>
        {
            ReloadUI();
        };
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

    /// <summary>
    /// Reloads the page UI.
    /// </summary>
    public void ReloadUI()
    {
        if (UserProfile.Current == null)
        {
            // no user profile loaded
            // draw a message
            rBilance.Text = "NaN";
            return;
        }

        decimal expanses = UserProfile.Current.GetExpanses().Sum(x => x.Value);
        decimal incomes = UserProfile.Current.GetIncomes().Sum(x => x.Value);
        decimal total = -expanses + incomes;

        rBilance.Text = total.ToString("C");
        return;
    }

    #endregion
}
