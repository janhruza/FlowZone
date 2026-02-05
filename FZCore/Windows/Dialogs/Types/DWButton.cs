using System;

namespace FZCore.Windows.Dialogs.Types;

/// <summary>
/// Representing valid custom dialog class buttons.
/// </summary>
[Flags]
public enum DWButton
{
    /// <summary>
    /// Representing tje OK button.
    /// </summary>
    OK = 0,

    /// <summary>
    /// Representing the YES button.
    /// </summary>
    YES = 1,

    /// <summary>
    /// Representing the NO button.
    /// </summary>
    NO = 2,

    /// <summary>
    /// Representing the RETRY button.
    /// </summary>
    RETRY = 4,

    /// <summary>
    /// Representing the CANCEL button.
    /// </summary>
    CANCEL = 8,

    /// <summary>
    /// Representing the CLOSE button.
    /// </summary>
    CLOSE = 16
}
