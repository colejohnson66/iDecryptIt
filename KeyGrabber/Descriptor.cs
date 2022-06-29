/* =============================================================================
 * File:   Descriptor.cs
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

namespace KeyGrabber;

public static class Descriptor
{
    public record DeviceEntry(
        string UrlPrefix,
        params MajorVersionEntry[] Entries);

    public record MajorVersionEntry(
        string MajorVersion,
        params string[] DslForTables);

    /* DSL Key:
     *   - vm: Marketing version column
     *   - v: Version column
     *   - bm: Marketing build column
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

    private static readonly DeviceEntry APPLE_TV = new(
        "Firmware/Apple_TV/",
        new("4.x", "vm v bm b k(AppleTV2,1) r u h s d"),
        new(
            "5.x",
            "vm v bm b k(AppleTV2,1) r u h s d",
            "vm v bm b k(AppleTV3,1) r u h s d",
            "vm v bm b k(AppleTV3,2) r u h s d"
        ),
        new(
            "6.x",
            "vm v bm b k(AppleTV2,1) r u h s d",
            "vm v bm b k(AppleTV3,1) r u h s d",
            "vm v bm b k(AppleTV3,2) r u h s d"
        ),
        new(
            "7.x",
            "vm v bm b k(AppleTV3,1) r u h s d",
            "vm v bm b k(AppleTV3,2) r u h s d"
        ),
        new("9.x", "v b k(AppleTV5,3) r u h s d"),
        new("10.x", "v b k(AppleTV5,3) r u h s d"),
        new(
            "11.x",
            "v b k(AppleTV5,3) r u h s d",
            "v b k(AppleTV6,2) r d"
        ),
        new(
            "12.x",
            "v b k(AppleTV5,3) r u h s d",
            "v b k(AppleTV6,2) r d"
        ),
        new(
            "13.x",
            "v b k(AppleTV5,3) r u h s d d",
            "v b k(AppleTV6,2) r d d"
        ),
        new(
            "14.x",
            "v b k(AppleTV5,3) r u h s d",
            "v b k(AppleTV6,2) r d",
            "v b k(AppleTV11,1) r d"
        ),
        new(
            "15.x",
            "v b k(AppleTV5,3) r u h s d",
            "v b k(AppleTV6,2) r d",
            "v b k(AppleTV11,1) r d"
        ));

    private static readonly DeviceEntry APPLE_TV_BETA = new(
        "Beta_Firmware/Apple_TV/",
        new("4.x", "vm v bm b k(AppleTV2,1) r u s"),
        new(
            "5.x",
            "vm v bm b k(AppleTV2,1) r u s",
            "vm v bm b k(AppleTV3,1) r u s",
            "vm v bm b k(AppleTV3,2) r u s"
        ),
        new(
            "6.x",
            "vm v bm b k(AppleTV2,1) r u s",
            "vm v bm b k(AppleTV3,1) r u s",
            "vm v bm b k(AppleTV3,2) r u s"
        ),
        new(
            "7.x",
            "vm v bm b k(AppleTV3,1) r u s",
            "vm v bm b k(AppleTV3,2) r u s"
        ),
        new("9.x", "v b k(AppleTV5,3) r u s"),
        new("10.x", "v b k(AppleTV5,3) r u s"),
        new(
            "11.x",
            "v b k(AppleTV5,3) r u s",
            "v b k(AppleTV6,2) r"
        ),
        new(
            "12.x",
            "v b k(AppleTV5,3) r u s",
            "v b k(AppleTV6,2) r"
        ),
        new(
            "13.x",
            "v b k(AppleTV5,3) r u s",
            "v b k(AppleTV6,2) r"
        ),
        new(
            "14.x",
            "v b k(AppleTV5,3) r u s",
            "v b k(AppleTV6,2) r",
            "v b k(AppleTV11,1) r"
        ),
        new(
            "15.x",
            "v b k(AppleTV5,3) r u s",
            "v b k(AppleTV6,2) r",
            "v b k(AppleTV11,1) r"
        ));

    private static readonly DeviceEntry APPLE_WATCH = new(
        "Firmware/Apple_Watch/",
        new(
            "1.x",
            "vm v b k(Watch1,1) r d",
            "vm v b k(Watch1,2) r d"),
        // TODO: Watch1,1 2.1/9.0 (13S661) has:
        //   IPSW: http://appldnld.apple.com/watch/os2.1/031-29536-20151208-ca603a1a-8eec-11e5-9b1f-e0c48b8beceb/watch1,1_2.1_13s661_restore.ipsw
        //   Hash: e02ab4ace8051ee12996e5547c397be30666d594
        //   Size: 924,502,956
        // TODO: Watch1,2 2.1/9.0 (13S661) has:
        //   IPSW: http://appldnld.apple.com/watch/os2.1/031-29539-20151208-ca603a1a-8eec-11e5-9b1f-e1c48b8beceb/watch1,2_2.1_13s661_restore.ipsw
        //   Hash: 44b29ebb1552c930c9fc8921a86e29f9bb9fe00f
        //   Size: 926,842,087
        new(
            "2.x",
            "vm v b k(Watch1,1) r d",
            "vm v b k(Watch1,2) r d"
        ),
        new(
            "3.x",
            "vm v b k(Watch1,1) r d",
            "vm v b k(Watch1,2) r d",
            "vm v b k(Watch2,6) r d",
            "vm v b k(Watch2,7) r d",
            "vm v b k(Watch2,3) r d",
            "vm v b k(Watch2,4) r d"
        ),
        new(
            "4.x",
            "vm v b k(Watch1,1) r d",
            "vm v b k(Watch1,2) r d",
            "vm v b k(Watch2,6) r d",
            "vm v b k(Watch2,7) r d",
            "vm v b k(Watch2,3) r d",
            "vm v b k(Watch2,4) r d",
            "vm v b k(Watch3,1) bb r d",
            "vm v b k(Watch3,2) bb r d",
            "vm v b k(Watch3,3) r d",
            "vm v b k(Watch3,4) r d"
        ),
        new(
            "5.x",
            "vm v b k(Watch2,6) r d",
            "vm v b k(Watch2,7) r d",
            "vm v b k(Watch2,3) r d",
            "vm v b k(Watch2,4) r d",
            "vm v b k(Watch3,1) bb r d",
            "vm v b k(Watch3,2) bb r d",
            "vm v b k(Watch3,3) r d",
            "vm v b k(Watch3,4) r d",
            "vm v b k(Watch4,1) r d",
            "vm v b k(Watch4,2) r d",
            "vm v b k(Watch4,3) bb r d",
            "vm v b k(Watch4,4) bb r d"
        ),
        new(
            "6.x",
            "vm v b k(Watch2,6) r u h s d",
            "vm v b k(Watch2,7) r u h s d",
            "vm v b k(Watch2,3) r u h s d",
            "vm v b k(Watch2,4) r u h s d",
            "vm v b k(Watch3,1) bb r d",
            "vm v b k(Watch3,2) bb r d",
            "vm v b k(Watch3,3) r d",
            "vm v b k(Watch3,4) r d",
            "vm v b k(Watch4,1) r d",
            "vm v b k(Watch4,2) r d",
            "vm v b k(Watch4,3) bb r d",
            "vm v b k(Watch4,4) bb r d",
            "vm v b k(Watch5,1) r d",
            "vm v b k(Watch5,2) r d",
            "vm v b k(Watch5,3) bb r d",
            "vm v b k(Watch5,4) bb r d"
        ),
        new(
            "7.x",
            "vm v b k(Watch3,1) bb r u h s d",
            "vm v b k(Watch3,2) bb r u h s d",
            "vm v b k(Watch3,3) r u h s d",
            "vm v b k(Watch3,4) r u h s d",
            "vm v b k(Watch4,1) r u h s d",
            "vm v b k(Watch4,2) r u h s d",
            "vm v b k(Watch4,3) bb r u h s d",
            "vm v b k(Watch4,4) bb r u h s d",
            "vm v b k(Watch5,1) r u h s d",
            "vm v b k(Watch5,2) r u h s d",
            "vm v b k(Watch5,3) bb r u h s d",
            "vm v b k(Watch5,4) bb r u h s d",
            "vm v b k(Watch5,9) r u h s d",
            "vm v b k(Watch5,10) r u h s d",
            "vm v b k(Watch5,11) bb r u h s d",
            "vm v b k(Watch5,12) bb r u h s d",
            "vm v b k(Watch6,1) r u h s d",
            "vm v b k(Watch6,2) r u h s d",
            "vm v b k(Watch6,3) bb r u h s d",
            "vm v b k(Watch6,4) bb r u h s d"
        ),
        new(
            "8.x",
            "vm v b k(Watch3,1) bb r u h s d",
            "vm v b k(Watch3,2) bb r u h s d",
            "vm v b k(Watch3,3) r u h s d",
            "vm v b k(Watch3,4) r u h s d",
            "vm v b k(Watch4,1) r u h s d",
            "vm v b k(Watch4,2) r u h s d",
            "vm v b k(Watch4,3) bb r u h s d",
            "vm v b k(Watch4,4) bb r u h s d",
            "vm v b k(Watch5,1) r u h s d",
            "vm v b k(Watch5,2) r u h s d",
            "vm v b k(Watch5,3) bb r u h s d",
            "vm v b k(Watch5,4) bb r u h s d",
            "vm v b k(Watch5,9) r u h s d",
            "vm v b k(Watch5,10) r u h s d",
            "vm v b k(Watch5,11) bb r u h s d",
            "vm v b k(Watch5,12) bb r u h s d",
            "vm v b k(Watch6,1) r u h s d",
            "vm v b k(Watch6,2) r u h s d",
            "vm v b k(Watch6,3) bb r u h s d",
            "vm v b k(Watch6,4) bb r u h s d",
            "vm v b k(Watch6,6) r u h s d",
            "vm v b k(Watch6,7) r u h s d",
            "vm v b k(Watch6,8) bb r u h s d",
            "vm v b k(Watch6,9) bb r u h s d"
        )
    );

    private static readonly DeviceEntry APPLE_WATCH_BETA = new(
        "Beta_Firmware/Apple_Watch/",
        new(
            "2.x",
            "vm v b k(Watch1,1) r",
            "vm v b k(Watch1,2) r"
        ),
        new(
            "3.x",
            "vm v b k(Watch1,1) r",
            "vm v b k(Watch1,2) r",
            "vm v b k(Watch2,6) r",
            "vm v b k(Watch2,7) r",
            "vm v b k(Watch2,3) r",
            "vm v b k(Watch2,4) r"
        ),
        new(
            "4.x",
            "vm v b k(Watch1,1) r",
            "vm v b k(Watch1,2) r",
            "vm v b k(Watch2,6) r",
            "vm v b k(Watch2,7) r",
            "vm v b k(Watch2,3) r",
            "vm v b k(Watch2,4) r",
            "vm v b k(Watch3,1) bb r",
            "vm v b k(Watch3,2) bb r",
            "vm v b k(Watch3,3) r",
            "vm v b k(Watch3,4) r"
        ),
        new(
            "5.x",
            "vm v b k(Watch2,6) r",
            "vm v b k(Watch2,7) r",
            "vm v b k(Watch2,3) r",
            "vm v b k(Watch2,4) r",
            "vm v b k(Watch3,1) bb r",
            "vm v b k(Watch3,2) bb r",
            "vm v b k(Watch3,3) r",
            "vm v b k(Watch3,4) r",
            "vm v b k(Watch4,1) r",
            "vm v b k(Watch4,2) r",
            "vm v b k(Watch4,3) bb r",
            "vm v b k(Watch4,4) bb r"
        ),
        new(
            "6.x",
            "vm v b k(Watch2,6) r",
            "vm v b k(Watch2,7) r",
            "vm v b k(Watch2,3) r",
            "vm v b k(Watch2,4) r",
            "vm v b k(Watch3,1) bb r",
            "vm v b k(Watch3,2) bb r",
            "vm v b k(Watch3,3) r",
            "vm v b k(Watch3,4) r",
            "vm v b k(Watch4,1) r",
            "vm v b k(Watch4,2) r",
            "vm v b k(Watch4,3) bb r",
            "vm v b k(Watch4,4) bb r",
            "vm v b k(Watch5,1) r",
            "vm v b k(Watch5,2) r",
            "vm v b k(Watch5,3) bb r",
            "vm v b k(Watch5,4) bb r"
        ),
        new(
            "7.x",
            "vm v b k(Watch3,1) bb r",
            "vm v b k(Watch3,2) bb r",
            "vm v b k(Watch3,3) r",
            "vm v b k(Watch3,4) r",
            "vm v b k(Watch4,1) r",
            "vm v b k(Watch4,2) r",
            "vm v b k(Watch4,3) bb r",
            "vm v b k(Watch4,4) bb r",
            "vm v b k(Watch5,1) r",
            "vm v b k(Watch5,2) r",
            "vm v b k(Watch5,3) bb r",
            "vm v b k(Watch5,4) bb r",
            "vm v b k(Watch5,9) r",
            "vm v b k(Watch5,10) r",
            "vm v b k(Watch5,11) bb r",
            "vm v b k(Watch5,12) bb r",
            "vm v b k(Watch6,1) r",
            "vm v b k(Watch6,2) r",
            "vm v b k(Watch6,3) bb r",
            "vm v b k(Watch6,4) bb r"
        ),
        new(
            "8.x",
            "vm v b k(Watch3,1) bb r",
            "vm v b k(Watch3,2) bb r",
            "vm v b k(Watch3,3) r",
            "vm v b k(Watch3,4) r",
            "vm v b k(Watch4,1) r",
            "vm v b k(Watch4,2) r",
            "vm v b k(Watch4,3) bb r",
            "vm v b k(Watch4,4) bb r",
            "vm v b k(Watch5,1) r",
            "vm v b k(Watch5,2) r",
            "vm v b k(Watch5,3) bb r",
            "vm v b k(Watch5,4) bb r",
            "vm v b k(Watch5,9) r",
            "vm v b k(Watch5,10) r",
            "vm v b k(Watch5,11) bb r",
            "vm v b k(Watch5,12) bb r",
            "vm v b k(Watch6,1) r",
            "vm v b k(Watch6,2) r",
            "vm v b k(Watch6,3) bb r",
            "vm v b k(Watch6,4) bb r",
            "vm v b k(Watch6,6) r",
            "vm v b k(Watch6,7) r",
            "vm v b k(Watch6,8) bb r",
            "vm v b k(Watch6,9) bb r"
        )
    );

    private static readonly DeviceEntry HOME_POD = new(
        "Firmware/HomePod/",
        new("11.x", "v b k(AudioAccessory1,1;AudioAccessory1,2) r d"),
        new("12.x", "v b k(AudioAccessory1,1;AudioAccessory1,2) r d"),
        new("13.x", "v b k(AudioAccessory1,1;AudioAccessory1,2) r i d"),
        new(
            "14.x",
            "v b k(AudioAccessory1,1;AudioAccessory1,2) r d",
            "v b k(AudioAccessory5,1) r u h s d"
        ),
        new(
            "15.x",
            "v b k(AudioAccessory1,1;AudioAccessory1,2) r d",
            "v b k(AudioAccessory5,1) r u h s d"
        )
    );

    private static readonly DeviceEntry HOME_POD_BETA = new(
        "Beta_Firmware/HomePod/",
        new("11.x", "v b k(AudioAccessory1,1) r"),
        new(
            "14.x",
            "v b k(AudioAccessory1,1;AudioAccessory1,2) r",
            "v b k(AudioAccessory5,1) r"
        ),
        new(
            "15.x",
            "v b k(AudioAccessory1,1;AudioAccessory1,2) r",
            "v b k(AudioAccessory5,1) r"
        )
    );

    // ReSharper disable once UnusedMember.Local
    private static readonly DeviceEntry APPLE_SILICON = new(
        "Firmware/Mac/",
        new(
            "11.x",
            "v b k(ADP3,2) r u h s d",
            "v b k(MacBookAir10,1) r u h s d",
            "v b k(MacBookPro17,1) r u h s d",
            "v b k(Macmini9,1) r u h s d",
            "v b k(iMac21,1;iMac21,2) r u h s d"
        ),
        new(
            "12.x",
            "v b k(MacBookAir10,1) r u h s d",
            "v b k(MacBookPro17,1) r u h s d",
            "v b k(Macmini9,1) r u h s d",
            "v b k(iMac21,1;iMac21,2) r u h s d",
            "v b k(MacBookPro18,3;MacBookPro18,4) r u h s d",
            "v b k(MacBookPro18,1;MacBookPro18,2) r u h s d"
        )
    );

    // ReSharper disable once UnusedMember.Local
    private static readonly DeviceEntry APPLE_SILICON_BETA = new(
        "Beta_Firmware/Mac/",
        new("11.x",
            "v b k(ADP3,2) r",
            "v b k(MacBookAir10,1) r u s",
            "v b k(MacBookPro17,1) r u s",
            "v b k(Macmini9,1) r u s",
            "v b k(iMac21,1;iMac21,2) r u s"
        ),
        new("12.x",
            "v b k(MacBookAir10,1) r u s",
            "v b k(MacBookPro17,1) r u s",
            "v b k(Macmini9,1) r u s",
            "v b k(iMac21,1;iMac21,2) r u s"
        )
    );

    // ReSharper disable once UnusedMember.Local
    // ReSharper disable once IdentifierTypo
    private static readonly DeviceEntry IBRIDGE = new(
        "Firmware/iBridge/",

        new(
            "2.x",
            "v b k(iBridge1,1) r u h s",
            "v b k(iBridge2,1) r u h s",
            "v b k(iBridge2,3) r u h s",
            "v b k(iBridge2,4) r u h s"
        ),
        new(
            "3.x",
            "v b k(iBridge2,1) r u h s",
            "v b k(iBridge2,3) r u h s",
            "v b k(iBridge2,4) r u h s",
            "v b k(iBridge2,5) r u h s",
            "v b k(iBridge2,6) r u h s",
            "v b k(iBridge2,7) r u h s",
            "v b k(iBridge2,8) r u h s",
            "v b k(iBridge2,10) r u h s",
            "v b k(iBridge2,11) r u h s"
        ),
        new("4.x", "v b k(iBridge) r u h s"), // TODO: all the IDs are combined here
        new("5.x", "v b k(iBridge) r u h s"), // TODO: all the IDs are combined here
        new("6.x", "v b k(iBridge) r u h s") // TODO: all the IDs are combined here
    );

    // TODO: iBridge betas?

    private static readonly DeviceEntry IPAD = new(
        "Firmware/iPad/",
        new("3.x", "v b k(iPad1,1) bb r u h s d"),
        new(
            "4.x", "v b k(iPad1,1) bb r u h s d",
            "v b k(iPad2,1) r u h s d",
            "v b k(iPad2,2) bb r u h s d",
            "v b k(iPad2,3) bb r u h s d"
        ),
        new(
            "5.x",
            "v b k(iPad1,1) bb r u h s d",
            "v b k(iPad2,1) r u h s d",
            "v b k(iPad2,2) bb r u h s d",
            "v b k(iPad2,3) bb r u h s d",
            "v b k(iPad2,4) r u h s d",
            "v b k(iPad3,1) r u h s d",
            "v b k(iPad3,2) bb r u h s d",
            "v b k(iPad3,3) bb r u h s d"
        ),
        new(
            "6.x",
            "v b k(iPad2,1) r u h s d",
            "v b k(iPad2,2) bb r u h s d",
            "v b k(iPad2,3) bb r u h s d",
            "v b k(iPad2,4) r u h s d",
            "v b k(iPad3,1) r u h s d",
            "v b k(iPad3,2) bb r u h s d",
            "v b k(iPad3,3) bb r u h s d",
            "v b k(iPad3,4) r u h s d",
            "v b k(iPad3,5) bb r u h s d",
            "v b k(iPad3,6) bb r u h s d"
        ),
        new(
            "7.x",
            "v b k(iPad2,1) r u h s d",
            "v b k(iPad2,2) bb r u h s d",
            "v b k(iPad2,3) bb r u h s d",
            "v b k(iPad2,4) r u h s d",
            "v b k(iPad3,1) r u h s d",
            "v b k(iPad3,2) bb r u h s d",
            "v b k(iPad3,3) bb r u h s d",
            "v b k(iPad3,4) r u h s d",
            "v b k(iPad3,5) bb r u h s d",
            "v b k(iPad3,6) bb r u h s d"
        ),
        new(
            "8.x",
            "v b k(iPad2,1) r u h s d",
            "v b k(iPad2,2) bb r u h s d",
            "v b k(iPad2,3) bb r u h s d",
            "v b k(iPad2,4) r u h s d",
            "v b k(iPad3,1) r u h s d",
            "v b k(iPad3,2) bb r u h s d",
            "v b k(iPad3,3) bb r u h s d",
            "v b k(iPad3,4) r u h s d",
            "v b k(iPad3,5) bb r u h s d",
            "v b k(iPad3,6) bb r u h s d"
        ),
        new(
            "9.x",
            "v b k(iPad2,1) r u h s d",
            "v b k(iPad2,2) bb r u h s d",
            "v b k(iPad2,3) bb r u h s d",
            "v b k(iPad2,4) r u h s d",
            "v b k(iPad3,1) r u h s d",
            "v b k(iPad3,2) bb r u h s d",
            "v b k(iPad3,3) bb r u h s d",
            "v b k(iPad3,4) r u h s d",
            "v b k(iPad3,5) bb r u h s d",
            "v b k(iPad3,6) bb r u h s d"
        ),
        new(
            "10.x",
            "v b k(iPad3,4;iPad3,5;iPad3,6) bb r u h s d",
            "v b k(iPad6,11;iPad6,12) bb r u h s d"
        ),
        new(
            "11.x",
            "v b k(iPad6,11;iPad6,12) bb r u h s d",
            "v b k(iPad7,5;iPad7,6) bb r u h s d"
        ),
        new(
            "12.x",
            "v b k(iPad6,11;iPad6,12) bb r u h s d",
            "v b k(iPad7,5;iPad7,6) bb r u h s d"
        ),
        new(
            "13.x",
            "v b k(iPad6,11;iPad6,12) bb r u h s d",
            "v b k(iPad7,5;iPad7,6) bb r u h s d",
            "v b k(iPad7,11;iPad7,12) bb r u h s d"
        ),
        new(
            "14.x",
            "v b k(iPad6,11;iPad6,12) bb r u h s d",
            "v b k(iPad7,5;iPad7,6) bb r u h s d",
            "v b k(iPad7,11;iPad7,12) bb r u h s d",
            "v b k(iPad11,6;iPad11,7) bb r u h s d"
        ),
        new(
            "15.x",
            "v b k(iPad6,11;iPad6,12) bb r u h s d",
            "v b k(iPad7,5;iPad7,6) bb r u h s d",
            "v b k(iPad7,11;iPad7,12) bb r u h s d",
            "v b k(iPad11,6;iPad11,7) bb r u h s d",
            "v b k(iPad12,1;iPad12,2) bb r u h s d"
        )
    );

    private static readonly DeviceEntry IPAD_BETA = new(
        "Beta_Firmware/iPad/",
        new("4.x", "v b k(iPad1,1) bb r u s"),
        new(
            "5.x",
            "v b k(iPad1,1) bb r u s",
            "v b k(iPad2,1) r u s",
            "v b k(iPad2,2) bb r u s",
            "v b k(iPad2,3) bb r u s"
        ),
        new(
            "6.x",
            "v b k(iPad2,1) r u s",
            "v b k(iPad2,2) bb r u s",
            "v b k(iPad2,3) bb r u s",
            "v b k(iPad2,4) r u s",
            "v b k(iPad3,1) r u s",
            "v b k(iPad3,2) bb r u s",
            "v b k(iPad3,3) bb r u s",
            "v b k(iPad3,4) r u s",
            "v b k(iPad3,5) bb r u s",
            "v b k(iPad3,6) bb r u s"
        ),
        new(
            "7.x",
            "v b k(iPad2,1) r u s",
            "v b k(iPad2,2) bb r u s",
            "v b k(iPad2,3) bb r u s",
            "v b k(iPad2,4) r u s",
            "v b k(iPad3,1) r u s",
            "v b k(iPad3,2) bb r u s",
            "v b k(iPad3,3) bb r u s",
            "v b k(iPad3,4) r u s",
            "v b k(iPad3,5) bb r u s",
            "v b k(iPad3,6) bb r u s"
        ),
        new(
            "8.x",
            "v b k(iPad2,1) r u s",
            "v b k(iPad2,2) bb r u s",
            "v b k(iPad2,3) bb r u s",
            "v b k(iPad2,4) r u s",
            "v b k(iPad3,1) r u s",
            "v b k(iPad3,2) bb r u s",
            "v b k(iPad3,3) bb r u s",
            "v b k(iPad3,4) r u s",
            "v b k(iPad3,5) bb r u s",
            "v b k(iPad3,6) bb r u s"
        ),
        new(
            "9.x",
            "v b k(iPad2,1) r u s",
            "v b k(iPad2,2) bb r u s",
            "v b k(iPad2,3) bb r u s",
            "v b k(iPad2,4) r u s",
            "v b k(iPad3,1) r u s",
            "v b k(iPad3,2) bb r u s",
            "v b k(iPad3,3) bb r u s",
            "v b k(iPad3,4) r u s",
            "v b k(iPad3,5) bb r u s",
            "v b k(iPad3,6) bb r u s"
        ),
        new(
            "10.x",
            "v b k(iPad3,4;iPad3,5;iPad3,6) bb r u s",
            "v b k(iPad6,11;iPad6,12) bb r u s"
        ),
        new(
            "11.x",
            "v b k(iPad6,11;iPad6,12) bb r u s",
            "v b k(iPad7,5;iPad7,6) bb r u s"
        ),
        new(
            "12.x",
            "v b k(iPad6,11;iPad6,12) bb r u s",
            "v b k(iPad7,5;iPad7,6) bb r u s"
        ),
        new(
            "13.x",
            "v b k(iPad6,11;iPad6,12) bb r u s",
            "v b k(iPad7,5;iPad7,6) bb r u s",
            "v b k(iPad7,11;iPad7,12) bb r u s"
        ),
        new(
            "14.x",
            "v b k(iPad6,11;iPad6,12) bb r u s",
            "v b k(iPad7,5;iPad7,6) bb r u s",
            "v b k(iPad7,11;iPad7,12) bb r u s",
            "v b k(iPad11,6;iPad11,7) bb r u s"
        ),
        new(
            "15.x",
            "v b k(iPad6,11;iPad6,12) bb r u s",
            "v b k(iPad7,5;iPad7,6) bb r u s",
            "v b k(iPad7,11;iPad7,12) bb r u s",
            "v b k(iPad11,6;iPad11,7) bb r u s",
            "v b k(iPad12,1;iPad12,2) bb r u s"
        )
    );

    private static readonly DeviceEntry IPAD_AIR = new(
        "Firmware/iPad_Air",
        new(
            "7.x",
            "v b k(iPad4,1) r u h s d",
            "v b k(iPad4,2) bb r u h s d",
            "v b k(iPad4,3) bb r u h s d"
        ),
        new(
            "8.x",
            "v b k(iPad4,1) r u h s d",
            "v b k(iPad4,2) bb r u h s d",
            "v b k(iPad4,3) bb r u h s d",
            "v b k(iPad5,3) r u h s d",
            "v b k(iPad5,4) bb r u h s d"
        ),
        new(
            "9.x",
            "v b k(iPad4,1) r u h s d",
            "v b k(iPad4,2) bb r u h s d",
            "v b k(iPad4,3) bb r u h s d",
            "v b k(iPad5,3) r u h s d",
            "v b k(iPad5,4) bb r u h s d"
        ),
        new(
            "10.x",
            "v b k(iPad4,1;iPad4,2;iPad4,3) bb r u h s d",
            "v b k(iPad5,3;iPad5,4) bb r u h s d"
        ),
        new(
            "11.x",
            "v b k(iPad4,1;iPad4,2;iPad4,3) bb r u h s d",
            "v b k(iPad5,3;iPad5,4) bb r u h s d"
        ),
        new(
            "12.x",
            "v b k(iPad4,1;iPad4,2;iPad4,3) bb r u h s d",
            "v b k(iPad5,3;iPad5,4) bb r u h s d",
            "v b k(iPad11,3;iPad11,4) bb r u h s d"
        ),
        new(
            "13.x",
            "v b k(iPad5,3;iPad5,4) bb r u h s d",
            "v b k(iPad11,3;iPad11,4) bb r u h s d"
        ),
        new(
            "14.x",
            "v b k(iPad5,3;iPad5,4) bb r u h s d",
            "v b k(iPad11,3;iPad11,4) bb r u h s d",
            "v b k(iPad13,1;iPad13,2) bb r u h s d"
        ),
        new(
            "15.x",
            "v b k(iPad5,3;iPad5,4) bb r u h s d",
            "v b k(iPad11,3;iPad11,4) bb r u h s d",
            "v b k(iPad13,1;iPad13,2) bb r u h s d",
            "v b k(iPad13,16;iPad13,17) bb r u h s d"
        )
    );

    private static readonly DeviceEntry IPAD_AIR_BETA = new(
        "Beta_Firmware/iPad_Air/",
        new(
            "7.x",
            "v b k(iPad4,1) r u s",
            "v b k(iPad4,2) bb r u s"
        ),
        new(
            "8.x",
            "v b k(iPad4,1) r u s",
            "v b k(iPad4,2) bb r u s",
            "v b k(iPad4,3) bb r u s",
            "v b k(iPad5,3) r u s",
            "v b k(iPad5,4) bb r u s"
        ),
        new(
            "9.x",
            "v b k(iPad4,1) r u s",
            "v b k(iPad4,2) bb r u s",
            "v b k(iPad4,3) bb r u s",
            "v b k(iPad5,3) r u s",
            "v b k(iPad5,4) bb r u s"
        ),
        new(
            "10.x",
            "v b k(iPad4,1;iPad4,2;iPad4,3) bb r u s",
            "v b k(iPad5,3;iPad5,4) bb r u s"
        ),
        new(
            "11.x",
            "v b k(iPad4,1;iPad4,2;iPad4,3) bb r u s",
            "v b k(iPad5,3;iPad5,4) bb r u s"
        ),
        new(
            "12.x",
            "v b k(iPad4,1;iPad4,2;iPad4,3) bb r u s",
            "v b k(iPad5,3;iPad5,4) bb r u s",
            "v b k(iPad11,3;iPad11,4) bb r u s"
        ),
        new(
            "13.x",
            "v b k(iPad5,3;iPad5,4) bb r u s",
            "v b k(iPad11,3;iPad11,4) bb r u s"
        ),
        new(
            "14.x",
            "v b k(iPad5,3;iPad5,4) bb r u s",
            "v b k(iPad11,3;iPad11,4) bb r u s",
            "v b k(iPad13,1;iPad13,2) bb r u s"
        ),
        new(
            "15.x",
            "v b k(iPad5,3;iPad5,4) bb r u s",
            "v b k(iPad11,3;iPad11,4) bb r u s",
            "v b k(iPad13,1;iPad13,2) bb r u s",
            "v b k(iPad13,16;iPad13,17) bb r u s"
        )
    );

    private static readonly DeviceEntry IPAD_PRO = new(
        "Firmware/iPad_Pro/",
        new(
            "9.x",
            "v b k(iPad6,7) r u h s d",
            "v b k(iPad6,8) bb r u h s d",
            "v b k(iPad6,3) r u h s d",
            "v b k(iPad6,4) bb r u h s d"
        ),
        new(
            "10.x",
            "v b k(iPad6,7;iPad6,8) bb r u h s d",
            "v b k(iPad6,3;iPad6,4) bb r u h s d",
            "v b k(iPad7,1;iPad7,2) bb r u h s d",
            "v b k(iPad7,3;iPad7,4) bb r u h s d"
        ),
        new(
            "11.x",
            "v b k(iPad6,7;iPad6,8) bb r u h s d",
            "v b k(iPad6,3;iPad6,4) bb r u h s d",
            "v b k(iPad7,1;iPad7,2) bb r u h s d",
            "v b k(iPad7,3;iPad7,4) bb r u h s d"
        ),
        new(
            "12.x",
            "v b k(iPad6,7;iPad6,8) bb r u h s d",
            "v b k(iPad6,3;iPad6,4) bb r u h s d",
            "v b k(iPad7,1;iPad7,2) bb r u h s d",
            "v b k(iPad7,3;iPad7,4) bb r u h s d",
            "v b k(iPad8,1;iPad8,2;iPad8,3;iPad8,4) bb r u h s d",
            "v b k(iPad8,5;iPad8,6;iPad8,7;iPad8,8) bb r u h s d"
        ),
        new(
            "13.x",
            "v b k(iPad6,7;iPad6,8) bb r u h s d",
            "v b k(iPad6,3;iPad6,4) bb r u h s d",
            "v b k(iPad7,1;iPad7,2) bb r u h s d",
            "v b k(iPad7,3;iPad7,4) bb r u h s d",
            "v b k(iPad8,1;iPad8,2;iPad8,3;iPad8,4) bb r u h s d",
            "v b k(iPad8,5;iPad8,6;iPad8,7;iPad8,8) bb r u h s d",
            "v b k(iPad8,9;iPad8,10) bb r u h s d",
            "v b k(iPad8,11;iPad8,12) bb r u h s d"
        ),
        new(
            "14.x",
            "v b k(iPad6,7;iPad6,8) bb r u h s d",
            "v b k(iPad6,3;iPad6,4) bb r u h s d",
            "v b k(iPad7,1;iPad7,2) bb r u h s d",
            "v b k(iPad7,3;iPad7,4) bb r u h s d",
            "v b k(iPad8,1;iPad8,2;iPad8,3;iPad8,4) bb r u h s d",
            "v b k(iPad8,5;iPad8,6;iPad8,7;iPad8,8) bb r u h s d",
            "v b k(iPad8,9;iPad8,10) bb r u h s d",
            "v b k(iPad8,11;iPad8,12) bb r u h s d",
            "v b k(iPad13,4;iPad13,5;iPad13,6;iPad13,7) bb r u h s d",
            "v b k(iPad13,8;iPad13,9;iPad13,10;iPad13,11) bb r u h s d"
        ),
        new(
            "15.x",
            "v b k(iPad6,7;iPad6,8) bb r u h s d",
            "v b k(iPad6,3;iPad6,4) bb r u h s d",
            "v b k(iPad7,1;iPad7,2) bb r u h s d",
            "v b k(iPad7,3;iPad7,4) bb r u h s d",
            "v b k(iPad8,1;iPad8,2;iPad8,3;iPad8,4) bb r u h s d",
            "v b k(iPad8,5;iPad8,6;iPad8,7;iPad8,8) bb r u h s d",
            "v b k(iPad8,9;iPad8,10) bb r u h s d",
            "v b k(iPad8,11;iPad8,12) bb r u h s d",
            "v b k(iPad13,4;iPad13,5;iPad13,6;iPad13,7) bb r u h s d",
            "v b k(iPad13,8;iPad13,9;iPad13,10;iPad13,11) bb r u h s d"
        )
    );

    private static readonly DeviceEntry IPAD_PRO_BETA = new(
        "Beta_Firmware/iPad_Pro",
        new(
            "9.x",
            "v b k(iPad6,7) r u s",
            "v b k(iPad6,8) bb r u s",
            "v b k(iPad6,3) r u s",
            "v b k(iPad6,4) bb r u s"
        ),
        new(
            "10.x",
            "v b k(iPad6,7;iPad6,8) bb r u s",
            "v b k(iPad6,3;iPad6,4) bb r u s",
            "v b k(iPad7,1;iPad7,2) bb r u s",
            "v b k(iPad7,3;iPad7,4) bb r u s"
        ),
        new(
            "11.x",
            "v b k(iPad6,7;iPad6,8) bb r u s",
            "v b k(iPad6,3;iPad6,4) bb r u s",
            "v b k(iPad7,1;iPad7,2) bb r u s",
            "v b k(iPad7,3;iPad7,4) bb r u s"
        ),
        new(
            "12.x",
            "v b k(iPad6,7;iPad6,8) bb r u s",
            "v b k(iPad6,3;iPad6,4) bb r u s",
            "v b k(iPad7,1;iPad7,2) bb r u s",
            "v b k(iPad7,3;iPad7,4) bb r u s",
            "v b k(iPad8,1;iPad8,2;iPad8,3;iPad8,4) bb r u s",
            "v b k(iPad8,5;iPad8,6;iPad8,7;iPad8,8) bb r u s"
        ),
        new(
            "13.x",
            "v b k(iPad6,7;iPad6,8) bb r u s",
            "v b k(iPad6,3;iPad6,4) bb r u s",
            "v b k(iPad7,1;iPad7,2) bb r u s",
            "v b k(iPad7,3;iPad7,4) bb r u s",
            "v b k(iPad8,1;iPad8,2;iPad8,3;iPad8,4) bb r u s",
            "v b k(iPad8,5;iPad8,6;iPad8,7;iPad8,8) bb r u s",
            "v b k(iPad8,9;iPad8,10) bb r u s",
            "v b k(iPad8,11;iPad8,12) bb r u s"
        ),
        new(
            "14.x",
            "v b k(iPad6,7;iPad6,8) bb r u s",
            "v b k(iPad6,3;iPad6,4) bb r u s",
            "v b k(iPad7,1;iPad7,2) bb r u s",
            "v b k(iPad7,3;iPad7,4) bb r u s",
            "v b k(iPad8,1;iPad8,2;iPad8,3;iPad8,4) bb r u s",
            "v b k(iPad8,5;iPad8,6;iPad8,7;iPad8,8) bb r u s",
            "v b k(iPad8,9;iPad8,10) bb r u s",
            "v b k(iPad8,11;iPad8,12) bb r u s",
            "v b k(iPad13,4;iPad13,5;iPad13,6;iPad13,7) bb r u s",
            "v b k(iPad13,8;iPad13,9;iPad13,10;iPad13,11) bb r u s"
        ),
        new(
            "15.x",
            "v b k(iPad6,7;iPad6,8) bb r u s",
            "v b k(iPad6,3;iPad6,4) bb r u s",
            "v b k(iPad7,1;iPad7,2) bb r u s",
            "v b k(iPad7,3;iPad7,4) bb r u s",
            "v b k(iPad8,1;iPad8,2;iPad8,3;iPad8,4) bb r u s",
            "v b k(iPad8,5;iPad8,6;iPad8,7;iPad8,8) bb r u s",
            "v b k(iPad8,9;iPad8,10) bb r u s",
            "v b k(iPad8,11;iPad8,12) bb r u s",
            "v b k(iPad13,4;iPad13,5;iPad13,6;iPad13,7) bb r u s",
            "v b k(iPad13,8;iPad13,9;iPad13,10;iPad13,11) bb r u s"
        )
    );

    private static readonly DeviceEntry IPAD_MINI = new(
        "Firmware/iPad_mini/",
        new(
            "6.x",
            "v b k(iPad2,5) r u h s d",
            "v b k(iPad2,6) bb r u h s d",
            "v b k(iPad2,7) bb r u h s d"
        ),
        new(
            "7.x",
            "v b k(iPad2,5) r u h s d",
            "v b k(iPad2,6) bb r u h s d",
            "v b k(iPad2,7) bb r u h s d",
            "v b k(iPad4,4) r u h s d",
            "v b k(iPad4,5) bb r u h s d",
            "v b k(iPad4,6) bb r u h s d"
        ),
        new(
            "8.x",
            "v b k(iPad2,5) r u h s d",
            "v b k(iPad2,6) bb r u h s d",
            "v b k(iPad2,7) bb r u h s d",
            "v b k(iPad4,4) r u h s d",
            "v b k(iPad4,5) bb r u h s d",
            "v b k(iPad4,6) bb r u h s d",
            "v b k(iPad4,7) r u h s d",
            "v b k(iPad4,8) bb r u h s d",
            "v b k(iPad4,9) bb r u h s d"
        ),
        new(
            "9.x",
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
            "v b k(iPad5,2) bb r u h s d"
        ),
        new(
            "10.x",
            "v b k(iPad4,4;iPad4,5;iPad4,6) bb r u h s d",
            "v b k(iPad4,7;iPad4,8;iPad4,9) bb r u h s d",
            "v b k(iPad5,1;iPad5,2) bb r u h s d"
        ),
        new(
            "11.x",
            "v b k(iPad4,4;iPad4,5;iPad4,6) bb r u h s d",
            "v b k(iPad4,7;iPad4,8;iPad4,9) bb r u h s d",
            "v b k(iPad5,1;iPad5,2) bb r u h s d"
        ),
        new(
            "12.x",
            "v b k(iPad4,4;iPad4,5;iPad4,6) bb r u h s d",
            "v b k(iPad4,7;iPad4,8;iPad4,9) bb r u h s d",
            "v b k(iPad5,1;iPad5,2) bb r u h s d",
            "v b k(iPad11,1;iPad11,2) bb r u h s d"
        ),
        new(
            "13.x",
            "v b k(iPad5,1;iPad5,2) bb r u h s d",
            "v b k(iPad11,1;iPad11,2) bb r u h s d"
        ),
        new(
            "14.x",
            "v b k(iPad5,1;iPad5,2) bb r u h s d",
            "v b k(iPad11,1;iPad11,2) bb r u h s d"
        ),
        new(
            "15.x",
            "v b k(iPad5,1;iPad5,2) bb r u h s d",
            "v b k(iPad11,1;iPad11,2) bb r u h s d",
            "v b k(iPad14,1;iPad14,2) bb r u h s d"
        )
    );

    private static readonly DeviceEntry IPAD_MINI_BETA = new(
        "Beta_Firmware/iPad_mini",
        new(
            "6.x",
            "v b k(iPad2,5) r u i s",
            "v b k(iPad2,6) bb r u i s",
            "v b k(iPad2,7) bb r u i s"
        ),
        new(
            "7.x",
            "v b k(iPad2,5) r u i s",
            "v b k(iPad2,6) bb r u i s",
            "v b k(iPad2,7) bb r u i s",
            "v b k(iPad4,4) r u i s",
            "v b k(iPad4,5) bb r u i s"
        ),
        new(
            "8.x",
            "v b k(iPad2,5) r u i s",
            "v b k(iPad2,6) bb r u i s",
            "v b k(iPad2,7) bb r u i s",
            "v b k(iPad4,4) r u i s",
            "v b k(iPad4,5) bb r u i s",
            "v b k(iPad4,6) bb r u i s",
            "v b k(iPad4,7) r u i s",
            "v b k(iPad4,8) bb r u i s",
            "v b k(iPad4,9) bb r u i s"
        ),
        new(
            "9.x",
            "v b k(iPad2,5) r u i s",
            "v b k(iPad2,6) bb r u i s",
            "v b k(iPad2,7) bb r u i s",
            "v b k(iPad4,4) r u i s",
            "v b k(iPad4,5) bb r u i s",
            "v b k(iPad4,6) bb r u i s",
            "v b k(iPad4,7) r u i s",
            "v b k(iPad4,8) bb r u i s",
            "v b k(iPad4,9) bb r u i s",
            "v b k(iPad5,1) r u i s",
            "v b k(iPad5,2) bb r u i s"
        ),
        new(
            "10.x",
            "v b k(iPad4,4;iPad4,5;iPad4,6) bb r u s",
            "v b k(iPad4,7;iPad4,8;iPad4,9) bb r u s",
            "v b k(iPad5,1;iPad5,2) bb r u s"
        ),
        new(
            "11.x",
            "v b k(iPad4,4;iPad4,5;iPad4,6) bb r u s",
            "v b k(iPad4,7;iPad4,8;iPad4,9) bb r u s",
            "v b k(iPad5,1;iPad5,2) bb r u s"
        ),
        new(
            "12.x",
            "v b k(iPad4,4;iPad4,5;iPad4,6) bb r u s",
            "v b k(iPad4,7;iPad4,8;iPad4,9) bb r u s",
            "v b k(iPad5,1;iPad5,2) bb r u s",
            "v b k(iPad11,1;iPad11,2) bb r u s"
        ),
        new(
            "13.x",
            "v b k(iPad5,1;iPad5,2) bb r u s",
            "v b k(iPad11,1;iPad11,2) bb r u s"
        ),
        new(
            "14.x",
            "v b k(iPad5,1;iPad5,2) bb r u s",
            "v b k(iPad11,1;iPad11,2) bb r u s"
        ),
        new(
            "15.x",
            "v b k(iPad5,1;iPad5,2) bb r u s",
            "v b k(iPad11,1;iPad11,2) bb r u s",
            "v b k(iPad14,1;iPad14,2) bb r u s"
        )
    );

    private static readonly DeviceEntry IPHONE = new(
        "Firmware/iPhone/",
        new("1.x", "v b k(iPhone1,1) bb r u h s d"),
        new(
            "2.x",
            "v b k(iPhone1,1) bb r u h s d",
            "v b k(iPhone1,2) bb r u h s d"
        ),
        new(
            "3.x",
            "v b k(iPhone1,1) bb r u h s d",
            "v b k(iPhone1,2) bb r u h s d",
            "v b k(iPhone2,1) bb r u h s d"
        ),
        new(
            "4.x",
            "v b k(iPhone1,2) bb r u h s d",
            "v b k(iPhone2,1) bb r u h s d",
            "v b k(iPhone3,1) bb r u h s d",
            "v b k(iPhone3,3) bb r u h s d"
        ),
        new(
            "5.x",
            "v b k(iPhone2,1) bb r u h s d",
            "v b k(iPhone3,1) bb r u h s d",
            "v b k(iPhone3,3) bb r u h s d",
            "v b k(iPhone4,1) bb r u h s d"
        ),
        new(
            "6.x",
            "v b k(iPhone2,1) bb r u h s d",
            "v b k(iPhone3,1) bb r u h s d",
            "v b k(iPhone3,2) bb r u h s d",
            "v b k(iPhone3,3) bb r u h s d",
            "v b k(iPhone4,1) bb r u h s d",
            "v b k(iPhone5,1) bb r u h s d",
            "v b k(iPhone5,2) bb r u h s d"
        ),
        new(
            "7.x",
            "v b k(iPhone3,1) bb r u h s d",
            "v b k(iPhone3,2) bb r u h s d",
            "v b k(iPhone3,3) bb r u h s d",
            "v b k(iPhone4,1) bb r u h s d",
            "v b k(iPhone5,1) bb r u h s d",
            "v b k(iPhone5,2) bb r u h s d",
            "v b k(iPhone5,3) bb r u h s d",
            "v b k(iPhone5,4) bb r u h s d",
            "v b k(iPhone6,1) bb r u h s d",
            "v b k(iPhone6,2) bb r u h s d"
        ),
        new(
            "8.x",
            "v b k(iPhone4,1) bb r u h s d",
            "v b k(iPhone5,1) bb r u h s d",
            "v b k(iPhone5,2) bb r u h s d",
            "v b k(iPhone5,3) bb r u h s d",
            "v b k(iPhone5,4) bb r u h s d",
            "v b k(iPhone6,1) bb r u h s d",
            "v b k(iPhone6,2) bb r u h s d",
            "v b k(iPhone7,2) bb r u h s d",
            "v b k(iPhone7,1) bb r u h s d"
        ),
        new(
            "9.x",
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
            "v b k(iPhone8,4) bb r u h s d"
        ),
        new(
            "10.x",
            "v b k(iPhone5,1;iPhone5,2) bb r u h s d",
            "v b k(iPhone5,3;iPhone5,4) bb r u h s d",
            "v b k(iPhone6,1;iPhone6,2) bb r u h s d",
            "v b k(iPhone7,2) bb r u h s d",
            "v b k(iPhone7,1) bb r u h s d",
            "v b k(iPhone8,1) bb r u h s d",
            "v b k(iPhone8,2) bb r u h s d",
            "v b k(iPhone8,4) bb r u h s d",
            "v b k(iPhone9,1;iPhone9,3) bb(iPhone9,1) bb(iPhone9,3) r u h s d",
            "v b k(iPhone9,2;iPhone9,4) bb(iPhone9,2) bb(iPhone9,4) r u h s d"
        ),
        new(
            "11.x",
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
            "v b k(iPhone10,3;iPhone10,6) bb(iPhone10,3) bb(iPhone10,6) r u h s d"
        ),
        new(
            "12.x",
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
            "v b k(iPhone11,4;iPhone11,6) bb r u h s d"
        ),
        new(
            "13.x",
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
            "v b k(iPhone12,8) bb r u h s d"
        ),
        new(
            "14.x",
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
            "v b k(iPhone13,4) bb r u h s d"
        ),
        new(
            "15.x",
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
            "v b k(iPhone14,6) bb r u h s d"
        )
    );

    private static readonly DeviceEntry IPHONE_BETA = new(
        "Beta_Firmware/iPhone/",
        new("1.x", "v b k(iPhone1,1) bb r u s"),
        new(
            "2.x",
            "v b k(iPhone1,1) bb r u s",
            "v b k(iPhone1,2) bb r u s"
        ),
        new(
            "3.x",
            "v b k(iPhone1,1) bb r u s",
            "v b k(iPhone1,2) bb r u s",
            "v b k(iPhone2,1) bb r u s"
        ),
        new(
            "4.x",
            "v b k(iPhone1,2) bb r u s",
            "v b k(iPhone2,1) bb r u s",
            "v b k(iPhone3,1) bb r u s"
        ),
        new(
            "5.x",
            "v b k(iPhone2,1) bb r u s",
            "v b k(iPhone3,1) bb r u s",
            "v b k(iPhone3,3) bb r u s",
            "v b k(iPhone4,1) bb r u s"
        ),
        new(
            "6.x",
            "v b k(iPhone2,1) bb r u s",
            "v b k(iPhone3,1) bb r u s",
            "v b k(iPhone3,2) bb r u s",
            "v b k(iPhone3,3) bb r u s",
            "v b k(iPhone4,1) bb r u s",
            "v b k(iPhone5,1) bb r u s",
            "v b k(iPhone5,2) bb r u s"
        ),
        new(
            "7.x",
            "v b k(iPhone3,1) bb r u s",
            "v b k(iPhone3,2) bb r u s",
            "v b k(iPhone3,3) bb r u s",
            "v b k(iPhone4,1) bb r u s",
            "v b k(iPhone5,1) bb r u s",
            "v b k(iPhone5,2) bb r u s",
            "v b k(iPhone5,3) bb r u s",
            "v b k(iPhone5,4) bb r u s",
            "v b k(iPhone6,1) bb r u s",
            "v b k(iPhone6,2) bb r u s"
        ),
        new(
            "8.x",
            "v b k(iPhone4,1) bb r u s",
            "v b k(iPhone5,1) bb r u s",
            "v b k(iPhone5,2) bb r u s",
            "v b k(iPhone5,3) bb r u s",
            "v b k(iPhone5,4) bb r u s",
            "v b k(iPhone6,1) bb r u s",
            "v b k(iPhone6,2) bb r u s",
            "v b k(iPhone7,2) bb r u s",
            "v b k(iPhone7,1) bb r u s"
        ),
        new(
            "9.x",
            "v b k(iPhone4,1) bb r u s",
            "v b k(iPhone5,1) bb r u s",
            "v b k(iPhone5,2) bb r u s",
            "v b k(iPhone5,3) bb r u s",
            "v b k(iPhone5,4) bb r u s",
            "v b k(iPhone6,1) bb r u s",
            "v b k(iPhone6,2) bb r u s",
            "v b k(iPhone7,2) bb r u s",
            "v b k(iPhone7,1) bb r u s",
            "v b k(iPhone8,1) bb r u s",
            "v b k(iPhone8,2) bb r u s",
            "v b k(iPhone8,4) bb r u s"
        ),
        new(
            "10.x",
            "v b k(iPhone5,1;iPhone5,2) bb r u s",
            "v b k(iPhone5,3;iPhone5,4) bb r u s",
            "v b k(iPhone6,1;iPhone6,2) bb r u s",
            "v b k(iPhone7,2) bb r u s",
            "v b k(iPhone7,1) bb r u s",
            "v b k(iPhone8,1) bb r u s",
            "v b k(iPhone8,2) bb r u s",
            "v b k(iPhone8,4) bb r u s",
            "v b k(iPhone9,1;iPhone9,3) bb(iPhone9,1) bb(iPhone9,3) r u s",
            "v b k(iPhone9,2;iPhone9,4) bb(iPhone9,2) bb(iPhone9,4) r u s"
        ),
        new(
            "11.x",
            "v b k(iPhone6,1;iPhone6,2) bb r u s",
            "v b k(iPhone7,2) bb r u s",
            "v b k(iPhone7,1) bb r u s",
            "v b k(iPhone8,1) bb r u s",
            "v b k(iPhone8,2) bb r u s",
            "v b k(iPhone8,4) bb r u s",
            "v b k(iPhone9,1;iPhone9,3) bb(iPhone9,1) bb(iPhone9,3) r u s",
            "v b k(iPhone9,2;iPhone9,4) bb(iPhone9,2) bb(iPhone9,4) r u s",
            "v b k(iPhone10,1;iPhone10,4) bb(iPhone10,1) bb(iPhone10,4) r u s",
            "v b k(iPhone10,2;iPhone10,5) bb(iPhone10,2) bb(iPhone10,5) r u s",
            "v b k(iPhone10,3;iPhone10,6) bb(iPhone10,3) bb(iPhone10,6) r u s"
        ),
        new(
            "12.x",
            "v b k(iPhone6,1;iPhone6,2) bb r u s",
            "v b k(iPhone7,2) bb r u s",
            "v b k(iPhone7,1) bb r u s",
            "v b k(iPhone8,1) bb r u s",
            "v b k(iPhone8,2) bb r u s",
            "v b k(iPhone8,4) bb r u s",
            "v b k(iPhone9,1;iPhone9,3) bb(iPhone9,1) bb(iPhone9,3) r u s",
            "v b k(iPhone9,2;iPhone9,4) bb(iPhone9,2) bb(iPhone9,4) r u s",
            "v b k(iPhone10,1;iPhone10,4) bb(iPhone10,1) bb(iPhone10,4) r u s",
            "v b k(iPhone10,2;iPhone10,5) bb(iPhone10,2) bb(iPhone10,5) r u s",
            "v b k(iPhone10,3;iPhone10,6) bb(iPhone10,3) bb(iPhone10,6) r u s",
            "v b k(iPhone11,8) bb r u s",
            "v b k(iPhone11,2) bb r u s",
            "v b k(iPhone11,6) bb r u s"
        ),
        new(
            "13.x",
            "v b k(iPhone8,1) bb r u s",
            "v b k(iPhone8,2) bb r u s",
            "v b k(iPhone8,4) bb r u s",
            "v b k(iPhone9,1;iPhone9,3) bb(iPhone9,1) bb(iPhone9,3) r u s",
            "v b k(iPhone9,2;iPhone9,4) bb(iPhone9,2) bb(iPhone9,4) r u s",
            "v b k(iPhone10,1;iPhone10,4) bb(iPhone10,1) bb(iPhone10,4) r u s",
            "v b k(iPhone10,2;iPhone10,5) bb(iPhone10,2) bb(iPhone10,5) r u s",
            "v b k(iPhone10,3;iPhone10,6) bb(iPhone10,3) bb(iPhone10,6) r u s",
            "v b k(iPhone11,8) bb r u s",
            "v b k(iPhone11,2) bb r u s",
            "v b k(iPhone11,4;iPhone11,6) bb r u s",
            "v b k(iPhone12,1) bb r u s",
            "v b k(iPhone12,3) bb r u s",
            "v b k(iPhone12,5) bb r u s",
            "v b k(iPhone12,8) bb r u s"
        ),
        new(
            "14.x",
            "v b k(iPhone8,1) bb r u s",
            "v b k(iPhone8,2) bb r u s",
            "v b k(iPhone8,4) bb r u s",
            "v b k(iPhone9,1;iPhone9,3) bb(iPhone9,1) bb(iPhone9,3) r u s",
            "v b k(iPhone9,2;iPhone9,4) bb(iPhone9,2) bb(iPhone9,4) r u s",
            "v b k(iPhone10,1;iPhone10,4) bb(iPhone10,1) bb(iPhone10,4) r u s",
            "v b k(iPhone10,2;iPhone10,5) bb(iPhone10,2) bb(iPhone10,5) r u s",
            "v b k(iPhone10,3;iPhone10,6) bb(iPhone10,3) bb(iPhone10,6) r u s",
            "v b k(iPhone11,8) bb r u s",
            "v b k(iPhone11,2) bb r u s",
            "v b k(iPhone11,4;iPhone11,6) bb r u s",
            "v b k(iPhone12,1) bb r u s",
            "v b k(iPhone12,3) bb r u s",
            "v b k(iPhone12,5) bb r u s",
            "v b k(iPhone12,8) bb r u s",
            "v b k(iPhone13,1) bb r u s",
            "v b k(iPhone13,2) bb r u s",
            "v b k(iPhone13,3) bb r u s",
            "v b k(iPhone13,4) bb r u s"
        ),
        new(
            "15.x",
            "v b k(iPhone8,1) bb r u s",
            "v b k(iPhone8,2) bb r u s",
            "v b k(iPhone8,4) bb r u s",
            "v b k(iPhone9,1;iPhone9,3) bb(iPhone9,1) bb(iPhone9,3) r u s",
            "v b k(iPhone9,2;iPhone9,4) bb(iPhone9,2) bb(iPhone9,4) r u s",
            "v b k(iPhone10,1;iPhone10,4) bb(iPhone10,1) bb(iPhone10,4) r u s",
            "v b k(iPhone10,2;iPhone10,5) bb(iPhone10,2) bb(iPhone10,5) r u s",
            "v b k(iPhone10,3;iPhone10,6) bb(iPhone10,3) bb(iPhone10,6) r u s",
            "v b k(iPhone11,8) bb r u s",
            "v b k(iPhone11,2) bb r u s",
            "v b k(iPhone11,4;iPhone11,6) bb r u s",
            "v b k(iPhone12,1) bb r u s",
            "v b k(iPhone12,3) bb r u s",
            "v b k(iPhone12,5) bb r u s",
            "v b k(iPhone12,8) bb r u s",
            "v b k(iPhone13,1) bb r u s",
            "v b k(iPhone13,2) bb r u s",
            "v b k(iPhone13,3) bb r u s",
            "v b k(iPhone13,4) bb r u s",
            "v b k(iPhone14,4) bb r u s",
            "v b k(iPhone14,5) bb r u s",
            "v b k(iPhone14,2) bb r u s",
            "v b k(iPhone14,3) bb r u s",
            "v b k(iPhone14,6) bb r u s"
        )
    );

    private static readonly DeviceEntry IPOD_TOUCH = new(
        "Firmware/iPod_touch/",
        new("1.x", "v b k(iPod1,1) r u h s d"),
        new(
            "2.x",
            "v b k(iPod1,1) r i h s d",
            "v b k(iPod2,1) r u h s d"
        ),
        new(
            "3.x",
            "v b k(iPod1,1) r i h s d",
            "v b k(iPod2,1) r i h s d",
            "v b k(iPod3,1) r u h s d"
        ),
        new(
            "4.x",
            "v b k(iPod2,1) r u h s d",
            "v b k(iPod3,1) r u h s d",
            "v b k(iPod4,1) r u h s d"
        ),
        new(
            "5.x",
            "v b k(iPod3,1) r u h s d",
            "v b k(iPod4,1) r u h s d"
        ),
        new(
            "6.x",
            "v b k(iPod4,1) r u h s d",
            "v b k(iPod5,1) r u h s d"
        ),
        new("7.x", "v b k(iPod5,1) r u h s d"),
        new(
            "8.x",
            "v b k(iPod5,1) r u h s d",
            "v b k(iPod7,1) r u h s d"
        ),
        new(
            "9.x",
            "v b k(iPod5,1) r u h s d",
            "v b k(iPod7,1) r u h s d"
        ),
        new("10.x", "v b k(iPod7,1) r u h s d"),
        new("11.x", "v b k(iPod7,1) r u h s d"),
        new(
            "12.x",
            "v b k(iPod7,1) r u h s d",
            "v b k(iPod9,1) r u h s d"
        ),
        new("13.x", "v b k(iPod9,1) r u h s d"),
        new("14.x", "v b k(iPod9,1) r u h s d"),
        new("15.x", "v b k(iPod9,1) r u h s d")
    );

    private static readonly DeviceEntry IPOD_TOUCH_BETA = new(
        "Beta_Firmware/iPod_touch/",
        new("1.x", "v b k(iPod1,1) r u s"),
        new("2.x", "v b k(iPod1,1) r i s"),
        new(
            "3.x",
            "v b k(iPod1,1) r i s",
            "v b k(iPod2,1) r i s"
        ),
        new(
            "4.x",
            "v b k(iPod2,1) r u s",
            "v b k(iPod3,1) r u s",
            "v b k(iPod4,1) r u s"
        ),
        new(
            "5.x",
            "v b k(iPod3,1) r u s",
            "v b k(iPod4,1) r u s"
        ),
        new(
            "6.x",
            "v b k(iPod4,1) r u s",
            "v b k(iPod5,1) r u s"
        ),
        new("7.x", "v b k(iPod5,1) r u s"),
        new(
            "8.x",
            "v b k(iPod5,1) r u s",
            "v b k(iPod7,1) r u s"
        ),
        new(
            "9.x",
            "v b k(iPod5,1) r u s",
            "v b k(iPod7,1) r u s"
        ),
        new("10.x", "v b k(iPod7,1) r u s"),
        new("11.x", "v b k(iPod7,1) r u s"),
        new(
            "12.x",
            "v b k(iPod7,1) r u s",
            "v b k(iPod9,1) r u s"
        ),
        new("13.x", "v b k(iPod9,1) r u s"),
        new("14.x", "v b k(iPod9,1) r u s"),
        new("15.x", "v b k(iPod9,1) r u s")
    );

    public static readonly DeviceEntry[] ALL_DESCRIPTORS = new[]
    {
        APPLE_TV,
        APPLE_WATCH,
        HOME_POD,
        // APPLE_SILICON,
        // ReSharper disable once CommentTypo
        // IBRIDGE,
        IPAD,
        IPAD_AIR,
        IPAD_PRO,
        IPAD_MINI,
        IPHONE,
        IPOD_TOUCH,

        APPLE_TV_BETA,
        APPLE_WATCH_BETA,
        HOME_POD_BETA,
        // APPLE_SILICON_BETA,
        // ReSharper disable once CommentTypo
        // IBRIDGE_BETA,
        IPAD_BETA,
        IPAD_AIR_BETA,
        IPAD_PRO_BETA,
        IPAD_MINI_BETA,
        IPHONE_BETA,
        IPOD_TOUCH_BETA,
    };
}
