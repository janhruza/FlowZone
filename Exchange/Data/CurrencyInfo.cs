using FZCore;

using System;

namespace Exchange.Data;

/// <summary>
/// Representing a single currency information.
/// </summary>
public struct CurrencyInfo
{
    /// <summary>
    /// Representing the counry name (in Czech).
    /// </summary>
    public string Country;

    /// <summary>
    /// Representing the name of the currency (in Czech).
    /// </summary>
    public string Currency;

    /// <summary>
    /// Representing the amount of the currency.
    /// </summary>
    public int Amount;

    /// <summary>
    /// Representing the currency code.
    /// </summary>
    public string Code;

    /// <summary>
    /// Representing the currency rate in CZK per <see cref="Amount"/> units.
    /// </summary>
    public decimal Rate;

    #region Static code

    /// <summary>
    /// Attempts to parse the <paramref name="result"/> from the given <paramref name="data"/>.
    /// </summary>
    /// <param name="data">Input data.</param>
    /// <param name="result">Output result.</param>
    /// <returns>Operation result.</returns>
    public static bool TryParse(string data, out CurrencyInfo result)
    {
        result = new CurrencyInfo();

        try
        {
            string[] parts = data.Split(';', StringSplitOptions.TrimEntries);

            result.Country = parts[0];
            result.Currency = parts[1];
            result.Amount = int.Parse(parts[2]);
            result.Code = parts[3];
            result.Rate = decimal.Parse(parts[4]);

            return true;
        }

        catch (Exception ex)
        {
            Log.Error(ex);
            return false;
        }
    }

    #endregion
}
