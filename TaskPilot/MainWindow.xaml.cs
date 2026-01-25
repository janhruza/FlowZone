using FZCore.Windows;

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

using TaskPilot.Core;

namespace TaskPilot;

/// <summary>
/// Representing the main window class.
/// </summary>
public partial class MainWindow : IconlessWindow
{
    /// <summary>
    /// Creates a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        // default init
        InitializeComponent();
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

        lbi.KeyDown += async (s, e) =>
        {
            if (e.Key == System.Windows.Input.Key.Delete)
            {
                await RemoveTask(task);
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

            miUpdate.Click += async (s, e) =>
            {
                // modify task
                await ModifyTask(task);
            };

            MenuItem miDelete = new MenuItem
            {
                Header = "Delete task",
                InputGestureText = "Del"
            };

            miDelete.Click += async (s, e) =>
            {
                // confirm deleting
                await RemoveTask(task);
            };

            // add items to the menu
            cm.Items.Add(miUpdate);
            cm.Items.Add(new Separator());
            cm.Items.Add(miDelete);
        }

        lbTasks.Items.Add(lbi);
    }

    private async Task RemoveTask(TaskItem task)
    {
        if (MessageBox.Show(Messages.ConfirmDeleteTask, $"Delete task '{task.Caption}'", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            if (App.RemoveTask(task.Id) == true)
            {
                await LoadTasks();
            }
        }
    }

    private async Task LoadTasks()
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

    private async Task GetNewTask()
    {
        if (NewTaskWindow.CreateTask(this) == true)
        {
            await LoadTasks();
        }
    }

    private async Task ModifyTask(TaskItem task)
    {
        if (NewTaskWindow.Modify(task, this) == true)
        {
            await LoadTasks();
        }
    }

    private async Task<bool> SaveTasks()
    {
        TasksCollection tasks = App.Tasks;
        return App.SaveTasks(App.TasksFileLocation, ref tasks);
    }

    private async void btnRefresh_Click(object sender, RoutedEventArgs e)
    {
        await LoadTasks();
    }

    private async void btnNewTask_Click(object sender, RoutedEventArgs e)
    {
        await GetNewTask();
    }

    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
        if (await SaveTasks() == false)
        {
            _ = MessageBox.Show(Messages.TasksSavingFailed, "Saving failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void miRefreshView_Click(object sender, RoutedEventArgs e)
    {
        await LoadTasks();
    }

    private async void IconlessWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.F5)
        {
            await LoadTasks();
        }
    }

    private async void IconlessWindow_Loaded(object sender, RoutedEventArgs e)
    {
        await LoadTasks();
    }
}