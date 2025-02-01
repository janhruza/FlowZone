namespace PassFort.Core;

/// <summary>
/// Representing the enumeration of the password categories.
/// </summary>
public enum PasswordCategory : byte
{
    /// <summary>
    /// Represents entries that do not fit into any other category. This can be useful for entries that are uncategorized or when a user wants to specify no particular category.
    /// </summary>
    None = 0,

    /// <summary>
    /// For accounts related to social media platforms like Facebook, Twitter, Instagram, etc.
    /// </summary>
    SocialMedia,

    /// <summary>
    /// Accounts that require email login credentials.
    /// </summary>
    Email,

    /// <summary>
    /// Includes banking, credit cards, investments, and any financial services.
    /// </summary>
    Finance,

    /// <summary>
    /// Online shopping sites like Amazon, eBay, or specific retailer logins.
    /// </summary>
    Shopping,

    /// <summary>
    /// Accounts for online games or gaming platforms such as Steam, Xbox Live, PlayStation Network.
    /// </summary>
    Gaming,

    /// <summary>
    /// For work-related accounts, including company portals and professional networks.
    /// </summary>
    Work,

    /// <summary>
    /// Accounts related to educational institutions, learning management systems, etc.
    /// </summary>
    School,

    /// <summary>
    /// Services like electricity, water, internet providers, and any other utility services.
    /// </summary>
    Utilities,

    /// <summary>
    /// A catch-all category for entries that don't fit into the predefined categories.
    /// </summary>
    Other,

    /// <summary>
    /// Representing a category that respresenting all categories. Used for printing all password entries of all categories.
    /// </summary>
    All
}
