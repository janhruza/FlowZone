using System;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using ResourceRadar.Core.Authentication;

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
    }

    private string sName = string.Empty;
    private string sDescription = string.Empty;

    private bool VerifyFields()
    {
        sName = txtUsername.Text.Trim();
        sDescription = txtDescription.Text.Trim();

        if (string.IsNullOrEmpty(sName) == true)
        {
            // name is mandatory
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
