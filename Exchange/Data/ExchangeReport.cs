using FZCore;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Data;

/// <summary>
/// Representing a single currency exchange report.
/// </summary>
public struct ExchangeReport
{
    /// <summary>
    /// Representing the additional identifier.
    /// </summary>
    public int Id;

    /// <summary>
    /// Representing the date of the rate.
    /// </summary>
    public DateOnly Date;

    /// <summary>
    /// Representing the list of available currencies.
    /// </summary>
    public List<CurrencyInfo> Currencies;

    #region Static code

    internal static string ReportsPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");

    private static DateOnly? ParseReportDate(string dateText)
    {
        try
        {
            dateText = dateText.Trim();
            if (string.IsNullOrWhiteSpace(dateText))
            {
                Log.Error("Input text is empty.", nameof(ParseReportDate));
                return null;
            }

            string[] parts = dateText.Split('.');
            int day = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int year = int.Parse(parts[2]);

            return new DateOnly(year, month, day);
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(ParseReportDate));
            return null;
        }
    }

    private static int? ParseReportId(string reportIdText)
    {
        try
        {
            reportIdText = reportIdText.Trim();
            if (string.IsNullOrEmpty(reportIdText)) return null;

            // first character '#' is ignored
            string text = reportIdText[1..];

            int id = int.Parse(text);
            return id;
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(ParseReportId));
            return null;
        }
    }

    private static string HashText(string text)
    {
        byte[] data = Encoding.UTF8.GetBytes(text);
        byte[] hashedData = SHA1.HashData(data);
        return Convert.ToHexString(hashedData).ToUpper();
    }

    internal static async Task<bool> SaveReport(string reportData)
    {
        if (Directory.Exists(ReportsPath) == false)
        {
            _ = Directory.CreateDirectory(ReportsPath);
        }

        string hash = HashText(reportData);
        string filename = Path.Combine(ReportsPath, $"{hash}.txt");

        if (ReportExists(hash) == false)
        {
            File.WriteAllText(filename, reportData, Encoding.UTF8);
            return true;
        }

        return false;
    }

    internal static async Task<ExchangeReport?> LoadReport(string path)
    {
        if (File.Exists(path) == false)
        {
            Log.Error($"File \'{path}\' not found.");
            return null;
        }

        string data = await File.ReadAllTextAsync(path);
        return await TryParse(data);
    }

    internal static bool ReportExists(string hash)
    {
        string path = Path.Combine(ReportsPath, $"{hash}.txt");
        return File.Exists(path);
    }

    /// <summary>
    /// Attempts to parse the exchange report data.
    /// </summary>
    /// <param name="data">Input report data.</param>
    /// <returns>Parsed report or <see langword="null"/> if an error occurred.</returns>
    public static async Task<ExchangeReport?> TryParse(string data)
    {
        try
        {
            string[] lines = data.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length <= 2)
            {
                Log.Error($"The response is empty or invalid.", nameof(TryParse));
                return null;
            }

            // get the metadata (first two lines)
            string[] metadata = lines[0].Split(' ');
            ExchangeReport report = new ExchangeReport();
            report.Date = ParseReportDate(metadata[0]) ?? new DateOnly();
            report.Id = ParseReportId(metadata[1]) ?? int.MinValue;

            List<CurrencyInfo> currencies = [];

            // get the currency infos
            for (int i = 2; i < lines.Length; i++)
            {
                if (CurrencyInfo.TryParse(lines[i], out CurrencyInfo currencyInfo) == true)
                {
                    currencies.Add(currencyInfo);
                }
            }

            // add the CZK currency
            currencies.Add(new CurrencyInfo
            {
                Code = "CZK",
                Amount = 1,
                Rate = 1,
                Country = "Česko",
                Currency = "koruna"
            });

            report.Currencies = [.. currencies.OrderBy(x => x.Currency)];
            LatestReport = report; // set the latest processed report value
            return report;
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(TryParse));
            return null;
        }
    }

    /// <summary>
    /// Attempts to fetch a currency report.
    /// </summary>
    /// <param name="date">Specified date or <see langword="null"/>. If this field is <see langword="null"/>, the current report will be fetched.</param>
    /// <returns>Fetched report or <see langword="null"/> if an error occurred.</returns>
    public static async Task<ExchangeReport?> FetchAsync(DateOnly? date)
    {
        try
        {
            string url = date.HasValue switch
            {
                true => Core.GetExchangeRatesHistoryUrl(date.Value),
                false => Core.ExchangeRatesUrl,
            };

            HttpResponseMessage response = await Core.HttpClient.GetAsync(url);
            if (response.IsSuccessStatusCode == false)
            {
                Log.Error($"Get request failed. Reason: {response.ReasonPhrase ?? "Unknown"} - status code: {(int)response.StatusCode}", nameof(FetchAsync));
                return null;
            }

            // gets the message content
            string data = await response.Content.ReadAsStringAsync();
            _ = await SaveReport(data);
            return await TryParse(data);
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(FetchAsync));
            return null;
        }
    }

    /// <summary>
    /// Attempts to fetch the current currency report.
    /// </summary>
    /// <returns>Fetched report or <see langword="null"/> if an error occurred.</returns>
    public static async Task<ExchangeReport?> FetchAsync()
    {
        return await FetchAsync(null);
    }

    /// <summary>
    /// Representing the last fetched report.
    /// </summary>
    public static ExchangeReport? LatestReport;

    #endregion
}
