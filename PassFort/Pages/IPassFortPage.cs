namespace PassFort.Pages;

/// <summary>
/// Representing the interface for all app pages.
/// </summary>
public interface IPassFortPage
{
    /// <summary>
    /// Reloads the page UI.
    /// </summary>
    void ReloadUI();

    /// <summary>
    /// Locks the page UI on longer inactivity.
    /// </summary>
    void Lock();

    /// <summary>
    /// Unlocks the locked page UI.
    /// </summary>
    void Unlock();

    /// <summary>
    /// Determines whether the page is locked or not.
    /// </summary>
    bool IsLocked { get; }
}
