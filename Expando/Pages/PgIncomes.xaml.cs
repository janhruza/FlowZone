using Expando.Core;
using Expando.Windows;

using System.Windows.Controls;

namespace Expando.Pages;

/// <summary>
/// Representing the incomes page.
/// </summary>
public partial class PgIncomes : Page, IExpandoPage
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgIncomes"/> class.
    /// </summary>
    public PgIncomes()
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
        this.lbIncomes.Items.Clear();

        if (UserProfile.Current == null)
        {
            // no profile loaded, can't to load profile data
            // draw a descriptive item (with a message)
            return;
        }

        if (UserProfile.Current.GetIncomes().Count == 0)
        {
            // no incomes yet
            return;
        }

        // list through all the incomes and draw them
        // to the list sorted by the newest to the oldest

        foreach (Transaction income in UserProfile.Current.GetIncomes())
        {
            // draw transaction items as listbox items
            void ModifyItem()
            {
                if (WndNewTransaction.ModifyTransaction(income) == true)
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

                if (UserProfile.Current.Transactions.Remove(income) == true)
                {
                    if (UserProfile.Current.SaveTransactions() == true)
                    {
                        ReloadUI();
                    }
                }
            }

            _ = this.lbIncomes.Items.Add(App.CreateTransactionItem(income, ModifyItem, RemoveItem));
        }

        return;
    }

    private void btnNewIncome_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if (WndNewTransaction.CreateIncome() == true)
        {
            ReloadUI();
        }
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
