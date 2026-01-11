namespace FZCore.Win32;

/// <summary>
/// Flags used by the DwmGetWindowAttribute and DwmSetWindowAttribute functions 
/// to specify window attributes for Desktop Window Manager (DWM) non-client rendering.
/// </summary>
public enum DWMWINDOWATTRIBUTE
{
    /// <summary>
    /// Use with DwmGetWindowAttribute. Discovers whether non-client rendering is enabled. 
    /// The retrieved value is of type BOOL. TRUE if non-client rendering is enabled; otherwise, FALSE.
    /// </summary>
    DWMWA_NCRENDERING_ENABLED,

    /// <summary>
    /// Use with DwmSetWindowAttribute. Sets the non-client rendering policy. 
    /// The pvAttribute parameter points to a value from the DWMNCRENDERINGPOLICY enumeration.
    /// </summary>
    DWMWA_NCRENDERING_POLICY,

    /// <summary>
    /// Use with DwmSetWindowAttribute. Enables or forcibly disables DWM transitions. 
    /// The pvAttribute parameter points to a BOOL value. TRUE to disable transitions, or FALSE to enable transitions.
    /// </summary>
    DWMWA_TRANSITIONS_FORCEDISABLED,

    /// <summary>
    /// Use with DwmSetWindowAttribute. Enables content rendered in the non-client area to be visible on the frame drawn by DWM. 
    /// The pvAttribute parameter points to a BOOL value. TRUE to enable content rendered in the non-client area to be visible on the frame; otherwise, FALSE.
    /// </summary>
    DWMWA_ALLOW_NCPAINT,

    /// <summary>
    /// Use with DwmGetWindowAttribute. Retrieves the bounds of the caption button area in window-relative space. 
    /// The retrieved value is of type RECT. If the window is minimized or otherwise not visible to the user, the value of the RECT retrieved is undefined.
    /// </summary>
    DWMWA_CAPTION_BUTTON_BOUNDS,

    /// <summary>
    /// Use with DwmSetWindowAttribute. Specifies whether non-client content is right-to-left (RTL) mirrored. 
    /// The pvAttribute parameter points to a BOOL value. TRUE if the non-client content is right-to-left (RTL) mirrored; otherwise, FALSE.
    /// </summary>
    DWMWA_NONCLIENT_RTL_LAYOUT,

    /// <summary>
    /// Use with DwmSetWindowAttribute. Forces the window to display an iconic thumbnail or peek representation (a static bitmap), 
    /// even if a live or snapshot representation of the window is available. This value is normally set during a window's creation, 
    /// and not changed throughout the window's lifetime. Some technologies, such as Microsoft Silverlight, 
    /// utilize this value to enable correctly reporting to the Windows AirSpace settings. 
    /// The pvAttribute parameter points to a BOOL value. TRUE to require a iconic thumbnail or peek representation; otherwise, FALSE.
    /// </summary>
    DWMWA_FORCE_ICONIC_REPRESENTATION,

    /// <summary>
    /// Use with DwmSetWindowAttribute. Sets how Flip3D treats the window. 
    /// The pvAttribute parameter points to a value from the DWMFLIP3DWINDOWPOLICY enumeration.
    /// </summary>
    DWMWA_FLIP3D_POLICY,

    /// <summary>
    /// Use with DwmGetWindowAttribute. Retrieves the extended frame bounds rectangle in screen space. 
    /// The retrieved value is of type RECT.
    /// </summary>
    DWMWA_EXTENDED_FRAME_BOUNDS,

    /// <summary>
    /// Use with DwmSetWindowAttribute. Indicates that the window has a bitmap to use as an iconic thumbnail or peek representation. 
    /// The pvAttribute parameter points to a BOOL value. TRUE to indicate that the window has a bitmap; otherwise, FALSE.
    /// </summary>
    DWMWA_HAS_ICONIC_BITMAP,

    /// <summary>
    /// Use with DwmSetWindowAttribute. Disallows peek functionality for the window. 
    /// The pvAttribute parameter points to a BOOL value. TRUE to disallow peek; otherwise, FALSE.
    /// </summary>
    DWMWA_DISALLOW_PEEK,

    /// <summary>
    /// Use with DwmSetWindowAttribute. Prevents a window from fading to a glass sheet when peek is invoked. 
    /// The pvAttribute parameter points to a BOOL value. TRUE to prevent the window from fading; otherwise, FALSE.
    /// </summary>
    DWMWA_EXCLUDED_FROM_PEEK,

    /// <summary>
    /// Use with DwmGetWindowAttribute or DwmSetWindowAttribute. Cloaks the window such that it is not visible to the user. 
    /// The window is still composed by DWM.
    /// </summary>
    DWMWA_CLOAK,

    /// <summary>
    /// Use with DwmGetWindowAttribute. If the window is cloaked, provides one of the following values explaining why: 
    /// DWM_CLOAKED_APP (value 0x0000001), DWM_CLOAKED_SHELL (value 0x0000002), or DWM_CLOAKED_INHERITED (value 0x0000004).
    /// </summary>
    DWMWA_CLOAKED,

    /// <summary>
    /// Use with DwmSetWindowAttribute. Freezes the window's thumbnail image with its current contents. 
    /// No further live updates to the thumbnail image will be made until the lock is cleared.
    /// </summary>
    DWMWA_FREEZE_REPRESENTATION,

    /// <summary>
    /// Use with DwmSetWindowAttribute. Updates the window only when it is being shown. 
    /// This provides a performance benefit for applications that have many windows that are not shown.
    /// </summary>
    DWMWA_PASSIVE_UPDATE_MODE,

    /// <summary>
    /// Use with DwmSetWindowAttribute. Enables a non-UWP window to use host backdrop brushes. 
    /// If so, the app can then use DWMWA_SYSTEMBACKDROP_TYPE to set the backdrop type. 
    /// The pvAttribute parameter points to a BOOL value. TRUE to enable host backdrop brushes; otherwise, FALSE.
    /// </summary>
    DWMWA_USE_HOSTBACKDROPBRUSH,

    /// <summary>
    /// Use with DwmSetWindowAttribute. Allows the window to use the immersive dark mode theme. 
    /// The pvAttribute parameter points to a BOOL value. TRUE to use immersive dark mode; otherwise, FALSE.
    /// </summary>
    DWMWA_USE_IMMERSIVE_DARK_MODE = 20,

    /// <summary>
    /// Use with DwmSetWindowAttribute. Specifies the rounded corner preference for the window. 
    /// The pvAttribute parameter points to a value from the DWM_WINDOW_CORNER_PREFERENCE enumeration.
    /// </summary>
    DWMWA_WINDOW_CORNER_PREFERENCE = 33,

    /// <summary>
    /// Use with DwmSetWindowAttribute. Sets the color of the window border. 
    /// The pvAttribute parameter points to a COLORREF value.
    /// </summary>
    DWMWA_BORDER_COLOR,

    /// <summary>
    /// Use with DwmSetWindowAttribute. Sets the color of the window caption (title bar). 
    /// The pvAttribute parameter points to a COLORREF value.
    /// </summary>
    DWMWA_CAPTION_COLOR,

    /// <summary>
    /// Use with DwmSetWindowAttribute. Sets the color of the window caption text. 
    /// The pvAttribute parameter points to a COLORREF value.
    /// </summary>
    DWMWA_TEXT_COLOR,

    /// <summary>
    /// Use with DwmGetWindowAttribute. Retrieves the thickness of the visible frame border. 
    /// The retrieved value is of type UINT.
    /// </summary>
    DWMWA_VISIBLE_FRAME_BORDER_THICKNESS,

    /// <summary>
    /// Use with DwmGetWindowAttribute or DwmSetWindowAttribute. 
    /// Specifies the system-drawn backdrop material of a window, including behind the non-client area. 
    /// The pvAttribute parameter points to a value from the DWM_SYSTEMBACKDROP_TYPE enumeration.
    /// </summary>
    DWMWA_SYSTEMBACKDROP_TYPE,

    /// <summary>
    /// The maximum value of this enumeration for validation purposes.
    /// </summary>
    DWMWA_LAST
};
