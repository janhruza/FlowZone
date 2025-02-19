using System;
using System.Threading.Tasks;
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

        Grid g = new Grid
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Width = double.NaN
        };

        g.ColumnDefinitions.Add(new ColumnDefinition());
        g.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

        // Control buttons
        // Remove button
        Button btnRemove = new Button
        {
            Content = "Remove",
            Visibility = Visibility.Collapsed
        };

        btnRemove.Click += (s, e) =>
        {
            UserProfile.Current.Items.Remove(item);
            RefreshItems();
        };

        g.Children.Add(btnRemove);
        Grid.SetColumn(btnRemove, 1);

        Label lblText = new Label
        {
            Content = item.Name
        };

        g.Children.Add(lblText);
        Grid.SetColumn(lblText, 0);

        ListBoxItem lbi = new ListBoxItem
        {
            Content = g
        };

        lbi.MouseEnter += (s, e) =>
        {
            btnRemove.Visibility = Visibility.Visible;
        };

        lbi.MouseLeave += (s, e) =>
        {
            btnRemove.Visibility = Visibility.Collapsed;
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
