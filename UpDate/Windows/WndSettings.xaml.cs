using FZCore.Windows;

using System;
using System.Windows;
using System.Windows.Controls;

namespace UpDate.Windows;

/// <summary>
/// Representing the settings window.
/// </summary>
public partial class WndSettings : IconlessWindow
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
        Close();
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void rRestore_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        // restore default title
        txtBaseTitle.Text = App.AppTitle;
    }

    private void rRestore_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        UpDateSettings.Current ??= new UpDateSettings();
        switch (UpDateSettings.Current.ThemeMode)
        {
            case FZCore.FZThemeMode.System:
                if (FZCore.Core.IsDarkModeEnabled() == true)
                {
                    goto case FZCore.FZThemeMode.Dark;
                }

                else
                {
                    goto case FZCore.FZThemeMode.Light;
                }

            case FZCore.FZThemeMode.Dark:
                rRestore.Foreground = SystemColors.AccentColorLight1Brush;
                return;

            case FZCore.FZThemeMode.None:
            case FZCore.FZThemeMode.Light:
                rRestore.Foreground = SystemColors.AccentColorDark1Brush;
                return;
        }
    }

    private void rRestore_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        rRestore.Foreground = SystemColors.AccentColorBrush;
    }
}
