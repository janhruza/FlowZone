using FZCore.Win32;

using System;
using System.Runtime.InteropServices;

namespace FZCore.Windows;

/// <summary>
/// Represents a window without an icon that applies a transient system backdrop effect when initialized.
/// </summary>
/// <remarks>Use this class to create windows that do not display an icon and feature a transient backdrop effect,
/// typically for temporary or overlay UI elements. The window automatically configures the system backdrop when its
/// source is initialized.</remarks>
public class TransientWindow : IconlessWindow
{
    /// <summary>
    /// Handles additional initialization when the window's underlying Win32 source is created.
    /// </summary>
    /// <remarks>This method is called when the window's handle has been created and is available for interop
    /// operations. Override this method to perform actions that require access to the window handle.</remarks>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        WinAPI.DwmSetWindowAttribute(Handle, (int)DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE, [3], Marshal.SizeOf<int>());
    }
}
