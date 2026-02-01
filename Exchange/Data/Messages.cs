namespace Exchange.Data;

/// <summary>
/// Representing various in-app messages.
/// </summary>
public static class Messages
{
    /// <summary>
    /// Represents an error message indicating that processing historical data has failed.
    /// </summary>
    public const string ProcessingHistoryFailed = "Unable to process historical data.";

    /// <summary>
    /// Represents the error message format used when a metric processing operation fails.
    /// </summary>
    /// <remarks>The message includes a format item for inserting the name or identifier of the metric that
    /// caused the error. Use with string formatting methods such as string.Format.</remarks>
    public const string HistoryMetricError = "An error has occurred when trying to process the following metric: {0}";

    /// <summary>
    /// Represents the message text indicating that a metric has been applied.
    /// </summary>
    public const string MetricApplied = "Metric applied.";
}
