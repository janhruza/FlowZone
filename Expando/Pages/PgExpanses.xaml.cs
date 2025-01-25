using System.Windows.Controls;

namespace Expando.Pages;

/// <summary>
/// Representing the expanses page.
/// </summary>
public partial class PgExpanses : Page
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgExpanses"/> class.
    /// </summary>
    public PgExpanses()
    {
        InitializeComponent();
    }

    #region Static code

    private static PgExpanses? _instance;

    /// <summary>
    /// Representing the working instance of the <see cref="PgExpanses"/> class.
    /// </summary>
    public static PgExpanses Instance
    {
        get
        {
            _instance ??= new PgExpanses();
            return _instance;
        }
    }

    #endregion
}
