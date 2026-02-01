using System;
using System.Collections.Generic;

namespace FZCore;

/// <summary>
/// Representing the customized <see cref="Object"/> class with few more base values.
/// </summary>
public class FZObject
{
    /// <summary>
    /// Initializes a new <see cref="FZObject"/>.
    /// </summary>
    public FZObject()
    {
        Id = Guid.CreateVersion7();
        Timestamp = DateTime.Now.ToBinary();

        _objects.Add(this);
    }

    /// <summary>
    /// Representing the base destructor.
    /// </summary>
    ~FZObject()
    {
        _ = _objects.Remove(this);
    }

    /// <summary>
    /// Representing the object's ID.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Representing the date and time (in binary) when the object was created.
    /// </summary>
    public long Timestamp { get; }

    #region Static methods and members

    private static List<FZObject> _objects { get; } = [];

    /// <summary>
    /// Representing a list of all currently initialized <see cref="FZObject"/>s.
    /// </summary>
    public static List<FZObject> All => _objects;

    #endregion
}
