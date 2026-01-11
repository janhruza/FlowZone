using System.Runtime.InteropServices;

namespace FZCore.WinAPI;

internal static class Win32
{
    [DllImport("dwmapi")]
    public static extern int DwmExtendFrameIntoClientArea(nint hwnd, ref MARGINS margins);
}
