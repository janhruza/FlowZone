using System.Windows;
using System.Windows.Interop;

using PVOID = System.IntPtr;

namespace FZCore.Extentions;

/// <summary>
/// Representing various <see cref="Window"/> class extentions.
/// </summary>
public static class WindowExtentions
{
    /// <summary>
    /// Gets the <paramref name="window"/>'s handle.
    /// </summary>
    /// <param name="window">Target window.</param>
    /// <returns>The handle (HWND) to the <paramref name="window"/>.</returns>
    public static PVOID GetHandle(this Window window)
    {
        return new WindowInteropHelper(window).EnsureHandle();
    }

    /// <summary>
    /// Shows a new message window with the specified <paramref name="message"/>. It will use <paramref name="window"/>'s title as the caption text.
    /// </summary>
    /// <param name="window">Target window.</param>
    /// <param name="message">Message text.</param>
    public static void ShowMessage(this Window window, string message)
    {
        _ = MessageBox.Show(message, window.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        return;
    }

    /// <summary>
    /// Gets the handle (HWND) of the parent window.
    /// </summary>
    /// <param name="window">Target window.</param>
    /// <returns>Handle to the parent window. If there is no parent,
    /// the return value is <see cref="PVOID.Zero"/> and if the parent is not a window,
    /// the <see cref="PVOID.MinValue"/> will be returned.</returns>
    public static PVOID GetParent(this Window window)
    {
        if (window.Parent == null)
        {
            return PVOID.Zero;
        }

        if (window.Parent.GetType() != typeof(Window))
        {
            return PVOID.MinValue;
        }

        return GetHandle((Window)window.Parent);
    }
}
