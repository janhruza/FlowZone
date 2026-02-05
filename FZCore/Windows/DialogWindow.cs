using FZCore.Windows.Dialogs.Types;

using System;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace FZCore.Windows;

[Obsolete]
/// <summary>
/// Representing a custom dialog window.
/// <see cref="DialogWindow"/> is a more extended version of the standard <see cref="MessageBox"/> class.
/// In essence, <see cref="DialogWindow"/> is a kind of a TaskDialog, a Windows API modern version of <see cref="MessageBox"/>.
/// </summary>
/// /// <remarks>
/// This class is obsolette. Please use the new <see cref="DlgMessageBox"/> instead.
/// </remarks>
public class DialogWindow
{
    /// <summary>
    /// Creates a new instance of the <see cref="DialogWindow"/> class.
    /// </summary>
    public DialogWindow()
    {
        Title = string.Empty;
        Caption = string.Empty;
        Message = string.Empty;
        Image = DWImage.INFO;
        Buttons = DWButton.OK;
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

    private static SystemSound GetSound(DWImage image)
    {
        switch (image)
        {
            default:
            case DWImage.INFO:
                return SystemSounds.Beep;

            case DWImage.WARNING:
                return SystemSounds.Exclamation;

            case DWImage.ERROR:
                return SystemSounds.Hand;

            case DWImage.SHIELD:
                return SystemSounds.Exclamation;
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
        // Create window
        IconlessWindow wnd = new IconlessWindow
        {
            Title = Title,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            ResizeMode = ResizeMode.NoResize,
            SizeToContent = SizeToContent.WidthAndHeight,
            MaxWidth = 600,
            MinWidth = 320,
            SnapsToDevicePixels = true,
            UseLayoutRounding = true
        };

        wnd.Loaded += (s, e) => GetSound(Image).Play();

        this._result = TDReturn.IDCANCEL;

        // Layout root
        Grid rootGrid = new Grid { Margin = new Thickness(10) };
        StackPanel mainPanel = new StackPanel();

        // Top section: image + caption
        Grid topGrid = new Grid();
        topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        topGrid.ColumnDefinitions.Add(new ColumnDefinition());

        Image img = new Image
        {
            Source = GetImage(Image),
            Width = 32,
            Height = 32
        };

        Label captionLabel = new Label
        {
            Content = Caption,
            Foreground = SystemColors.AccentColorBrush,
            FontSize = 20,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(5, 0, 0, 0)
        };

        _ = topGrid.Children.Add(img);
        _ = topGrid.Children.Add(captionLabel);
        Grid.SetColumn(img, 0);
        Grid.SetColumn(captionLabel, 1);

        _ = mainPanel.Children.Add(topGrid);

        // Message
        _ = mainPanel.Children.Add(new Label
        {
            Content = new TextBlock
            {
                Text = Message,
                TextWrapping = TextWrapping.Wrap,
                TextTrimming = TextTrimming.CharacterEllipsis
            },
            Margin = new Thickness(0, 5, 0, 0)
        });

        // Footer buttons
        StackPanel footerPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(0, 10, 0, 0)
        };

        if (Buttons == DWButton.OK)
        {
            AddButton(footerPanel, DWButton.OK, wnd);
        }

        else
        {
            foreach (DWButton btn in Enum.GetValues<DWButton>())
            {
                if (btn == DWButton.OK) continue; // Skip 0-value flag

                if ((Buttons & btn) == btn)
                {
                    AddButton(footerPanel, btn, wnd);
                }
            }
        }

        _ = mainPanel.Children.Add(footerPanel);
        _ = rootGrid.Children.Add(mainPanel);
        wnd.Content = rootGrid;

        _ = wnd.ShowDialog();
        return this._result;
    }

    private void AddButton(StackPanel panel, DWButton btn, Window wnd)
    {
        Button b = new Button
        {
            Content = GetButtonText(btn),
            Margin = new Thickness(5, 0, 0, 0)
        };

        b.Click += (s, e) =>
        {
            this._result = GetResult(btn);
            wnd.Close();
        };

        _ = panel.Children.Add(b);
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
