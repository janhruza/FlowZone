using FZCore.Extensions;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace FZCore.Windows;

/// <summary>
/// Representing an extension class to any <see cref="Window"/> class.
/// </summary>
public class WindowExtender
{
    private const int WM_SYSCOMMAND = 0x0112;
    private const int MF_STRING = 0x00000000;
    private const int MF_SEPARATOR = 0x00000800;
    private const int SC_RESTORE = 0xF120; // Example: Standard system command
    private const uint SC_CLOSE = 0xF060;
    private const uint MF_BYCOMMAND = 0x00000000;

    private Window _window;
    private IntPtr _hwnd;
    private IntPtr _originalSystemMenu;
    private Dictionary<int, ExtendedMenuItem> _customMenuItems;
    private bool _consoleAttached;

    /// <summary>
    /// Creates a new instance of the <see cref="WindowExtender"/> class.
    /// </summary>
    /// <param name="window">Target window to be extended.</param>
    /// <exception cref="ArgumentNullException">Target widow was not fund.</exception>
    public WindowExtender(Window window)
    {
        if (window == null) throw new ArgumentNullException(nameof(window));

        _window = window;
        _hwnd = new WindowInteropHelper(window).EnsureHandle();
        _customMenuItems = new Dictionary<int, ExtendedMenuItem>();
        _consoleAttached = false;

        // Hook WndProc
        var source = HwndSource.FromHwnd(_hwnd);
        source.AddHook(WndProc);

        // Save the original system menu
        _originalSystemMenu = GetSystemMenu(_hwnd, false);
    }

    /// <summary>
    /// Toggles command line window.
    /// </summary>
    public void ToggleConsole()
    {
        if (_consoleAttached == true)
        {
            FreeConsole();
            _consoleAttached = false;
        }

        else
        {
            // show console window
            AllocConsole();

            // get console window handle
            nint hConsole = GetConsoleWindow();

            // get console menu handle
            nint hMenu = GetSystemMenu(hConsole, false);

            // delete close menu item (disables the close button)
            DeleteMenu(hMenu, SC_CLOSE, MF_BYCOMMAND);

            Console.Title = $"{Process.GetProcessById(Environment.ProcessId).ProcessName} Debug Console";
            _consoleAttached = true;
        }
    }

    /// <summary>
    /// Adds new menu item to the end of the sequence.
    /// </summary>
    /// <param name="id">Menu item ID.</param>
    /// <param name="header">enu item header text.</param>
    /// <param name="onClick">On-click action for the menu item.</param>
    public void AddMenuItem(int id, string header, Action onClick)
    {
        _customMenuItems[id] = new ExtendedMenuItem { Header = header, OnClick = onClick };
        RedrawMenu();
    }

    /// <summary>
    /// Adds new separator to the given position within the list of custom items.
    /// </summary>
    /// <param name="id"></param>
    public void AddSeparator(int id)
    {
        _customMenuItems[id] = new ExtendedMenuItem
        {
            Header = "-"
        };

        RedrawMenu();
    }

    /// <summary>
    /// Adds new menu item to the end of the sequence.
    /// </summary>
    /// <param name="id">Menu item ID.</param>
    /// <param name="menuItem">Menu item struct.</param>
    public void AddMenuItem(int id, ExtendedMenuItem menuItem)
    {
        _customMenuItems[id] = menuItem;
        RedrawMenu();
    }

    /// <summary>
    /// Removes the menu item with the given <paramref name="id"/> from the custom menu items list.
    /// </summary>
    /// <param name="id">ID of the target item.</param>
    public void RemoveMenuItem(int id)
    {
        if (_customMenuItems.ContainsKey(id))
        {
            _customMenuItems.Remove(id);
            RedrawMenu();
        }
    }

    /// <summary>
    /// Restores the default system menu.
    /// </summary>
    public void RestoreDefaultMenu()
    {
        GetSystemMenu(_hwnd, true); // Restore the original menu
        _customMenuItems.Clear();
    }

    /// <summary>
    /// Enables extended functionality on the associated window.
    /// </summary>
    public void EmpowerWindow()
    {
        if (_window == null) return;

        // enable keydown events
        _window.KeyDown += (s, e) =>
        {
            if (e.Key == Key.F1)
            {
                // open app log
                Core.ViewLog();
            }

            if (e.Key == System.Windows.Input.Key.F11)
            {
                // empower window
                _window.ToggleFullScreenMode();
            }

            if (e.Key == System.Windows.Input.Key.F12)
            {
                // toggle console
                ToggleConsole();
            }
        };
    }

    private void RedrawMenu()
    {
        // Clear the current menu and restore the default system menu
        GetSystemMenu(_hwnd, true);

        IntPtr systemMenu = GetSystemMenu(_hwnd, false);

        // Insert custom items at their respective positions based on IDs
        foreach (var menuItem in _customMenuItems.OrderBy(m => m.Key))
        {
            if (string.IsNullOrEmpty(menuItem.Value.Header) || menuItem.Value.Header == "-")
            {
                InsertMenu(systemMenu, menuItem.Key, MF_SEPARATOR, 0, string.Empty);
            }

            else
            {
                InsertMenu(systemMenu, menuItem.Key, MF_STRING, menuItem.Key, menuItem.Value.Header);
            }
        }
    }


    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == WM_SYSCOMMAND)
        {
            int commandId = wParam.ToInt32();
            if (_customMenuItems.TryGetValue(commandId, out var menuItem))
            {
                menuItem.OnClick?.Invoke();
                handled = true;
            }
        }

        return IntPtr.Zero;
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern bool InsertMenu(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, string lpNewItem);

    [DllImport("uxtheme.dll", EntryPoint = "#135", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern int SetPreferredAppMode(int preferredAppMode);

    [DllImport("uxtheme.dll", EntryPoint = "#136", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern void FlushMenuThemes();

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool AllocConsole();

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool FreeConsole();

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool DeleteMenu(IntPtr hMenu, uint uPosition, uint uFlags);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetConsoleWindow();

    /// <summary>
    /// Draws the system menu using the dark theme.
    /// </summary>
    public static void EnableDarkMode()
    {
        // Set the preferred app mode to dark
        _ = SetPreferredAppMode(2); // 2 corresponds to "AllowDark" mode
        FlushMenuThemes(); // Apply the theme changes
    }

    /// <summary>
    /// Draws the system menu using the light (default) theme.
    /// </summary>
    public static void EnableLightMode()
    {
        _ = SetPreferredAppMode(4); // force light mode
        FlushMenuThemes();
    }

    /// <summary>
    /// Draws the system menu with the system theme.
    /// </summary>
    public static void EnableSystemMode()
    {
        _ = SetPreferredAppMode(0); // use system default (light)
        FlushMenuThemes();
    }
}