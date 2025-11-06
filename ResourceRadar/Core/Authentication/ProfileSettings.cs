using FZCore;

using System.Globalization;

namespace ResourceRadar.Core.Authentication;

/// <summary>
/// Representing the settngs class for the <see cref="UserProfile"/> class.
/// </summary>
public class ProfileSettings
{
    /// <summary>
    /// Creates a new instance of <see cref="ProfileSettings"/> class with default values.
    /// </summary>
    public ProfileSettings()
    {
        ThemeMode = FZThemeMode.System;
        CultureName = CultureInfo.CurrentCulture.Name;
    }

    /// <summary>
    /// Representing the preferred application theme.
    /// </summary>
    public FZThemeMode ThemeMode { get; set; }

    /// <summary>
    /// Representing the selected culture - sed currency, etc.
    /// </summary>
    public string CultureName { get; set; }
}
