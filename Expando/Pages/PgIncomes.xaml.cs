using System.Windows.Controls;

namespace Expando.Pages;

/// <summary>
/// Representing the incomes page.
/// </summary>
public partial class PgIncomes : Page
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgIncomes"/> class.
    /// </summary>
    public PgIncomes()
    {
        InitializeComponent();
    }

    #region Static code

    private static PgIncomes? _instance;

    /// <summary>
    /// Representing the working instance of the <see cref="PgIncomes"/> class.
    /// </summary>
    public static PgIncomes Instance
    {
        get
        {
            _instance ??= new PgIncomes();
            return _instance;
        }
    }

    #endregion
}
