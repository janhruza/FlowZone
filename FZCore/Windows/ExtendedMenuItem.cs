using System;

namespace FZCore.Windows;

/// <summary>
/// Representing a single <see cref="ExtendedMenuItem"/> structure.
/// </summary>
public struct ExtendedMenuItem
{
    /// <summary>
    /// Representing the header text of the menu item.
    /// </summary>
    public string Header { get; set; }

    /// <summary>
    /// Representing the action executed when the menu item is clicked.
    /// </summary>
    public Action OnClick { get; set; }
}
