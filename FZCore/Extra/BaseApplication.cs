using FZCore.Windows;

using System;
using System.Diagnostics;
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

    /// <summary>
    /// Representing the application theme.
    /// </summary>
    /// <remarks>
    /// This property is new and it's hiding the base <see cref="Application.ThemeMode"/> property.
    /// </remarks>
    public new FZThemeMode ThemeMode
    {
        get => field;
        set
        {
            Core.SetApplicationTheme(value);
            field = value;
        }
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Log.AppStarted();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        Log.AppExited();
    }
}
