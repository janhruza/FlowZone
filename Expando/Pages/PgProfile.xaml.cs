using Expando.Core;

using System.Windows;
using System.Windows.Controls;

namespace Expando.Pages;

/// <summary>
/// Representing a user profile page (not a profile selection page).
/// </summary>
public partial class PgMyProfile : Page, IExpandoPage
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgMyProfile"/> class.
    /// </summary>
    public PgMyProfile()
    {
        InitializeComponent();

        // load info on load
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
        if (UserProfile.IsProfileLoaded() == false)
        {
            // no profile loaded
            // can't show any info
            Page pg = PgNoProfile.Instance;
            _ = MainWindow.SetActivePage(ref pg);
            return;
        }

        // show profile info
        this.rUsername.Text = UserProfile.Current?.Username.ToUpper();
        this.rCreation.Text = $"{UserProfile.Current?.CreationDate.ToShortDateString()} {UserProfile.Current?.CreationDate.ToShortTimeString()}";
        this.rId.Text = UserProfile.Current?.Id.ToString();

        return;
    }

    private void ClearTransactionsList()
    {
        if (UserProfile.IsProfileLoaded() == false)
        {
            return;
        }

        if (MessageBox.Show(Messages.ConfirmDeleteTransactions, "Delete all transactions", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            // clear transactions list
            UserProfile.Current?.Transactions.Clear();
            if (UserProfile.Current?.SaveTransactions() == true)
            {
                _ = MessageBox.Show(Messages.TransactionsDeleted, "Transactions deleted.", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            else
            {
                // unable to save the list of transactions
                _ = MessageBox.Show(Messages.TransactionsSavingError, "Updating error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        return;
    }

    private void DeleteUserProfile()
    {
        if (UserProfile.Current == null)
        {
            return;
        }

        if (MessageBox.Show(Messages.ConfirmDeleteAccount, "Delete your profile", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            // delete user profile

            if (UserProfile.Profiles.Remove(UserProfile.Current) == true)
            {
                // remove the currently loaded profile
                UserProfile.Current = null;

                if (UserProfile.RebuildUserIndexFile() == true)
                {
                    // navigate to the home page
                    _ = UserProfile.LoadUsersData();
                    _ = MainWindow.SetHomePage();
                }

                else
                {
                    // can't rebuild user index file
                    _ = MessageBox.Show(Messages.CantRebuildIndexFile, "Can't rebuild index file", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            else
            {
                // can't remove profile from the loaded profiles list
                _ = MessageBox.Show(Messages.CantRemoveUser, "Can't remove profile.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        return;
    }

    private void btnClearTransactions_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        ClearTransactionsList();
    }

    private void btnDeleteProfile_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        DeleteUserProfile();
    }

    #region Static code

    private static PgMyProfile? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="PgMyProfile"/> class.
    /// </summary>
    public static PgMyProfile Instance
    {
        get
        {
            _instance ??= new PgMyProfile();
            return _instance;
        }
    }

    #endregion
}
