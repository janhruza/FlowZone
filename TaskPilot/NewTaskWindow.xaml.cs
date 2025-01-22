using System;
using System.Windows;
using TaskPilot.Core;

namespace TaskPilot;

/// <summary>
/// Representing a window with the add task functionality.
/// </summary>
public partial class NewTaskWindow : Window
{
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
    }

    /// <summary>
    /// Representing the associated task to this window.
    /// </summary>
    public TaskItem Task { get; }

    /// <summary>
    /// Shows the <see cref="NewTaskWindow"/> as a dialog and returns true if a new user-defined task was created, otherwise false.
    /// </summary>
    /// <returns>True if a new task is created, otherwise false.</returns>
    public static bool CreateTask()
    {
        NewTaskWindow window = new NewTaskWindow();
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
            App.Tasks.Add(this.Task);

            // close dialog
            DialogResult = true;
            this.Close();
        }
    }

    private void cbCanExpire_Checked(object sender, RoutedEventArgs e)
    {
        dtExpiration.SelectedDate = DateTime.MinValue;
        bdExpiration.Visibility = Visibility.Visible;
    }

    private void cbCanExpire_Unchecked(object sender, RoutedEventArgs e)
    {
        dtExpiration.SelectedDate = DateTime.MaxValue;
        bdExpiration.Visibility = Visibility.Collapsed;
    }
}
