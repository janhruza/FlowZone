using System.Threading.Tasks;
using System.Windows;
using FZCore;
using ResourceRadar.Core;
using ResourceRadar.Core.Authentication;
using ResourceRadar.Windows;

namespace ResourceRadar;

/// <summary>
/// Representing the main application class.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Representing the base window title.
    /// </summary>
    public const string Title = "Resource Radar";

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        MainWindow mw = new MainWindow();
        MainWindow = mw;
        MainWindow.Show();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        // Post exit cleanup
        if (UserProfile.Current != null)
        {
            _ = UserProfile.SaveSettings(UserProfile.Current);
            _ = UserProfile.WriteUserItems(UserProfile.Current, UserProfile.Current.Items);
        }

        return;
    }

    /// <summary>
    /// Attempts to create a new user profile.
    /// </summary>
    /// <returns>True if new user profile was created.</returns>
    public static bool CreateNewProfile()
    {
        WndNewProfile wnd = new WndNewProfile();
        if (wnd.ShowDialog() != true)
        {
            Log.Info("Profile creation cancelled.", nameof(CreateNewProfile));
            return false;
        }

        return true;
    }

    /// <summary>
    /// Opens a new item dialog window.
    /// </summary>
    /// <param name="oItem">A place to store the created inventory item (in case of need).</param>
    /// <returns>True, if new inventory item was created, otherwise false.</returns>
    public static bool AddNewItem(out InventoryItem? oItem)
    {
        WndNewItem wnd = new WndNewItem();
        if (wnd.ShowDialog() == true)
        {
            oItem = wnd.Value;
            return true;
        }

        oItem = null;
        return false;
    }

    /// <summary>
    /// Displays the message box saying no user is logged in.
    /// </summary>
    public static void NoLoggedUser()
    {
        Log.Error(Messages.NO_USER_LOGGED, nameof(NoLoggedUser));
        _ = MessageBox.Show("", "No user logged", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
    }
}

