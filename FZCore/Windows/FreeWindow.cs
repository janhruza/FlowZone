using System.Windows;
using System.Windows.Shell;

namespace FZCore.Windows;

/// <summary>
/// Representing a custom window without the default frame.
/// </summary>
public class FreeWindow : BaseWindow
{
    /// <summary>
    /// Creates a new <see cref="FreeWindow"/> instance.
    /// </summary>
    public FreeWindow()
    {
        MinWidth = 350;
        MinHeight = 350;

        WindowChrome wc = new WindowChrome
        {
            GlassFrameThickness = new System.Windows.Thickness(1),
            ResizeBorderThickness = new System.Windows.Thickness(5),
            CornerRadius = new CornerRadius(15),
            CaptionHeight = 0
        };

        WindowChrome.SetWindowChrome(this, wc);

        StateChanged += FreeWindow_StateChanged;
    }

    private void FreeWindow_StateChanged(object? sender, System.EventArgs e)
    {
        if (WindowState == WindowState.Maximized)
        {

            var thickness = SystemParameters.WindowResizeBorderThickness;
            var fixedFrame = SystemParameters.FixedFrameHorizontalBorderHeight;
            double overhead = thickness.Top + fixedFrame + 1;
            BorderThickness = new Thickness(overhead);

            //this.Padding = new Thickness(
            //SystemParameters.WindowResizeBorderThickness.Left,
            //SystemParameters.WindowResizeBorderThickness.Top,
            //SystemParameters.WindowResizeBorderThickness.Right,
            //SystemParameters.WindowResizeBorderThickness.Bottom);
        }
        else
        {
            Padding = new Thickness(0);
            BorderThickness = new Thickness(0);
        }
    }
}
