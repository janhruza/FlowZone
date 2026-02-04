using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FZCore.Windows.Extra;

internal class LogEntry
{
    public string Date { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// Represents the event viewer window.
/// </summary>
public partial class LogViewer : IconlessWindow
{
    /// <summary>
    /// Creates a new instance of the <see cref="LogViewer"/> class.
    /// </summary>
    /// <param name="logFilePath">Path to the log file.</param>
    public LogViewer(string logFilePath)
    {
        InitializeComponent();
        this.dgEntries.RowHeight = double.NaN;
        this._dataLoaded = false;
        this._loadedData = [];

        // Load the log file when the window is fully loaded
        Loaded += async (s, e) =>
        {
            await LoadFile(logFilePath);
            await DisplayData(string.Empty);
        };
    }

    private bool _dataLoaded;
    private List<string> _loadedData;

    private async Task LoadFile(string filename)
    {
        // If the file doesn't exist, display the filename and return
        if (!File.Exists(filename))
        {
            this.rFilename.Text = "FILE_NOT_FOUND";
            return;
        }

        // Display the file path
        this.rFilename.Text = Path.GetFileName(filename);
        string[] data = await File.ReadAllLinesAsync(filename);
        this._loadedData = data.ToList();
        this._dataLoaded = true;
        return;
    }

    private async Task DisplayData(string filter)
    {
        // check if data are loaded
        if (this._dataLoaded == false)
        {
            // no loaded data
            return;
        }

        ProgressIntermediate();

        // Create an ObservableCollection for data binding
        List<LogEntry> logEntries = new List<LogEntry>();

        try
        {
            // Parse the log file line by line
            foreach (string line in this._loadedData)
            {
                /*
                 * FORMAT:
                 * date;type;tag;message
                 */
                string[] parts = line.Split(';');
                if (parts.Length != 4)
                {
                    // Skip invalid lines
                    continue;
                }

                // Create and add a new LogEntry
                LogEntry entry = new LogEntry
                {
                    Date = parts[0],
                    Type = parts[1],
                    Tag = parts[2],
                    Message = parts[3]
                };

                // filter tags
                if (string.IsNullOrEmpty(filter) == true)
                {
                    // no filter
                    logEntries.Add(entry);
                }

                else
                {
                    if (entry.Tag.ToLower().Contains(filter.ToLower()))
                    {
                        logEntries.Add(entry);
                    }
                }
            }

            // sort by date (newest first)
            logEntries = [.. logEntries.OrderByDescending(x => DateTime.Parse(x.Date))];

            // Bind the ObservableCollection to the DataGrid
            this.dgEntries.ItemsSource = logEntries;
        }
        catch (Exception ex)
        {
            // Handle any file parsing or IO exceptions
            _ = MessageBox.Show($"Error loading log file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        finally
        {
            ProgressDisable();
        }
    }

    private async void txtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        await DisplayData(this.txtSearch.Text);
    }
}
