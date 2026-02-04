using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FZCore.Controls;

/// <summary>
/// Representing a custom caption buttons control.
/// </summary>
public partial class CtlCaptionButtons : UserControl
{
    private Window? _window;

    private const string TEXT_BTN_MAXIMIZE = "";
    private const string TEXT_BTN_RESTORE = "";

    private Button[] _buttons;

    /// <summary>
    /// Creatse a new <see cref="CtlCaptionButtons"/> instance.
    /// </summary>
    public CtlCaptionButtons()
    {
        InitializeComponent();
        this._buttons = [this.btnClose, this.btnMaximize, this.btnMinimize];
        Foreground = SystemColors.WindowTextBrush;
    }

    /// <summary>
    /// Custom destructor.
    /// </summary>
    ~CtlCaptionButtons()
    {
        if (this._window != null)
        {
            this._window.StateChanged -= _window_StateChanged;
        }
    }

    /// <summary>
    /// Gets or sets the target window.
    /// </summary>
    public Window? Target
    {
        get => field;
        set
        {
            this._window = value;
            field = value;

            if (this._window != null)
            {
                this._window.StateChanged += _window_StateChanged;
                //foreach (Button btn in _buttons)
                //{
                //    btn.Foreground = _window.Foreground;
                //}
            }
        }
    }

    private void SetForeground(Brush color)
    {
        foreach (Button btn in this._buttons)
        {
            btn.Foreground = color;
        }
    }

    /// <summary>
    /// Gets or sets the brush used to paint the foreground of the control and its child buttons.
    /// </summary>
    /// <remarks>Setting this property updates the foreground brush for all contained buttons. This property
    /// overrides the base class implementation to ensure consistency across child elements.</remarks>
    public new Brush Foreground
    {
        get => field;
        set
        {
            SetForeground(value);
            field = value;
        }
    }

    private void _window_StateChanged(object? sender, System.EventArgs e)
    {
        switch (this._window?.WindowState)
        {
            default:
                break;

            case WindowState.Normal:
                this.btnMaximize.Content = TEXT_BTN_MAXIMIZE;
                break;

            case WindowState.Maximized:
                this.btnMaximize.Content = TEXT_BTN_RESTORE;
                break;
        }
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        this._window?.Close();
    }

    private void btnMaximize_Click(object sender, RoutedEventArgs e)
    {
        if (this._window?.WindowState == WindowState.Normal)
        {
            _ = (this._window?.WindowState = WindowState.Maximized);
        }

        else
        {
            _ = (this._window?.WindowState = WindowState.Normal);
        }
    }

    private void btnMinimize_Click(object sender, RoutedEventArgs e)
    {
        _ = (this._window?.WindowState = WindowState.Minimized);
    }
}
