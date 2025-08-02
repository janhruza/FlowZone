using System;

namespace Expando.Core;

/// <summary>
/// Representing a single transaction object.
/// </summary>
public struct Transaction
{
    /// <summary>
    /// Representing the transaction identification number.
    /// </summary>
    public ulong Id { get; set; }

    /// <summary>
    /// Representing a user identification number of the corresponding user.
    /// </summary>
    public ulong UserId { get; set; }

    /// <summary>
    /// Representing the transaction's time stamp. This value is obtained by <see cref="GetTimestamp(DateTime)"/>.
    /// </summary>
    public long Timestamp { get; set; }

    /// <summary>
    /// Representing a transaction type. Values defined as <see cref="TypeExpanse"/> and <see cref="TypeIncome"/>.
    /// </summary>
    public bool Type { get; set; }

    /// <summary>
    /// Representing the value of the transaction.
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// Representing the user-defined description of this transaction.
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Representing the transaction category.
    /// </summary>
    public string Category { get; set; }

    #region Static data

    /// <summary>
    /// Representing the expanse transaction type.
    /// </summary>
    public const bool TypeExpanse = false;

    /// <summary>
    /// Representing the income transaction type.
    /// </summary>
    public const bool TypeIncome = true;

    /// <summary>
    /// Gets the timestamp as <see cref="long"/> from the given <paramref name="dateTime"/>.
    /// </summary>
    /// <param name="dateTime">A date and time to be parsed. If using in new transaction creation, use <see cref="DateTime.Now"/> or <see cref="DateTime.UtcNow"/>.</param>
    /// <returns>Returns a <paramref name="dateTime"/> representation using the <see cref="DateTime.ToBinary()"/> method.</returns>
    public static long GetTimestamp(DateTime dateTime)
    {
        return dateTime.ToBinary();
    }

    #endregion
}
