using RipTide.Windows;

namespace RipTide.Core;

/// <summary>
/// Representing the valid icons for the custom dialog window (<see cref="WndDialog"/>).
/// </summary>
public enum DialogIcon : byte
{
    /// <summary>
    /// Representing the information sign.
    /// </summary>
    Information = 0,

    /// <summary>
    /// Representing the error sign.
    /// </summary>
    Error       = 1,

    /// <summary>
    /// Representing the warning sign.
    /// </summary>
    Warning     = 2,

    /// <summary>
    /// Representing the success sign.
    /// </summary>
    Success     = 3
}
