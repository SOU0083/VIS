using DataLayer;
using DomainLayer.DomainModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Presentation_Layer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            int role = 2;   //Nastavení role

            if (role == 0)
            {
                MainWindow mainView = new MainWindow();
                mainView.Show();
            }
            else if (role == 1)
            {
                Main_Customer mainView = new Main_Customer();
                mainView.Show();
            }
            else if (role == 2)
            {
                Main_Admin mainView = new Main_Admin();
                mainView.Show();
            }
        }
    }
}
