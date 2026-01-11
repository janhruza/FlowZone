using System.Runtime.InteropServices;

namespace FZCore.Win32;

internal static class WinAPI
{
    public const int WM_DESTROY = 0x0002;

    [DllImport("dwmapi")]
    public static extern HRESULT DwmExtendFrameIntoClientArea(nint hwnd, ref MARGINS margins);

    [DllImport("dwmapi")]
    public static extern HRESULT DwmSetWindowAttribute(nint hWnd, int dwAttribute, int[] pwAttribute, int cbSize);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool PostMessage(nint hWnd, uint Msg, nint wParam, nint lParam);
}
