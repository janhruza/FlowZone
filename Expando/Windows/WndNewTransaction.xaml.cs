using System;
using System.Linq;
using System.Windows;
using Expando.Core;

namespace Expando.Windows;

/// <summary>
/// Representing the new transaction window.
/// </summary>
public partial class WndNewTransaction : Window
{
    /// <summary>
    /// Creates a new instance of the <see cref="WndNewTransaction"/> class.
    /// </summary>
    /// <param name="transactionType">Representing the type of the transaction. It can be either <see cref="Transaction.TypeExpanse"/> or <see cref="Transaction.TypeIncome"/>.</param>
    /// <param name="transactionBase">Representing the base transaction (if any). Used when for modifying the existing transaction.</param>
    /// <param name="editMode">Determines whether a transaction is being modified or not.</param>
    public WndNewTransaction(bool transactionType, Transaction? transactionBase = null, bool editMode = false)
    {
        InitializeComponent();
        this.transactionType = transactionType;
        this.editMode = editMode;

        if (transactionBase.HasValue && editMode == true)
        {
            Transaction tr = transactionBase.Value;
            this._transaction = new Transaction
            {
                Id = tr.Id,
                UserId = tr.UserId,
                Timestamp = tr.Timestamp,
                Type = tr.Type,
                Description = tr.Description,
                Value = tr.Value,
                Category = tr.Category
            };
        }

        else
        {
            DateTime dt = DateTime.Now;
            _transaction = new Transaction
            {
                Id = (ulong)dt.ToBinary(),
                UserId = UserProfile.Current?.Id ?? ulong.MinValue,
                Timestamp = dt.ToBinary(),
                Type = transactionType,
                Value = decimal.MinValue,
                Description = string.Empty,
                Category = string.Empty
            };
        }

        // provide categories based oon transaction types
        cbType.Items.Clear();
        switch (transactionType)
        {
            // expanse
            case Transaction.TypeExpanse:
                foreach (string sExpanse in _expanses)
                {
                    cbType.Items.Add(sExpanse);
                }
                break;
            
            // income
            case Transaction.TypeIncome:
                foreach (string sIncome in _incomes)
                {
                    cbType.Items.Add(sIncome);
                }
                break;
            
            default: break;
        }
    }

    private string[] _incomes = ["Salary / Wage", "Donate"];
    private string[] _expanses = ["Food", "Clothing", "Housing"];
    
    /// <summary>
    /// Representing the category of the transaction.
    /// </summary>
    private string transactionCategory;
    
    /// <summary>
    /// Representing the transaction type.
    /// </summary>
    private bool transactionType;

    /// <summary>
    /// Determines whether an existing task is being modified.
    /// </summary>
    private bool editMode;

    private bool VerifyTransaction(Transaction transaction)
    {
        if (transaction.Id == ulong.MinValue) return false;         // invalid transaction ID
        if (transaction.UserId == ulong.MaxValue) return false;     // invalid user ID

        return true;
    }

    private Transaction _transaction;
    private Transaction CreateTransaction(Transaction? transactionBase = null)
    {
        Transaction transaction;
        decimal value = decimal.Zero;

        if (decimal.TryParse(txtValue.Text.Trim(), out value) == false)
        {
            value = decimal.Zero;
        }

        value = decimal.Abs(value);

        if (transactionBase == null)
        {
            DateTime dt = DateTime.Now;
            transaction = new Transaction
            {
                Id = (ulong)dt.ToBinary(),
                UserId = UserProfile.Current?.Id ?? ulong.MinValue,
                Timestamp = dt.ToBinary(),
                Type = transactionType,
                Value = value,
                Description = txtDescription.Text.Trim(),
                Category = (string)cbType.SelectedItem
            };

            return transaction;
        }
        
        else
        {
            transaction = new Transaction
            {
                Id = transactionBase.Value.Id,
                UserId = transactionBase.Value.UserId,
                Timestamp = transactionBase.Value.Timestamp,
                Type = transactionBase.Value.Type,
                Value = value,
                Description = txtDescription.Text.Trim(),
                Category = (string)cbType.SelectedItem
            };

            return transaction;
        }
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
        this.Close();
    }

    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
        // check if the transaction is valid
        Transaction transaction = CreateTransaction(_transaction);

        if (VerifyTransaction(transaction) == true)
        {
            // transaction is valid
            // add transaction to the list of user's transactions
            if (UserProfile.Current != null)
            {
                // is edit mode
                if (editMode == true)
                {
                    // need toremove the original transaction first
                    UserProfile.Current.Transactions.Remove(UserProfile.Current.Transactions.Where(x => x.Id == transaction.Id).FirstOrDefault());
                }

                // save the transaction
                UserProfile.Current.Transactions.Add(transaction);

                // resave transactions
                if (UserProfile.Current.SaveTransactions() == false)
                {
                    // unable to save the list of the transactions
                    _ = MessageBox.Show(Messages.TransactionsSavingError, "Saving error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // close the window with successful dialog result state
                this.DialogResult = true;
                this.Close();
            }

            else
            {
                // user is not logged in
                _ = MessageBox.Show(Messages.UserNotLoggedIn, "New transaction", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        else
        {
            // invalid transaction
            _ = MessageBox.Show(Messages.CantCreateTransaction, "New transaction", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #region Static code

    /// <summary>
    /// Attempts to create a new expanse to the currently loaded user.
    /// </summary>
    /// <returns>True, if the transaction was created, otherwise false.</returns>
    public static bool CreateExpanse()
    {
        if (UserProfile.IsProfileLoaded() == false)
        {
            _ = MessageBox.Show(Messages.UserNotLoggedIn, "No profile", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        WndNewTransaction wnd = new WndNewTransaction(Transaction.TypeExpanse);
        return wnd.ShowDialog() == true;
    }

    /// <summary>
    /// Attempts to create a new income to the currently loaded user.
    /// </summary>
    /// <returns>True, if the transaction was created, otherwise false.</returns>
    public static bool CreateIncome()
    {
        if (UserProfile.IsProfileLoaded() == false)
        {
            _ = MessageBox.Show(Messages.UserNotLoggedIn, "No profile", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        WndNewTransaction wnd = new WndNewTransaction(Transaction.TypeIncome, null, false);
        return wnd.ShowDialog() == true;
    }

    /// <summary>
    /// Attempts to modify the existing <paramref name="transaction"/>.
    /// </summary>
    /// <param name="transaction">A transaction to be modified.</param>
    /// <returns></returns>
    public static bool ModifyTransaction(Transaction transaction)
    {
        if (UserProfile.IsProfileLoaded() == false)
        {
            _ = MessageBox.Show(Messages.UserNotLoggedIn, "No profile", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        WndNewTransaction wnd = new WndNewTransaction(transaction.Type, transaction, true);
        wnd.txtDescription.Text = transaction.Description;
        wnd.txtValue.Text = transaction.Value.ToString();
        return wnd.ShowDialog() == true;
    }

    #endregion
}
