/* =============================================================================
 * File:   FirmwareItemTypeConverters.cs
 * Author: Cole Tobin
 * =============================================================================
 * Copyright (c) 2022 Cole Tobin
 *
 * This file is part of iDecryptIt.
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

using Avalonia;
using Avalonia.Data.Converters;
using iDecryptIt.Shared;
using System;
using System.Globalization;
using static iDecryptIt.Shared.FirmwareItemType;

namespace iDecryptIt.Converters;

public sealed class FirmwareItemTypeNameConverter : IValueConverter
{
    private FirmwareItemTypeNameConverter()
    { }

    public static FirmwareItemTypeNameConverter Instance { get; } = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not FirmwareItemType item)
            return AvaloniaProperty.UnsetValue;
        if (targetType != typeof(string))
            return AvaloniaProperty.UnsetValue;

        return item switch
        {
            FirmwareItemType.RootFS => "Root Filesystem",
            RootFSBeta => "Root Filesystem (Beta)",
            UpdateRamdisk or UpdateRamdisk2 => "Update Ramdisk",
            UpdateRamdiskOTA or UpdateRamdiskOTA2 => "Update Ramdisk (OTA)",
            UpdateRamdiskBeta or UpdateRamdiskBeta2 => "Update Ramdisk (Beta)",
            RestoreRamdisk or RestoreRamdisk2 => "Restore Ramdisk",
            RestoreRamdiskBeta or RestoreRamdiskBeta2 => "Restore Ramdisk (Beta)",
            ACIBTFirmware or ACIBTFirmware2 => "ACI BT Firmware",
            ACIWiFiFirmware or ACIWiFiFirmware2 => "ACI Wi-Fi Firmware",
            ADCPetra or ADCPetra2 => "ADC Petra",
            ANE or ANE2 => "ANE",
            ANSF or ANSF2 => "ANSF",
            AOPFirmware or AOPFirmware2 => "AOP Firmware",
            AppleAVE or AppleAVE2 => "Apple AVE",
            AppleLogo or AppleLogo2 => "AppleLogo",
            AppleMaggie or AppleMaggie2 => "Apple Maggie",
            ARMFW or ARMFW2 => "ARM Firmware",
            AudioCodecFirmware or AudioCodecFirmware2 => "AudioCodec Firmware",
            AudioDSP or AudioDSP2 => "Audio DSP",
            BatteryCharging or BatteryCharging2 => "BatteryCharging",
            BatteryCharging0 or BatteryCharging02 => "BatteryCharging0",
            BatteryCharging1 or BatteryCharging12 => "BatteryCharging1",
            BatteryFull or BatteryFull2 => "BatteryFull",
            BatteryLow0 or BatteryLow02 => "BatteryLow0",
            BatteryLow1 or BatteryLow12 => "BatteryLow1",
            Dali or Dali2 => "Dali",
            DCP or DCP2 => "DCP",
            DeviceTree or DeviceTree2 => "DeviceTree",
            GlyphCharging or GlyphCharging2 => "GlyphCharging",
            GlyphPlugin or GlyphPlugin2 => "GlyphPlugin",
            HapticAssets or HapticAssets2 => "HapticAssets",
            Homer or Homer2 => "Homer",
            iBEC or iBEC2 => "iBEC",
            iBoot or iBoot2 => "iBoot",
            iBootData or iBootData2 => "iBootData",
            iBSS or iBSS2 => "iBSS",
            ISP or ISP2 => "ISP",
            Kernelcache or Kernelcache2 => "Kernelcache",
            LeapHaptics or LeapHaptics2 => "LeapHaptics",
            LiquidDetect or LiquidDetect2 => "LiquidDetect",
            LLB or LLB2 => "LLB",
            LowPowerMode or LowPowerMode2 => "LowPowerMode",
            LowPowerFindMyMode or LowPowerFindMyMode2 => "LowPowerFindMyMode",
            MConnector or MConnector2 => "MConnector",
            Multitouch or Multitouch2 => "Multitouch",
            NeedService or NeedService2 => "NeedService",
            PMP or PMP2 => "PMP",
            RANS or RANS2 => "RANS",
            RecoveryMode or RecoveryMode2 => "RecoveryMode",
            RTPFirmware or RTPFirmware2 => "RTP Firmware",
            SEPFirmware or SEPFirmware2 => "SEP Firmware",
            SmartIOFirmware or SmartIOFirmware2 => "SmartIO Firmware",
            WirelessPower or WirelessPower2 => "Wireless Power",
            _ => AvaloniaProperty.UnsetValue,
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        AvaloniaProperty.UnsetValue;
}
