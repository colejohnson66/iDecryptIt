using JetBrains.Annotations;
using System;

namespace iDecryptIt.Shared;

[PublicAPI]
public enum DeviceID
{
    AppleTV2_1, // 2nd gen
    AppleTV3_1, // 3rd gen
    AppleTV3_2,
    AppleTV5_3, // "HD"
    AppleTV6_2, // "4K"
    AppleTV11_1, // "4K" 2nd gen

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

    AudioAccessory1_1, // HomePod
    AudioAccessory1_2,
    AudioAccessory5_1, // HomePod mini

    iPad1_1, // (1st gen)
    iPad2_1, // 2nd gen
    iPad2_2,
    iPad2_3,
    iPad2_4,
    iPad2_5, // mini (1st gen)
    iPad2_6,
    iPad2_7,
    iPad3_1, // 3rd gen
    iPad3_2,
    iPad3_3,
    iPad3_4, // 4th gen
    iPad3_5,
    iPad3_6,
    iPad4_1, // Air (1st gen)
    iPad4_2,
    iPad4_3,
    iPad4_4, // mini 2
    iPad4_5,
    iPad4_6,
    iPad4_7, // mini 3
    iPad4_8,
    iPad4_9,
    iPad5_1, // mini 4
    iPad5_2,
    iPad5_3, // Air 2
    iPad5_4,
    iPad6_3, // Pro (1st gen) 9.7"
    iPad6_4,
    iPad6_7, // Pro (1st gen) 12.9"
    iPad6_8,
    iPad6_11, // 5th gen
    iPad6_12,
    iPad7_1, // Pro 2nd gen 12.9"
    iPad7_2,
    iPad7_3, // Pro (2nd gen) 10.5"
    iPad7_4,
    iPad7_5, // 6th gen
    iPad7_6,
    iPad7_11, // 7th gen
    iPad7_12,
    iPad8_1, // Pro (3rd gen) 11"
    iPad8_2,
    iPad8_3,
    iPad8_4,
    iPad8_5, // Pro 3rd gen 12.9" // 4th?
    iPad8_6,
    iPad8_7,
    iPad8_8,
    iPad8_9, // Pro 2nd (4th*) gen 11"
    iPad8_10,
    iPad8_11,
    iPad8_12,
    iPad11_1, // mini 5th gen
    iPad11_2,
    iPad11_3, // Air 3rd gen
    iPad11_4,
    iPad11_6, // 8th gen
    iPad11_7,
    iPad12_1, // 9th gen
    iPad12_2,
    iPad13_1, // Air 4th gen
    iPad13_2,
    iPad13_4, // Pro 3rd (5th*) gen 11"
    iPad13_5,
    iPad13_6,
    iPad13_7,
    iPad13_8, // Pro 5th gen 12.9"
    iPad13_9,
    iPad13_10,
    iPad13_11,
    iPad14_1, // mini 6th gen
    iPad14_2,

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

    iPod1_1, // (1st gen)
    iPod2_1, // 2nd gen
    iPod3_1, // 3rd gen
    iPod4_1, // 4th gen
    iPod5_1, // 5th gen
    iPod7_1, // 6th gen
    iPod9_1, // 7th gen
}

public static class DeviceIDExtensions
{
    public static string ToStringID(this DeviceID id)
    {
        return id switch
        {
            DeviceID.AppleTV2_1 => "AppleTV2,1",
            DeviceID.AppleTV3_1 => "AppleTV3,1",
            DeviceID.AppleTV3_2 => "AppleTV3,2",
            DeviceID.AppleTV5_3 => "AppleTV5,3",
            DeviceID.AppleTV6_2 => "AppleTV6,2",
            DeviceID.AppleTV11_1 => "AppleTV11,1",

            DeviceID.Watch1_1 => "Watch1,1",
            DeviceID.Watch1_2 => "Watch1,2",
            DeviceID.Watch2_3 => "Watch2,3",
            DeviceID.Watch2_4 => "Watch2,4",
            DeviceID.Watch2_6 => "Watch2,6",
            DeviceID.Watch2_7 => "Watch2,7",
            DeviceID.Watch3_1 => "Watch3,1",
            DeviceID.Watch3_2 => "Watch3,2",
            DeviceID.Watch3_3 => "Watch3,3",
            DeviceID.Watch3_4 => "Watch3,4",
            DeviceID.Watch4_1 => "Watch4,1",
            DeviceID.Watch4_2 => "Watch4,2",
            DeviceID.Watch4_3 => "Watch4,3",
            DeviceID.Watch4_4 => "Watch4,4",
            DeviceID.Watch5_1 => "Watch5,1",
            DeviceID.Watch5_2 => "Watch5,2",
            DeviceID.Watch5_3 => "Watch5,3",
            DeviceID.Watch5_4 => "Watch5,4",
            DeviceID.Watch5_9 => "Watch5,9",
            DeviceID.Watch5_10 => "Watch5,10",
            DeviceID.Watch5_11 => "Watch5,11",
            DeviceID.Watch5_12 => "Watch5,12",
            DeviceID.Watch6_1 => "Watch6,1",
            DeviceID.Watch6_2 => "Watch6,2",
            DeviceID.Watch6_3 => "Watch6,3",
            DeviceID.Watch6_4 => "Watch6,4",
            DeviceID.Watch6_6 => "Watch6,6",
            DeviceID.Watch6_7 => "Watch6,7",
            DeviceID.Watch6_8 => "Watch6,8",
            DeviceID.Watch6_9 => "Watch6,9",

            DeviceID.iBridge2_1 => "iBridge2,1",
            DeviceID.iBridge2_3 => "iBridge2,3",
            DeviceID.iBridge2_4 => "iBridge2,4",
            DeviceID.iBridge2_5 => "iBridge2,5",
            DeviceID.iBridge2_6 => "iBridge2,6",
            DeviceID.iBridge2_7 => "iBridge2,7",
            DeviceID.iBridge2_8 => "iBridge2,8",
            DeviceID.iBridge2_10 => "iBridge2,10",
            DeviceID.iBridge2_12 => "iBridge2,12",
            DeviceID.iBridge2_14 => "iBridge2,14",
            DeviceID.iBridge2_15 => "iBridge2,15",
            DeviceID.iBridge2_16 => "iBridge2,16",
            DeviceID.iBridge2_19 => "iBridge2,19",
            DeviceID.iBridge2_20 => "iBridge2,20",
            DeviceID.iBridge2_21 => "iBridge2,21",
            DeviceID.iBridge2_22 => "iBridge2,22",

            DeviceID.AudioAccessory1_1 => "AudioAccessory1,1",
            DeviceID.AudioAccessory1_2 => "AudioAccessory1,2",
            DeviceID.AudioAccessory5_1 => "AudioAccessory5,1",

            DeviceID.iPad1_1 => "iPad1,1",
            DeviceID.iPad2_1 => "iPad2,1",
            DeviceID.iPad2_2 => "iPad2,2",
            DeviceID.iPad2_3 => "iPad2,3",
            DeviceID.iPad2_4 => "iPad2,4",
            DeviceID.iPad2_5 => "iPad2,5",
            DeviceID.iPad2_6 => "iPad2,6",
            DeviceID.iPad2_7 => "iPad2,7",
            DeviceID.iPad3_1 => "iPad3,1",
            DeviceID.iPad3_2 => "iPad3,2",
            DeviceID.iPad3_3 => "iPad3,3",
            DeviceID.iPad3_4 => "iPad3,4",
            DeviceID.iPad3_5 => "iPad3,5",
            DeviceID.iPad3_6 => "iPad3,6",
            DeviceID.iPad4_1 => "iPad4,1",
            DeviceID.iPad4_2 => "iPad4,2",
            DeviceID.iPad4_3 => "iPad4,3",
            DeviceID.iPad4_4 => "iPad4,4",
            DeviceID.iPad4_5 => "iPad4,5",
            DeviceID.iPad4_6 => "iPad4,6",
            DeviceID.iPad4_7 => "iPad4,7",
            DeviceID.iPad4_8 => "iPad4,8",
            DeviceID.iPad4_9 => "iPad4,9",
            DeviceID.iPad5_1 => "iPad5,1",
            DeviceID.iPad5_2 => "iPad5,2",
            DeviceID.iPad5_3 => "iPad5,3",
            DeviceID.iPad5_4 => "iPad5,4",
            DeviceID.iPad6_3 => "iPad6,3",
            DeviceID.iPad6_4 => "iPad6,4",
            DeviceID.iPad6_7 => "iPad6,7",
            DeviceID.iPad6_8 => "iPad6,8",
            DeviceID.iPad6_11 => "iPad6,11",
            DeviceID.iPad6_12 => "iPad6,12",
            DeviceID.iPad7_1 => "iPad7,1",
            DeviceID.iPad7_2 => "iPad7,2",
            DeviceID.iPad7_3 => "iPad7,3",
            DeviceID.iPad7_4 => "iPad7,4",
            DeviceID.iPad7_5 => "iPad7,5",
            DeviceID.iPad7_6 => "iPad7,6",
            DeviceID.iPad7_11 => "iPad7,11",
            DeviceID.iPad7_12 => "iPad7,12",
            DeviceID.iPad8_1 => "iPad8,1",
            DeviceID.iPad8_2 => "iPad8,2",
            DeviceID.iPad8_3 => "iPad8,3",
            DeviceID.iPad8_4 => "iPad8,4",
            DeviceID.iPad8_5 => "iPad8,5",
            DeviceID.iPad8_6 => "iPad8,6",
            DeviceID.iPad8_7 => "iPad8,7",
            DeviceID.iPad8_8 => "iPad8,8",
            DeviceID.iPad8_9 => "iPad8,9",
            DeviceID.iPad8_10 => "iPad8,10",
            DeviceID.iPad8_11 => "iPad8,11",
            DeviceID.iPad8_12 => "iPad8,12",
            DeviceID.iPad11_1 => "iPad11,1",
            DeviceID.iPad11_2 => "iPad11,2",
            DeviceID.iPad11_3 => "iPad11,3",
            DeviceID.iPad11_4 => "iPad11,4",
            DeviceID.iPad11_6 => "iPad11,6",
            DeviceID.iPad11_7 => "iPad11,7",
            DeviceID.iPad12_1 => "iPad12,1",
            DeviceID.iPad12_2 => "iPad12,2",
            DeviceID.iPad13_1 => "iPad13,1",
            DeviceID.iPad13_2 => "iPad13,2",
            DeviceID.iPad13_4 => "iPad13,4",
            DeviceID.iPad13_5 => "iPad13,5",
            DeviceID.iPad13_6 => "iPad13,6",
            DeviceID.iPad13_7 => "iPad13,7",
            DeviceID.iPad13_8 => "iPad13,8",
            DeviceID.iPad13_9 => "iPad13,9",
            DeviceID.iPad13_10 => "iPad13,10",
            DeviceID.iPad13_11 => "iPad13,11",
            DeviceID.iPad14_1 => "iPad14,1",
            DeviceID.iPad14_2 => "iPad14,2",

            DeviceID.iPhone1_1 => "iPhone1,1",
            DeviceID.iPhone1_2 => "iPhone1,2",
            DeviceID.iPhone2_1 => "iPhone2,1",
            DeviceID.iPhone3_1 => "iPhone3,1",
            DeviceID.iPhone3_2 => "iPhone3,2",
            DeviceID.iPhone3_3 => "iPhone3,3",
            DeviceID.iPhone4_1 => "iPhone4,1",
            DeviceID.iPhone5_1 => "iPhone5,1",
            DeviceID.iPhone5_2 => "iPhone5,2",
            DeviceID.iPhone5_3 => "iPhone5,3",
            DeviceID.iPhone5_4 => "iPhone5,4",
            DeviceID.iPhone6_1 => "iPhone6,1",
            DeviceID.iPhone6_2 => "iPhone6,2",
            DeviceID.iPhone7_1 => "iPhone7,1",
            DeviceID.iPhone7_2 => "iPhone7,2",
            DeviceID.iPhone8_1 => "iPhone8,1",
            DeviceID.iPhone8_2 => "iPhone8,2",
            DeviceID.iPhone8_4 => "iPhone8,4",
            DeviceID.iPhone9_1 => "iPhone9,1",
            DeviceID.iPhone9_2 => "iPhone9,2",
            DeviceID.iPhone9_3 => "iPhone9,3",
            DeviceID.iPhone9_4 => "iPhone9,4",
            DeviceID.iPhone10_1 => "iPhone10,1",
            DeviceID.iPhone10_2 => "iPhone10,2",
            DeviceID.iPhone10_3 => "iPhone10,3",
            DeviceID.iPhone10_4 => "iPhone10,4",
            DeviceID.iPhone10_5 => "iPhone10,5",
            DeviceID.iPhone10_6 => "iPhone10,6",
            DeviceID.iPhone11_2 => "iPhone11,2",
            DeviceID.iPhone11_4 => "iPhone11,4",
            DeviceID.iPhone11_6 => "iPhone11,6",
            DeviceID.iPhone11_8 => "iPhone11,8",
            DeviceID.iPhone12_1 => "iPhone12,1",
            DeviceID.iPhone12_3 => "iPhone12,3",
            DeviceID.iPhone12_5 => "iPhone12,5",
            DeviceID.iPhone12_8 => "iPhone12,8",
            DeviceID.iPhone13_1 => "iPhone13,1",
            DeviceID.iPhone13_2 => "iPhone13,2",
            DeviceID.iPhone13_3 => "iPhone13,3",
            DeviceID.iPhone13_4 => "iPhone13,4",
            DeviceID.iPhone14_2 => "iPhone14,2",
            DeviceID.iPhone14_3 => "iPhone14,3",
            DeviceID.iPhone14_4 => "iPhone14,4",
            DeviceID.iPhone14_5 => "iPhone14,5",

            DeviceID.iPod1_1 => "iPod1,1",
            DeviceID.iPod2_1 => "iPod2,1",
            DeviceID.iPod3_1 => "iPod3,1",
            DeviceID.iPod4_1 => "iPod4,1",
            DeviceID.iPod5_1 => "iPod5,1",
            DeviceID.iPod7_1 => "iPod7,1",
            DeviceID.iPod9_1 => "iPod9,1",
            _ => throw new ArgumentException($"Unknown {nameof(DeviceID)}: {id}", nameof(id)),
        };
    }
}

[PublicAPI]
public static class DeviceIDHelpers
{
    public static DeviceID FromStringID(string id)
    {
        return id switch
        {
            "AppleTV2,1" => DeviceID.AppleTV2_1,
            "AppleTV3,1" => DeviceID.AppleTV3_1,
            "AppleTV3,2" => DeviceID.AppleTV3_2,
            "AppleTV5,3" => DeviceID.AppleTV5_3,
            "AppleTV6,2" => DeviceID.AppleTV6_2,
            "AppleTV11,1" => DeviceID.AppleTV11_1,

            "Watch1,1" => DeviceID.Watch1_1,
            "Watch1,2" => DeviceID.Watch1_2,
            "Watch2,3" => DeviceID.Watch2_3,
            "Watch2,4" => DeviceID.Watch2_4,
            "Watch2,6" => DeviceID.Watch2_6,
            "Watch2,7" => DeviceID.Watch2_7,
            "Watch3,1" => DeviceID.Watch3_1,
            "Watch3,2" => DeviceID.Watch3_2,
            "Watch3,3" => DeviceID.Watch3_3,
            "Watch3,4" => DeviceID.Watch3_4,
            "Watch4,1" => DeviceID.Watch4_1,
            "Watch4,2" => DeviceID.Watch4_2,
            "Watch4,3" => DeviceID.Watch4_3,
            "Watch4,4" => DeviceID.Watch4_4,
            "Watch5,1" => DeviceID.Watch5_1,
            "Watch5,2" => DeviceID.Watch5_2,
            "Watch5,3" => DeviceID.Watch5_3,
            "Watch5,4" => DeviceID.Watch5_4,
            "Watch5,9" => DeviceID.Watch5_9,
            "Watch5,10" => DeviceID.Watch5_10,
            "Watch5,11" => DeviceID.Watch5_11,
            "Watch5,12" => DeviceID.Watch5_12,
            "Watch6,1" => DeviceID.Watch6_1,
            "Watch6,2" => DeviceID.Watch6_2,
            "Watch6,3" => DeviceID.Watch6_3,
            "Watch6,4" => DeviceID.Watch6_4,
            "Watch6,6" => DeviceID.Watch6_6,
            "Watch6,7" => DeviceID.Watch6_7,
            "Watch6,8" => DeviceID.Watch6_8,
            "Watch6,9" => DeviceID.Watch6_9,

            "iBridge2,1" => DeviceID.iBridge2_1,
            "iBridge2,3" => DeviceID.iBridge2_3,
            "iBridge2,4" => DeviceID.iBridge2_4,
            "iBridge2,5" => DeviceID.iBridge2_5,
            "iBridge2,6" => DeviceID.iBridge2_6,
            "iBridge2,7" => DeviceID.iBridge2_7,
            "iBridge2,8" => DeviceID.iBridge2_8,
            "iBridge2,10" => DeviceID.iBridge2_10,
            "iBridge2,12" => DeviceID.iBridge2_12,
            "iBridge2,14" => DeviceID.iBridge2_14,
            "iBridge2,15" => DeviceID.iBridge2_15,
            "iBridge2,16" => DeviceID.iBridge2_16,
            "iBridge2,19" => DeviceID.iBridge2_19,
            "iBridge2,20" => DeviceID.iBridge2_20,
            "iBridge2,21" => DeviceID.iBridge2_21,
            "iBridge2,22" => DeviceID.iBridge2_22,

            "AudioAccessory1,1" => DeviceID.AudioAccessory1_1,
            "AudioAccessory1,2" => DeviceID.AudioAccessory1_2,
            "AudioAccessory5,1" => DeviceID.AudioAccessory5_1,

            "iPad1,1" => DeviceID.iPad1_1,
            "iPad2,1" => DeviceID.iPad2_1,
            "iPad2,2" => DeviceID.iPad2_2,
            "iPad2,3" => DeviceID.iPad2_3,
            "iPad2,4" => DeviceID.iPad2_4,
            "iPad2,5" => DeviceID.iPad2_5,
            "iPad2,6" => DeviceID.iPad2_6,
            "iPad2,7" => DeviceID.iPad2_7,
            "iPad3,1" => DeviceID.iPad3_1,
            "iPad3,2" => DeviceID.iPad3_2,
            "iPad3,3" => DeviceID.iPad3_3,
            "iPad3,4" => DeviceID.iPad3_4,
            "iPad3,5" => DeviceID.iPad3_5,
            "iPad3,6" => DeviceID.iPad3_6,
            "iPad4,1" => DeviceID.iPad4_1,
            "iPad4,2" => DeviceID.iPad4_2,
            "iPad4,3" => DeviceID.iPad4_3,
            "iPad4,4" => DeviceID.iPad4_4,
            "iPad4,5" => DeviceID.iPad4_5,
            "iPad4,6" => DeviceID.iPad4_6,
            "iPad4,7" => DeviceID.iPad4_7,
            "iPad4,8" => DeviceID.iPad4_8,
            "iPad4,9" => DeviceID.iPad4_9,
            "iPad5,1" => DeviceID.iPad5_1,
            "iPad5,2" => DeviceID.iPad5_2,
            "iPad5,3" => DeviceID.iPad5_3,
            "iPad5,4" => DeviceID.iPad5_4,
            "iPad6,3" => DeviceID.iPad6_3,
            "iPad6,4" => DeviceID.iPad6_4,
            "iPad6,7" => DeviceID.iPad6_7,
            "iPad6,8" => DeviceID.iPad6_8,
            "iPad6,11" => DeviceID.iPad6_11,
            "iPad6,12" => DeviceID.iPad6_12,
            "iPad7,1" => DeviceID.iPad7_1,
            "iPad7,2" => DeviceID.iPad7_2,
            "iPad7,3" => DeviceID.iPad7_3,
            "iPad7,4" => DeviceID.iPad7_4,
            "iPad7,5" => DeviceID.iPad7_5,
            "iPad7,6" => DeviceID.iPad7_6,
            "iPad7,11" => DeviceID.iPad7_11,
            "iPad7,12" => DeviceID.iPad7_12,
            "iPad8,1" => DeviceID.iPad8_1,
            "iPad8,2" => DeviceID.iPad8_2,
            "iPad8,3" => DeviceID.iPad8_3,
            "iPad8,4" => DeviceID.iPad8_4,
            "iPad8,5" => DeviceID.iPad8_5,
            "iPad8,6" => DeviceID.iPad8_6,
            "iPad8,7" => DeviceID.iPad8_7,
            "iPad8,8" => DeviceID.iPad8_8,
            "iPad8,9" => DeviceID.iPad8_9,
            "iPad8,10" => DeviceID.iPad8_10,
            "iPad8,11" => DeviceID.iPad8_11,
            "iPad8,12" => DeviceID.iPad8_12,
            "iPad11,1" => DeviceID.iPad11_1,
            "iPad11,2" => DeviceID.iPad11_2,
            "iPad11,3" => DeviceID.iPad11_3,
            "iPad11,4" => DeviceID.iPad11_4,
            "iPad11,6" => DeviceID.iPad11_6,
            "iPad11,7" => DeviceID.iPad11_7,
            "iPad12,1" => DeviceID.iPad12_1,
            "iPad12,2" => DeviceID.iPad12_2,
            "iPad13,1" => DeviceID.iPad13_1,
            "iPad13,2" => DeviceID.iPad13_2,
            "iPad13,4" => DeviceID.iPad13_4,
            "iPad13,5" => DeviceID.iPad13_5,
            "iPad13,6" => DeviceID.iPad13_6,
            "iPad13,7" => DeviceID.iPad13_7,
            "iPad13,8" => DeviceID.iPad13_8,
            "iPad13,9" => DeviceID.iPad13_9,
            "iPad13,10" => DeviceID.iPad13_10,
            "iPad13,11" => DeviceID.iPad13_11,
            "iPad14,1" => DeviceID.iPad14_1,
            "iPad14,2" => DeviceID.iPad14_2,
            "iPhone1,1" => DeviceID.iPhone1_1,

            "iPhone1,2" => DeviceID.iPhone1_2,
            "iPhone2,1" => DeviceID.iPhone2_1,
            "iPhone3,1" => DeviceID.iPhone3_1,
            "iPhone3,2" => DeviceID.iPhone3_2,
            "iPhone3,3" => DeviceID.iPhone3_3,
            "iPhone4,1" => DeviceID.iPhone4_1,
            "iPhone5,1" => DeviceID.iPhone5_1,
            "iPhone5,2" => DeviceID.iPhone5_2,
            "iPhone5,3" => DeviceID.iPhone5_3,
            "iPhone5,4" => DeviceID.iPhone5_4,
            "iPhone6,1" => DeviceID.iPhone6_1,
            "iPhone6,2" => DeviceID.iPhone6_2,
            "iPhone7,1" => DeviceID.iPhone7_1,
            "iPhone7,2" => DeviceID.iPhone7_2,
            "iPhone8,1" => DeviceID.iPhone8_1,
            "iPhone8,2" => DeviceID.iPhone8_2,
            "iPhone8,4" => DeviceID.iPhone8_4,
            "iPhone9,1" => DeviceID.iPhone9_1,
            "iPhone9,2" => DeviceID.iPhone9_2,
            "iPhone9,3" => DeviceID.iPhone9_3,
            "iPhone9,4" => DeviceID.iPhone9_4,
            "iPhone10,1" => DeviceID.iPhone10_1,
            "iPhone10,2" => DeviceID.iPhone10_2,
            "iPhone10,3" => DeviceID.iPhone10_3,
            "iPhone10,4" => DeviceID.iPhone10_4,
            "iPhone10,5" => DeviceID.iPhone10_5,
            "iPhone10,6" => DeviceID.iPhone10_6,
            "iPhone11,2" => DeviceID.iPhone11_2,
            "iPhone11,4" => DeviceID.iPhone11_4,
            "iPhone11,6" => DeviceID.iPhone11_6,
            "iPhone11,8" => DeviceID.iPhone11_8,
            "iPhone12,1" => DeviceID.iPhone12_1,
            "iPhone12,3" => DeviceID.iPhone12_3,
            "iPhone12,5" => DeviceID.iPhone12_5,
            "iPhone12,8" => DeviceID.iPhone12_8,
            "iPhone13,1" => DeviceID.iPhone13_1,
            "iPhone13,2" => DeviceID.iPhone13_2,
            "iPhone13,3" => DeviceID.iPhone13_3,
            "iPhone13,4" => DeviceID.iPhone13_4,
            "iPhone14,2" => DeviceID.iPhone14_2,
            "iPhone14,3" => DeviceID.iPhone14_3,
            "iPhone14,4" => DeviceID.iPhone14_4,
            "iPhone14,5" => DeviceID.iPhone14_5,

            "iPod1,1" => DeviceID.iPod1_1,
            "iPod2,1" => DeviceID.iPod2_1,
            "iPod3,1" => DeviceID.iPod3_1,
            "iPod4,1" => DeviceID.iPod4_1,
            "iPod5,1" => DeviceID.iPod5_1,
            "iPod7,1" => DeviceID.iPod7_1,
            "iPod9,1" => DeviceID.iPod9_1,
            _ => throw new ArgumentException($"Unknown device model: {id}", nameof(id)),
        };
    }
}
