using System.Collections.Generic;
using System.Collections.ObjectModel;
using static iDecryptIt.Shared.DeviceID;
using static iDecryptIt.Shared.DeviceIDGroup;

namespace iDecryptIt.Shared;

public static class DeviceIDMappings
{
    public static readonly ReadOnlyDictionary<DeviceIDGroup, ReadOnlyCollection<DeviceID>> Groups = new(
        new SortedDictionary<DeviceIDGroup, ReadOnlyCollection<DeviceID>>
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

    public static readonly ReadOnlyDictionary<DeviceIDGroup, string> GroupName = new(
        new SortedDictionary<DeviceIDGroup, string>
        {
            { AppleTV, "Apple TV" },
            { AppleWatch, "Apple Watch" },
            { IBridge, "iBridge" },
            { AudioAccessory, "HomePod" },
            { IPad, "iPad" },
            { IPadAir, "iPad Air" },
            { IPadMini, "iPad mini" },
            { IPadPro, "iPad Pro" },
            { IPhone, "iPhone" },
            { IPodTouch, "iPod touch" },
        });

    public static readonly ReadOnlyDictionary<DeviceID, string> DeviceName = new(
        new SortedDictionary<DeviceID, string>
        {
            { AppleTV2_1, "Apple TV (2nd generation)" },
            { AppleTV3_1, "Apple TV (3rd generation)" },
            { AppleTV3_2, "Apple TV (3rd generation)" },
            { AppleTV5_3, "Apple TV HD" },
            { AppleTV6_2, "Apple TV 4K" },
            { AppleTV11_1, "Apple TV 4K (2nd generation)" },
            { Watch1_1, "Apple Watch 38mm" },
            { Watch1_2, "Apple Watch 42mm" },
            { Watch2_3, "Apple Watch Series 2 38mm" },
            { Watch2_4, "Apple Watch Series 2 42mm" },
            { Watch2_6, "Apple Watch Series 1 38mm" },
            { Watch2_7, "Apple Watch Series 1 42mm" },
            { Watch3_1, "Apple Watch Series 3 38mm" },
            { Watch3_2, "Apple Watch Series 3 42mm" },
            { Watch3_3, "Apple Watch Series 3 38mm" },
            { Watch3_4, "Apple Watch Series 3 42mm" },
            { Watch4_1, "Apple Watch Series 4 38mm" },
            { Watch4_2, "Apple Watch Series 4 42mm" },
            { Watch4_3, "Apple Watch Series 4 38mm" },
            { Watch4_4, "Apple Watch Series 4 42mm" },
            { Watch5_1, "Apple Watch Series 5 40mm" },
            { Watch5_2, "Apple Watch Series 5 44mm" },
            { Watch5_3, "Apple Watch Series 5 40mm" },
            { Watch5_4, "Apple Watch Series 5 44mm" },
            { Watch5_9, "Apple Watch SE 40mm" },
            { Watch5_10, "Apple Watch SE 44mm" },
            { Watch5_11, "Apple Watch SE 40mm" },
            { Watch5_12, "Apple Watch SE 44mm" },
            { Watch6_1, "Apple Watch Series 6 40mm" },
            { Watch6_2, "Apple Watch Series 6 44mm" },
            { Watch6_3, "Apple Watch Series 6 40mm" },
            { Watch6_4, "Apple Watch Series 6 44mm" },
            { Watch6_6, "Apple Watch Series 7 41mm" },
            { Watch6_7, "Apple Watch Series 7 45mm" },
            { Watch6_8, "Apple Watch Series 7 41mm" },
            { Watch6_9, "Apple Watch Series 7 45mm" },
            { iBridge2_1, "" },
            { iBridge2_3, "" },
            { iBridge2_4, "" },
            { iBridge2_5, "" },
            { iBridge2_6, "" },
            { iBridge2_7, "" },
            { iBridge2_8, "" },
            { iBridge2_10, "" },
            { iBridge2_12, "" },
            { iBridge2_14, "" },
            { iBridge2_15, "" },
            { iBridge2_16, "" },
            { iBridge2_19, "" },
            { iBridge2_20, "" },
            { iBridge2_21, "" },
            { iBridge2_22, "" },
            { AudioAccessory1_1, "HomePod" },
            { AudioAccessory1_2, "HomePod" },
            { AudioAccessory5_1, "HomePod mini" },
            { iPad1_1, "iPad" },
            { iPad2_1, "iPad 2" },
            { iPad2_2, "iPad 2" },
            { iPad2_3, "iPad 2" },
            { iPad2_4, "iPad 2" },
            { iPad2_5, "iPad mini" },
            { iPad2_6, "iPad mini" },
            { iPad2_7, "iPad mini" },
            { iPad3_1, "iPad (3rd generation)" },
            { iPad3_2, "iPad (3rd generation)" },
            { iPad3_3, "iPad (3rd generation)" },
            { iPad3_4, "iPad (4th generation)" },
            { iPad3_5, "iPad (4th generation)" },
            { iPad3_6, "iPad (4th generation)" },
            { iPad4_1, "iPad Air" },
            { iPad4_2, "iPad Air" },
            { iPad4_3, "iPad Air" },
            { iPad4_4, "iPad mini 2" },
            { iPad4_5, "iPad mini 2" },
            { iPad4_6, "iPad mini 2" },
            { iPad4_7, "iPad mini 3" },
            { iPad4_8, "iPad mini 3" },
            { iPad4_9, "iPad mini 3" },
            { iPad5_1, "iPad mini 4" },
            { iPad5_2, "iPad mini 4" },
            { iPad5_3, "iPad Air 2" },
            { iPad5_4, "iPad Air 2" },
            { iPad6_3, "iPad Pro 9.7\"" },
            { iPad6_4, "iPad Pro 9.7\"" },
            { iPad6_7, "iPad Pro 12.9\"" },
            { iPad6_8, "iPad Pro 12.9\"" },
            { iPad6_11, "iPad (5th generation)" },
            { iPad6_12, "iPad (5th generation)" },
            { iPad7_1, "iPad Pro (2nd generation) 12.9\"" },
            { iPad7_2, "iPad Pro (2nd generation) 12.9\"" },
            { iPad7_3, "iPad Pro (2nd generation) 10.5\"" },
            { iPad7_4, "iPad Pro (2nd generation) 10.5\"" },
            { iPad7_5, "iPad (6th generation)" },
            { iPad7_6, "iPad (6th generation)" },
            { iPad7_11, "iPad (7th generation)" },
            { iPad7_12, "iPad (7th generation)" },
            { iPad8_1, "iPad Pro (3rd generation) 11\"" },
            { iPad8_2, "iPad Pro (3rd generation) 11\"" },
            { iPad8_3, "iPad Pro (3rd generation) 11\"" },
            { iPad8_4, "iPad Pro (3rd generation) 11\"" },
            { iPad8_5, "iPad Pro (3rd generation) 12.9\"" },
            { iPad8_6, "iPad Pro (3rd generation) 12.9\"" },
            { iPad8_7, "iPad Pro (3rd generation) 12.9\"" },
            { iPad8_8, "iPad Pro (3rd generation) 12.9\"" },
            { iPad8_9, "iPad Pro (4th generation) 11\"" },
            { iPad8_10, "iPad Pro (4th generation) 11\"" },
            { iPad8_11, "iPad Pro (4th generation) 12.9\"" },
            { iPad8_12, "iPad Pro (4th generation) 12.9\"" },
            { iPad11_1, "iPad mini (5th generation)" },
            { iPad11_2, "iPad mini (5th generation)" },
            { iPad11_3, "iPad Air (3rd generation)" },
            { iPad11_4, "iPad Air (3rd generation)" },
            { iPad11_6, "iPad (8th generation)" },
            { iPad11_7, "iPad (8th generation)" },
            { iPad12_1, "iPad (9th generation)" },
            { iPad12_2, "iPad (9th generation)" },
            { iPad13_1, "iPad Air (4th generation)" },
            { iPad13_2, "iPad Air (4th generation)" },
            { iPad13_4, "iPad Pro (5th generation) 11\"" },
            { iPad13_5, "iPad Pro (5th generation) 11\"" },
            { iPad13_6, "iPad Pro (5th generation) 11\"" },
            { iPad13_7, "iPad Pro (5th generation) 11\"" },
            { iPad13_8, "iPad Pro (5th generation) 12.9\"" },
            { iPad13_9, "iPad Pro (5th generation) 12.9\"" },
            { iPad13_10, "iPad Pro (5th generation) 12.9\"" },
            { iPad13_11, "iPad Pro (5th generation) 12.9\"" },
            { iPad14_1, "iPad mini (6th generation)" },
            { iPad14_2, "iPad mini (6th generation)" },
            { iPhone1_1, "iPhone" },
            { iPhone1_2, "iPhone 3G" },
            { iPhone2_1, "iPhone 3GS" },
            { iPhone3_1, "iPhone 4" },
            { iPhone3_2, "iPhone 4" },
            { iPhone3_3, "iPhone 4" },
            { iPhone4_1, "iPhone 4S" },
            { iPhone5_1, "iPhone 5" },
            { iPhone5_2, "iPhone 5" },
            { iPhone5_3, "iPhone 5c" },
            { iPhone5_4, "iPhone 5c" },
            { iPhone6_1, "iPhone 5s" },
            { iPhone6_2, "iPhone 5s" },
            { iPhone7_1, "iPhone 6 Plus" },
            { iPhone7_2, "iPhone 6" },
            { iPhone8_1, "iPhone 6s" },
            { iPhone8_2, "iPhone 6s Plus" },
            { iPhone8_4, "iPhone SE" },
            { iPhone9_1, "iPhone 7" },
            { iPhone9_2, "iPhone 7 Plus" },
            { iPhone9_3, "iPhone 7" },
            { iPhone9_4, "iPhone 7 Plus" },
            { iPhone10_1, "iPhone 8" },
            { iPhone10_2, "iPhone 8 Plus" },
            { iPhone10_3, "iPhone X" },
            { iPhone10_4, "iPhone 8" },
            { iPhone10_5, "iPhone 8 Plus" },
            { iPhone10_6, "iPhone X" },
            { iPhone11_2, "iPhone XS" },
            { iPhone11_4, "iPhone XS Max" },
            { iPhone11_6, "iPhone XS Max" },
            { iPhone11_8, "iPhone XR" },
            { iPhone12_1, "iPhone 11" },
            { iPhone12_3, "iPhone 11 Pro" },
            { iPhone12_5, "iPhone 11 Pro Max" },
            { iPhone12_8, "iPhone SE (2nd generation)" },
            { iPhone13_1, "iPhone 12 mini" },
            { iPhone13_2, "iPhone 12" },
            { iPhone13_3, "iPhone 12 Pro" },
            { iPhone13_4, "iPhone 12 Pro Max" },
            { iPhone14_2, "iPhone 13 Pro" },
            { iPhone14_3, "iPhone 13 Pro Max" },
            { iPhone14_4, "iPhone 13 mini" },
            { iPhone14_5, "iPhone 13" },
            { iPod1_1, "iPod touch" },
            { iPod2_1, "iPod touch (2nd generation)" },
            { iPod3_1, "iPod touch (3rd generation)" },
            { iPod4_1, "iPod touch (4th generation)" },
            { iPod5_1, "iPod touch (5th generation)" },
            { iPod7_1, "iPod touch (6th generation)" },
            { iPod9_1, "iPod touch (7th generation)" },
        });
}
