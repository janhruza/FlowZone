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
}
