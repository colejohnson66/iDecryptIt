using Hexware.DataManipulation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;

namespace Hexware.Programs.iDecryptIt
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern bool FreeConsole();

        //Ini l18n;

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
            GlobalVars.executionargs = Environment.GetCommandLineArgs();
        }
        private void goconsole()
        {
            ConsoleVersion.Main(GlobalVars.executionargs);
        }
        private void updateprog(string text)
        {
            lblProg.Content = text;
        }
        private void loadmain()
        {
            new MainWindow().Show();
        }
        private void close()
        {
            this.Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Action act;

            // Is this console
            int length = GlobalVars.executionargs.Length;
            for (int i = 0; i < length; i++)
            {
                if (GlobalVars.executionargs[i] == "/console")
                {
                    // Go Console
                    act = () =>
                    {
                        goconsole();
                    };
                    Dispatcher.BeginInvoke(act);
                    this.Close();
                    return;
                }
            }

            // Close console
            if (FreeConsole() == true)
            {
                // Console failed to close
            }

            // Show Window
            Opacity = 100;

            // Localize this window
            /*string wantedlang;
            RegistryKey langcode;
            langcode = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Hexware\\iDecryptIt", true);
            if (langcode != null)
            {
                wantedlang = langcode.GetValue("language").ToString();
                switch (wantedlang)
                {
                    // Spanish
                    case "spa":
                        l18n = new Ini(Directory.GetCurrentDirectory() + @"\l18n\spa.ini");
                        Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Hexware\\iDecryptIt", "language", "spa", RegistryValueKind.String);
                        break;

                    default:
                        l18n = new Ini(Directory.GetCurrentDirectory() + @"\l18n\eng.ini");
                        break;
                }
            }*/

            // File Name
            if (GlobalVars.executionargs != null)
            {
                act = () =>
                {
                    updateprog("Grabbing DMG");
                };
                Dispatcher.BeginInvoke(act);
            }

            act = () =>
            {
                updateprog("Loading iDecryptIt");
            };
            Dispatcher.BeginInvoke(act);

            act = () =>
            {
                loadmain();
            };
            Dispatcher.BeginInvoke(act);

            act = () =>
            {
                updateprog("Enjoy!");
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