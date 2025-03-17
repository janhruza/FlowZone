namespace HashHive;

/// <summary>
/// Representing all the valid pages of the HashHive project.
/// </summary>
public enum HashHivePage
{
    /// <summary>
    /// Representing the <see cref="Pages.PgTextHash"/> page.
    /// </summary>
    HashText        = 0,

    /// <summary>
    /// 
    /// </summary>
    HashFile        = 1,

    /// <summary>
    /// 
    /// </summary>
    CompareHashes   = 2,

    /// <summary>
    /// Representing a blank page (no content at all).
    /// </summary>
    Null            = 0xFF
}
