using FZCore.Windows;

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace HashHive;

/// <summary>
/// Representing the main application window.
/// This class derives directly from the <see cref="Window"/> class.
/// </summary>
public partial class MainWindow : IconlessWindow
{
    /// <summary>
    /// Creates a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        // sets the current instance value
        _instance = this;

        // loads the window
        InitializeComponent();
        _toggleButtons = [btnText, btnFile];

        // set hashing text page as default
        SetPage(HashHivePage.HashText);

        App.CenterWindow(this);
    }

    private static MainWindow? _instance;

    /// <summary>
    /// Representing the current instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public static MainWindow Instance => _instance ??= new MainWindow();

    private void SetPage(HashHivePage page)
    {
        switch (page)
        {
            default:
            case HashHivePage.Null:
                frmContent.Content = null;
                UncheckAll(null);
                break;

            case HashHivePage.HashText:
                LoadPage(Pages.PgTextHash.Instance);
                UncheckAll(btnText);
                break;

            case HashHivePage.HashFile:
                LoadPage(Pages.PgFileHash.Instance);
                UncheckAll(btnFile);
                break;
        }

        return;
    }

    private List<ToggleButton> _toggleButtons;

    private void UncheckAll(ToggleButton? toggleButton)
    {
        if (toggleButton == null)
        {
            foreach (ToggleButton toggle in _toggleButtons)
            {
                toggle.IsEnabled = false;
            }

            return;
        }

        foreach (ToggleButton button in _toggleButtons)
        {
            if (toggleButton == button)
            {
                continue;
            }

            else
            {
                button.IsChecked = false;
            }
        }

        return;
    }

    private bool LoadPage(Page? page)
    {
        if (page == null)
        {
            return false;
        }

        frmContent.Content = page;
        Title = $"{page.Title} - {App.Title}";
        return true;
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void btnText_Click(object sender, RoutedEventArgs e)
    {
        SetPage(HashHivePage.HashText);
    }

    private void btnFile_Click(object sender, RoutedEventArgs e)
    {
        SetPage(HashHivePage.HashFile);
    }
}