using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace UpDate;

/// <summary>
/// Representing a class with UpDate project settings.
/// </summary>
public class UpDateSettings
{
    #region Static methods
    private const string SETTINGS_PATH = "settings.json";

    /// <summary>
    /// Loads the settings from the settings file and if it fails, it will create a new instance of the <see cref="UpDateSettings"/> class.
    /// </summary>
    /// <returns>Loaded or created instance of the <see cref="UpDateSettings"/> class.</returns>
    public static UpDateSettings EnsureSettings()
    {
        string jsonData;
        UpDateSettings settings = new();

        if (File.Exists(SETTINGS_PATH) == false)
        {
            // create a new settings
            settings = new UpDateSettings();
            jsonData = JsonSerializer.Serialize<UpDateSettings>(settings);
            File.WriteAllText(SETTINGS_PATH, jsonData);
        }

        else
        {
            // get settings content
            jsonData = File.ReadAllText(SETTINGS_PATH);

            // parse file data
            var set = JsonSerializer.Deserialize<UpDateSettings>(jsonData);

            if (set == null)
            {
                // if parsing fails, return default settings
                return settings;
            }

            // set settings if read settings aren't null
            settings = set;
        }

        // return settings
        return settings;
    }

    /// <summary>
    /// Representing the currently loaded instance of the <see cref="UpDateSettings"/> class.
    /// </summary>
    public static UpDateSettings? Current { get; set; }

    /// <summary>
    /// Representing the list of default RSS feeds.
    /// </summary>
    /// <returns>A new list of default RSS feeds.</returns>
    public static List<string> GetDefaultFeeds()
    {
        return
        [
            "https://www.novinky.cz/rss",
            "https://ct24.ceskatelevize.cz/rss",
            "https://www.ceskatelevize.cz/sport/rss/vsechny-zpravy/",
            "https://servis.idnes.cz/rss.aspx?c=zpravodaj",
            "https://servis.idnes.cz/rss.aspx?c=sport",
            "https://servis.idnes.cz/rss.aspx?c=ekonomikah",
            "https://www.irishtimes.com/arc/outboundfeeds/sitemap-news-index/latest/"
        ];
    }

    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="UpDateSettings"/> class.
    /// </summary>
    public UpDateSettings()
    {
        WindowSize = new Size(640, 400);
        Title = App.AppTitle;
        Feeds = GetDefaultFeeds();
        ThemeMode = FZCore.FZThemeMode.System;
    }

    /// <summary>
    /// Representing the main window size.
    /// </summary>
    public Size WindowSize { get; set; }

    /// <summary>
    /// Representing the main window default title (title when the window is loaded).
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Representing a list of saved RSS feed sources.
    /// </summary>
    public List<string> Feeds { get; set; }

    /// <summary>
    /// Representing the app theme mode.
    /// </summary>
    public FZCore.FZThemeMode ThemeMode { get; set; }

    /// <summary>
    /// Saves the settings object into the specified <paramref name="savePath"/>.
    /// </summary>
    /// <param name="savePath">Path to the settings file that will be created or overwritten.</param>
    public void Save(string savePath)
    {
        string data = JsonSerializer.Serialize<UpDateSettings>(this);
        File.WriteAllText(savePath, data);
        return;
    }

    /// <summary>
    /// Saves the settings object into the default settings file location.
    /// </summary>
    public void SaveAsDefault()
    {
        Save(SETTINGS_PATH);
        return;
    }
}
