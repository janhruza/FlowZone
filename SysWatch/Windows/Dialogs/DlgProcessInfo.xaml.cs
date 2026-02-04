using FZCore;
using FZCore.Windows;

using System;
using System.ComponentModel;
using System.Diagnostics;

namespace SysWatch.Windows.Dialogs;

/// <summary>
/// Representing the process info dialog.
/// </summary>
public partial class DlgProcessInfo : IconlessWindow
{
    const string NO_INFO = "N/A";

    /// <summary>
    /// Creates a new <see cref="DlgProcessInfo"/> instance with the process Id specified.
    /// </summary>
    /// <param name="processId">Representing the target process identificator.</param>
    public DlgProcessInfo(int processId)
    {
        InitializeComponent();
        InvalidateFields();

        _pid = processId;

        if (processId >= 0)
        {
            try
            {
                _proc = Process.GetProcessById(processId);
            }

            catch (Exception ex)
            {
                _proc = null;
                Log.Error(ex, nameof(DlgProcessInfo));
            }
        }
    }

    private int _pid;
    private Process? _proc;

    private void LoadProcessInfo()
    {
        // check process validity
        if (_proc is null) return;

        try
        {
            // display its info
            txtProcessName.Text = _proc.ProcessName;
            txtExtra.Text = _proc.TotalProcessorTime.ToString();
            txtWindowTitle.Text = _proc.MainWindowHandle != nint.Zero ? _proc.MainWindowTitle : NO_INFO;
        }

        catch (Win32Exception win32ex)
        {
            // access denied
            Log.Error(win32ex, nameof(DlgProcessInfo));
        }

        catch (InvalidOperationException ioex)
        {
            Log.Error(ioex, nameof(LoadProcessInfo));
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(LoadProcessInfo));
            FZCore.Core.ErrorBox(ex.Message, ex.GetType().ToString());
        }
    }

    private void InvalidateFields()
    {
        if (!IsLoaded)
        {
            return;
        }

        txtProcessName.Text = NO_INFO;
        txtExtra.Text = NO_INFO;
        return;
    }

    private void IconlessWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        LoadProcessInfo();
    }
}
