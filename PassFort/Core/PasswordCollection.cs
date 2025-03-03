using System.Collections.Generic;

namespace PassFort.Core;

/// <summary>
/// Representing a <see cref="List{T}"/> of <see cref="PasswordEntry"/> items.
/// </summary>
public class PasswordCollection : List<PasswordEntry>
{
    /// <summary>
    /// Creates a new list of type <see cref="PasswordEntry"/> with no items inside of it.
    /// </summary>
    public PasswordCollection()
    {
    }

    /// <summary>
    /// Creates a new collection of <see cref="PasswordEntry"/> and fills it's content with items from the <paramref name="collection"/>.
    /// </summary>
    /// <param name="collection"></param>
    public PasswordCollection(IEnumerable<PasswordEntry> collection) : base(collection)
    {
    }
}
