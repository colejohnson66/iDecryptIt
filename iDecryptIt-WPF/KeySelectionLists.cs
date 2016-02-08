/* =============================================================================
 * File:   KeySelectionLists.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2014-2016 Cole Johnson
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
        private static List<ComboBoxEntry> iPad67;
        private static List<ComboBoxEntry> iPad68;

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

        // iPod touch
        private static List<ComboBoxEntry> iPod11;
        private static List<ComboBoxEntry> iPod21;
        private static List<ComboBoxEntry> iPod31;
        private static List<ComboBoxEntry> iPod41;
        private static List<ComboBoxEntry> iPod51;
        private static List<ComboBoxEntry> iPod71;

        internal static void Init()
        {
            Products = new List<ComboBoxEntry>();
            Products.Add(new ComboBoxEntry("AppleTV", "Apple TV"));
            Products.Add(new ComboBoxEntry("iPad", "iPad"));
            Products.Add(new ComboBoxEntry("iPadMini", "iPad mini"));
            Products.Add(new ComboBoxEntry("iPhone", "iPhone"));
            Products.Add(new ComboBoxEntry("iPodTouch", "iPod touch"));
            
            InitModels();

            InitDevices();

            // MUST BE LAST
            InitHelpers();
        }
        private static void InitHelpers()
        {
            ProductsHelper = new Dictionary<string, List<ComboBoxEntry>>();
            ProductsHelper.Add("AppleTV", AppleTV);
            ProductsHelper.Add("iPad", iPad);
            ProductsHelper.Add("iPadMini", iPadMini);
            ProductsHelper.Add("iPhone", iPhone);
            ProductsHelper.Add("iPodTouch", iPodTouch);

            ModelsHelper = new Dictionary<string, List<ComboBoxEntry>>();
            ModelsHelper.Add("AppleTV2,1", AppleTV21); // Apple TV 2G
            ModelsHelper.Add("AppleTV3,1", AppleTV31); // Apple TV 3G
            ModelsHelper.Add("AppleTV3,2", AppleTV32);
            ModelsHelper.Add("AppleTV5,3", AppleTV53);
            ModelsHelper.Add("iPad1,1", iPad11); // iPad 1G
            ModelsHelper.Add("iPad2,1", iPad21); // iPad 2
            ModelsHelper.Add("iPad2,2", iPad22);
            ModelsHelper.Add("iPad2,3", iPad23);
            ModelsHelper.Add("iPad2,4", iPad24);
            ModelsHelper.Add("iPad2,5", iPad25); // iPad mini 1G
            ModelsHelper.Add("iPad2,6", iPad26);
            ModelsHelper.Add("iPad2,7", iPad27);
            ModelsHelper.Add("iPad3,1", iPad31); // iPad 3
            ModelsHelper.Add("iPad3,2", iPad32);
            ModelsHelper.Add("iPad3,3", iPad33);
            ModelsHelper.Add("iPad3,4", iPad34); // iPad 4
            ModelsHelper.Add("iPad3,5", iPad35);
            ModelsHelper.Add("iPad3,6", iPad36);
            ModelsHelper.Add("iPad4,1", iPad41); // iPad Air
            ModelsHelper.Add("iPad4,2", iPad42);
            ModelsHelper.Add("iPad4,3", iPad43);
            ModelsHelper.Add("iPad4,4", iPad44); // iPad mini 2
            ModelsHelper.Add("iPad4,5", iPad45);
            ModelsHelper.Add("iPad4,6", iPad46);
            ModelsHelper.Add("iPad4,7", iPad47); // iPad mini 3
            ModelsHelper.Add("iPad4,8", iPad48);
            ModelsHelper.Add("iPad4,9", iPad49);
            ModelsHelper.Add("iPad5,1", iPad51); // iPad mini 4
            ModelsHelper.Add("iPad5,2", iPad52);
            ModelsHelper.Add("iPad5,3", iPad53); // iPad Air 2
            ModelsHelper.Add("iPad5,4", iPad54);
            ModelsHelper.Add("iPad6,7", iPad67); // iPad Pro
            ModelsHelper.Add("iPad6,8", iPad68);
            ModelsHelper.Add("iPhone1,1", iPhone11); // iPhone 2G
            ModelsHelper.Add("iPhone1,2", iPhone12); // iPhone 3G
            ModelsHelper.Add("iPhone2,1", iPhone21); // iPhone 3GS
            ModelsHelper.Add("iPhone3,1", iPhone31); // iPhone 4
            ModelsHelper.Add("iPhone3,2", iPhone32);
            ModelsHelper.Add("iPhone3,3", iPhone33);
            ModelsHelper.Add("iPhone4,1", iPhone41); // iPhone 4S
            ModelsHelper.Add("iPhone5,1", iPhone51); // iPhone 5
            ModelsHelper.Add("iPhone5,2", iPhone52);
            ModelsHelper.Add("iPhone5,3", iPhone53); // iPhone 5c
            ModelsHelper.Add("iPhone5,4", iPhone54);
            ModelsHelper.Add("iPhone6,1", iPhone61); // iPhone 5s
            ModelsHelper.Add("iPhone6,2", iPhone62);
            ModelsHelper.Add("iPhone7,1", iPhone71); // iPhone 6+
            ModelsHelper.Add("iPhone7,2", iPhone72); // iPhone 6
            ModelsHelper.Add("iPhone8,1", iPhone81); // iPhone 6s
            ModelsHelper.Add("iPhone8,2", iPhone82); // iPhone 6s+
            ModelsHelper.Add("iPod1,1", iPod11); // iPod 1G
            ModelsHelper.Add("iPod2,1", iPod21); // iPod 2G
            ModelsHelper.Add("iPod3,1", iPod31); // iPod 3G
            ModelsHelper.Add("iPod4,1", iPod41); // iPod 4G
            ModelsHelper.Add("iPod5,1", iPod51); // iPod 5G
            ModelsHelper.Add("iPod7,1", iPod71); // iPod 6G
        }
        private static void InitModels()
        {
            AppleTV = new List<ComboBoxEntry>();
            AppleTV.Add(new ComboBoxEntry("AppleTV2,1", "2G (AppleTV2,1)"));
            AppleTV.Add(new ComboBoxEntry("AppleTV3,1", "3G (AppleTV3,1)"));
            AppleTV.Add(new ComboBoxEntry("AppleTV3,2", "3G Rev A (AppleTV3,2)"));
            AppleTV.Add(new ComboBoxEntry("AppleTV5,3", "4G (AppleTV5,3)"));

            iPad = new List<ComboBoxEntry>();
            iPad.Add(new ComboBoxEntry("iPad1,1", "1G (iPad1,1)"));
            iPad.Add(new ComboBoxEntry("iPad2,1", "2 Wi-Fi (iPad2,1)"));
            iPad.Add(new ComboBoxEntry("iPad2,2", "2 GSM (iPad2,2)"));
            iPad.Add(new ComboBoxEntry("iPad2,3", "2 CDMA (iPad2,3)"));
            iPad.Add(new ComboBoxEntry("iPad2,4", "2 Wi-Fi Rev A (iPad2,4)"));
            iPad.Add(new ComboBoxEntry("iPad3,1", "3 Wi-Fi (iPad3,1)"));
            iPad.Add(new ComboBoxEntry("iPad3,2", "3 CDMA (iPad3,2)"));
            iPad.Add(new ComboBoxEntry("iPad3,3", "3 Global (iPad3,3)"));
            iPad.Add(new ComboBoxEntry("iPad3,4", "4 Wi-Fi (iPad3,4)"));
            iPad.Add(new ComboBoxEntry("iPad3,5", "4 GSM (iPad3,5)"));
            iPad.Add(new ComboBoxEntry("iPad3,6", "4 Global (iPad3,6)"));
            iPad.Add(new ComboBoxEntry("iPad4,1", "Air Wi-Fi (iPad4,1)"));
            iPad.Add(new ComboBoxEntry("iPad4,2", "Air Cellular (iPad4,2)"));
            iPad.Add(new ComboBoxEntry("iPad4,3", "Air Cellular China (iPad4,3)"));
            iPad.Add(new ComboBoxEntry("iPad5,3", "Air 2 Wi-Fi (iPad5,3)"));
            iPad.Add(new ComboBoxEntry("iPad5,4", "Air 2 Cellular (iPad5,4)"));
            iPad.Add(new ComboBoxEntry("iPad6,7", "Pro Wi-Fi (iPad6,7)"));
            iPad.Add(new ComboBoxEntry("iPad6,8", "Pro Cellular (iPad6,8)"));

            iPadMini = new List<ComboBoxEntry>();
            iPadMini.Add(new ComboBoxEntry("iPad2,5", "1G Wi-Fi (iPad2,5)"));
            iPadMini.Add(new ComboBoxEntry("iPad2,5", "1G GSM (iPad2,5)"));
            iPadMini.Add(new ComboBoxEntry("iPad2,5", "1G Global (iPad2,5)"));
            iPadMini.Add(new ComboBoxEntry("iPad4,4", "2 Wi-Fi (iPad4,4)"));
            iPadMini.Add(new ComboBoxEntry("iPad4,5", "2 Cellular (iPad4,5)"));
            iPadMini.Add(new ComboBoxEntry("iPad4,6", "2 Cellular China (iPad4,6)"));
            iPadMini.Add(new ComboBoxEntry("iPad4,7", "3 Wi-Fi (iPad4,7)"));
            iPadMini.Add(new ComboBoxEntry("iPad4,8", "3 Cellular (iPad4,8)"));
            iPadMini.Add(new ComboBoxEntry("iPad4,9", "3 Cellular China (iPad4,9)"));
            iPadMini.Add(new ComboBoxEntry("iPad5,1", "4 Wi-Fi (iPad5,1)"));
            iPadMini.Add(new ComboBoxEntry("iPad5,2", "4 Cellular (iPad5,2)"));

            iPhone = new List<ComboBoxEntry>();
            iPhone.Add(new ComboBoxEntry("iPhone1,1", "2G (iPhone1,1)"));
            iPhone.Add(new ComboBoxEntry("iPhone1,2", "3G (iPhone1,2)"));
            iPhone.Add(new ComboBoxEntry("iPhone2,1", "3GS (iPhone2,1)"));
            iPhone.Add(new ComboBoxEntry("iPhone3,1", "4 GSM (iPhone3,1)"));
            iPhone.Add(new ComboBoxEntry("iPhone3,2", "4 CDMA (iPhone3,2)"));
            iPhone.Add(new ComboBoxEntry("iPhone3,3", "4 GSM Rev A (iPhone3,3)"));
            iPhone.Add(new ComboBoxEntry("iPhone4,1", "4S (iPhone4,1)"));
            iPhone.Add(new ComboBoxEntry("iPhone5,1", "5 GSM (iPhone5,1)"));
            iPhone.Add(new ComboBoxEntry("iPhone5,2", "5 CDMA (iPhone5,2)"));
            iPhone.Add(new ComboBoxEntry("iPhone5,3", "5c GSM (iPhone5,3)"));
            iPhone.Add(new ComboBoxEntry("iPhone5,4", "5c Global (iPhone5,4)"));
            iPhone.Add(new ComboBoxEntry("iPhone6,1", "5s GSM (iPhone6,1)"));
            iPhone.Add(new ComboBoxEntry("iPhone6,2", "5s Global (iPhone6,2)"));
            iPhone.Add(new ComboBoxEntry("iPhone7,1", "6 Plus (iPhone7,1)"));
            iPhone.Add(new ComboBoxEntry("iPhone7,2", "6 (iPhone7,2)"));
            iPhone.Add(new ComboBoxEntry("iPhone8,1", "6s (iPhone8,1)"));
            iPhone.Add(new ComboBoxEntry("iPhone8,2", "6s Plus (iPhone8,2)"));

            iPodTouch = new List<ComboBoxEntry>();
            iPodTouch.Add(new ComboBoxEntry("iPod1,1", "1G (iPod1,1)"));
            iPodTouch.Add(new ComboBoxEntry("iPod2,1", "2G (iPod2,1)"));
            iPodTouch.Add(new ComboBoxEntry("iPod3,1", "3G (iPod3,1)"));
            iPodTouch.Add(new ComboBoxEntry("iPod4,1", "4G (iPod4,1)"));
            iPodTouch.Add(new ComboBoxEntry("iPod5,1", "5G (iPod5,1)"));
            iPodTouch.Add(new ComboBoxEntry("iPod7,1", "6G (iPod7,1)"));
        }
        private static void InitDevices()
        {
            PlistDocument keyList = new PlistDocument(
                Globals.GetStream("KeyList.plist"));
            PlistDict rootNode = (PlistDict)keyList.RootNode;

            // Apple TV
            InitDevice(ref AppleTV21, rootNode.Get<PlistArray>("AppleTV2,1"));
            InitDevice(ref AppleTV31, rootNode.Get<PlistArray>("AppleTV3,1"));
            InitDevice(ref AppleTV32, rootNode.Get<PlistArray>("AppleTV3,2"));
            InitDevice(ref AppleTV53, rootNode.Get<PlistArray>("AppleTV5,3"));

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
            InitDevice(ref iPad67, rootNode.Get<PlistArray>("iPad6,7"));
            InitDevice(ref iPad68, rootNode.Get<PlistArray>("iPad6,8"));

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
                //bool hasKeys = dict.Get<PlistBool>("Has Keys").Value;
                device.Add(new ComboBoxEntry(
                    build,
                    $"{version} ({build})"));
            }
        }
    }
}