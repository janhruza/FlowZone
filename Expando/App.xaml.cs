using System.Windows;
using Expando.Core;

namespace Expando
{
    /// <summary>
    /// Representing the main Expando application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Creates a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            return;
        }

        /// <summary>
        /// Attempts to load user profiles data and shows an error box if it's unsuccessful.
        /// </summary>
        public static void ReloadUserData()
        {
            // loads the users data
            if (UserProfile.LoadUsersData() != 0)
            {
                _ = MessageBox.Show(Messages.CantLoadUserData, "Load Profiles", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Expando.MainWindow mw = new Expando.MainWindow();
            MainWindow = mw;
            MainWindow.Show();

            // loads users data on startup
            ReloadUserData();
        }
    }

}
