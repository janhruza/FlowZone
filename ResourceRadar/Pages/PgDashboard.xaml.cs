using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ResourceRadar.Core;
using ResourceRadar.Core.Authentication;

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
        this.Loaded += async (s, e) =>
        {
            await RefreshUI();
        };
    }

    private async Task RefreshUI()
    {
        if (UserProfile.Current == null)
        {
            // no loaded user
            return;
        }

        // clear old data
        stpHistory.Children.Clear();

        // get and await for data
        int count = Analytics.Count(UserProfile.Current.Items);
        InventoryItemsCollection history = [];

        var items = UserProfile.Current.Items;
        if (items.Count == 0)
        {
            stpHistory.Children.Add(new Label { Content = "No items created so far.", Foreground = Brushes.Red, FontWeight = FontWeights.Bold});
        }

        else
        {
            history = await Analytics.GetHistoryAsync(UserProfile.Current.Items, 5);
        }

        // display retrieved data
        rStoredItemsCount.Text = count.ToString();

        int x = 0;
        foreach (var item in history)
        {
            Label lbl = new Label
            {
                Content = item.Name,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Cascadia Mono"),
            };

            Label lblDate = new Label
            {
                Content = item.PurchaseDate.ToShortDateString(),
                Margin = new Thickness(10,0,10,0),
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Cascadia Mono")
            };

            Button btnRemove = new Button
            {
                Content = "Remove",
                BorderBrush = SystemColors.AccentColorLight1Brush,
                Background = SystemColors.AccentColorBrush,
            };

            btnRemove.Click += async (s, e) =>
            {
                if (UserProfile.Current == null) return;

                if (InventoryItem.ConfirmDelete([item]) == true)
                {
                    UserProfile.Current.Items.Remove(item);
                    await RefreshUI();
                }
            };

            Grid g = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(),
                    new ColumnDefinition() { Width = GridLength.Auto },
                    new ColumnDefinition() { Width = GridLength.Auto }
                },

                Children =
                {
                    lbl,
                    lblDate,
                    btnRemove
                }
            };

            Grid.SetColumn(lbl, 0);
            Grid.SetColumn(lblDate, 1);
            Grid.SetColumn(btnRemove, 2);


            Border bd = new Border
            {
                Child = g,
                Padding = new Thickness(5, 5, 5, 5),
                Background = (x % 2 == 1 ? Brushes.Transparent : new SolidColorBrush(Color.FromArgb(0x35, 0x00, 0x00, 0x00))),
                Margin = new Thickness(0, 0, 0, 0)
            };

            stpHistory.Children.Add(bd);
            x++;
        }

        return;
    }

    #region Static code

    private static PgDashboard? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="PgDashboard"/> class.
    /// </summary>
    public static PgDashboard Instance => _instance ??= new PgDashboard();

    #endregion

    private async void btnRefresh_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        await RefreshUI();
    }
}
