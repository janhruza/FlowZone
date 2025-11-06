using FZCore;

using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace RipTide.Core;

/// <summary>
/// Representing the enumeration of available application styles.
/// </summary>
public enum ThemeMode : byte
{
    /// <summary>
    /// Representing the system theme mode. This mode automatically decides whether to use the
    /// <see cref="Light"/> or <see cref="Dark"/> mode based on your operating system configuration.
    /// </summary>
    System = 0,

    /// <summary>
    /// Representing the light theme mode.
    /// </summary>
    Light = 1,

    /// <summary>
    /// Representing the dark theme mode.
    /// </summary>
    Dark = 2,

    /// <summary>
    /// No Windows Fluent theme.
    /// </summary>
    None = 3
}

/// <summary>
/// Representing the settings class for the RipTide project.
/// </summary>
public class RTSettings
{
    /// <summary>
    /// Creates a new instance of the <see cref="RTSettings"/> class.
    /// </summary>
    public RTSettings()
    {
        ThemeMode = ThemeMode.System;
        WindowPosition = new Vector(0, 0);
        AlwaysOnTop = false;
    }

    static RTSettings()
    {
        // ensures the existence of current settings
        s_Settings = new RTSettings();
        EnsureSettings();
    }

    #region Settings properties

    /// <summary>
    /// Representing the desired application theme mode.
    /// </summary>
    public ThemeMode ThemeMode { get; set; }

    /// <summary>
    /// Representing the startup position of the main window.
    /// </summary>
    public Vector WindowPosition { get; set; }

    /// <summary>
    /// Representing whether the main window should be always visible over all other windows.
    /// </summary>
    public bool AlwaysOnTop { get; set; }

    /// <summary>
    /// Representing the custom downloader path.
    /// </summary>
    public string CustomDownloaderPath { get; set; } = string.Empty;

    #endregion

    #region Static code

    private static string s_DefaultPath => "settings.json";

    private static RTSettings s_Settings;

    /// <summary>
    /// Gets the current instance of the <see cref="RTSettings"/> class.
    /// </summary>
    public static RTSettings Current => s_Settings;

    /// <summary>
    /// Attempts to save the <paramref name="settings"/> into the given file at <paramref name="settingsPath"/>.
    /// </summary>
    /// <param name="settingsPath">Path to the settings file.</param>
    /// <param name="settings">Settings object to be saved.</param>
    /// <returns>True, if the save is successful, otherwise false.</returns>
    public static bool Save(string settingsPath, RTSettings? settings)
    {
        try
        {
            if (settings == null)
            {
                Log.Error($"Unable to save the given settings because the settings object is null.", nameof(Save));
                return false;
            }

            // get and write JSON structured data into a file
            string data = JsonSerializer.Serialize<RTSettings>(settings);
            File.WriteAllText(settingsPath, data, Encoding.UTF8);

            return true;
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(Save));
            return false;
        }
    }

    /// <summary>
    /// Attempts to load the existing settings from the given file at <paramref name="settingsPath"/>.
    /// </summary>
    /// <param name="settingsPath">Path to the settings file.</param>
    /// <param name="settings">Where to store the loaded settings to.</param>
    /// <returns>True, if load was successful, otherwise false.</returns>
    public static bool Load(string settingsPath, ref RTSettings settings)
    {
        try
        {
            if (File.Exists(settingsPath) == false)
            {
                Log.Error($"Settings file '{settingsPath}' was not found.", nameof(Load));
                return false;
            }

            // load JSON structured data and deserialize them
            string data = File.ReadAllText(settingsPath, Encoding.UTF8);
            RTSettings? set = (RTSettings?)JsonSerializer.Deserialize<RTSettings>(data);

            if (set != null)
            {
                settings = set;
            }

            if (settings == null)
            {
                Log.Error($"Unable to parse the settings file. Path: {settingsPath}", nameof(Load));
                return false;
            }

            return true;
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(Load));
            return false;
        }
    }

    /// <summary>
    /// Loads the settings from the default settings file.
    /// </summary>
    /// <returns>True, if the load is successful, otherwise false.</returns>
    public static bool LoadCurrent()
    {
        return Load(s_DefaultPath, ref s_Settings);
    }

    /// <summary>
    /// Saves the current settings into the default settings file.
    /// </summary>
    /// <returns>True, if the save is successful, otherwise false.</returns>
    public static bool SaveCurrent()
    {
        return Save(s_DefaultPath, s_Settings);
    }

    /// <summary>
    /// Ensures the existence of the default settings file and loads the settings into current.
    /// </summary>
    /// <returns>True, if the default settings exists, otherwise false.</returns>
    private static bool EnsureSettings()
    {
        if (File.Exists(s_DefaultPath) == false)
        {
            s_Settings ??= new RTSettings();
            return SaveCurrent();
        }

        else
        {
            return LoadCurrent();
        }
    }

    #endregion
}
