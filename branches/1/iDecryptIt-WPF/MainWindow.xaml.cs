using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

namespace iDecryptIt_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Strings
        string wantedlang;
        string nokey = "None Published";
        string unavailable = "Build not available for this device";
        int result;
        // What is this
        string[] strArr;
        int count;
        string device;
        string device2;
        string version;
        string build;
        // File paths
        string rundir = Directory.GetCurrentDirectory() + "\\";
        string tempdir = System.IO.Path.GetTempPath() + "idecryptit\\"; // System.IO Required as Path can mean directory or drawing
        string helpdir = Directory.GetCurrentDirectory() + "\\help\\";
        // Code
        public MainWindow()
        {
            InitializeComponent();
        }
        private void clear()
        {
            clear_keys();
            clear_dmgs();
        }
        private void clear_keys()
        {
            // 1.x Final
            key1a543a.Text = unavailable;
            key1c25.Text = unavailable;
            key1c28.Text = unavailable;
            key3a100a.Text = unavailable;
            key3a101a.Text = unavailable;
            key3a109a.Text = unavailable;
            key3a110a.Text = unavailable;
            key3b48b.Text = unavailable;
            key4a93.Text = unavailable;
            key4a102.Text = unavailable;
            key4b1.Text = unavailable;
            // 1.x Beta
            key5a147p.Text = unavailable;
            // 2.x Final
            key5a345final.Text = unavailable;
            key5a347.Text = unavailable;
            key5b108.Text = unavailable;
            key5c1.Text = unavailable;
            key5f136.Text = unavailable;
            key5f137.Text = unavailable;
            key5f138.Text = unavailable;
            key5g77.Text = unavailable;
            key5g77a.Text = unavailable;
            key5h11.Text = unavailable;
            key5h11a.Text = unavailable;
            // 2.x Beta
            key5a225c.Text = unavailable;
            key5a240d.Text = unavailable;
            key5a258f.Text = unavailable;
            key5a274d.Text = unavailable;
            key5a292g.Text = unavailable;
            key5a308.Text = unavailable;
            key5a331.Text = unavailable;
            key5a345beta.Text = unavailable;
            key5f90.Text = unavailable;
            key5g27.Text = unavailable;
            // 3.x Final
            key7a341.Text = unavailable;
            key7a400.Text = unavailable;
            key7c144.Text = unavailable;
            key7c145.Text = unavailable;
            key7c146.Text = unavailable;
            key7d11.Text = unavailable;
            key7e18.Text = unavailable;
            key7b367.Text = unavailable;
            key7b405.Text = unavailable;
            key7b500.Text = unavailable;
            // 3.x Beta
            key7a238j.Text = unavailable;
            key7a259g.Text = unavailable;
            key7a280f.Text = unavailable;
            key7a300g.Text = unavailable;
            key7a312g.Text = unavailable;
            key7c97d.Text = unavailable;
            key7c106c.Text = unavailable;
            key7c116a.Text = unavailable;
            // 4.x Final
            key8a293final.Text = unavailable;
            key8a306.Text = unavailable;
            key8a400.Text = unavailable;
            key8b117.Text = unavailable;
            key8b118.Text = unavailable;
            key8c148final.Text = unavailable;
            key8c148a.Text = unavailable;
            key8e128.Text = unavailable;
            key8e200.Text = unavailable;
            key8e303.Text = unavailable;
            key8e401.Text = unavailable;
            key8e501.Text = unavailable;
            key8e600.Text = unavailable;
            key8f190final.Text = unavailable;
            key8f191.Text = unavailable;
            key8g4.Text = unavailable;
            key8h7.Text = unavailable;
            key8h8.Text = unavailable;
            key8j2.Text = unavailable;
            key8j3.Text = unavailable;
            key8k2.Text = unavailable;
            key8l1.Text = unavailable;
            // 4.x Final ATV
            key8m89.Text = unavailable;
            key8c150.Text = unavailable;
            key8c154.Text = unavailable;
            key8f191m.Text = unavailable;
            key8f202.Text = unavailable;
            key8f305.Text = unavailable;
            key8f455.Text = unavailable;
            key9a334v.Text = unavailable;
            key9a335a.Text = unavailable;
            key9a336a.Text = unavailable;
            // 4.x Beta
            key8a230m.Text = unavailable;
            key8a248c.Text = unavailable;
            key8a260b.Text = unavailable;
            key8a274b.Text = unavailable;
            key8a293beta.Text = unavailable;
            key8b5080.Text = unavailable;
            key8b5080c.Text = unavailable;
            key8b5091b.Text = unavailable;
            key8c5091e.Text = unavailable;
            key8c5101c.Text = unavailable;
            key8c5115c.Text = unavailable;
            key8c134.Text = unavailable;
            key8c134b.Text = unavailable;
            key8c148beta.Text = unavailable;
            key8f5148b.Text = unavailable;
            key8f5153d.Text = unavailable;
            key8f5166b.Text = unavailable;
            key8f190beta.Text = unavailable;
            // 4.x Beta ATV
            key8f5148cATV.Text = unavailable;
            key8f5153dATV.Text = unavailable;
            key8f5166bATV.Text = unavailable;
            key9a5220pATV.Text = unavailable;
            key9a5248dATV.Text = unavailable;
            key9a5259fATV.Text = unavailable;
            key9a5288dATV.Text = unavailable;
            key9a5302bATV.Text = unavailable;
            key9a5313eATV.Text = unavailable;
            // 5.x Final
            key9a334final.Text = unavailable;
            // 5.x Beta
            key9a5220p.Text = unavailable;
            key9a5248d.Text = unavailable;
            key9a5259f.Text = unavailable;
            key9a5274d.Text = unavailable;
            key9a5288d.Text = unavailable;
            key9a5302b.Text = unavailable;
            key9a5313e.Text = unavailable;
            key9a334beta.Text = unavailable;
            key9a402.Text = unavailable;
            key9a404.Text = unavailable;
        }
        private void clear_dmgs()
        {
            //  1.x Final
            dmg1a543a.Text = "XXX-XXXX-XXX.dmg";
            dmg1c25.Text = "XXX-XXXX-XXX.dmg";
            dmg1c28.Text = "XXX-XXXX-XXX.dmg";
            dmg3a100a.Text = "XXX-XXXX-XXX.dmg";
            dmg3a101a.Text = "XXX-XXXX-XXX.dmg";
            dmg3a109a.Text = "XXX-XXXX-XXX.dmg";
            dmg3a110a.Text = "XXX-XXXX-XXX.dmg";
            dmg3b48b.Text = "XXX-XXXX-XXX.dmg";
            dmg4a93.Text = "XXX-XXXX-XXX.dmg";
            dmg4a102.Text = "XXX-XXXX-XXX.dmg";
            dmg4b1.Text = "XXX-XXXX-XXX.dmg";
            // 1.x Beta
            dmg5a147p.Text = "XXX-XXXX-XXX.dmg";
            // 2.x Final
            dmg5a345final.Text = "XXX-XXXX-XXX.dmg";
            dmg5a347.Text = "XXX-XXXX-XXX.dmg";
            dmg5b108.Text = "XXX-XXXX-XXX.dmg";
            dmg5c1.Text = "XXX-XXXX-XXX.dmg";
            dmg5f136.Text = "XXX-XXXX-XXX.dmg";
            dmg5f137.Text = "XXX-XXXX-XXX.dmg";
            dmg5f138.Text = "XXX-XXXX-XXX.dmg";
            dmg5g77.Text = "XXX-XXXX-XXX.dmg";
            dmg5g77a.Text = "XXX-XXXX-XXX.dmg";
            dmg5h11.Text = "XXX-XXXX-XXX.dmg";
            dmg5h11a.Text = "XXX-XXXX-XXX.dmg";
            // 2.x Beta
            dmg5a225c.Text = "XXX-XXXX-XXX.dmg";
            dmg5a240d.Text = "XXX-XXXX-XXX.dmg";
            dmg5a258f.Text = "XXX-XXXX-XXX.dmg";
            dmg5a274d.Text = "XXX-XXXX-XXX.dmg";
            dmg5a292g.Text = "XXX-XXXX-XXX.dmg";
            dmg5a308.Text = "XXX-XXXX-XXX.dmg";
            dmg5a331.Text = "XXX-XXXX-XXX.dmg";
            dmg5a345beta.Text = "XXX-XXXX-XXX.dmg";
            dmg5f90.Text = "XXX-XXXX-XXX.dmg";
            dmg5g27.Text = "XXX-XXXX-XXX.dmg";
            // 3.x Final
            dmg7a341.Text = "XXX-XXXX-XXX.dmg";
            dmg7a400.Text = "XXX-XXXX-XXX.dmg";
            dmg7c144.Text = "XXX-XXXX-XXX.dmg";
            dmg7c145.Text = "XXX-XXXX-XXX.dmg";
            dmg7c146.Text = "XXX-XXXX-XXX.dmg";
            dmg7d11.Text = "XXX-XXXX-XXX.dmg";
            dmg7e18.Text = "XXX-XXXX-XXX.dmg";
            dmg7b367.Text = "XXX-XXXX-XXX.dmg";
            dmg7b405.Text = "XXX-XXXX-XXX.dmg";
            dmg7b500.Text = "XXX-XXXX-XXX.dmg";
            // 3.x Beta
            dmg7a238j.Text = "XXX-XXXX-XXX.dmg";
            dmg7a259g.Text = "XXX-XXXX-XXX.dmg";
            dmg7a280f.Text = "XXX-XXXX-XXX.dmg";
            dmg7a300g.Text = "XXX-XXXX-XXX.dmg";
            dmg7a312g.Text = "XXX-XXXX-XXX.dmg";
            dmg7c97d.Text = "XXX-XXXX-XXX.dmg";
            dmg7c106c.Text = "XXX-XXXX-XXX.dmg";
            dmg7c116a.Text = "XXX-XXXX-XXX.dmg";
            // 4.x Final
            dmg8a293final.Text = "XXX-XXXX-XXX.dmg";
            dmg8a306.Text = "XXX-XXXX-XXX.dmg";
            dmg8a400.Text = "XXX-XXXX-XXX.dmg";
            dmg8b117.Text = "XXX-XXXX-XXX.dmg";
            dmg8b118.Text = "XXX-XXXX-XXX.dmg";
            dmg8c148final.Text = "XXX-XXXX-XXX.dmg";
            dmg8c148a.Text = "XXX-XXXX-XXX.dmg";
            dmg8e128.Text = "XXX-XXXX-XXX.dmg";
            dmg8e200.Text = "XXX-XXXX-XXX.dmg";
            dmg8e303.Text = "XXX-XXXX-XXX.dmg";
            dmg8e401.Text = "XXX-XXXX-XXX.dmg";
            dmg8e501.Text = "XXX-XXXX-XXX.dmg";
            dmg8e600.Text = "XXX-XXXX-XXX.dmg";
            dmg8f190final.Text = "XXX-XXXX-XXX.dmg";
            dmg8f191.Text = "XXX-XXXX-XXX.dmg";
            dmg8g4.Text = "XXX-XXXX-XXX.dmg";
            dmg8h7.Text = "XXX-XXXX-XXX.dmg";
            dmg8h8.Text = "XXX-XXXX-XXX.dmg";
            dmg8j2.Text = "XXX-XXXX-XXX.dmg";
            dmg8j3.Text = "XXX-XXXX-XXX.dmg";
            dmg8k2.Text = "XXX-XXXX-XXX.dmg";
            dmg8l1.Text = "XXX-XXXX-XXX.dmg";
            // 4.x Final ATV
            dmg8m89.Text = "XXX-XXXX-XXX.dmg";
            dmg8c150.Text = "XXX-XXXX-XXX.dmg";
            dmg8c154.Text = "XXX-XXXX-XXX.dmg";
            dmg8f191m.Text = "XXX-XXXX-XXX.dmg";
            dmg8f202.Text = "XXX-XXXX-XXX.dmg";
            dmg8f305.Text = "XXX-XXXX-XXX.dmg";
            dmg8f455.Text = "XXX-XXXX-XXX.dmg";
            dmg9a334v.Text = "XXX-XXXX-XXX.dmg";
            dmg9a335a.Text = "XXX-XXXX-XXX.dmg";
            dmg9a336a.Text = "XXX-XXXX-XXX.dmg";
            // 4.x Beta
            dmg8a230m.Text = "XXX-XXXX-XXX.dmg";
            dmg8a248c.Text = "XXX-XXXX-XXX.dmg";
            dmg8a260b.Text = "XXX-XXXX-XXX.dmg";
            dmg8a274b.Text = "XXX-XXXX-XXX.dmg";
            dmg8a293beta.Text = "XXX-XXXX-XXX.dmg";
            dmg8b5080.Text = "XXX-XXXX-XXX.dmg";
            dmg8b5080c.Text = "XXX-XXXX-XXX.dmg";
            dmg8b5091b.Text = "XXX-XXXX-XXX.dmg";
            dmg8c5091e.Text = "XXX-XXXX-XXX.dmg";
            dmg8c5101c.Text = "XXX-XXXX-XXX.dmg";
            dmg8c5115c.Text = "XXX-XXXX-XXX.dmg";
            dmg8c134.Text = "XXX-XXXX-XXX.dmg";
            dmg8c134b.Text = "XXX-XXXX-XXX.dmg";
            dmg8c148beta.Text = "XXX-XXXX-XXX.dmg";
            dmg8f5148b.Text = "XXX-XXXX-XXX.dmg";
            dmg8f5153d.Text = "XXX-XXXX-XXX.dmg";
            dmg8f5166b.Text = "XXX-XXXX-XXX.dmg";
            dmg8f190beta.Text = "XXX-XXXX-XXX.dmg";
            // 4.x Beta ATV
            dmg8f5148cATV.Text = "XXX-XXXX-XXX.dmg";
            dmg8f5153dATV.Text = "XXX-XXXX-XXX.dmg";
            dmg8f5166bATV.Text = "XXX-XXXX-XXX.dmg";
            dmg9a5220pATV.Text = "XXX-XXXX-XXX.dmg";
            dmg9a5248dATV.Text = "XXX-XXXX-XXX.dmg";
            dmg9a5259fATV.Text = "XXX-XXXX-XXX.dmg";
            dmg9a5288dATV.Text = "XXX-XXXX-XXX.dmg";
            dmg9a5302bATV.Text = "XXX-XXXX-XXX.dmg";
            dmg9a5313eATV.Text = "XXX-XXXX-XXX.dmg";
            // 5.x Final
            dmg9a334final.Text = "XXX-XXXX-XXX.dmg";
            // 5.x Beta
            dmg9a5220p.Text = "XXX-XXXX-XXX.dmg";
            dmg9a5248d.Text = "XXX-XXXX-XXX.dmg";
            dmg9a5259f.Text = "XXX-XXXX-XXX.dmg";
            dmg9a5274d.Text = "XXX-XXXX-XXX.dmg";
            dmg9a5288d.Text = "XXX-XXXX-XXX.dmg";
            dmg9a5302b.Text = "XXX-XXXX-XXX.dmg";
            dmg9a5313e.Text = "XXX-XXXX-XXX.dmg";
            dmg9a334beta.Text = "XXX-XXXX-XXX.dmg";
            dmg9a402.Text = "XXX-XXXX-XXX.dmg";
            dmg9a404.Text = "XXX-XXXX-XXX.dmg";
        }
        private void btnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            if (textInputFileName.Text == "")
            {
                MessageBox.Show("Make sure there is an input file!", "Something went wrong!", MessageBoxButton.OK);
                return;
            }
            if (textOuputFileName.Text == "")
            {
                MessageBox.Show("Make sure there is an output file!", "Something went wrong!", MessageBoxButton.OK);
                return;
            }
            if (textDecryptKey.Text == "")
            {
                MessageBox.Show("Make sure there is a key inputed!", "Something went wrong!", MessageBoxButton.OK);
                return;
            }
            DoCMD(rundir + "\\vfdecrypt.exe",
                  " -i \"" + textInputFileName.Text + "\"" +
                  " -k " + textDecryptKey.Text + 
                  " -o \"" + textOuputFileName.Text + "\""
                  );
            MessageBox.Show("Decrypting Done!", "Done!", MessageBoxButton.OK);
        }
        private void btnSelectVFDecryptInputFile_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnClearKey_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnExtract_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnSelect7ZInputFile_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btniPad11_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btniPad21_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btniPad22_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btniPad23_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btniPhone11_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btniPhone12_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btniPhone21_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btniPhone31_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btniPhone33_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btniPhone41_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btniPod11_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btniPod21_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btniPod31_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btniPod41_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnAppleTV21_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnSelectWhatAmIFile_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnWhatAmI_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("file://" + helpdir + "about_iDecryptIt.html");
        }
        private void btnREADME_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("file://" + helpdir + "README.html");
        }
        private void btnChangelog_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("file://" + helpdir + "changelog.html");
        }
        private void btnHelpOut_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("file://" + helpdir + "submitkey.html");
        }
        private void btnChangeLanguage_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btn1a420_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://rapidshare.com/files/207764160/iphoneproto.zip");
        }
        static void DoCMD(string prog)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = prog;
            p.StartInfo.Arguments = "";
            p.Start();
            p.WaitForExit();
        }
        static void DoCMD(string prog, string args)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = prog;
            p.StartInfo.Arguments = args;
            p.Start();
            p.WaitForExit();
        }
    }
}
