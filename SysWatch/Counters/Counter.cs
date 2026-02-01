using FZCore;

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SysWatch.Counters;

/// <summary>
/// Represents a counter that periodically retrieves performance data using a specified category, counter, and instance
/// name.
/// </summary>
/// <remarks>Use the Counter class to monitor system or application performance metrics at regular intervals. The
/// counter raises the ValueObtained event each time a new value is retrieved. This class is typically used for
/// scenarios where real-time or periodic performance monitoring is required.</remarks>
public class Counter : ICounter
{
    protected PerformanceCounter _counter;

    /// <inheritdoc />
    public event EventHandler<float> ValueObtained = delegate { };

    /// <summary>
    /// Initializes a new instance of the Counter class for the specified performance counter category, counter, and
    /// instance.
    /// </summary>
    /// <remarks>Use this constructor to create a Counter object that targets a specific performance counter.
    /// Ensure that the specified category, counter, and instance exist on the system; otherwise, an exception may be
    /// thrown when accessing the counter.</remarks>
    /// <param name="categoryName">The name of the performance counter category. Cannot be null or empty.</param>
    /// <param name="counterName">The name of the performance counter within the specified category. Cannot be null or empty.</param>
    /// <param name="instanceName">The name of the performance counter instance. Specify an empty string for single-instance counters.</param>
    public Counter(string categoryName, string counterName, string instanceName)
    {
        _counter = new PerformanceCounter(categoryName, counterName, instanceName);
        try { _ = _counter.NextValue(); } catch { }
    }

    /// <inheritdoc />
    public bool IsActive { get; set; }

    /// <summary>
    /// Enables the component, allowing it to begin operation.
    /// </summary>
    public void Start() => IsActive = true;

    /// <summary>
    /// Disables the current instance, stopping any ongoing activity or processing.
    /// </summary>
    public void Stop() => IsActive = false;

    /// <summary>
    /// Gets the latest value from the counter.
    /// </summary>
    public virtual async Task Update() // must be virtual
    {
        try
        {
            if (!IsActive) return;

            float value = _counter.NextValue();
            ValueObtained.Invoke(this, value);
        }
        catch (Exception ex)
        {
            Log.Error(ex, nameof(Update));
        }
    }
}
