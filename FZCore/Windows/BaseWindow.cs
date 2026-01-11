using FZCore.Win32;

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
    }

    /// <summary>
    /// Initializes a new instance of the BaseWindow class with the specified owner window.
    /// </summary>
    /// <param name="owner">The window that will act as the owner of this window. Can be null if the window has no owner.</param>
    public BaseWindow(Window owner)
    {
        this.Owner = owner;
    }

    /// <summary>
    /// Representing the window handle.
    /// </summary>
    public nint Handle { get; protected set; }

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
        return HRESULT.SUCCEEDED(WinAPI.DwmSetWindowAttribute(this.Handle, (int)DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, [iValue], sizeof(int)));
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
        return HRESULT.SUCCEEDED(WinAPI.DwmSetWindowAttribute(this.Handle, (int)DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE, [3], sizeof(int)));
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
}
