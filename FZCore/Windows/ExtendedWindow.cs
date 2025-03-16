using System;
using System.Windows;
using FZCore.Extensions;

namespace FZCore.Windows;

/// <summary>
/// Representing the enumeration of the custom, <see cref="ExtendedWindow"/> window states.
/// </summary>
public enum ExtendedWindowState : byte
{
    /// <summary>
    /// Representing the normal state, overlapped window.
    /// </summary>
    Normal = WindowState.Normal,

    /// <summary>
    /// Representing the maximized window state - <see cref="WindowState.Maximized"/>.
    /// </summary>
    Maximized = WindowState.Maximized,

    /// <summary>
    /// Representing the minimized window state - <see cref="WindowState.Minimized"/>.
    /// </summary>
    Minimized = WindowState.Minimized,

    /// <summary>
    /// Representing the maximized window state - <see cref="WindowState.Maximized"/>
    /// with all window borders removed and <see cref="Window.TopmostProperty"/> set to true.
    /// </summary>
    Fullscreen = 0xFF
}

/// <summary>
/// Representing the already extended <see cref="Window"/>.
/// This <see cref="Window"/> can be used as a base for other windows.
/// </summary>
public class ExtendedWindow : Window
{
    /// <summary>
    /// Initializes a new <see cref="ExtendedWindow"/>.
    /// </summary>
    public ExtendedWindow()
    {
        // creates the window identifier
        _guid = Guid.CreateVersion7();

        // creates the window handle
        _handle = (nuint)this.GetHandle();

        // initializes the extender class
        _extender = new WindowExtender(this);

        // sets the default index for menu items
        idx = 0x10;

        // extends the window with basic extended functionality
        _extender.EmpowerWindow();
    }

    #region Private members and methods

    /// <summary>
    /// Representing the index for all new items in the window's system menu.
    /// </summary>
    private int idx;

    /// <summary>
    /// Representing the working window extender object.
    /// </summary>
    private WindowExtender _extender;

    private void RemoveBorders()
    {
        this.WindowStyle = WindowStyle.None;
        return;
    }

    private void RestoreBorders()
    {
        this.WindowStyle = WindowStyle.SingleBorderWindow;
        return;
    }

    /// <summary>
    /// Representing the extended window state.
    /// </summary>
    private ExtendedWindowState _state = ExtendedWindowState.Normal;

    /// <summary>
    /// Representing the window handle.
    /// </summary>
    private nuint _handle;

    /// <summary>
    /// Representing the window GUID.
    /// </summary>
    private Guid _guid;

    #endregion

    #region Public members and methods

    /// <summary>
    /// Adds a menu item to the window's system menu.
    /// </summary>
    /// <param name="item">Information about the menu item.</param>
    public void AddMenuItem(ExtendedMenuItem item) => _extender.AddMenuItem(idx++, item);

    /// <summary>
    /// Adds a separator to the window's system menu.
    /// </summary>
    public void AddMenuSeparator() => _extender.AddSeparator(idx++);

    /// <summary>
    /// Sets the window's extended <see cref="WindowStyle"/>.
    /// </summary>
    /// <param name="state">New window state of the window.</param>
    public void SetWindowState(ExtendedWindowState state)
    {
        // set new state
        _state = state;

        switch (state)
        {
            default:
                // no default action
                return;

            case ExtendedWindowState.Normal:
                this.WindowState = WindowState.Normal;
                RestoreBorders();
                this.Topmost = false;
                return;

            case ExtendedWindowState.Minimized:
                this.WindowState = WindowState.Minimized;
                return;

            case ExtendedWindowState.Maximized:
                RestoreBorders();
                this.WindowState = WindowState.Maximized;
                this.Topmost = false;
                return;

            case ExtendedWindowState.Fullscreen:
                RemoveBorders();
                this.WindowState = WindowState.Maximized;
                this.Topmost = true;
                return;
        }
    }

    /// <summary>
    /// Gets or sets the extended <see cref="WindowState"/> of this window.
    /// </summary>
    public ExtendedWindowState WindowStateEx { get => _state; set => SetWindowState(value); }

    /// <summary>
    /// Representing the attached <see cref="WindowExtender"/> object.
    /// </summary>
    public WindowExtender Extender => _extender;

    /// <summary>
    /// Representing the window handle (HWND) as <see cref="UIntPtr"/>.
    /// </summary>
    public nuint Handle => _handle;

    /// <summary>
    /// Representing the window identification as <see cref="Guid"/>.
    /// </summary>
    public Guid Id => _guid;

    #endregion
}
