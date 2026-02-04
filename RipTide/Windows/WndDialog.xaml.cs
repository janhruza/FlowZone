using FZCore.Windows;

using RipTide.Core;

using System.Collections.Generic;
using System.Media;
using System.Windows;

namespace RipTide.Windows;

/// <summary>
/// Representing the custom dialog window.
/// </summary>
public partial class WndDialog : IconlessWindow
{
    #region Static data

    private const char C_ERROR = '';
    private const char C_WARNING = '';
    private const char C_INFO = '';
    private const char C_SUCCESS = '';

    private Dictionary<DialogIcon, char> IconsByImage => new Dictionary<DialogIcon, char>
    {
        { DialogIcon.Information, C_INFO },
        { DialogIcon.Error, C_ERROR },
        { DialogIcon.Warning, C_WARNING },
        { DialogIcon.Success, C_SUCCESS }
    };

    #endregion

    /// <summary>
    /// Representing the message that is displayed inside of the dialog window.
    /// </summary>
    public string Message
    {
        get => this.tbText.Text;
        set => this.tbText.Text = value;
    }

    /// <summary>
    /// Representing the dialog icon as <see cref="char"/>.
    /// </summary>
    public char IconGlyph
    {
        get
        {
            if (this.lImage.Content.ToString() == null)
            {
                return C_INFO;
            }

            else
            {
                return this.lImage.ToString()[0];
            }
        }

        set => this.lImage.Content = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="WndDialog"/> class with default values.
    /// </summary>
    public WndDialog()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Creates a new instance of the <see cref="WndDialog"/> class with specified message.
    /// </summary>
    /// <param name="message">The message text.</param>
    public WndDialog(string message)
    {
        InitializeComponent();
        Message = message;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="WndDialog"/> class with specified message and window title.
    /// </summary>
    /// <param name="message">The message text.</param>
    /// <param name="title">Title of the dialog window.</param>
    public WndDialog(string message, string title)
    {
        InitializeComponent();
        Message += message;
        Title = title;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="WndDialog"/> class with specified message, window title and dialog icon.
    /// </summary>
    /// <param name="message">The message text.</param>
    /// <param name="title">Title of the dialog window.</param>
    /// <param name="icon">Icon of the dialog window.</param>
    public WndDialog(string message, string title, DialogIcon icon)
    {
        InitializeComponent();
        Message = message;
        Title = title;
        IconGlyph = IconsByImage[icon];
    }

    /// <summary>
    /// Opens the window as dialog. This is a custom method, overloading the base one.
    /// </summary>
    /// <returns></returns>
    public new bool? ShowDialog()
    {
        switch (IconGlyph)
        {

            case C_INFO:
                SystemSounds.Beep.Play();
                break;

            case C_ERROR:
                SystemSounds.Hand.Play();
                break;

            case C_WARNING:
                SystemSounds.Exclamation.Play();
                break;

            case C_SUCCESS:
                SystemSounds.Beep.Play();
                break;
        }

        return base.ShowDialog();
    }

    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }
}
