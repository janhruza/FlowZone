namespace RipTide.Core;

/// <summary>
/// Representing the enumeration of all valid browsers that can be used to use existing cookies to download videos (e. g, to bypass logins, etc.).
/// </summary>
public enum CookiesBrowser
{
    /// <summary>
    /// No browser. Will not use any browser's cookies.
    /// </summary>
    None = 0,

    /// <summary>
    /// Brave Browser
    /// </summary>
    Brave,

    /// <summary>
    /// Google Chrome web browser.
    /// </summary>
    Chrome,

    /// <summary>
    /// Chromium web browser.
    /// </summary>
    Chromium,

    /// <summary>
    /// Microsoft Edge web browser.
    /// </summary>
    Edge,

    /// <summary>
    /// Mozilla Firefox web browser.
    /// </summary>
    Firefox,

    /// <summary>
    /// Opera web browser.
    /// </summary>
    Opera,

    /// <summary>
    /// Apple Safari web browser.
    /// </summary>
    Safari,

    /// <summary>
    /// Vivaldi web browser.
    /// </summary>
    Vivaldi,

    /// <summary>
    /// Whale web browser.
    /// </summary>
    Whale
}
