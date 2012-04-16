using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows;

namespace Hexware.Programs.iDecryptIt.Updater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string tempdir = Path.Combine(Path.GetTempPath() + "Hexware\\iDecryptIt") + "\\";
        string rundir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\";
        string[] checkerArr;
        string[] installArr = new string[4] {
            "5",
            "10",
            "0",
            "2B38"};

        public MainWindow()
        {
            if (File.Exists(rundir + "iDecryptIt.exe.new"))
            {
                // Kill iDecryptIt
                Process[] runningapps = Process.GetProcesses();
                foreach (Process p in runningapps)
                {
                    if (p.ProcessName.Contains("iDecryptIt") &&
                        !p.ProcessName.Contains("Updater") &&
                        !p.ProcessName.Contains("vshost"))
                    {
                        // ONLY on main iDecryptIt (not Updater or Debugger)
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
                    Process.Start("iDecryptIt.exe");
                    Environment.Exit(1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Unknown error!\n\n" +
                        "Delete \"iDecryptIt.exe\" and move\n\"iDecryptIt.exe.new\" to \"iDecryptIt.exe\"\n\n" +
                        "Exception: " + ex.Message,
                        "iDecryptIt",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }

                InitializeComponent();
            }
        }
        private void Window_Loaded(object sender, EventArgs e)
        {
            btnDownload.Visibility = Visibility.Hidden;

            // Download the raw code
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFile(
                    @"http://theiphonewiki.com/wiki/index.php?title=User:5urd/Latest_stable_software_release/iDecryptIt&action=raw",
                    tempdir + "update.txt");
                webClient.Dispose();
                checkerArr = File.ReadAllText(tempdir + "update.txt").Split('.');
            }
            catch (Exception ex)
            {
                /*MessageBox.Show(
                    "Unable to download version info!\n\n" +
                    "Exception: " + ex.Message,
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);*/
                Environment.Exit(-1);
            }
            
            // Compare
            if (installArr[3] == checkerArr[3])
            {
                Close();
            }
            this.Title = "Update Available";
            txtHeader.Text = "Update Available";
            txtInstalled.Text = "Installed version: " + installArr[0] + "." + installArr[1] + "." + installArr[2] + " (Build " + installArr[3] + ")";
            txtAvailable.Text = "Latest version: " + checkerArr[0] + "." + checkerArr[1] + "." + checkerArr[2] + " (Build " + checkerArr[3] + ")";
            btnDownload.Visibility = Visibility.Visible;
        }
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            btnDownload.IsEnabled = false;
            btnOk.IsEnabled = false;
            this.Height = this.Height + 30;
        }
    }
}
