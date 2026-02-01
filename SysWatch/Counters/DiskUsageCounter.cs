namespace SysWatch.Counters;

/// <summary>
/// Represents a performance counter that measures the percentage of time the physical disk is busy processing read or
/// write requests on the system.
/// </summary>
/// <remarks>This counter monitors the aggregate disk activity across all physical disks by tracking the '% Disk
/// Time' metric for the '_Total' instance. It is useful for identifying disk bottlenecks and assessing overall disk
/// utilization. The counter is typically used to monitor system performance and diagnose disk-related issues.</remarks>
public class DiskUsageCounter : Counter
{
    /// <summary>
    /// Initializes a new instance of the DiskUsageCounter class to monitor the percentage of time the physical disk is
    /// busy processing read or write requests.
    /// </summary>
    /// <remarks>This counter aggregates disk activity across all physical disks on the system. The sampling
    /// interval is set to 500 milliseconds by default.</remarks>
    public DiskUsageCounter() : base("PhysicalDisk", "% Disk Time", "_Total") { }
}
