using FZCore.Win32;

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Shell;

namespace FZCore.Windows;

/// <summary>
/// Representing a custom window without the default frame.
/// </summary>
public class FreeWindow : BaseWindow
{
    /// <summary>
    /// Creates a new <see cref="FreeWindow"/> instance.
    /// </summary>
    public FreeWindow()
    {
        MinWidth = 350;
        MinHeight = 350;

        WindowChrome wc = new WindowChrome
        {
            GlassFrameThickness = new Thickness(-1),
            ResizeBorderThickness = new Thickness(5),
            CornerRadius = new CornerRadius(15),
            CaptionHeight = 0
        };

        WindowChrome.SetWindowChrome(this, wc);
        StateChanged += FreeWindow_StateChanged;
        SourceInitialized += FreeWindow_SourceInitialized;
    }

    private void FreeWindow_SourceInitialized(object? sender, EventArgs e)
    {
        nint hwnd = new WindowInteropHelper(this).EnsureHandle();
        WinAPI.DwmSetWindowAttribute(Handle, (int)DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE, [2], Marshal.SizeOf<int>());
        return;
    }

    private void FreeWindow_StateChanged(object? sender, EventArgs e)
    {
        if (WindowState == WindowState.Maximized)
        {

            var thickness = SystemParameters.WindowResizeBorderThickness;
            var fixedFrame = SystemParameters.FixedFrameHorizontalBorderHeight;
            double overhead = thickness.Top + fixedFrame + 1;
            BorderThickness = new Thickness(overhead);

            //this.Padding = new Thickness(
            //SystemParameters.WindowResizeBorderThickness.Left,
            //SystemParameters.WindowResizeBorderThickness.Top,
            //SystemParameters.WindowResizeBorderThickness.Right,
            //SystemParameters.WindowResizeBorderThickness.Bottom);
        }
        else
        {
            Padding = new Thickness(0);
            BorderThickness = new Thickness(0);
        }
    }
}
