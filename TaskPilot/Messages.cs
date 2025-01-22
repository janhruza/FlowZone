namespace TaskPilot;

/// <summary>
/// Representing a data class with in app messages.
/// </summary>
public static class Messages
{
    /// <summary>
    /// Invalid task data. Please make sure the task caption is valid.
    /// </summary>
    public const string CreateTaskInvalidData = "Invalid task data. Please make sure the task caption is valid.";

    /// <summary>
    /// There was an error while saving your tasks.
    /// </summary>
    public const string TasksSavingFailed = "There was an error while saving your tasks.";

    /// <summary>
    /// Do you want to delete this task?
    /// </summary>
    public const string ConfirmDeleteTask = "Do you want to delete this task?";

    /// <summary>
    /// No task description.
    /// </summary>
    public const string NoTaskDescription = "No task description.";
}
