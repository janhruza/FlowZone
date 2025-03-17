using System;
using System.Collections.Generic;

namespace TaskPilot.Core;

/// <summary>
/// Representing a single <see cref="TaskItem"/> object.
/// </summary>
public class TaskItem
{
    /// <summary>
    /// Creates a new instance of the <see cref="TaskItem"/> with the default values.
    /// </summary>
    public TaskItem()
    {
        Id = DateTimeOffset.Now.ToUnixTimeSeconds();
        Caption = string.Empty;
        Text = string.Empty;
        CreationDate = DateTime.Now;
        ExpirationDate = DateTime.MaxValue;
    }

    /// <summary>
    /// Representing the task identification number.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Representing the title of the task.
    /// </summary>
    public string Caption { get; set; }

    /// <summary>
    /// Representing the description text of the task.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Representing the expiration date and time of the task. If you want to set the task without the expiration date, set this value to <see cref="DateTime.MaxValue"/>.
    /// </summary>
    public DateTime ExpirationDate { get; set; }

    /// <summary>
    /// Representing the date and time when the task was created.
    /// </summary>
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// Determines whether the task has an expiration date or is set to <see cref="DateTime.MaxValue"/> which indicates indefinite task.
    /// </summary>
    public bool IsIndefinite => ExpirationDate == DateTime.MaxValue;
}

/// <summary>
/// Representing a collection of <see cref="TaskItem"/>s as <see cref="List{TaskItem}"/>.
/// </summary>
public class TasksCollection : List<TaskItem>;