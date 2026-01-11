using System.Runtime.InteropServices;

namespace FZCore.Win32;

/// <summary>
/// Defines the width and height margins of a window for use with window composition APIs.
/// </summary>
/// <remarks>The MARGINS structure is typically used with functions such as DwmExtendFrameIntoClientArea to
/// specify the distances, in pixels, to extend the window frame into the client area on each side. All values are
/// measured in device pixels and can be set independently. Setting a value to -1 typically indicates that the entire
/// corresponding dimension should be extended.</remarks>
[StructLayout(LayoutKind.Sequential)]
public struct MARGINS
{
    /// <summary>
    /// Specifies the width, in pixels, of the left edge of a rectangle or window frame.
    /// </summary>
    public int cxLeftWidth;

    /// <summary>
    /// Specifies the width, in pixels, of the right edge of a rectangle or window frame.
    /// </summary>
    public int cxRightWidth;

    /// <summary>
    /// Gets or sets the height, in pixels, of the top border or area.
    /// </summary>
    public int cyTopHeight;

    /// <summary>
    /// Gets or sets the height, in pixels, of the bottom area.
    /// </summary>
    public int cyBottomHeight;
}
