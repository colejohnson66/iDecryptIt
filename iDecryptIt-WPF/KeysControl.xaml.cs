using Hexware.DataManipulation;
using Hexware.Plist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Hexware.Programs.iDecryptIt
{
    /// <summary>
    /// Interaction logic for KeysControl.xaml
    /// </summary>
    public partial class KeysControl : Window
    {
        private const string nokey = "";
        private MainWindow mainwindow;

        public KeysControl(MainWindow mw, string path, bool gm)
        {
            PlistDict plist;
            string temp;
            try
            {
                plist = new PlistDoc(path).Data.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                this.Close();
                return;
            }
            mainwindow = mw;

            // Set up variables
            // Use collapsed to "collapse" the row they are in
            #region Device
            switch (plist.Get<PlistString>("Device").Value)
            {
                case "appletv21":
                    txtDevice.Text = "Apple TV 2G";
                    break;
                case "appletv31":
                    txtDevice.Text = "Apple TV 3G";
                    break;
                case "ipad11":
                    txtDevice.Text = "iPad 1G";
                    break;
                case "ipad21":
                    txtDevice.Text = "iPad 2 (Wi-Fi)";
                    break;
                case "ipad22":
                    txtDevice.Text = "iPad 2 (GSM)";
                    break;
                case "ipad23":
                    txtDevice.Text = "iPad 2 (CDMA)";
                    break;
                case "ipad24":
                    txtDevice.Text = "iPad 2 (Wi-Fi) [R2]";
                    break;
                case "ipad31":
                    txtDevice.Text = "iPad 2 (Wi-Fi)";
                    break;
                case "ipad32":
                    txtDevice.Text = "iPad 2 (GSM)";
                    break;
                case "ipad33":
                    txtDevice.Text = "iPad 3 (Global)";
                    break;
                case "iphone11":
                    txtDevice.Text = "iPhone 2G";
                    break;
                case "iphone12":
                    txtDevice.Text = "iPhone 3G";
                    break;
                case "iphone21":
                    txtDevice.Text = "iPhone 3GS";
                    break;
                case "iphone31":
                    txtDevice.Text = "iPhone 4 (GSM)";
                    break;
                case "iphone33":
                    txtDevice.Text = "iPhone 4 (CDMA)";
                    break;
                case "iphone41":
                    txtDevice.Text = "iPhone 4S";
                    break;
                case "ipod11":
                    txtDevice.Text = "iPod touch 1G";
                    break;
                case "ipod21":
                    txtDevice.Text = "iPod touch 2G";
                    break;
                case "ipod31":
                    txtDevice.Text = "iPod touch 3G";
                    break;
                case "ipod41":
                    txtDevice.Text = "iPod touch 4G";
                    break;
            }
            #endregion
            #region Version
            temp = plist.Get<PlistString>("Build").Value;
            txtVersion.Text =
                plist.Get<PlistString>("Version").Value +
                " (Build " + temp + ")";

            switch (temp)
            {
                case "5A345":
                    // 2.0
                    if (gm)
                    {
                        txtVersion.Text = txtVersion.Text + " [GM]";
                    }
                    break;

                case "8A293":
                    // 4.0
                    if (gm && plist.Get<PlistString>("Device").Value != "iphone31")
                    {
                        txtVersion.Text = txtVersion.Text + " [GM]";
                    }
                    break;

                case "9A334":
                    // 5.0
                    if (gm)
                    {
                        txtVersion.Text = txtVersion.Text + " [GM]";
                    }
                    break;
            }
            #endregion
            #region VFDecrypt Key
            temp = plist.Get<PlistDict>("Root FS").Get<PlistString>("Key").Value;
            keyVFDecrypt.Text = (temp == "TODO") ? temp : nokey;
            temp = plist.Get<PlistDict>("Root FS").Get<PlistString>("File Name").Value;
            fileVFDecrypt.Text = (temp == "XXX-XXXX-XXX" || temp == "") ? temp + ".dmg" : "XXX-XXXX-XXX.dmg";
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
            }
            #endregion
            #region AppleLogo
            if (plist.Get<PlistDict>("AppleLogo").Exists("Encryption") &&
                plist.Get<PlistDict>("AppleLogo").Get<PlistBool>("Encryption").Value)
            {
                // Hide Encrypted AppleLogo
                lblAppleLogoIV.Visibility = Visibility.Collapsed;
                lblAppleLogoKey.Visibility = Visibility.Collapsed;
                keyAppleLogoIV.Visibility = Visibility.Collapsed;
                keyAppleLogoKey.Visibility = Visibility.Collapsed;
                fileAppleLogo.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Hide Unencrypted AppleLogo
                lblAppleLogoNoEncrypt.Visibility = Visibility.Collapsed;
                keyAppleLogoNoEncrypt.Visibility = Visibility.Collapsed;
                fileAppleLogoNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region BatteryCharging0
            if (plist.Get<PlistDict>("BatteryCharging0").Exists("Encryption") &&
                plist.Get<PlistDict>("BatteryCharging0").Get<PlistBool>("Encryption").Value)
            {
                // Hide Encrypted BatteryCharging0
                lblBatteryLow0IV.Visibility = Visibility.Collapsed;
                lblBatteryLow0Key.Visibility = Visibility.Collapsed;
                keyBatteryLow0IV.Visibility = Visibility.Collapsed;
                keyBatteryLow0Key.Visibility = Visibility.Collapsed;
                fileBatteryLow0.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Hide Unencrypted BatterCharging0
                lblBatteryLow0NoEncrypt.Visibility = Visibility.Collapsed;
                keyBatteryLow0NoEncrypt.Visibility = Visibility.Collapsed;
                fileBatteryLow0NoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region BatteryCharging1
            if (plist.Get<PlistDict>("BatteryCharging1").Exists("Encryption") &&
                plist.Get<PlistDict>("BatteryCharging1").Get<PlistBool>("Encryption").Value)
            {
                // Hide Encrypted BatteryCharging1
                lblBatteryLow1IV.Visibility = Visibility.Collapsed;
                lblBatteryLow1Key.Visibility = Visibility.Collapsed;
                keyBatteryLow1IV.Visibility = Visibility.Collapsed;
                keyBatteryLow1Key.Visibility = Visibility.Collapsed;
                fileBatteryLow1.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Hide Unencrypted BatterCharging1
                lblBatteryLow1NoEncrypt.Visibility = Visibility.Collapsed;
                keyBatteryLow1NoEncrypt.Visibility = Visibility.Collapsed;
                fileBatteryLow1NoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region BatteryFull
            if (plist.Get<PlistDict>("BatteryFull").Exists("Encryption") &&
                plist.Get<PlistDict>("BatteryFull").Get<PlistBool>("Encryption").Value)
            {
                // Hide Encrypted BatteryFull
                lblBatteryFullIV.Visibility = Visibility.Collapsed;
                lblBatteryFullKey.Visibility = Visibility.Collapsed;
                keyBatteryFullIV.Visibility = Visibility.Collapsed;
                keyBatteryFullKey.Visibility = Visibility.Collapsed;
                fileBatteryFull.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Hide Unencrypted BatterFull
                lblBatteryFullNoEncrypt.Visibility = Visibility.Collapsed;
                keyBatteryFullNoEncrypt.Visibility = Visibility.Collapsed;
                fileBatteryFullNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region BatteryLow0
            if (plist.Get<PlistDict>("BatteryLow0").Exists("Encryption") &&
                plist.Get<PlistDict>("BatteryLow0").Get<PlistBool>("Encryption").Value)
            {
                // Hide Encrypted BatteryLow0
                lblBatteryLow0IV.Visibility = Visibility.Collapsed;
                lblBatteryLow0Key.Visibility = Visibility.Collapsed;
                keyBatteryLow0IV.Visibility = Visibility.Collapsed;
                keyBatteryLow0Key.Visibility = Visibility.Collapsed;
                fileBatteryLow0.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Hide Unencrypted BatteryLow0
                lblBatteryLow0NoEncrypt.Visibility = Visibility.Collapsed;
                keyBatteryLow0NoEncrypt.Visibility = Visibility.Collapsed;
                fileBatteryLow0NoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region BatteryLow1
            if (plist.Get<PlistDict>("BatteryLow1").Exists("Encryption") &&
                plist.Get<PlistDict>("BatteryLow1").Get<PlistBool>("Encryption").Value)
            {
                // Hide Encrypted BatteryLow1
                lblBatteryLow1IV.Visibility = Visibility.Collapsed;
                lblBatteryLow1Key.Visibility = Visibility.Collapsed;
                keyBatteryLow1IV.Visibility = Visibility.Collapsed;
                keyBatteryLow1Key.Visibility = Visibility.Collapsed;
                fileBatteryLow1.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Hide Unencrypted BatteryLow1
                lblBatteryLow1NoEncrypt.Visibility = Visibility.Collapsed;
                keyBatteryLow1NoEncrypt.Visibility = Visibility.Collapsed;
                fileBatteryLow1NoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region DeviceTree
            if (plist.Get<PlistDict>("DeviceTree").Exists("Encryption") &&
                plist.Get<PlistDict>("DeviceTree").Get<PlistBool>("Encryption").Value)
            {
                // Hide Encrypted DeviceTree
                lblDeviceTreeIV.Visibility = Visibility.Collapsed;
                lblDeviceTreeKey.Visibility = Visibility.Collapsed;
                keyDeviceTreeIV.Visibility = Visibility.Collapsed;
                keyDeviceTreeKey.Visibility = Visibility.Collapsed;
                fileDeviceTree.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Hide Unencrypted DeviceTree
                lblDeviceTreeNoEncrypt.Visibility = Visibility.Collapsed;
                keyDeviceTreeNoEncrypt.Visibility = Visibility.Collapsed;
                fileDeviceTreeNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region GlyphCharging
            if (plist.Get<PlistDict>("GlyphCharging").Exists("Encryption") &&
                plist.Get<PlistDict>("GlyphCharging").Get<PlistBool>("Encryption").Value)
            {
                // Hide Encrypted GlyphCharging
                lblGlyphChargingIV.Visibility = Visibility.Collapsed;
                lblGlyphChargingKey.Visibility = Visibility.Collapsed;
                keyGlyphChargingIV.Visibility = Visibility.Collapsed;
                keyGlyphChargingKey.Visibility = Visibility.Collapsed;
                fileGlyphCharging.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Hide Unencrypted GlyphCharging
                lblGlyphChargingNoEncrypt.Visibility = Visibility.Collapsed;
                keyGlyphChargingNoEncrypt.Visibility = Visibility.Collapsed;
                fileGlyphChargingNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region GlyphPlugin
            if (plist.Get<PlistDict>("GlyphPlugin").Exists("Encryption") &&
                plist.Get<PlistDict>("GlyphPlugin").Get<PlistBool>("Encryption").Value)
            {
                // Hide Encrypted GlyphPlugin
                lblGlyphPluginIV.Visibility = Visibility.Collapsed;
                lblGlyphPluginKey.Visibility = Visibility.Collapsed;
                keyGlyphPluginIV.Visibility = Visibility.Collapsed;
                keyGlyphPluginKey.Visibility = Visibility.Collapsed;
                fileGlyphPlugin.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Hide Unencrypted GlyphPlugin
                lblGlyphPluginNoEncrypt.Visibility = Visibility.Collapsed;
                keyGlyphPluginNoEncrypt.Visibility = Visibility.Collapsed;
                fileGlyphPluginNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region iBEC
            if (plist.Get<PlistDict>("iBEC").Exists("Encryption") &&
                plist.Get<PlistDict>("iBEC").Get<PlistBool>("Encryption").Value)
            {
                // Hide Encrypted iBEC
                lbliBECIV.Visibility = Visibility.Collapsed;
                lbliBECKey.Visibility = Visibility.Collapsed;
                keyiBECIV.Visibility = Visibility.Collapsed;
                keyiBECKey.Visibility = Visibility.Collapsed;
                fileiBEC.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Hide Unencrypted iBEC
                lbliBECNoEncrypt.Visibility = Visibility.Collapsed;
                keyiBECNoEncrypt.Visibility = Visibility.Collapsed;
                fileiBECNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region iBoot
            if (plist.Get<PlistDict>("iBoot").Exists("Encryption") &&
                plist.Get<PlistDict>("iBoot").Get<PlistBool>("Encryption").Value)
            {
                // Hide Encrypted iBoot
                lbliBootIV.Visibility = Visibility.Collapsed;
                lbliBootKey.Visibility = Visibility.Collapsed;
                keyiBootIV.Visibility = Visibility.Collapsed;
                keyiBootKey.Visibility = Visibility.Collapsed;
                fileiBoot.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Hide Unencrypted iBoot
                lbliBootNoEncrypt.Visibility = Visibility.Collapsed;
                keyiBootNoEncrypt.Visibility = Visibility.Collapsed;
                fileiBootNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region iBSS
            if (plist.Get<PlistDict>("iBSS").Exists("Encryption") &&
                plist.Get<PlistDict>("iBSS").Get<PlistBool>("Encryption").Value)
            {
                // Hide Encrypted iBSS
                lbliBSSIV.Visibility = Visibility.Collapsed;
                lbliBSSKey.Visibility = Visibility.Collapsed;
                keyiBSSIV.Visibility = Visibility.Collapsed;
                keyiBSSKey.Visibility = Visibility.Collapsed;
                fileiBSS.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Hide Unencrypted iBSS
                lbliBSSNoEncrypt.Visibility = Visibility.Collapsed;
                keyiBSSNoEncrypt.Visibility = Visibility.Collapsed;
                fileiBSSNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region Kernelcache
            if (plist.Get<PlistDict>("Kernelcache").Exists("Encryption") &&
                plist.Get<PlistDict>("Kernelcache").Get<PlistBool>("Encryption").Value)
            {
                // Hide Encrypted Kernelcache
                lblKernelcacheIV.Visibility = Visibility.Collapsed;
                lblKernelcacheKey.Visibility = Visibility.Collapsed;
                keyKernelcacheIV.Visibility = Visibility.Collapsed;
                keyKernelcacheKey.Visibility = Visibility.Collapsed;
                fileKernelcache.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Hide Unencrypted Kernelcache
                lblKernelcacheNoEncrypt.Visibility = Visibility.Collapsed;
                keyKernelcacheNoEncrypt.Visibility = Visibility.Collapsed;
                fileKernelcacheNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region LLB
            if (plist.Get<PlistDict>("LLB").Exists("Encryption") &&
                plist.Get<PlistDict>("LLB").Get<PlistBool>("Encryption").Value)
            {
                // Hide Encrypted LLB
                lblLLBIV.Visibility = Visibility.Collapsed;
                lblLLBKey.Visibility = Visibility.Collapsed;
                keyLLBIV.Visibility = Visibility.Collapsed;
                keyLLBKey.Visibility = Visibility.Collapsed;
                fileLLB.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Hide Unencrypted LLB
                lblLLBNoEncrypt.Visibility = Visibility.Collapsed;
                keyLLBNoEncrypt.Visibility = Visibility.Collapsed;
                fileLLBNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region NeedService
            if (plist.Get<PlistDict>("NeedService").Exists("Encryption") &&
                plist.Get<PlistDict>("NeedService").Get<PlistBool>("Encryption").Value)
            {
                // Hide Encrypted NeedService
                lblNeedServiceIV.Visibility = Visibility.Collapsed;
                lblNeedServiceKey.Visibility = Visibility.Collapsed;
                keyNeedServiceIV.Visibility = Visibility.Collapsed;
                keyNeedServiceKey.Visibility = Visibility.Collapsed;
                fileNeedService.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Hide Unencrypted NeedService
                lblNeedServiceNoEncrypt.Visibility = Visibility.Collapsed;
                keyNeedServiceNoEncrypt.Visibility = Visibility.Collapsed;
                fileNeedServiceNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
            #region RecoveryMode
            if (plist.Get<PlistDict>("RecoveryMode").Exists("Encryption") &&
                plist.Get<PlistDict>("RecoveryMode").Get<PlistBool>("Encryption").Value)
            {
                // Hide Encrypted RecoveryMode
                lblRecoveryModeIV.Visibility = Visibility.Collapsed;
                lblRecoveryModeKey.Visibility = Visibility.Collapsed;
                keyRecoveryModeIV.Visibility = Visibility.Collapsed;
                keyRecoveryModeKey.Visibility = Visibility.Collapsed;
                fileRecoveryMode.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Hide Unencrypted RecoveryMode
                lblRecoveryModeNoEncrypt.Visibility = Visibility.Collapsed;
                keyRecoveryModeNoEncrypt.Visibility = Visibility.Collapsed;
                fileRecoveryModeNoEncrypt.Visibility = Visibility.Collapsed;
            }
            #endregion
        }
    }
}