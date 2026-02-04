using Expando.Core;
using Expando.Windows;

using System.Windows.Controls;

namespace Expando.Pages;

/// <summary>
/// Representing the expanses page.
/// </summary>
public partial class PgExpanses : Page, IExpandoPage
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgExpanses"/> class.
    /// </summary>
    public PgExpanses()
    {
        InitializeComponent();

        Loaded += (s, e) =>
        {
            ReloadUI();
        };
    }

    /// <summary>
    /// Reloads the page UI.
    /// </summary>
    public void ReloadUI()
    {
        // clear the list of expanses
        this.lbExpanses.Items.Clear();

        if (UserProfile.Current == null)
        {
            // no profile loaded, can't to load profile data
            // draw a descriptive item (with a message)
            return;
        }

        if (UserProfile.Current.GetExpanses().Count == 0)
        {
            // no expanses yet
            return;
        }

        // list through all the expanses and draw them
        // to the list sorted by the newest to the oldest

        foreach (Transaction expanse in UserProfile.Current.GetExpanses())
        {
            // draw transaction items as listbox items
            void ModifyItem()
            {
                if (WndNewTransaction.ModifyTransaction(expanse) == true)
                {
                    ReloadUI();
                }
            }

            void RemoveItem()
            {
                if (UserProfile.Current == null)
                {
                    return;
                }

                if (UserProfile.Current.Transactions.Remove(expanse) == true)
                {
                    if (UserProfile.Current.SaveTransactions() == true)
                    {
                        ReloadUI();
                    }
                }
            }

            _ = this.lbExpanses.Items.Add(App.CreateTransactionItem(expanse, ModifyItem, RemoveItem));
        }

        return;
    }

    private void btnNewExpanse_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        // Shows a new expanse dialog
        if (WndNewTransaction.CreateExpanse() == true)
        {
            ReloadUI();
        }
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
