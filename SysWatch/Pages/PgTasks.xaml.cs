using FZCore;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using SysWatch.Controls;
using SysWatch.Windows.Dialogs;

namespace SysWatch.Pages;

/// <summary>
/// Representing the processes page.
/// </summary>
public partial class PgTasks : Page
{
    private ContextMenu _cmNoProcess { get; } = new ContextMenu();
    private ContextMenu _cmProcess { get; } = new ContextMenu();

    /// <summary>
    /// Creates a new <see cref="PgTasks"/> instance.
    /// </summary>
    public PgTasks()
    {
        InitializeComponent();
    }

    private int _pId = -1;
    private int GetSelectedPid()
    {
        return _pId;
    }

    private MenuItem CreateRefreshItem()
    {
        MenuItem miRefresh = new MenuItem
        {
            Header = "Refresh",
            InputGestureText = "F5"
        };

        miRefresh.Click += async (s, e) => await LoadUI();

        return miRefresh;
    }

    private async Task ConstructNoProcessMenu()
    {
        _cmNoProcess.Items.Clear();

        MenuItem miRefresh = CreateRefreshItem();
        _cmNoProcess.Items.Add(miRefresh);

        return;
    }

    private async Task ConstructProcessMenu()
    {
        _cmProcess.Items.Clear();

        MenuItem miDetails = new MenuItem
        {
            Header = "Details",
            InputGestureText = "F7"
        };

        miDetails.Click += (s, e) =>
        {
            int pId = GetSelectedPid();
            if (pId >= 0) ShowProcessInfo(pId);
        };

        _cmProcess.Items.Add(miDetails);

        MenuItem miKill = new MenuItem
        {
            Header = "Terminate",
            InputGestureText = "Del"
        };

        miKill.Click += async (s, e) =>
        {
            int pId = GetSelectedPid();
            if (pId >= 0) await ProcTerminate(pId);
        };

        _cmProcess.Items.Add(miKill);
        _cmProcess.Items.Add(new Separator());

        MenuItem miRefresh = CreateRefreshItem();
        _cmProcess.Items.Add(miRefresh);

        return;
    }

    private async Task ConstructMenus()
    {
        await ConstructNoProcessMenu();
        await ConstructProcessMenu();
        return;
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
                Content = ctlProcItem,
                Tag = proc.Id
            };

            lbi.Selected += (s, e) =>
            {
                _pId = proc.Id;
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

    private void ShowProcessInfo(int pId)
    {
        // shows process information
        DlgProcessInfo dlgInfo = new DlgProcessInfo(pId);

        try
        {
            _ = dlgInfo.ShowDialog();
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(ShowProcessInfo));
        }

        return;
    }

    private async Task ProcTerminate(int pId)
    {
        try
        {
            Process.GetProcessById(pId).Kill();
        }

        catch (Exception ex)
        {
            FZCore.Core.ErrorBox(ex.Message, "End Task");
            Log.Error(ex, nameof(ProcTerminate));
        }

        finally
        {
            await LoadUI();
        }
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        await LoadUI();
        await ConstructMenus();
    }

    private async void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
    {
        await FilterUI(txtFilter.Text);
    }

    private void lbxTasks_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (lbxTasks.SelectedIndex != -1)
        {
            lbxTasks.ContextMenu = _cmProcess;
        }

        else
        {
            lbxTasks.ContextMenu = _cmNoProcess;
        }
    }

    private async void lbxTasks_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.F5)
        {
            await LoadUI();
        }

        else if (e.Key == System.Windows.Input.Key.F7)
        {
            if (lbxTasks.SelectedIndex == -1)
            {
                // no process selected
                return;
            }

            // show process info if the item is valid
            int pId = GetSelectedPid();
            if (pId >= 0) ShowProcessInfo(pId);

            return;
        }

        else if (e.Key == System.Windows.Input.Key.Delete)
        {
            if (lbxTasks.SelectedIndex == -1)
            {
                // no process selected
                return;
            }

            int pId = GetSelectedPid();
            await ProcTerminate(pId);
        }
    }
}
