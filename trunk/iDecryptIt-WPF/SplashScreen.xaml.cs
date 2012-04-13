using Hexware.DataManipulation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Threading;
using System.IO;

namespace Hexware.Programs.iDecryptIt
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        Ini l18n;
        string[] global = null;

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
            global = Environment.GetCommandLineArgs();
        }
        private void goconsole()
        {
            ConsoleVersion.Main();
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
            int length = global.Length;
            for (int i = 0; i < length; i++)
            {
                if (global[i] == "/console")
                {
                    // Go Console
                    act = () =>
                    {
                        goconsole();
                    };
                    Dispatcher.BeginInvoke(act);
                    Close(); // Close the window
                    return; // Then end
                }
            }

            // Show Window
            Opacity = 100;

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
                        l18n = new Ini(Directory.GetCurrentDirectory() + @"\l18n\spa.ini");
                        Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Cole Stuff\\iDecryptIt", "language", "spa", RegistryValueKind.String);
                        break;

                    default:
                        l18n = new Ini(Directory.GetCurrentDirectory() + @"\l18n\eng.ini");
                        break;
                }
            }

            // File Name
            if (global != null)
            {
                act = () =>
                {
                    updateprog(l18n.IniReadValue("SplashScreen", "grabdmg"));
                };
                Dispatcher.BeginInvoke(act);
                GlobalVars.executionargs = global;
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
