using FZCore.Controls;
using FZCore.Windows;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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
            Title = "My Awesome Window"
        };

        FZCore.Core.SetWindowTheme(fw, FZCore.FZThemeMode.System);

        Grid g = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition()
            }
        };

        fw.Content = g;

        CtlCaptionBar ctlCaption = new CtlCaptionBar
        {
            Target = fw,
            Background = fw.Background
        };

        g.Children.Add(ctlCaption);
        Grid.SetRow(ctlCaption, 0);

        fw.ShowDialog();
        return 0;
    }
}
