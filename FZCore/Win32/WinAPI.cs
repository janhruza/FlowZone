using System.Runtime.InteropServices;

namespace FZCore.Win32;

internal static class WinAPI
{
    [DllImport("dwmapi")]
    public static extern HRESULT DwmExtendFrameIntoClientArea(nint hwnd, ref MARGINS margins);

    [DllImport("dwmapi")]
    public static extern HRESULT DwmSetWindowAttribute(nint hWnd, int dwAttribute, int[] pwAttribute, int cbSize);
}
