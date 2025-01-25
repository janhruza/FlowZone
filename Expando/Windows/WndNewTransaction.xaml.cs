using System;
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
    public WndNewTransaction(bool transactionType)
    {
        InitializeComponent();
        this.transactionType = transactionType;
    }

    private bool transactionType;

    private bool VerifyTransaction(Transaction transaction)
    {
        if (transaction.Id == ulong.MinValue) return false;         // invalid transaction ID
        if (transaction.UserId == ulong.MaxValue) return false;     // invalid user ID

        return true;
    }

    private Transaction CreateTransaction()
    {
        DateTime dt = DateTime.Now;
        decimal value = decimal.Zero;

        if (decimal.TryParse(txtValue.Text.Trim(), out value) == false)
        {
            value = decimal.Zero;
        }

        Transaction transaction = new Transaction
        {
            Id = (ulong)dt.ToBinary(),
            UserId = UserProfile.Current?.Id ?? ulong.MinValue,
            Timestamp = dt.ToBinary(),
            Type = transactionType,
            Value = value,
            Description = txtDescription.Text.Trim()
        };

        return transaction;
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
        this.Close();
    }

    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
        // check if the transaction is valid
        Transaction transaction = CreateTransaction();

        if (VerifyTransaction(transaction) == true)
        {
            // transaction is valid
            // add transaction to the list of user's transactions
            if (UserProfile.Current != null)
            {
                // save the transaction
                UserProfile.Current.Transactions.Add(transaction);

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

        WndNewTransaction wnd = new WndNewTransaction(Transaction.TypeIncome);
        return wnd.ShowDialog() == true;
    }

    #endregion
}
