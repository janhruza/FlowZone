using System.Windows.Controls;

namespace FZCore.Controls;

/// <summary>
/// Representing a custom text input control.
/// </summary>
public partial class CtlTextInput : UserControl
{
    /// <summary>
    /// Initializes a new instance of the CtlTextInput class.
    /// </summary>
    /// <remarks>This constructor sets up the control and prepares it for use. Call this constructor when
    /// creating a new CtlTextInput control in your application.</remarks>
    public CtlTextInput()
    {
        InitializeComponent();
        txt.Clear();
    }

    /// <summary>
    /// Gets or sets the header content of the control - <see cref="Label"/>.
    /// </summary>
    public object Header
    {
        get => lbl.Content;
        set => lbl.Content = value;
    }

    /// <summary>
    /// Gets or sets the placeholder text.
    /// </summary>
    public string Placeholder
    {
        get => tbPlaceholder.Text;
        set => tbPlaceholder.Text = value;
    }

    /// <summary>
    /// Gets or sets the text of the text box area.
    /// </summary>
    public string Text
    {
        get => txt.Text;
        set => txt.Text = value;
    }

    /// <summary>
    /// Determines whether the text area is read only or not.
    /// </summary>
    public bool IsReadOnly
    {
        get => txt.IsReadOnly;
        set => txt.IsReadOnly = value;
    }

    /// <summary>
    /// Gets the underlying <see cref="System.Windows.Controls.TextBox"/> control associated with this instance.
    /// </summary>
    public TextBox TextBox => txt;

    private void txt_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (tbPlaceholder is null) return;

        if (txt.Text.Length == 0)
        {
            tbPlaceholder.Visibility = System.Windows.Visibility.Visible;
        }

        else
        {
            tbPlaceholder.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
