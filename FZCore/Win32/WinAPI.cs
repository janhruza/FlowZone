using System.Runtime.InteropServices;

using DWORD = int;
using HWND = nint;
using LPARAM = nint;
using WPARAM = nint;

namespace FZCore.Win32;

/// <summary>
/// Exposes some of the the Windows API (WinAPI) functions used in FlowZone applications.
/// </summary>
public static class WinAPI
{
    [DllImport("dwmapi")]
    internal static extern HRESULT DwmExtendFrameIntoClientArea(HWND hwnd, ref MARGINS margins);

    [DllImport("dwmapi")]
    internal static extern HRESULT DwmSetWindowAttribute(HWND hWnd, DWORD dwAttribute, int[] pwAttribute, int cbSize);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool PostMessage(HWND hWnd, uint Msg, WPARAM wParam, LPARAM lParam);


    /// <summary>
    /// Allocates a new console for the calling process.
    /// </summary>
    /// <returns>true if the operation succeeds; otherwise false.</returns>
    [DllImport("kernel32", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool AllocConsole();

    /// <summary>
    /// Detaches the calling process from its console, if one is attached.
    /// </summary>
    /// <remarks>If the process is not attached to a console, this function has no effect. To obtain extended
    /// error information, call GetLastError after a failure.</remarks>
    /// <returns>true if the operation succeeds; otherwise, false.</returns>
    [DllImport("kernel32", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool FreeConsole();
}
