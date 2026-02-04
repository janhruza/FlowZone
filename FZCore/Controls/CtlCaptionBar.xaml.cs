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
        get => this.tbTitle.Text;
        set => this.tbTitle.Text = value;
    }

    private UIElement? _element;

    /// <summary>
    /// Representing a custom element in the left side of the caption bar. It can be a custom <see cref="Button"/>, <see cref="Image"/> with the application icon or anything else.
    /// </summary>
    public UIElement? HeaderElement
    {
        get => this._element;
        set
        {
            if (value != null)
            {
                this._element = value;

                // set the element
                _ = this.grid.Children.Add(this._element);
                Grid.SetColumn(this._element, 0);
            }

            else
            {
                if (this.grid.Children.Contains(this._element))
                {
                    this.grid.Children.Remove(this._element);
                }

                this._element = null;
            }
        }
    }

    /// <summary>
    /// Gets or sets the target <see cref="Window"/>.
    /// </summary>
    public Window? Target
    {
        get => this._window;
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
            this.btnMinimize.IsEnabled = value;
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
            this.btnMaximize.IsEnabled = value;
            field = value;
        }
    }

    private void RegisterTarget(Window? window)
    {
        if (window == null)
        {
            if (this._window != null)
            {
                this._window.StateChanged -= Window_StateChanged;
            }
            return;
        }

        this._window = window;
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
        this._window?.StateChanged -= Window_StateChanged;
    }

    private void Window_StateChanged(object? sender, System.EventArgs e)
    {
        switch (this._window?.WindowState)
        {
            case WindowState.Normal:
                this.btnMaximize.Content = TEXT_BTN_MAXIMIZE;
                //miRestore.IsEnabled = false;
                //miMaximize.IsEnabled = true;
                break;

            case WindowState.Maximized:
                this.btnMaximize.Content = TEXT_BTN_RESTORE;
                //miRestore.IsEnabled = true;
                //miMaximize.IsEnabled = false;
                break;

            default:
                break;
        }
    }

    private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        this._window?.DragMove();
    }

    private void btnMinimize_Click(object sender, RoutedEventArgs e)
    {
        _ = (this._window?.WindowState = WindowState.Minimized);
    }

    private void Maximize()
    {
        _ = (this._window?.WindowState = WindowState.Maximized);
    }

    private void Restore()
    {
        _ = (this._window?.WindowState = WindowState.Normal);
    }

    private void btnMaximize_Click(object sender, RoutedEventArgs e)
    {
        switch (this._window?.WindowState)
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
        this._window?.Close();
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
