using System;
using System.Collections.Generic;
using FZCore;

namespace ResourceRadar.Core.Authentication;

/// <summary>
/// Representing a user profile class.
/// </summary>
public class UserProfile
{
    /// <summary>
    /// Creates a new instance of the <see cref="UserProfile"/> class.
    /// </summary>
    public UserProfile()
    {
        Name = Environment.UserName;
        Description = string.Empty;
        Settings = new ProfileSettings();
        Items = [];
    }

    /// <summary>
    /// Representing the user profile name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Representing the description of this profile.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Representing the profile settings.
    /// </summary>
    public ProfileSettings Settings { get; set; }

    /// <summary>
    /// Representing all user's stored inventory items.
    /// </summary>
    public List<InventoryItem> Items { get; set; }

    /// <summary>
    /// Logouts the currently loaded profile.
    /// </summary>
    /// <returns>Always returns true.</returns>
    public static bool Logout()
    {
        _current = null;
        Log.Success($"Active user was logged out.", nameof(Logout));
        return true;
    }

    /// <summary>
    /// Sets the currently loaded profile to <paramref name="profile"/>.
    /// </summary>
    /// <param name="profile">New active profile.</param>
    /// <returns>True, if the active profile has changed, otherwise false.</returns>
    public static bool Login(UserProfile? profile)
    {
        if (profile == null)
        {
            Log.Error("User profile is null.", nameof(Login));
            return false;
        }

        _current = profile;
        Log.Success($"User \'{profile.Name}\' was logged in.", nameof(Login));
        return true;
    }

    private static UserProfile? _current;

    /// <summary>
    /// Representing the user account in use (if any).
    /// </summary>
    public static UserProfile? Current => _current;
}
