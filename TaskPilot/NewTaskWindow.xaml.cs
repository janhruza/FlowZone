using System;
using System.Windows;
using TaskPilot.Core;

namespace TaskPilot;

/// <summary>
/// Representing a window with the add task functionality.
/// </summary>
public partial class NewTaskWindow : Window
{
    private bool _isNewTask;

    /// <summary>
    /// Creates a new instance of the <see cref="NewTaskWindow"/> class.
    /// </summary>
    public NewTaskWindow()
    {
        InitializeComponent();
        Task = new TaskItem();

        // default dtValue - indefinite expiration
        cbCanExpire.IsChecked = false;
        dtExpiration.SelectedDate = DateTime.MaxValue;

        // is new task
        _isNewTask = true;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="NewTaskWindow"/> class with a task specified. This is used to modify the existing task.
    /// </summary>
    /// <param name="task">The task to be modified.</param>
    public NewTaskWindow(TaskItem task)
    {
        InitializeComponent();
        Task = task;

        cbCanExpire.IsChecked = task.ExpirationDate != DateTime.MaxValue; // must be MaxValue to be indefinite
        dtExpiration.SelectedDate = task.ExpirationDate;

        // load fields data
        txtCaption.Text = task.Caption;
        txtDescription.Text = task.Text;

        // is not new task - already existing task is being modified
        _isNewTask = false;
    }

    /// <summary>
    /// Representing the associated task to this window.
    /// </summary>
    public TaskItem Task { get; }

    /// <summary>
    /// Shows the <see cref="NewTaskWindow"/> as a dialog and returns true if a new user-defined task was created, otherwise false.
    /// </summary>
    /// <param name="parent">Optional. Window owner.</param>
    /// <returns>True if a new task is created, otherwise false.</returns>
    public static bool CreateTask(Window? parent = null)
    {
        NewTaskWindow window = new NewTaskWindow
        {
            Owner = parent,
            WindowStartupLocation = (parent == null) ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner
        };

        return window.ShowDialog() == true;
    }

    /// <summary>
    /// Shows the <see cref="NewTaskWindow"/> as a dialog and returns true if a user-defined task was modified, otherwise false.
    /// </summary>
    /// <param name="task"></param>
    /// <param name="parent">Optional. Window owner.</param>
    /// <returns></returns>
    public static bool Modify(TaskItem task, Window? parent = null)
    {
        NewTaskWindow window = new NewTaskWindow(task)
        {
            Owner = parent,
            WindowStartupLocation = (parent == null) ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner
        };

        return window.ShowDialog() == true;
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        this.Close();
    }

    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
        // assign values
        Task.Caption = txtCaption.Text;                                         // mandatory
        Task.Text = txtDescription.Text;                                        // optional
        Task.ExpirationDate = dtExpiration.SelectedDate ?? DateTime.MinValue;   // mandatory but default case is handled

        // evaluate results
        if (string.IsNullOrEmpty(Task.Caption) || Task.ExpirationDate == DateTime.MinValue)
        {
            // invalid input
            _ = MessageBox.Show(Messages.CreateTaskInvalidData, "Invalid input data", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        else
        {
            // valid input, create task
            if (_isNewTask == true)
            {
                // create only if it's a new task
                App.Tasks.Add(this.Task);
            }

            // close dialog
            DialogResult = true;
            this.Close();
        }
    }

    private void cbCanExpire_Checked(object sender, RoutedEventArgs e)
    {
        dtExpiration.SelectedDate = DateTime.Now;
        bdExpiration.Visibility = Visibility.Visible;
    }

    private void cbCanExpire_Unchecked(object sender, RoutedEventArgs e)
    {
        dtExpiration.SelectedDate = DateTime.MaxValue;
        bdExpiration.Visibility = Visibility.Collapsed;
    }
}
