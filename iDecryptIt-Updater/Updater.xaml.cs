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

namespace iDecryptIt_Updater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] doc;
        string tempdir = System.IO.Path.GetTempPath() + "\\Cole Stuff\\iDecryptIt\\";
        string rundir = Directory.GetCurrentDirectory() + "\\";
        string contacturl = "http://theiphonewiki.com/wiki/index.php?title=User:Balloonhead66/Latest_stable_software_release/iDecryptIt&action=raw";
        string checker;
        string major = "5";
        string updatemajor;
        string minor = "10";
        string updateminor;
        string rev = "0";
        string updaterev;
        string build = "1J59";
        string updatebuild;
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
                File.Delete(rundir + "iDecryptIt.exe.new");
                File.Move(rundir + "iDecryptIt.exe.new", rundir + "iDecryptIt.exe");
            }
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
