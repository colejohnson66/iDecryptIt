/* =============================================================================
 * File:   Descriptors.cs
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

using System.Collections.Generic;

namespace KeyGrabber;

public static class Descriptors
{
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

    /// <summary><c>Firmware/Apple_TV/???</c></summary>
    private static readonly Dictionary<string, string[]> APPLE_TV_FW_PAGES = new()
    {
        { "4.x", new[] { "vm v bm b k(AppleTV2,1) r u h s d" } },
        {
            "5.x", new[]
            {
                "vm v bm b k(AppleTV2,1) r u h s d",
                "vm v bm b k(AppleTV3,1) r u h s d",
                "vm v bm b k(AppleTV3,2) r u h s d",
            }
        },
        {
            "6.x", new[]
            {
                "vm v bm b k(AppleTV2,1) r u h s d",
                "vm v bm b k(AppleTV3,1) r u h s d",
                "vm v bm b k(AppleTV3,2) r u h s d",
            }
        },
        {
            "7.x", new[]
            {
                "vm v bm b k(AppleTV3,1) r u h s d",
                "vm v bm b k(AppleTV3,2) r u h s d",
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

    /// <summary><c>Beta_Firmware/Apple_TV/???</c></summary>
    private static readonly Dictionary<string, string[]> APPLE_TV_BETA_FW_PAGES = new()
    {
        { "4.x", new[] { "vm v bm b k(AppleTV2,1) r u s" } },
        {
            "5.x", new[]
            {
                "vm v bm b k(AppleTV2,1) r u s",
                "vm v bm b k(AppleTV3,1) r u s",
                "vm v bm b k(AppleTV3,2) r u s",
            }
        },
        {
            "6.x", new[]
            {
                "vm v bm b k(AppleTV2,1) r u s",
                "vm v bm b k(AppleTV3,1) r u s",
                "vm v bm b k(AppleTV3,2) r u s",
            }
        },
        {
            "7.x", new[]
            {
                "vm v bm b k(AppleTV3,1) r u s",
                "vm v bm b k(AppleTV3,2) r u s",
            }
        },
        { "9.x", new[] { "v b k(AppleTV5,3) r u s" } },
        { "10.x", new[] { "v b k(AppleTV5,3) r u s" } },
        {
            "11.x", new[]
            {
                "v b k(AppleTV5,3) r u s",
                "v b k(AppleTV6,2) r",
            }
        },
        {
            "12.x", new[]
            {
                "v b k(AppleTV5,3) r u s",
                "v b k(AppleTV6,2) r",
            }
        },
        {
            "13.x", new[]
            {
                "v b k(AppleTV5,3) r u s",
                "v b k(AppleTV6,2) r",
            }
        },
        {
            "14.x", new[]
            {
                "v b k(AppleTV5,3) r u s",
                "v b k(AppleTV6,2) r",
                "v b k(AppleTV11,1) r",
            }
        },
        {
            "15.x", new[]
            {
                "v b k(AppleTV5,3) r u s",
                "v b k(AppleTV6,2) r",
                "v b k(AppleTV11,1) r",
            }
        },
    };

    /// <summary><c>Firmware/Apple_Watch/???</c></summary>
    private static readonly Dictionary<string, string[]> APPLE_WATCH_FW_PAGES = new()
    {
        {
            "1.x", new[]
            {
                "vm v b k(Watch1,1) r d",
                "vm v b k(Watch1,2) r d",
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
                "vm v b k(Watch1,1) r d",
                "vm v b k(Watch1,2) r d",
            }
        },
        {
            "3.x", new[]
            {
                "vm v b k(Watch1,1) r d",
                "vm v b k(Watch1,2) r d",
                "vm v b k(Watch2,6) r d",
                "vm v b k(Watch2,7) r d",
                "vm v b k(Watch2,3) r d",
                "vm v b k(Watch2,4) r d",
            }
        },
        {
            "4.x", new[]
            {
                "vm v b k(Watch1,1) r d",
                "vm v b k(Watch1,2) r d",
                "vm v b k(Watch2,6) r d",
                "vm v b k(Watch2,7) r d",
                "vm v b k(Watch2,3) r d",
                "vm v b k(Watch2,4) r d",
                "vm v b k(Watch3,1) bb r d",
                "vm v b k(Watch3,2) bb r d",
                "vm v b k(Watch3,3) r d",
                "vm v b k(Watch3,4) r d",
            }
        },
        {
            "5.x", new[]
            {
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
                "vm v b k(Watch4,4) bb r d",
            }
        },
        {
            "6.x", new[]
            {
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
                "vm v b k(Watch5,4) bb r d",
            }
        },
        {
            "7.x", new[]
            {
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
            }
        },
        {
            "8.x", new[]
            {
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
                "vm v b k(Watch6,9) bb r u h s d",
            }
        },
    };

    /// <summary><c>Beta_Firmware/Apple_Watch/???</c></summary>
    private static readonly Dictionary<string, string[]> APPLE_WATCH_BETA_FW_PAGES = new()
    {
        {
            "2.x", new[]
            {
                "vm v b k(Watch1,1) r",
                "vm v b k(Watch1,2) r",
            }
        },
        {
            "3.x", new[]
            {
                "vm v b k(Watch1,1) r",
                "vm v b k(Watch1,2) r",
                "vm v b k(Watch2,6) r",
                "vm v b k(Watch2,7) r",
                "vm v b k(Watch2,3) r",
                "vm v b k(Watch2,4) r",
            }
        },
        {
            "4.x", new[]
            {
                "vm v b k(Watch1,1) r",
                "vm v b k(Watch1,2) r",
                "vm v b k(Watch2,6) r",
                "vm v b k(Watch2,7) r",
                "vm v b k(Watch2,3) r",
                "vm v b k(Watch2,4) r",
                "vm v b k(Watch3,1) bb r",
                "vm v b k(Watch3,2) bb r",
                "vm v b k(Watch3,3) r",
                "vm v b k(Watch3,4) r",
            }
        },
        {
            "5.x", new[]
            {
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
            }
        },
        {
            "6.x", new[]
            {
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
                "vm v b k(Watch5,4) bb r",
            }
        },
        {
            "7.x", new[]
            {
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
            }
        },
        {
            "8.x", new[]
            {
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
                "vm v b k(Watch6,9) bb r",
            }
        },
    };

    /// <summary><c>Firmware/HomePod/???</c></summary>
    private static readonly Dictionary<string, string[]> HOME_POD_FW_PAGES = new()
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

    /// <summary><c>Beta_Firmware/HomePod/???</c></summary>
    private static readonly Dictionary<string, string[]> HOME_POD_BETA_FW_PAGES = new()
    {
        { "11.x", new[] { "v b k(AudioAccessory1,1) r" } },
        {
            "14.x", new[]
            {
                "v b k(AudioAccessory1,1;AudioAccessory1,2) r",
                "v b k(AudioAccessory5,1) r",
            }
        },
        {
            "15.x", new[]
            {
                "v b k(AudioAccessory1,1;AudioAccessory1,2) r",
                "v b k(AudioAccessory5,1) r",
            }
        },
    };

    /// <summary><c>Firmware/Mac/???</c></summary>
    private static readonly Dictionary<string, string[]> APPLE_SILICON_FW_PAGES = new()
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

    /// <summary><c>Beta_Firmware/Mac/???</c></summary>
    private static readonly Dictionary<string, string[]> APPLE_SILICON_BETA_FW_PAGES = new()
    {
        {
            "11.x", new[]
            {
                "v b k(ADP3,2) r",
                "v b k(MacBookAir10,1) r u s",
                "v b k(MacBookPro17,1) r u s",
                // ReSharper disable once StringLiteralTypo
                "v b k(Macmini9,1) r u s",
                "v b k(iMac21,1;iMac21,2) r u s",
            }
        },
        {
            "12.x", new[]
            {
                "v b k(MacBookAir10,1) r u s",
                "v b k(MacBookPro17,1) r u s",
                // ReSharper disable once StringLiteralTypo
                "v b k(Macmini9,1) r u s",
                "v b k(iMac21,1;iMac21,2) r u s",
            }
        },
    };

    /// <summary><c>Firmware/iBridge/???</c></summary>
    private static readonly Dictionary<string, string[]> IBRIDGE_FW_PAGES = new()
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

    // TODO: iBridge betas?

    /// <summary><c>Firmware/iPad/???</c></summary>
    private static readonly Dictionary<string, string[]> IPAD_FW_PAGES = new()
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
                "v b k(iPad3,4;iPad3,5;iPad3,6) bb r u h s d",
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
                "v b k(iPad11,6;iPad11,7) bb r u h s d",
            }
        },
        {
            "15.x", new[]
            {
                "v b k(iPad6,11;iPad6,12) bb r u h s d",
                "v b k(iPad7,5;iPad7,6) bb r u h s d",
                "v b k(iPad7,11;iPad7,12) bb r u h s d",
                "v b k(iPad11,6;iPad11,7) bb r u h s d",
                "v b k(iPad12,1;iPad12,2) bb r u h s d",
            }
        },
    };

    /// <summary><c>Beta_Firmware/iPad/???</c></summary>
    private static readonly Dictionary<string, string[]> IPAD_BETA_FW_PAGES = new()
    {
        {
            "4.x", new[] { "v b k(iPad1,1) bb r u s" }
        },
        {
            "5.x", new[]
            {
                "v b k(iPad1,1) bb r u s",
                "v b k(iPad2,1) r u s",
                "v b k(iPad2,2) bb r u s",
                "v b k(iPad2,3) bb r u s",
            }
        },
        {
            "6.x", new[]
            {
                "v b k(iPad2,1) r u s",
                "v b k(iPad2,2) bb r u s",
                "v b k(iPad2,3) bb r u s",
                "v b k(iPad2,4) r u s",
                "v b k(iPad3,1) r u s",
                "v b k(iPad3,2) bb r u s",
                "v b k(iPad3,3) bb r u s",
                "v b k(iPad3,4) r u s",
                "v b k(iPad3,5) bb r u s",
                "v b k(iPad3,6) bb r u s",
            }
        },
        {
            "7.x", new[]
            {
                "v b k(iPad2,1) r u s",
                "v b k(iPad2,2) bb r u s",
                "v b k(iPad2,3) bb r u s",
                "v b k(iPad2,4) r u s",
                "v b k(iPad3,1) r u s",
                "v b k(iPad3,2) bb r u s",
                "v b k(iPad3,3) bb r u s",
                "v b k(iPad3,4) r u s",
                "v b k(iPad3,5) bb r u s",
                "v b k(iPad3,6) bb r u s",
            }
        },
        {
            "8.x", new[]
            {
                "v b k(iPad2,1) r u s",
                "v b k(iPad2,2) bb r u s",
                "v b k(iPad2,3) bb r u s",
                "v b k(iPad2,4) r u s",
                "v b k(iPad3,1) r u s",
                "v b k(iPad3,2) bb r u s",
                "v b k(iPad3,3) bb r u s",
                "v b k(iPad3,4) r u s",
                "v b k(iPad3,5) bb r u s",
                "v b k(iPad3,6) bb r u s",
            }
        },
        {
            "9.x", new[]
            {
                "v b k(iPad2,1) r u s",
                "v b k(iPad2,2) bb r u s",
                "v b k(iPad2,3) bb r u s",
                "v b k(iPad2,4) r u s",
                "v b k(iPad3,1) r u s",
                "v b k(iPad3,2) bb r u s",
                "v b k(iPad3,3) bb r u s",
                "v b k(iPad3,4) r u s",
                "v b k(iPad3,5) bb r u s",
                "v b k(iPad3,6) bb r u s",
            }
        },
        {
            "10.x", new[]
            {
                "v b k(iPad3,4;iPad3,5;iPad3,6) bb r u s",
                "v b k(iPad6,11;iPad6,12) bb r u s",
            }
        },
        {
            "11.x", new[]
            {
                "v b k(iPad6,11;iPad6,12) bb r u s",
                "v b k(iPad7,5;iPad7,6) bb r u s",
            }
        },
        {
            "12.x", new[]
            {
                "v b k(iPad6,11;iPad6,12) bb r u s",
                "v b k(iPad7,5;iPad7,6) bb r u s",
            }
        },
        {
            "13.x", new[]
            {
                "v b k(iPad6,11;iPad6,12) bb r u s",
                "v b k(iPad7,5;iPad7,6) bb r u s",
                "v b k(iPad7,11;iPad7,12) bb r u s",
            }
        },
        {
            "14.x", new[]
            {
                "v b k(iPad6,11;iPad6,12) bb r u s",
                "v b k(iPad7,5;iPad7,6) bb r u s",
                "v b k(iPad7,11;iPad7,12) bb r u s",
                "v b k(iPad11,6;iPad11,7) bb r u s",
            }
        },
        {
            "15.x", new[]
            {
                "v b k(iPad6,11;iPad6,12) bb r u s",
                "v b k(iPad7,5;iPad7,6) bb r u s",
                "v b k(iPad7,11;iPad7,12) bb r u s",
                "v b k(iPad11,6;iPad11,7) bb r u s",
                "v b k(iPad12,1;iPad12,2) bb r u s",
            }
        },
    };

    /// <summary><c>Firmware/iPad_Air/???</c></summary>
    private static readonly Dictionary<string, string[]> IPAD_AIR_FW_PAGES = new()
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
                "v b k(iPad13,16;iPad13,17) bb r u h s d",
            }
        },
    };

    /// <summary><c>Beta_Firmware/iPad_Air/???</c></summary>
    private static readonly Dictionary<string, string[]> IPAD_AIR_BETA_FW_PAGES = new()
    {
        {
            "7.x", new[]
            {
                "v b k(iPad4,1) r u s",
                "v b k(iPad4,2) bb r u s",
            }
        },
        {
            "8.x", new[]
            {
                "v b k(iPad4,1) r u s",
                "v b k(iPad4,2) bb r u s",
                "v b k(iPad4,3) bb r u s",
                "v b k(iPad5,3) r u s",
                "v b k(iPad5,4) bb r u s",
            }
        },
        {
            "9.x", new[]
            {
                "v b k(iPad4,1) r u s",
                "v b k(iPad4,2) bb r u s",
                "v b k(iPad4,3) bb r u s",
                "v b k(iPad5,3) r u s",
                "v b k(iPad5,4) bb r u s",
            }
        },
        {
            "10.x", new[]
            {
                "v b k(iPad4,1;iPad4,2;iPad4,3) bb r u s",
                "v b k(iPad5,3;iPad5,4) bb r u s",
            }
        },
        {
            "11.x", new[]
            {
                "v b k(iPad4,1;iPad4,2;iPad4,3) bb r u s",
                "v b k(iPad5,3;iPad5,4) bb r u s",
            }
        },
        {
            "12.x", new[]
            {
                "v b k(iPad4,1;iPad4,2;iPad4,3) bb r u s",
                "v b k(iPad5,3;iPad5,4) bb r u s",
                "v b k(iPad11,3;iPad11,4) bb r u s",
            }
        },
        {
            "13.x", new[]
            {
                "v b k(iPad5,3;iPad5,4) bb r u s",
                "v b k(iPad11,3;iPad11,4) bb r u s",
            }
        },
        {
            "14.x", new[]
            {
                "v b k(iPad5,3;iPad5,4) bb r u s",
                "v b k(iPad11,3;iPad11,4) bb r u s",
                "v b k(iPad13,1;iPad13,2) bb r u s",
            }
        },
        {
            "15.x", new[]
            {
                "v b k(iPad5,3;iPad5,4) bb r u s",
                "v b k(iPad11,3;iPad11,4) bb r u s",
                "v b k(iPad13,1;iPad13,2) bb r u s",
				"v b k(iPad13,16;iPad13,17) bb r u s",
            }
        },
    };

    /// <summary><c>Firmware/iPad_Pro/???</c></summary>
    private static readonly Dictionary<string, string[]> IPAD_PRO_FW_PAGES = new()
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

    /// <summary><c>Beta_Firmware/iPad_Pro/???</c></summary>
    private static readonly Dictionary<string, string[]> IPAD_PRO_BETA_FW_PAGES = new()
    {
        {
            "9.x", new[]
            {
                "v b k(iPad6,7) r u s",
                "v b k(iPad6,8) bb r u s",
                "v b k(iPad6,3) r u s",
                "v b k(iPad6,4) bb r u s",
            }
        },
        {
            "10.x", new[]
            {
                "v b k(iPad6,7;iPad6,8) bb r u s",
                "v b k(iPad6,3;iPad6,4) bb r u s",
                "v b k(iPad7,1;iPad7,2) bb r u s",
                "v b k(iPad7,3;iPad7,4) bb r u s",
            }
        },
        {
            "11.x", new[]
            {
                "v b k(iPad6,7;iPad6,8) bb r u s",
                "v b k(iPad6,3;iPad6,4) bb r u s",
                "v b k(iPad7,1;iPad7,2) bb r u s",
                "v b k(iPad7,3;iPad7,4) bb r u s",
            }
        },
        {
            "12.x", new[]
            {
                "v b k(iPad6,7;iPad6,8) bb r u s",
                "v b k(iPad6,3;iPad6,4) bb r u s",
                "v b k(iPad7,1;iPad7,2) bb r u s",
                "v b k(iPad7,3;iPad7,4) bb r u s",
                "v b k(iPad8,1;iPad8,2;iPad8,3;iPad8,4) bb r u s",
                "v b k(iPad8,5;iPad8,6;iPad8,7;iPad8,8) bb r u s",
            }
        },
        {
            "13.x", new[]
            {
                "v b k(iPad6,7;iPad6,8) bb r u s",
                "v b k(iPad6,3;iPad6,4) bb r u s",
                "v b k(iPad7,1;iPad7,2) bb r u s",
                "v b k(iPad7,3;iPad7,4) bb r u s",
                "v b k(iPad8,1;iPad8,2;iPad8,3;iPad8,4) bb r u s",
                "v b k(iPad8,5;iPad8,6;iPad8,7;iPad8,8) bb r u s",
                "v b k(iPad8,9;iPad8,10) bb r u s",
                "v b k(iPad8,11;iPad8,12) bb r u s",
            }
        },
        {
            "14.x", new[]
            {
                "v b k(iPad6,7;iPad6,8) bb r u s",
                "v b k(iPad6,3;iPad6,4) bb r u s",
                "v b k(iPad7,1;iPad7,2) bb r u s",
                "v b k(iPad7,3;iPad7,4) bb r u s",
                "v b k(iPad8,1;iPad8,2;iPad8,3;iPad8,4) bb r u s",
                "v b k(iPad8,5;iPad8,6;iPad8,7;iPad8,8) bb r u s",
                "v b k(iPad8,9;iPad8,10) bb r u s",
                "v b k(iPad8,11;iPad8,12) bb r u s",
                "v b k(iPad13,4;iPad13,5;iPad13,6;iPad13,7) bb r u s",
                "v b k(iPad13,8;iPad13,9;iPad13,10;iPad13,11) bb r u s",
            }
        },
        {
            "15.x", new[]
            {
                "v b k(iPad6,7;iPad6,8) bb r u s",
                "v b k(iPad6,3;iPad6,4) bb r u s",
                "v b k(iPad7,1;iPad7,2) bb r u s",
                "v b k(iPad7,3;iPad7,4) bb r u s",
                "v b k(iPad8,1;iPad8,2;iPad8,3;iPad8,4) bb r u s",
                "v b k(iPad8,5;iPad8,6;iPad8,7;iPad8,8) bb r u s",
                "v b k(iPad8,9;iPad8,10) bb r u s",
                "v b k(iPad8,11;iPad8,12) bb r u s",
                "v b k(iPad13,4;iPad13,5;iPad13,6;iPad13,7) bb r u s",
                "v b k(iPad13,8;iPad13,9;iPad13,10;iPad13,11) bb r u s",
            }
        },
    };

    /// <summary><c>Firmware/iPad_mini/???</c></summary>
    private static readonly Dictionary<string, string[]> IPAD_MINI_FW_PAGES = new()
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

    /// <summary><c>Beta_Firmware/iPad_mini/???</c></summary>
    private static readonly Dictionary<string, string[]> IPAD_MINI_BETA_FW_PAGES = new()
    {
        {
            "6.x", new[]
            {
                "v b k(iPad2,5) r u i s",
                "v b k(iPad2,6) bb r u i s",
                "v b k(iPad2,7) bb r u i s",
            }
        },
        {
            "7.x", new[]
            {
                "v b k(iPad2,5) r u i s",
                "v b k(iPad2,6) bb r u i s",
                "v b k(iPad2,7) bb r u i s",
                "v b k(iPad4,4) r u i s",
                "v b k(iPad4,5) bb r u i s",
            }
        },
        {
            "8.x", new[]
            {
                "v b k(iPad2,5) r u i s",
                "v b k(iPad2,6) bb r u i s",
                "v b k(iPad2,7) bb r u i s",
                "v b k(iPad4,4) r u i s",
                "v b k(iPad4,5) bb r u i s",
                "v b k(iPad4,6) bb r u i s",
                "v b k(iPad4,7) r u i s",
                "v b k(iPad4,8) bb r u i s",
                "v b k(iPad4,9) bb r u i s",
            }
        },
        {
            "9.x", new[]
            {
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
                "v b k(iPad5,2) bb r u i s",
            }
        },
        {
            "10.x", new[]
            {
                "v b k(iPad4,4;iPad4,5;iPad4,6) bb r u s",
                "v b k(iPad4,7;iPad4,8;iPad4,9) bb r u s",
                "v b k(iPad5,1;iPad5,2) bb r u s",
            }
        },
        {
            "11.x", new[]
            {
                "v b k(iPad4,4;iPad4,5;iPad4,6) bb r u s",
                "v b k(iPad4,7;iPad4,8;iPad4,9) bb r u s",
                "v b k(iPad5,1;iPad5,2) bb r u s",
            }
        },
        {
            "12.x", new[]
            {
                "v b k(iPad4,4;iPad4,5;iPad4,6) bb r u s",
                "v b k(iPad4,7;iPad4,8;iPad4,9) bb r u s",
                "v b k(iPad5,1;iPad5,2) bb r u s",
                "v b k(iPad11,1;iPad11,2) bb r u s",
            }
        },
        {
            "13.x", new[]
            {
                "v b k(iPad5,1;iPad5,2) bb r u s",
                "v b k(iPad11,1;iPad11,2) bb r u s",
            }
        },
        {
            "14.x", new[]
            {
                "v b k(iPad5,1;iPad5,2) bb r u s",
                "v b k(iPad11,1;iPad11,2) bb r u s",
            }
        },
        {
            "15.x", new[]
            {
                "v b k(iPad5,1;iPad5,2) bb r u s",
                "v b k(iPad11,1;iPad11,2) bb r u s",
                "v b k(iPad14,1;iPad14,2) bb r u s",
            }
        },
    };

    /// <summary><c>Firmware/iPhone/???</c></summary>
    private static readonly Dictionary<string, string[]> IPHONE_FW_PAGES = new()
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
                "v b k(iPhone14,6) bb r u h s d",
            }
        },
    };

    /// <summary><c>Beta_Firmware/iPhone/???</c></summary>
    private static readonly Dictionary<string, string[]> IPHONE_BETA_FW_PAGES = new()
    {
        { "1.x", new[] { "v b k(iPhone1,1) bb r u s" } },
        {
            "2.x", new[]
            {
                "v b k(iPhone1,1) bb r u s",
                "v b k(iPhone1,2) bb r u s",
            }
        },
        {
            "3.x", new[]
            {
                "v b k(iPhone1,1) bb r u s",
                "v b k(iPhone1,2) bb r u s",
                "v b k(iPhone2,1) bb r u s",
            }
        },
        {
            "4.x", new[]
            {
                "v b k(iPhone1,2) bb r u s",
                "v b k(iPhone2,1) bb r u s",
                "v b k(iPhone3,1) bb r u s",
            }
        },
        {
            "5.x", new[]
            {
                "v b k(iPhone2,1) bb r u s",
                "v b k(iPhone3,1) bb r u s",
                "v b k(iPhone3,3) bb r u s",
                "v b k(iPhone4,1) bb r u s",
            }
        },
        {
            "6.x", new[]
            {
                "v b k(iPhone2,1) bb r u s",
                "v b k(iPhone3,1) bb r u s",
                "v b k(iPhone3,2) bb r u s",
                "v b k(iPhone3,3) bb r u s",
                "v b k(iPhone4,1) bb r u s",
                "v b k(iPhone5,1) bb r u s",
                "v b k(iPhone5,2) bb r u s",
            }
        },
        {
            "7.x", new[]
            {
                "v b k(iPhone3,1) bb r u s",
                "v b k(iPhone3,2) bb r u s",
                "v b k(iPhone3,3) bb r u s",
                "v b k(iPhone4,1) bb r u s",
                "v b k(iPhone5,1) bb r u s",
                "v b k(iPhone5,2) bb r u s",
                "v b k(iPhone5,3) bb r u s",
                "v b k(iPhone5,4) bb r u s",
                "v b k(iPhone6,1) bb r u s",
                "v b k(iPhone6,2) bb r u s",
            }
        },
        {
            "8.x", new[]
            {
                "v b k(iPhone4,1) bb r u s",
                "v b k(iPhone5,1) bb r u s",
                "v b k(iPhone5,2) bb r u s",
                "v b k(iPhone5,3) bb r u s",
                "v b k(iPhone5,4) bb r u s",
                "v b k(iPhone6,1) bb r u s",
                "v b k(iPhone6,2) bb r u s",
                "v b k(iPhone7,2) bb r u s",
                "v b k(iPhone7,1) bb r u s",
            }
        },
        {
            "9.x", new[]
            {
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
                "v b k(iPhone8,4) bb r u s",
            }
        },
        {
            "10.x", new[]
            {
                "v b k(iPhone5,1;iPhone5,2) bb r u s",
                "v b k(iPhone5,3;iPhone5,4) bb r u s",
                "v b k(iPhone6,1;iPhone6,2) bb r u s",
                "v b k(iPhone7,2) bb r u s",
                "v b k(iPhone7,1) bb r u s",
                "v b k(iPhone8,1) bb r u s",
                "v b k(iPhone8,2) bb r u s",
                "v b k(iPhone8,4) bb r u s",
                "v b k(iPhone9,1;iPhone9,3) bb(iPhone9,1) bb(iPhone9,3) r u s",
                "v b k(iPhone9,2;iPhone9,4) bb(iPhone9,2) bb(iPhone9,4) r u s",
            }
        },
        {
            "11.x", new[]
            {
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
            }
        },
        {
            "12.x", new[]
            {
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
                "v b k(iPhone11,6) bb r u s",
            }
        },
        {
            "13.x", new[]
            {
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
            }
        },
        {
            "14.x", new[]
            {
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
            }
        },
        {
            "15.x", new[]
            {
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
                "v b k(iPhone14,6) bb r u s",
            }
        },
    };

    /// <summary><c>Firmware/iPod_touch/???</c></summary>
    private static readonly Dictionary<string, string[]> IPOD_TOUCH_FW_PAGES = new()
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

    /// <summary><c>Beta_Firmware/iPod_touch/???</c></summary>
    private static readonly Dictionary<string, string[]> IPOD_TOUCH_BETA_FW_PAGES = new()
    {
        { "1.x", new[] { "v b k(iPod1,1) r u s" } },
        { "2.x", new[] { "v b k(iPod1,1) r i s" } },
        {
            "3.x", new[]
            {
                "v b k(iPod1,1) r i s",
                "v b k(iPod2,1) r i s",
            }
        },
        {
            "4.x", new[]
            {
                "v b k(iPod2,1) r u s",
                "v b k(iPod3,1) r u s",
                "v b k(iPod4,1) r u s",
            }
        },
        {
            "5.x", new[]
            {
                "v b k(iPod3,1) r u s",
                "v b k(iPod4,1) r u s",
            }
        },
        {
            "6.x", new[]
            {
                "v b k(iPod4,1) r u s",
                "v b k(iPod5,1) r u s",
            }
        },
        { "7.x", new[] { "v b k(iPod5,1) r u s" } },
        {
            "8.x", new[]
            {
                "v b k(iPod5,1) r u s",
                "v b k(iPod7,1) r u s",
            }
        },
        {
            "9.x", new[]
            {
                "v b k(iPod5,1) r u s",
                "v b k(iPod7,1) r u s",
            }
        },
        { "10.x", new[] { "v b k(iPod7,1) r u s" } },
        { "11.x", new[] { "v b k(iPod7,1) r u s" } },
        {
            "12.x", new[]
            {
                "v b k(iPod7,1) r u s",
                "v b k(iPod9,1) r u s",
            }
        },
        { "13.x", new[] { "v b k(iPod9,1) r u s" } },
        { "14.x", new[] { "v b k(iPod9,1) r u s" } },
        { "15.x", new[] { "v b k(iPod9,1) r u s" } },
    };

    public static readonly List<(string, Dictionary<string, string[]>)> ALL_DESCRIPTORS = new()
    {
        ("Firmware/Apple_TV/", APPLE_TV_FW_PAGES),
        ("Firmware/Apple_Watch/", APPLE_WATCH_FW_PAGES),
        ("Firmware/HomePod/", HOME_POD_FW_PAGES),
        // ("Firmware/Mac/", APPLE_SILICON_FW_PAGES),
        // ("Firmware/iBridge/", IBRIDGE_FW_PAGES),
        ("Firmware/iPad/", IPAD_FW_PAGES),
        ("Firmware/iPad_Air/", IPAD_AIR_FW_PAGES),
        ("Firmware/iPad_Pro/", IPAD_PRO_FW_PAGES),
        ("Firmware/iPad_mini/", IPAD_MINI_FW_PAGES),
        ("Firmware/iPhone/", IPHONE_FW_PAGES),
        ("Firmware/iPod_touch/", IPOD_TOUCH_FW_PAGES),

        ("Beta_Firmware/Apple_TV/", APPLE_TV_BETA_FW_PAGES),
        ("Beta_Firmware/Apple_Watch/", APPLE_WATCH_BETA_FW_PAGES),
        ("Beta_Firmware/HomePod/", HOME_POD_BETA_FW_PAGES),
        // ("Beta_Firmware/Mac/", APPLE_SILICON_BETA_FW_PAGES),
        // ("Beta_Firmware/iBridge/", IBRIDGE_BETA_FW_PAGES),
        ("Beta_Firmware/iPad/", IPAD_BETA_FW_PAGES),
        ("Beta_Firmware/iPad_Air/", IPAD_AIR_BETA_FW_PAGES),
        ("Beta_Firmware/iPad_Pro/", IPAD_PRO_BETA_FW_PAGES),
        ("Beta_Firmware/iPad_mini/", IPAD_MINI_BETA_FW_PAGES),
        ("Beta_Firmware/iPhone/", IPHONE_BETA_FW_PAGES),
        ("Beta_Firmware/iPod_touch/", IPOD_TOUCH_BETA_FW_PAGES),
    };
}
