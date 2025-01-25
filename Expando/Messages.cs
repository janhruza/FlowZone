namespace Expando;

/// <summary>
/// Representing all in-app messages. It's a data class.
/// </summary>
public static class Messages
{
    /// <summary>
    /// Representing the main window title.
    /// </summary>
    public const string AppTitle = "Expando";

    /// <summary>
    /// Unable to load user profiles.
    /// </summary>
    public const string CantLoadUserData = "Unable to load user profiles.";

    /// <summary>
    /// Unable to create a new profile because the input data are invalid.
    /// </summary>
    public const string InvalidProfileData = "Unable to create a new profile because the input data are invalid.";

    /// <summary>
    /// Unable to create a new profile because of an internal error.
    /// </summary>
    public const string CantCreateProfile = "Unable to create a new profile because of an internal error.";

    /// <summary>
    /// No description.
    /// </summary>
    public const string NoDescription = "No description.";

    /// <summary>
    /// Unable to create a new transaction. Make sure the you are using a valid user profile.
    /// </summary>
    public const string CantCreateTransaction = "Unable to create a new transaction. Make sure the you are using a valid user profile.";

    /// <summary>
    /// No user profile was loaded. Please visit the 'Profiles' page in order to load a user profile.
    /// </summary>

    public const string UserNotLoggedIn = "No user profile was loaded. Please visit the 'Profiles' page in order to load a user profile.";

    /// <summary>
    /// No user profile loaded. You can load your profile on the 'Profiles' page.
    /// </summary>
    public const string NoProfileLoaded = "No user profile loaded. You can load your profile on the 'Profiles' page.";

    /// <summary>
    /// Unable to save your transactions. This is an internal error.
    /// </summary>
    public const string TransactionsSavingError = "Unable to save your transactions. This is an internal error.";

    /// <summary>
    /// Do you want to delete all your transactions? This action is irreversible.
    /// </summary>

    public const string ConfirmDeleteTransactions = "Do you want to delete all your transactions? This action is irreversible.";

    /// <summary>
    /// All your transactions were deleted.
    /// </summary>
    public const string TransactionsDeleted = "All your transactions were deleted.";

    /// <summary>
    /// Do you want to delete your profile? This action is irreversible and permanent.
    /// </summary>

    public const string ConfirmDeleteAccount = "Do you want to delete your profile? This action is irreversible and permanent.";

    /// <summary>
    /// Unable to create the user index file. This is an internal error.
    /// </summary>
    public const string CantRebuildIndexFile = "Unable to create the user index file. This is an internal error.";

    /// <summary>
    /// Unable to remove the user from the loaded users list.
    /// </summary>
    public const string CantRemoveUser = "Unable to remove the user from the loaded users list.";
}
