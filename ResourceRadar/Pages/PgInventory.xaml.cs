using System.Windows.Controls;

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
    }

    #region Static code

    private static PgInventory? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="PgInventory"/> class.
    /// </summary>
    public static PgInventory Instance => _instance ??= new PgInventory();

    #endregion
}
