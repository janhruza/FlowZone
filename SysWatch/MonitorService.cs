using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

using SysWatch.Counters;

namespace SysWatch;

/// <summary>
/// Provides static methods to manage and coordinate periodic updates for registered counters using a shared timer.
/// </summary>
/// <remarks>The MonitorService class enables centralized scheduling of update operations for multiple ICounter
/// implementations. Registered counters are updated at regular intervals determined by the internal timer. This class
/// is thread-safe for registering counters and controlling the timer. All members are static; instantiation is not
/// required.</remarks>
public static class MonitorService
{
    private static readonly DispatcherTimer _masterTimer;
    private static readonly List<ICounter> _activeCounters = new();

    static MonitorService()
    {
        _masterTimer = new DispatcherTimer(DispatcherPriority.Background);
        _masterTimer.Interval = TimeSpan.FromMilliseconds(1000);
        _masterTimer.Tick += _masterTimer_Tick;
    }

    private static async void _masterTimer_Tick(object? sender, EventArgs e)
    {
        //foreach (var counter in _activeCounters)
        //{
        //    await counter.Update();
        //}
        var tasks = _activeCounters.Select(c => Task.Run(() => c.Update()));
        await Task.WhenAll(tasks);
    }

    /// <summary>
    /// Registers the specified counter to receive updates from the system.
    /// </summary>
    /// <param name="counter">The counter to register. Cannot be null.</param>
    public static void Register(ICounter counter) => _activeCounters.Add(counter);

    /// <summary>
    /// Registers the specified counter from receiving updates from the system.
    /// </summary>
    /// <param name="counter">The counter to register. Cannot be null.</param>
    public static void Unregister(ICounter counter)
    {
        if (_activeCounters.Contains(counter) == true)
        {
            _ = _activeCounters.Remove(counter);
        }
    }

    /// <summary>
    /// Starts the master timer if it is not already running.
    /// </summary>
    /// <remarks>Calling this method has no effect if the timer is already running. Use this method to begin
    /// timer-based operations managed by the master timer.</remarks>
    public static void Start()
    {
        _masterTimer.Start();

        // notifies the timers
        foreach (var counter in _activeCounters)
        {
            counter.Start();
        }
    }

    /// <summary>
    /// Stops the master timer if it is currently running.
    /// </summary>
    /// <remarks>Calling this method has no effect if the timer is already stopped. Use this method to pause
    /// or halt timer-based operations managed by the master timer.</remarks>
    public static void Stop()
    {
        _masterTimer.Stop();

        // notifies the timers
        foreach (var counter in _activeCounters)
        {
            counter.Stop();
        }
    }
}