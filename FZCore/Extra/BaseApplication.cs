using FZCore.Windows;

using System.Windows;

namespace FZCore.Extra;

/// <summary>
/// Representing a custom <see cref="Application"/> class.
/// </summary>
public class BaseApplication : Application
{
    /// <summary>
    /// Representing the main application window.
    /// </summary>
    public new IconlessWindow MainWindow { get; set; }
}
