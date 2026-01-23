using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FZCore.Controls;

/// <summary>
/// Representing a custom caption bar control.
/// </summary>
public partial class CtlCaptionBar : UserControl
{
    private Window? _window;

    private const string TEXT_BTN_MAXIMIZE = "";
    private const string TEXT_BTN_RESTORE = "";

    /// <summary>
    /// Gets or sets the controls caption text.
    /// </summary>
    public string Title
    {
        get => tbTitle.Text;
        set => tbTitle.Text = value;
    }

    private UIElement? _element;

    /// <summary>
    /// Representing a custom element in the left side of the caption bar. It can be a custom <see cref="Button"/>, <see cref="Image"/> with the application icon or anything else.
    /// </summary>
    public UIElement? HeaderElement
    {
        get => _element;
        set
        {
            if (value != null)
            {
                _element = value;

                // set the element
                grid.Children.Add(_element);
                Grid.SetColumn(_element, 0);
            }

            else
            {
                if (grid.Children.Contains(_element))
                {
                    grid.Children.Remove(_element);
                }

                _element = null;
            }
        }
    }

    /// <summary>
    /// Gets or sets the target <see cref="Window"/>.
    /// </summary>
    public Window? Target
    {
        get => _window;
        set
        {
            RegisterTarget(value);
        }
    }

    /// <summary>
    /// Determines whether the window can be minimized using the corresponding controls.
    /// </summary>
    public bool CanMinimize
    {
        get => field;
        set
        {
            btnMinimize.IsEnabled = value;
            field = value;
        }
    }

    /// <summary>
    /// Determines whether the window can be maximized using the corresponding controls.
    /// </summary>
    public bool CanMaximize
    {
        get => field;
        set
        {
            btnMaximize.IsEnabled = value;
            field = value;
        }
    }

    private void RegisterTarget(Window? window)
    {
        if (window == null)
        {
            if (_window != null)
            {
                _window.StateChanged -= Window_StateChanged;
            }
            return;
        }

        _window = window;
        window.StateChanged += Window_StateChanged;
        Title = window.Title;
    }

    /// <summary>
    /// Creates a new <see cref="CtlCaptionBar"/> instance.
    /// </summary>
    public CtlCaptionBar()
    {
        InitializeComponent();
        CanMaximize = true;
        CanMinimize = true;
    }

    /// <summary>
    /// Creates a new <see cref="CtlCaptionBar"/> instance.
    /// </summary>
    /// <param name="window">Target window.</param>
    public CtlCaptionBar(Window window)
    {
        InitializeComponent();
        RegisterTarget(window);

        CanMaximize = true;
        CanMinimize = true;
    }

    /// <summary>
    /// Class destructor.
    /// </summary>
    ~CtlCaptionBar()
    {
        _window?.StateChanged -= Window_StateChanged;
    }

    private void Window_StateChanged(object? sender, System.EventArgs e)
    {
        switch (_window?.WindowState)
        {
            case WindowState.Normal:
                btnMaximize.Content = TEXT_BTN_MAXIMIZE;
                //miRestore.IsEnabled = false;
                //miMaximize.IsEnabled = true;
                break;

            case WindowState.Maximized:
                btnMaximize.Content = TEXT_BTN_RESTORE;
                //miRestore.IsEnabled = true;
                //miMaximize.IsEnabled = false;
                break;

            default:
                break;
        }
    }

    private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _window?.DragMove();
    }

    private void btnMinimize_Click(object sender, RoutedEventArgs e)
    {
        _window?.WindowState = WindowState.Minimized;
    }

    private void Maximize()
    {
        _window?.WindowState = WindowState.Maximized;
    }

    private void Restore()
    {
        _window?.WindowState = WindowState.Normal;
    }

    private void btnMaximize_Click(object sender, RoutedEventArgs e)
    {
        switch (_window?.WindowState)
        {
            default:
                break;

            case WindowState.Maximized:
                Restore();
                break;

            case WindowState.Normal:
                Maximize();
                break;
        }
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        _window?.Close();
    }

    private void miRestore_Click(object sender, RoutedEventArgs e)
    {
        Restore();
    }

    private void miMaximize_Click(object sender, RoutedEventArgs e)
    {
        Maximize();
    }
}
