using System.IO;
using System.Windows;
using System;
using TaskPilot.Core;

namespace TaskPilot;

/// <summary>
/// Main Task Pilot application class.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Creates a new instance of the <see cref="App"/> class.
    /// </summary>
    public App()
    {
        // loads all the saved tasks
        if (LoadTasks(TasksFileLocation, out TasksCollection tasks) == false)
        {
            // unable to load tasks
            Tasks = [];
        }

        else
        {
            // load tasks
            Tasks = tasks;
        }
    }

    /// <summary>
    /// Creates the static data for this class on the first use.
    /// </summary>
    static App()
    {
        Tasks = [];
    }

    /// <summary>
    /// Representing a list of all loaded tasks.
    /// </summary>
    public static TasksCollection Tasks { get; set; }

    /// <summary>
    /// Representing the base file path for the saved tasks.
    /// </summary>
    public static string TasksFileLocation => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tasks.bin");

    /// <summary>
    /// Loads all tasks from the given file (<paramref name="fileName"/>) inti the given list (<paramref name="tasks"/>).
    /// </summary>
    /// <param name="fileName">Path to the file that contains tasks data.</param>
    /// <param name="tasks">A place to store loaded tasks.</param>
    /// <returns></returns>
    public static bool LoadTasks(string fileName, out TasksCollection tasks)
    {
        tasks = [];

        try
        {
            if (File.Exists(fileName) == false)
            {
                Log.Error($"LoadTasks: Unable to load tasks from '{fileName}'.");
                return false;
            }

            using (FileStream fs = File.OpenRead(fileName))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    // get number of tasks
                    int count = br.ReadInt32();

                    for (int x = 0; x < count; x++)
                    {
                        // read all tasks, one by one
                        TaskItem item = new TaskItem
                        {
                            Id = br.ReadInt64(),
                            Caption = br.ReadString(),
                            Text = br.ReadString(),
                            CreationDate = DateTime.FromBinary(br.ReadInt64()),     // creatinon date as long
                            ExpirationDate = DateTime.FromBinary(br.ReadInt64()),   // expiration date as long
                        };

                        // add task to list
                        tasks.Add(item);
                    }
                }
            }

            Log.Success($"LoadTasks: Tasks were loaded from '{fileName}'.");
            return true;
        }

        catch (IOException iex)
        {
            Log.Error(iex);
            return false;
        }

        catch (Exception ex)
        {
            Log.Error(ex);
            return false;
        }
    }

    /// <summary>
    /// Saves all the tasks from the <paramref name="tasks"/> list into the file at <paramref name="fileName"/>.
    /// </summary>
    /// <param name="fileName">A path to the output file where the tasks will be stored.</param>
    /// <param name="tasks">List of all tasks to save.</param>
    /// <returns></returns>
    public static bool SaveTasks(string fileName, ref TasksCollection tasks)
    {
        try
        {
            using (FileStream fs = File.Create(fileName))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    // gets the count of all tasks and writes it into the stream
                    int count = tasks.Count;
                    bw.Write(count);

                    // writes all tasks into the stream one by one
                    foreach (TaskItem task in tasks)
                    {
                        bw.Write(task.Id);                          // as long
                        bw.Write(task.Caption);                     // as string
                        bw.Write(task.Text);                        // as string
                        bw.Write(task.CreationDate.ToBinary());     // as long
                        bw.Write(task.ExpirationDate.ToBinary());   // as long
                    }
                }
            }

            Log.Success($"SaveTasks: Tasks saved to '{fileName}'.");
            return true;
        }

        catch (IOException iex)
        {
            Log.Error(iex);
            return false;
        }

        catch (Exception ex)
        {
            Log.Error(ex);
            return false;
        }
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        TaskPilot.MainWindow mw = new TaskPilot.MainWindow();
        MainWindow = mw;
        MainWindow.Show();
    }
}
