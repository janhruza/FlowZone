using FZCore.Controls;
using FZCore.Windows;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FZTest;

internal class Program
{
    [STAThread]
    static int Main(string[] args)
    {
        FreeWindow fw = new FreeWindow
        {
            Width = 800,
            Height = 500,
            Title = "My Awesome Window",
            Background = Brushes.Black,
            Foreground = Brushes.White
        };

        fw.MouseLeftButtonDown += (s, e) => fw.DragMove();

        FZCore.Core.SetWindowTheme(fw, FZCore.FZThemeMode.None);

        Grid g = new Grid();
        fw.Content = g;

        CtlCaptionButtons ctlButtons = new CtlCaptionButtons
        {
            Target = fw,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        };

        g.Children.Add(ctlButtons);
        fw.UpdateLayout();
        fw.ShowDialog();
        return 0;
    }
}
