using Hexware.DataManipulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hexware.Programs.iDecryptIt
{
    /// <summary>
    /// Interaction logic for KeysControl.xaml
    /// </summary>
    public partial class KeysControl : Window
    {
        string nokey = "";
        MainWindow mainwindow;

        public KeysControl(MainWindow mw, AssocArray<object> values)
        {
            InitializeComponent();
            mainwindow = mw; // Used to access the VFDecrypt and XPwn key boxes
            
            // Device + Version
            txtDevice.Text = values.Get("device").ToString();
            txtVersion.Text = values.Get("version").ToString() + "(Build " + values.Get("build").ToString() + ")";
            switch (values.Get("build").ToString())
            {
                case "5A345":
                    // 2.0
                    if (Convert.ToBoolean(values.Get("isgm")) == true)
                    {
                        txtVersion.Text = txtVersion.Text + " [GM]";
                    }
                    break;

                case "8A293":
                    // 4.0
                    if (values.Get("device").ToString() != "iphone31" &&
                        Convert.ToBoolean(values.Get("isgm")) == true)
                    {
                        txtVersion.Text = txtVersion.Text + " [GM]";
                    }
                    break;

                case "9A334":
                    // 5.0
                    if (Convert.ToBoolean(values.Get("isgm")) == true)
                    {
                        txtVersion.Text = txtVersion.Text + " [GM]";
                    }
                    break;
            }
            // VFDecrypt
            keyVFDecrypt.Text = values.Exists("vfdecrypt") ? values.Get("vfdecrypt").ToString() : nokey;
            fileVFDecrypt.Text = values.Exists("vfdecryptdmg") ? values.Get("vfdecryptdmg").ToString() + ".dmg" : "XXX-XXXX-XXX.dmg";
            // Ramdisks
            if (Convert.ToBoolean(values.Get("NoUpdateRamdisk")) == true)
            {
                lblUpdateIV.Visibility = Visibility.Collapsed;
                lblUpdateKey.Visibility = Visibility.Collapsed;
                lblUpdateNoEncrypt.Visibility = Visibility.Collapsed;
                keyUpdateIV.Visibility = Visibility.Collapsed;
                keyUpdateKey.Visibility = Visibility.Collapsed;
                keyUpdateNoEncrypt.Visibility = Visibility.Collapsed;
                fileUpdate.Visibility = Visibility.Collapsed;
                fileUpdateNoEncrypt.Visibility = Visibility.Collapsed;
            }
            if (Convert.ToBoolean(values.Get("RamdiskNotEncrypted")) == true)
            {
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
                lblUpdateNoEncrypt.Visibility = Visibility.Collapsed;
                keyUpdateNoEncrypt.Visibility = Visibility.Collapsed;
                fileUpdateNoEncrypt.Visibility = Visibility.Collapsed;
                lblRestoreNoEncrypt.Visibility = Visibility.Collapsed;
                keyRestoreNoEncrypt.Visibility = Visibility.Collapsed;
                fileRestoreNoEncrypt.Visibility = Visibility.Collapsed;
            }
            // AppleLogo
            if (Convert.ToBoolean(values.Get("AppleLogoNotEncrypted")) == true)
            {
                lblAppleLogoIV.Visibility = Visibility.Collapsed;
                lblAppleLogoKey.Visibility = Visibility.Collapsed;
                keyAppleLogoIV.Visibility = Visibility.Collapsed;
                keyAppleLogoKey.Visibility = Visibility.Collapsed;
                fileAppleLogo.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblAppleLogoNoEncrypt.Visibility = Visibility.Collapsed;
                keyAppleLogoNoEncrypt.Visibility = Visibility.Collapsed;
                fileAppleLogoNoEncrypt.Visibility = Visibility.Collapsed;
            }
            // BatteryLow0
            if (Convert.ToBoolean(values.Get("BatteryLow0NotEncrypted")) == true)
            {
                lblBatteryLow0IV.Visibility = Visibility.Collapsed;
                lblBatteryLow0Key.Visibility = Visibility.Collapsed;
                keyBatteryLow0IV.Visibility = Visibility.Collapsed;
                keyBatteryLow0Key.Visibility = Visibility.Collapsed;
                fileBatteryLow0.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblBatteryLow0NoEncrypt.Visibility = Visibility.Collapsed;
                keyBatteryLow0NoEncrypt.Visibility = Visibility.Collapsed;
                fileBatteryLow0NoEncrypt.Visibility = Visibility.Collapsed;
            }
            // BatteryLow1
            if (Convert.ToBoolean(values.Get("BatteryLow1NotEncrypted")) == true)
            {
                lblBatteryLow1IV.Visibility = Visibility.Collapsed;
                lblBatteryLow1Key.Visibility = Visibility.Collapsed;
                keyBatteryLow1IV.Visibility = Visibility.Collapsed;
                keyBatteryLow1Key.Visibility = Visibility.Collapsed;
                fileBatteryLow1.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblBatteryLow1NoEncrypt.Visibility = Visibility.Collapsed;
                keyBatteryLow1NoEncrypt.Visibility = Visibility.Collapsed;
                fileBatteryLow1NoEncrypt.Visibility = Visibility.Collapsed;
            }
            // BatteryFull
            if (Convert.ToBoolean(values.Get("BatteryFullNotEncrypted")) == true)
            {
                lblBatteryFullIV.Visibility = Visibility.Collapsed;
                lblBatteryFullKey.Visibility = Visibility.Collapsed;
                keyBatteryFullIV.Visibility = Visibility.Collapsed;
                keyBatteryFullKey.Visibility = Visibility.Collapsed;
                fileBatteryFull.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblBatteryFullNoEncrypt.Visibility = Visibility.Collapsed;
                keyBatteryFullNoEncrypt.Visibility = Visibility.Collapsed;
                fileBatteryFullNoEncrypt.Visibility = Visibility.Collapsed;
            }
            // DeviceTree
            if (Convert.ToBoolean(values.Get("DeviceTreeNotEncrypted")) == true)
            {
                lblDeviceTreeIV.Visibility = Visibility.Collapsed;
                lblDeviceTreeKey.Visibility = Visibility.Collapsed;
                keyDeviceTreeIV.Visibility = Visibility.Collapsed;
                keyDeviceTreeKey.Visibility = Visibility.Collapsed;
                fileDeviceTree.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblDeviceTreeNoEncrypt.Visibility = Visibility.Collapsed;
                keyDeviceTreeNoEncrypt.Visibility = Visibility.Collapsed;
                fileDeviceTreeNoEncrypt.Visibility = Visibility.Collapsed;
            }
            // GlyphCharging
            if (Convert.ToBoolean(values.Get("GlyphChargingNotEncrypted")) == true)
            {
                lblGlyphChargingIV.Visibility = Visibility.Collapsed;
                lblGlyphChargingKey.Visibility = Visibility.Collapsed;
                keyGlyphChargingIV.Visibility = Visibility.Collapsed;
                keyGlyphChargingKey.Visibility = Visibility.Collapsed;
                fileGlyphCharging.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblGlyphChargingNoEncrypt.Visibility = Visibility.Collapsed;
                keyGlyphChargingNoEncrypt.Visibility = Visibility.Collapsed;
                fileGlyphChargingNoEncrypt.Visibility = Visibility.Collapsed;
            }
            // GlyphPlugin
            if (Convert.ToBoolean(values.Get("GlyphPluginNotEncrypted")) == true)
            {
                lblGlyphPluginIV.Visibility = Visibility.Collapsed;
                lblGlyphPluginKey.Visibility = Visibility.Collapsed;
                keyGlyphPluginIV.Visibility = Visibility.Collapsed;
                keyGlyphPluginKey.Visibility = Visibility.Collapsed;
                fileGlyphPlugin.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblGlyphPluginNoEncrypt.Visibility = Visibility.Collapsed;
                keyGlyphPluginNoEncrypt.Visibility = Visibility.Collapsed;
                fileGlyphPluginNoEncrypt.Visibility = Visibility.Collapsed;
            }
            // iBEC
            if (Convert.ToBoolean(values.Get("iBECNotEncrypted")) == true)
            {
                lbliBECIV.Visibility = Visibility.Collapsed;
                lbliBECKey.Visibility = Visibility.Collapsed;
                keyiBECIV.Visibility = Visibility.Collapsed;
                keyiBECKey.Visibility = Visibility.Collapsed;
                fileiBEC.Visibility = Visibility.Collapsed;
            }
            else
            {
                lbliBECNoEncrypt.Visibility = Visibility.Collapsed;
                keyiBECNoEncrypt.Visibility = Visibility.Collapsed;
                fileiBECNoEncrypt.Visibility = Visibility.Collapsed;
            }
            // iBoot
            if (Convert.ToBoolean(values.Get("iBootNotEncrypted")) == true)
            {
                lbliBootIV.Visibility = Visibility.Collapsed;
                lbliBootKey.Visibility = Visibility.Collapsed;
                keyiBootIV.Visibility = Visibility.Collapsed;
                keyiBootKey.Visibility = Visibility.Collapsed;
                fileiBoot.Visibility = Visibility.Collapsed;
            }
            else
            {
                lbliBootNoEncrypt.Visibility = Visibility.Collapsed;
                keyiBootNoEncrypt.Visibility = Visibility.Collapsed;
                fileiBootNoEncrypt.Visibility = Visibility.Collapsed;
            }
            // iBSS
            if (Convert.ToBoolean(values.Get("iBSSNotEncrypted")) == true)
            {
                lbliBSSIV.Visibility = Visibility.Collapsed;
                lbliBSSKey.Visibility = Visibility.Collapsed;
                keyiBSSIV.Visibility = Visibility.Collapsed;
                keyiBSSKey.Visibility = Visibility.Collapsed;
                fileiBSS.Visibility = Visibility.Collapsed;
            }
            else
            {
                lbliBSSNoEncrypt.Visibility = Visibility.Collapsed;
                keyiBSSNoEncrypt.Visibility = Visibility.Collapsed;
                fileiBSSNoEncrypt.Visibility = Visibility.Collapsed;
            }
            // KernelCache
            if (Convert.ToBoolean(values.Get("KernelCacheNotEncrypted")) == true)
            {
                lblKernelCacheIV.Visibility = Visibility.Collapsed;
                lblKernelCacheKey.Visibility = Visibility.Collapsed;
                keyKernelCacheIV.Visibility = Visibility.Collapsed;
                keyKernelCacheKey.Visibility = Visibility.Collapsed;
                fileKernelCache.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblKernelCacheNoEncrypt.Visibility = Visibility.Collapsed;
                keyKernelCacheNoEncrypt.Visibility = Visibility.Collapsed;
                fileKernelCacheNoEncrypt.Visibility = Visibility.Collapsed;
            }
            // LLB
            if (Convert.ToBoolean(values.Get("LLBNoteEncrypted")) == true)
            {
                lblLLBIV.Visibility = Visibility.Collapsed;
                lblLLBKey.Visibility = Visibility.Collapsed;
                keyLLBIV.Visibility = Visibility.Collapsed;
                keyLLBKey.Visibility = Visibility.Collapsed;
                fileLLB.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblLLBNoEncrypt.Visibility = Visibility.Collapsed;
                keyLLBNoEncrypt.Visibility = Visibility.Collapsed;
                fileLLBNoEncrypt.Visibility = Visibility.Collapsed;
            }
            // NeedService
            if (Convert.ToBoolean(values.Get("NeedServiceNotEncrypted")) == true)
            {
                lblNeedServiceIV.Visibility = Visibility.Collapsed;
                lblNeedServiceKey.Visibility = Visibility.Collapsed;
                keyNeedServiceIV.Visibility = Visibility.Collapsed;
                keyNeedServiceKey.Visibility = Visibility.Collapsed;
                fileNeedService.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblNeedServiceNoEncrypt.Visibility = Visibility.Collapsed;
                keyNeedServiceNoEncrypt.Visibility = Visibility.Collapsed;
                fileNeedServiceNoEncrypt.Visibility = Visibility.Collapsed;
            }
            // RecoveryMode
            if (Convert.ToBoolean(values.Get("RecoveryModeNotEncrypted")) == true)
            {
                lblRecoveryModeIV.Visibility = Visibility.Collapsed;
                lblRecoveryModeKey.Visibility = Visibility.Collapsed;
                keyRecoveryModeIV.Visibility = Visibility.Collapsed;
                keyRecoveryModeKey.Visibility = Visibility.Collapsed;
                fileRecoveryMode.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblRecoveryModeNoEncrypt.Visibility = Visibility.Collapsed;
                keyRecoveryModeNoEncrypt.Visibility = Visibility.Collapsed;
                fileRecoveryModeNoEncrypt.Visibility = Visibility.Collapsed;
            }
        }
    }
}