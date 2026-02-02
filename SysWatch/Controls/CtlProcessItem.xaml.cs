using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SysWatch.Controls;

/// <summary>
/// Representing a single process item control.
/// </summary>
public partial class CtlProcessItem : UserControl
{
    /// <summary>
    /// Creates a new <see cref="CtlProcessItem"/> instance that serves as the header row.
    /// </summary>
    public CtlProcessItem()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Creates a new <see cref="CtlProcessItem"/> instance with the <paramref name="proc"/> specified.
    /// </summary>
    /// <param name="proc">Target process to include information about.</param>
    public CtlProcessItem(Process proc)
    {
        InitializeComponent();
        if (proc is not null)
        {
            // store the process int the 'Tag' property
            this.Tag = proc;
        }
    }

    private async Task LoadProcessInfo()
    {
        // load the process from the 'Tag' property and display it
        if (this.Tag is Process proc)
        {
            string name = proc.ProcessName;
            int pid = proc.Id;

            // set the content
            tbName.Text = name;
            tbPID.Text = pid.ToString();
            tbWorkingSet.Text = proc.WorkingSet64.ToString();
            tbExtra.Text = proc.Threads.Count.ToString();
        }
    }

    private async void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        await LoadProcessInfo();
    }
}
