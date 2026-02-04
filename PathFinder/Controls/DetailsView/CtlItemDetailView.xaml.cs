using PathFinder.Data;

using System.IO;
using System.Windows.Controls;

namespace PathFinder.Controls;

/// <summary>
/// Representing a detailed view for a single item.
/// </summary>
public partial class CtlItemDetailView : UserControl
{
    /// <summary>
    /// Creates a <see cref="CtlItemDetailView"/> instance.
    /// </summary>
    public CtlItemDetailView()
    {
        InitializeComponent();
        Loaded += (s, e) =>
        {
            this.tbName.Text = "Name";
            this.tbExtension.Text = "Extension";
            this.tbModified.Text = "Modified";
            this.tbSize.Text = "Size (bytes)";
        };
    }

    /// <summary>
    /// Creates a <see cref="CtlItemDetailView"/> instance.
    /// </summary>
    public CtlItemDetailView(ref FSObjectInfo obj)
    {
        InitializeComponent();
        this._info = obj;
        Loaded += CtlItemDetailView_Loaded;
    }

    private FSObjectInfo _info;

    private void CtlItemDetailView_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        LoadData(ref this._info);
    }

    private void LoadData(ref FSObjectInfo info)
    {
        if (info.IsSpecial)
        {
            // special items not permitted
            return;
        }

        string name, ext, modified, size;

        if (info.IsFile)
        {
            FileInfo fi = (FileInfo)info.Info;
            name = fi.Name;
            ext = fi.Extension;
            modified = fi.LastWriteTime.ToString();
            size = fi.Length.ToString();
        }

        else
        {
            DirectoryInfo di = (DirectoryInfo)info.Info;
            name = di.Name;
            ext = string.Empty;
            modified = di.LastWriteTime.ToString();
            size = string.Empty;
        }

        this.tbName.Text = name;
        this.tbExtension.Text = ext;
        this.tbModified.Text = modified;
        this.tbSize.Text = size;
        return;
    }
}
