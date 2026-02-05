namespace FZCore.Windows.Dialogs.Types;

/// <summary>
/// Representing the return value of any custom dialog window that is inteded as a message dialog.
/// </summary>
public enum TDReturn
{
    /// <summary>
    /// Function failed.
    /// </summary>
    ERROR = 0,

    /// <summary>
    /// Cancel button was pressed or Alt+F4 was pressed.
    /// </summary>
    IDCANCEL = 1,

    /// <summary>
    /// No button was pressed.
    /// </summary>
    IDNO = 2,

    /// <summary>
    /// OK button was pressed.
    /// </summary>
    IDOK = 3,

    /// <summary>
    /// Retry button was pressed.
    /// </summary>
    IDRETRY = 4,

    /// <summary>
    /// Yes button was pressed.
    /// </summary>
    IDYES = 5,

    /// <summary>
    /// Close button was pressed.
    /// </summary>
    IDCLOSE = 6
}