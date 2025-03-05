using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace FZCore.Windows;

/// <summary>
/// Representing the return value of the <see cref="DialogWindow.Show()"/> function.
/// This value is the result of the <see cref="DialogWindow"/>.
/// </summary>
public enum TDReturn
{
    /// <summary>
    /// Function failed.
    /// </summary>
    ERROR   = 0,

    /// <summary>
    /// Cancel button was pressed or Alt+F4 was pressed.
    /// </summary>
    IDCANCEL    = 1,

    /// <summary>
    /// No button was pressed.
    /// </summary>
    IDNO        = 2,

    /// <summary>
    /// OK button was pressed.
    /// </summary>
    IDOK        = 3,

    /// <summary>
    /// Retry button was pressed.
    /// </summary>
    IDRETRY     = 4,

    /// <summary>
    /// Yes button was pressed.
    /// </summary>
    IDYES       = 5,

    /// <summary>
    /// Close button was pressed.
    /// </summary>
    IDCLOSE     = 6
}

/// <summary>
/// Representing valid <see cref="DialogWindow"/> images.
/// </summary>
public enum DWImage
{
    /// <summary>
    /// Representing the error image.
    /// </summary>
    ERROR,

    /// <summary>
    /// Representing the informational image.
    /// </summary>
    INFO,

    /// <summary>
    /// Representing the shield image.
    /// </summary>
    SHIELD,

    /// <summary>
    /// Representing the warning image.
    /// </summary>
    WARNING
}

/// <summary>
/// Representing valid <see cref="DialogWindow"/> buttons.
/// </summary>
[Flags]
public enum DWButton
{
    /// <summary>
    /// 
    /// </summary>
    OK = 0,

    /// <summary>
    /// 
    /// </summary>
    YES = 1,

    /// <summary>
    /// 
    /// </summary>
    NO = 2,

    /// <summary>
    /// 
    /// </summary>
    RETRY = 4,

    /// <summary>
    /// 
    /// </summary>
    CANCEL = 8,

    /// <summary>
    /// 
    /// </summary>
    CLOSE = 16
}

/// <summary>
/// Representing a custom dialog window.
/// <see cref="DialogWindow"/> is a more extended version of the standard <see cref="MessageBox"/> class.
/// In essence, <see cref="DialogWindow"/> is a kind of a TaskDialog, a Windows API modern version of <see cref="MessageBox"/>.
/// </summary>
public class DialogWindow
{
    /// <summary>
    /// Creates a new instance of the <see cref="DialogWindow"/> class.
    /// </summary>
    public DialogWindow()
    {
        this.Title = string.Empty;
        this.Caption = string.Empty;
        this.Message = string.Empty;
        this.Image = DWImage.INFO;
        this.Buttons = DWButton.OK;
    }

    #region Private members and methods

    private TDReturn _result = TDReturn.ERROR;

    private static BitmapImage GetImage(DWImage image)
    {
        switch (image)
        {
            case DWImage.ERROR:
                return Core.GetImageSource("close.png");

            default:
            case DWImage.INFO:
                return Core.GetImageSource("information.png");

            case DWImage.SHIELD:
                return Core.GetImageSource("shield.png");

            case DWImage.WARNING:
                return Core.GetImageSource("warning.png");
        }
    }

    private static string GetButtonText(DWButton btn)
    {
        switch (btn)
        {
            default:
            case DWButton.OK:
                return "OK";

            case DWButton.YES:
                return "Yes";

            case DWButton.NO:
                return "No";

            case DWButton.RETRY:
                return "Retry";

            case DWButton.CANCEL:
                return "Cancel";

            case DWButton.CLOSE:
                return "Close";
        }
    }

    private static TDReturn GetResult(DWButton btn)
    {
        switch (btn)
        {
            case DWButton.OK: return TDReturn.IDOK;
            case DWButton.YES: return TDReturn.IDYES;
            case DWButton.NO: return TDReturn.IDNO;
            case DWButton.RETRY: return TDReturn.IDRETRY;
            case DWButton.CANCEL: return TDReturn.IDCANCEL;

            default:
            case DWButton.CLOSE: return TDReturn.IDCLOSE;
        }
    }

    #endregion

    #region Dialog properties

    /// <summary>
    /// Representing the dialog window title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Representng the message caption text. This text is different from the <see cref="Window.Title"/> property. 
    /// </summary>
    public string Caption { get; set; }

    /// <summary>
    /// Representing the message text itself.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Representing the dialog window image.
    /// </summary>
    public DWImage Image { get; set; }

    /// <summary>
    /// Representing dialog button options. Each of those options has an associated action to it.
    /// Use pipe symbol (|) to chain multiple buttons.
    /// </summary>
    public DWButton Buttons { get; set; }

    #endregion

    #region Methods

    /// <summary>
    /// Creates the dialog and displays it on the screen.
    /// </summary>
    public TDReturn Show()
    {
        // create window
        Window wnd = new Window
        {
            Title = this.Title,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            ResizeMode = ResizeMode.NoResize,
            SizeToContent = SizeToContent.WidthAndHeight,
            MaxWidth = 640
        };

        // window created, set default return value
        _result = TDReturn.IDCANCEL;

        // create layout
        // display caption, message, image and selected buttons

        Grid g = new Grid
        {
            Margin = new Thickness(10)
        };

        StackPanel stp = new StackPanel();

        // top grid - image and caption
        Grid gTop = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition {Width = GridLength.Auto },
                new ColumnDefinition { }
            }
        };

        Image img = new Image
        {
            Source = GetImage(this.Image)
        };

        Label lbl = new Label
        {
            Content = this.Caption
        };

        gTop.Children.Add(img);
        gTop.Children.Add(lbl);

        Grid.SetColumn(img, 0);
        Grid.SetColumn(lbl, 1);

        stp.Children.Add(gTop);

        // message itself
        Label lblMessage = new Label
        {
            Content = new TextBlock
            {
                Text = this.Message,
                TextWrapping = TextWrapping.Wrap,
                TextTrimming = TextTrimming.CharacterEllipsis
            },

            Margin = new Thickness(0, 5, 0, 0)
        };

        stp.Children.Add(lblMessage);

        StackPanel stpFooter = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(0, 10, 0, 0)
        };

        // draw buttons
        foreach (DWButton btn in Enum.GetValues<DWButton>())
        {
            if (this.Buttons.HasFlag(btn) == true)
            {
                // draw button
                Button b = new Button
                {
                    Content = GetButtonText(btn),
                    Margin = new Thickness(5, 0, 0, 0)
                };

                b.Click += (s, e) =>
                {
                    // set status and close
                    _result = GetResult(btn);
                    wnd.Close();
                };

                stpFooter.Children.Add(b);
            }
        }

        stp.Children.Add(stpFooter);

        g.Children.Add(stp);
        wnd.Content = g;

        // show window as dialog
        _ = wnd.ShowDialog();

        // return result
        return _result;
    }

    #endregion

    #region Static methods and elements

    /// <summary>
    /// Shows the custom WinAPI-like TaskDialog.
    /// </summary>
    /// <param name="message">Main dialog message. This is the actual message field.</param>
    /// <param name="title">Window title bar text.</param>
    /// <param name="caption">Caption of the message.</param>
    /// <param name="image">Specified dialog image. Default is <see cref="DWImage.INFO"/>.</param>
    /// <param name="buttons">Available buttons for user to choose from. These buttons directly influence the return value of this method.</param>
    /// <returns>Value specified by the button user pressed.</returns>
    public static TDReturn ShowDialog(string message, string title, string caption, DWImage image, DWButton buttons)
    {
        DialogWindow dw = new DialogWindow
        {
            Title = title,
            Message = message,
            Caption = caption,
            Image = image,
            Buttons = buttons
        };

        return dw.Show();
    }

    #endregion
}
