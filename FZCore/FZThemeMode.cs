namespace FZCore;

/// <summary>
/// Representing an enumeration of the avalable WPF themes.
/// </summary>
public enum FZThemeMode : byte
{
    /// <summary>
    /// No custom theme.
    /// </summary>
    None = 0,

    /// <summary>
    /// Light Fluent theme.
    /// </summary>
    Light = 1,

    /// <summary>
    /// Dark Fluent theme.
    /// </summary>
    Dark = 2,

    /// <summary>
    /// Representing the system theme, either <see cref="Light"/> or <see cref="Dark"/> based on your operating system configuration.
    /// </summary>
    System = 3
}
