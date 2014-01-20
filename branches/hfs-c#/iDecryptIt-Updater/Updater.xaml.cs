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
    public partial class MainWindow : Window
    {
        static string newVersionFolder = Path.Combine(
            Path.GetTempPath(),
            "Hexware",
            "iDecryptIt-Update");
        string rundir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        string[] checkerArr;
        string[] installArr = new string[] {
            "5",
            "10",
            "0",
            "2B39"};

        public MainWindow()
        {
            this.Closing += MainWindow_Closing;

            //if (Folder.Exists(newVersionFolder))
            if (File.Exists(Path.Combine(rundir, "iDecryptIt.exe.new")))
            {
                try
                {
                    Process[] runningapps = Process.GetProcesses();
                    for (int i = 0; i < runningapps.Length; i++)
                    {
                        if (runningapps[i].ProcessName.Contains("iDecryptIt") &&
                            !runningapps[i].ProcessName.Contains("Updater") &&
                            !runningapps[i].ProcessName.Contains("vshost"))
                        {
                            // Don't kill the updater or the VS debugger
                            runningapps[i].Kill();
                        }
                    }
                }
                catch (Exception)
                {
                    this.Close();
                    return;
                }
                try
                {
                    File.Delete(rundir + "iDecryptIt.exe");
                    File.Move(rundir + "iDecryptIt.exe.new", rundir + "iDecryptIt.exe");

                    // Relaunch iDecryptIt
                    Process.Start("iDecryptIt.exe");
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "An unknown error has occured.\n\n" +
                        "Delete \"iDecryptIt.exe\" and move\n\"iDecryptIt.exe.new\" to \"iDecryptIt.exe\"\n\n" +
                        "Exception: " + ex.Message,
                        "iDecryptIt - Unknown error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }

                InitializeComponent();
            }
        }
        private void Window_Loaded(object sender, EventArgs e)
        {
            btnDownload.Visibility = Visibility.Hidden;

            // Download the version
            try
            {
                WebClient webClient = new WebClient();
                byte[] buf = webClient.DownloadData(
                    @"http://theiphonewiki.com/w/index.php?title=User:5urd/Latest_stable_software_release/iDecryptIt&action=raw");
                webClient.Dispose();
                checkerArr = File.ReadAllText(Path.Combine(tempdir, "update.txt")).Split('.');
            }
#if DEBUG
            catch (Exception ex)
#else
            catch (Exception)
#endif
            {
                this.Close();
                return;
            }
            
            // Compare build numbers
            if (installArr[3] == checkerArr[3])
            {
                this.Close();
                return;
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
            this.Height = this.Height + progDownload.Height; // + 27;
        }
    }
}