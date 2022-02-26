namespace KeyGrabber;

public static class Descriptors
{
    public static readonly string[] DeviceIDs =
    {
        "AppleTV2,1", // 2nd gen
        "AppleTV3,1", // 3rd gen
        "AppleTV3,2",
        "AppleTV5,3", // "HD"
        "AppleTV6,2", // "4K"
        "AppleTV11,1", // "4K" 2nd gen
        "Watch1,1", // 1st gen
        "Watch1,2",
        "Watch2,3", // series 2
        "Watch2,4",
        "Watch2,6", // series 1
        "Watch2,7",
        "Watch3,1", // series 3
        "Watch3,2",
        "Watch3,3",
        "Watch3,4",
        "Watch4,1", // series 4
        "Watch4,2",
        "Watch4,3",
        "Watch4,4",
        "Watch5,1", // series 5
        "Watch5,2",
        "Watch5,3",
        "Watch5,4",
        "Watch5,9", // SE
        "Watch5,10",
        "Watch5,11",
        "Watch5,12",
        "Watch6,1", // series 6
        "Watch6,2",
        "Watch6,3",
        "Watch6,4",
        "Watch6,6", // series 7
        "Watch6,7",
        "Watch6,8",
        "Watch6,9",
        "iBridge",

        //
        "iBridge2,1",
        "iBridge2,3",
        "iBridge2,4",
        "iBridge2,5",
        "iBridge2,6",
        "iBridge2,7",
        "iBridge2,8",
        "iBridge2,10",
        "iBridge2,12",
        "iBridge2,14",
        "iBridge2,15",
        "iBridge2,16",
        "iBridge2,19",
        "iBridge2,20",
        "iBridge2,21",
        "iBridge2,22",

        //
        "AudioAccessory1,1", // HomePod
        "AudioAccessory1,2",
        "AudioAccessory5,1", // HomePod mini
        //
        "iPad1,1", // (1st gen)
        "iPad2,1", // 2nd gen
        "iPad2,2",
        "iPad2,3",
        "iPad2,4",
        "iPad2,5", // mini (1st gen)
        "iPad2,6",
        "iPad2,7",
        "iPad3,1", // 3rd gen
        "iPad3,2",
        "iPad3,3",
        "iPad3,4", // 4th gen
        "iPad3,5",
        "iPad3,6",
        "iPad4,1", // Air (1st gen)
        "iPad4,2",
        "iPad4,3",
        "iPad4,4", // mini 2
        "iPad4,5",
        "iPad4,6",
        "iPad4,7", // mini 3
        "iPad4,8",
        "iPad4,9",
        "iPad5,1", // mini 4
        "iPad5,2",
        "iPad5,3", // Air 2
        "iPad5,4",
        "iPad6,3", // Pro (1st gen) 9.7"
        "iPad6,4",
        "iPad6,7", // Pro (1st gen) 12.9"
        "iPad6,8",
        "iPad6,11", // 5th gen
        "iPad6,12",
        "iPad7,1", // Pro 2nd gen 12.9"
        "iPad7,2",
        "iPad7,3", // Pro (2nd gen) 10.5"
        "iPad7,4",
        "iPad7,5", // 6th gen
        "iPad7,6",
        "iPad7,11", // 7th gen
        "iPad7,12",
        "iPad8,1", // Pro (3rd gen) 11"
        "iPad8,2",
        "iPad8,3",
        "iPad8,4",
        "iPad8,5", // Pro 3rd gen 12.9" // 4th?
        "iPad8,6",
        "iPad8,7",
        "iPad8,8",
        "iPad8,9", // Pro 2nd (4th*) gen 11"
        "iPad8,10",
        "iPad8,11",
        "iPad8,12",
        "iPad11,1", // mini 5th gen
        "iPad11,2",
        "iPad11,3", // Air 3rd gen
        "iPad11,4",
        "iPad11,6", // 8th gen
        "iPad11,7",
        "iPad12,1", // 9th gen
        "iPad12,2",
        "iPad13,1", // Air 4th gen
        "iPad13,2",
        "iPad13,4", // Pro 3rd (5th*) gen 11"
        "iPad13,5",
        "iPad13,6",
        "iPad13,7",
        "iPad13,8", // Pro 5th gen 12.9"
        "iPad13,9",
        "iPad13,10",
        "iPad13,11",
        "iPad14,1", // mini 6th gen
        "iPad14,2",

        //
        "iPhone1,1", // (1st gen)
        "iPhone1,2", // 3G
        "iPhone2,1", // 3GS
        "iPhone3,1", // 4
        "iPhone3,2",
        "iPhone3,3",
        "iPhone4,1", // 4S
        "iPhone5,1", // 5
        "iPhone5,2",
        "iPhone5,3", // 5c
        "iPhone5,4",
        "iPhone6,1", // 5s
        "iPhone6,2",
        "iPhone7,1", // 6+
        "iPhone7,2", // 6
        "iPhone8,1", // 6s
        "iPhone8,2", // 6s+
        "iPhone8,4", // SE
        "iPhone9,1", // 7
        "iPhone9,2", // 7+
        "iPhone9,3", // 7
        "iPhone9,4", // 7+
        "iPhone10,1", // 8
        "iPhone10,2", // 8+
        "iPhone10,3", // X
        "iPhone10,4", // 8
        "iPhone10,5", // 8+
        "iPhone10,6", // X
        "iPhone11,2", // XS
        "iPhone11,4", // XS Max
        "iPhone11,6",
        "iPhone11,8", // XR
        "iPhone12,1", // 11
        "iPhone12,3", // 11 Pro
        "iPhone12,5", // 11 Pro Max
        "iPhone12,8", // SE 2nd gen
        "iPhone13,1", // 12 mini
        "iPhone13,2", // 12
        "iPhone13,3", // 12 Pro
        "iPhone13,4", // 12 Pro Max
        "iPhone14,2", // 13 Pro
        "iPhone14,3", // 13 Pro Max
        "iPhone14,4", // 13 mini
        "iPhone14,5", // 13
        //
        "iPod1,1", // (1st gen)
        "iPod2,1", // 2nd gen
        "iPod3,1", // 3rd gen
        "iPod4,1", // 4th gen
        "iPod5,1", // 5th gen
        "iPod7,1", // 6th gen
        "iPod9,1", // 7th gen
    };

    /* DSL Key:
     *   - v: Version column
     *   - b: Build column
     *   - k(IDs): Keys column (IDs separated by semicolons)
     *   - bb: Baseband column
     *   - r: Release date column
     *   - u: URL column
     *   - h: Hash column
     *   - s: File size column
     *   - d: Documentation (or release notes) column
     *   - i: Ignore this column (usually comments)
     *
     * Each column definition is separated by a space
     *
     * Each entry in the `string[]` array MUST be in the order the tables appear on the page
     */

    /// <summary><c>Firmware/Apple_TV/???</c></summary>
    public static readonly Dictionary<string, string[]> AppleTVFirmwarePages = new()
    {
        { "4.x", new[] { "v v b b k(AppleTV2,1) r u h s d" } },
        {
            "5.x", new[]
            {
                "v v b b k(AppleTV2,1) r u h s d",
                "v v b b k(AppleTV3,1) r u h s d",
                "v v b b k(AppleTV3,2) r u h s d",
            }
        },
        {
            "6.x", new[]
            {
                "v v b b k(AppleTV2,1) r u h s d",
                "v v b b k(AppleTV3,1) r u h s d",
                "v v b b k(AppleTV3,2) r u h s d",
            }
        },
        {
            "7.x", new[]
            {
                "v v b b k(AppleTV3,1) r u h s d",
                "v v b b k(AppleTV3,2) r u h s d",
            }
        },
        { "9.x", new[] { "v b k(AppleTV5,3) r u h s d" } },
        { "10.x", new[] { "v b k(AppleTV5,3) r u h s d" } },
        {
            "11.x", new[]
            {
                "v b k(AppleTV5,3) r u h s d",
                "v b k(AppleTV6,2) r d",
            }
        },
        {
            "12.x", new[]
            {
                "v b k(AppleTV5,3) r u h s d",
                "v b k(AppleTV6,2) r d",
            }
        },
        {
            "13.x", new[]
            {
                "v b k(AppleTV5,3) r u h s d d",
                "v b k(AppleTV6,2) r d d",
            }
        },
        {
            "14.x", new[]
            {
                "v b k(AppleTV5,3) r u h s d",
                "v b k(AppleTV6,2) r d",
                "v b k(AppleTV11,1) r d",
            }
        },
        {
            "15.x", new[]
            {
                "v b k(AppleTV5,3) r u h s d",
                "v b k(AppleTV6,2) r d",
                "v b k(AppleTV11,1) r d",
            }
        },
    };

    /// <summary><c>Firmware/Apple_Watch/???</c></summary>
    public static readonly Dictionary<string, string[]> AppleWatchFirmwarePages = new()
    {
        {
            "1.x", new[]
            {
                "v v b k(Watch1,1) r d",
                "v v b k(Watch1,2) r d",
            }
        },
        {
            // TODO: Watch1,1 2.1/9.0 (13S661) has:
            //   IPSW: http://appldnld.apple.com/watch/os2.1/031-29536-20151208-ca603a1a-8eec-11e5-9b1f-e0c48b8beceb/watch1,1_2.1_13s661_restore.ipsw
            //   Hash: e02ab4ace8051ee12996e5547c397be30666d594
            //   Size: 924,502,956
            // TODO: Watch1,2 2.1/9.0 (13S661) has:
            //   IPSW: http://appldnld.apple.com/watch/os2.1/031-29539-20151208-ca603a1a-8eec-11e5-9b1f-e1c48b8beceb/watch1,2_2.1_13s661_restore.ipsw
            //   Hash: 44b29ebb1552c930c9fc8921a86e29f9bb9fe00f
            //   Size: 926,842,087
            "2.x", new[]
            {
                "v v b k(Watch1,1) r d",
                "v v b k(Watch1,2) r d",
            }
        },
        {
            "3.x", new[]
            {
                "v v b k(Watch1,1) r d",
                "v v b k(Watch1,2) r d",
                "v v b k(Watch2,6) r d",
                "v v b k(Watch2,7) r d",
                "v v b k(Watch2,3) r d",
                "v v b k(Watch2,4) r d",
            }
        },
        {
            "4.x", new[]
            {
                "v v b k(Watch1,1) r d",
                "v v b k(Watch1,2) r d",
                "v v b k(Watch2,6) r d",
                "v v b k(Watch2,7) r d",
                "v v b k(Watch2,3) r d",
                "v v b k(Watch2,4) r d",
                "v v b k(Watch3,1) bb r d",
                "v v b k(Watch3,2) bb r d",
                "v v b k(Watch3,3) r d",
                "v v b k(Watch3,4) r d",
            }
        },
        {
            "5.x", new[]
            {
                "v v b k(Watch2,6) r d",
                "v v b k(Watch2,7) r d",
                "v v b k(Watch2,3) r d",
                "v v b k(Watch2,4) r d",
                "v v b k(Watch3,1) bb r d",
                "v v b k(Watch3,2) bb r d",
                "v v b k(Watch3,3) r d",
                "v v b k(Watch3,4) r d",
                "v v b k(Watch4,1) r d",
                "v v b k(Watch4,2) r d",
                "v v b k(Watch4,3) bb r d",
                "v v b k(Watch4,4) bb r d",
            }
        },
        {
            "6.x", new[]
            {
                "v v b k(Watch2,6) r u h s d",
                "v v b k(Watch2,7) r u h s d",
                "v v b k(Watch2,3) r u h s d",
                "v v b k(Watch2,4) r u h s d",
                "v v b k(Watch3,1) bb r d",
                "v v b k(Watch3,2) bb r d",
                "v v b k(Watch3,3) r d",
                "v v b k(Watch3,4) r d",
                "v v b k(Watch4,1) r d",
                "v v b k(Watch4,2) r d",
                "v v b k(Watch4,3) bb r d",
                "v v b k(Watch4,4) bb r d",
                "v v b k(Watch5,1) r d",
                "v v b k(Watch5,2) r d",
                "v v b k(Watch5,3) bb r d",
                "v v b k(Watch5,4) bb r d",
            }
        },
        {
            "7.x", new[]
            {
                "v v b k(Watch3,1) bb r u h s d",
                "v v b k(Watch3,2) bb r u h s d",
                "v v b k(Watch3,3) r u h s d",
                "v v b k(Watch3,4) r u h s d",
                "v v b k(Watch4,1) r u h s d",
                "v v b k(Watch4,2) r u h s d",
                "v v b k(Watch4,3) bb r u h s d",
                "v v b k(Watch4,4) bb r u h s d",
                "v v b k(Watch5,1) r u h s d",
                "v v b k(Watch5,2) r u h s d",
                "v v b k(Watch5,3) bb r u h s d",
                "v v b k(Watch5,4) bb r u h s d",
                "v v b k(Watch5,9) r u h s d",
                "v v b k(Watch5,10) r u h s d",
                "v v b k(Watch5,11) bb r u h s d",
                "v v b k(Watch5,12) bb r u h s d",
                "v v b k(Watch6,1) r u h s d",
                "v v b k(Watch6,2) r u h s d",
                "v v b k(Watch6,3) bb r u h s d",
                "v v b k(Watch6,4) bb r u h s d",
            }
        },
        {
            "8.x", new[]
            {
                "v v b k(Watch3,1) bb r u h s d",
                "v v b k(Watch3,2) bb r u h s d",
                "v v b k(Watch3,3) r u h s d",
                "v v b k(Watch3,4) r u h s d",
                "v v b k(Watch4,1) r u h s d",
                "v v b k(Watch4,2) r u h s d",
                "v v b k(Watch4,3) bb r u h s d",
                "v v b k(Watch4,4) bb r u h s d",
                "v v b k(Watch5,1) r u h s d",
                "v v b k(Watch5,2) r u h s d",
                "v v b k(Watch5,3) bb r u h s d",
                "v v b k(Watch5,4) bb r u h s d",
                "v v b k(Watch5,9) r u h s d",
                "v v b k(Watch5,10) r u h s d",
                "v v b k(Watch5,11) bb r u h s d",
                "v v b k(Watch5,12) bb r u h s d",
                "v v b k(Watch6,1) r u h s d",
                "v v b k(Watch6,2) r u h s d",
                "v v b k(Watch6,3) bb r u h s d",
                "v v b k(Watch6,4) bb r u h s d",
                "v v b k(Watch6,6) r u h s d",
                "v v b k(Watch6,7) r u h s d",
                "v v b k(Watch6,8) bb r u h s d",
                "v v b k(Watch6,9) bb r u h s d",
            }
        },
    };

    /// <summary><c>Firmware/HomePod/???</c></summary>
    public static readonly Dictionary<string, string[]> HomePodFirmwarePages = new()
    {
        { "11.x", new[] { "v b k(AudioAccessory1,1;AudioAccessory1,2) r d" } },
        { "12.x", new[] { "v b k(AudioAccessory1,1;AudioAccessory1,2) r d" } },
        { "13.x", new[] { "v b k(AudioAccessory1,1;AudioAccessory1,2) r i d" } },
        {
            "14.x", new[]
            {
                "v b k(AudioAccessory1,1;AudioAccessory1,2) r d",
                "v b k(AudioAccessory5,1) r u h s d",
            }
        },
        {
            "15.x", new[]
            {
                "v b k(AudioAccessory1,1;AudioAccessory1,2) r d",
                "v b k(AudioAccessory5,1) r u h s d",
            }
        },
    };

    /// <summary><c>Firmware/Mac/???</c></summary>
    public static readonly Dictionary<string, string[]> AppleSiliconFirmwarePages = new()
    {
        {
            "11.x", new[]
            {
                "v b k(ADP3,2) r u h s d",
                "v b k(MacBookAir10,1) r u h s d",
                "v b k(MacBookPro17,1) r u h s d",
                // ReSharper disable once StringLiteralTypo
                "v b k(Macmini9,1) r u h s d",
                "v b k(iMac21,1;iMac21,2) r u h s d",
            }
        },
        {
            "12.x", new[]
            {
                "v b k(MacBookAir10,1) r u h s d",
                "v b k(MacBookPro17,1) r u h s d",
                // ReSharper disable once StringLiteralTypo
                "v b k(Macmini9,1) r u h s d",
                "v b k(iMac21,1;iMac21,2) r u h s d",
                "v b k(MacBookPro18,3;MacBookPro18,4) r u h s d",
                "v b k(MacBookPro18,1;MacBookPro18,2) r u h s d",
            }
        },
    };

    /// <summary><c>Firmware/iBridge/???</c></summary>
    public static readonly Dictionary<string, string[]> IBridgeFirmwarePages = new()
    {
        {
            "2.x", new[]
            {
                "v b k(iBridge1,1) r u h s",
                "v b k(iBridge2,1) r u h s",
                "v b k(iBridge2,3) r u h s",
                "v b k(iBridge2,4) r u h s",
            }
        },
        {
            "3.x", new[]
            {
                "v b k(iBridge2,1) r u h s",
                "v b k(iBridge2,3) r u h s",
                "v b k(iBridge2,4) r u h s",
                "v b k(iBridge2,5) r u h s",
                "v b k(iBridge2,6) r u h s",
                "v b k(iBridge2,7) r u h s",
                "v b k(iBridge2,8) r u h s",
                "v b k(iBridge2,10) r u h s",
                "v b k(iBridge2,11) r u h s",
            }
        },
        {
            // TODO: all the IDs are combined here
            "4.x", new[] { "v b k(iBridge) r u h s" }
        },
        {
            // TODO: all the IDs are combined here
            "5.x", new[] { "v b k(iBridge) r u h s" }
        },
        {
            // TODO: all the IDs are combined here
            "6.x", new[] { "v b k(iBridge) r u h s" }
        },
    };

    /// <summary><c>Firmware/iPad/???</c></summary>
    public static readonly Dictionary<string, string[]> IPadFirmwarePages = new()
    {
        { "3.x", new[] { "v b k(iPad1,1) bb r u h s d" } },
        {
            "4.x", new[]
            {
                "v b k(iPad1,1) bb r u h s d",
                "v b k(iPad2,1) r u h s d",
                "v b k(iPad2,2) bb r u h s d",
                "v b k(iPad2,3) bb r u h s d",
            }
        },
        {
            "5.x", new[]
            {
                "v b k(iPad1,1) bb r u h s d",
                "v b k(iPad2,1) r u h s d",
                "v b k(iPad2,2) bb r u h s d",
                "v b k(iPad2,3) bb r u h s d",
                "v b k(iPad2,4) r u h s d",
                "v b k(iPad3,1) r u h s d",
                "v b k(iPad3,2) bb r u h s d",
                "v b k(iPad3,3) bb r u h s d",
            }
        },
        {
            "6.x", new[]
            {
                "v b k(iPad2,1) r u h s d",
                "v b k(iPad2,2) bb r u h s d",
                "v b k(iPad2,3) bb r u h s d",
                "v b k(iPad2,4) r u h s d",
                "v b k(iPad3,1) r u h s d",
                "v b k(iPad3,2) bb r u h s d",
                "v b k(iPad3,3) bb r u h s d",
                "v b k(iPad3,4) r u h s d",
                "v b k(iPad3,5) bb r u h s d",
                "v b k(iPad3,6) bb r u h s d",
            }
        },
        {
            "7.x", new[]
            {
                "v b k(iPad2,1) r u h s d",
                "v b k(iPad2,2) bb r u h s d",
                "v b k(iPad2,3) bb r u h s d",
                "v b k(iPad2,4) r u h s d",
                "v b k(iPad3,1) r u h s d",
                "v b k(iPad3,2) bb r u h s d",
                "v b k(iPad3,3) bb r u h s d",
                "v b k(iPad3,4) r u h s d",
                "v b k(iPad3,5) bb r u h s d",
                "v b k(iPad3,6) bb r u h s d",
            }
        },
        {
            "8.x", new[]
            {
                "v b k(iPad2,1) r u h s d",
                "v b k(iPad2,2) bb r u h s d",
                "v b k(iPad2,3) bb r u h s d",
                "v b k(iPad2,4) r u h s d",
                "v b k(iPad3,1) r u h s d",
                "v b k(iPad3,2) bb r u h s d",
                "v b k(iPad3,3) bb r u h s d",
                "v b k(iPad3,4) r u h s d",
                "v b k(iPad3,5) bb r u h s d",
                "v b k(iPad3,6) bb r u h s d",
            }
        },
        {
            "9.x", new[]
            {
                "v b k(iPad2,1) r u h s d",
                "v b k(iPad2,2) bb r u h s d",
                "v b k(iPad2,3) bb r u h s d",
                "v b k(iPad2,4) r u h s d",
                "v b k(iPad3,1) r u h s d",
                "v b k(iPad3,2) bb r u h s d",
                "v b k(iPad3,3) bb r u h s d",
                "v b k(iPad3,4) r u h s d",
                "v b k(iPad3,5) bb r u h s d",
                "v b k(iPad3,6) bb r u h s d",
            }
        },
        {
            "10.x", new[]
            {
                "v b k(iPad3,4;iPad3,5;iPad3;6) bb r u h s d",
                "v b k(iPad6,11;iPad6,12) bb r u h s d",
            }
        },
        {
            "11.x", new[]
            {
                "v b k(iPad6,11;iPad6,12) bb r u h s d",
                "v b k(iPad7,5;iPad7,6) bb r u h s d",
            }
        },
        {
            "12.x", new[]
            {
                "v b k(iPad6,11;iPad6,12) bb r u h s d",
                "v b k(iPad7,5;iPad7,6) bb r u h s d",
            }
        },
        {
            "13.x", new[]
            {
                "v b k(iPad6,11;iPad6,12) bb r u h s d",
                "v b k(iPad7,5;iPad7,6) bb r u h s d",
                "v b k(iPad7,11;iPad7,12) bb r u h s d",
            }
        },
        {
            "14.x", new[]
            {
                "v b k(iPad6,11;iPad6,12) bb r u h s d",
                "v b k(iPad7,5;iPad7,6) bb r u h s d",
                "v b k(iPad7,11;iPad7,12) bb r u h s d",
                "v b k(iPad7,6;iPad7,7) bb r u h s d",
            }
        },
        {
            "15.x", new[]
            {
                "v b k(iPad6,11;iPad6,12) bb r u h s d",
                "v b k(iPad7,5;iPad7,6) bb r u h s d",
                "v b k(iPad7,11;iPad7,12) bb r u h s d",
                "v b k(iPad7,6;iPad7,7) bb r u h s d",
                "v b k(iPad12,1;iPad12,2) bb r u h s d",
            }
        },
    };

    /// <summary><c>Firmware/iPad_Air/???</c></summary>
    public static readonly Dictionary<string, string[]> IPadAirFirmwarePages = new()
    {
        {
            "7.x", new[]
            {
                "v b k(iPad4,1) r u h s d",
                "v b k(iPad4,2) bb r u h s d",
                "v b k(iPad4,3) bb r u h s d",
            }
        },
        {
            "8.x", new[]
            {
                "v b k(iPad4,1) r u h s d",
                "v b k(iPad4,2) bb r u h s d",
                "v b k(iPad4,3) bb r u h s d",
                "v b k(iPad5,3) r u h s d",
                "v b k(iPad5,4) bb r u h s d",
            }
        },
        {
            "9.x", new[]
            {
                "v b k(iPad4,1) r u h s d",
                "v b k(iPad4,2) bb r u h s d",
                "v b k(iPad4,3) bb r u h s d",
                "v b k(iPad5,3) r u h s d",
                "v b k(iPad5,4) bb r u h s d",
            }
        },
        {
            "10.x", new[]
            {
                "v b k(iPad4,1;iPad4,2;iPad4,3) bb r u h s d",
                "v b k(iPad5,3;iPad5,4) bb r u h s d",
            }
        },
        {
            "11.x", new[]
            {
                "v b k(iPad4,1;iPad4,2;iPad4,3) bb r u h s d",
                "v b k(iPad5,3;iPad5,4) bb r u h s d",
            }
        },
        {
            "12.x", new[]
            {
                "v b k(iPad4,1;iPad4,2;iPad4,3) bb r u h s d",
                "v b k(iPad5,3;iPad5,4) bb r u h s d",
                "v b k(iPad11,3;iPad11,4) bb r u h s d",
            }
        },
        {
            "13.x", new[]
            {
                "v b k(iPad5,3;iPad5,4) bb r u h s d",
                "v b k(iPad11,3;iPad11,4) bb r u h s d",
            }
        },
        {
            "14.x", new[]
            {
                "v b k(iPad5,3;iPad5,4) bb r u h s d",
                "v b k(iPad11,3;iPad11,4) bb r u h s d",
                "v b k(iPad13,1;iPad13,2) bb r u h s d",
            }
        },
        {
            "15.x", new[]
            {
                "v b k(iPad5,3;iPad5,4) bb r u h s d",
                "v b k(iPad11,3;iPad11,4) bb r u h s d",
                "v b k(iPad13,1;iPad13,2) bb r u h s d",
            }
        },
    };

    /// <summary><c>Firmware/iPad_Pro/???</c></summary>
    public static readonly Dictionary<string, string[]> IPadProFirmwarePages = new()
    {
        {
            "9.x", new[]
            {
                "v b k(iPad6,7) r u h s d",
                "v b k(iPad6,8) bb r u h s d",
                "v b k(iPad6,3) r u h s d",
                "v b k(iPad6,4) bb r u h s d",
            }
        },
        {
            "10.x", new[]
            {
                "v b k(iPad6,7;iPad6,8) bb r u h s d",
                "v b k(iPad6,3;iPad6,4) bb r u h s d",
                "v b k(iPad7,1;iPad7,2) bb r u h s d",
                "v b k(iPad7,3;iPad7,4) bb r u h s d",
            }
        },
        {
            "11.x", new[]
            {
                "v b k(iPad6,7;iPad6,8) bb r u h s d",
                "v b k(iPad6,3;iPad6,4) bb r u h s d",
                "v b k(iPad7,1;iPad7,2) bb r u h s d",
                "v b k(iPad7,3;iPad7,4) bb r u h s d",
            }
        },
        {
            "12.x", new[]
            {
                "v b k(iPad6,7;iPad6,8) bb r u h s d",
                "v b k(iPad6,3;iPad6,4) bb r u h s d",
                "v b k(iPad7,1;iPad7,2) bb r u h s d",
                "v b k(iPad7,3;iPad7,4) bb r u h s d",
                "v b k(iPad8,1;iPad8,2;iPad8,3;iPad8,4) bb r u h s d",
                "v b k(iPad8,5;iPad8,6;iPad8,7;iPad8,8) bb r u h s d",
            }
        },
        {
            "13.x", new[]
            {
                "v b k(iPad6,7;iPad6,8) bb r u h s d",
                "v b k(iPad6,3;iPad6,4) bb r u h s d",
                "v b k(iPad7,1;iPad7,2) bb r u h s d",
                "v b k(iPad7,3;iPad7,4) bb r u h s d",
                "v b k(iPad8,1;iPad8,2;iPad8,3;iPad8,4) bb r u h s d",
                "v b k(iPad8,5;iPad8,6;iPad8,7;iPad8,8) bb r u h s d",
                "v b k(iPad8,9;iPad8,10) bb r u h s d",
                "v b k(iPad8,11;iPad8,12) bb r u h s d",
            }
        },
        {
            "14.x", new[]
            {
                "v b k(iPad6,7;iPad6,8) bb r u h s d",
                "v b k(iPad6,3;iPad6,4) bb r u h s d",
                "v b k(iPad7,1;iPad7,2) bb r u h s d",
                "v b k(iPad7,3;iPad7,4) bb r u h s d",
                "v b k(iPad8,1;iPad8,2;iPad8,3;iPad8,4) bb r u h s d",
                "v b k(iPad8,5;iPad8,6;iPad8,7;iPad8,8) bb r u h s d",
                "v b k(iPad8,9;iPad8,10) bb r u h s d",
                "v b k(iPad8,11;iPad8,12) bb r u h s d",
                "v b k(iPad13,4;iPad13,5;iPad13,6;iPad13,7) bb r u h s d",
                "v b k(iPad13,8;iPad13,9;iPad13,10;iPad13,11) bb r u h s d",
            }
        },
        {
            "15.x", new[]
            {
                "v b k(iPad6,7;iPad6,8) bb r u h s d",
                "v b k(iPad6,3;iPad6,4) bb r u h s d",
                "v b k(iPad7,1;iPad7,2) bb r u h s d",
                "v b k(iPad7,3;iPad7,4) bb r u h s d",
                "v b k(iPad8,1;iPad8,2;iPad8,3;iPad8,4) bb r u h s d",
                "v b k(iPad8,5;iPad8,6;iPad8,7;iPad8,8) bb r u h s d",
                "v b k(iPad8,9;iPad8,10) bb r u h s d",
                "v b k(iPad8,11;iPad8,12) bb r u h s d",
                "v b k(iPad13,4;iPad13,5;iPad13,6;iPad13,7) bb r u h s d",
                "v b k(iPad13,8;iPad13,9;iPad13,10;iPad13,11) bb r u h s d",
            }
        },
    };

    /// <summary><c>Firmware/iPad_mini/???</c></summary>
    public static readonly Dictionary<string, string[]> IPadMiniFirmwarePages = new()
    {
        {
            "6.x", new[]
            {
                "v b k(iPad2,5) r u h s d",
                "v b k(iPad2,6) bb r u h s d",
                "v b k(iPad2,7) bb r u h s d",
            }
        },
        {
            "7.x", new[]
            {
                "v b k(iPad2,5) r u h s d",
                "v b k(iPad2,6) bb r u h s d",
                "v b k(iPad2,7) bb r u h s d",
                "v b k(iPad4,4) r u h s d",
                "v b k(iPad4,5) bb r u h s d",
                "v b k(iPad4,6) bb r u h s d",
            }
        },
        {
            "8.x", new[]
            {
                "v b k(iPad2,5) r u h s d",
                "v b k(iPad2,6) bb r u h s d",
                "v b k(iPad2,7) bb r u h s d",
                "v b k(iPad4,4) r u h s d",
                "v b k(iPad4,5) bb r u h s d",
                "v b k(iPad4,6) bb r u h s d",
                "v b k(iPad4,7) r u h s d",
                "v b k(iPad4,8) bb r u h s d",
                "v b k(iPad4,9) bb r u h s d",
            }
        },
        {
            "9.x", new[]
            {
                "v b k(iPad2,5) r u h s d",
                "v b k(iPad2,6) bb r u h s d",
                "v b k(iPad2,7) bb r u h s d",
                "v b k(iPad4,4) r u h s d",
                "v b k(iPad4,5) bb r u h s d",
                "v b k(iPad4,6) bb r u h s d",
                "v b k(iPad4,7) r u h s d",
                "v b k(iPad4,8) bb r u h s d",
                "v b k(iPad4,9) bb r u h s d",
                "v b k(iPad5,1) r u h s d",
                "v b k(iPad5,2) bb r u h s d",
            }
        },
        {
            "10.x", new[]
            {
                "v b k(iPad4,4;iPad4,5;iPad4,6) bb r u h s d",
                "v b k(iPad4,7;iPad4,8;iPad4,9) bb r u h s d",
                "v b k(iPad5,1;iPad5,2) bb r u h s d",
            }
        },
        {
            "11.x", new[]
            {
                "v b k(iPad4,4;iPad4,5;iPad4,6) bb r u h s d",
                "v b k(iPad4,7;iPad4,8;iPad4,9) bb r u h s d",
                "v b k(iPad5,1;iPad5,2) bb r u h s d",
            }
        },
        {
            "12.x", new[]
            {
                "v b k(iPad4,4;iPad4,5;iPad4,6) bb r u h s d",
                "v b k(iPad4,7;iPad4,8;iPad4,9) bb r u h s d",
                "v b k(iPad5,1;iPad5,2) bb r u h s d",
                "v b k(iPad11,1;iPad11,2) bb r u h s d",
            }
        },
        {
            "13.x", new[]
            {
                "v b k(iPad5,1;iPad5,2) bb r u h s d",
                "v b k(iPad11,1;iPad11,2) bb r u h s d",
            }
        },
        {
            "14.x", new[]
            {
                "v b k(iPad5,1;iPad5,2) bb r u h s d",
                "v b k(iPad11,1;iPad11,2) bb r u h s d",
            }
        },
        {
            "15.x", new[]
            {
                "v b k(iPad5,1;iPad5,2) bb r u h s d",
                "v b k(iPad11,1;iPad11,2) bb r u h s d",
                "v b k(iPad14,1;iPad14,2) bb r u h s d",
            }
        },
    };

    /// <summary><c>Firmware/iPhone/???</c></summary>
    public static readonly Dictionary<string, string[]> IPhoneFirmwarePages = new()
    {
        { "1.x", new[] { "v b k(iPhone1,1) bb r u h s d" } },
        {
            "2.x", new[]
            {
                "v b k(iPhone1,1) bb r u h s d",
                "v b k(iPhone1,2) bb r u h s d",
            }
        },
        {
            "3.x", new[]
            {
                "v b k(iPhone1,1) bb r u h s d",
                "v b k(iPhone1,2) bb r u h s d",
                "v b k(iPhone2,1) bb r u h s d",
            }
        },
        {
            "4.x", new[]
            {
                "v b k(iPhone1,2) bb r u h s d",
                "v b k(iPhone2,1) bb r u h s d",
                "v b k(iPhone3,1) bb r u h s d",
                "v b k(iPhone3,3) bb r u h s d",
            }
        },
        {
            "5.x", new[]
            {
                "v b k(iPhone2,1) bb r u h s d",
                "v b k(iPhone3,1) bb r u h s d",
                "v b k(iPhone3,3) bb r u h s d",
                "v b k(iPhone4,1) bb r u h s d",
            }
        },
        {
            "6.x", new[]
            {
                "v b k(iPhone2,1) bb r u h s d",
                "v b k(iPhone3,1) bb r u h s d",
                "v b k(iPhone3,2) bb r u h s d",
                "v b k(iPhone3,3) bb r u h s d",
                "v b k(iPhone4,1) bb r u h s d",
                "v b k(iPhone5,1) bb r u h s d",
                "v b k(iPhone5,2) bb r u h s d",
            }
        },
        {
            "7.x", new[]
            {
                "v b k(iPhone3,1) bb r u h s d",
                "v b k(iPhone3,2) bb r u h s d",
                "v b k(iPhone3,3) bb r u h s d",
                "v b k(iPhone4,1) bb r u h s d",
                "v b k(iPhone5,1) bb r u h s d",
                "v b k(iPhone5,2) bb r u h s d",
                "v b k(iPhone5,3) bb r u h s d",
                "v b k(iPhone5,4) bb r u h s d",
                "v b k(iPhone6,1) bb r u h s d",
                "v b k(iPhone6,2) bb r u h s d",
            }
        },
        {
            "8.x", new[]
            {
                "v b k(iPhone4,1) bb r u h s d",
                "v b k(iPhone5,1) bb r u h s d",
                "v b k(iPhone5,2) bb r u h s d",
                "v b k(iPhone5,3) bb r u h s d",
                "v b k(iPhone5,4) bb r u h s d",
                "v b k(iPhone6,1) bb r u h s d",
                "v b k(iPhone6,2) bb r u h s d",
                "v b k(iPhone7,2) bb r u h s d",
                "v b k(iPhone7,1) bb r u h s d",
            }
        },
        {
            "9.x", new[]
            {
                "v b k(iPhone4,1) bb r u h s d",
                "v b k(iPhone5,1) bb r u h s d",
                "v b k(iPhone5,2) bb r u h s d",
                "v b k(iPhone5,3) bb r u h s d",
                "v b k(iPhone5,4) bb r u h s d",
                "v b k(iPhone6,1) bb r u h s d",
                "v b k(iPhone6,2) bb r u h s d",
                "v b k(iPhone7,2) bb r u h s d",
                "v b k(iPhone7,1) bb r u h s d",
                "v b k(iPhone8,1) bb r u h s d",
                "v b k(iPhone8,2) bb r u h s d",
                "v b k(iPhone8,4) bb r u h s d",
            }
        },
        {
            "10.x", new[]
            {
                "v b k(iPhone5,1;iPhone5,2) bb r u h s d",
                "v b k(iPhone5,3;iPhone5,4) bb r u h s d",
                "v b k(iPhone6,1;iPhone6,2) bb r u h s d",
                "v b k(iPhone7,2) bb r u h s d",
                "v b k(iPhone7,1) bb r u h s d",
                "v b k(iPhone8,1) bb r u h s d",
                "v b k(iPhone8,2) bb r u h s d",
                "v b k(iPhone8,4) bb r u h s d",
                "v b k(iPhone9,1;iPhone9,3) bb(iPhone9,1) bb(iPhone9,3) r u h s d",
                "v b k(iPhone9,2;iPhone9,4) bb(iPhone9,2) bb(iPhone9,4) r u h s d",
            }
        },
        {
            "11.x", new[]
            {
                "v b k(iPhone6,1;iPhone6,2) bb r u h s d",
                "v b k(iPhone7,2) bb r u h s d",
                "v b k(iPhone7,1) bb r u h s d",
                "v b k(iPhone8,1) bb r u h s d",
                "v b k(iPhone8,2) bb r u h s d",
                "v b k(iPhone8,4) bb r u h s d",
                "v b k(iPhone9,1;iPhone9,3) bb(iPhone9,1) bb(iPhone9,3) r u h s d",
                "v b k(iPhone9,2;iPhone9,4) bb(iPhone9,2) bb(iPhone9,4) r u h s d",
                "v b k(iPhone10,1;iPhone10,4) bb(iPhone10,1) bb(iPhone10,4) r u h s d",
                "v b k(iPhone10,2;iPhone10,5) bb(iPhone10,2) bb(iPhone10,5) r u h s d",
                "v b k(iPhone10,3;iPhone10,6) bb(iPhone10,3) bb(iPhone10,6) r u h s d",
            }
        },
        {
            "12.x", new[]
            {
                "v b k(iPhone6,1;iPhone6,2) bb r u h s d",
                "v b k(iPhone7,2) bb r u h s d",
                "v b k(iPhone7,1) bb r u h s d",
                "v b k(iPhone8,1) bb r u h s d",
                "v b k(iPhone8,2) bb r u h s d",
                "v b k(iPhone8,4) bb r u h s d",
                "v b k(iPhone9,1;iPhone9,3) bb(iPhone9,1) bb(iPhone9,3) r u h s d",
                "v b k(iPhone9,2;iPhone9,4) bb(iPhone9,2) bb(iPhone9,4) r u h s d",
                "v b k(iPhone10,1;iPhone10,4) bb(iPhone10,1) bb(iPhone10,4) r u h s d",
                "v b k(iPhone10,2;iPhone10,5) bb(iPhone10,2) bb(iPhone10,5) r u h s d",
                "v b k(iPhone10,3;iPhone10,6) bb(iPhone10,3) bb(iPhone10,6) r u h s d",
                "v b k(iPhone11,8) bb r u h s d",
                "v b k(iPhone11,2) bb r u h s d",
                "v b k(iPhone11,4;iPhone11,6) bb r u h s d",
            }
        },
        {
            "13.x", new[]
            {
                "v b k(iPhone8,1) bb r u h s d",
                "v b k(iPhone8,2) bb r u h s d",
                "v b k(iPhone8,4) bb r u h s d",
                "v b k(iPhone9,1;iPhone9,3) bb(iPhone9,1) bb(iPhone9,3) r u h s d",
                "v b k(iPhone9,2;iPhone9,4) bb(iPhone9,2) bb(iPhone9,4) r u h s d",
                "v b k(iPhone10,1;iPhone10,4) bb(iPhone10,1) bb(iPhone10,4) r u h s d",
                "v b k(iPhone10,2;iPhone10,5) bb(iPhone10,2) bb(iPhone10,5) r u h s d",
                "v b k(iPhone10,3;iPhone10,6) bb(iPhone10,3) bb(iPhone10,6) r u h s d",
                "v b k(iPhone11,8) bb r u h s d",
                "v b k(iPhone11,2) bb r u h s d",
                "v b k(iPhone11,4;iPhone11,6) bb r u h s d",
                "v b k(iPhone12,1) bb r u h s d",
                "v b k(iPhone12,3) bb r u h s d",
                "v b k(iPhone12,5) bb r u h s d",
                "v b k(iPhone12,8) bb r u h s d",
            }
        },
        {
            "14.x", new[]
            {
                "v b k(iPhone8,1) bb r u h s d",
                "v b k(iPhone8,2) bb r u h s d",
                "v b k(iPhone8,4) bb r u h s d",
                "v b k(iPhone9,1;iPhone9,3) bb(iPhone9,1) bb(iPhone9,3) r u h s d",
                "v b k(iPhone9,2;iPhone9,4) bb(iPhone9,2) bb(iPhone9,4) r u h s d",
                "v b k(iPhone10,1;iPhone10,4) bb(iPhone10,1) bb(iPhone10,4) r u h s d",
                "v b k(iPhone10,2;iPhone10,5) bb(iPhone10,2) bb(iPhone10,5) r u h s d",
                "v b k(iPhone10,3;iPhone10,6) bb(iPhone10,3) bb(iPhone10,6) r u h s d",
                "v b k(iPhone11,8) bb r u h s d",
                "v b k(iPhone11,2) bb r u h s d",
                "v b k(iPhone11,4;iPhone11,6) bb r u h s d",
                "v b k(iPhone12,1) bb r u h s d",
                "v b k(iPhone12,3) bb r u h s d",
                "v b k(iPhone12,5) bb r u h s d",
                "v b k(iPhone12,8) bb r u h s d",
                "v b k(iPhone13,1) bb r u h s d",
                "v b k(iPhone13,2) bb r u h s d",
                "v b k(iPhone13,3) bb r u h s d",
                "v b k(iPhone13,4) bb r u h s d",
            }
        },
        {
            "15.x", new[]
            {
                "v b k(iPhone8,1) bb r u h s d",
                "v b k(iPhone8,2) bb r u h s d",
                "v b k(iPhone8,4) bb r u h s d",
                "v b k(iPhone9,1;iPhone9,3) bb(iPhone9,1) bb(iPhone9,3) r u h s d",
                "v b k(iPhone9,2;iPhone9,4) bb(iPhone9,2) bb(iPhone9,4) r u h s d",
                "v b k(iPhone10,1;iPhone10,4) bb(iPhone10,1) bb(iPhone10,4) r u h s d",
                "v b k(iPhone10,2;iPhone10,5) bb(iPhone10,2) bb(iPhone10,5) r u h s d",
                "v b k(iPhone10,3;iPhone10,6) bb(iPhone10,3) bb(iPhone10,6) r u h s d",
                "v b k(iPhone11,8) bb r u h s d",
                "v b k(iPhone11,2) bb r u h s d",
                "v b k(iPhone11,4;iPhone11,6) bb r u h s d",
                "v b k(iPhone12,1) bb r u h s d",
                "v b k(iPhone12,3) bb r u h s d",
                "v b k(iPhone12,5) bb r u h s d",
                "v b k(iPhone12,8) bb r u h s d",
                "v b k(iPhone13,1) bb r u h s d",
                "v b k(iPhone13,2) bb r u h s d",
                "v b k(iPhone13,3) bb r u h s d",
                "v b k(iPhone13,4) bb r u h s d",
                "v b k(iPhone14,4) bb r u h s d",
                "v b k(iPhone14,5) bb r u h s d",
                "v b k(iPhone14,2) bb r u h s d",
                "v b k(iPhone14,3) bb r u h s d",
            }
        },
    };

    /// <summary><c>Firmware/iPod_touch/???</c></summary>
    public static readonly Dictionary<string, string[]> IPodTouchFirmwarePages = new()
    {
        { "1.x", new[] { "v b k(iPod1,1) r u h s d" } },
        {
            "2.x", new[]
            {
                "v b k(iPod1,1) r i h s d",
                "v b k(iPod2,1) r u h s d",
            }
        },
        {
            "3.x", new[]
            {
                "v b k(iPod1,1) r i h s d",
                "v b k(iPod2,1) r i h s d",
                "v b k(iPod3,1) r u h s d",
            }
        },
        {
            "4.x", new[]
            {
                "v b k(iPod2,1) r u h s d",
                "v b k(iPod3,1) r u h s d",
                "v b k(iPod4,1) r u h s d",
            }
        },
        {
            "5.x", new[]
            {
                "v b k(iPod3,1) r u h s d",
                "v b k(iPod4,1) r u h s d",
            }
        },
        {
            "6.x", new[]
            {
                "v b k(iPod4,1) r u h s d",
                "v b k(iPod5,1) r u h s d",
            }
        },
        { "7.x", new[] { "v b k(iPod5,1) r u h s d" } },
        {
            "8.x", new[]
            {
                "v b k(iPod5,1) r u h s d",
                "v b k(iPod7,1) r u h s d",
            }
        },
        {
            "9.x", new[]
            {
                "v b k(iPod5,1) r u h s d",
                "v b k(iPod7,1) r u h s d",
            }
        },
        { "10.x", new[] { "v b k(iPod7,1) r u h s d" } },
        { "11.x", new[] { "v b k(iPod7,1) r u h s d" } },
        {
            "12.x", new[]
            {
                "v b k(iPod7,1) r u h s d",
                "v b k(iPod9,1) r u h s d",
            }
        },
        { "13.x", new[] { "v b k(iPod9,1) r u h s d" } },
        { "14.x", new[] { "v b k(iPod9,1) r u h s d" } },
        { "15.x", new[] { "v b k(iPod9,1) r u h s d" } },
    };
}
