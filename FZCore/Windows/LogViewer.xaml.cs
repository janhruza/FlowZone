﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
            _dataLoaded = false;
            _loadedData = [];

            // Load the log file when the window is fully loaded
            this.Loaded += (s, e) =>
            {
                LoadFile(logFilePath);
                DisplayData(string.Empty);
            };
        }

        private bool _dataLoaded;
        private List<string> _loadedData;

        private void LoadFile(string filename)
        {
            // If the file doesn't exist, display the filename and return
            if (!File.Exists(filename))
            {
                rFilename.Text = "FILE_NOT_FOUND";
                return;
            }

            // Display the file path
            rFilename.Text = Path.GetFileName(filename);
            _loadedData = File.ReadAllLines(filename).ToList();
            _dataLoaded = true;
        }

        private void DisplayData(string filter)
        {
            // check if data are loaded
            if (_dataLoaded == false)
            {
                // no loaded data
                return;
            }

            // Create an ObservableCollection for data binding
            ObservableCollection<LogEntry> logEntries = new ObservableCollection<LogEntry>();

            try
            {
                // Parse the log file line by line
                foreach (string line in _loadedData)
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

                // Bind the ObservableCollection to the DataGrid
                dgEntries.ItemsSource = logEntries;
            }
            catch (Exception ex)
            {
                // Handle any file parsing or IO exceptions
                MessageBox.Show($"Error loading log file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            DisplayData(txtSearch.Text);
        }
    }
}
