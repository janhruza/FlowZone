using System.Windows;

namespace FZCore.Windows.Dialogs;

/// <summary>
/// Representing a custom, text input dialog.
/// </summary>
public partial class DlgTextInput : IconlessWindow
{
    /// <summary>
    /// Creates a new <see cref="DlgTextInput"/> instance.
    /// </summary>
    private DlgTextInput(string header, string defaultValue)
    {
        InitializeComponent();

        // set the values
        txt.Header = header;
        txt.Text = defaultValue;
    }

    /// <summary>
    /// Gets the input value.
    /// </summary>
    public string Value
    {
        get => txt.Text;
    }

    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = true;
        this.Close();
    }

    /// <summary>
    /// Reads the input from a user.
    /// </summary>
    /// <param name="header">Input prompt.</param>
    /// <param name="defaultValue">Default, pre-filled value.</param>
    /// <returns>A <see cref="string"/> representation of the user input.</returns>
    public static string GetInput(string header, string defaultValue)
    {
        DlgTextInput input = new DlgTextInput(header, defaultValue);
        return input.ShowDialog() == true ? input.Value : string.Empty;
    }

    /// <summary>
    /// Gets the user input as a <see cref="string"/>.
    /// </summary>
    /// <returns>A <see cref="string"/> representation of the user input.</returns>
    public static string GetInput()
    {
        return GetInput("Enter a value", string.Empty);
    }
}
