namespace PassFort;

/// <summary>
/// Representing a class with various in-app messages.
/// </summary>
public static class Messages
{
    /// <summary>
    /// Unable to create database file because the input data are invalid.
    /// </summary>
    public const string CantCreateDatabase = "Unable to create database file because the input data are invalid.";

    /// <summary>
    /// Unable to open database file.
    /// </summary>
    public const string CantOpenDbFile = "Unable to open database file.";

    /// <summary>
    /// Unable to validate your input. Please make sure all required fields are properly filled.
    /// </summary>

    public const string CanValidatePassword = "Unable to validate your input. Please make sure all required fields are properly filled.";

    /// <summary>
    /// Unable to continue because no database file was loaded. Please load a valid database file and repeat the operation.
    /// </summary>
    public const string NoDatabaseLoaded = "Unable to continue because no database file was loaded. Please load a valid database file and repeat the operation.";

    /// <summary>
    /// Selected database file was not found. Do you want to remove it from the history?
    /// </summary>
    public const string DbFileNotFound = "Selected database file was not found. Do you want to remove it from the history?";
}
