using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace FZCore.Windows
{
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
    public partial class LogViewer : Window
    {
        /// <summary>
        /// Creates a new instance of the <see cref="LogViewer"/> class.
        /// </summary>
        /// <param name="logFilePath">Path to the log file.</param>
        public LogViewer(string logFilePath)
        {
            InitializeComponent();
            dgEntries.RowHeight = double.NaN;

            // Load the log file when the window is fully loaded
            this.Loaded += (s, e) =>
            {
                LoadLogFile(logFilePath);
            };
        }

        private void LoadLogFile(string filename)
        {
            // If the file doesn't exist, display the filename and return
            if (!File.Exists(filename))
            {
                rFilename.Text = "FILE_NOT_FOUND";
                return;
            }

            // Display the file path
            rFilename.Text = Path.GetFileName(filename);

            // Create an ObservableCollection for data binding
            ObservableCollection<LogEntry> logEntries = new ObservableCollection<LogEntry>();

            try
            {
                // Parse the log file line by line
                foreach (string line in File.ReadAllLines(filename))
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

                    logEntries.Add(entry);
                }

                // Bind the ObservableCollection to the DataGrid
                dgEntries.ItemsSource = logEntries;
            }
            catch (Exception ex)
            {
                // Handle any file parsing or IO exceptions
                MessageBox.Show($"Error loading log file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
