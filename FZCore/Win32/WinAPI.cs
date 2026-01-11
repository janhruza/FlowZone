using System.Runtime.InteropServices;

using LPARAM = nint;
using WPARAM = nint;
using DWORD = int;
using HWND = nint;

namespace FZCore.Win32;

internal static class WinAPI
{
    [DllImport("dwmapi")]
    public static extern HRESULT DwmExtendFrameIntoClientArea(HWND hwnd, ref MARGINS margins);

    [DllImport("dwmapi")]
    public static extern HRESULT DwmSetWindowAttribute(HWND hWnd, DWORD dwAttribute, int[] pwAttribute, int cbSize);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool PostMessage(HWND hWnd, uint Msg, WPARAM wParam, LPARAM lParam);
}
