using FZCore.Win32;

using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace FZCore.Windows;

/// <summary>
/// Representing a basic WPF Window without an icon.
/// </summary>
/// <remarks>
/// This class inplements the Win32 API to remove the icon from the window's title bar.
/// </remarks>
public class IconlessWindow : BaseWindow
{
    const int GWL_EXSTYLE = -20;
    const int WS_EX_DLGMODALFRAME = 0x0001;
    const int SWP_NOSIZE = 0x0001;
    const int SWP_NOMOVE = 0x0002;
    const int SWP_NOZORDER = 0x0004;
    const int SWP_FRAMECHANGED = 0x0020;

    [DllImport("user32.dll")]
    static extern int GetWindowLong(IntPtr hwnd, int index);

    [DllImport("user32.dll")]
    static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
        int x, int y, int cx, int cy, uint flags);

    private void RemoveDialogFrame()
    {
        var hwnd = new WindowInteropHelper(this).Handle;
        int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
        SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME);

        // Force non-client area to update
        SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0,
            SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
    }

    private void ExtendFrameIntoClient()
    {
        MARGINS margins = new MARGINS
        {
            cxLeftWidth = -1,
            cxRightWidth = -1,
            cyTopHeight = -1,
            cyBottomHeight = -1
        };

        _ = WinAPI.DwmExtendFrameIntoClientArea(Handle, ref margins);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="IconlessWindow"/> class.
    /// </summary>
    public IconlessWindow()
    {
        SnapsToDevicePixels = true;
        UseLayoutRounding = true;

        SourceUpdated += (s, e) =>
        {
            // Ensure the window is redrawn correctly when the source is updated
            RemoveDialogFrame();
        };
    }

    /// <summary>
    /// Handles additional initialization when the window's underlying source is created.
    /// </summary>
    /// <remarks>This override removes the window icon after the native window source has been initialized.
    /// Call the base method to ensure standard initialization occurs.</remarks>
    /// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
    protected override void OnSourceInitialized(System.EventArgs e)
    {
        base.OnSourceInitialized(e);

        // remove the icon from the window using the Win32 API
        RemoveDialogFrame();
        ExtendFrameIntoClient();
    }
}
