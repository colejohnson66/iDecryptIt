using ColeStuff.DataManipulation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Threading;
using System.IO;

namespace iDecryptIt_WPF
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        INI l18n;
        string global = null;

        public SplashScreen()
        {
            // Grab .NET version
            RegistryKey installed_versions = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP");
            string[] version_names = installed_versions.GetSubKeyNames();
            double Framework = Convert.ToDouble(
                version_names[version_names.Length - 1].Remove(0, 1),
                CultureInfo.InvariantCulture);
            //int SP = Convert.ToInt32(installed_versions.OpenSubKey(version_names[version_names.Length - 1]).GetValue("SP", 0));

            InitializeComponent();

            if (Framework < 4.0)
            {
                MessageBox.Show(
                    "You need .NET 4.0 or better to run iDecryptIt\n\nYou can download it at www.microsoft.com\nor through Windows Update",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                Environment.Exit(-1); // this.Close() does not work if it has not been initialized
            }

            // If all goes well, grab command line options
            string[] commandlines = Environment.GetCommandLineArgs();
            if (commandlines.Length >= 2)
            {
                global = commandlines[1];
            }
        }
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
            // Localize this window
            string wantedlang;
            RegistryKey langcode;
            langcode = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Cole Stuff\\iDecryptIt", true);
            if (langcode != null)
            {
                wantedlang = langcode.GetValue("language").ToString();
                switch (wantedlang)
                {
                    // Spanish
                    case "spa":
                        l18n = new INI(Directory.GetCurrentDirectory() + @"\l18n\spa.ini");
                        Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Cole Stuff\\iDecryptIt", "language", "spa", RegistryValueKind.String);
                        break;

                    default:
                        l18n = new INI(Directory.GetCurrentDirectory() + @"\l18n\eng.ini");
                        break;
                }
            }


            Action act;

            // File Name
            if (global != null)
            {
                act = () =>
                {
                    updateprog(l18n.IniReadValue("SplashScreen", "grabdmg"));
                };
                Dispatcher.BeginInvoke(act);
                GlobalClass.GlobalVar = global;
            }

            act = () =>
            {
                updateprog(l18n.IniReadValue("SplashScreen", "loading"));
            };
            Dispatcher.BeginInvoke(act);
            
            act = () =>
            {
                loadmain();
            };
            Dispatcher.BeginInvoke(act);

            act = () =>
            {
                updateprog(l18n.IniReadValue("SplashScreen", "enjoy"));
            };
            Dispatcher.BeginInvoke(act);

            act = () =>
            {
                close();
            };
            Dispatcher.BeginInvoke(act);
        }
    }
}
