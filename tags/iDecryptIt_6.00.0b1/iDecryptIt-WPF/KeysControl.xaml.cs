using Hexware.Plist;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Text;

namespace Hexware.Programs.iDecryptIt
{
    /// <summary>
    /// Interaction logic for KeysControl.xaml
    /// </summary>
    public partial class KeysControl : Window
    {
        private const string nokey = "";
        private MainWindow mainwindow;
        private PlistDocument doc = null;
        private bool loadSuccess = true;
        private bool gm;

        internal KeysControl(MainWindow mw, Stream document, bool goldMaster)
        {
            try
            {
                doc = new PlistDocument(document);
            }
            catch (Exception ex)
            {
                mainwindow.Error("Error loading key file.", ex);
                try
                {
                    // Wrap it in case the constructor has disposed of it already.
                    doc.Dispose();
                }
                catch (Exception)
                {
                }
                doc = null;
                loadSuccess = false;
                this.Close();
                return;
            }
            InitializeComponent();
            mainwindow = mw;
            gm = goldMaster;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!loadSuccess)
            {
                return;
            }

            PlistDict plist = doc.Document.Value;
            string temp;

            // Set up variables
            #region Device
            txtDevice.Text = plist.Get<PlistString>("Device").Value;
            #endregion
            #region Version
            StringBuilder sb = new StringBuilder(64);
            temp = plist.Get<PlistString>("Build").Value;
            sb.Append(plist.Get<PlistString>("Version").Value);
            sb.Append(" (Build " + temp + ")");
            switch (temp)
            {
                case "5A345":
                    // 2.0
                    if (gm && plist.Get<PlistString>("Device").Value != "iPhone 3G")
                    {
                        sb.Append(" [GM]");
                    }
                    break;

                case "8A293":
                    // 4.0
                    if (gm && plist.Get<PlistString>("Device").Value != "iPhone 4 (GSM)")
                    {
                        sb.Append(" [GM]");
                    }
                    break;

                case "9A334":
                    // 5.0
                    if (gm && plist.Get<PlistString>("Device").Value != "iPhone 4S")
                    {
                        sb.Append(" [GM]");
                    }
                    break;
            }
            txtVersion.Text = sb.ToString();
            sb.Clear();
            #endregion
            #region VFDecrypt Key
            temp = plist.Get<PlistDict>("Root FS").Get<PlistString>("File Name").Value;
            fileVFDecrypt.Text = (temp != "XXX-XXXX-XXX" && temp != "") ? temp + ".dmg" : "XXX-XXXX-XXX.dmg";
            temp = plist.Get<PlistDict>("Root FS").Get<PlistString>((gm) ? "GM Key" : "Key").Value;
            keyVFDecrypt.Text = (temp == "TODO") ? nokey : temp;
            #endregion
            #region Ramdisks
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
            fileRestore.Text = plist.Get<PlistDict>("Restore Ramdisk").Get<PlistString>("File Name").Value + ".dmg";
            fileRestoreNoEncrypt.Text = fileRestore.Text;
            if (plist.Exists("Ramdisk Not Encrypted") && plist.Get<PlistBool>("Ramdisk Not Encrypted").Value)
            {
                // Hide Encrypted Ramdisks
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
                // Hide Unencrypted Ramdisks
                lblUpdateNoEncrypt.Visibility = Visibility.Collapsed;
                keyUpdateNoEncrypt.Visibility = Visibility.Collapsed;
                fileUpdateNoEncrypt.Visibility = Visibility.Collapsed;
                lblRestoreNoEncrypt.Visibility = Visibility.Collapsed;
                keyRestoreNoEncrypt.Visibility = Visibility.Collapsed;
                fileRestoreNoEncrypt.Visibility = Visibility.Collapsed;
                // Keys
                if (!plist.Exists("No Update Ramdisk") || !plist.Get<PlistBool>("No Update Ramdisk").Value)
                {
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
                    // Hide Unencrypted AppleLogo
                    lblAppleLogoNoEncrypt.Visibility = Visibility.Collapsed;
                    keyAppleLogoNoEncrypt.Visibility = Visibility.Collapsed;
                    fileAppleLogoNoEncrypt.Visibility = Visibility.Collapsed;
                    // Keys
                    keyAppleLogoIV.Text = plist.Get<PlistString>("IV").Value;
                    keyAppleLogoKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide Encrypted AppleLogo
                    lblAppleLogoIV.Visibility = Visibility.Collapsed;
                    lblAppleLogoKey.Visibility = Visibility.Collapsed;
                    keyAppleLogoIV.Visibility = Visibility.Collapsed;
                    keyAppleLogoKey.Visibility = Visibility.Collapsed;
                    fileAppleLogo.Visibility = Visibility.Collapsed;
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
                    // Hide Unencrypted BatterCharging0
                    lblBatteryCharging0NoEncrypt.Visibility = Visibility.Collapsed;
                    keyBatteryCharging0NoEncrypt.Visibility = Visibility.Collapsed;
                    fileBatteryCharging0NoEncrypt.Visibility = Visibility.Collapsed;
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
                    // Hide Unencrypted BatteryCharging1
                    lblBatteryCharging1NoEncrypt.Visibility = Visibility.Collapsed;
                    keyBatteryCharging1NoEncrypt.Visibility = Visibility.Collapsed;
                    fileBatteryCharging1NoEncrypt.Visibility = Visibility.Collapsed;
                    // Keys
                    keyBatteryCharging1IV.Text = plist.Get<PlistString>("IV").Value;
                    keyBatteryCharging1Key.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide Encrypted BatteryCharging1
                    lblBatteryCharging1IV.Visibility = Visibility.Collapsed;
                    lblBatteryCharging1Key.Visibility = Visibility.Collapsed;
                    keyBatteryCharging1IV.Visibility = Visibility.Collapsed;
                    keyBatteryCharging1Key.Visibility = Visibility.Collapsed;
                    fileBatteryCharging1.Visibility = Visibility.Collapsed;
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
                    // Hide Unencrypted BatteryFull
                    lblBatteryFullNoEncrypt.Visibility = Visibility.Collapsed;
                    keyBatteryFullNoEncrypt.Visibility = Visibility.Collapsed;
                    fileBatteryFullNoEncrypt.Visibility = Visibility.Collapsed;
                    // Keys
                    keyBatteryFullIV.Text = plist.Get<PlistString>("IV").Value;
                    keyBatteryFullKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide Encrypted BatteryFull
                    lblBatteryFullIV.Visibility = Visibility.Collapsed;
                    lblBatteryFullKey.Visibility = Visibility.Collapsed;
                    keyBatteryFullIV.Visibility = Visibility.Collapsed;
                    keyBatteryFullKey.Visibility = Visibility.Collapsed;
                    fileBatteryFull.Visibility = Visibility.Collapsed;
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
                    // Hide Unencrypted BatteryLow0
                    lblBatteryLow0NoEncrypt.Visibility = Visibility.Collapsed;
                    keyBatteryLow0NoEncrypt.Visibility = Visibility.Collapsed;
                    fileBatteryLow0NoEncrypt.Visibility = Visibility.Collapsed;
                    // Keys
                    keyBatteryLow0IV.Text = plist.Get<PlistString>("IV").Value;
                    keyBatteryLow0Key.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide Encrypted BatteryLow0
                    lblBatteryLow0IV.Visibility = Visibility.Collapsed;
                    lblBatteryLow0Key.Visibility = Visibility.Collapsed;
                    keyBatteryLow0IV.Visibility = Visibility.Collapsed;
                    keyBatteryLow0Key.Visibility = Visibility.Collapsed;
                    fileBatteryLow0.Visibility = Visibility.Collapsed;
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
                    // Hide Unencrypted BatteryLow1
                    lblBatteryLow1NoEncrypt.Visibility = Visibility.Collapsed;
                    keyBatteryLow1NoEncrypt.Visibility = Visibility.Collapsed;
                    fileBatteryLow1NoEncrypt.Visibility = Visibility.Collapsed;
                    // Keys
                    keyBatteryLow1IV.Text = plist.Get<PlistString>("IV").Value;
                    keyBatteryLow1Key.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide Encrypted BatteryLow1
                    lblBatteryLow1IV.Visibility = Visibility.Collapsed;
                    lblBatteryLow1Key.Visibility = Visibility.Collapsed;
                    keyBatteryLow1IV.Visibility = Visibility.Collapsed;
                    keyBatteryLow1Key.Visibility = Visibility.Collapsed;
                    fileBatteryLow1.Visibility = Visibility.Collapsed;
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
                    // Hide Unencrypted DeviceTree
                    lblDeviceTreeNoEncrypt.Visibility = Visibility.Collapsed;
                    keyDeviceTreeNoEncrypt.Visibility = Visibility.Collapsed;
                    fileDeviceTreeNoEncrypt.Visibility = Visibility.Collapsed;
                    // Keys
                    keyDeviceTreeIV.Text = plist.Get<PlistString>("IV").Value;
                    keyDeviceTreeKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide Encrypted DeviceTree
                    lblDeviceTreeIV.Visibility = Visibility.Collapsed;
                    lblDeviceTreeKey.Visibility = Visibility.Collapsed;
                    keyDeviceTreeIV.Visibility = Visibility.Collapsed;
                    keyDeviceTreeKey.Visibility = Visibility.Collapsed;
                    fileDeviceTree.Visibility = Visibility.Collapsed;
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
                    // Hide Unencrypted GlyphCharging
                    lblGlyphChargingNoEncrypt.Visibility = Visibility.Collapsed;
                    keyGlyphChargingNoEncrypt.Visibility = Visibility.Collapsed;
                    fileGlyphChargingNoEncrypt.Visibility = Visibility.Collapsed;
                    // Keys
                    keyGlyphChargingIV.Text = plist.Get<PlistString>("IV").Value;
                    keyGlyphChargingKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide Encrypted GlyphCharging
                    lblGlyphChargingIV.Visibility = Visibility.Collapsed;
                    lblGlyphChargingKey.Visibility = Visibility.Collapsed;
                    keyGlyphChargingIV.Visibility = Visibility.Collapsed;
                    keyGlyphChargingKey.Visibility = Visibility.Collapsed;
                    fileGlyphCharging.Visibility = Visibility.Collapsed;
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
                    // Hide Unencrypted GlyphPlugin
                    lblGlyphPluginNoEncrypt.Visibility = Visibility.Collapsed;
                    keyGlyphPluginNoEncrypt.Visibility = Visibility.Collapsed;
                    fileGlyphPluginNoEncrypt.Visibility = Visibility.Collapsed;
                    // Keys
                    keyGlyphPluginIV.Text = plist.Get<PlistString>("IV").Value;
                    keyGlyphPluginKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide Encrypted GlyphPlugin
                    lblGlyphPluginIV.Visibility = Visibility.Collapsed;
                    lblGlyphPluginKey.Visibility = Visibility.Collapsed;
                    keyGlyphPluginIV.Visibility = Visibility.Collapsed;
                    keyGlyphPluginKey.Visibility = Visibility.Collapsed;
                    fileGlyphPlugin.Visibility = Visibility.Collapsed;
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
                    // Hide Unencrypted iBEC
                    lbliBECNoEncrypt.Visibility = Visibility.Collapsed;
                    keyiBECNoEncrypt.Visibility = Visibility.Collapsed;
                    fileiBECNoEncrypt.Visibility = Visibility.Collapsed;
                    // Keys
                    keyiBECIV.Text = plist.Get<PlistString>("IV").Value;
                    keyiBECKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide Encrypted iBEC
                    lbliBECIV.Visibility = Visibility.Collapsed;
                    lbliBECKey.Visibility = Visibility.Collapsed;
                    keyiBECIV.Visibility = Visibility.Collapsed;
                    keyiBECKey.Visibility = Visibility.Collapsed;
                    fileiBEC.Visibility = Visibility.Collapsed;
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
                    // Hide Unencrypted iBoot
                    lbliBootNoEncrypt.Visibility = Visibility.Collapsed;
                    keyiBootNoEncrypt.Visibility = Visibility.Collapsed;
                    fileiBootNoEncrypt.Visibility = Visibility.Collapsed;
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
                    // Hide Unencrypted iBSS
                    lbliBSSNoEncrypt.Visibility = Visibility.Collapsed;
                    keyiBSSNoEncrypt.Visibility = Visibility.Collapsed;
                    fileiBSSNoEncrypt.Visibility = Visibility.Collapsed;
                    // Keys
                    keyiBSSIV.Text = plist.Get<PlistString>("IV").Value;
                    keyiBSSKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide Encrypted iBSS
                    lbliBSSIV.Visibility = Visibility.Collapsed;
                    lbliBSSKey.Visibility = Visibility.Collapsed;
                    keyiBSSIV.Visibility = Visibility.Collapsed;
                    keyiBSSKey.Visibility = Visibility.Collapsed;
                    fileiBSS.Visibility = Visibility.Collapsed;
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
                    // Hide Unencrypted Kernelcache
                    lblKernelcacheNoEncrypt.Visibility = Visibility.Collapsed;
                    keyKernelcacheNoEncrypt.Visibility = Visibility.Collapsed;
                    fileKernelcacheNoEncrypt.Visibility = Visibility.Collapsed;
                    // Keys
                    keyKernelcacheIV.Text = plist.Get<PlistString>("IV").Value;
                    keyKernelcacheKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide Encrypted Kernelcache
                    lblKernelcacheIV.Visibility = Visibility.Collapsed;
                    lblKernelcacheKey.Visibility = Visibility.Collapsed;
                    keyKernelcacheIV.Visibility = Visibility.Collapsed;
                    keyKernelcacheKey.Visibility = Visibility.Collapsed;
                    fileKernelcache.Visibility = Visibility.Collapsed;
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
                    // Hide Unencrypted LLB
                    lblLLBNoEncrypt.Visibility = Visibility.Collapsed;
                    keyLLBNoEncrypt.Visibility = Visibility.Collapsed;
                    fileLLBNoEncrypt.Visibility = Visibility.Collapsed;
                    // Keys
                    keyLLBIV.Text = plist.Get<PlistString>("IV").Value;
                    keyLLBKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide Encrypted LLB
                    lblLLBIV.Visibility = Visibility.Collapsed;
                    lblLLBKey.Visibility = Visibility.Collapsed;
                    keyLLBIV.Visibility = Visibility.Collapsed;
                    keyLLBKey.Visibility = Visibility.Collapsed;
                    fileLLB.Visibility = Visibility.Collapsed;
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
                    // Hide Unencrypted NeedService
                    lblNeedServiceNoEncrypt.Visibility = Visibility.Collapsed;
                    keyNeedServiceNoEncrypt.Visibility = Visibility.Collapsed;
                    fileNeedServiceNoEncrypt.Visibility = Visibility.Collapsed;
                    // Keys
                    keyNeedServiceIV.Text = plist.Get<PlistString>("IV").Value;
                    keyNeedServiceKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide Encrypted NeedService
                    lblNeedServiceIV.Visibility = Visibility.Collapsed;
                    lblNeedServiceKey.Visibility = Visibility.Collapsed;
                    keyNeedServiceIV.Visibility = Visibility.Collapsed;
                    keyNeedServiceKey.Visibility = Visibility.Collapsed;
                    fileNeedService.Visibility = Visibility.Collapsed;
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
                    // Hide Unencrypted RecoveryMode
                    lblRecoveryModeNoEncrypt.Visibility = Visibility.Collapsed;
                    keyRecoveryModeNoEncrypt.Visibility = Visibility.Collapsed;
                    fileRecoveryModeNoEncrypt.Visibility = Visibility.Collapsed;
                    // Keys
                    keyRecoveryModeIV.Text = plist.Get<PlistString>("IV").Value;
                    keyRecoveryModeKey.Text = plist.Get<PlistString>("Key").Value;
                }
                else
                {
                    // Hide Encrypted RecoveryMode
                    lblRecoveryModeIV.Visibility = Visibility.Collapsed;
                    lblRecoveryModeKey.Visibility = Visibility.Collapsed;
                    keyRecoveryModeIV.Visibility = Visibility.Collapsed;
                    keyRecoveryModeKey.Visibility = Visibility.Collapsed;
                    fileRecoveryMode.Visibility = Visibility.Collapsed;
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
    }
}