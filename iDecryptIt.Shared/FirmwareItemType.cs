/* =============================================================================
 * File:   FirmwareItemType.cs
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

using JetBrains.Annotations;

namespace iDecryptIt.Shared;

[PublicAPI]
public enum FirmwareItemType
{
    RootFS,
    RootFSBeta,
    UpdateRamdisk,
    UpdateRamdisk2,
    UpdateRamdiskOTA,
    UpdateRamdiskOTA2,
    UpdateRamdiskBeta,
    UpdateRamdiskBeta2,
    RestoreRamdisk,
    RestoreRamdisk2,
    RestoreRamdiskBeta,
    RestoreRamdiskBeta2,
    ACIBTFirmware,
    ACIBTFirmware2,
    ACIWiFiFirmware,
    ACIWiFiFirmware2,
    ADCPetra,
    ADCPetra2,
    ANE,
    ANE2,
    ANSF,
    ANSF2,
    AOPFirmware,
    AOPFirmware2,
    AppleAVE,
    AppleAVE2,
    AppleLogo,
    AppleLogo2,
    AppleMaggie,
    AppleMaggie2,
    ARMFW,
    ARMFW2,
    AudioCodecFirmware,
    AudioCodecFirmware2,
    AudioDSP,
    AudioDSP2,
    BatteryCharging,
    BatteryCharging2,
    BatteryCharging0,
    BatteryCharging02,
    BatteryCharging1,
    BatteryCharging12,
    BatteryFull,
    BatteryFull2,
    BatteryLow0,
    BatteryLow02,
    BatteryLow1,
    BatteryLow12,
    Dali,
    Dali2,
    DCP,
    DCP2,
    DeviceTree,
    DeviceTree2,
    GlyphCharging,
    GlyphCharging2,
    GlyphPlugin,
    GlyphPlugin2,
    HapticAssets,
    HapticAssets2,
    Homer,
    Homer2,
    iBEC,
    iBEC2,
    iBoot,
    iBoot2,
    iBootData,
    iBootData2,
    iBSS,
    iBSS2,
    ISP,
    ISP2,
    Kernelcache,
    Kernelcache2,
    LeapHaptics,
    LeapHaptics2,
    LiquidDetect,
    LiquidDetect2,
    LLB,
    LLB2,
    LowPowerMode,
    LowPowerMode2,
    LowPowerFindMyMode,
    LowPowerFindMyMode2,
    MConnector,
    MConnector2,
    Multitouch,
    Multitouch2,
    NeedService,
    NeedService2,
    PMP,
    PMP2,
    RANS,
    RANS2,
    RecoveryMode,
    RecoveryMode2,
    RTPFirmware,
    RTPFirmware2,
    SEPFirmware,
    SEPFirmware2,
    SmartIOFirmware,
    SmartIOFirmware2,
    WirelessPower,
    WirelessPower2,
}
