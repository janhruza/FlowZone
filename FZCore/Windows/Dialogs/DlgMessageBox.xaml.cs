using FZCore.Windows.Dialogs.Types;

using System;
using System.Collections.Generic;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FZCore.Windows.Dialogs;

/// <summary>
/// Representing a custom message box dialog window.
/// </summary>
public partial class DlgMessageBox : IconlessWindow
{
    private ImageSource? GetImage(DWImage image)
    {
        switch (image)
        {
            case DWImage.INFO:
                return Core.GetImageSource("information.png");

            case DWImage.ERROR:
                return Core.GetImageSource("close.png");

            case DWImage.WARNING:
                return Core.GetImageSource("warning.png");

            case DWImage.SHIELD:
                return Core.GetImageSource("shield.png");

            default:
                return null;
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

    private Dictionary<DWButton, Button> _buttons = new Dictionary<DWButton, Button>();
    private DWImage _image = default;

    private void AddButton(StackPanel panel, DWButton btn)
    {
        Button b = new Button
        {
            Content = GetButtonText(btn),
            Margin = new Thickness(5, 0, 0, 0)
        };

        b.Click += (s, e) =>
        {
            Result = GetResult(btn);
            Close();
        };

        _ = panel.Children.Add(b);
        this._buttons.Add(btn, b);
    }

    /// <summary>
    /// Creates a new <see cref="DlgMessageBox"/> instance.
    /// </summary>
    private DlgMessageBox(string message, string caption, DWButton buttons, DWImage image, DWButton? active)
    {
        InitializeComponent();

        // basic properties
        Title = caption;
        this.tbMessage.Text = message;
        this._image = image;

        // icon
        ImageSource? icon = GetImage(image);
        if (icon == null)
        {
            // no image
            this.img.Margin = new System.Windows.Thickness(0);
            this.img.Visibility = System.Windows.Visibility.Collapsed;
        }

        else
        {
            this.img.Source = icon;
        }

        // buttons
        if (buttons == DWButton.OK)
        {
            AddButton(this.stpButtons, DWButton.OK);
        }

        else
        {
            foreach (DWButton btn in Enum.GetValues<DWButton>())
            {
                if (btn == DWButton.OK) continue; // Skip 0-value flag

                if ((buttons & btn) == btn)
                {
                    AddButton(this.stpButtons, btn);
                }
            }
        }

        // sets the default button
        if (active.HasValue && buttons.HasFlag(active))
        {
            this._buttons[active.Value].IsDefault = true;
        }
    }

    /// <summary>
    /// Representing a custom dialog result property.
    /// </summary>
    /// <remarks>
    /// Use this instead of the <see cref="Window.DialogResult"/> property to accurately handle the dialog result.
    /// </remarks>
    public TDReturn Result { get; private set; } = TDReturn.IDCANCEL;

    private void IconlessWindow_Loaded(object sender, RoutedEventArgs e)
    {
        // play the target sound
        GetSound(this._image).Play();
    }

    /// <summary>
    /// Shows the dialog window with the given properties.
    /// </summary>
    /// <param name="message">A message to be shown.</param>
    /// <param name="caption">Message box caption.</param>
    /// <param name="buttons">User action buttons.</param>
    /// <param name="image">Dialog icon.</param>
    /// <param name="activeButton">Default action.</param>
    /// <returns>Message box result.</returns>
    public static TDReturn Show(string message, string caption, DWButton buttons, DWImage image, DWButton? activeButton = null)
    {
        DlgMessageBox msgBox = new DlgMessageBox(message, caption, buttons, image, activeButton);
        _ = msgBox.ShowDialog();
        return msgBox.Result;
    }
}
