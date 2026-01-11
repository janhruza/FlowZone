using System.Runtime.InteropServices;

namespace FZCore.Win32;

internal static class WinAPI
{
    [DllImport("dwmapi")]
    public static extern int DwmExtendFrameIntoClientArea(nint hwnd, ref MARGINS margins);
}
