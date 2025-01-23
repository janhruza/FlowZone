using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
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
            TextTrimming = TextTrimming.CharacterEllipsis,
            Inlines =
                {
                    new Run(task.Caption)
                    {
                        FontSize = 18
                    },

                    new LineBreak(),

                    new Run(string.IsNullOrEmpty(task.Text.Trim()) == false ? task.Text : Messages.NoTaskDescription)
                    {
                        FontSize = 14
                    },

                    new LineBreak(),

                    new Run("Expiration: ")
                    {
                        FontSize = 12
                    },

                    new Run(task.IsIndefinite ? "Indefinite" : task.ExpirationDate.ToShortDateString())
                    {
                        FontSize = 12,
                        Foreground = (task.IsIndefinite ? Brushes.LimeGreen : SystemColors.GrayTextBrush)
                    }
                }
        };

        ListBoxItem lbi = new ListBoxItem
        {
            Content = tb,
        };

        lbi.KeyDown += (s, e) =>
        {
            if (e.Key == System.Windows.Input.Key.Delete)
            {
                RemoveTask(task);
            }
        };

        // add context menu to the valid items
        // invalid items are those with id equal to long.MinValue
        if (task.Id != long.MinValue)
        {
            // context menu
            ContextMenu cm = new ContextMenu();
            lbi.ContextMenu = cm;

            // context menu items

            MenuItem miUpdate = new MenuItem
            {
                Header = "Modify"
            };

            miUpdate.Click += (s, e) =>
            {
                // modify task
                ModifyTask(ref task);
            };

            MenuItem miDelete = new MenuItem
            {
                Header = "Delete task",
                InputGestureText = "Del"
            };

            miDelete.Click += (s, e) =>
            {
                // confirm deleting
                RemoveTask(task);
            };

            // add items to the menu
            cm.Items.Add(miUpdate);
            cm.Items.Add(new Separator());
            cm.Items.Add(miDelete);
        }

        lbTasks.Items.Add(lbi);
    }

    private void RemoveTask(TaskItem task)
    {
        if (MessageBox.Show(Messages.ConfirmDeleteTask, $"Delete '{task.Caption}'", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            App.RemoveTask(task.Id);
        }
    }

    private void LoadTasks()
    {
        lbTasks.Items.Clear();

        if (App.Tasks.Count == 0)
        {
            // load default tasks
            TaskItem task1 = new TaskItem
            {
                Id = long.MinValue,
                Caption = "Add a new task",
                Text = "To add a new task, click the '+' button in the bottom-right corner!",
                CreationDate = DateTime.Now,
                ExpirationDate = DateTime.MaxValue
            };

            TaskItem task2 = new TaskItem
            {
                Id = long.MinValue,
                Caption = "Reload the task list",
                Text = "To reload the task list, click the middle button in the bottom-right corner.",
                CreationDate = DateTime.Now,
                ExpirationDate = DateTime.MaxValue
            };

            TaskItem task3 = new TaskItem
            {
                Id = long.MinValue,
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

    private void ModifyTask(ref TaskItem task)
    {
        if (NewTaskWindow.Modify(task) == true)
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

    private void miRefreshView_Click(object sender, RoutedEventArgs e)
    {
        LoadTasks();
    }
}