using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Timers;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace iDecryptIt_WPF
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        //Timer timer = new Timer();
        //bool timer_GoneOff = false;

        public SplashScreen()
        {
            //timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            //timer.Interval = 1500;

            // Grab .NET version
            RegistryKey installed_versions = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP");
            string[] version_names = installed_versions.GetSubKeyNames();
            double Framework = Convert.ToDouble(version_names[version_names.Length - 1].Remove(0, 1), CultureInfo.InvariantCulture);
            //int SP = Convert.ToInt32(installed_versions.OpenSubKey(version_names[version_names.Length - 1]).GetValue("SP", 0));

            InitializeComponent();

            // It can't close proporly if it has not been initialized
            if (Framework < 4.0)
            {
                MessageBox.Show("You need .NET 4.0 or better to run iDecryptIt\n\nYou can download it at www.microsoft.com/net\nor through Windows Update", "iDecryptIt", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.Close(); // No need for dispatcher
            }
        }
        /*private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer_GoneOff = true;
        }*/
        private void updateprog(string text)
        {
            Action act = () =>
            {
                _updateprog(text);
            };
            Dispatcher.BeginInvoke(act);
        }
        private void _updateprog(string text)
        {
            lblProg.Content = text;
        }
        private void loadmain()
        {
            Action act = () =>
            {
                _loadmain();
            };
            Dispatcher.BeginInvoke(act);
        }
        private void _loadmain()
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
        }
        private void close()
        {
            // Wait for 1.5 seconds to occur before closing
            /*while (!timer_GoneOff)
            {
            }*/

            Action act = () =>
            {
                _close();
            };
            Dispatcher.BeginInvoke(act);
        }
        private void _close()
        {
            this.Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Action act;
            //timer.Enabled = true;

            // Text
            act = () =>
            {
                updateprog("Loading iDecryptIt");
            };
            Dispatcher.BeginInvoke(act);
            
            // Load
            act = () =>
            {
                loadmain();
            };
            Dispatcher.BeginInvoke(act);

            // Close
            act = () =>
            {
                close();
            };
            Dispatcher.BeginInvoke(act);
        }
    }
}
