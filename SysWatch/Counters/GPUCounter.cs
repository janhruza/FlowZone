using FZCore;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Threading;

namespace SysWatch.Counters;

/// <summary>
/// Provides functionality to monitor and report GPU utilization percentage using Windows performance counters.
/// </summary>
/// <remarks>GPUCounter periodically samples the GPU Engine performance counters to calculate overall GPU usage.
/// The class raises the ValueObtained event with the current utilization percentage at regular intervals. GPU
/// performance counters may not be available on all systems; on unsupported systems, no usage values will be reported.
/// This class is not thread-safe and should be used from the UI thread when integrating with DispatcherTimer.</remarks>
public class GPUCounter : ICounter
{
    private DispatcherTimer _timer;
    private List<PerformanceCounter> _counters = new();

    /// <inheritdoc />
    public event EventHandler<float> ValueObtained = delegate { };

    /// <summary>
    /// Initializes a new instance of the GPUCounter class and sets up the timer for periodic GPU usage monitoring.
    /// </summary>
    /// <remarks>The timer is configured to trigger every second to update GPU usage statistics. This
    /// constructor prepares the instance for immediate use; no additional initialization is required before starting
    /// monitoring.</remarks>
    public GPUCounter()
    {
        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1000) };
        _timer.Tick += _timer_Tick;
        InitializeCounters();
    }

    private void InitializeCounters()
    {
        try
        {
            var category = new PerformanceCounterCategory("GPU Engine");
            var instances = category.GetInstanceNames()
                .Where(id => id.Contains("engtype_3D"));

            foreach (var instance in instances)
            {
                _counters.Add(new PerformanceCounter("GPU Engine", "Utilization Percentage", instance));
            }
        }

        catch (Exception ex) 
        {
            Log.Error(ex, nameof(InitializeCounters));
        }
    }

    /// <inheritdoc />
    public void Start() => _timer.Start();

    /// <inheritdoc />
    public void Stop() => _timer.Stop();

    private void _timer_Tick(object? sender, EventArgs e)
    {
        float totalUsage = 0;
        foreach (var counter in _counters)
        {
            try { totalUsage += counter.NextValue(); } catch { }
        }

        // multiple GPUs can cause results > 100%
        ValueObtained.Invoke(this, MathF.Min(totalUsage, 100));
    }
}