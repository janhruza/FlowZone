using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using FZCore;
using ResourceRadar.Core;
using ResourceRadar.Core.Authentication;

namespace ResourceRadar.Pages;

/// <summary>
/// Representing the inventory page.
/// </summary>
public partial class PgInventory : Page
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgInventory"/> class.
    /// </summary>
    public PgInventory()
    {
        InitializeComponent();

        this.Loaded += this.PgInventory_Loaded;
    }

    private async void PgInventory_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        await RefreshItems();
    }

    private ListBoxItem? CreateListItem(InventoryItem item)
    {
        if (UserProfile.Current == null)
        {
            return null;
        }

        // menu items
        MenuItem miDelete = new MenuItem
        {
            Header = "Delete",
            InputGestureText = "Del"
        };

        miDelete.Click += async (s, e) =>
        {
            await RemoveItem(item);
        };

        // list box item itself
        ListBoxItem lbi = new ListBoxItem
        {
            Content = item.Name,
            ContextMenu = new ContextMenu
            {
                Items =
                {
                    miDelete
                }
            }
        };

        lbi.KeyDown += async (s, e) =>
        {
            if (e.Key == Key.Delete)
            {
                await RemoveItem(item);
            }
        };

        return lbi;
    }

    private async Task RemoveItem(InventoryItem item)
    {
        if (UserProfile.Current == null)
        {
            return;
        }

        if (InventoryItem.ConfirmDelete([item]) == true)
        {
            if (UserProfile.Current.Items.Remove(item) == true)
            {
                await RefreshItems();
            }
        }
    }

    private bool FilterItems => string.IsNullOrEmpty(txtSearch.Text.Trim()) == false;

    private async Task RefreshItems()
    {
        lbiItems.Items.Clear();

        if (UserProfile.Current == null)
        {
            return;
        }

        if (UserProfile.Current.Items.Count == 0)
        {
            return;
        }

        if (FilterItems == false)
        {
            // refresh UI
            foreach (InventoryItem item in UserProfile.Current.Items)
            {
                // create item
                ListBoxItem? lbi = CreateListItem(item);

                if (lbi == null) continue;
                lbiItems.Items.Add(lbi);
            }
        }

        else
        {
            InventoryItemsCollection items = await Analytics.FilterItemsAsync(UserProfile.Current.Items, txtSearch.Text.Trim());

            if (items.Any() == false)
            {
                ListBoxItem lbi = new ListBoxItem
                {
                    Content = $"No results for \'{txtSearch.Text.Trim()}\' found.",
                    Background = SystemColors.AccentColorBrush
                };

                lbiItems.Items.Add(lbi);
            }

            else
            {
                foreach (var item in items)
                {
                    // create item
                    ListBoxItem? lbi = CreateListItem(item);

                    if (lbi == null) continue;
                    lbiItems.Items.Add(lbi);
                }
            }
        }
    }

    #region Static code

    private static PgInventory? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="PgInventory"/> class.
    /// </summary>
    public static PgInventory Instance => _instance ??= new PgInventory();

    #endregion

    private async void btnRefresh_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        await RefreshItems();
    }

    private async void btnAddItem_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if (UserProfile.Current == null)
        {
            // no user logged in
            App.NoLoggedUser();
            return;
        }

        if (App.AddNewItem(out InventoryItem? item) == true)
        {
            if (item == null)
            {
                Log.Error(Messages.NO_ITEM_CREATED, nameof(btnAddItem_Click));
                _ = MessageBox.Show(Messages.NO_ITEM_CREATED, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            UserProfile.Current.Items.Add(item);
            await RefreshItems();
        }
    }

    private async void txtSearch_TextChanged(object sender, RoutedEventArgs e)
    {
        await RefreshItems();
    }
}
