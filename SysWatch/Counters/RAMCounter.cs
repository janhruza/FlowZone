namespace SysWatch.Counters;

/// <summary>
/// Represents a performance counter that measures the percentage of committed memory in use on the system.
/// </summary>
/// <remarks>Use this counter to monitor overall memory usage and assess how much of the system's physical and
/// virtual memory is currently committed. This can help identify memory pressure or potential resource constraints on
/// the machine.</remarks>
public class RAMCounter : Counter
{
    /// <summary>
    /// Initializes a new instance of the RAMCounter class to monitor the percentage of committed bytes in use for
    /// system memory.
    /// </summary>
    /// <remarks>This constructor configures the counter to track overall memory usage as a percentage of
    /// committed bytes. Use this instance to retrieve real-time memory utilization metrics for performance monitoring
    /// or diagnostics.</remarks>
    public RAMCounter() : base("Memory", "% Committed Bytes In Use", string.Empty, Counter.Interval) { }
}
