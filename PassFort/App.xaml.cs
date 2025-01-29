using System.Windows;
using System.Collections.Generic;
using System.IO;

namespace PassFort;

/// <summary>
/// Representing the main application class.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Representing the app window title.
    /// </summary>
    public const string Title = "PassFort";

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        // load history (if any)
        LoadHistory();

        MainWindow mw = new MainWindow();
        MainWindow = mw;
        MainWindow.Show();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        // save history
        SaveHistory();
    }

    #region History saving and loadng

    private static string _historyPath = "history.txt";

    private static List<string> _history = [];

    /// <summary>
    /// Representing a list of recently opened databases.
    /// </summary>
    public static List<string> History => _history;

    private static void LoadHistory()
    {
        if (File.Exists(_historyPath) == false)
        {
            return;
        }

        _history.Clear();
        foreach (string line in File.ReadAllLines(_historyPath))
        {
            _history.Add(line);
        }
    }

    private static void SaveHistory()
    {
        string[] lines = _history.ToArray();
        File.WriteAllLines(_historyPath, lines);
        return;
    }

    #endregion
}
