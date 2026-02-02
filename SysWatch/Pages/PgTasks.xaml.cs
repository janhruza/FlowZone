using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using SysWatch.Controls;

namespace SysWatch.Pages;

/// <summary>
/// Representing the processes page.
/// </summary>
public partial class PgTasks : Page
{
    /// <summary>
    /// Creates a new <see cref="PgTasks"/> instance.
    /// </summary>
    public PgTasks()
    {
        InitializeComponent();
    }

    private static async Task<List<Process>> FetchProcesses()
    {
        _latestReport = [.. Process.GetProcesses()];
        return _latestReport;
    }

    private static List<Process> _latestReport = [];

    private static ListBoxItem CreateHeaderRow()
    {
        CtlProcessItem ctlHeader = new CtlProcessItem
        {
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            FontSize = 16,
            FontWeight = FontWeights.Bold
        };

        ListBoxItem lbiHeader = new ListBoxItem
        {
            HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch,
            Content = ctlHeader,
            IsEnabled = false
        };

        return lbiHeader;
    }

    private async Task UpdateView(List<Process> processList, string filter)
    {
        lbxTasks.Items.Clear();

        if (string.IsNullOrWhiteSpace(filter) == false)
        {
            // filter processes
            processList = [.. processList.Where(p => p.ProcessName.Contains(filter, System.StringComparison.OrdinalIgnoreCase))];
        }

        // header row
        //ListBoxItem lbiHeader = CreateHeaderRow();
        //lbxTasks.Items.Add(lbiHeader);

        foreach (Process proc in processList)
        {
            CtlProcessItem ctlProcItem = new CtlProcessItem(proc)
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
            };

            ListBoxItem lbi = new ListBoxItem
            {
                HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch,
                Content = ctlProcItem
            };

            lbxTasks.Items.Add(lbi);
        }
    }

    private async Task LoadUI()
    {
        List<Process> processes = await FetchProcesses();
        await UpdateView(processes, string.Empty);
        return;
    }

    private async Task FilterUI(string filter)
    {
        await UpdateView(_latestReport, filter);
        return;
    }

    private async void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        await LoadUI();
    }

    private async void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
    {
        await FilterUI(txtFilter.Text);
    }
}
