using FZCore;
using FZCore.Windows;

using System;
using System.Media;
using System.Windows;

namespace UpDate.Windows;

/// <summary>
/// Representing the window for adding new RSS feeds.
/// </summary>
public partial class WndAddFeed : IconlessWindow
{
    /// <summary>
    /// Creates a new instance of the <see cref="WndAddFeed"/> class.
    /// </summary>
    public WndAddFeed()
    {
        InitializeComponent();
    }

    private bool _isDialog;

    /// <summary>
    /// Representing the standard <see cref="Window.Show"/> method but also marks this window as a non-dialog.
    /// </summary>
    public new void Show()
    {
        this._isDialog = false;
        base.Show();
    }

    /// <summary>
    /// Representing the standard <see cref="Window.ShowDialog"/> method but also marks this window as a dialog.
    /// </summary>
    /// <returns></returns>
    public new bool? ShowDialog()
    {
        this._isDialog = true;
        Loaded += (s, e) => SystemSounds.Beep.Play();
        return base.ShowDialog();
    }

    private bool ProcessInput()
    {
        try
        {
            // gets the raw input
            string url = this.txtUrl.Text.Trim();

            if (string.IsNullOrEmpty(url) == true || Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri? uri) == false)
            {
                // invalid input, show error
                _ = MessageBox.Show($"Unable to add the RSS feed. The given address is not a valid URL.", Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // check passed, register feed
            UpDateSettings.Current ??= UpDateSettings.EnsureSettings();

            if (UpDateSettings.Current.Feeds.Contains(uri.AbsoluteUri) == true)
            {
                Log.Info($"Selected RSS feed \'{uri.AbsoluteUri}\' already exists. Success code is being returned.");
                return true;
            }

            UpDateSettings.Current.Feeds.Add(uri.AbsoluteUri);
            Log.Success($"New feed \'{uri.AbsoluteUri}\' was added to the list.", nameof(ProcessInput));
            return true;
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(ProcessInput));
            return false;
        }
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        if (this._isDialog)
        {
            DialogResult = false;
        }

        Close();
    }

    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
        if (ProcessInput() == false)
        {
            // an error occurred
            return;
        }

        if (this._isDialog)
        {
            DialogResult = true;
        }

        Close();
    }

    private void txtUrl_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        this.btnOK.IsEnabled = this.txtUrl.Text.Length > 0;
    }
}
