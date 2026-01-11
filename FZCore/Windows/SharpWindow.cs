using FZCore.Win32;

using System.Runtime.InteropServices;

namespace FZCore.Windows;

/// <summary>
/// Representing a siimple window based on <see cref="IconlessWindow"/> which also has rounded corners disabled.
/// </summary>
public class SharpWindow : IconlessWindow
{
    /// <summary>
    /// Handles additional window initialization when the window's underlying Win32 source is created.
    /// </summary>
    /// <remarks>This method is called after the window's handle has been created. It can be overridden to
    /// perform custom initialization that requires access to the window handle. When overriding, always call the base
    /// implementation to ensure proper initialization.</remarks>
    /// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
    protected override void OnSourceInitialized(System.EventArgs e)
    {
        base.OnSourceInitialized(e);
        WinAPI.DwmSetWindowAttribute(this.Handle, (int)DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, [1], Marshal.SizeOf<int>());
    }
}
