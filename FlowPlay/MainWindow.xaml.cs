using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace FlowPlay;

/// <summary>
/// Representing the main application window.
/// </summary>
public partial class MainWindow : Window
{
    // Importing WinAPI methods
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr CreatePopupMenu();

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern bool InsertMenu(IntPtr hMenu, uint uPosition, uint uFlags, uint uIDNewItem, string lpNewItem);

    [DllImport("user32.dll")]
    private static extern bool TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y, int nReserved, IntPtr hWnd, IntPtr prcRect);

    [DllImport("user32.dll")]
    private static extern int DestroyMenu(IntPtr hMenu);

    // Constants for menu flags
    private const uint MF_STRING = 0x00000000;
    private const uint TPM_LEFTALIGN = 0x0000;

    /// <summary>
    /// Creates a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();

        this.KeyDown += (s, e) =>
        {
            if (e.Key == System.Windows.Input.Key.Apps)
            {
                SystemCommands.ShowSystemMenu(this, this.PointToScreen(new Point(0, 0)));
            }
        };

        this.MouseRightButtonDown += (s, e) =>
        {
            CreateContextMenu();
        };
    }

    private void CreateContextMenu()
    {
        IntPtr hWnd = new WindowInteropHelper(this).Handle;
        IntPtr hMenu = CreatePopupMenu();
        Point pMouse = Mouse.GetPosition(this);
        Point pReal = PointToScreen(pMouse);

        InsertMenu(hMenu, TPM_LEFTALIGN, MF_STRING, 0x10, "Item #1\tF10");
        InsertMenu(hMenu, TPM_LEFTALIGN, MF_STRING, 0x11, "Item #2\tF11");
        _ = TrackPopupMenu(hMenu, TPM_LEFTALIGN, (int)pReal.X, (int)pReal.Y, 0, hWnd, IntPtr.Zero);
        _ = DestroyMenu(hMenu);
    }
}