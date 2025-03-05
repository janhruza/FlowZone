using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

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

    #region Private members

    private TDReturn _result = TDReturn.ERROR;

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
            SizeToContent = SizeToContent.WidthAndHeight
        };

        // window created, set default return value
        _result = TDReturn.IDCANCEL;

        // create layout
        // display caption, message, image and selected buttons

        Grid g = new Grid();
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
            
        };

        Label lbl = new Label
        {
            Content = this.Caption
        };

        g.Children.Add(stp);
        wnd.Content = g;
        // show window as dialog
        _ = wnd.ShowDialog();

        return _result;
    }

    #endregion

    #region Static methods and elements



    #endregion
}
