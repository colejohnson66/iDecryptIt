/* =============================================================================
 * File:   KeySelectionLists.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2014-2018 Cole Johnson
 * 
 * This file is part of iDecryptIt
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
using Hexware.Plist;
using System.Collections.Generic;
using System.IO;

namespace Hexware.Programs.iDecryptIt
{
    internal static class KeySelectionLists
    {
        internal static List<ComboBoxEntry> Products;
        internal static Dictionary<string, List<ComboBoxEntry>> ProductsHelper;

        private static List<ComboBoxEntry> AppleTV;
        private static List<ComboBoxEntry> iPad;
        private static List<ComboBoxEntry> iPadMini;
        private static List<ComboBoxEntry> iPhone;
        private static List<ComboBoxEntry> iPodTouch;
        internal static Dictionary<string, List<ComboBoxEntry>> ModelsHelper;

        // Apple TV
        private static List<ComboBoxEntry> AppleTV21;
        private static List<ComboBoxEntry> AppleTV31;
        private static List<ComboBoxEntry> AppleTV32;
        private static List<ComboBoxEntry> AppleTV53;
        private static List<ComboBoxEntry> AppleTV62;

        // iPad
        private static List<ComboBoxEntry> iPad11;
        private static List<ComboBoxEntry> iPad21;
        private static List<ComboBoxEntry> iPad22;
        private static List<ComboBoxEntry> iPad23;
        private static List<ComboBoxEntry> iPad24;
        private static List<ComboBoxEntry> iPad31;
        private static List<ComboBoxEntry> iPad32;
        private static List<ComboBoxEntry> iPad33;
        private static List<ComboBoxEntry> iPad34;
        private static List<ComboBoxEntry> iPad35;
        private static List<ComboBoxEntry> iPad36;
        private static List<ComboBoxEntry> iPad41;
        private static List<ComboBoxEntry> iPad42;
        private static List<ComboBoxEntry> iPad43;
        private static List<ComboBoxEntry> iPad53;
        private static List<ComboBoxEntry> iPad54;
        private static List<ComboBoxEntry> iPad63;
        private static List<ComboBoxEntry> iPad64;
        private static List<ComboBoxEntry> iPad67;
        private static List<ComboBoxEntry> iPad68;
        private static List<ComboBoxEntry> iPad611;
        private static List<ComboBoxEntry> iPad612;
        private static List<ComboBoxEntry> iPad71;
        private static List<ComboBoxEntry> iPad72;
        private static List<ComboBoxEntry> iPad73;
        private static List<ComboBoxEntry> iPad74;
        private static List<ComboBoxEntry> iPad75;
        private static List<ComboBoxEntry> iPad76;

        // iPad mini
        private static List<ComboBoxEntry> iPad25;
        private static List<ComboBoxEntry> iPad26;
        private static List<ComboBoxEntry> iPad27;
        private static List<ComboBoxEntry> iPad44;
        private static List<ComboBoxEntry> iPad45;
        private static List<ComboBoxEntry> iPad46;
        private static List<ComboBoxEntry> iPad47;
        private static List<ComboBoxEntry> iPad48;
        private static List<ComboBoxEntry> iPad49;
        private static List<ComboBoxEntry> iPad51;
        private static List<ComboBoxEntry> iPad52;

        // iPhone
        private static List<ComboBoxEntry> iPhone11;
        private static List<ComboBoxEntry> iPhone12;
        private static List<ComboBoxEntry> iPhone21;
        private static List<ComboBoxEntry> iPhone31;
        private static List<ComboBoxEntry> iPhone32;
        private static List<ComboBoxEntry> iPhone33;
        private static List<ComboBoxEntry> iPhone41;
        private static List<ComboBoxEntry> iPhone51;
        private static List<ComboBoxEntry> iPhone52;
        private static List<ComboBoxEntry> iPhone53;
        private static List<ComboBoxEntry> iPhone54;
        private static List<ComboBoxEntry> iPhone61;
        private static List<ComboBoxEntry> iPhone62;
        private static List<ComboBoxEntry> iPhone71;
        private static List<ComboBoxEntry> iPhone72;
        private static List<ComboBoxEntry> iPhone81;
        private static List<ComboBoxEntry> iPhone82;
        private static List<ComboBoxEntry> iPhone84;
        private static List<ComboBoxEntry> iPhone91;
        private static List<ComboBoxEntry> iPhone93;
        private static List<ComboBoxEntry> iPhone92;
        private static List<ComboBoxEntry> iPhone94;
        private static List<ComboBoxEntry> iPhone101;
        private static List<ComboBoxEntry> iPhone104;
        private static List<ComboBoxEntry> iPhone102;
        private static List<ComboBoxEntry> iPhone105;
        private static List<ComboBoxEntry> iPhone103;
        private static List<ComboBoxEntry> iPhone106;

        // iPod touch
        private static List<ComboBoxEntry> iPod11;
        private static List<ComboBoxEntry> iPod21;
        private static List<ComboBoxEntry> iPod31;
        private static List<ComboBoxEntry> iPod41;
        private static List<ComboBoxEntry> iPod51;
        private static List<ComboBoxEntry> iPod71;

        internal static void Init()
        {
            Products = new List<ComboBoxEntry>
            {
                new ComboBoxEntry("AppleTV", "Apple TV"),
                new ComboBoxEntry("iPad", "iPad"),
                new ComboBoxEntry("iPadMini", "iPad mini"),
                new ComboBoxEntry("iPhone", "iPhone"),
                new ComboBoxEntry("iPodTouch", "iPod touch")
            };

            InitModels();

            InitDevices();

            // MUST BE LAST
            InitHelpers();
        }
        private static void InitHelpers()
        {
            ProductsHelper = new Dictionary<string, List<ComboBoxEntry>>
            {
                { "AppleTV", AppleTV },
                { "iPad", iPad },
                { "iPadMini", iPadMini },
                { "iPhone", iPhone },
                { "iPodTouch", iPodTouch }
            };

            ModelsHelper = new Dictionary<string, List<ComboBoxEntry>>
            {
                { "AppleTV2,1", AppleTV21 }, // Apple TV 2G
                { "AppleTV3,1", AppleTV31 }, // Apple TV 3G
                { "AppleTV3,2", AppleTV32 },
                { "AppleTV5,3", AppleTV53 },
                { "AppleTV6,2", AppleTV62 },
                { "iPad1,1", iPad11 }, // iPad 1G
                { "iPad2,1", iPad21 }, // iPad 2
                { "iPad2,2", iPad22 },
                { "iPad2,3", iPad23 },
                { "iPad2,4", iPad24 },
                { "iPad2,5", iPad25 }, // iPad mini 1G
                { "iPad2,6", iPad26 },
                { "iPad2,7", iPad27 },
                { "iPad3,1", iPad31 }, // iPad 3
                { "iPad3,2", iPad32 },
                { "iPad3,3", iPad33 },
                { "iPad3,4", iPad34 }, // iPad 4
                { "iPad3,5", iPad35 },
                { "iPad3,6", iPad36 },
                { "iPad4,1", iPad41 }, // iPad Air
                { "iPad4,2", iPad42 },
                { "iPad4,3", iPad43 },
                { "iPad4,4", iPad44 }, // iPad mini 2
                { "iPad4,5", iPad45 },
                { "iPad4,6", iPad46 },
                { "iPad4,7", iPad47 }, // iPad mini 3
                { "iPad4,8", iPad48 },
                { "iPad4,9", iPad49 },
                { "iPad5,1", iPad51 }, // iPad mini 4
                { "iPad5,2", iPad52 },
                { "iPad5,3", iPad53 }, // iPad Air 2
                { "iPad5,4", iPad54 },
                { "iPad6,3", iPad63 }, // iPad Pro 9.7"
                { "iPad6,4", iPad64 },
                { "iPad6,7", iPad67 }, // iPad Pro 12.9"
                { "iPad6,8", iPad68 },
                { "iPad6,11", iPad611 }, // iPad 5
                { "iPad6,12", iPad612 },
                { "iPad7,1", iPad71 }, // iPad Pro 2 12.9"
                { "iPad7,2", iPad72 },
                { "iPad7,3", iPad73 }, // iPad Pro 2 10.5"
                { "iPad7,4", iPad74 },
                { "iPad7,5", iPad75 }, // iPad 6
                { "iPad7,6", iPad76 },
                { "iPhone1,1", iPhone11 }, // iPhone 2G
                { "iPhone1,2", iPhone12 }, // iPhone 3G
                { "iPhone2,1", iPhone21 }, // iPhone 3GS
                { "iPhone3,1", iPhone31 }, // iPhone 4
                { "iPhone3,2", iPhone32 },
                { "iPhone3,3", iPhone33 },
                { "iPhone4,1", iPhone41 }, // iPhone 4S
                { "iPhone5,1", iPhone51 }, // iPhone 5
                { "iPhone5,2", iPhone52 },
                { "iPhone5,3", iPhone53 }, // iPhone 5c
                { "iPhone5,4", iPhone54 },
                { "iPhone6,1", iPhone61 }, // iPhone 5s
                { "iPhone6,2", iPhone62 },
                { "iPhone7,1", iPhone71 }, // iPhone 6+
                { "iPhone7,2", iPhone72 }, // iPhone 6
                { "iPhone8,1", iPhone81 }, // iPhone 6s
                { "iPhone8,2", iPhone82 }, // iPhone 6s+
                { "iPhone8,4", iPhone84 }, // iPhone SE
                { "iPhone9,1", iPhone91 }, // iPhone 7
                { "iPhone9,3", iPhone93 },
                { "iPhone9,2", iPhone92 }, // iPhone 7+
                { "iPhone9,4", iPhone94 },
                { "iPhone10,1", iPhone101 }, // iPhone 8
                { "iPhone10,4", iPhone104 },
                { "iPhone10,2", iPhone102 }, // iPhone 8+
                { "iPhone10,5", iPhone105 },
                { "iPhone10,3", iPhone103 }, // iPhone X
                { "iPhone10,6", iPhone106 },
                { "iPod1,1", iPod11 }, // iPod 1G
                { "iPod2,1", iPod21 }, // iPod 2G
                { "iPod3,1", iPod31 }, // iPod 3G
                { "iPod4,1", iPod41 }, // iPod 4G
                { "iPod5,1", iPod51 }, // iPod 5G
                { "iPod7,1", iPod71 } // iPod 6G
            };
        }
        private static void InitModels()
        {
            AppleTV = new List<ComboBoxEntry>
            {
                new ComboBoxEntry("AppleTV2,1", "2G (AppleTV2,1)"),
                new ComboBoxEntry("AppleTV3,1", "3G (AppleTV3,1)"),
                new ComboBoxEntry("AppleTV3,2", "3G Rev A (AppleTV3,2)"),
                new ComboBoxEntry("AppleTV5,3", "4G (AppleTV5,3)")
            };

            iPad = new List<ComboBoxEntry>
            {
                new ComboBoxEntry("iPad1,1", "1G (iPad1,1)"),
                new ComboBoxEntry("iPad2,1", "2 Wi-Fi (iPad2,1)"),
                new ComboBoxEntry("iPad2,2", "2 GSM (iPad2,2)"),
                new ComboBoxEntry("iPad2,3", "2 CDMA (iPad2,3)"),
                new ComboBoxEntry("iPad2,4", "2 Wi-Fi Rev A (iPad2,4)"),
                new ComboBoxEntry("iPad3,1", "3 Wi-Fi (iPad3,1)"),
                new ComboBoxEntry("iPad3,2", "3 CDMA (iPad3,2)"),
                new ComboBoxEntry("iPad3,3", "3 Global (iPad3,3)"),
                new ComboBoxEntry("iPad3,4", "4 Wi-Fi (iPad3,4)"),
                new ComboBoxEntry("iPad3,5", "4 GSM (iPad3,5)"),
                new ComboBoxEntry("iPad3,6", "4 Global (iPad3,6)"),
                new ComboBoxEntry("iPad4,1", "Air Wi-Fi (iPad4,1)"),
                new ComboBoxEntry("iPad4,2", "Air Cellular (iPad4,2)"),
                new ComboBoxEntry("iPad4,3", "Air Cellular China (iPad4,3)"),
                new ComboBoxEntry("iPad5,3", "Air 2 Wi-Fi (iPad5,3)"),
                new ComboBoxEntry("iPad5,4", "Air 2 Cellular (iPad5,4)"),
                new ComboBoxEntry("iPad6,3", "Pro 9.7\" Wi-Fi (iPad6,3)"),
                new ComboBoxEntry("iPad6,4", "Pro 9.7\" Cellular (iPad6,4)"),
                new ComboBoxEntry("iPad6,7", "Pro 12.9\" Wi-Fi (iPad6,7)"),
                new ComboBoxEntry("iPad6,8", "Pro 12.9\" Cellular (iPad6,8)"),
                new ComboBoxEntry("iPad6,11", "5 Wi-Fi (iPad6,11)"),
                new ComboBoxEntry("iPad6,12", "5 Cellular (iPad6,12)"),
                new ComboBoxEntry("iPad7,1", "Pro 2 12.9\" Wi-Fi (iPad7,1)"),
                new ComboBoxEntry("iPad7,2", "Pro 2 12.9\" Cellular (iPad7,2)"),
                new ComboBoxEntry("iPad7,3", "Pro 2 10.5\" Wi-Fi (iPad7,3)"),
                new ComboBoxEntry("iPad7,4", "Pro 2 10.5\" Cellular (iPad7,4)"),
                new ComboBoxEntry("iPad7,5", "6 Wi-Fi (iPad7,5)"),
                new ComboBoxEntry("iPad7,6", "6 Cellular (iPad7,6)")
            };

            iPadMini = new List<ComboBoxEntry>
            {
                new ComboBoxEntry("iPad2,5", "1G Wi-Fi (iPad2,5)"),
                new ComboBoxEntry("iPad2,5", "1G GSM (iPad2,5)"),
                new ComboBoxEntry("iPad2,5", "1G Global (iPad2,5)"),
                new ComboBoxEntry("iPad4,4", "2 Wi-Fi (iPad4,4)"),
                new ComboBoxEntry("iPad4,5", "2 Cellular (iPad4,5)"),
                new ComboBoxEntry("iPad4,6", "2 Cellular China (iPad4,6)"),
                new ComboBoxEntry("iPad4,7", "3 Wi-Fi (iPad4,7)"),
                new ComboBoxEntry("iPad4,8", "3 Cellular (iPad4,8)"),
                new ComboBoxEntry("iPad4,9", "3 Cellular China (iPad4,9)"),
                new ComboBoxEntry("iPad5,1", "4 Wi-Fi (iPad5,1)"),
                new ComboBoxEntry("iPad5,2", "4 Cellular (iPad5,2)")
            };

            iPhone = new List<ComboBoxEntry>
            {
                new ComboBoxEntry("iPhone1,1", "2G (iPhone1,1)"),
                new ComboBoxEntry("iPhone1,2", "3G (iPhone1,2)"),
                new ComboBoxEntry("iPhone2,1", "3GS (iPhone2,1)"),
                new ComboBoxEntry("iPhone3,1", "4 GSM (iPhone3,1)"),
                new ComboBoxEntry("iPhone3,2", "4 CDMA (iPhone3,2)"),
                new ComboBoxEntry("iPhone3,3", "4 GSM Rev A (iPhone3,3)"),
                new ComboBoxEntry("iPhone4,1", "4S (iPhone4,1)"),
                new ComboBoxEntry("iPhone5,1", "5 GSM (iPhone5,1)"),
                new ComboBoxEntry("iPhone5,2", "5 CDMA (iPhone5,2)"),
                new ComboBoxEntry("iPhone5,3", "5c GSM (iPhone5,3)"),
                new ComboBoxEntry("iPhone5,4", "5c Global (iPhone5,4)"),
                new ComboBoxEntry("iPhone6,1", "5s GSM (iPhone6,1)"),
                new ComboBoxEntry("iPhone6,2", "5s Global (iPhone6,2)"),
                new ComboBoxEntry("iPhone7,1", "6 Plus (iPhone7,1)"),
                new ComboBoxEntry("iPhone7,2", "6 (iPhone7,2)"),
                new ComboBoxEntry("iPhone8,1", "6s (iPhone8,1)"),
                new ComboBoxEntry("iPhone8,2", "6s Plus (iPhone8,2)"),
                new ComboBoxEntry("iPhone8,4", "SE (iPhone8,4)"),
                new ComboBoxEntry("iPhone9,1", "7 (iPhone9,1)"),
                new ComboBoxEntry("iPhone9,2", "7 (iPhone9,2)"),
                new ComboBoxEntry("iPhone9,3", "7 Plus (iPhone9,3)"),
                new ComboBoxEntry("iPhone9,4", "7 Plus (iPhone9,4)"),
                new ComboBoxEntry("iPhone10,1", "8 (iPhone10,1)"),
                new ComboBoxEntry("iPhone10,4", "8 (iPhone10,4)"),
                new ComboBoxEntry("iPhone10,2", "8 Plus (iPhone10,2)"),
                new ComboBoxEntry("iPhone10,5", "8 Plus (iPhone10,5)"),
                new ComboBoxEntry("iPhone10,3", "X (iPhone10,3)"),
                new ComboBoxEntry("iPhone10,6", "X (iPhone10,6)")
            };

            iPodTouch = new List<ComboBoxEntry>
            {
                new ComboBoxEntry("iPod1,1", "1G (iPod1,1)"),
                new ComboBoxEntry("iPod2,1", "2G (iPod2,1)"),
                new ComboBoxEntry("iPod3,1", "3G (iPod3,1)"),
                new ComboBoxEntry("iPod4,1", "4G (iPod4,1)"),
                new ComboBoxEntry("iPod5,1", "5G (iPod5,1)"),
                new ComboBoxEntry("iPod7,1", "6G (iPod7,1)")
            };
        }
        private static void InitDevices()
        {
            Stream keyListPlist = Globals.GetStream("KeyList.plist");
            PlistDocument keyList = new PlistDocument(keyListPlist);
            PlistDict rootNode = (PlistDict)keyList.RootNode;
            keyListPlist.Close();

            // Apple TV
            InitDevice(ref AppleTV21, rootNode.Get<PlistArray>("AppleTV2,1"));
            InitDevice(ref AppleTV31, rootNode.Get<PlistArray>("AppleTV3,1"));
            InitDevice(ref AppleTV32, rootNode.Get<PlistArray>("AppleTV3,2"));
            InitDevice(ref AppleTV53, rootNode.Get<PlistArray>("AppleTV5,3"));
            InitDevice(ref AppleTV62, rootNode.Get<PlistArray>("AppleTV6,2"));

            // iPad
            InitDevice(ref iPad11, rootNode.Get<PlistArray>("iPad1,1"));
            InitDevice(ref iPad21, rootNode.Get<PlistArray>("iPad2,1"));
            InitDevice(ref iPad22, rootNode.Get<PlistArray>("iPad2,2"));
            InitDevice(ref iPad23, rootNode.Get<PlistArray>("iPad2,3"));
            InitDevice(ref iPad24, rootNode.Get<PlistArray>("iPad2,4"));
            InitDevice(ref iPad31, rootNode.Get<PlistArray>("iPad3,1"));
            InitDevice(ref iPad32, rootNode.Get<PlistArray>("iPad3,2"));
            InitDevice(ref iPad33, rootNode.Get<PlistArray>("iPad3,3"));
            InitDevice(ref iPad34, rootNode.Get<PlistArray>("iPad3,4"));
            InitDevice(ref iPad35, rootNode.Get<PlistArray>("iPad3,5"));
            InitDevice(ref iPad36, rootNode.Get<PlistArray>("iPad3,6"));
            InitDevice(ref iPad41, rootNode.Get<PlistArray>("iPad4,1"));
            InitDevice(ref iPad42, rootNode.Get<PlistArray>("iPad4,2"));
            InitDevice(ref iPad43, rootNode.Get<PlistArray>("iPad4,3"));
            InitDevice(ref iPad53, rootNode.Get<PlistArray>("iPad5,3"));
            InitDevice(ref iPad54, rootNode.Get<PlistArray>("iPad5,4"));
            InitDevice(ref iPad63, rootNode.Get<PlistArray>("iPad6,3"));
            InitDevice(ref iPad64, rootNode.Get<PlistArray>("iPad6,4"));
            InitDevice(ref iPad67, rootNode.Get<PlistArray>("iPad6,7"));
            InitDevice(ref iPad68, rootNode.Get<PlistArray>("iPad6,8"));
            InitDevice(ref iPad611, rootNode.Get<PlistArray>("iPad6,11"));
            InitDevice(ref iPad612, rootNode.Get<PlistArray>("iPad6,12"));
            InitDevice(ref iPad71, rootNode.Get<PlistArray>("iPad7,1"));
            InitDevice(ref iPad72, rootNode.Get<PlistArray>("iPad7,2"));
            InitDevice(ref iPad73, rootNode.Get<PlistArray>("iPad7,3"));
            InitDevice(ref iPad74, rootNode.Get<PlistArray>("iPad7,4"));
            InitDevice(ref iPad75, rootNode.Get<PlistArray>("iPad7,5"));
            InitDevice(ref iPad76, rootNode.Get<PlistArray>("iPad7,6"));

            // iPad mini
            InitDevice(ref iPad25, rootNode.Get<PlistArray>("iPad2,5"));
            InitDevice(ref iPad26, rootNode.Get<PlistArray>("iPad2,6"));
            InitDevice(ref iPad27, rootNode.Get<PlistArray>("iPad2,7"));
            InitDevice(ref iPad44, rootNode.Get<PlistArray>("iPad4,4"));
            InitDevice(ref iPad45, rootNode.Get<PlistArray>("iPad4,5"));
            InitDevice(ref iPad46, rootNode.Get<PlistArray>("iPad4,6"));
            InitDevice(ref iPad47, rootNode.Get<PlistArray>("iPad4,7"));
            InitDevice(ref iPad48, rootNode.Get<PlistArray>("iPad4,8"));
            InitDevice(ref iPad49, rootNode.Get<PlistArray>("iPad4,9"));
            InitDevice(ref iPad51, rootNode.Get<PlistArray>("iPad5,1"));
            InitDevice(ref iPad52, rootNode.Get<PlistArray>("iPad5,2"));

            // iPhone
            InitDevice(ref iPhone11, rootNode.Get<PlistArray>("iPhone1,1"));
            InitDevice(ref iPhone12, rootNode.Get<PlistArray>("iPhone1,2"));
            InitDevice(ref iPhone21, rootNode.Get<PlistArray>("iPhone2,1"));
            InitDevice(ref iPhone31, rootNode.Get<PlistArray>("iPhone3,1"));
            InitDevice(ref iPhone32, rootNode.Get<PlistArray>("iPhone3,2"));
            InitDevice(ref iPhone33, rootNode.Get<PlistArray>("iPhone3,3"));
            InitDevice(ref iPhone41, rootNode.Get<PlistArray>("iPhone4,1"));
            InitDevice(ref iPhone51, rootNode.Get<PlistArray>("iPhone5,1"));
            InitDevice(ref iPhone52, rootNode.Get<PlistArray>("iPhone5,2"));
            InitDevice(ref iPhone53, rootNode.Get<PlistArray>("iPhone5,3"));
            InitDevice(ref iPhone54, rootNode.Get<PlistArray>("iPhone5,4"));
            InitDevice(ref iPhone61, rootNode.Get<PlistArray>("iPhone6,1"));
            InitDevice(ref iPhone62, rootNode.Get<PlistArray>("iPhone6,2"));
            InitDevice(ref iPhone71, rootNode.Get<PlistArray>("iPhone7,1"));
            InitDevice(ref iPhone72, rootNode.Get<PlistArray>("iPhone7,2"));
            InitDevice(ref iPhone81, rootNode.Get<PlistArray>("iPhone8,1"));
            InitDevice(ref iPhone82, rootNode.Get<PlistArray>("iPhone8,2"));
            InitDevice(ref iPhone84, rootNode.Get<PlistArray>("iPhone8,4"));
            InitDevice(ref iPhone91, rootNode.Get<PlistArray>("iPhone9,1"));
            InitDevice(ref iPhone93, rootNode.Get<PlistArray>("iPhone9,3"));
            InitDevice(ref iPhone92, rootNode.Get<PlistArray>("iPhone9,2"));
            InitDevice(ref iPhone94, rootNode.Get<PlistArray>("iPhone9,4"));
            InitDevice(ref iPhone101, rootNode.Get<PlistArray>("iPhone10,1"));
            InitDevice(ref iPhone104, rootNode.Get<PlistArray>("iPhone10,4"));
            InitDevice(ref iPhone102, rootNode.Get<PlistArray>("iPhone10,2"));
            InitDevice(ref iPhone105, rootNode.Get<PlistArray>("iPhone10,5"));
            InitDevice(ref iPhone103, rootNode.Get<PlistArray>("iPhone10,3"));
            InitDevice(ref iPhone106, rootNode.Get<PlistArray>("iPhone10,6"));

            // iPod touch
            InitDevice(ref iPod11, rootNode.Get<PlistArray>("iPod1,1"));
            InitDevice(ref iPod21, rootNode.Get<PlistArray>("iPod2,1"));
            InitDevice(ref iPod31, rootNode.Get<PlistArray>("iPod3,1"));
            InitDevice(ref iPod41, rootNode.Get<PlistArray>("iPod4,1"));
            InitDevice(ref iPod51, rootNode.Get<PlistArray>("iPod5,1"));
            InitDevice(ref iPod71, rootNode.Get<PlistArray>("iPod7,1"));
        }
        private static void InitDevice(ref List<ComboBoxEntry> device, PlistArray versionArr)
        {
            device = new List<ComboBoxEntry>();
            foreach (IPlistElement elem in versionArr.Value)
            {
                PlistDict dict = (PlistDict)elem;
                string build = dict.Get<PlistString>("Build").Value;
                string version = dict.Get<PlistString>("Version").Value;
                bool hasKeys = dict.Get<PlistBool>("Has Keys").Value;
                device.Add(new ComboBoxEntry(
                    build,
                    $"{version} ({build})",
                    hasKeys));
            }
        }
    }
}