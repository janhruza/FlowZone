using System.Collections.Generic;
using System;
using System.Windows;
using System.Linq;

namespace ResourceRadar.Core;

/// <summary>
/// Representing a single inventory item.
/// </summary>
public class InventoryItem
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Item name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Category of the item.
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// Item's brand.
    /// </summary>
    public string Brand { get; set; }

    /// <summary>
    /// Item's model (if any).
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// Representing the storage location of the item.
    /// </summary>
    public StorageInformation Storage { get; init; }

    /// <summary>
    /// Representing the date of the item's purchase.
    /// </summary>
    public DateTime PurchaseDate { get; set; }

    /// <summary>
    /// Representing the price of the item at purchase time.
    /// </summary>
    public decimal PurchasePrice { get; set; }

    /// <summary>
    /// Representing the name of the store where the item was purchased from.
    /// </summary>
    public string PurchasedFrom { get; set; }  // Store name, online retailer

    /// <summary>
    /// Representing whether the item has a warranty or not.
    /// </summary>
    public bool HasWarranty { get; set; }

    /// <summary>
    /// Representing the warranty expiration date (if any).
    /// </summary>
    public DateTime? WarrantyExpiry { get; set; }

    /// <summary>
    /// Representing whether the item is in use or not.
    /// </summary>
    public bool IsInUse { get; set; }  // If actively used or stored

    /// <summary>
    /// Representing the last date and time when the item was used.
    /// </summary>
    public DateTime? LastUsed { get; set; }

    /// <summary>
    /// Representing an expiration date (for perishable items).
    /// </summary>
    public DateTime? ExpirationDate { get; set; }  // For perishable items

    /// <summary>
    /// Representing the maintance history.
    /// </summary>
    public List<string> MaintenanceHistory { get; set; }  // Repairs, cleanings

    /// <summary>
    /// Representing additional attachments (recipts, user manuals, images, etc.).
    /// </summary>
    public List<string> Attachments { get; set; }  // Receipts, user manuals, images

    /// <summary>
    /// Representing any additional notes.
    /// </summary>
    public string Notes { get; set; }  // Personal notes

    /// <summary>
    /// Creates a new instance of the <see cref="InventoryItem"/> class with default values.
    /// </summary>
    public InventoryItem()
    {
        Name = string.Empty;
        Category = string.Empty;
        Brand = string.Empty;
        Model = string.Empty;
        Storage = new StorageInformation();
        PurchasedFrom = string.Empty;
        Notes = string.Empty;
        MaintenanceHistory = [];
        Attachments = [];
    }

    #region Static code

    /// <summary>
    /// Shows a confirmation dialog for item deletion.
    /// </summary>
    /// <param name="items">List of items or a single item.</param>
    /// <returns>True, if user confirms the action, otherwise false.</returns>
    public static bool ConfirmDelete(InventoryItemsCollection items)
    {
        MessageBoxResult result;

        if (items.Any() == false)
        {
            return false;
        }

        if (items.Count == 1)
        {
            // single item
            result = MessageBox.Show($"Are you sure you want to permanently remove \'{items.First().Name}\' from your inventory? This action is irreversible.", Messages.REMOVE_ITEM_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Warning);
        }

        else
        {
            // multiple items
            result = MessageBox.Show($"Are you sure you want to permanently remove {items.Count} items from your inventory? This action is irreversible.", Messages.REMOVE_ITEM_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Warning);
        }

        return result == MessageBoxResult.Yes;
    }

    #endregion
}

/// <summary>
/// Representing a list of multiple <see cref="InventoryItem"/>s.
/// </summary>
public class InventoryItemsCollection : List<InventoryItem>
{
    /// <summary>
    /// Representing the base consructor.
    /// </summary>
    public InventoryItemsCollection() : base() { }

    /// <summary>
    /// Creates the collection with the items from the <paramref name="collection"/>.
    /// </summary>
    /// <param name="collection">Collection of items to add into the list.</param>
    public InventoryItemsCollection(IEnumerable<InventoryItem> collection) : base(collection) { }
}