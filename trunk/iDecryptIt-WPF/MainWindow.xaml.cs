/* =============================================================================
 * File:   MainWindow.xaml.cs
 * Author: Hexware
 * =============================================================================
 * Copyright (c) 2011-2013, Hexware
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 *   along with this program. If not, see <http://www.gnu.org/licenses/>.
 * =============================================================================
 */
using Hexware.Plist;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Hexware.Programs.iDecryptIt
{
    public partial class MainWindow : Window
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern bool FreeConsole();

        static string rundir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\";
        string tempdir;
        static string helpdir = rundir + "help\\";

        internal static bool debug;

        BackgroundWorker decryptworker;
        Process decryptProc;
        FileInfo decryptFromFile;
        string decryptFrom;
        string decryptTo;
        double decryptProg;

        public MainWindow()
        {
            InitializeComponent();

            if (!debug)
                FreeConsole();
        }

        private void decryptworker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!decryptworker.CancellationPending)
            {
                if (decryptProc.HasExited)
                {
                    decryptworker.ReportProgress(100);
                }
                else
                {
                    decryptProg = ((new FileInfo(decryptTo).Length) * 100.0) / decryptFromFile.Length;
                    decryptworker.ReportProgress(0);
                    Thread.Sleep(100); // don't hog the CPU
                }
            }
        }
        private void decryptworker_ProgressReported(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 100)
            {
                decryptworker.CancelAsync();
                progDecrypt.Value = 100.0;
                gridDecrypt.IsEnabled = true;
                progDecrypt.Visibility = Visibility.Hidden;
                return;
            }
            progDecrypt.Value = (decryptProg > 100.0) ? 100.0 : decryptProg;
        }

        private void Cleanup()
        {
            Debug("DEINIT", "Clearing temp directory.");
            try
            {
                if (Directory.Exists(tempdir))
                {
                    Directory.Delete(tempdir, true);
                }

                string temp = Path.Combine(Path.GetTempPath(), "Hexware", "iDecryptIt-Setup");
                if (Directory.Exists(temp))
                {
                    Directory.Delete(temp, true);
                }
            }
            catch (Exception)
            {
                // don't error here. it's just a temp directory
            }
        }
        internal void Debug(string component, string message)
        {
            if (debug)
            {
                if (component.Length > 10)
                {
                    Console.WriteLine("[{0}] {1}", component, message);
                }
                else
                {
                    Console.WriteLine("[{0}]{1} {2}", component, new String(' ', 10 - component.Length), message);
                }
            }
        }
        internal void Error(string message, Exception ex)
        {
            MessageBox.Show(
                message + "\r\n\r\n" +
                "Please file a bug report with the following data and an" +
                "explanation of what was happening:\r\n\r\n" +
                (ex == null ? "" : "Exception: " + ex.Message + "\r\n") +
                "Version: " + GlobalVars.Version + "\r\n",
                "iDecryptIt",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        internal void FatalError(string message, Exception ex)
        {
            MessageBox.Show(
                "iDecryptIt has encountered a fatal error and must close.\r\n\r\n" +
                message + "\r\n\r\n" +
                "Please file a bug report with the following data and an" +
                "explanation of what was happening:\r\n\r\n" +
                (ex == null ? "" : "Exception: " + ex.Message + "\r\n") +
                "Version: " + GlobalVars.Version + "\r\n",
                "iDecryptIt",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            Close();
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
            }
            catch (Exception ex)
            {
                Error("Unable to retrieve keys.", ex);
            }
            return Stream.Null;
        }
        private void LoadKey(Stream document, bool goldenMaster)
        {
            // This code is hideous. I'm not proud of it, but it works.
            PlistDocument doc = null;

            // Open stream
            try
            {
                doc = new PlistDocument(document);
            }
            catch (Exception ex)
            {
                Error("Error loading key file.", ex);
                try
                {
                    // Wrap it in case the constructor has disposed of it already.
                    doc.Dispose();
                }
                catch (Exception)
                {
                }
                doc = null;
                this.Close();
                return;
            }

            // Magic. Don't touch.
            PlistDict plist = doc.Document.Value;
            string temp;
            #region Device
            txtDevice.Text = plist.Get<PlistString>("Device").Value;
            #endregion
            #region Version
            StringBuilder sb = new StringBuilder(64);
            temp = plist.Get<PlistString>("Build").Value;
            sb.Append(plist.Get<PlistString>("Version").Value);
            sb.Append(" (Build " + temp + ")");
            if (goldenMaster)
            {
                switch (temp)
                {
                    case "5A345":
                        // 2.0
                        if (plist.Get<PlistString>("Device").Value != "iPhone 3G")
                        {
                            sb.Append(" [GM]");
                        }
                        break;
                    case "7A341":
                        // 3.0
                        if (plist.Get<PlistString>("Device").Value != "iPhone 3GS")
                        {
                            sb.Append(" [GM]");
                        }
                        break;
                    case "8A293":
                        // 4.0
                        if (plist.Get<PlistString>("Device").Value != "iPhone 4 (GSM)")
                        {
                            sb.Append(" [GM]");
                        }
                        break;
                    case "9A334":
                        // 5.0
                        if (plist.Get<PlistString>("Device").Value != "iPhone 4S")
                        {
                            sb.Append(" [GM]");
                        }
                        break;
                    // 6.0 not needed as iPhone 5 was 10A405 and iPod touch 5G was 10A406
                }
            }
            txtVersion.Text = sb.ToString();
            sb.Clear();
            #endregion
            #region Root FS Key
            temp = plist.Get<PlistDict>("Root FS").Get<PlistString>("File Name").Value;
            fileRootFS.Text = (temp != "XXX-XXXX-XXX" && temp != "") ? temp + ".dmg" : "XXX-XXXX-XXX.dmg";
            temp = plist.Get<PlistDict>("Root FS").Get<PlistString>((goldenMaster) ? "GM Key" : "Key").Value;
            if (temp != "TODO")
            {
                keyRootFS.Text = temp;
            }
            #endregion
            #region Ramdisks
            // Show everything
            lblUpdateIV.Visibility = Visibility.Visible;
            lblUpdateKey.Visibility = Visibility.Visible;
            lblUpdateNoEncrypt.Visibility = Visibility.Visible;
            keyUpdateIV.Visibility = Visibility.Visible;
            keyUpdateKey.Visibility = Visibility.Visible;
            keyUpdateNoEncrypt.Visibility = Visibility.Visible;
            fileUpdate.Visibility = Visibility.Visible;
            fileUpdateNoEncrypt.Visibility = Visibility.Visible;
            lblRestoreIV.Visibility = Visibility.Visible;
            lblRestoreKey.Visibility = Visibility.Visible;
            lblRestoreNoEncrypt.Visibility = Visibility.Visible;
            keyRestoreIV.Visibility = Visibility.Visible;
            keyRestoreKey.Visibility = Visibility.Visible;
            keyRestoreNoEncrypt.Visibility = Visibility.Visible;
            fileRestore.Visibility = Visibility.Visible;
            fileRestoreNoEncrypt.Visibility = Visibility.Visible;

            if (plist.Exists("No Update Ramdisk") && plist.Get<PlistBool>("No Update Ramdisk").Value)
            {
                // Hide Update Ramdisk
                lblUpdateIV.Visibility = Visibility.Collapsed;
                lblUpdateKey.Visibility = Visibility.Collapsed;
                lblUpdateNoEncrypt.Visibility = Visibility.Collapsed;
                keyUpdateIV.Visibility = Visibility.Collapsed;
                keyUpdateKey.Visibility = Visibility.Collapsed;
                keyUpdateNoEncrypt.Visibility = Visibility.Collapsed;
                fileUpdate.Visibility = Visibility.Collapsed;
                fileUpdateNoEncrypt.Visibility = Visibility.Collapsed;
            }
            else
            {
                fileUpdate.Text = plist.Get<PlistDict>("Update Ramdisk").Get<PlistString>("File Name").Value + ".dmg";
                fileUpdateNoEncrypt.Text = fileUpdate.Text;
            }

            fileRestore.Visibility = Visibility.Visible;
            fileRestoreNoEncrypt.Visibility = Visibility.Visible;
            fileRestore.Text = plist.Get<PlistDict>("Restore Ramdisk").Get<PlistString>("File Name").Value + ".dmg";
            fileRestoreNoEncrypt.Text = fileRestore.Text;
            // (ramdisk not encrypted) || (ramdisk encrypted and no IV/key)
            if ((plist.Exists("Ramdisk Not Encrypted") && plist.Get<PlistBool>("Ramdisk Not Encrypted").Value) ||
                (!plist.Exists("Ramdisk Not Encrypted") && !plist.Get<PlistDict>("Restore Ramdisk").Exists("IV")))
            {
                // Hide encrypted Ramdisks
                lblUpdateIV.Visibility = Visibility.Collapsed;
                lblUpdateKey.Visibility = Visibility.Collapsed;
                keyUpdateIV.Visibility = Visibility.Collapsed;
                keyUpdateKey.Visibility = Visibility.Collapsed;
                fileUpdate.Visibility = Visibility.Collapsed;
                lblRestoreIV.Visibility = Visibility.Collapsed;
                lblRestoreKey.Visibility = Visibility.Collapsed;
                keyRestoreIV.Visibility = Visibility.Collapsed;
                keyRestoreKey.Visibility = Visibility.Collapsed;
                fileRestore.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Hide unencrypted Ramdisks
                lblUpdateNoEncrypt.Visibility = Visibility.Collapsed;
                keyUpdateNoEncrypt.Visibility = Visibility.Collapsed;
                fileUpdateNoEncrypt.Visibility = Visibility.Collapsed;
                lblRestoreNoEncrypt.Visibility = Visibility.Collapsed;
                keyRestoreNoEncrypt.Visibility = Visibility.Collapsed;
                fileRestoreNoEncrypt.Visibility = Visibility.Collapsed;
                // Keys
                if (!plist.Exists("No Update Ramdisk") || !plist.Get<PlistBool>("No Update Ramdisk").Value)
                {
                    keyUpdateIV.Visibility = Visibility.Visible;
                    keyUpdateKey.Visibility = Visibility.Visible;
                    keyUpdateIV.Text = plist.Get<PlistDict>("Update Ramdisk").Get<PlistString>("IV").Value;
                    keyUpdateKey.Text = plist.Get<PlistDict>("Update Ramdisk").Get<PlistString>("Key").Value;
                }
                keyRestoreIV.Text = plist.Get<PlistDict>("Restore Ramdisk").Get<PlistString>("IV").Value;
                keyRestoreKey.Text = plist.Get<PlistDict>("Restore Ramdisk").Get<PlistString>("Key").Value;
            }
            #endregion
            #region AppleLogo
            if (plist.Exists("AppleLogo"))
            {
                plist = plist.Get<PlistDict>("AppleLogo");
                if (plist.Exists("Encryption") &&
                    plist.Get<PlistBool>("Encryption").Value)
                {
                    // Hide unencrypted AppleLogo
                    lblAppleLogoNoEncrypt.Visibility = Visibility.Collapsed;
                    keyAppleLogoNoEncrypt.Visibility = Visibility.Collapsed;
                    fileAppleLogoNoEncrypt.Visibility = Visibility.Collapsed;
                    // Show encrypted AppleLogo
                    lblAppleLogoIV.Visibility = Visibility.Visible;
                    lblAppleLogoKey.Visibility = Visibility.Visible;
                    keyAppleLogoIV.Visibility = Visibility.Visible;
                    keyAppleLogoKey.Visibility = Visibility.Visible;
                    fileAppleLogo.Visibility = Visibility.Visible;
                    // Keys
                    keyAppleLogoIV.Text = plist.Get<PlistString>("IV").Value;
                    keyAppleLogoKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide encrypted AppleLogo
                    lblAppleLogoIV.Visibility = Visibility.Collapsed;
                    lblAppleLogoKey.Visibility = Visibility.Collapsed;
                    keyAppleLogoIV.Visibility = Visibility.Collapsed;
                    keyAppleLogoKey.Visibility = Visibility.Collapsed;
                    fileAppleLogo.Visibility = Visibility.Collapsed;
                    // Show unencrypted BatteryCharging0
                    lblAppleLogoNoEncrypt.Visibility = Visibility.Visible;
                    keyAppleLogoNoEncrypt.Visibility = Visibility.Visible;
                    fileAppleLogoNoEncrypt.Visibility = Visibility.Visible;
                }
                plist = (PlistDict)plist.Parent;
            }
            else
            {
                lblAppleLogoIV.Visibility = Visibility.Collapsed;
                lblAppleLogoKey.Visibility = Visibility.Collapsed;
                keyAppleLogoIV.Visibility = Visibility.Collapsed;
                keyAppleLogoKey.Visibility = Visibility.Collapsed;
                fileAppleLogo.Visibility = Visibility.Collapsed;
                lblAppleLogoNoEncrypt.Visibility = Visibility.Collapsed;
                keyAppleLogoNoEncrypt.Visibility = Visibility.Collapsed;
                fileAppleLogoNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region BatteryCharging0
            if (plist.Exists("BatteryCharging0"))
            {
                plist = plist.Get<PlistDict>("BatteryCharging0");
                if (plist.Exists("Encryption") &&
                    plist.Get<PlistBool>("Encryption").Value)
                {
                    // Hide unencrypted BatteryCharging0
                    lblBatteryCharging0NoEncrypt.Visibility = Visibility.Collapsed;
                    keyBatteryCharging0NoEncrypt.Visibility = Visibility.Collapsed;
                    fileBatteryCharging0NoEncrypt.Visibility = Visibility.Collapsed;
                    // Show encrypted BatteryCharging0
                    lblBatteryCharging0IV.Visibility = Visibility.Visible;
                    lblBatteryCharging0Key.Visibility = Visibility.Visible;
                    keyBatteryCharging0IV.Visibility = Visibility.Visible;
                    keyBatteryCharging0Key.Visibility = Visibility.Visible;
                    fileBatteryCharging0.Visibility = Visibility.Visible;
                    // Keys
                    keyBatteryCharging0IV.Text = plist.Get<PlistString>("IV").Value;
                    keyBatteryCharging0Key.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide Encrypted BatteryCharging0
                    lblBatteryCharging0IV.Visibility = Visibility.Collapsed;
                    lblBatteryCharging0Key.Visibility = Visibility.Collapsed;
                    keyBatteryCharging0IV.Visibility = Visibility.Collapsed;
                    keyBatteryCharging0Key.Visibility = Visibility.Collapsed;
                    fileBatteryCharging0.Visibility = Visibility.Collapsed;
                    // Show unencrypted BatteryCharging0
                    lblBatteryCharging0NoEncrypt.Visibility = Visibility.Visible;
                    keyBatteryCharging0NoEncrypt.Visibility = Visibility.Visible;
                    fileBatteryCharging0NoEncrypt.Visibility = Visibility.Visible;
                }
                plist = (PlistDict)plist.Parent;
            }
            else
            {
                lblBatteryCharging0IV.Visibility = Visibility.Collapsed;
                lblBatteryCharging0Key.Visibility = Visibility.Collapsed;
                keyBatteryCharging0IV.Visibility = Visibility.Collapsed;
                keyBatteryCharging0Key.Visibility = Visibility.Collapsed;
                fileBatteryCharging0.Visibility = Visibility.Collapsed;
                lblBatteryCharging0NoEncrypt.Visibility = Visibility.Collapsed;
                keyBatteryCharging0NoEncrypt.Visibility = Visibility.Collapsed;
                fileBatteryCharging0NoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region BatteryCharging1
            if (plist.Exists("BatteryCharging1"))
            {
                plist = plist.Get<PlistDict>("BatteryCharging1");
                if (plist.Exists("Encryption") &&
                    plist.Get<PlistBool>("Encryption").Value)
                {
                    // Hide unencrypted BatteryCharging1
                    lblBatteryCharging1NoEncrypt.Visibility = Visibility.Collapsed;
                    keyBatteryCharging1NoEncrypt.Visibility = Visibility.Collapsed;
                    fileBatteryCharging1NoEncrypt.Visibility = Visibility.Collapsed;
                    // Show encrypted BatteryCharging1
                    lblBatteryCharging1IV.Visibility = Visibility.Visible;
                    lblBatteryCharging1Key.Visibility = Visibility.Visible;
                    keyBatteryCharging1IV.Visibility = Visibility.Visible;
                    keyBatteryCharging1Key.Visibility = Visibility.Visible;
                    fileBatteryCharging1.Visibility = Visibility.Visible;
                    // Keys
                    keyBatteryCharging1IV.Text = plist.Get<PlistString>("IV").Value;
                    keyBatteryCharging1Key.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide encrypted BatteryCharging1
                    lblBatteryCharging1IV.Visibility = Visibility.Collapsed;
                    lblBatteryCharging1Key.Visibility = Visibility.Collapsed;
                    keyBatteryCharging1IV.Visibility = Visibility.Collapsed;
                    keyBatteryCharging1Key.Visibility = Visibility.Collapsed;
                    fileBatteryCharging1.Visibility = Visibility.Collapsed;
                    // Show unencrypted BatteryCharging1
                    lblBatteryCharging1NoEncrypt.Visibility = Visibility.Visible;
                    keyBatteryCharging1NoEncrypt.Visibility = Visibility.Visible;
                    fileBatteryCharging1NoEncrypt.Visibility = Visibility.Visible;
                }
                plist = (PlistDict)plist.Parent;
            }
            else
            {
                lblBatteryCharging1IV.Visibility = Visibility.Collapsed;
                lblBatteryCharging1Key.Visibility = Visibility.Collapsed;
                keyBatteryCharging1IV.Visibility = Visibility.Collapsed;
                keyBatteryCharging1Key.Visibility = Visibility.Collapsed;
                fileBatteryCharging1.Visibility = Visibility.Collapsed;
                lblBatteryCharging1NoEncrypt.Visibility = Visibility.Collapsed;
                keyBatteryCharging1NoEncrypt.Visibility = Visibility.Collapsed;
                fileBatteryCharging1NoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region BatteryFull
            if (plist.Exists("BatteryFull"))
            {
                plist = plist.Get<PlistDict>("BatteryFull");
                if (plist.Exists("Encryption") &&
                    plist.Get<PlistBool>("Encryption").Value)
                {
                    // Hide unencrypted BatteryFull
                    lblBatteryFullNoEncrypt.Visibility = Visibility.Collapsed;
                    keyBatteryFullNoEncrypt.Visibility = Visibility.Collapsed;
                    fileBatteryFullNoEncrypt.Visibility = Visibility.Collapsed;
                    // Show encrypted BatteryFull
                    lblBatteryFullIV.Visibility = Visibility.Visible;
                    lblBatteryFullKey.Visibility = Visibility.Visible;
                    keyBatteryFullIV.Visibility = Visibility.Visible;
                    keyBatteryFullKey.Visibility = Visibility.Visible;
                    fileBatteryFull.Visibility = Visibility.Visible;
                    // Keys
                    keyBatteryFullIV.Text = plist.Get<PlistString>("IV").Value;
                    keyBatteryFullKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide encrypted BatteryFull
                    lblBatteryFullIV.Visibility = Visibility.Collapsed;
                    lblBatteryFullKey.Visibility = Visibility.Collapsed;
                    keyBatteryFullIV.Visibility = Visibility.Collapsed;
                    keyBatteryFullKey.Visibility = Visibility.Collapsed;
                    fileBatteryFull.Visibility = Visibility.Collapsed;
                    // Show unencrypted BatteryFull
                    lblBatteryFullNoEncrypt.Visibility = Visibility.Visible;
                    keyBatteryFullNoEncrypt.Visibility = Visibility.Visible;
                    fileBatteryFullNoEncrypt.Visibility = Visibility.Visible;
                }
                plist = (PlistDict)plist.Parent;
            }
            else
            {
                lblBatteryFullIV.Visibility = Visibility.Collapsed;
                lblBatteryFullKey.Visibility = Visibility.Collapsed;
                keyBatteryFullIV.Visibility = Visibility.Collapsed;
                keyBatteryFullKey.Visibility = Visibility.Collapsed;
                fileBatteryFull.Visibility = Visibility.Collapsed;
                lblBatteryFullNoEncrypt.Visibility = Visibility.Collapsed;
                keyBatteryFullNoEncrypt.Visibility = Visibility.Collapsed;
                fileBatteryFullNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region BatteryLow0
            if (plist.Exists("BatteryLow0"))
            {
                plist = plist.Get<PlistDict>("BatteryLow0");
                if (plist.Exists("Encryption") &&
                    plist.Get<PlistBool>("Encryption").Value)
                {
                    // Hide unencrypted BatteryLow0
                    lblBatteryLow0NoEncrypt.Visibility = Visibility.Collapsed;
                    keyBatteryLow0NoEncrypt.Visibility = Visibility.Collapsed;
                    fileBatteryLow0NoEncrypt.Visibility = Visibility.Collapsed;
                    // Show encrypted BatteryLow0
                    lblBatteryLow0IV.Visibility = Visibility.Visible;
                    lblBatteryLow0Key.Visibility = Visibility.Visible;
                    keyBatteryLow0IV.Visibility = Visibility.Visible;
                    keyBatteryLow0Key.Visibility = Visibility.Visible;
                    fileBatteryLow0.Visibility = Visibility.Visible;
                    // Keys
                    keyBatteryLow0IV.Text = plist.Get<PlistString>("IV").Value;
                    keyBatteryLow0Key.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide encrypted BatteryLow0
                    lblBatteryLow0IV.Visibility = Visibility.Collapsed;
                    lblBatteryLow0Key.Visibility = Visibility.Collapsed;
                    keyBatteryLow0IV.Visibility = Visibility.Collapsed;
                    keyBatteryLow0Key.Visibility = Visibility.Collapsed;
                    fileBatteryLow0.Visibility = Visibility.Collapsed;
                    // Show unencrypted BatteryLow0
                    lblBatteryLow0NoEncrypt.Visibility = Visibility.Visible;
                    keyBatteryLow0NoEncrypt.Visibility = Visibility.Visible;
                    fileBatteryLow0NoEncrypt.Visibility = Visibility.Visible;
                }
                plist = (PlistDict)plist.Parent;
            }
            else
            {
                lblBatteryLow0IV.Visibility = Visibility.Collapsed;
                lblBatteryLow0Key.Visibility = Visibility.Collapsed;
                keyBatteryLow0IV.Visibility = Visibility.Collapsed;
                keyBatteryLow0Key.Visibility = Visibility.Collapsed;
                fileBatteryLow0.Visibility = Visibility.Collapsed;
                lblBatteryLow0NoEncrypt.Visibility = Visibility.Collapsed;
                keyBatteryLow0NoEncrypt.Visibility = Visibility.Collapsed;
                fileBatteryLow0NoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region BatteryLow1
            if (plist.Exists("BatteryLow1"))
            {
                plist = plist.Get<PlistDict>("BatteryLow1");
                if (plist.Exists("Encryption") &&
                    plist.Get<PlistBool>("Encryption").Value)
                {
                    // Hide unencrypted BatteryLow1
                    lblBatteryLow1NoEncrypt.Visibility = Visibility.Collapsed;
                    keyBatteryLow1NoEncrypt.Visibility = Visibility.Collapsed;
                    fileBatteryLow1NoEncrypt.Visibility = Visibility.Collapsed;
                    // Show encrypted BatteryLow1
                    lblBatteryLow1IV.Visibility = Visibility.Visible;
                    lblBatteryLow1Key.Visibility = Visibility.Visible;
                    keyBatteryLow1IV.Visibility = Visibility.Visible;
                    keyBatteryLow1Key.Visibility = Visibility.Visible;
                    fileBatteryLow1.Visibility = Visibility.Visible;
                    // Keys
                    keyBatteryLow1IV.Text = plist.Get<PlistString>("IV").Value;
                    keyBatteryLow1Key.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide encrypted BatteryLow1
                    lblBatteryLow1IV.Visibility = Visibility.Collapsed;
                    lblBatteryLow1Key.Visibility = Visibility.Collapsed;
                    keyBatteryLow1IV.Visibility = Visibility.Collapsed;
                    keyBatteryLow1Key.Visibility = Visibility.Collapsed;
                    fileBatteryLow1.Visibility = Visibility.Collapsed;
                    // Show unencrypted BatteryLow1
                    lblBatteryLow1NoEncrypt.Visibility = Visibility.Visible;
                    keyBatteryLow1NoEncrypt.Visibility = Visibility.Visible;
                    fileBatteryLow1NoEncrypt.Visibility = Visibility.Visible;
                }
                plist = (PlistDict)plist.Parent;
            }
            else
            {
                lblBatteryLow1IV.Visibility = Visibility.Collapsed;
                lblBatteryLow1Key.Visibility = Visibility.Collapsed;
                keyBatteryLow1IV.Visibility = Visibility.Collapsed;
                keyBatteryLow1Key.Visibility = Visibility.Collapsed;
                fileBatteryLow1.Visibility = Visibility.Collapsed;
                lblBatteryLow1NoEncrypt.Visibility = Visibility.Collapsed;
                keyBatteryLow1NoEncrypt.Visibility = Visibility.Collapsed;
                fileBatteryLow1NoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region DeviceTree
            if (plist.Exists("DeviceTree"))
            {
                plist = plist.Get<PlistDict>("DeviceTree");
                if (plist.Exists("Encryption") &&
                    plist.Get<PlistBool>("Encryption").Value)
                {
                    // Hide unencrypted DeviceTree
                    lblDeviceTreeNoEncrypt.Visibility = Visibility.Collapsed;
                    keyDeviceTreeNoEncrypt.Visibility = Visibility.Collapsed;
                    fileDeviceTreeNoEncrypt.Visibility = Visibility.Collapsed;
                    // Show encrypted DeviceTree
                    lblDeviceTreeIV.Visibility = Visibility.Visible;
                    lblDeviceTreeKey.Visibility = Visibility.Visible;
                    keyDeviceTreeIV.Visibility = Visibility.Visible;
                    keyDeviceTreeKey.Visibility = Visibility.Visible;
                    fileDeviceTree.Visibility = Visibility.Visible;
                    // Keys
                    keyDeviceTreeIV.Text = plist.Get<PlistString>("IV").Value;
                    keyDeviceTreeKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide encrypted DeviceTree
                    lblDeviceTreeIV.Visibility = Visibility.Collapsed;
                    lblDeviceTreeKey.Visibility = Visibility.Collapsed;
                    keyDeviceTreeIV.Visibility = Visibility.Collapsed;
                    keyDeviceTreeKey.Visibility = Visibility.Collapsed;
                    fileDeviceTree.Visibility = Visibility.Collapsed;
                    // Show unencrypted DeviceTree
                    lblDeviceTreeNoEncrypt.Visibility = Visibility.Visible;
                    keyDeviceTreeNoEncrypt.Visibility = Visibility.Visible;
                    fileDeviceTreeNoEncrypt.Visibility = Visibility.Visible;
                }
                plist = (PlistDict)plist.Parent;
            }
            else
            {
                lblDeviceTreeIV.Visibility = Visibility.Collapsed;
                lblDeviceTreeKey.Visibility = Visibility.Collapsed;
                keyDeviceTreeIV.Visibility = Visibility.Collapsed;
                keyDeviceTreeKey.Visibility = Visibility.Collapsed;
                fileDeviceTree.Visibility = Visibility.Collapsed;
                lblDeviceTreeNoEncrypt.Visibility = Visibility.Collapsed;
                keyDeviceTreeNoEncrypt.Visibility = Visibility.Collapsed;
                fileDeviceTreeNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region GlyphCharging
            if (plist.Exists("GlyphCharging"))
            {
                plist = plist.Get<PlistDict>("GlyphCharging");
                if (plist.Exists("Encryption") &&
                    plist.Get<PlistBool>("Encryption").Value)
                {
                    // Hide unencrypted GlyphCharging
                    lblGlyphChargingNoEncrypt.Visibility = Visibility.Collapsed;
                    keyGlyphChargingNoEncrypt.Visibility = Visibility.Collapsed;
                    fileGlyphChargingNoEncrypt.Visibility = Visibility.Collapsed;
                    // Show encrypted GlyphCharging
                    lblGlyphChargingIV.Visibility = Visibility.Visible;
                    lblGlyphChargingKey.Visibility = Visibility.Visible;
                    keyGlyphChargingIV.Visibility = Visibility.Visible;
                    keyGlyphChargingKey.Visibility = Visibility.Visible;
                    fileGlyphCharging.Visibility = Visibility.Visible;
                    // Keys
                    keyGlyphChargingIV.Text = plist.Get<PlistString>("IV").Value;
                    keyGlyphChargingKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide encrypted GlyphCharging
                    lblGlyphChargingIV.Visibility = Visibility.Collapsed;
                    lblGlyphChargingKey.Visibility = Visibility.Collapsed;
                    keyGlyphChargingIV.Visibility = Visibility.Collapsed;
                    keyGlyphChargingKey.Visibility = Visibility.Collapsed;
                    fileGlyphCharging.Visibility = Visibility.Collapsed;
                    // Show unencrypted GlyphCharging
                    lblGlyphChargingNoEncrypt.Visibility = Visibility.Visible;
                    keyGlyphChargingNoEncrypt.Visibility = Visibility.Visible;
                    fileGlyphChargingNoEncrypt.Visibility = Visibility.Visible;
                }
                plist = (PlistDict)plist.Parent;
            }
            else
            {
                lblGlyphChargingIV.Visibility = Visibility.Collapsed;
                lblGlyphChargingKey.Visibility = Visibility.Collapsed;
                keyGlyphChargingIV.Visibility = Visibility.Collapsed;
                keyGlyphChargingKey.Visibility = Visibility.Collapsed;
                fileGlyphCharging.Visibility = Visibility.Collapsed;
                lblGlyphChargingNoEncrypt.Visibility = Visibility.Collapsed;
                keyGlyphChargingNoEncrypt.Visibility = Visibility.Collapsed;
                fileGlyphChargingNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region GlyphPlugin
            if (plist.Exists("GlyphPlugin"))
            {
                plist = plist.Get<PlistDict>("GlyphPlugin");
                if (plist.Exists("Encryption") &&
                    plist.Get<PlistBool>("Encryption").Value)
                {
                    // Hide unencrypted GlyphPlugin
                    lblGlyphPluginNoEncrypt.Visibility = Visibility.Collapsed;
                    keyGlyphPluginNoEncrypt.Visibility = Visibility.Collapsed;
                    fileGlyphPluginNoEncrypt.Visibility = Visibility.Collapsed;
                    // Show encrypted GlyphPlugin
                    lblGlyphPluginIV.Visibility = Visibility.Visible;
                    lblGlyphPluginKey.Visibility = Visibility.Visible;
                    keyGlyphPluginIV.Visibility = Visibility.Visible;
                    keyGlyphPluginKey.Visibility = Visibility.Visible;
                    fileGlyphPlugin.Visibility = Visibility.Visible;
                    // Keys
                    keyGlyphPluginIV.Text = plist.Get<PlistString>("IV").Value;
                    keyGlyphPluginKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide encrypted GlyphPlugin
                    lblGlyphPluginIV.Visibility = Visibility.Collapsed;
                    lblGlyphPluginKey.Visibility = Visibility.Collapsed;
                    keyGlyphPluginIV.Visibility = Visibility.Collapsed;
                    keyGlyphPluginKey.Visibility = Visibility.Collapsed;
                    fileGlyphPlugin.Visibility = Visibility.Collapsed;
                    // Show unencrypted GlyphPlugin
                    lblGlyphPluginNoEncrypt.Visibility = Visibility.Visible;
                    keyGlyphPluginNoEncrypt.Visibility = Visibility.Visible;
                    fileGlyphPluginNoEncrypt.Visibility = Visibility.Visible;
                }
                plist = (PlistDict)plist.Parent;
            }
            else
            {
                lblGlyphPluginIV.Visibility = Visibility.Collapsed;
                lblGlyphPluginKey.Visibility = Visibility.Collapsed;
                keyGlyphPluginIV.Visibility = Visibility.Collapsed;
                keyGlyphPluginKey.Visibility = Visibility.Collapsed;
                fileGlyphPlugin.Visibility = Visibility.Collapsed;
                lblGlyphPluginNoEncrypt.Visibility = Visibility.Collapsed;
                keyGlyphPluginNoEncrypt.Visibility = Visibility.Collapsed;
                fileGlyphPluginNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region iBEC
            if (plist.Exists("iBEC"))
            {
                plist = plist.Get<PlistDict>("iBEC");
                if (plist.Exists("Encryption") &&
                    plist.Get<PlistBool>("Encryption").Value)
                {
                    // Hide unencrypted iBEC
                    lbliBECNoEncrypt.Visibility = Visibility.Collapsed;
                    keyiBECNoEncrypt.Visibility = Visibility.Collapsed;
                    fileiBECNoEncrypt.Visibility = Visibility.Collapsed;
                    // Show encrypted iBEC
                    lbliBECIV.Visibility = Visibility.Visible;
                    lbliBECKey.Visibility = Visibility.Visible;
                    keyiBECIV.Visibility = Visibility.Visible;
                    keyiBECKey.Visibility = Visibility.Visible;
                    fileiBEC.Visibility = Visibility.Visible;
                    // Keys
                    keyiBECIV.Text = plist.Get<PlistString>("IV").Value;
                    keyiBECKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide encrypted iBEC
                    lbliBECIV.Visibility = Visibility.Collapsed;
                    lbliBECKey.Visibility = Visibility.Collapsed;
                    keyiBECIV.Visibility = Visibility.Collapsed;
                    keyiBECKey.Visibility = Visibility.Collapsed;
                    fileiBEC.Visibility = Visibility.Collapsed;
                    // Show unencrypted iBEC
                    lbliBECNoEncrypt.Visibility = Visibility.Visible;
                    keyiBECNoEncrypt.Visibility = Visibility.Visible;
                    fileiBECNoEncrypt.Visibility = Visibility.Visible;
                }
                plist = (PlistDict)plist.Parent;
            }
            else
            {
                lbliBECIV.Visibility = Visibility.Collapsed;
                lbliBECKey.Visibility = Visibility.Collapsed;
                keyiBECIV.Visibility = Visibility.Collapsed;
                keyiBECKey.Visibility = Visibility.Collapsed;
                fileiBEC.Visibility = Visibility.Collapsed;
                lbliBECNoEncrypt.Visibility = Visibility.Collapsed;
                keyiBECNoEncrypt.Visibility = Visibility.Collapsed;
                fileiBECNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region iBoot
            if (plist.Exists("iBoot"))
            {
                plist = plist.Get<PlistDict>("iBoot");
                if (plist.Exists("Encryption") &&
                    plist.Get<PlistBool>("Encryption").Value)
                {
                    // Hide unencrypted iBoot
                    lbliBootNoEncrypt.Visibility = Visibility.Collapsed;
                    keyiBootNoEncrypt.Visibility = Visibility.Collapsed;
                    fileiBootNoEncrypt.Visibility = Visibility.Collapsed;
                    // Show encrypted iBoot
                    lbliBootIV.Visibility = Visibility.Visible;
                    lbliBootKey.Visibility = Visibility.Visible;
                    keyiBootIV.Visibility = Visibility.Visible;
                    keyiBootKey.Visibility = Visibility.Visible;
                    fileiBoot.Visibility = Visibility.Visible;
                    // Keys
                    keyiBootIV.Text = plist.Get<PlistString>("IV").Value;
                    keyiBootKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide Encrypted iBoot
                    lbliBootIV.Visibility = Visibility.Collapsed;
                    lbliBootKey.Visibility = Visibility.Collapsed;
                    keyiBootIV.Visibility = Visibility.Collapsed;
                    keyiBootKey.Visibility = Visibility.Collapsed;
                    fileiBoot.Visibility = Visibility.Collapsed;
                    // Show unencrypted iBoot
                    lbliBootNoEncrypt.Visibility = Visibility.Visible;
                    keyiBootNoEncrypt.Visibility = Visibility.Visible;
                    fileiBootNoEncrypt.Visibility = Visibility.Visible;
                }
                plist = (PlistDict)plist.Parent;
            }
            else
            {
                lbliBootIV.Visibility = Visibility.Collapsed;
                lbliBootKey.Visibility = Visibility.Collapsed;
                keyiBootIV.Visibility = Visibility.Collapsed;
                keyiBootKey.Visibility = Visibility.Collapsed;
                fileiBoot.Visibility = Visibility.Collapsed;
                lbliBootNoEncrypt.Visibility = Visibility.Collapsed;
                keyiBootNoEncrypt.Visibility = Visibility.Collapsed;
                fileiBootNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region iBSS
            if (plist.Exists("iBSS"))
            {
                plist = plist.Get<PlistDict>("iBSS");
                if (plist.Exists("Encryption") &&
                    plist.Get<PlistBool>("Encryption").Value)
                {
                    // Hide unencrypted iBSS
                    lbliBSSNoEncrypt.Visibility = Visibility.Collapsed;
                    keyiBSSNoEncrypt.Visibility = Visibility.Collapsed;
                    fileiBSSNoEncrypt.Visibility = Visibility.Collapsed;
                    // Show encrypted iBSS
                    lbliBSSIV.Visibility = Visibility.Visible;
                    lbliBSSKey.Visibility = Visibility.Visible;
                    keyiBSSIV.Visibility = Visibility.Visible;
                    keyiBSSKey.Visibility = Visibility.Visible;
                    fileiBSS.Visibility = Visibility.Visible;
                    // Keys
                    keyiBSSIV.Text = plist.Get<PlistString>("IV").Value;
                    keyiBSSKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide encrypted iBSS
                    lbliBSSIV.Visibility = Visibility.Collapsed;
                    lbliBSSKey.Visibility = Visibility.Collapsed;
                    keyiBSSIV.Visibility = Visibility.Collapsed;
                    keyiBSSKey.Visibility = Visibility.Collapsed;
                    fileiBSS.Visibility = Visibility.Collapsed;
                    // Show unencrypted iBSS
                    lbliBSSNoEncrypt.Visibility = Visibility.Visible;
                    keyiBSSNoEncrypt.Visibility = Visibility.Visible;
                    fileiBSSNoEncrypt.Visibility = Visibility.Visible;
                }
                plist = (PlistDict)plist.Parent;
            }
            else
            {
                lbliBSSIV.Visibility = Visibility.Collapsed;
                lbliBSSKey.Visibility = Visibility.Collapsed;
                keyiBSSIV.Visibility = Visibility.Collapsed;
                keyiBSSKey.Visibility = Visibility.Collapsed;
                fileiBSS.Visibility = Visibility.Collapsed;
                lbliBSSNoEncrypt.Visibility = Visibility.Collapsed;
                keyiBSSNoEncrypt.Visibility = Visibility.Collapsed;
                fileiBSSNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region Kernelcache
            if (plist.Exists("Kernelcache"))
            {
                plist = plist.Get<PlistDict>("Kernelcache");
                if (plist.Exists("Encryption") &&
                    plist.Get<PlistBool>("Encryption").Value)
                {
                    // Hide unencrypted Kernelcache
                    lblKernelcacheNoEncrypt.Visibility = Visibility.Collapsed;
                    keyKernelcacheNoEncrypt.Visibility = Visibility.Collapsed;
                    fileKernelcacheNoEncrypt.Visibility = Visibility.Collapsed;
                    // Show encrypted Kernelcache
                    lblKernelcacheIV.Visibility = Visibility.Visible;
                    lblKernelcacheKey.Visibility = Visibility.Visible;
                    keyKernelcacheIV.Visibility = Visibility.Visible;
                    keyKernelcacheKey.Visibility = Visibility.Visible;
                    fileKernelcache.Visibility = Visibility.Visible;
                    // Keys
                    keyKernelcacheIV.Text = plist.Get<PlistString>("IV").Value;
                    keyKernelcacheKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide encrypted Kernelcache
                    lblKernelcacheIV.Visibility = Visibility.Collapsed;
                    lblKernelcacheKey.Visibility = Visibility.Collapsed;
                    keyKernelcacheIV.Visibility = Visibility.Collapsed;
                    keyKernelcacheKey.Visibility = Visibility.Collapsed;
                    fileKernelcache.Visibility = Visibility.Collapsed;
                    // Show unencrypted Kernelcache
                    lblKernelcacheNoEncrypt.Visibility = Visibility.Visible;
                    keyKernelcacheNoEncrypt.Visibility = Visibility.Visible;
                    fileKernelcacheNoEncrypt.Visibility = Visibility.Visible;
                }
                plist = (PlistDict)plist.Parent;
            }
            else
            {
                lblKernelcacheIV.Visibility = Visibility.Collapsed;
                lblKernelcacheKey.Visibility = Visibility.Collapsed;
                keyKernelcacheIV.Visibility = Visibility.Collapsed;
                keyKernelcacheKey.Visibility = Visibility.Collapsed;
                fileKernelcache.Visibility = Visibility.Collapsed;
                lblKernelcacheNoEncrypt.Visibility = Visibility.Collapsed;
                keyKernelcacheNoEncrypt.Visibility = Visibility.Collapsed;
                fileKernelcacheNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region LLB
            if (plist.Exists("LLB"))
            {
                plist = plist.Get<PlistDict>("LLB");
                if (plist.Exists("Encryption") &&
                    plist.Get<PlistBool>("Encryption").Value)
                {
                    // Hide unencrypted LLB
                    lblLLBNoEncrypt.Visibility = Visibility.Collapsed;
                    keyLLBNoEncrypt.Visibility = Visibility.Collapsed;
                    fileLLBNoEncrypt.Visibility = Visibility.Collapsed;
                    // Show encrypted LLB
                    lblLLBIV.Visibility = Visibility.Visible;
                    lblLLBKey.Visibility = Visibility.Visible;
                    keyLLBIV.Visibility = Visibility.Visible;
                    keyLLBKey.Visibility = Visibility.Visible;
                    fileLLB.Visibility = Visibility.Visible;
                    // Keys
                    keyLLBIV.Text = plist.Get<PlistString>("IV").Value;
                    keyLLBKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide encrypted LLB
                    lblLLBIV.Visibility = Visibility.Collapsed;
                    lblLLBKey.Visibility = Visibility.Collapsed;
                    keyLLBIV.Visibility = Visibility.Collapsed;
                    keyLLBKey.Visibility = Visibility.Collapsed;
                    fileLLB.Visibility = Visibility.Collapsed;
                    // Show unencrypted LLB
                    lblLLBNoEncrypt.Visibility = Visibility.Visible;
                    keyLLBNoEncrypt.Visibility = Visibility.Visible;
                    fileLLBNoEncrypt.Visibility = Visibility.Visible;
                }
                plist = (PlistDict)plist.Parent;
            }
            else
            {
                lblLLBIV.Visibility = Visibility.Collapsed;
                lblLLBKey.Visibility = Visibility.Collapsed;
                keyLLBIV.Visibility = Visibility.Collapsed;
                keyLLBKey.Visibility = Visibility.Collapsed;
                fileLLB.Visibility = Visibility.Collapsed;
                lblLLBNoEncrypt.Visibility = Visibility.Collapsed;
                keyLLBNoEncrypt.Visibility = Visibility.Collapsed;
                fileLLBNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region NeedService
            if (plist.Exists("NeedService"))
            {
                plist = plist.Get<PlistDict>("NeedService");
                if (plist.Exists("Encryption") &&
                    plist.Get<PlistBool>("Encryption").Value)
                {
                    // Hide unencrypted NeedService
                    lblNeedServiceNoEncrypt.Visibility = Visibility.Collapsed;
                    keyNeedServiceNoEncrypt.Visibility = Visibility.Collapsed;
                    fileNeedServiceNoEncrypt.Visibility = Visibility.Collapsed;
                    // Show encrypted NeedService
                    lblNeedServiceIV.Visibility = Visibility.Visible;
                    lblNeedServiceKey.Visibility = Visibility.Visible;
                    keyNeedServiceIV.Visibility = Visibility.Visible;
                    keyNeedServiceKey.Visibility = Visibility.Visible;
                    fileNeedService.Visibility = Visibility.Visible;
                    // Keys
                    keyNeedServiceIV.Text = plist.Get<PlistString>("IV").Value;
                    keyNeedServiceKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide encrypted NeedService
                    lblNeedServiceIV.Visibility = Visibility.Collapsed;
                    lblNeedServiceKey.Visibility = Visibility.Collapsed;
                    keyNeedServiceIV.Visibility = Visibility.Collapsed;
                    keyNeedServiceKey.Visibility = Visibility.Collapsed;
                    fileNeedService.Visibility = Visibility.Collapsed;
                    // Show unencrypted NeedService
                    lblNeedServiceNoEncrypt.Visibility = Visibility.Visible;
                    keyNeedServiceNoEncrypt.Visibility = Visibility.Visible;
                    fileNeedServiceNoEncrypt.Visibility = Visibility.Visible;
                }
                plist = (PlistDict)plist.Parent;
            }
            else
            {
                lblNeedServiceIV.Visibility = Visibility.Collapsed;
                lblNeedServiceKey.Visibility = Visibility.Collapsed;
                keyNeedServiceIV.Visibility = Visibility.Collapsed;
                keyNeedServiceKey.Visibility = Visibility.Collapsed;
                fileNeedService.Visibility = Visibility.Collapsed;
                lblNeedServiceNoEncrypt.Visibility = Visibility.Collapsed;
                keyNeedServiceNoEncrypt.Visibility = Visibility.Collapsed;
                fileNeedServiceNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region RecoveryMode
            if (plist.Exists("RecoveryMode"))
            {
                plist = plist.Get<PlistDict>("RecoveryMode");
                if (plist.Exists("Encryption") &&
                    plist.Get<PlistBool>("Encryption").Value)
                {
                    // Hide unencrypted RecoveryMode
                    lblRecoveryModeNoEncrypt.Visibility = Visibility.Collapsed;
                    keyRecoveryModeNoEncrypt.Visibility = Visibility.Collapsed;
                    fileRecoveryModeNoEncrypt.Visibility = Visibility.Collapsed;
                    // Show encrypted RecoveryMode
                    lblRecoveryModeIV.Visibility = Visibility.Visible;
                    lblRecoveryModeKey.Visibility = Visibility.Visible;
                    keyRecoveryModeIV.Visibility = Visibility.Visible;
                    keyRecoveryModeKey.Visibility = Visibility.Visible;
                    fileRecoveryMode.Visibility = Visibility.Visible;
                    // Keys
                    keyRecoveryModeIV.Text = plist.Get<PlistString>("IV").Value;
                    keyRecoveryModeKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide encrypted RecoveryMode
                    lblRecoveryModeIV.Visibility = Visibility.Collapsed;
                    lblRecoveryModeKey.Visibility = Visibility.Collapsed;
                    keyRecoveryModeIV.Visibility = Visibility.Collapsed;
                    keyRecoveryModeKey.Visibility = Visibility.Collapsed;
                    fileRecoveryMode.Visibility = Visibility.Collapsed;
                    // Show unencrypted RecoveryMode
                    lblRecoveryModeNoEncrypt.Visibility = Visibility.Visible;
                    keyRecoveryModeNoEncrypt.Visibility = Visibility.Visible;
                    fileRecoveryModeNoEncrypt.Visibility = Visibility.Visible;
                }
                plist = (PlistDict)plist.Parent;
            }
            else
            {
                lblRecoveryModeIV.Visibility = Visibility.Collapsed;
                lblRecoveryModeKey.Visibility = Visibility.Collapsed;
                keyRecoveryModeIV.Visibility = Visibility.Collapsed;
                keyRecoveryModeKey.Visibility = Visibility.Collapsed;
                fileRecoveryMode.Visibility = Visibility.Collapsed;
                lblRecoveryModeNoEncrypt.Visibility = Visibility.Collapsed;
                keyRecoveryModeNoEncrypt.Visibility = Visibility.Collapsed;
                fileRecoveryModeNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region SEP-Firmware
            if (plist.Exists("SEP-Firmware"))
            {
                plist = plist.Get<PlistDict>("SEP-Firmware");
                if (plist.Exists("Encryption") &&
                    plist.Get<PlistBool>("Encryption").Value)
                {
                    // Hide unencrypted SEP-Firmware
                    lblSEPFirmwareNoEncrypt.Visibility = Visibility.Collapsed;
                    keySEPFirmwareNoEncrypt.Visibility = Visibility.Collapsed;
                    fileSEPFirmwareNoEncrypt.Visibility = Visibility.Collapsed;
                    // Show encrypted SEP-Firmware
                    lblSEPFirmwareIV.Visibility = Visibility.Visible;
                    lblSEPFirmwareKey.Visibility = Visibility.Visible;
                    keySEPFirmwareIV.Visibility = Visibility.Visible;
                    keySEPFirmwareKey.Visibility = Visibility.Visible;
                    fileSEPFirmware.Visibility = Visibility.Visible;
                    // Keys
                    keySEPFirmwareIV.Text = plist.Get<PlistString>("IV").Value;
                    keySEPFirmwareKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide encrypted SEP-Firmware
                    lblSEPFirmwareIV.Visibility = Visibility.Collapsed;
                    lblSEPFirmwareKey.Visibility = Visibility.Collapsed;
                    keySEPFirmwareIV.Visibility = Visibility.Collapsed;
                    keySEPFirmwareKey.Visibility = Visibility.Collapsed;
                    fileSEPFirmware.Visibility = Visibility.Collapsed;
                    // Show unencrypted SEP-Firmware
                    lblSEPFirmwareNoEncrypt.Visibility = Visibility.Visible;
                    keySEPFirmwareNoEncrypt.Visibility = Visibility.Visible;
                    fileSEPFirmwareNoEncrypt.Visibility = Visibility.Visible;
                }
                plist = (PlistDict)plist.Parent;
            }
            else
            {
                lblSEPFirmwareIV.Visibility = Visibility.Collapsed;
                lblSEPFirmwareKey.Visibility = Visibility.Collapsed;
                keySEPFirmwareIV.Visibility = Visibility.Collapsed;
                keySEPFirmwareKey.Visibility = Visibility.Collapsed;
                fileSEPFirmware.Visibility = Visibility.Collapsed;
                lblSEPFirmwareNoEncrypt.Visibility = Visibility.Collapsed;
                keySEPFirmwareNoEncrypt.Visibility = Visibility.Collapsed;
                fileSEPFirmwareNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion

            // Cleanup
            try
            {
                doc.Dispose();
            }
            catch (Exception)
            {
            }
            doc = null;
        }

        private void btnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            Debug("DECRYPT", "Validating input.");
            #region Input Validation
            if (String.IsNullOrWhiteSpace(textInputFileName.Text) ||
                String.IsNullOrWhiteSpace(textOutputFileName.Text) ||
                String.IsNullOrWhiteSpace(textDecryptKey.Text))
            {
                return;
            }
            if (!File.Exists(textInputFileName.Text))
            {
                MessageBox.Show(
                    "The input file does not exist.",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (File.Exists(textOutputFileName.Text))
            {
                if (MessageBox.Show(
                    "The output file already exists. Shall I delete it?",
                    "iDecryptIt",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return;
                }
                File.Delete(textOutputFileName.Text);
            }
            #endregion

            decryptFrom = textInputFileName.Text;
            decryptTo = textOutputFileName.Text;
            decryptFromFile = new FileInfo(decryptFrom);

            Debug("DECRYPT", "Launching dmg.");
            ProcessStartInfo x = new ProcessStartInfo();
            x.RedirectStandardError = true;
            x.RedirectStandardOutput = true;
            x.UseShellExecute = false;
            x.FileName = rundir + "dmg.exe";
            x.Arguments = String.Format("extract \"{0}\" \"{1}\" -k {2}", textInputFileName.Text, textOutputFileName.Text, textDecryptKey.Text);
            Debug("DECRYPT", "Args: " + x.Arguments);
            x.ErrorDialog = true;

            decryptProc = new Process();
            decryptProc.EnableRaisingEvents = true;
            decryptProc.OutputDataReceived +=decryptProc_OutputDataReceived;
            decryptProc.StartInfo = x;
            decryptProc.ErrorDataReceived += decryptProc_ErrorDataReceived;
            decryptProc.Start();
            decryptProc.BeginOutputReadLine(); // The program pauses if the buffer is full
            decryptProc.BeginErrorReadLine();

            // Screen mods
            gridDecrypt.IsEnabled = false;
            progDecrypt.Visibility = Visibility.Visible;

            // Wait for file to exist before starting worker (processes are asynchronous)
            while (!File.Exists(decryptTo))
            {
            }
            Debug("DECRYPT", "Starting progress checker.");
            decryptworker = new BackgroundWorker();
            decryptworker.WorkerSupportsCancellation = true;
            decryptworker.WorkerReportsProgress = true;
            decryptworker.DoWork += decryptworker_DoWork;
            decryptworker.ProgressChanged += decryptworker_ProgressReported;
            decryptworker.RunWorkerAsync();
        }

        private void decryptProc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        void decryptProc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
        private void btnExtract_Click(object sender, RoutedEventArgs e)
        {
            Debug("EXTRACT", "Validating input.");
            #region Input validation
            if (String.IsNullOrWhiteSpace(text7ZInputFileName.Text) ||
                String.IsNullOrWhiteSpace(text7ZOuputFolder.Text))
            {
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

            Debug("EXTRACT", "Launching 7zip.");
            Process.Start(
                rundir + "7z.exe",
                " x \"" + text7ZInputFileName.Text + "\" \"" + "-o" + text7ZOuputFolder.Text + "\"");

            // Prepare to extract HFS
            /*string[] files = Directory.GetFiles(tempdir, "*.hfs*", SearchOption.AllDirectories);
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
                if (String.IsNullOrWhiteSpace(extract.SafeFileName) || !File.Exists(extract.FileName))
                {
                    return;
                }
                file = extract.FileName;
            }

            Process.Start(
                rundir + "7z.exe",
                " x \"" + file + "\" \"" + "-o" + text7ZOuputFolder.Text + "\"");*/
        }
        private void btnChangelog_Click(object sender, RoutedEventArgs e)
        {
            Debug("CHANGE", "Loading Changelog.");
            Process.Start("file://" + helpdir + "changelog.html");
        }
        private void btnREADME_Click(object sender, RoutedEventArgs e)
        {
            Debug("README", "Loading README.");
            Process.Start("file://" + helpdir + "README.html");
        }
        /*private void btn1a420_Click(object sender, RoutedEventArgs e)
        {
            // TODO: https://mega.co.nz/#!Ml8hyCQI!d2ihbCEvtkFcFSgldAPqIQ1_OpRIWAeJZl_HODWjC7s
            Process.Start("http://rapidshare.com/files/207764160/iphoneproto.zip");
        }*/
        private void btnSelectRootFSInputFile_Click(object sender, RoutedEventArgs e)
        {
            Debug("SELECTFS", "Loading file dialog.");
            OpenFileDialog decrypt = new OpenFileDialog();
            decrypt.FileName = "";
            decrypt.RestoreDirectory = true;
            decrypt.DefaultExt = ".dmg";
            decrypt.Filter = "Apple Disk Images|*.dmg";
            decrypt.ShowDialog();
            Debug("SELECTFS", "Closing file dialog.");
            if (!String.IsNullOrWhiteSpace(decrypt.SafeFileName))
            {
                textInputFileName.Text = decrypt.FileName;
            }
        }
        private void btnSelect7ZInputFile_Click(object sender, RoutedEventArgs e)
        {
            Debug("SELECT7Z", "Loading file dialog.");
            OpenFileDialog extract = new OpenFileDialog();
            extract.FileName = "";
            extract.RestoreDirectory = true;
            extract.Multiselect = false;
            extract.DefaultExt = ".dmg";
            extract.Filter = "Apple Disk Images|*.dmg";//|Apple Firmware Files|*.ipsw";
            extract.ShowDialog();
            Debug("SELECT7Z", "Closing file dialog.");
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
            Debug("SELECTWHAT", "Opening file dialog.");
            OpenFileDialog what = new OpenFileDialog();
            what.FileName = "";
            what.RestoreDirectory = true;
            what.Multiselect = false;
            what.DefaultExt = ".ipsw";
            what.Filter = "Apple Firmware Files|*.ipsw";
            what.ShowDialog();
            Debug("SELECTWHAT", "Closing file dialog.");
            if (!String.IsNullOrWhiteSpace(what.SafeFileName))
            {
                textWhatAmIFileName.Text = what.SafeFileName;
            }
        }
        private void btnWhatAmI_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Open the archive and parse the Restore.plist file
            string[] strArr;
            string device;
            string version;
            string build;

            if (String.IsNullOrWhiteSpace(textWhatAmIFileName.Text))
                return;

            strArr = textWhatAmIFileName.Text.Split('_');
            if (strArr.Length != 4)
            {
                MessageBox.Show(
                    "The supplied IPSW File that was given is not in the following format:\r\n" +
                        "{DEVICE}_{VERSION}_{BUILD}_Restore.ipsw",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (strArr[3] != "Restore.ipsw")
            {
                MessageBox.Show(
                    "The supplied IPSW File that was given is not in the following format:\r\n" +
                        "{DEVICE}_{VERSION}_{BUILD}_Restore.ipsw",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            device = strArr[0];
            version = strArr[1];
            build = strArr[2];
            // TODO: Replace this with a dictionary
            // TODO: Update Apple TV versions
            #region Device Switch
            switch (device)
            {
                case "iPad1,1":
                    device = "iPad 1G";
                    break;
                case "iPad2,1":
                    device = "iPad 2 Wi-Fi";
                    break;
                case "iPad2,2":
                    device = "iPad 2 GSM";
                    break;
                case "iPad2,3":
                    device = "iPad 2 CDMA";
                    break;
                case "iPad2,4":
                    device = "iPad 2 Wi-Fi (Rev A)";
                    break;
                case "iPad2,5":
                    device = "iPad mini 1G Wi-Fi";
                    break;
                case "iPad2,6":
                    device = "iPad mini 1G GSM";
                    break;
                case "iPad2,7":
                    device = "iPad mini 1G Global";
                    break;
                case "iPad3,1":
                    device = "iPad 3 Wi-Fi";
                    break;
                case "iPad3,2":
                    device = "iPad 3 CDMA";
                    break;
                case "iPad3,3":
                    device = "iPad 3 Global";
                    break;
                case "iPad3,4":
                    device = "iPad 4 Wi-Fi";
                    break;
                case "iPad3,5":
                    device = "iPad 4 GSM";
                    break;
                case "iPad3,6":
                    device = "iPad 4 Global";
                    break;
                case "iPad4,1":
                    device = "iPad Air Wi-Fi";
                    break;
                case "iPad4,2":
                    device = "iPad Air Cellular";
                    break;
                case "iPad4,3":
                    device = "iPad Air Cellular (Rev A)";
                    break;
                case "iPad4,4":
                    device = "iPad mini 2G Wi-Fi";
                    break;
                case "iPad4,5":
                    device = "iPad mini 2G Cellular";
                    break;
                case "iPad4,6":
                    device = "iPad mini 2G Cellular (Rev A)";
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
                    device = "iPhone 4 GSM (Rev A)";
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
                    device = "iPhone 5 Global";
                    break;
                case "iPhone5,3":
                    device = "iPhone 5c GSM";
                    break;
                case "iPhone5,4":
                    device = "iPhone 5c Global";
                    break;
                case "iPhone6,1":
                    device = "iPhone 5s GSM";
                    break;
                case "iPhone6,2":
                    device = "iPhone 5s Global";
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
                        case "10A406e":
                            version = "5.1/6.0";
                            break;
                        case "10B5105c":
                        case "10B5117b":
                        case "10B5126b":
                        case "10B144b":
                            version = "5.2/6.1";
                            break;
                        case "10B329a":
                            version = "5.2.1/6.1.3";
                            break;
                        case "10B809":
                            version = "5.3/6.1.4";
                            break;
                        case "11A4372q":
                        case "11A4400f":
                            version = "5.4/6.0";
                            break;
                        case "11A4435d":
                        case "11A4449a":
                            version = "6.0/7.0";
                            break;
                        case "11A470e":
                            version = "6.0/7.0.1";
                            break;
                        case "11A502":
                            version = "6.0/7.0.2";
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
                        case "10A406e":
                            version = "5.1/6.0";
                            break;
                        case "10B5105c":
                        case "10B5117b":
                        case "10B5126b":
                        case "10B144b":
                            version = "5.2/6.1";
                            break;
                        case "10B329a":
                            version = "5.2.1/6.1.3";
                            break;
                        case "10B809":
                            version = "5.3/6.1.4";
                            break;
                        case "11A4372q":
                        case "11A4400f":
                            version = "5.4/6.0";
                            break;
                        case "11A4435d":
                        case "11A4449a":
                            version = "6.0/7.0";
                            break;
                        case "11A470e":
                            version = "6.0/7.0.1";
                            break;
                        case "11A502":
                            version = "6.0/7.0.2";
                            break;
                    }
                    #endregion
                    break;
                case "AppleTV3,2":
                    device = "Apple TV 3G (Rev A)";
                    #region Apple TV 3G Rev A
                    switch (build)
                    {
                        case "10B144b":
                            version = "5.2/6.1";
                            break;
                        case "10B329a":
                            version = "5.2.1/6.1.3";
                            break;
                        case "10B809":
                            version = "5.3/6.1.4";
                            break;
                        case "11A4372q":
                        case "11A4400f":
                            version = "5.4/6.0";
                            break;
                        case "11A4435d":
                        case "11A4449a":
                            version = "6.0/7.0";
                            break;
                        case "11A470e":
                            version = "6.0/7.0.1";
                            break;
                        case "11A502":
                            version = "6.0/7.0.2";
                            break;
                    }
                    #endregion
                    break;
                default:
                    MessageBox.Show(
                        "The supplied device: '" + device + "' does not follow the format:\r\n" +
                            "\t{iPad/iPhone/iPad/AppleTV}{#},{#}" +
                            "or is not supported at the moment.",
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
        private void textInputFileName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] split = textInputFileName.Text.Split('\\');
            int length = split.Length;
            string lastIndex = split[length - 1];
            string returntext = null;
            for (int i = 0; i < length; i++)
            {
                if (i == 0)
                {
                    returntext = split[0];
                }
                else if (i == length - 1)
                {
                    returntext = Path.Combine(
                        returntext,
                        lastIndex.Substring(0, lastIndex.Length - 4) + "_decrypted.dmg");
                }
                else
                {
                    returntext = Path.Combine(returntext, split[i]);
                }
            }
            textOutputFileName.Text = returntext;
        }
        private void key_Click(object sender, RoutedEventArgs e)
        {
            // Remove "btn", then split
            string[] value = ((MenuItem)sender).Name.Substring(3).Split('_');
            /*bool gm = value[1].Contains("GM");
            if (gm)
            {
                // remove GM
                value[1] = value[1].Substring(0, value[1].Length - 2);
            }*/

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
            LoadKey(stream, false); // gm);
        }
        private void keySelect_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ContextMenu menu = button.ContextMenu;
            menu.IsEnabled = true;
            menu.Placement = PlacementMode.Bottom;
            menu.PlacementTarget = button;
            menu.IsOpen = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Find a temp directory
            Debug("INIT", "Finding a temp directory.");
            tempdir = Path.Combine(
                Path.GetTempPath(),
                "Hexware",
                "iDecryptIt_" + new Random().Next(0, Int32.MaxValue).ToString("X")) + "\\";

            if (!Directory.Exists(tempdir))
            {
                Directory.CreateDirectory(tempdir);
            }

            // Check for updates
            Debug("INIT", "Checking for updates.");
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFile(
                    @"http://theiphonewiki.com/w/index.php?title=User:5urd/Latest_stable_software_release/iDecryptIt&action=raw",
                    tempdir + "update.txt");
                webClient.Dispose();

                string version = File.ReadAllText(tempdir + "update.txt");
                Debug("INIT", "Installed version: " + GlobalVars.Version);
                Debug("INIT", "Latest version: " + version);
#if !DEBUG
                if (version != GlobalVars.Version)
                {
                    MessageBox.Show(
                        "Update Available.",
                        "iDecryptIt: Update Available",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
#endif
            }
            catch (Exception)
            {
            }

            // Passed argument (see console portion)
            Debug("INIT", "Checking for program argument.");
            if (GlobalVars.ExecutionArgs.ContainsKey("dmg"))
            {
                textInputFileName.Text = (string)GlobalVars.ExecutionArgs["dmg"];
            }
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Debug("DEINIT", "Closing.");
            Cleanup();
            Thread.Sleep(500);
            Application.Current.Shutdown();
        }
    }
}