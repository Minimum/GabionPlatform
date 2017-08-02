using System;
using System.Windows;
using GabionLauncher.View;

namespace GabionLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void Application_Startup(object sender, StartupEventArgs e)
        {
            DevMainWindow window = new DevMainWindow();

            window.Show();

            /*if (e.Args.Length > 0 && e.Args[0].Equals("-dev", StringComparison.OrdinalIgnoreCase))
            {
                DevMainWindow window = new DevMainWindow();

                window.Show();
            }
            else
            {
                ClientMainWindow window = new ClientMainWindow();

                window.Show();
            }*/
        }
    }
}
