using ResourceRadar.Core;
using ResourceRadar.Core.Authentication;

using System.Linq;
using System.Threading.Tasks;
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
        this.Loaded += async (s, e) =>
        {
            await ReloadUI();
        };

        this.KeyDown += async (s, e) =>
        {
            if (e.Key == System.Windows.Input.Key.F5)
            {
                FZCore.Core.InfoBox("REFRESH INVOKED");
                await ReloadUI();
            }
        };
    }

    private async Task ReloadUI()
    {
        if (UserProfile.Current == null)
        {
            App.NoLoggedUser();
            return;
        }

        // get data
        string name = UserProfile.Current.Name;
        int count = Analytics.Count(UserProfile.Current.Items);
        decimal totalValue = await Analytics.GetTotalValueAsync(UserProfile.Current.Items);
        var last = await Analytics.GetHistoryAsync(UserProfile.Current.Items, 1);

        // display data
        rName.Text = name;
        rItemsCount.Text = count.ToString();
        rTotalValue.Text = totalValue.ToString("C");
        rLast.Text = (last.Any() ? last.First().Name : "No items created.");
        return;
    }

    #region Static code

    private static PgAnalytics? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="PgAnalytics"/> class.
    /// </summary>
    public static PgAnalytics Instance => _instance ??= new PgAnalytics();

    #endregion
}
