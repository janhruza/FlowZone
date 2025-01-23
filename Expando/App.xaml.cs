using System.Windows;

namespace Expando
{
    /// <summary>
    /// Interaction logic for App.xaml
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
