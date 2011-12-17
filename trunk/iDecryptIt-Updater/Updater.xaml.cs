using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Diagnostics;

namespace iDecryptIt_Updater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string tempdir = System.IO.Path.GetTempPath() + "\\Cole Stuff\\iDecryptIt\\";
        string rundir = Directory.GetCurrentDirectory() + "\\";
        string contacturl = "http://theiphonewiki.com/wiki/index.php?title=User:Balloonhead66/Latest_stable_software_release/iDecryptIt&action=raw";
        string checker;
        string major = "5";
        string minor = "10";
        string rev = "0";
        string build = "2B39";
        string updatemajor;
        string updateminor;
        string updaterev;
        string updatebuild;

        Run run = new Run();

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, EventArgs e)
        {
            // Because the updater downloads to .exe.new after extraction,
            // if that file exists, delete the .exe and rename the .exe.new to .exe
            if (File.Exists(rundir + "iDecryptIt.exe.new"))
            {
                // Kill iDecryptIt
                /*Process[] runningapps = Process.GetProcesses();
                foreach (Process p in runningapps)
                {
                    if (p.ProcessName == "iDecryptIt" || p.ProcessName == "iDecryptIt.vshost")
                    {
                        p.Kill();
                        while (!p.HasExited)
                        {
                        }
                    }
                }
                try
                {
                    File.Delete(rundir + "iDecryptIt.exe");
                    while (File.Exists(rundir + "iDecryptIt.exe"))
                    {
                    }
                    File.Move(rundir + "iDecryptIt.exe.new", rundir + "iDecryptIt.exe");

                    // Relaunch iDecryptIt
                    run.DoCMD("iDecryptIt.exe", false);
                    Close();
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show(
                        "Unable to delete iDecryptIt!\n\n" +
                        "Delete \"iDecryptIt.exe\" and move\n\"iDecryptIt.exe.new\" to \"iDecryptIt.exe\"\n\n" +
                        "Exception: " + ex.Message,
                        "iDecryptIt Updater",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    Close();
                }*/
            }

            btnTop.Visibility = Visibility.Hidden;

            // Download the raw code
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFile(contacturl, @tempdir + "update.txt");
                checker = File.ReadAllText(tempdir + "update.txt");
            }
            catch
            {
                MessageBox.Show("Unable to contact the iPhone Wiki to download version info!", "ERROR!", MessageBoxButton.OK);
                Close();
            }
            string[] checkerArr = checker.Split('.');
            updatemajor = checkerArr[0];
            updateminor = checkerArr[1];
            updaterev = checkerArr[2];
            updatebuild = checkerArr[3];
            
            // Compare
            if (build == updatebuild)
            {
                Close();
            }
            this.Title = "Update Available";
            this.txtHeader.Text = "Update Available";
            this.txtInstalled.Text = "Installed version: " + major + "." + minor + "." + rev + " (Build " + build + ")";
            this.txtAvailable.Text = "Latest version: " + updatemajor + "." + updateminor + "." + updaterev + " (Build " + updatebuild + ")";
        }
        private void btnBottom_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void btnTop_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
