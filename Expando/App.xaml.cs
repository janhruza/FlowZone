using System.Windows;

namespace Expando
{
    /// <summary>
    /// Representing the main Expando application class.
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Expando.MainWindow mw = new Expando.MainWindow();
            MainWindow = mw;
            MainWindow.Show();
        }
    }

}
