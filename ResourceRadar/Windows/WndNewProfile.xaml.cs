using ResourceRadar.Core.Authentication;

using System.Globalization;
using System.Media;
using System.Windows;
using System.Windows.Controls;

namespace ResourceRadar.Windows;

/// <summary>
/// Representing the profile creation window.
/// </summary>
public partial class WndNewProfile : Window
{
    /// <summary>
    /// Creates a new instance of the <see cref="WndNewProfile"/> class.
    /// </summary>
    public WndNewProfile()
    {
        InitializeComponent();
        this.Loaded += (s, e) =>
        {
            // load cultures
            cbLocales.Items.Clear();

            foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                ComboBoxItem cbi = new ComboBoxItem
                {
                    Content = $"{culture.DisplayName} {(string.IsNullOrEmpty(culture.Name) == true ? string.Empty : $"({culture.Name.ToUpper()})")}",
                    Tag = culture.Name,
                    Uid = culture.Name
                };

                cbi.Selected += (s, e) =>
                {
                    sLocale = culture.Name;
                };

                cbLocales.Items.Add(cbi);
            }

        };
    }

    private string sName = string.Empty;
    private string sDescription = string.Empty;
    private string sLocale = string.Empty;

    private bool VerifyFields()
    {
        sName = txtUsername.Text.Trim();
        sDescription = txtDescription.Text.Trim();

        if (string.IsNullOrEmpty(sName) == true || string.IsNullOrEmpty(sLocale) == true)
        {
            // fields are required
            return false;
        }

        return true;
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
        this.Close();
    }

    private async void btnOk_Click(object sender, RoutedEventArgs e)
    {
        if (VerifyFields() == true)
        {
            bool result = await UserProfile.CreateUserProfile(sName, sDescription);
            if (result == true)
            {
                this.DialogResult = true;
                this.Close();
                return;
            }

            else
            {
                _ = MessageBox.Show(Messages.CREATE_USER_FAILED, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        else
        {
            _ = MessageBox.Show(Messages.CREATE_USER_CHECK_FAILED, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
    }

    private void txtUsername_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        btnOk.IsEnabled = txtUsername.Text.Length > 0;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        SystemSounds.Beep.Play();
    }
}
