
using System.Collections.Generic;

namespace Hexware.Programs.iDecryptIt
{
    internal static class KeySelectionLists
    {
        internal static List<ComboBoxEntry> Devices;

        internal static List<ComboBoxEntry> AppleTV;
        internal static List<ComboBoxEntry> iPad;
        internal static List<ComboBoxEntry> iPadMini;
        internal static List<ComboBoxEntry> iPhone;
        internal static List<ComboBoxEntry> iPodTouch;

        internal static void Init()
        {
            Devices = new List<ComboBoxEntry>();
            Devices.Add(new ComboBoxEntry("appletv", "Apple TV"));
            Devices.Add(new ComboBoxEntry("ipad", "iPad"));
            Devices.Add(new ComboBoxEntry("ipadmini", "iPad mini"));
            Devices.Add(new ComboBoxEntry("iphone", "iPhone"));
            Devices.Add(new ComboBoxEntry("ipodtouch", "iPod touch"));

            AppleTV = new List<ComboBoxEntry>();
            AppleTV.Add(new ComboBoxEntry("AppleTV2,1", "2G (AppleTV2,1)"));
            AppleTV.Add(new ComboBoxEntry("AppleTV3,1", "3G (AppleTV3,1)"));
            AppleTV.Add(new ComboBoxEntry("AppleTV3,2", "3G Rev A (AppleTV3,2)"));

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
            iPad.Add(new ComboBoxEntry("iPad4,3", "Air Cellular Rev A (iPad4,3)"));

            iPadMini = new List<ComboBoxEntry>();
            iPadMini.Add(new ComboBoxEntry("iPad2,5", "1G Wi-Fi (iPad2,5)"));
            iPadMini.Add(new ComboBoxEntry("iPad2,5", "1G GSM (iPad2,5)"));
            iPadMini.Add(new ComboBoxEntry("iPad2,5", "1G Global (iPad2,5)"));
            iPadMini.Add(new ComboBoxEntry("iPad4,4", "2G Wi-Fi (iPad4,4)"));
            iPadMini.Add(new ComboBoxEntry("iPad4,5", "2G Cellular (iPad4,5)"));
            iPadMini.Add(new ComboBoxEntry("iPad4,6", "2G Cellular Rev A (iPad4,6)"));

            iPhone = new List<ComboBoxEntry>();
            iPhone.Add(new ComboBoxEntry("iPhone1,1", "2G (iPhone1,1)"));
            iPhone.Add(new ComboBoxEntry("iPhone1,2", "3G (iPhone1,2)"));
            iPhone.Add(new ComboBoxEntry("iPhone2,1", "3GS (iPhone2,1)"));
            iPhone.Add(new ComboBoxEntry("iPhone3,1", "4 GSM (iPhone3,1)"));
            iPhone.Add(new ComboBoxEntry("iPhone3,2", "4 CDMA (iPhone3,2)"));
            iPhone.Add(new ComboBoxEntry("iPhone3,3", "4 GSM Rev A (iPhone3,3)"));
            iPhone.Add(new ComboBoxEntry("iPhone4,1", "4S (iPhone4,1)"));
            iPhone.Add(new ComboBoxEntry("iPhone5,1", "5 GSM (iPhone5,1)"));
            iPhone.Add(new ComboBoxEntry("iPhone5,2", "5 Global (iPhone5,2)"));
            iPhone.Add(new ComboBoxEntry("iPhone5,3", "5c GSM (iPhone5,3)"));
            iPhone.Add(new ComboBoxEntry("iPhone5,4", "5c Global (iPhone5,4)"));
            iPhone.Add(new ComboBoxEntry("iPhone6,1", "5s GSM (iPhone6,1)"));
            iPhone.Add(new ComboBoxEntry("iPhone6,2", "5s Global (iPhone6,2)"));

            iPodTouch = new List<ComboBoxEntry>();
            iPodTouch.Add(new ComboBoxEntry("iPod1,1", "1G (iPod1,1)"));
            iPodTouch.Add(new ComboBoxEntry("iPod2,1", "2G (iPod2,1)"));
            iPodTouch.Add(new ComboBoxEntry("iPod3,1", "3G (iPod3,1)"));
            iPodTouch.Add(new ComboBoxEntry("iPod4,1", "4G (iPod4,1)"));
            iPodTouch.Add(new ComboBoxEntry("iPod5,1", "5G (iPod5,1)"));
        }
    }
}
