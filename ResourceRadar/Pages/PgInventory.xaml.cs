using System.Windows;
using System.Windows.Controls;
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

    private void PgInventory_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        RefreshItems();
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

        miDelete.Click += (s, e) =>
        {
            if (UserProfile.Current.Items.Remove(item) == true)
            {
                RefreshItems();
            }
        };

        // list box item iteself
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

        return lbi;
    }


    private void RefreshItems()
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

        // refresh UI
        foreach (InventoryItem item in UserProfile.Current.Items)
        {
            // create item
            ListBoxItem? lbi = CreateListItem(item);

            if (lbi == null) continue;
            lbiItems.Items.Add(lbi);
        }
    }

    #region Static code

    private static PgInventory? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="PgInventory"/> class.
    /// </summary>
    public static PgInventory Instance => _instance ??= new PgInventory();

    #endregion

    private void btnRefresh_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        RefreshItems();
    }

    private void btnAddItem_Click(object sender, System.Windows.RoutedEventArgs e)
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
            RefreshItems();
        }
    }
}
