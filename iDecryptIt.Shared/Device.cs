/* =============================================================================
 * File:   Device.cs
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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using static iDecryptIt.Shared.DeviceGroup;

namespace iDecryptIt.Shared;

[PublicAPI]
public sealed class Device : IComparable, IComparable<Device>, IEquatable<Device>, IEquatable<string>
{
    // NOTE: When adding new devices, ensure they are added to `AllDevices`, `MappingGroupToDevices`, and `TryParse`

    public static readonly Device AppleTV2_1 = new(AppleTV, 2, 1); // 2nd gen
    public static readonly Device AppleTV3_1 = new(AppleTV, 3, 1); // 3rd gen
    public static readonly Device AppleTV3_2 = new(AppleTV, 3, 2);
    public static readonly Device AppleTV5_3 = new(AppleTV, 5, 3); // "HD"
    public static readonly Device AppleTV6_2 = new(AppleTV, 6, 2); // "4K"
    public static readonly Device AppleTV11_1 = new(AppleTV, 11, 1); // "4K" 2nd gen
    public static readonly Device Watch1_1 = new(AppleWatch, 1, 1); // 1st gen
    public static readonly Device Watch1_2 = new(AppleWatch, 1, 2);
    public static readonly Device Watch2_3 = new(AppleWatch, 2, 3); // series 2
    public static readonly Device Watch2_4 = new(AppleWatch, 2, 4);
    public static readonly Device Watch2_6 = new(AppleWatch, 2, 6); // series 1
    public static readonly Device Watch2_7 = new(AppleWatch, 2, 7);
    public static readonly Device Watch3_1 = new(AppleWatch, 3, 1); // series 3
    public static readonly Device Watch3_2 = new(AppleWatch, 3, 2);
    public static readonly Device Watch3_3 = new(AppleWatch, 3, 3);
    public static readonly Device Watch3_4 = new(AppleWatch, 3, 4);
    public static readonly Device Watch4_1 = new(AppleWatch, 4, 1); // series 4
    public static readonly Device Watch4_2 = new(AppleWatch, 4, 2);
    public static readonly Device Watch4_3 = new(AppleWatch, 4, 3);
    public static readonly Device Watch4_4 = new(AppleWatch, 4, 4);
    public static readonly Device Watch5_1 = new(AppleWatch, 5, 1); // series 5
    public static readonly Device Watch5_2 = new(AppleWatch, 5, 2);
    public static readonly Device Watch5_3 = new(AppleWatch, 5, 3);
    public static readonly Device Watch5_4 = new(AppleWatch, 5, 4);
    public static readonly Device Watch5_9 = new(AppleWatch, 5, 9); // SE
    public static readonly Device Watch5_10 = new(AppleWatch, 5, 10);
    public static readonly Device Watch5_11 = new(AppleWatch, 5, 11);
    public static readonly Device Watch5_12 = new(AppleWatch, 5, 12);
    public static readonly Device Watch6_1 = new(AppleWatch, 6, 1); // series 6
    public static readonly Device Watch6_2 = new(AppleWatch, 6, 2);
    public static readonly Device Watch6_3 = new(AppleWatch, 6, 3);
    public static readonly Device Watch6_4 = new(AppleWatch, 6, 4);
    public static readonly Device Watch6_6 = new(AppleWatch, 6, 6); // series 7
    public static readonly Device Watch6_7 = new(AppleWatch, 6, 7);
    public static readonly Device Watch6_8 = new(AppleWatch, 6, 8);
    public static readonly Device Watch6_9 = new(AppleWatch, 6, 9);
    public static readonly Device Watch6_10 = new(AppleWatch, 6, 10); // SE 2nd gen
    public static readonly Device Watch6_11 = new(AppleWatch, 6, 11);
    public static readonly Device Watch6_12 = new(AppleWatch, 6, 12);
    public static readonly Device Watch6_13 = new(AppleWatch, 6, 13);
    public static readonly Device Watch6_14 = new(AppleWatch, 6, 14); // series 8
    public static readonly Device Watch6_15 = new(AppleWatch, 6, 15);
    public static readonly Device Watch6_16 = new(AppleWatch, 6, 16);
    public static readonly Device Watch6_17 = new(AppleWatch, 6, 17);
    public static readonly Device Watch6_18 = new(AppleWatch, 6, 18); // ultra
    public static readonly Device iBridge2_1 = new(IBridge, 2, 1);
    public static readonly Device iBridge2_3 = new(IBridge, 2, 3);
    public static readonly Device iBridge2_4 = new(IBridge, 2, 4);
    public static readonly Device iBridge2_5 = new(IBridge, 2, 5);
    public static readonly Device iBridge2_6 = new(IBridge, 2, 6);
    public static readonly Device iBridge2_7 = new(IBridge, 2, 7);
    public static readonly Device iBridge2_8 = new(IBridge, 2, 8);
    public static readonly Device iBridge2_10 = new(IBridge, 2, 10);
    public static readonly Device iBridge2_12 = new(IBridge, 2, 12);
    public static readonly Device iBridge2_14 = new(IBridge, 2, 14);
    public static readonly Device iBridge2_15 = new(IBridge, 2, 15);
    public static readonly Device iBridge2_16 = new(IBridge, 2, 16);
    public static readonly Device iBridge2_19 = new(IBridge, 2, 19);
    public static readonly Device iBridge2_20 = new(IBridge, 2, 20);
    public static readonly Device iBridge2_21 = new(IBridge, 2, 21);
    public static readonly Device iBridge2_22 = new(IBridge, 2, 22);
    public static readonly Device AudioAccessory1_1 = new(AudioAccessory, 1, 1); // HomePod
    public static readonly Device AudioAccessory1_2 = new(AudioAccessory, 1, 2);
    public static readonly Device AudioAccessory5_1 = new(AudioAccessory, 5, 1); // HomePod mini
    public static readonly Device iPad1_1 = new(IPad, 1, 1); // (1st gen)
    public static readonly Device iPad2_1 = new(IPad, 2, 1); // 2nd gen
    public static readonly Device iPad2_2 = new(IPad, 2, 2);
    public static readonly Device iPad2_3 = new(IPad, 2, 3);
    public static readonly Device iPad2_4 = new(IPad, 2, 4);
    public static readonly Device iPad2_5 = new(IPadMini, 2, 5); // mini (1st gen)
    public static readonly Device iPad2_6 = new(IPadMini, 2, 6);
    public static readonly Device iPad2_7 = new(IPadMini, 2, 7);
    public static readonly Device iPad3_1 = new(IPad, 3, 1); // 3rd gen
    public static readonly Device iPad3_2 = new(IPad, 3, 2);
    public static readonly Device iPad3_3 = new(IPad, 3, 3);
    public static readonly Device iPad3_4 = new(IPad, 3, 4); // 4th gen
    public static readonly Device iPad3_5 = new(IPad, 3, 5);
    public static readonly Device iPad3_6 = new(IPad, 3, 6);
    public static readonly Device iPad4_1 = new(IPadAir, 4, 1); // Air (1st gen)
    public static readonly Device iPad4_2 = new(IPadAir, 4, 2);
    public static readonly Device iPad4_3 = new(IPadAir, 4, 3);
    public static readonly Device iPad4_4 = new(IPadMini, 4, 4); // mini 2
    public static readonly Device iPad4_5 = new(IPadMini, 4, 5);
    public static readonly Device iPad4_6 = new(IPadMini, 4, 6);
    public static readonly Device iPad4_7 = new(IPadMini, 4, 7); // mini 3
    public static readonly Device iPad4_8 = new(IPadMini, 4, 8);
    public static readonly Device iPad4_9 = new(IPadMini, 4, 9);
    public static readonly Device iPad5_1 = new(IPadMini, 5, 1); // mini 4
    public static readonly Device iPad5_2 = new(IPadMini, 5, 2);
    public static readonly Device iPad5_3 = new(IPadAir, 5, 3); // Air 2
    public static readonly Device iPad5_4 = new(IPadAir, 5, 4);
    public static readonly Device iPad6_3 = new(IPadPro, 6, 3); // Pro (1st gen) 9.7"
    public static readonly Device iPad6_4 = new(IPadPro, 6, 4);
    public static readonly Device iPad6_7 = new(IPadPro, 6, 7); // Pro (1st gen) 12.9"
    public static readonly Device iPad6_8 = new(IPadPro, 6, 8);
    public static readonly Device iPad6_11 = new(IPad, 6, 11); // 5th gen
    public static readonly Device iPad6_12 = new(IPad, 6, 12);
    public static readonly Device iPad7_1 = new(IPadPro, 7, 1); // Pro 2nd gen 12.9"
    public static readonly Device iPad7_2 = new(IPadPro, 7, 2);
    public static readonly Device iPad7_3 = new(IPadPro, 7, 3); // Pro 2nd gen 10.5"
    public static readonly Device iPad7_4 = new(IPadPro, 7, 4);
    public static readonly Device iPad7_5 = new(IPad, 7, 5); // 6th gen
    public static readonly Device iPad7_6 = new(IPad, 7, 6);
    public static readonly Device iPad7_11 = new(IPad, 7, 11); // 7th gen
    public static readonly Device iPad7_12 = new(IPad, 7, 12);
    public static readonly Device iPad8_1 = new(IPadPro, 8, 1); // Pro 3rd gen 11"
    public static readonly Device iPad8_2 = new(IPadPro, 8, 2);
    public static readonly Device iPad8_3 = new(IPadPro, 8, 3);
    public static readonly Device iPad8_4 = new(IPadPro, 8, 4);
    public static readonly Device iPad8_5 = new(IPadPro, 8, 5); // Pro 3rd gen 12.9"
    public static readonly Device iPad8_6 = new(IPadPro, 8, 6);
    public static readonly Device iPad8_7 = new(IPadPro, 8, 7);
    public static readonly Device iPad8_8 = new(IPadPro, 8, 8);
    public static readonly Device iPad8_9 = new(IPadPro, 8, 9); // Pro 4th gen 11"
    public static readonly Device iPad8_10 = new(IPadPro, 8, 10);
    public static readonly Device iPad8_11 = new(IPadPro, 8, 11); // Pro 4th gen 12.9"
    public static readonly Device iPad8_12 = new(IPadPro, 8, 12);
    public static readonly Device iPad11_1 = new(IPadMini, 11, 1); // mini 5th gen
    public static readonly Device iPad11_2 = new(IPadMini, 11, 2);
    public static readonly Device iPad11_3 = new(IPadAir, 11, 3); // Air 3rd gen
    public static readonly Device iPad11_4 = new(IPadAir, 11, 4);
    public static readonly Device iPad11_6 = new(IPad, 11, 6); // 8th gen
    public static readonly Device iPad11_7 = new(IPad, 11, 7);
    public static readonly Device iPad12_1 = new(IPad, 12, 1); // 9th gen
    public static readonly Device iPad12_2 = new(IPad, 12, 2);
    public static readonly Device iPad13_1 = new(IPadAir, 13, 1); // Air 4th gen
    public static readonly Device iPad13_2 = new(IPadAir, 13, 2);
    public static readonly Device iPad13_4 = new(IPadPro, 13, 4); // Pro 5th gen 11"
    public static readonly Device iPad13_5 = new(IPadPro, 13, 5);
    public static readonly Device iPad13_6 = new(IPadPro, 13, 6);
    public static readonly Device iPad13_7 = new(IPadPro, 13, 7);
    public static readonly Device iPad13_8 = new(IPadPro, 13, 8); // Pro 5th gen 12.9"
    public static readonly Device iPad13_9 = new(IPadPro, 13, 9);
    public static readonly Device iPad13_10 = new(IPadPro, 13, 10);
    public static readonly Device iPad13_11 = new(IPadPro, 13, 11);
    public static readonly Device iPad13_16 = new(IPadAir, 13, 16); // Air 5th gen
    public static readonly Device iPad13_17 = new(IPadAir, 13, 17);
    public static readonly Device iPad14_1 = new(IPadMini, 14, 1); // mini 6th gen
    public static readonly Device iPad14_2 = new(IPadMini, 14, 2);
    public static readonly Device iPhone1_1 = new(IPhone, 1, 1); // (1st gen)
    public static readonly Device iPhone1_2 = new(IPhone, 1, 2); // 3G
    public static readonly Device iPhone2_1 = new(IPhone, 2, 1); // 3GS
    public static readonly Device iPhone3_1 = new(IPhone, 3, 1); // 4
    public static readonly Device iPhone3_2 = new(IPhone, 3, 2);
    public static readonly Device iPhone3_3 = new(IPhone, 3, 3);
    public static readonly Device iPhone4_1 = new(IPhone, 4, 1); // 4S
    public static readonly Device iPhone5_1 = new(IPhone, 5, 1); // 5
    public static readonly Device iPhone5_2 = new(IPhone, 5, 2);
    public static readonly Device iPhone5_3 = new(IPhone, 5, 3); // 5c
    public static readonly Device iPhone5_4 = new(IPhone, 5, 4);
    public static readonly Device iPhone6_1 = new(IPhone, 6, 1); // 5s
    public static readonly Device iPhone6_2 = new(IPhone, 6, 2);
    public static readonly Device iPhone7_1 = new(IPhone, 7, 1); // 6+
    public static readonly Device iPhone7_2 = new(IPhone, 7, 2); // 6
    public static readonly Device iPhone8_1 = new(IPhone, 8, 1); // 6s
    public static readonly Device iPhone8_2 = new(IPhone, 8, 2); // 6s+
    public static readonly Device iPhone8_4 = new(IPhone, 8, 4); // SE
    public static readonly Device iPhone9_1 = new(IPhone, 9, 1); // 7
    public static readonly Device iPhone9_2 = new(IPhone, 9, 2); // 7+
    public static readonly Device iPhone9_3 = new(IPhone, 9, 3); // 7
    public static readonly Device iPhone9_4 = new(IPhone, 9, 4); // 7+
    public static readonly Device iPhone10_1 = new(IPhone, 10, 1); // 8
    public static readonly Device iPhone10_2 = new(IPhone, 10, 2); // 8+
    public static readonly Device iPhone10_3 = new(IPhone, 10, 3); // X
    public static readonly Device iPhone10_4 = new(IPhone, 10, 4); // 8
    public static readonly Device iPhone10_5 = new(IPhone, 10, 5); // 8+
    public static readonly Device iPhone10_6 = new(IPhone, 10, 6); // X
    public static readonly Device iPhone11_2 = new(IPhone, 11, 2); // XS
    public static readonly Device iPhone11_4 = new(IPhone, 11, 4); // XS Max
    public static readonly Device iPhone11_6 = new(IPhone, 11, 6);
    public static readonly Device iPhone11_8 = new(IPhone, 11, 8); // XR
    public static readonly Device iPhone12_1 = new(IPhone, 12, 1); // 11
    public static readonly Device iPhone12_3 = new(IPhone, 12, 3); // 11 Pro
    public static readonly Device iPhone12_5 = new(IPhone, 12, 5); // 11 Pro Max
    public static readonly Device iPhone12_8 = new(IPhone, 12, 8); // SE 2nd gen
    public static readonly Device iPhone13_1 = new(IPhone, 13, 1); // 12 mini
    public static readonly Device iPhone13_2 = new(IPhone, 13, 2); // 12
    public static readonly Device iPhone13_3 = new(IPhone, 13, 3); // 12 Pro
    public static readonly Device iPhone13_4 = new(IPhone, 13, 4); // 12 Pro Max
    public static readonly Device iPhone14_2 = new(IPhone, 14, 2); // 13 Pro
    public static readonly Device iPhone14_3 = new(IPhone, 14, 3); // 13 Pro Max
    public static readonly Device iPhone14_4 = new(IPhone, 14, 4); // 13 mini
    public static readonly Device iPhone14_5 = new(IPhone, 14, 5); // 13
    public static readonly Device iPhone14_6 = new(IPhone, 14, 6); // SE 3rd gen
    public static readonly Device iPhone14_7 = new(IPhone, 14, 7); // 14
    public static readonly Device iPhone14_8 = new(IPhone, 14, 8); // 14 Plus
    public static readonly Device iPhone15_2 = new(IPhone, 15, 2); // 14 Pro
    public static readonly Device iPhone15_3 = new(IPhone, 15, 3); // 14 Pro Max
    public static readonly Device iPod1_1 = new(IPodTouch, 1, 1); // (1st gen)
    public static readonly Device iPod2_1 = new(IPodTouch, 2, 1); // 2nd gen
    public static readonly Device iPod3_1 = new(IPodTouch, 3, 1); // 3rd gen
    public static readonly Device iPod4_1 = new(IPodTouch, 4, 1); // 4th gen
    public static readonly Device iPod5_1 = new(IPodTouch, 5, 1); // 5th gen
    public static readonly Device iPod7_1 = new(IPodTouch, 7, 1); // 6th gen
    public static readonly Device iPod9_1 = new(IPodTouch, 9, 1); // 7th gen

    public static readonly ReadOnlyCollection<Device> AllDevices = Array.AsReadOnly(
        new[]
        {
            AppleTV2_1, AppleTV3_1, AppleTV3_2, AppleTV5_3, AppleTV6_2, AppleTV11_1, Watch1_1, Watch1_2, Watch2_3,
            Watch2_4, Watch2_6, Watch2_7, Watch3_1, Watch3_2, Watch3_3, Watch3_4, Watch4_1, Watch4_2, Watch4_3,
            Watch4_4, Watch5_1, Watch5_2, Watch5_3, Watch5_4, Watch5_9, Watch5_10, Watch5_11, Watch5_12, Watch6_1,
            Watch6_2, Watch6_3, Watch6_4, Watch6_6, Watch6_7, Watch6_8, Watch6_9, Watch6_10, Watch6_11, Watch6_12,
            Watch6_13, Watch6_14, Watch6_15, Watch6_16, Watch6_17, Watch6_18, iBridge2_1, iBridge2_3, iBridge2_4,
            iBridge2_5, iBridge2_6, iBridge2_7, iBridge2_8, iBridge2_10, iBridge2_12, iBridge2_14, iBridge2_15,
            iBridge2_16, iBridge2_19, iBridge2_20, iBridge2_21, iBridge2_22, AudioAccessory1_1, AudioAccessory1_2,
            AudioAccessory5_1, iPad1_1, iPad2_1, iPad2_2, iPad2_3, iPad2_4, iPad2_5, iPad2_6, iPad2_7, iPad3_1, iPad3_2,
            iPad3_3, iPad3_4, iPad3_5, iPad3_6, iPad4_1, iPad4_2, iPad4_3, iPad4_4, iPad4_5, iPad4_6, iPad4_7, iPad4_8,
            iPad4_9, iPad5_1, iPad5_2, iPad5_3, iPad5_4, iPad6_3, iPad6_4, iPad6_7, iPad6_8, iPad6_11, iPad6_12,
            iPad7_1, iPad7_2, iPad7_3, iPad7_4, iPad7_5, iPad7_6, iPad7_11, iPad7_12, iPad8_1, iPad8_2, iPad8_3,
            iPad8_4, iPad8_5, iPad8_6, iPad8_7, iPad8_8, iPad8_9, iPad8_10, iPad8_11, iPad8_12, iPad11_1, iPad11_2,
            iPad11_3, iPad11_4, iPad11_6, iPad11_7, iPad12_1, iPad12_2, iPad13_1, iPad13_2, iPad13_4, iPad13_5,
            iPad13_6, iPad13_7, iPad13_8, iPad13_9, iPad13_10, iPad13_11, iPad13_16, iPad13_17, iPad14_1, iPad14_2,
            iPhone1_1, iPhone1_2, iPhone2_1, iPhone3_1, iPhone3_2, iPhone3_3, iPhone4_1, iPhone5_1, iPhone5_2,
            iPhone5_3, iPhone5_4, iPhone6_1, iPhone6_2, iPhone7_1, iPhone7_2, iPhone8_1, iPhone8_2, iPhone8_4,
            iPhone9_1, iPhone9_2, iPhone9_3, iPhone9_4, iPhone10_1, iPhone10_2, iPhone10_3, iPhone10_4, iPhone10_5,
            iPhone10_6, iPhone11_2, iPhone11_4, iPhone11_6, iPhone11_8, iPhone12_1, iPhone12_3, iPhone12_5, iPhone12_8,
            iPhone13_1, iPhone13_2, iPhone13_3, iPhone13_4, iPhone14_2, iPhone14_3, iPhone14_4, iPhone14_5, iPhone14_6,
            iPhone14_7, iPhone14_8, iPhone15_2, iPhone15_3, iPod1_1, iPod2_1, iPod3_1, iPod4_1, iPod5_1, iPod7_1,
            iPod9_1,
        });

    public static readonly ReadOnlyDictionary<DeviceGroup, ReadOnlyCollection<Device>> MappingGroupToDevices = new(
        new SortedDictionary<DeviceGroup, ReadOnlyCollection<Device>>
        {
            {
                AppleTV, new(
                    new[]
                    {
                        AppleTV2_1, // 2nd gen
                        AppleTV3_1, // 3rd gen
                        AppleTV3_2,
                        AppleTV5_3, // "HD"
                        AppleTV6_2, // "4K"
                        AppleTV11_1, // "4K" 2nd gen
                    })
            },
            {
                AppleWatch, new(
                    new[]
                    {
                        Watch1_1, // 1st gen
                        Watch1_2,
                        Watch2_3, // series 2
                        Watch2_4,
                        Watch2_6, // series 1
                        Watch2_7,
                        Watch3_1, // series 3
                        Watch3_2,
                        Watch3_3,
                        Watch3_4,
                        Watch4_1, // series 4
                        Watch4_2,
                        Watch4_3,
                        Watch4_4,
                        Watch5_1, // series 5
                        Watch5_2,
                        Watch5_3,
                        Watch5_4,
                        Watch5_9, // SE
                        Watch5_10,
                        Watch5_11,
                        Watch5_12,
                        Watch6_1, // series 6
                        Watch6_2,
                        Watch6_3,
                        Watch6_4,
                        Watch6_6, // series 7
                        Watch6_7,
                        Watch6_8,
                        Watch6_9,
                    })
            },
            {
                IBridge, new(
                    new[]
                    {
                        iBridge2_1,
                        iBridge2_3,
                        iBridge2_4,
                        iBridge2_5,
                        iBridge2_6,
                        iBridge2_7,
                        iBridge2_8,
                        iBridge2_10,
                        iBridge2_12,
                        iBridge2_14,
                        iBridge2_15,
                        iBridge2_16,
                        iBridge2_19,
                        iBridge2_20,
                        iBridge2_21,
                        iBridge2_22,
                    })
            },
            {
                AudioAccessory, new(
                    new[]
                    {
                        AudioAccessory1_1,
                        AudioAccessory1_2,
                        AudioAccessory5_1,
                    })
            },
            {
                IPad, new(
                    new[]
                    {
                        iPad1_1, // (1st gen)
                        iPad2_1, // 2nd gen
                        iPad2_2,
                        iPad2_3,
                        iPad2_4,
                        iPad3_1, // 3rd gen
                        iPad3_2,
                        iPad3_3,
                        iPad3_4, // 4th gen
                        iPad3_5,
                        iPad3_6,
                        iPad6_11, // 5th gen
                        iPad6_12,
                        iPad7_5, // 6th gen
                        iPad7_6,
                        iPad7_11, // 7th gen
                        iPad7_12,
                        iPad11_6, // 8th gen
                        iPad11_7,
                        iPad12_1, // 9th gen
                        iPad12_2,
                    })
            },
            {
                IPadAir, new(
                    new[]
                    {
                        iPad4_1, // (1st gen)
                        iPad4_2,
                        iPad4_3,
                        iPad5_3, // Air 2
                        iPad5_4,
                        iPad11_3, // 3rd gen
                        iPad11_4,
                        iPad13_1, // 4th gen
                        iPad13_2,
                        iPad13_16, // 5th gen
                        iPad13_17,
                    })
            },
            {
                IPadMini, new(
                    new[]
                    {
                        iPad2_5, // (1st gen)
                        iPad2_6,
                        iPad2_7,
                        iPad4_4, // mini 2
                        iPad4_5,
                        iPad4_6,
                        iPad4_7, // mini 3
                        iPad4_8,
                        iPad4_9,
                        iPad5_1, // mini 4
                        iPad5_2,
                        iPad11_1, // mini 5th gen
                        iPad11_2,
                        iPad14_1, // mini 6th gen
                        iPad14_2,
                    })
            },
            {
                IPadPro, new(
                    new[]
                    {
                        iPad6_3, // (1st gen) 9.7"
                        iPad6_4,
                        iPad6_7, // (1st gen) 12.9"
                        iPad6_8,
                        iPad7_1, // 2nd gen 12.9"
                        iPad7_2,
                        iPad7_3, // (2nd gen) 10.5"
                        iPad7_4,
                        iPad8_1, // (3rd gen) 11"
                        iPad8_2,
                        iPad8_3,
                        iPad8_4,
                        iPad8_5, // 3rd gen 12.9" // 4th?
                        iPad8_6,
                        iPad8_7,
                        iPad8_8,
                        iPad8_9, // 2nd (4th*) gen 11"
                        iPad8_10,
                        iPad8_11,
                        iPad8_12,
                        iPad13_4, // 3rd (5th*) gen 11"
                        iPad13_5,
                        iPad13_6,
                        iPad13_7,
                        iPad13_8, // 5th gen 12.9"
                        iPad13_9,
                        iPad13_10,
                        iPad13_11,
                    })
            },
            {
                IPhone, new(
                    new[]
                    {
                        iPhone1_1, // (1st gen)
                        iPhone1_2, // 3G
                        iPhone2_1, // 3GS
                        iPhone3_1, // 4
                        iPhone3_2,
                        iPhone3_3,
                        iPhone4_1, // 4S
                        iPhone5_1, // 5
                        iPhone5_2,
                        iPhone5_3, // 5c
                        iPhone5_4,
                        iPhone6_1, // 5s
                        iPhone6_2,
                        iPhone7_1, // 6+
                        iPhone7_2, // 6
                        iPhone8_1, // 6s
                        iPhone8_2, // 6s+
                        iPhone8_4, // SE
                        iPhone9_1, // 7
                        iPhone9_2, // 7+
                        iPhone9_3, // 7
                        iPhone9_4, // 7+
                        iPhone10_1, // 8
                        iPhone10_2, // 8+
                        iPhone10_3, // X
                        iPhone10_4, // 8
                        iPhone10_5, // 8+
                        iPhone10_6, // X
                        iPhone11_2, // XS
                        iPhone11_4, // XS Max
                        iPhone11_6,
                        iPhone11_8, // XR
                        iPhone12_1, // 11
                        iPhone12_3, // 11 Pro
                        iPhone12_5, // 11 Pro Max
                        iPhone12_8, // SE 2nd gen
                        iPhone13_1, // 12 mini
                        iPhone13_2, // 12
                        iPhone13_3, // 12 Pro
                        iPhone13_4, // 12 Pro Max
                        iPhone14_2, // 13 Pro
                        iPhone14_3, // 13 Pro Max
                        iPhone14_4, // 13 mini
                        iPhone14_5, // 13
                        iPhone14_6, // SE 3rd gen
                    })
            },
            {
                IPodTouch, new(
                    new[]
                    {
                        iPod1_1, // (1st gen)
                        iPod2_1, // 2nd gen
                        iPod3_1, // 3rd gen
                        iPod4_1, // 4th gen
                        iPod5_1, // 5th gen
                        iPod7_1, // 6th gen
                        iPod9_1, // 7th gen
                    })
            },
        });

    private Device(DeviceGroup group, int major, int minor)
    {
        Group = group;
        MajorModelNumber = major;
        MinorModelNumber = minor;
        ModelString = $"{group.ModelNamePrefix()}{major},{minor}"; // eg. "iPhone1,1"
    }

    public DeviceGroup Group { get; }
    public int MajorModelNumber { get; }
    public int MinorModelNumber { get; }
    public string ModelString { get; }

    public override bool Equals(object? obj) =>
        obj switch
        {
            string str => Equals(str),
            Device device => Equals(device),
            _ => false,
        };

    public override int GetHashCode() =>
        HashCode.Combine((int)Group, MajorModelNumber, MinorModelNumber);

    public override string ToString() => ModelString;

    public int CompareTo(object? obj) =>
        obj is Device device
            ? CompareTo(device)
            : throw new ArgumentException($"Object is not a {nameof(Device)}.", nameof(obj));

    public int CompareTo(Device? other)
    {
        int res = Group.CompareTo(other?.Group);
        if (res is not 0)
            return res;
        res = MajorModelNumber.CompareTo(other?.MajorModelNumber);
        return res is not 0 ? res : MinorModelNumber.CompareTo(other?.MinorModelNumber);
    }

    public bool Equals(Device? other) =>
        Group == other?.Group && MajorModelNumber == other.MajorModelNumber && MinorModelNumber == other.MinorModelNumber;

    public bool Equals(string? other) =>
        ModelString == other;

    public static bool TryParse(string str, [NotNullWhen(true)] out Device? device)
    {
        device = str switch
        {
            "AppleTV2,1" => AppleTV2_1,
            "AppleTV3,1" => AppleTV3_1,
            "AppleTV3,2" => AppleTV3_2,
            "AppleTV5,3" => AppleTV5_3,
            "AppleTV6,2" => AppleTV6_2,
            "AppleTV11,1" => AppleTV11_1,
            "Watch1,1" => Watch1_1,
            "Watch1,2" => Watch1_2,
            "Watch2,3" => Watch2_3,
            "Watch2,4" => Watch2_4,
            "Watch2,6" => Watch2_6,
            "Watch2,7" => Watch2_7,
            "Watch3,1" => Watch3_1,
            "Watch3,2" => Watch3_2,
            "Watch3,3" => Watch3_3,
            "Watch3,4" => Watch3_4,
            "Watch4,1" => Watch4_1,
            "Watch4,2" => Watch4_2,
            "Watch4,3" => Watch4_3,
            "Watch4,4" => Watch4_4,
            "Watch5,1" => Watch5_1,
            "Watch5,2" => Watch5_2,
            "Watch5,3" => Watch5_3,
            "Watch5,4" => Watch5_4,
            "Watch5,9" => Watch5_9,
            "Watch5,10" => Watch5_10,
            "Watch5,11" => Watch5_11,
            "Watch5,12" => Watch5_12,
            "Watch6,1" => Watch6_1,
            "Watch6,2" => Watch6_2,
            "Watch6,3" => Watch6_3,
            "Watch6,4" => Watch6_4,
            "Watch6,6" => Watch6_6,
            "Watch6,7" => Watch6_7,
            "Watch6,8" => Watch6_8,
            "Watch6,9" => Watch6_9,
            "Watch6,10" => Watch6_10,
            "Watch6,11" => Watch6_11,
            "Watch6,12" => Watch6_12,
            "Watch6,13" => Watch6_13,
            "Watch6,14" => Watch6_14,
            "Watch6,15" => Watch6_15,
            "Watch6,16" => Watch6_16,
            "Watch6,17" => Watch6_17,
            "Watch6,18" => Watch6_18,
            "iBridge2,1" => iBridge2_1,
            "iBridge2,3" => iBridge2_3,
            "iBridge2,4" => iBridge2_4,
            "iBridge2,5" => iBridge2_5,
            "iBridge2,6" => iBridge2_6,
            "iBridge2,7" => iBridge2_7,
            "iBridge2,8" => iBridge2_8,
            "iBridge2,10" => iBridge2_10,
            "iBridge2,12" => iBridge2_12,
            "iBridge2,14" => iBridge2_14,
            "iBridge2,15" => iBridge2_15,
            "iBridge2,16" => iBridge2_16,
            "iBridge2,19" => iBridge2_19,
            "iBridge2,20" => iBridge2_20,
            "iBridge2,21" => iBridge2_21,
            "iBridge2,22" => iBridge2_22,
            "AudioAccessory1,1" => AudioAccessory1_1,
            "AudioAccessory1,2" => AudioAccessory1_2,
            "AudioAccessory5,1" => AudioAccessory5_1,
            "iPad1,1" => iPad1_1,
            "iPad2,1" => iPad2_1,
            "iPad2,2" => iPad2_2,
            "iPad2,3" => iPad2_3,
            "iPad2,4" => iPad2_4,
            "iPad2,5" => iPad2_5,
            "iPad2,6" => iPad2_6,
            "iPad2,7" => iPad2_7,
            "iPad3,1" => iPad3_1,
            "iPad3,2" => iPad3_2,
            "iPad3,3" => iPad3_3,
            "iPad3,4" => iPad3_4,
            "iPad3,5" => iPad3_5,
            "iPad3,6" => iPad3_6,
            "iPad4,1" => iPad4_1,
            "iPad4,2" => iPad4_2,
            "iPad4,3" => iPad4_3,
            "iPad4,4" => iPad4_4,
            "iPad4,5" => iPad4_5,
            "iPad4,6" => iPad4_6,
            "iPad4,7" => iPad4_7,
            "iPad4,8" => iPad4_8,
            "iPad4,9" => iPad4_9,
            "iPad5,1" => iPad5_1,
            "iPad5,2" => iPad5_2,
            "iPad5,3" => iPad5_3,
            "iPad5,4" => iPad5_4,
            "iPad6,3" => iPad6_3,
            "iPad6,4" => iPad6_4,
            "iPad6,7" => iPad6_7,
            "iPad6,8" => iPad6_8,
            "iPad6,11" => iPad6_11,
            "iPad6,12" => iPad6_12,
            "iPad7,1" => iPad7_1,
            "iPad7,2" => iPad7_2,
            "iPad7,3" => iPad7_3,
            "iPad7,4" => iPad7_4,
            "iPad7,5" => iPad7_5,
            "iPad7,6" => iPad7_6,
            "iPad7,11" => iPad7_11,
            "iPad7,12" => iPad7_12,
            "iPad8,1" => iPad8_1,
            "iPad8,2" => iPad8_2,
            "iPad8,3" => iPad8_3,
            "iPad8,4" => iPad8_4,
            "iPad8,5" => iPad8_5,
            "iPad8,6" => iPad8_6,
            "iPad8,7" => iPad8_7,
            "iPad8,8" => iPad8_8,
            "iPad8,9" => iPad8_9,
            "iPad8,10" => iPad8_10,
            "iPad8,11" => iPad8_11,
            "iPad8,12" => iPad8_12,
            "iPad11,1" => iPad11_1,
            "iPad11,2" => iPad11_2,
            "iPad11,3" => iPad11_3,
            "iPad11,4" => iPad11_4,
            "iPad11,6" => iPad11_6,
            "iPad11,7" => iPad11_7,
            "iPad12,1" => iPad12_1,
            "iPad12,2" => iPad12_2,
            "iPad13,1" => iPad13_1,
            "iPad13,2" => iPad13_2,
            "iPad13,4" => iPad13_4,
            "iPad13,5" => iPad13_5,
            "iPad13,6" => iPad13_6,
            "iPad13,7" => iPad13_7,
            "iPad13,8" => iPad13_8,
            "iPad13,9" => iPad13_9,
            "iPad13,10" => iPad13_10,
            "iPad13,11" => iPad13_11,
            "iPad13,16" => iPad13_16,
            "iPad13,17" => iPad13_17,
            "iPad14,1" => iPad14_1,
            "iPad14,2" => iPad14_2,
            "iPhone1,1" => iPhone1_1,
            "iPhone1,2" => iPhone1_2,
            "iPhone2,1" => iPhone2_1,
            "iPhone3,1" => iPhone3_1,
            "iPhone3,2" => iPhone3_2,
            "iPhone3,3" => iPhone3_3,
            "iPhone4,1" => iPhone4_1,
            "iPhone5,1" => iPhone5_1,
            "iPhone5,2" => iPhone5_2,
            "iPhone5,3" => iPhone5_3,
            "iPhone5,4" => iPhone5_4,
            "iPhone6,1" => iPhone6_1,
            "iPhone6,2" => iPhone6_2,
            "iPhone7,1" => iPhone7_1,
            "iPhone7,2" => iPhone7_2,
            "iPhone8,1" => iPhone8_1,
            "iPhone8,2" => iPhone8_2,
            "iPhone8,4" => iPhone8_4,
            "iPhone9,1" => iPhone9_1,
            "iPhone9,2" => iPhone9_2,
            "iPhone9,3" => iPhone9_3,
            "iPhone9,4" => iPhone9_4,
            "iPhone10,1" => iPhone10_1,
            "iPhone10,2" => iPhone10_2,
            "iPhone10,3" => iPhone10_3,
            "iPhone10,4" => iPhone10_4,
            "iPhone10,5" => iPhone10_5,
            "iPhone10,6" => iPhone10_6,
            "iPhone11,2" => iPhone11_2,
            "iPhone11,4" => iPhone11_4,
            "iPhone11,6" => iPhone11_6,
            "iPhone11,8" => iPhone11_8,
            "iPhone12,1" => iPhone12_1,
            "iPhone12,3" => iPhone12_3,
            "iPhone12,5" => iPhone12_5,
            "iPhone12,8" => iPhone12_8,
            "iPhone13,1" => iPhone13_1,
            "iPhone13,2" => iPhone13_2,
            "iPhone13,3" => iPhone13_3,
            "iPhone13,4" => iPhone13_4,
            "iPhone14,2" => iPhone14_2,
            "iPhone14,3" => iPhone14_3,
            "iPhone14,4" => iPhone14_4,
            "iPhone14,5" => iPhone14_5,
            "iPhone14,6" => iPhone14_6,
            "iPhone14,7" => iPhone14_7,
            "iPhone14,8" => iPhone14_8,
            "iPhone15,2" => iPhone15_2,
            "iPhone15,3" => iPhone15_3,
            "iPod1,1" => iPod1_1,
            "iPod2,1" => iPod2_1,
            "iPod3,1" => iPod3_1,
            "iPod4,1" => iPod4_1,
            "iPod5,1" => iPod5_1,
            "iPod7,1" => iPod7_1,
            "iPod9,1" => iPod9_1,
            _ => null,
        };
        return device is not null;
    }

    public static Device Parse(string str) =>
        TryParse(str, out Device? device)
            ? device
            : throw new ArgumentException($"Unknown model ID '{str}'.", nameof(str));
}
