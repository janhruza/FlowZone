using FZCore.Windows;

using System;
using System.Windows.Controls;

namespace FZTest;

internal class Program
{
    [STAThread]
    private static int Main(string[] args)
    {
        Console.Title = "FlowZone Test Utility";

        const int MIN_WIDTH = 250;
        const int MIN_HEIGHT = 100;

        MenuItem miClose = new MenuItem
        {
            Header = "Close",
            InputGestureText = "Alt+F4"
        };

        ContextMenu cm = new ContextMenu
        {
            Items =
            {
                miClose
            }
        };

        BaseWindow wnd = new FreeWindow
        {
            Title = Console.Title,
            MinWidth = MIN_WIDTH,
            MinHeight = MIN_HEIGHT,
            Width = MIN_WIDTH,
            Height = MIN_HEIGHT,
            ContextMenu = cm,
            ThemeMode = FZCore.FZThemeMode.System
        };

        miClose.Click += (s, e) => wnd.Close();

        wnd.MouseLeftButtonDown += (s, e) => wnd.DragMove();
        _ = wnd.ShowDialog();

        return 0;
    }
}
