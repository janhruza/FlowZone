using PathFinder.Data;

using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PathFinder.Controls.GridView;

/// <summary>
/// Representing a single grid view item.
/// </summary>
public partial class CtlGridViewItem : UserControl
{
    private FSObjectInfo _info;

    /// <summary>
    /// Creates a new <see cref="CtlGridViewItem"/> instance.
    /// This creates an empty item suitable as an item referring to the parent directory.
    /// </summary>
    public CtlGridViewItem()
    {
        InitializeComponent();
        tbIcon.Text = ""; // arrow back
        tbText.Text = "[Parent]";
    }

    /// <summary>
    /// Creates a new <see cref="CtlGridViewItem"/> instance.
    /// </summary>
    /// <param name="obj">Associatd object.</param>
    public CtlGridViewItem(ref FSObjectInfo obj)
    {
        InitializeComponent();
        _info = obj;
        _ = ReloadUI();
    }

    private const string TEXT_ICON_FILE = "";
    private const string TEXT_ICON_FOLDER = "";

    /// <summary>
    /// Updates the control's content.
    /// </summary>
    /// <returns>Operation result.</returns>
    public bool ReloadUI()
    {
        if (_info.Exists == false) return false;

        FileSystemInfo fsi = (FileSystemInfo)_info.Info;
        tbText.Text = fsi.Name;

        switch (_info.IsFile)
        {
            // file
            case true:
                tbIcon.Text = TEXT_ICON_FILE;
                return true;

            // folder
            default:
                tbIcon.Text = TEXT_ICON_FOLDER;
                return true;
        }
    }

    /// <summary>
    /// Updates the control's content asynchronously.
    /// </summary>
    /// <returns>Operation result.</returns>
    public async Task<bool> ReloadUIAsync()
    {
        return ReloadUI();
    }
}
