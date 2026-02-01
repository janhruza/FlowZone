namespace SysWatch.Counters;

/// <summary>
/// Representing a CPU counter.
/// </summary>
public class CPUCounter : Counter
{
    /// <summary>
    /// Initializes a new instance of the CPUCounter class and configures the timer interval for CPU usage monitoring.
    /// </summary>
    /// <remarks>The timer is set to trigger at 500-millisecond intervals, which determines how frequently CPU
    /// usage data is sampled. This constructor prepares the instance for use but does not start monitoring
    /// automatically.</remarks>
    public CPUCounter() : base("Processor", "% Processor Time", "_Total")
    {
    }
}
