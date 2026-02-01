using System;

namespace SysWatch.Counters;

/// <summary>
/// Defines the contract for a counter that can be implemented to track and manipulate a numeric value.
/// </summary>
/// <remarks>Implementations of this interface typically provide methods or properties to increment, decrement, or
/// retrieve the current count. This interface does not specify any particular behavior or thread safety guarantees;
/// those are determined by the implementing type.</remarks>
public interface ICounter
{
    /// <summary>
    /// Occurs when a new floating-point value is obtained.
    /// </summary>
    /// <remarks>The event provides the obtained value as the <see cref="System.Single"/> argument in the
    /// event data. Subscribers can use this event to react to value updates as they become available.</remarks>
    event EventHandler<float> ValueObtained;

    /// <summary>
    /// Initiates the operation or process represented by the current instance.
    /// </summary>
    void Update();

    /// <summary>
    /// Activates the counter.
    /// </summary>
    void Start();

    /// <summary>
    /// Deactivates the timer.
    /// </summary>
    void Stop();   // Nastaví IsActive = false

    /// <summary>
    /// Determines whether the counter is active or not.
    /// </summary>
    bool IsActive { get; }
}
