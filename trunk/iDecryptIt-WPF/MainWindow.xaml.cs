using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace Hexware.Programs.iDecryptIt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Strings
        //string wantedlang;
        // File paths
        static string rundir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\";
        static string tempdir = Path.Combine(
            Path.GetTempPath(),
            "Hexware\\iDecryptIt_",
            new Random().Next(0, Int32.MaxValue).ToString("X")) + "\\";
        static string helpdir = rundir + "help\\";
        // XPwn's dmg
        BackgroundWorker decryptworker;
        Process decryptproc;
        FileInfo decryptfromfile;
        string decryptfrom;
        string decryptto;
        double decryptprog;
        // INI files
        //XmlDocument l18n = new XmlDocument();
        
        internal MainWindow()
        {
            InitializeComponent();
        }

        // Background workers
        private void decryptworker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!decryptworker.CancellationPending)
            {
                if (decryptproc.HasExited)
                {
                    decryptworker.ReportProgress(100);
                }
                else
                {
                    decryptprog = ((new FileInfo(decryptto).Length) * 100.0) / decryptfromfile.Length;
                    decryptworker.ReportProgress(0);
                }
            }
        }
        private void decryptworker_ProgressReported(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 100)
            {
                decryptworker.CancelAsync();
                progDecrypt.Value = 100;
                gridDecrypt.IsEnabled = true;
                progDecrypt.Visibility = Visibility.Hidden;
            }
            progDecrypt.Value = decryptprog;
        }

        // Functions
        private void cleanup()
        {
            try
            {
                if (Directory.Exists(tempdir))
                {
                    Directory.Delete(tempdir, true);
                }
                if (Directory.Exists(System.IO.Path.GetTempPath() + "Hexware\\iDecryptIt-Setup\\"))
                {
                    Directory.Delete(System.IO.Path.GetTempPath() + "Hexware\\iDecryptIt-Setup\\", true);
                }
            }
            catch (Exception)
            {
                // don't error here. it's just a temp directory
            }
        }
        internal void Error(string message, Exception ex)
        {
            MessageBox.Show(
                message + "\r\n\r\n" +
                "Please file a bug report\r\n" +// at:\r\n" +
                "http://twitter.com/HexwareLLC\r\n" +
                "With the following data and an explanation of what was happening:\r\n\r\n" +
                (ex == null ? "" : "Exception: " + ex.Message + "\r\n") +
                "Version: " + GlobalVars.Version + "\r\n",
                "iDecryptIt",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        internal Stream GetStream(string resourceName)
        {
            try
            {
                Assembly assy = Assembly.GetExecutingAssembly();
                string[] resources = assy.GetManifestResourceNames();
                int length = resources.Length;
                for (int i = 0; i < length; i++)
                {
                    if (resources[i].ToLower().IndexOf(resourceName.ToLower()) != -1)
                    {
                        // resource found
                        return assy.GetManifestResourceStream(resources[i]);
                    }
                }
                Error("Key page not found.", null);
            }
            catch (Exception ex)
            {
                Error("Unable to open key page.", ex);
            }
            return Stream.Null;
        }

        // Clicks and Stuff
        /*private void btnChangeLanguage_Click(object sender, RoutedEventArgs e)
        {
            new SelectLangControl(this).ShowDialog();
        }*/
        private void btnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            #region Is data filled?
            if (String.IsNullOrWhiteSpace(textInputFileName.Text))
            {
                MessageBox.Show(
                    "Make sure there is an input file!",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (String.IsNullOrWhiteSpace(textOuputFileName.Text))
            {
                MessageBox.Show(
                    "Make sure there is an output file!",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (String.IsNullOrWhiteSpace(textDecryptKey.Text))
            {
                MessageBox.Show(
                    "Make sure there is a key!",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (!File.Exists(textInputFileName.Text))
            {
                MessageBox.Show(
                    "The input file does not exist!",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (File.Exists(textOuputFileName.Text))
            {
                if (MessageBox.Show(
                    "The output file already exists! Shall I delete it?",
                    "iDecryptIt",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return;
                }
                File.Delete(textOuputFileName.Text);
            }
            #endregion

            // Variables
            decryptfrom = textInputFileName.Text;
            decryptto = textOuputFileName.Text;

            // File length
            decryptfromfile = new FileInfo(decryptfrom);

            // Process
            decryptproc = new Process();
            decryptproc.StartInfo.UseShellExecute = false;
            decryptproc.StartInfo.FileName = rundir + "dmg.exe";
            decryptproc.StartInfo.RedirectStandardError = true;
            decryptproc.StartInfo.RedirectStandardInput = true;
            decryptproc.StartInfo.RedirectStandardOutput = true;
            decryptproc.StartInfo.Arguments =
                "extract \"" + textInputFileName.Text + "\" \"" + textOuputFileName.Text + "\" " + textDecryptKey.Text;
            decryptproc.Start();

            // Screen mods
            gridDecrypt.IsEnabled = false;
            progDecrypt.Visibility = Visibility.Visible;

            // Wait for file to exist before starting worker (processes are aSYNC)
            while (!File.Exists(decryptto))
            {
            }
            decryptworker = new BackgroundWorker();
            decryptworker.WorkerSupportsCancellation = true;
            decryptworker.WorkerReportsProgress = true;
            decryptworker.DoWork += decryptworker_DoWork;
            decryptworker.ProgressChanged += decryptworker_ProgressReported;
            decryptworker.RunWorkerAsync();
        }
        private void btnExtract_Click(object sender, RoutedEventArgs e)
        {
            #region Is data filled?
            if (String.IsNullOrWhiteSpace(text7ZInputFileName.Text))
            {
                MessageBox.Show(
                    "Make sure there is an input file.",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (!File.Exists(text7ZInputFileName.Text))
            {
                MessageBox.Show(
                    "The input file does not exist.",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (Directory.Exists(text7ZInputFileName.Text))
            {
                MessageBox.Show(
                    "The specified location is actually a directory.",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (String.IsNullOrWhiteSpace(text7ZOuputFolder.Text))
            {
                MessageBox.Show(
                    "Make sure there is an output directory selected.",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (File.Exists(text7ZOuputFolder.Text))
            {
                MessageBox.Show(
                    "The output folder is actually a file.",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            #endregion

            Process.Start(
                rundir + "7z.exe",
                " e \"" + text7ZInputFileName.Text + "\" \"" + "-o" + tempdir + "\"");

            #region Prepare to extract HFS
            string[] files = Directory.GetFiles(tempdir, "*.hfs*", SearchOption.AllDirectories);
            string file;
            if (files.Length == 1)
            {
                file = files[0];
            }
            else
            {
                MessageBox.Show(
                    "Please select the biggest file.",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                OpenFileDialog extract = new OpenFileDialog();
                extract.FileName = "";
                extract.Multiselect = false;
                extract.Filter = "All Files (*.*)|*.*";
                extract.InitialDirectory = tempdir;
                extract.ShowDialog();
                if (!String.IsNullOrWhiteSpace(extract.SafeFileName) && File.Exists(extract.FileName))
                {
                    file = extract.FileName;
                }
                else
                {
                    return;
                }
            }
            #endregion

            Process.Start(
                rundir + "7z.exe",
                " x \"" + file + "\" \"" + "-o" + text7ZOuputFolder.Text + "\"");
        }
        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            new About().ShowDialog();
        }
        private void btnChangelog_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("file://" + helpdir + "changelog.html");
        }
        private void btnREADME_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("file://" + helpdir + "README.html");
        }
        private void btnHelpOut_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("file://" + helpdir + "submitkey.html");
        }
        private void btn1a420_Click(object sender, RoutedEventArgs e)
        {
            // File removed from RapidShare
            MessageBox.Show(
                "Sorry, but that file is no longer available.",
                "iDecryptIt",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
            //Process.Start("http://rapidshare.com/files/207764160/iphoneproto.zip");
        }
        private void btnSelectVFDecryptInputFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog decrypt = new OpenFileDialog();
            decrypt.FileName = "";
            decrypt.RestoreDirectory = true;
            decrypt.DefaultExt = ".dmg";
            decrypt.Filter = "Apple Disk Images|*.dmg";
            decrypt.ShowDialog();
            if (!String.IsNullOrWhiteSpace(decrypt.SafeFileName))
            {
                textInputFileName.Text = decrypt.FileName;
            }
        }
        private void btnSelect7ZInputFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog extract = new OpenFileDialog();
            extract.FileName = "";
            extract.RestoreDirectory = true;
            extract.Multiselect = false;
            extract.DefaultExt = ".dmg";
            extract.Filter = "Apple Disk Images|*.dmg";//|Apple Firmware Files|*.ipsw";
            extract.ShowDialog();
            if (!String.IsNullOrWhiteSpace(extract.SafeFileName))
            {
                /*
                int length = extract.SafeFileName.Length - 1;
                if (extract.SafeFileName[length - 4] == '.' &&
                    extract.SafeFileName[length - 3] == 'd' &&
                    extract.SafeFileName[length - 2] == 'm' &&
                    extract.SafeFileName[length - 1] == 'g')
                {*/
                    string[] split = extract.FileName.Split('\\');
                    string returntext;
                    int lastindexnum = split.Length - 1;
                    returntext = split[0];
                    for (int i = 1; i < split.Length; i++)
                    {
                        if (i != lastindexnum)
                        {
                            returntext = returntext + '\\' + split[i];
                        }
                    }
                    text7ZOuputFolder.Text = returntext + '\\';
                /*}
                else if (extract.SafeFileName[length - 5] == '.' &&
                         extract.SafeFileName[length - 4] == 'i' &&
                         extract.SafeFileName[length - 3] == 'p' &&
                         extract.SafeFileName[length - 2] == 's' &&
                         extract.SafeFileName[length - 1] == 'w')
                {
                    string[] split = extract.FileName.Split('\\');
                    string returntext;
                    int lastindexnum = split.Length - 1;
                    returntext = split[0];
                    for (int i = 1; i < split.Length; i++)
                    {
                        if (i != lastindexnum)
                        {
                            returntext = returntext + '\\' + split[i];
                        }
                        else
                        {
                            // Put file name minus ".ipsw" in output
                        }
                    }
                    text7ZOuputFolder.Text = returntext + '\\';
                }
                else
                {
                    // Dunno how, but it happened
                    return;
                }*/

                text7ZInputFileName.Text = extract.FileName;
            }
        }
        private void btnSelectWhatAmIFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog what = new OpenFileDialog();
            what.FileName = "";
            what.RestoreDirectory = true;
            what.Multiselect = false;
            what.DefaultExt = ".ipsw";
            what.Filter = "Apple Firmware Files|*.ipsw";
            what.ShowDialog();
            if (!String.IsNullOrWhiteSpace(what.SafeFileName))
            {
                textWhatAmIFileName.Text = what.SafeFileName;
            }
        }
        private void btnWhatAmI_Click(object sender, RoutedEventArgs e)
        {
            // What we should do is open the archive and parse the Restore.plist file
            string[] strArr;
            string device;
            string version;
            string build;
            if (textWhatAmIFileName.Text == "")
            {
                return;
            }
            strArr = textWhatAmIFileName.Text.Split('_');
            if (strArr.Length == 4)
            {
                // If the last index is 'Restore.ipsw', proceed
                if (strArr[3] == "Restore.ipsw")
                {
                    device = strArr[0];
                    version = strArr[1];
                    build = strArr[2];
                    #region Device Switch
                    switch (device)
                    {
                        case "iPad1,1":
                            device = "iPad 1G Wi-Fi/Wi-Fi+3G";
                            break;
                        case "iPad2,1":
                            device = "iPad 2 Wi-Fi";
                            break;
                        case "iPad2,2":
                            device = "iPad 2 Wi-Fi+3G GSM";
                            break;
                        case "iPad2,3":
                            device = "iPad 2 Wi-Fi+3G CDMA";
                            break;
                        case "iPad2,4":
                            device = "iPad 2 Wi-Fi (R2)";
                            break;
                        case "iPad3,1":
                            device = "iPad 3 Wi-Fi";
                            break;
                        case "iPad3,2":
                            device = "iPad 3 Wi-Fi+3G CDMA";
                            break;
                        case "iPad3,3":
                            device = "iPad 3 Wi-Fi+3G Global";
                            break;
                        case "iPhone1,1":
                            device = "iPhone 2G";
                            break;
                        case "iPhone1,2":
                            device = "iPhone 3G";
                            break;
                        case "iPhone2,1":
                            device = "iPhone 3GS";
                            break;
                        case "iPhone3,1":
                            device = "iPhone 4 GSM";
                            break;
                        case "iPhone3,2":
                            device = "iPhone 4 GSM (R2)";
                            break;
                        case "iPhone3,3":
                            device = "iPhone 4 CDMA";
                            break;
                        case "iPhone4,1":
                            device = "iPhone 4S";
                            break;
                        case "iPhone5,1":
                            device = "iPhone 5 GSM";
                            break;
                        case "iPhone5,2":
                            device = "iPhone 5 CDMA";
                            break;
                        case "iPod1,1":
                            device = "iPod touch 1G";
                            break;
                        case "iPod2,1":
                            device = "iPod touch 2G";
                            break;
                        case "iPod3,1":
                            device = "iPod touch 3G";
                            break;
                        case "iPod4,1":
                            device = "iPod touch 4G";
                            break;
                        case "iPod5,1":
                            device = "iPod touch 5G";
                            break;
                        case "AppleTV2,1":
                            device = "Apple TV 2G";
                            #region Apple TV 2G
                            switch (build)
                            {
                                case "8M89":
                                    version = "4.0/4.1";
                                    break;
                                case "8C150":
                                    version = "4.1/4.2";
                                    break;
                                case "8C154":
                                    version = "4.1.1/4.2.1";
                                    break;
                                case "8F5148c":
                                case "8F5153d":
                                case "8F5166b":
                                case "8F191m":
                                    version = "4.2/4.3";
                                    break;
                                case "8F202":
                                    version = "4.2.1/4.3";
                                    break;
                                case "8F305":
                                    version = "4.2.2/4.3";
                                    break;
                                case "8F455":
                                    version = "4.3";
                                    break;
                                case "9A5220p":
                                case "9A5248d":
                                case "9A5259f":
                                case "9A5288d":
                                case "9A5302b":
                                case "9A5313e":
                                case "9A334v":
                                    version = "4.4/5.0";
                                    break;
                                case "9A335a":
                                    version = "4.4.1/5.0";
                                    break;
                                case "9A336a":
                                    version = "4.4.2/5.0";
                                    break;
                                case "9A405l":
                                    version = "4.4.3/5.0.1";
                                    break;
                                case "9A406a":
                                    version = "4.4.4/5.0.1";
                                    break;
                                case "9B5127c":
                                case "9B5141a":
                                case "9B179b":
                                    version = "5.0/5.1";
                                    break;
                                case "9B206f":
                                    version = "5.0.1/5.1";
                                    break;
                                case "9B830":
                                    version = "5.0.2/5.1";
                                    break;
                                case "10A5316k":
                                case "10A5338d":
                                case "10A5355d":
                                case "10A5376e":
                                    version = "6.0";
                                    break;
                                case "10A406e":
                                    version = "5.1/6.0";
                                    break;
                            }
                            #endregion
                            break;
                        case "AppleTV3,1":
                            device = "Apple TV 3G";
                            #region Apple TV 3G
                            switch (build)
                            {
                                case "9B179b":
                                    version = "5.0/5.1";
                                    break;
                                case "9B206f":
                                    version = "5.0.1/5.1";
                                    break;
                                case "9B830":
                                    version = "5.0.2/5.1";
                                    break;
                                case "10A5316k":
                                case "10A5338d":
                                case "10A5355d":
                                case "10A5376e":
                                    version = "6.0";
                                    break;
                                case "10A406e":
                                    version = "5.1/6.0";
                                    break;
                            }
                            #endregion
                            break;
                        default:
                            MessageBox.Show(
                                "The supplied device: '" + device + "' does not follow the format:\r\n" +
                                    "{iPad/iPhone/iPad/AppleTV}{#},{#}",
                                "iDecryptIt",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            return;
                    }
                    #endregion
                    MessageBox.Show(
                        "Device: " + device + "\r\n" +
                            "Version: " + version + "\r\n" +
                            "Build: " + build,
                        "iDecryptIt",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(
                        "The supplied IPSW File that was given is not in the following format:\r\n" +
                            "{DEVICE}_{VERSION}_{BUILD}_Restore.ipsw",
                        "iDecryptIt",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show(
                    "The supplied IPSW File that was given is not in the following format:\r\n" +
                        "{DEVICE}_{VERSION}_{BUILD}_Restore.ipsw",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
        }
        private void textInputFileName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] split = textInputFileName.Text.Split('\\');
            int length = split.Length;
            string lastindex = split[length - 1];
            string returntext = "";
            for (int i = 0; i < length; i++)
            {
                if (i == 0)
                {
                    returntext = split[0];
                }
                else if (i == length)
                {
                    returntext = returntext + "\\" + lastindex.Substring(0, split[length].Length - 4);
                    /*switch (wantedlang)
                    {
                        case "en":
                            returntext = returntext + "_decrypted.dmg";
                            break;

                        case "es":
                            returntext = returntext + "_descifrado.dmg";
                            break;

                        default:*/
                            // Fall back to English
                            returntext = returntext + "_decrypted.dmg";
                            //break;
                    //}
                }
                else
                {
                    returntext = returntext + "\\" + split[i];
                }
            }
            textOuputFileName.Text = returntext;
        }
        private void key_Click(object sender, RoutedEventArgs e)
        {
            // Remove "btn", then split
            string[] value = ((MenuItem)sender).Name.Substring(3).Split('_');
            bool gm = value[1].Contains("GM");
            if (gm)
            {
                value[1] = value[1].Substring(0, value[1].Length - 2);
            }

            // Add ',' between the two digits
            int length = value[0].Length - 1; // -1 for last digit (char)
            value[0] = value[0].Substring(0, length) + "," + value[0][length];

            // Load key page
            Stream stream = GetStream(value[0] + "_" + value[1] + ".plist");
            if (stream == Stream.Null)
            {
                MessageBox.Show(
                    "Sorry, but that version doesn't have any published keys.",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }
            new KeysControl(this, stream, false).Show(); // gm).Show();
        }

        // Load and Close
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // cleanup() is called before to cleanup any leftover crap in case of a crash
            cleanup();
            Directory.CreateDirectory(tempdir);

            // Because the updater downloads to .exe.new after extraction,
            // if that file exists, delete the .exe and rename the .exe.new to .exe
            // (This exe should have been killed during the update to allow updating of this exe)
            if (File.Exists(rundir + "iDecryptIt-Updater.exe.new"))
            {
                File.Delete(rundir + "iDecryptIt-Updater.exe.new");
                File.Move(rundir + "iDecryptIt-Updater.exe.new", rundir + "iDecryptIt-Updater.exe");
            }
            if (File.Exists(rundir + "iDecryptIt-Updater.exe"))
            {
                Process.Start(rundir + "iDecryptIt-Updater.exe");
            }

            /*RegistryKey langcode;
            langcode = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Hexware\\iDecryptIt", true);
            if (langcode == null)
            {
                new SelectLangControl(this).Show();
            }
            else
            {
                wantedlang = langcode.GetValue("language").ToString();
            }
            SetLangAtStartup();*/

            if (GlobalVars.ExecutionArgs.Length >= 2 &&
                GlobalVars.ExecutionArgs[1].Substring(0, GlobalVars.ExecutionArgs[1].Length - 4) == ".dmg")
            {
                textInputFileName.Text = GlobalVars.ExecutionArgs[1];
            }
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            cleanup();
            Environment.Exit(0); // Just in case
        }

        // Language Stuff
        /*private void SetLangAtStartup()
        {
            switch (wantedlang)
            {
                // English
                case "eng":
                    break;

                // Spanish
                case "spa":
                    setlang("spa");
                    break;

                default:
                    new SelectLangControl(this).Show();
                    break;
            }
        }
        internal void setlang(string lang)
        {
            if (!File.Exists(rundir + "l18n\\" + lang + ".xml"))
            {
                throw new FileNotFoundException(
                    "The supplied l18n code \"" + lang + "\" was not found",
                    rundir + "l18n\\" + lang + ".xml");
            }

            //l18n.Load(rundir + "l18n\\" + lang + ".xml");
            
            // Decrypt Area
            btnDecryptText.Text = l18n.IniReadValue("MainWindow", "btnDecryptText");
            txtInputLabel.Text = l18n.IniReadValue("MainWindow", "txtInputLabel");
            txtOutputLabel.Text = l18n.IniReadValue("MainWindow", "txtOutputLabel");
            btnSelectVFDecryptInputFile.Content = l18n.IniReadValue("MainWindow", "btnSelectVFDecryptInputFile");
            textDecryptLabel.Text = l18n.IniReadValue("MainWindow", "textDecryptLabel");
            // 7-Zip Area
            btnExtractText.Text = l18n.IniReadValue("MainWindow", "btnExtractText");
            txt7ZInputLabel.Text = l18n.IniReadValue("MainWindow", "txt7ZInputLabel");
            txt7ZOutputLabel.Text = l18n.IniReadValue("MainWindow", "txt7ZOutputLabel");
            btnSelect7ZInputFile.Content = l18n.IniReadValue("MainWindow", "btnSelect7ZInputFile");
            // Extras Area
            btnAbout.ToolTip = l18n.IniReadValue("MainWindow", "btnAbout_ToolTip");
            btnREADME.ToolTip = l18n.IniReadValue("MainWindow", "btnREADME_ToolTip");
            btnChangelog.ToolTip = l18n.IniReadValue("MainWindow", "btnChangelog_ToolTip");
            btnHelpOut.ToolTip = l18n.IniReadValue("MainWindow", "btnHelpOut_ToolTip");
            btnChangeLanguage.ToolTip = l18n.IniReadValue("MainWindow", "btnChangeLanguage_ToolTip");
            // Main Area
            btn1a420.Content = l18n.IniReadValue("MainWindow", "btn1a420");
            tabFinal.Header = l18n.IniReadValue("MainWindow", "tabFinal");
            tabFinalATV.Header = l18n.IniReadValue("MainWindow", "tabFinal");
            tabBeta.Header = l18n.IniReadValue("MainWindow", "tabBeta");
            tabBetaATV.Header = l18n.IniReadValue("MainWindow", "tabBeta");
            // Notes
            nokey = l18n.IniReadValue("MainWindow", "nokey");
            unavailable = l18n.IniReadValue("MainWindow", "unavailable");
        }*/
    }
}