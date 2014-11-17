/* =============================================================================
 * File:   MainWindow.xaml.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2010-2014, Cole Johnson
 * 
 * This file is part of iDecryptIt
 * 
 * iDecryptIt is free software: you can redistribute it and/or modify it under
 *   the terms of the GNU General Public License as published by the Free
 *   Software Foundation, either version 3 of the License, or (at your option)
 *   any later version.
 * 
 * iDecryptIt is distributed in the hope that it will be useful, but WITHOUT
 *   ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 *   FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for
 *   more details.
 * 
 * You should have received a copy of the GNU General Public License along with
 *   iDecryptIt. If not, see <http://www.gnu.org/licenses/>.
 * =============================================================================
 */
using Hexware.Plist;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Shell;

namespace Hexware.Programs.iDecryptIt
{
    public partial class MainWindow : Window
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern bool FreeConsole();

        static string execDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string execHash = new Random().Next().ToString("X");
        static string helpDir = Path.Combine(execDir, "help");

        internal static bool debug;

        public KeySelectionViewModel DevicesViewModel;
        private string selectedDevice;
        public KeySelectionViewModel ModelsViewModel;
        private string selectedModel;
        public KeySelectionViewModel VersionsViewModel;
        private string selectedVersion;

        BackgroundWorker decryptWorker;
        Process decryptProc;
        long decryptFromLength;
        string decryptFrom;
        string decryptTo;
        double decryptProg;

        public MainWindow()
        {
            if (!debug)
                FreeConsole();

            DevicesViewModel = new KeySelectionViewModel();
            ModelsViewModel = new KeySelectionViewModel();
            VersionsViewModel = new KeySelectionViewModel();

            InitializeComponent();

            this.DataContext = this;
            KeySelectionLists.Init();
            cmbDeviceDropDown.ItemsSource = KeySelectionLists.Products;

            this.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
        }

        internal void Debug(string component, string message)
        {
            if (!debug)
                return;

            Console.WriteLine("{0} {1}", component.PadRight(12), message);
        }
        internal void Error(string message, Exception except)
        {
            Debug("[ERROR]", message);
            if (except == null) {
                Debug("[ERROR]", "Exception type: null");
                Debug("[ERROR]", "Exception message: null");
            } else {
                Debug("[ERROR]", "Exception type: " + except.GetType().Name);
                Debug("[ERROR]", "Exception message: " + except.Message);
            }

            MessageBoxResult res = MessageBox.Show(
                message + "\r\n\r\n" +
                    "Do you want iDecryptIt to save an error log for bug reporting?" +
                    "(no personally identifiable information will be included)",
                "iDecryptIt",
                MessageBoxButton.YesNo,
                MessageBoxImage.Error);

            if (res != MessageBoxResult.Yes)
                return;

            string fileName = null;
            try {
                fileName = SaveErrorLog(message, except);
            } catch (Exception ex) {
                string exName = ex.GetType().Name;
                Debug("[ERRLOG]", exName + " thrown while saving log.");
                MessageBox.Show(
                    "A(n) " + exName + " error occured while saving the error log.",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            MessageBox.Show(
                "The error log was saved to your desktop as \"" + fileName + "\".\r\n" +
                "Please file a bug report.",
                "iDecryptIt",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        internal void FatalError(string message, Exception except)
        {
            Debug("[ERROR]", message);
            if (except == null) {
                Debug("[ERROR]", "Exception type: null");
                Debug("[ERROR]", "Exception message: null");
            } else {
                Debug("[ERROR]", "Exception type: " + except.GetType().Name);
                Debug("[ERROR]", "Exception message: " + except.Message);
            }

            MessageBox.Show(
                "iDecryptIt has encoundered a fatal error and must close.\r\n" + message,
                "iDecryptIt",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            string fileName = null;
            try {
                fileName = SaveErrorLog(message, except);
            } catch (Exception ex) {
                string exName = ex.GetType().Name;
                Debug("[ERRLOG]", exName + " thrown while saving log.");
                MessageBox.Show(
                    "A(n) " + exName + " error occured while saving the error log.",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                Close();
                return;
            }

            MessageBox.Show(
                "An error log was saved to your desktop as \"" + fileName + "\".\r\n" +
                "Please file a bug report. (no personally identifiable information was included)",
                "iDecryptIt",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            Close();
        }
        internal string SaveErrorLog(string message, Exception except)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = "iDecryptIt_" + execHash + ".log";
            string fullPath = Path.Combine(desktopPath, fileName);
            Debug("[ERRLOG]", "Saving to \"" + fullPath + "\".");

            StreamWriter stream = new StreamWriter(fullPath, true, Encoding.UTF8);
            Debug("[ERRLOG]", "Saving log.");

            stream.WriteLine("iDecryptIt " + GlobalVars.Version + GlobalVars.Version64);
            stream.WriteLine("Compile time: " + GlobalVars.CompileTimestamp.ToString() + " UTC");
            stream.WriteLine("Log time: " + DateTime.UtcNow + " UTC");
            stream.WriteLine("Execution string: " + Environment.CommandLine);
            WriteSystemConfig(stream);
            stream.WriteLine("Error message: " + message);
            if (except == null) {
                stream.WriteLine("Exception type:    null");
                stream.WriteLine("Exception message: null");
            } else {
                stream.WriteLine("Exception type:    " + except.GetType().Name);
                stream.WriteLine("Exception message: " + except.Message);
            }
            stream.WriteLine("Stack trace: ");
            stream.WriteLine(Environment.StackTrace);
            stream.WriteLine();
            stream.WriteLine();

            Debug("[ERRLOG]", "Closing log.");
            stream.Close();
            stream.Dispose();

            return fileName;
        }
        internal void WriteSystemConfig(StreamWriter stream)
        {
            stream.WriteLine("System config:");
            stream.WriteLine("  Current dir:  " + Environment.CurrentDirectory);
            stream.WriteLine("  Is 64-bit OS: " + Environment.Is64BitOperatingSystem.ToString());
            stream.WriteLine("  OS version:   " + Environment.OSVersion.ToString());
            stream.WriteLine("  Processors:   " + Environment.ProcessorCount);
            stream.WriteLine("  .NET version: " + Environment.Version);
            stream.WriteLine("  Working set:  " + Environment.WorkingSet);

            Process me = Process.GetCurrentProcess();
            stream.WriteLine("Process info:");
            stream.WriteLine("  Processor time:   " + me.PrivilegedProcessorTime);
            stream.WriteLine("  Process affinity: " + me.ProcessorAffinity);
            stream.WriteLine("  Execution time:   " + me.StartTime.ToUniversalTime() + " UTC");
        }

        private void btnGetKeys_Click(object sender, RoutedEventArgs e)
        {
            Debug("[KEYSELECT]", "Validating input.");
            if (selectedModel == null || selectedVersion == null)
                return;

            Debug("[KEYSELECT]", "Opening keys for " + selectedModel + " " + selectedVersion + ".");

            // Oh, you want the prototype beta, huh?
            if (selectedModel == "iPhone1,1" && selectedVersion == "1A420") {
                try {
                    Process.Start("https://mega.co.nz/#!Ml8hyCQI!d2ihbCEvtkFcFSgldAPqIQ1_OpRIWAeJZl_HODWjC7s");
                } catch (Exception ex) {
                    Error("Unable to open prototype beta webpage", ex);
                }
                return;
            }

            Stream stream = GetStream(selectedModel + "_" + selectedVersion + ".plist");
            if (stream == Stream.Null) {
                Debug("[KEYSELECT]", "Key file doesn't exist. No keys available.");
                MessageBox.Show(
                    "Sorry, but that version doesn't have any published keys.",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }
            LoadFirmwareKeys(stream, false);
        }
        internal Stream GetStream(string resourceName)
        {
            Debug("[GETSTREAM]", "Attempting read of stored resource, \"" + resourceName + "\".");
            try {
                Assembly assy = Assembly.GetExecutingAssembly();
                string[] resources = assy.GetManifestResourceNames();
                int length = resources.Length;
                for (int i = 0; i < length; i++) {
                    if (resources[i].ToLower().IndexOf(resourceName.ToLower()) != -1) {
                        // resource found
                        return assy.GetManifestResourceStream(resources[i]);
                    }
                }
            } catch (Exception ex) {
                Error("Unable to retrieve keys.", ex);
            }
            return Stream.Null;
        }
        private void LoadFirmwareKeys(Stream document, bool goldMaster)
        {
            PlistDocument doc = null;

            // Open stream
            Debug("[LOADKEYS]", "Opening key stream.");
            try {
                doc = new PlistDocument(document);
            } catch (Exception ex) {
                Error("Error loading key file.", ex);
                return;
            }

            // Magic. Don't touch.
            PlistDict plist = doc.Document.Value;
            PlistDict temp;
            #region Device
            Debug("[LOADKEYS]", "Device.");
            txtDevice.Text = GlobalVars.DeviceNames[plist.Get<PlistString>("Device").Value];
            #endregion
            #region Version
            txtVersion.Text = plist.Get<PlistString>("Version").Value +
                " (Build " + plist.Get<PlistString>("Build").Value + ")";
            //if (goldMaster)
            //    txtVersion.Text = txtVersion.Text + " [GM]";
            #endregion
            #region Root FS
            fileRootFS.Text = plist.Get<PlistDict>("Root FS").Get<PlistString>("File Name").Value;
            keyRootFS.Text = plist.Get<PlistDict>("Root FS").Get<PlistString>((goldMaster) ? "GM Key" : "Key").Value;
            #endregion
            #region Restore Ramdisk
            if (plist.Exists("Restore Ramdisk")) {
                temp = plist.Get<PlistDict>("Restore Ramdisk");
                if (temp.Get<PlistBool>("Encryption").Value) {
                    grdRestore.Visibility = Visibility.Visible;
                    grdRestoreNoEncrypt.Visibility = Visibility.Collapsed;
                    keyRestoreIV.Text = temp.Get<PlistString>("IV").Value;
                    keyRestoreKey.Text = temp.Get<PlistString>("Key").Value;
                    fileRestore.Text = temp.Get<PlistString>("File Name").Value;
                } else {
                    grdRestore.Visibility = Visibility.Collapsed;
                    grdRestoreNoEncrypt.Visibility = Visibility.Visible;
                    fileRestore.Text = temp.Get<PlistString>("File Name").Value;
                }
            } else {
                grdRestore.Visibility = Visibility.Collapsed;
                grdRestoreNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion

            doc.Dispose();
        }

        private void btnSelectRootFSInputFile_Click(object sender, RoutedEventArgs e)
        {
            Debug("[SELECTFS]", "Loading file dialog.");
            OpenFileDialog decrypt = new OpenFileDialog();
            decrypt.Filter = "Apple Disk Images|*.dmg";
            decrypt.CheckFileExists = true;
            decrypt.ShowDialog();
            Debug("[SELECTFS]", "File dialog closed.");
            if (!String.IsNullOrWhiteSpace(decrypt.SafeFileName)) {
                textInputFileName.Text = decrypt.FileName;
            }
        }
        private void btnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            Debug("[DECRYPT]", "Validating input.");
            #region Input Validation
            if (String.IsNullOrWhiteSpace(textInputFileName.Text) ||
                String.IsNullOrWhiteSpace(textOutputFileName.Text) ||
                String.IsNullOrWhiteSpace(textDecryptKey.Text)) {
                return;
            }
            if (!File.Exists(textInputFileName.Text)) {
                MessageBox.Show(
                    "The input file does not exist.",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (File.Exists(textOutputFileName.Text)) {
                if (MessageBox.Show(
                    "The output file already exists. Shall I delete it?",
                    "iDecryptIt",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.No) {
                    return;
                }
                File.Delete(textOutputFileName.Text);
            }
            #endregion

            decryptFrom = textInputFileName.Text;
            decryptTo = textOutputFileName.Text;
            decryptFromLength = new FileInfo(decryptFrom).Length;

            Debug("[DECRYPT]", "Launching dmg.");
            ProcessStartInfo x = new ProcessStartInfo();
            x.RedirectStandardError = true;
            x.RedirectStandardOutput = true;
            x.UseShellExecute = false;
            x.FileName = Path.Combine(execDir, "dmg.exe");
            x.Arguments = String.Format("extract \"{0}\" \"{1}\" -k {2}", textInputFileName.Text, textOutputFileName.Text, textDecryptKey.Text);
            x.ErrorDialog = true;

            decryptProc = new Process();
            decryptProc.EnableRaisingEvents = true;
            decryptProc.OutputDataReceived += decryptProc_OutputDataReceived;
            decryptProc.StartInfo = x;
            decryptProc.ErrorDataReceived += decryptProc_ErrorDataReceived;
            decryptProc.Start();
            decryptProc.BeginOutputReadLine(); // Execution halts if the buffer is full
            decryptProc.BeginErrorReadLine();

            // Screen mods
            gridDecrypt.IsEnabled = false;
            progDecrypt.Visibility = Visibility.Visible;
            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;

            // Wait for file to exist before starting worker (processes are asynchronous)
            while (!File.Exists(decryptTo)) { }
            Debug("[DECRYPT]", "Starting progress checker.");
            decryptWorker = new BackgroundWorker();
            decryptWorker.WorkerSupportsCancellation = true;
            decryptWorker.WorkerReportsProgress = true;
            decryptWorker.DoWork += decryptWorker_DoWork;
            decryptWorker.ProgressChanged += decryptWorker_ProgressReported;
            decryptWorker.RunWorkerAsync();
        }
        private void textInputFileName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try {
                string folder = Path.GetDirectoryName(textInputFileName.Text);
                string file = Path.GetFileName(textInputFileName.Text);
                if (file.Substring(file.Length - 4, 4) != ".dmg") {
                    return;
                }
                file = file.Substring(0, file.Length - 4) + "_decrypted.dmg";
                textOutputFileName.Text = Path.Combine(folder, file);
            } catch (Exception) { }
        }
        private void decryptProc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (debug)
                Console.WriteLine(e.Data);
        }
        private void decryptProc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (debug)
                Console.WriteLine(e.Data);
        }
        private void decryptWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!decryptWorker.CancellationPending) {
                if (decryptProc.HasExited) {
                    decryptWorker.ReportProgress(100);
                } else {
                    decryptProg = ((new FileInfo(decryptTo).Length) * 100.0) / decryptFromLength;
                    decryptWorker.ReportProgress(0);
                    Thread.Sleep(100); // don't hog the CPU
                }
            }
        }
        private void decryptWorker_ProgressReported(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 100 && !decryptWorker.CancellationPending) {
                decryptWorker.CancelAsync();
                gridDecrypt.IsEnabled = true;
                // reset progress values
                decryptProg = 0.0;
                TaskbarItemInfo.ProgressValue = 0.0;
                // hide progress indicators
                progDecrypt.Visibility = Visibility.Hidden;
                TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
                return;
            }

            double progress = decryptProg;
            if (progress > 100.0) {
                progDecrypt.Value = 100.0;
                TaskbarItemInfo.ProgressValue = 100.0;
            } else {
                progDecrypt.Value = progress;
                TaskbarItemInfo.ProgressValue = progress;
            }
        }

        private void btnChangelog_Click(object sender, RoutedEventArgs e)
        {
            Debug("[CHANGE]", "Loading Changelog.");
            Process.Start("file://" + helpDir + "changelog.html");
        }
        private void btnReadme_Click(object sender, RoutedEventArgs e)
        {
            Debug("[README]", "Loading Readme.");
            Process.Start("file://" + helpDir + "readme.html");
        }

        private void btnExtract_Click(object sender, RoutedEventArgs e)
        {
            Debug("[EXTRACT]", "Validating input.");
            #region Input validation
            if (String.IsNullOrWhiteSpace(text7ZInputFileName.Text) ||
                String.IsNullOrWhiteSpace(text7ZOuputFolder.Text)) {
                return;
            }
            if (!File.Exists(text7ZInputFileName.Text)) {
                MessageBox.Show(
                    "The input file does not exist.",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (Directory.Exists(text7ZInputFileName.Text)) {
                MessageBox.Show(
                    "The specified location is actually a directory.",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (File.Exists(text7ZOuputFolder.Text)) {
                MessageBox.Show(
                    "The output folder is actually a file.",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            #endregion

            Debug("[EXTRACT]", "Launching 7zip.");
            Process.Start(
                Path.Combine(execDir, "7z.exe"),
                " x \"" + text7ZInputFileName.Text + "\" \"-o" + text7ZOuputFolder.Text + "\"");
        }
        private void btnSelect7ZInputFile_Click(object sender, RoutedEventArgs e)
        {
            Debug("[SELECT7Z]", "Loading file dialog.");
            OpenFileDialog extract = new OpenFileDialog();
            extract.Filter = "Apple Disk Images|*.dmg";
            extract.CheckFileExists = true;
            extract.ShowDialog();
            Debug("[SELECT7Z]", "File dialog closed.");
            if (!String.IsNullOrWhiteSpace(extract.SafeFileName)) {
                text7ZInputFileName.Text = extract.FileName;
                text7ZOuputFolder.Text = Path.GetDirectoryName(extract.FileName);
            }
        }
        private void btnSelectWhatAmIFile_Click(object sender, RoutedEventArgs e)
        {
            Debug("[SELECTWHAT]", "Opening file dialog.");
            OpenFileDialog what = new OpenFileDialog();
            what.Filter = "Apple Firmware Files|*.ipsw";
            what.CheckFileExists = true;
            what.ShowDialog();
            Debug("[SELECTWHAT]", "Closing file dialog.");
            if (!String.IsNullOrWhiteSpace(what.SafeFileName)) {
                textWhatAmIFileName.Text = what.SafeFileName;
            }
        }

        private void btnWhatAmI_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Open the archive and parse the Restore.plist file
            //   If it doesn't exist, use the filename
            string[] strArr;

            if (String.IsNullOrWhiteSpace(textWhatAmIFileName.Text))
                return;

            strArr = textWhatAmIFileName.Text.Split('_');
            if (strArr.Length != 4 || strArr[3] != "Restore.ipsw") {
                MessageBox.Show(
                    "The supplied IPSW File that was given is not in the following format:\r\n" +
                        "\t{DEVICE}_{VERSION}_{BUILD}_Restore.ipsw",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            string device;
            if (!GlobalVars.DeviceNames.TryGetValue(strArr[0], out device)) {
                MessageBox.Show(
                    "The supplied device: '" + strArr[0] + "' does not follow the format:\r\n" +
                        "\t{iPad/iPhone/iPad/AppleTV}{#},{#} " +
                        "or is not supported at the moment.",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            string version = strArr[1];
            if (strArr[0][0] == 'A') {
                string temp = BuildToAppleTVVersion(strArr[0], strArr[2]);
                if (temp != null)
                    version = temp;
            }

            MessageBox.Show(
                "Device: " + device + "\r\n" +
                    "Version: " + version + "\r\n" +
                    "Build: " + strArr[2],
                "iDecryptIt",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        private string BuildToAppleTVVersion(string device, string build)
        {
            if (device != "AppleTV2,1" && device != "AppleTV3,1" && device != "AppleTV3,2")
                return null;

            switch (build) {
                case "8M89":
                    return "4.0/4.1";
                case "8C150":
                    return "4.1/4.2";
                case "8C154":
                    return "4.1.1/4.2.1";
                case "8F5148c":
                case "8F5153d":
                case "8F5166b":
                case "8F191m":
                    return "4.2/4.3";
                case "8F202":
                    return "4.2.1/4.3";
                case "8F305":
                    return "4.2.2/4.3";
                case "8F455":
                    return "4.3";
                case "9A5220p":
                case "9A5248d":
                case "9A5259f":
                case "9A5288d":
                case "9A5302b":
                case "9A5313e":
                case "9A334v":
                    return "4.4/5.0";
                case "9A335a":
                    return "4.4.1/5.0";
                case "9A336a":
                    return "4.4.2/5.0";
                case "9A405l":
                    return "4.4.3/5.0.1";
                case "9A406a":
                    return "4.4.4/5.0.1";
                case "9B5127c":
                case "9B5141a":
                    return "5.0/5.1";
                case "9B179b": // AppleTV3,1 introduced
                    return "5.0/5.1";
                case "9B206f":
                    return "5.0.1/5.1";
                case "9B830":
                    return "5.0.2/5.1";
                case "10A5316k":
                case "10A5338d":
                case "10A5355d":
                case "10A5376e":
                case "10A406e":
                    return "5.1/6.0";
                case "10A831":
                    return "5.1.1/6.0.1";
                case "10B5105c":
                case "10B5117b":
                case "10B5126b":
                    return "5.2/6.1";
                case "10B144b": // AppleTV3,2 introduced
                    return "5.2/6.1";
                case "10B329a":
                    return "5.2.1/6.1.3";
                case "10B809":
                    return "5.3/6.1.4";
                case "11A4372q":
                case "11A4400f":
                    return "5.4/6.0";
                case "11A4435d":
                case "11A4449a":
                    return "6.0/7.0";
                case "11A470e":
                    return "6.0/7.0.1";
                case "11A502":
                    return "6.0/7.0.2";
                case "11B511d":
                    return "6.0.1/7.0.3";
                case "11B554a":
                    return "6.0.2/7.0.4";
                case "11B651":
                    return "6.0.2/7.0.6";
                case "11D5099e":
                case "11D5115d":
                case "11D5127c":
                case "11D5134c":
                case "11D5145e":
                case "11D169b":
                    return "6.1/7.1";
                case "11D201c":
                    return "6.1.1/7.1.1";
                case "11D257c":
                    return "6.2/7.1.2";
                case "11D258": // AppleTV2,1 exclusive
                    return "6.2.1/7.1.2";
                case "12A4297e": // AppleTV2,1 dropped
                case "12A4318c":
                case "12A4331d":
                case "12A4345d":
                case "12A365b":
                    return "7.0/8.0";
                case "12B401":
                case "12B407":
                case "12B410a":
                    return "7.0.1/8.1";
            }
            return null;
        }

        private void cmbDeviceDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            ComboBoxEntry entry = (ComboBoxEntry)e.AddedItems[0];
            Debug("[KEYSELECT]", "Selected device changed: \"" + entry.ID + "\".");

            selectedDevice = entry.ID;

            selectedModel = null;
            cmbModelDropDown.IsEnabled = true;
            cmbModelDropDown.ItemsSource = KeySelectionLists.ProductsHelper[entry.ID];

            selectedVersion = null;
            cmbVersionDropDown.IsEnabled = false;
            cmbVersionDropDown.ItemsSource = null;
        }
        private void cmbModelDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            ComboBoxEntry entry = (ComboBoxEntry)e.AddedItems[0];
            Debug("[KEYSELECT]", "Selected model changed: \"" + entry.ID + "\".");

            selectedModel = entry.ID;

            selectedVersion = null;
            cmbVersionDropDown.IsEnabled = true;
            cmbVersionDropDown.ItemsSource = KeySelectionLists.ModelsHelper[entry.ID];
        }
        private void cmbVersionDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            ComboBoxEntry entry = (ComboBoxEntry)e.AddedItems[0];

            Debug("[KEYSELECT]", "Selected version changed: \"" + entry.ID + "\".");

            selectedVersion = entry.ID;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (GlobalVars.ExecutionArgs.ContainsKey("dmg")) {
                string fileName = GlobalVars.ExecutionArgs["dmg"];
                Debug("[INIT]", "File argument supplied: \"" + fileName + "\".");
                textInputFileName.Text = fileName;
            }

            Debug("[UPDATE]", "Checking for updates.");
            try {
                WebClient updateChecker = new WebClient();
                updateChecker.DownloadStringCompleted += updateChecker_DownloadStringCompleted;
                updateChecker.DownloadStringAsync(new Uri(
                    @"http://theiphonewiki.com/w/index.php?title=User:5urd/Latest_stable_software_release/iDecryptIt&action=raw"));
            } catch (Exception) { }
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Debug("[DEINIT]", "Closing.");
            Thread.Sleep(500);
            Application.Current.Shutdown();
        }
        private void updateChecker_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Result != null) {
                Debug("[UPDATE]", "Installed version: " + GlobalVars.Version);
                Debug("[UPDATE]", "Latest version: " + e.Result);

#if !DEBUG
                if (e.Result != GlobalVars.Version)
                {
                    MessageBox.Show(
                        "Update Available.",
                        "iDecryptIt: Update Available",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
#endif
            }
        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            FatalError("An unknown error has occured.", e.Exception);
        }
    }
}