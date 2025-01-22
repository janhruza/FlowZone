using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using TaskPilot.Core;

namespace TaskPilot;

/// <summary>
/// Representing the main window class.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// Creates a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        // default init
        InitializeComponent();

        // load tasks
        LoadTasks();
    }

    private void DisplayTask(TaskItem task)
    {
        TextBlock tb = new TextBlock
        {
            Inlines =
                {
                    new Run(task.Caption)
                    {
                        FontSize = 18
                    },

                    new LineBreak(),

                    new Run(task.Text)
                    {
                        FontSize = 14
                    }
                }
        };

        ListBoxItem lbi = new ListBoxItem
        {
            Content = tb,
        };

        lbTasks.Items.Add(lbi);
    }

    private void LoadTasks()
    {
        lbTasks.Items.Clear();

        if (App.Tasks.Count == 0)
        {
            // load default tasks
            TaskItem task1 = new TaskItem
            {
                Id = 0,
                Caption = "Add a new task",
                Text = "To add a new task, click the '+' button in the bottom-right corner!",
                CreationDate = DateTime.Now,
                ExpirationDate = DateTime.MaxValue
            };

            TaskItem task2 = new TaskItem
            {
                Id = 1,
                Caption = "Reload the task list",
                Text = "To reload the task list, click the middle button in the bottom-right corner.",
                CreationDate = DateTime.Now,
                ExpirationDate = DateTime.MaxValue
            };

            TaskItem task3 = new TaskItem
            {
                Id = 2,
                Caption = "Export your tasks",
                Text = "To export your tasks, click the left button in the bottom-right corner.",
                CreationDate = DateTime.Now,
                ExpirationDate = DateTime.MaxValue
            };

            DisplayTask(task1);
            DisplayTask(task2);
            DisplayTask(task3);
            return;
        }

        foreach (TaskItem item in App.Tasks)
        {
            DisplayTask(item);
        }
    }

    private void GetNewTask()
    {
        if (NewTaskWindow.CreateTask() == true)
        {
            LoadTasks();
        }
    }

    private void SaveTasks()
    {
        TasksCollection tasks = App.Tasks;
        if (App.SaveTasks(App.TasksFileLocation, ref tasks) == false)
        {
            // unabke to save tasks
            _ = MessageBox.Show(Messages.TasksSavingFailed, "Saving failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return;
    }

    private void btnRefresh_Click(object sender, RoutedEventArgs e)
    {
        LoadTasks();
    }

    private void btnNewTask_Click(object sender, RoutedEventArgs e)
    {
        GetNewTask();
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
        SaveTasks();
    }
}