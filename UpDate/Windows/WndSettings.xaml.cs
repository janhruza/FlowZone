using System;
using System.Windows;
using System.Windows.Controls;

namespace UpDate.Windows;

/// <summary>
/// Representing the settings window.
/// </summary>
public partial class WndSettings : Window
{
    /// <summary>
    /// Creates a new instance of the <see cref="WndSettings"/> class.
    /// </summary>
    public WndSettings()
    {
        InitializeComponent();
        Result = false;

        // draw UI
        RedrawUI();
    }

    private void RedrawUI()
    {
        if (UpDateSettings.Current == null)
        {
            UpDateSettings.Current = UpDateSettings.EnsureSettings();
        }

        cbThemes.Items.Clear();
        int idx = 0;

        foreach (FZCore.FZThemeMode themeMode in Enum.GetValues<FZCore.FZThemeMode>())
        {
            int index = cbThemes.Items.Add(new ComboBoxItem()
            {
                Tag = themeMode,
                Content = themeMode.ToString()
            });

            if (UpDateSettings.Current.ThemeMode == themeMode)
            {
                idx = index;
            }
        }

        if (cbThemes.Items.Count > 0)
        {
            cbThemes.SelectedIndex = idx;
        }

        txtBaseTitle.Text = UpDateSettings.Current.Title;
    }

    /// <summary>
    /// Representing a value whether the settings were changed or not.
    /// </summary>
    public bool Result { get; private set; }

    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
        UpDateSettings.Current ??= new UpDateSettings();
        ComboBoxItem item = (ComboBoxItem)cbThemes.SelectedItem;

        UpDateSettings.Current.Title = txtBaseTitle.Text;
        UpDateSettings.Current.ThemeMode = (FZCore.FZThemeMode)item.Tag;

        Result = true;
        this.Close();
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
