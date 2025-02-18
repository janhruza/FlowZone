using System;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceRadar.Core;

/// <summary>
/// Representing the analytics class for <see cref="InventoryItemsCollection"/>.
/// </summary>
public class Analytics
{
    /// <summary>
    /// Gets the number of items inside of the <paramref name="items"/> collection.
    /// </summary>
    /// <param name="items">All items in the collection.</param>
    /// <returns>Number of items in the collecton.</returns>
    public static int Count(InventoryItemsCollection items) => items.Count;

    /// <summary>
    /// Gets all items from the <paramref name="collection"/> whose expiration date has come.
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static Task<InventoryItemsCollection> ExpiredAsync(InventoryItemsCollection collection)
    {
        return Task.Run<InventoryItemsCollection>(() =>
        {
            InventoryItemsCollection list = [];

            foreach (var item in collection)
            {
                // skip items with no expiration date
                if (item.ExpirationDate == DateTime.MinValue) continue;

                if (item.ExpirationDate <= DateTime.Now)
                {
                    list.Add(item);
                }
            }

            return list;
        });
    }

    /// <summary>
    /// Gets the sum of all the items' <see cref="InventoryItem.PurchasePrice"/> properties in the given <paramref name="collection"/>.
    /// </summary>
    /// <param name="collection">Collection of all items to analyze.</param>
    /// <returns>The sum of all items' purchasing prices.</returns>
    public static Task<decimal> GetTotalValueAsync(InventoryItemsCollection collection)
    {
        return Task.Run<decimal>(() =>
        {
            decimal total = collection.Sum(x => x.PurchasePrice);
            return total;
        });
    }

    /// <summary>
    /// Gets a list of all items that are about to expire (determined by the <paramref name="daysThreshold"/> variable).
    /// </summary>
    /// <param name="collection">Collection of items.</param>
    /// <param name="daysThreshold">Number of dates ahead of today that is considered being about to expire.</param>
    /// <returns>Líst of all items that are about to expire.</returns>
    public static Task<InventoryItemsCollection> NearExpiredAsync(InventoryItemsCollection collection, int daysThreshold)
    {
        return Task.Run<InventoryItemsCollection>(() =>
        {
            InventoryItemsCollection list = [];

            // target date (with threshold)
            DateTime date = DateTime.Now.AddDays(daysThreshold);

            foreach (var item in collection)
            {
                // skip items with no expiration date
                if (item.ExpirationDate == DateTime.MinValue) continue;

                if (item.ExpirationDate <= date)
                {
                    list.Add(item);
                }
            }

            return list;
        });
    }

    /// <summary>
    /// Gets the <paramref name="topN"/> number of the most expansive items.
    /// </summary>
    /// <param name="collection">Collection of items.</param>
    /// <param name="topN">Number of items to return.</param>
    /// <returns>Returns the <paramref name="topN"/> most expansive items from the given <paramref name="collection"/>.</returns>
    public static Task<InventoryItemsCollection> GetMostExpansiveItemsAsync(InventoryItemsCollection collection, int topN)
    {
        return Task.Run<InventoryItemsCollection>(() =>
        {
            // logic checks
            if (topN <= 0) return [];
            if (collection.Count < topN) return [];

            var list = collection.OrderByDescending(x => x.PurchasePrice).Take(topN).ToList();
            return (InventoryItemsCollection)list;
        });
    }

    /// <summary>
    /// Gets the most expansive item from the <paramref name="collection"/> of items.
    /// </summary>
    /// <param name="collection">Representing the collection of items.</param>
    /// <returns>The most expansive item or null, if the <paramref name="collection"/> is empty.</returns>
    public static Task<InventoryItem?> GetMostExpansiveItemAsync(InventoryItemsCollection collection)
    {
        return Task.Run<InventoryItem?>(() =>
        {
            // logic checks
            if (collection.Any() == false) return null;
            
            // gets the most expansive item
            return collection.OrderByDescending(x => x.PurchasePrice).First();
        });
    }

    /// <summary>
    /// Gets the list of all items with expiration date within the <paramref name="daysThreshold"/> in the future.
    /// </summary>
    /// <param name="collection">Collection of items.</param>
    /// <param name="daysThreshold">Number of days in the future from today that is considered to be in range for near expiration date.</param>
    /// <returns>List of all items with expiration date in the near future - determined by the <paramref name="daysThreshold"/> variable.</returns>
    public static Task<InventoryItemsCollection> NearExpiredWarrantyAsync(InventoryItemsCollection collection, int daysThreshold)
    {
        return Task.Run<InventoryItemsCollection>(() =>
        {
            if (collection.Any() == false) return [];

            if (daysThreshold < 0) return [];

            DateTime date = DateTime.Now.AddDays(daysThreshold);
            InventoryItemsCollection list = [];

            foreach (var item in collection)
            {
                if (item.HasWarranty == false || item.WarrantyExpiry.HasValue == false) continue;

                if (item.WarrantyExpiry.Value < date)
                {
                    list.Add(item);
                }
            }

            return list;
        });
    }

    /// <summary>
    /// Gets the list of all items with their warranties expired.
    /// </summary>
    /// <param name="collection">Collection of items.</param>
    /// <returns>List of items with their warranties expired.</returns>
    public static Task<InventoryItemsCollection> ExpiredWarrantyAsync(InventoryItemsCollection collection)
    {
        return NearExpiredWarrantyAsync(collection, 0);
    }
}
