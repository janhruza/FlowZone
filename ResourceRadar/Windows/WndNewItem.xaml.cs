using ResourceRadar.Core;

using System;
using System.Windows;
using System.Windows.Controls;

namespace ResourceRadar.Windows;

/// <summary>
/// Representing the new inventory iem window.
/// </summary>
public partial class WndNewItem : Window
{
    /// <summary>
    /// Creates a new instance of the <see cref="WndNewItem"/> class.
    /// </summary>
    public WndNewItem()
    {
        InitializeComponent();

        // Creates the item
        _item = new InventoryItem();

        // register events
        foreach (Expander ex in expanders)
        {
            ex.Expanded += (s, e) =>
            {
                foreach (var exp in expanders)
                {
                    if (ex == exp)
                    {
                        continue;
                    }

                    else
                    {
                        exp.IsExpanded = false;
                    }
                }
            };
        }
    }

    private Expander[] expanders => [eBasicInfo, eStorage, ePurchase, eOther];

    private void Close(bool result)
    {
        DialogResult = result;
        this.Close();
        return;
    }

    private InventoryItem _item;

    /// <summary>
    /// Representing the created inventory item.
    /// </summary>
    public InventoryItem Value => _item;

    private void ValidateData()
    {
        _item.Id = DateTime.Now.ToBinary();
        _item.Name = txtName.Text.Trim();
        _item.Category = txtCategory.Text.Trim();
        _item.Brand = txtBrand.Text.Trim();
        _item.Model = txtModel.Text.Trim();

        _item.Storage.Room = txtStorageRoom.Text.Trim();
        _item.Storage.StorageArea = txtStorageArea.Text.Trim();
        _item.Storage.Condition = txtStorageCondition.Text.Trim();

        _item.PurchaseDate = dtPurchase.SelectedDate ?? DateTime.Now;
        _item.PurchasePrice = Convert.ToDecimal(txtPrice.Text);
        _item.ExpirationDate = dtExpiration.SelectedDate ?? DateTime.MinValue;
        _item.WarrantyExpiry = dtWarranty.SelectedDate ?? DateTime.MinValue;

        _item.PurchasedFrom = txtSeller.Text.Trim();
        _item.Notes = txtNotes.Text;
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        this.Close(false);
    }

    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
        ValidateData();
        this.Close(true);
    }

    private void cbHasWarranty_Checked(object sender, RoutedEventArgs e)
    {
        stpWarranty.Visibility = Visibility.Visible;
    }

    private void cbHasWarranty_Unchecked(object sender, RoutedEventArgs e)
    {
        stpWarranty.Visibility = Visibility.Collapsed;
    }
}
