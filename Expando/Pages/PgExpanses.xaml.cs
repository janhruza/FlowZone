using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Expando.Core;

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
    }

    /// <summary>
    /// Reloads the page UI.
    /// </summary>
    public void ReloadUI()
    {
        // clear the list of expanses
        lbExpanses.Items.Clear();

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

        foreach (Transaction expanse in UserProfile.Current.Transactions)
        {
            // draw transaction items as listbox items
            lbExpanses.Items.Add(CreateTransactionItem(expanse));
        }

        return;
    }

    private ListBoxItem CreateTransactionItem(Transaction transaction)
    {
        // listbox item itself
        ListBoxItem lbi = new ListBoxItem
        {
            Tag = transaction.Id
        };

        // container border
        Border bd = new Border();

        // value info (label)
        Label lbValue = new Label
        {
            // set content as the transaction value with the currency format
            Content = transaction.Value.ToString("C"),
            FontSize = 16
        };

        // description info (label)
        Label lbDescription = new Label
        {
            // get the transaction description
            Content = (string.IsNullOrEmpty(transaction.Description.Trim()) == false ? transaction.Description : Messages.NoDescription),
            FontSize = 12
        };

        // data panel
        StackPanel sp = new StackPanel
        {
            Children =
            {
                lbValue,
                lbDescription,
            }
        };

        // set data panel as the border content
        bd.Child = sp;

        // set the listbox item content
        lbi.Content = bd;
        return lbi;
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
