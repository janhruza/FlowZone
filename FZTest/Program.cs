using FZCore.Controls;
using FZCore.Windows;

using System;

namespace FZTest;

internal class Program
{
    [STAThread]
    private static int Main(string[] args)
    {
        CtlTextInput ctlInput = new CtlTextInput
        {
            Margin = new System.Windows.Thickness(10),
            Width = 220
        };

        BaseWindow wnd = new BaseWindow
        {
            ResizeMode = System.Windows.ResizeMode.NoResize,
            Title = "Control Test",
            SizeToContent = System.Windows.SizeToContent.WidthAndHeight,
            Content = ctlInput,
            MinWidth = ctlInput.RenderSize.Width,
            MinHeight = ctlInput.RenderSize.Height
        };

        wnd.MouseLeftButtonDown += (s, e) => wnd.DragMove();

        _ = wnd.ShowDialog();
        return 0;
    }
}
