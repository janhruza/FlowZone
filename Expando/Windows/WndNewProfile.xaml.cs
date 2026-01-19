using Expando.Core;

using FZCore.Windows;

using System;
using System.Windows;

namespace Expando.Windows;

/// <summary>
/// Representing a new user creation window.
/// </summary>
public partial class WndNewProfile : IconlessWindow
{
    /// <summary>
    /// Creates a new instance of the <see cref="WndNewProfile"/> class.
    /// </summary>
    public WndNewProfile()
    {
        InitializeComponent();
        txtUsername.Text = Environment.UserName;
    }

    private UserProfile _profile = new UserProfile();

    /// <summary>
    /// Representing the created user profile.
    /// </summary>
    public UserProfile Profile => _profile;

    private bool ValidateData()
    {
        if (_profile.Id == ulong.MinValue) return false;
        if (_profile.CreationDate == DateTime.MinValue) return false;
        if (string.IsNullOrEmpty(_profile.Username.Trim())) return false;

        return true;
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
        // set the data
        DateTime dt = DateTime.Now;
        _profile = new UserProfile
        {
            Id = (ulong)dt.ToBinary(),
            Username = txtUsername.Text,
            CreationDate = dt
        };

        // check if input is valid
        if (ValidateData() == false)
        {
            _ = MessageBox.Show(Messages.InvalidProfileData, "Create profile", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        else
        {
            UserProfile profile = _profile;
            if (UserProfile.CreateUser(ref profile) == 0)
            {
                UserProfile.Profiles.Add(profile);
                DialogResult = true;
                Close();
            }

            else
            {
                _ = MessageBox.Show(Messages.CantCreateProfile, "Create profile", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    #region Static code

    /// <summary>
    /// Shows a <see cref="WndNewProfile"/> dialog and attempts to create a new user.
    /// </summary>
    /// <returns>True, if the new user is created, otherwise false.</returns>
    public static bool CreateUser()
    {
        WndNewProfile profile = new WndNewProfile();
        return profile.ShowDialog() == true;
    }

    #endregion
}
