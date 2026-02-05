using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

using PVOID = nint;

namespace FZCore.Extensions;

/// <summary>
/// Representing various <see cref="Window"/> class extentions.
/// </summary>
public static class WindowExtensions
{
    #region P/Invoke

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool FlashWindow([In] PVOID wnd, [In] bool bInvert);

    #endregion

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

        return ((Window)window.Parent).GetHandle();
    }

    /// <summary>
    /// Flashes the specified window one time. It does not change the active state of the window.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>
    /// The return value specifies the window's state before the call to the FlashWindow function.
    /// If the window caption was drawn as active before the call,
    /// the return value is nonzero. Otherwise, the return value is zero.
    /// </returns>
    public static bool Flash(this Window window)
    {
        PVOID hWnd = GetHandle(window);
        return FlashWindow(hWnd, false);
    }

    /// <summary>
    /// Toggles fullscreen mode on the selected <paramref name="window"/>.
    /// </summary>
    /// <param name="window">Window to toggle fullscreen mode.</param>
    public static void ToggleFullScreenMode(this Window window)
    {
        bool isEnabled = (window.WindowState == WindowState.Maximized && window.WindowStyle == WindowStyle.None);
        if (isEnabled == true)
        {
            // restore window
            window.WindowStyle = WindowStyle.SingleBorderWindow;
            window.WindowState = WindowState.Normal;
        }

        else
        {
            window.WindowStyle = WindowStyle.None;
            window.WindowState = WindowState.Maximized;
        }

        return;
    }
}
