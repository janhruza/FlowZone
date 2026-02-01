using System;
using System.Diagnostics;
using System.Windows.Threading;

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
    private DispatcherTimer _timer;
    private PerformanceCounter _counter;

    /// <inheritdoc />
    public event EventHandler<float> ValueObtained = delegate { };

    /// <inheritdoc />
    public void Start()
    {
        _timer.Start();
    }

    /// <inheritdoc />
    public void Stop()
    {
        _timer.Stop();
    }

    private void _timer_Tick(object? sender, EventArgs e)
    {
        float value = _counter.NextValue();
        this.ValueObtained.Invoke(this, value);
    }

    /// <summary>
    /// Initializes a new instance of the Counter class for monitoring a specific performance counter instance.
    /// </summary>
    /// <remarks>This constructor sets up the Counter to periodically sample the specified performance
    /// counter. Ensure that the provided category, counter, and instance names correspond to valid performance counters
    /// on the system.</remarks>
    /// <param name="categoryName">The name of the performance counter category to monitor. Cannot be null or empty.</param>
    /// <param name="counterName">The name of the performance counter within the specified category. Cannot be null or empty.</param>
    /// <param name="instanceName">The name of the instance of the performance counter to monitor. Use an empty string for the default instance.</param>
    /// <param name="interval">The interval of the timer.</param>
    public Counter(string categoryName, string counterName, string instanceName, int interval)
    {
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(interval)
        };

        _counter = new PerformanceCounter(categoryName, counterName, instanceName);
        _timer.Tick += _timer_Tick;
    }
}
