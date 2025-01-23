using System;
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

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Expando.MainWindow mw = new Expando.MainWindow();
            MainWindow = mw;
            MainWindow.Show();
        }
    }

}
