using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordStore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        private void App_Startup(object sender, StartupEventArgs e) 
        {
            MainWindow mainWindow = new MainWindow();

            if (e.Args.Length > 0 && File.Exists(e.Args[0]))
            {
                mainWindow.mainWindowViewModel.LoadFile(e.Args[0]);
            }

            mainWindow.Show();
        }
    }
}
