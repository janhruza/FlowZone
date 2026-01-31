using System;

namespace Exchange;

/// <summary>
/// Representing the core class.
/// </summary>
public static class Core
{
    /// <summary>
    /// Representing a link to the current exchange rates.
    /// </summary>
    public const string ExchangeRatesUrl = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";

    /// <summary>
    /// Representing a template link with definitions for a specific date.
    /// </summary>
    private const string ExchangeRatesWithDateUrl = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt?date={0}.{1}.{2}";

    /// <summary>
    /// Constructs a link to the exchange rates dating to the specified <paramref name="date"/>.
    /// </summary>
    /// <param name="date">Historic date to which the exchamge rates should be fetched.</param>
    /// <returns>A link to the exchange rates with the date specified.</returns>
    public static string GetExchangeRatesHistoryUrl(DateOnly date)
    {
        return string.Format(ExchangeRatesWithDateUrl, date.Day, date.Month, date.Year);
    }
}
