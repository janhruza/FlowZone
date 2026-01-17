using FZCore.Win32;

using System;
using System.Collections.Generic;
using System.Windows;

namespace FZCore.Windows;

/// <summary>
/// Provides a base class for application windows that extends the standard Window functionality.
/// </summary>
/// <remarks>
/// This class doesn't modify the base <see cref="Window"/> in any way but implements functionalities for all inheriting windows.
/// </remarks>
public class BaseWindow : Window
{
    /// <summary>
    /// Initializes a new instance of the BaseWindow class with no owner window set.
    /// </summary>
    public BaseWindow()
    {
        this.Owner = null;
        this.Initialize();
    }

    /// <summary>
    /// Initializes a new instance of the BaseWindow class with the specified owner window.
    /// </summary>
    /// <param name="owner">The window that will act as the owner of this window. Can be null if the window has no owner.</param>
    public BaseWindow(Window owner)
    {
        this.Owner = owner;
        this.Initialize();
    }

    /// <summary>
    /// Window destructor.
    /// </summary>
    ~BaseWindow()
    {
        _windows.Remove(this);
    }

    /// <summary>
    /// Occurs when the user presses the Help key, allowing subscribers to respond to help requests.
    /// </summary>
    /// <remarks>Subscribe to this event to provide custom help functionality when the Help key is activated.
    /// The event handler receives standard event arguments and does not provide additional context about the help
    /// request.</remarks>
    public event EventHandler HelpKeyPressed = delegate { };

    /// <summary>
    /// Occurrs when the F12 key is pressed.
    /// </summary>
    public event EventHandler DevToolsKeyPressed = delegate { };

    private bool _initialized = false;
    private void Initialize()
    {
        // allow only one initialization
        if (_initialized == true) return;

        // top priority setup
        _windows.Add(this);

        // initialize the window
        this.SnapsToDevicePixels = true;
        this.UseLayoutRounding = true;
        this.TaskbarItemInfo = new System.Windows.Shell.TaskbarItemInfo();

        // enable events
        this.KeyDown += (s, e) =>
        {
            if (e.Key == System.Windows.Input.Key.F1 || e.Key == System.Windows.Input.Key.Help)
            {
                HelpKeyPressed.Invoke(this, EventArgs.Empty);
            }

            else if (e.Key == System.Windows.Input.Key.F12)
            {
                DevToolsKeyPressed.Invoke(this, EventArgs.Empty);
            }
        };

        // window initialized
        _initialized = true;
        return;
    }

    private nint _handle;

    /// <summary>
    /// Representing the window handle.
    /// </summary>
    public nint Handle => _handle;

    /// <summary>
    /// Overrides the OnSourceInitialized method to capture the window handle after the source is initialized.
    /// </summary>
    /// <param name="e">Possible arguments.</param>
    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        _handle = new System.Windows.Interop.WindowInteropHelper(this).EnsureHandle();
    }

    /// <summary>
    /// Enables or disables dark mode for the window.
    /// </summary>
    /// <remarks>This method attempts to set the window's appearance to dark mode using the system's immersive
    /// dark mode attribute. The effect may depend on the operating system version and user settings.</remarks>
    /// <param name="value">true to enable dark mode; false to disable it.</param>
    /// <returns>true if the dark mode setting was applied.</returns>
    public bool SetDarkMode(bool value)
    {
        int iValue = value == true ? 1 : 0;
        return HRESULT.SUCCEEDED(WinAPI.DwmSetWindowAttribute(_handle, (int)DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, [iValue], sizeof(int)));
    }

    /// <summary>
    /// Attempts to set the window's system backdrop to a transient style.
    /// </summary>
    /// <remarks>This method modifies the appearance of the window by changing its system backdrop type. The
    /// availability and effect of the transient backdrop may depend on the operating system version and system
    /// configuration.</remarks>
    /// <returns>true if the system backdrop was successfully set to transient; otherwise, false.</returns>
    public bool MakeTransient()
    {
        return HRESULT.SUCCEEDED(WinAPI.DwmSetWindowAttribute(_handle, (int)DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE, [3], sizeof(int)));
    }

    /// <summary>
    /// Displays the window as a modal dialog box and returns only when the newly opened window is closed.
    /// </summary>
    /// <remarks>If the window has an owner, the dialog is centered relative to its owner; otherwise, it is
    /// centered on the screen.</remarks>
    /// <returns>A nullable Boolean value that specifies how a window was closed by the user. Returns <see langword="true"/> if
    /// the user accepted the dialog box (for example, by clicking OK); <see langword="false"/> if the user canceled the
    /// dialog box; or <see langword="null"/> if the dialog result is not set.</returns>
    public new bool? ShowDialog()
    {
        if (this.Owner != null)
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        else
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        return base.ShowDialog();
    }

    /// <summary>
    /// Sets the progress value for the taskbar item, updating its visual progress indicator.
    /// </summary>
    /// <remarks>If the specified progress value exceeds 1, it is treated as a percentage and divided by 100
    /// before being applied. The progress indicator reflects the value set, where 0 represents no progress and 1
    /// represents completion.</remarks>
    /// <param name="progress">The progress value to display, typically between 0 and 1. Values greater than 1 are interpreted as percentages
    /// and automatically scaled.</param>
    public void ProgressUpdate(double progress)
    {
        if (progress > 1)
        {
            progress /= 100;
        }

        this.TaskbarItemInfo.ProgressValue = progress;
        return;
    }

    /// <summary>
    /// Sets the taskbar progress indicator to the paused state, visually indicating that the current operation is
    /// temporarily halted.
    /// </summary>
    /// <remarks>Use this method to signal to users that a process is paused but not completed or canceled.
    /// The taskbar button will display the paused progress state until updated by another method.</remarks>
    public void ProgressPause()
    {
        this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Paused;
        return;
    }

    /// <summary>
    /// Sets the taskbar progress state to indicate an error condition.
    /// </summary>
    /// <remarks>Call this method to visually signal an error on the application's taskbar button, typically
    /// when a background operation fails. The taskbar will display the progress indicator in the error state, which is
    /// usually shown as a red overlay. This method does not reset or clear any previous progress value.</remarks>
    public void ProgressError()
    {
        this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Error;
        return;
    }

    /// <summary>
    /// Sets the taskbar progress indicator to the indeterminate state, indicating that progress is ongoing but its
    /// completion percentage is unknown.
    /// </summary>
    /// <remarks>Use this method when the duration or completion of the associated task cannot be determined.
    /// The taskbar will display a continuous animation to signal activity without a specific progress value.</remarks>
    public void ProgressIntermediate()
    {
        this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Indeterminate;
        return;
    }

    /// <summary>
    /// Removes the progress indicator from the application's taskbar button.
    /// </summary>
    /// <remarks>Call this method to clear any progress state previously set on the taskbar button, such as
    /// when an operation completes or is canceled. This method has no effect if no progress indicator is currently
    /// displayed.</remarks>
    public void ProgressDisable()
    {
        this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
        return;
    }

    #region Static section

    private static readonly List<BaseWindow> _windows = [];

    /// <summary>
    /// Retrieves a list of all currently active BaseWindow instances.
    /// </summary>
    public static List<BaseWindow> ActiveWindows => _windows;

    #endregion
}
