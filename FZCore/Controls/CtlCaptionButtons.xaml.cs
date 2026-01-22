using System.Windows;
using System.Windows.Controls;

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
        _buttons = [btnClose, btnMaximize, btnMinimize];
    }

    /// <summary>
    /// Custom destructor.
    /// </summary>
    ~CtlCaptionButtons()
    {
        if (_window != null)
        {
            _window.StateChanged -= _window_StateChanged;
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
            _window = value;
            field = value;

            if (_window != null)
            {
                _window.StateChanged += _window_StateChanged;
                foreach (Button btn in _buttons)
                {
                    btn.Foreground = _window.Foreground;
                }
            }
        }
    }

    private void _window_StateChanged(object? sender, System.EventArgs e)
    {
        switch (_window?.WindowState)
        {
            default:
                break;

            case WindowState.Normal:
                btnMaximize.Content = TEXT_BTN_MAXIMIZE;
                break;

            case WindowState.Maximized:
                btnMaximize.Content = TEXT_BTN_RESTORE;
                break;
        }
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        _window?.Close();
    }

    private void btnMaximize_Click(object sender, RoutedEventArgs e)
    {
        if (_window?.WindowState == WindowState.Normal)
        {
            _window?.WindowState = WindowState.Maximized;
        }

        else
        {
            _window?.WindowState = WindowState.Normal;
        }
    }

    private void btnMinimize_Click(object sender, RoutedEventArgs e)
    {
        _window?.WindowState = WindowState.Minimized;
    }
}
