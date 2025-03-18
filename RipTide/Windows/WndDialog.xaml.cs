using System.Collections.Generic;
using System.Media;
using System.Windows;
using RipTide.Core;

namespace RipTide.Windows;

/// <summary>
/// Representing the custom dialog window.
/// </summary>
public partial class WndDialog : Window
{
    #region Static data

    const char C_ERROR = '';
    const char C_WARNING = '';
    const char C_INFO = '';
    const char C_SUCCESS = '';

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
        get => tbText.Text;
        set => tbText.Text = value;
    }

    /// <summary>
    /// Representing the dialog icon as <see cref="System.Char"/>.
    /// </summary>
    public char IconGlyph
    {
        get
        {
            if (lImage.Content.ToString() == null)
            {
                return C_INFO;
            }

            else
            {
                return this.lImage.ToString()[0];
            }
        }

        set => lImage.Content = value;
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
        this.DialogResult = true;
        this.Close();
    }
}
