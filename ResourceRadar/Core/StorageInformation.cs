namespace ResourceRadar.Core;

/// <summary>
/// Representing the information about the storage of a single <see cref="InventoryItem"/>.
/// </summary>
public class StorageInformation
{
    /// <summary>
    /// Creates a new instance of the <see cref="StorageInformation"/> class.
    /// </summary>
    public StorageInformation()
    {
        Room = string.Empty;
        StorageArea = string.Empty;
        Condition = string.Empty;
    }

    /// <summary>
    /// Gets or sets the room of the item.
    /// </summary>
    public string Room { get; set; }  // E.g., Kitchen, Living Room, Garage

    /// <summary>
    /// Representing the storage area of the item.
    /// </summary>
    public string StorageArea { get; set; }  // Shelf, cabinet, box name

    /// <summary>
    /// Representing the condition of the stored item.
    /// </summary>
    public string Condition { get; set; }  // E.g., New, Used, Damaged
}
