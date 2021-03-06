/* =============================================================================
 * File:   Globals.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2012-2018 Cole Johnson
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
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace Hexware.Programs.iDecryptIt
{
    internal static class Globals
    {
        internal static string Version;
        internal static string Version64;
        internal static DateTime CompileTimestamp;
        internal static Dictionary<string, string> ExecutionArgs = new Dictionary<string, string>();
        internal static bool Debug;
        internal static Firmware.TarFile KeyArchive;

        internal static Dictionary<string, string> DeviceNames = new Dictionary<string, string>() {
            { "AppleTV2,1", "Apple TV 2G" },
            { "AppleTV3,1", "Apple TV 3G" },
            { "AppleTV3,2", "Apple TV 3G [Rev A]" },
            { "AppleTV5,3", "Apple TV 4G" },
            { "AppleTV6,2", "Apple TV 4K" },

            { "iPad2,5", "iPad mini (Wi-Fi)" },
            { "iPad2,6", "iPad mini (GSM)" },
            { "iPad2,7", "iPad mini (Global)" },
            { "iPad4,4", "iPad mini 2 (Wi-Fi)" },
            { "iPad4,5", "iPad mini 2 (Cellular)" },
            { "iPad4,6", "iPad mini 2 (Cellular China)" },
            { "iPad4,7", "iPad mini 3 (Wi-Fi)" },
            { "iPad4,8", "iPad mini 3 (Cellular)" },
            { "iPad4,9", "iPad mini 3 (Cellular China)" },
            { "iPad5,1", "iPad mini 4 (Wi-Fi)" },
            { "iPad5,2", "iPad mini 4 (Cellular)" },

            { "iPad1,1", "iPad 1G" },
            { "iPad2,1", "iPad 2 (Wi-Fi)" },
            { "iPad2,2", "iPad 2 (GSM)" },
            { "iPad2,3", "iPad 2 (CDMA)" },
            { "iPad2,4", "iPad 2 (Wi-Fi) [Rev A]" },
            { "iPad3,1", "iPad 3 (Wi-Fi)" },
            { "iPad3,2", "iPad 3 (CDMA)" },
            { "iPad3,3", "iPad 3 (Global)" },
            { "iPad3,4", "iPad 4 (Wi-Fi)" },
            { "iPad3,5", "iPad 4 (GSM)" },
            { "iPad3,6", "iPad 4 (Global)" },
            { "iPad4,1", "iPad Air (Wi-Fi)" },
            { "iPad4,2", "iPad Air (Cellular)" },
            { "iPad4,3", "iPad Air (Cellular China)" },
            { "iPad5,3", "iPad Air 2 (Wi-Fi)" },
            { "iPad5,4", "iPad Air 2 (Cellular)" },
            { "iPad6,3", "iPad Pro 9.7\" (Wi-Fi)" },
            { "iPad6,4", "iPad Pro 9.7\" (Cellular)" },
            { "iPad6,7", "iPad Pro 12.9\" (Wi-Fi)" },
            { "iPad6,8", "iPad Pro 12.9\" (Cellular)" },
            { "iPad6,11", "iPad 5 (Wi-Fi)" },
            { "iPad6,12", "iPad 5 (Cellular)" },
            { "iPad7,1", "iPad Pro 2 12.9\" (Wi-Fi)" },
            { "iPad7,2", "iPad Pro 2 12.9\" (Cellular)" },
            { "iPad7,3", "iPad Pro 2 10.5\" (Wi-Fi)" },
            { "iPad7,4", "iPad Pro 2 10.5\" (Cellular)" },
            { "iPad7,5", "iPad 6 (Wi-Fi)" },
            { "iPad7,6", "iPad 6 (Cellular)" },

            { "iPhone1,1", "iPhone 2G" },
            { "iPhone1,2", "iPhone 3G" },
            { "iPhone2,1", "iPhone 3GS" },
            { "iPhone3,1", "iPhone 4 (GSM)" },
            { "iPhone3,2", "iPhone 4 (GSM) [Rev A]" },
            { "iPhone3,3", "iPhone 4 (CDMA)" },
            { "iPhone4,1", "iPhone 4S" },
            { "iPhone5,1", "iPhone 5 (iPhone5,1)" }, // TODO (A1428)
            { "iPhone5,2", "iPhone 5 (iPhone5,2)" }, // TODO (A1429, A1442)
            { "iPhone5,3", "iPhone 5c (iPhone5,3)" }, // North America, Japan
            { "iPhone5,4", "iPhone 5c (iPhone5,4)" }, // Asia-Pacific, China, Europe, Middle East
            { "iPhone6,1", "iPhone 5s (iPhone6,1)" }, // Asia-Pacific, China, Europe, Middle East
            { "iPhone6,2", "iPhone 5s (iPhone6,2)" }, // North America, Japan
            { "iPhone7,1", "iPhone 6 Plus" },
            { "iPhone7,2", "iPhone 6" },
            { "iPhone8,1", "iPhone 6s" },
            { "iPhone8,2", "iPhone 6s Plus" },
            { "iPhone8,4", "iPhone SE" },
            { "iPhone9,1", "iPhone 7 (iPhone9,1)" }, // VZW, Sprint, China, Japan
            { "iPhone9,3", "iPhone 7 (iPhone9,3)" }, // AT&T, TM, Global
            { "iPhone9,2", "iPhone 7 Plus (iPhone9,2)" }, // VZW, Sprint, China, Japan
            { "iPhone9,4", "iPhone 7 Plus (iPhone9,4)" }, // AT&T, TM, Global
            { "iPhone10,1", "iPhone 8 (iPhone10,1)" },
            { "iPhone10,4", "iPhone 8 (iPhone10,4)" },
            { "iPhone10,2", "iPhone 8 Plus (iPhone10,2)" },
            { "iPhone10,5", "iPhone 8 Plus (iPhone10,5)" },
            { "iPhone10,3", "iPhone X (iPhone10,3)" },
            { "iPhone10,6", "iPhone X (iPhone10,6)" },

            { "iPod1,1", "iPod touch 1G" },
            { "iPod2,1", "iPod touch 2G" },
            { "iPod3,1", "iPod touch 3G" },
            { "iPod4,1", "iPod touch 4G" },
            { "iPod5,1", "iPod touch 5G" },
            { "iPod7,1", "iPod touch 6G" }
        };

        internal static void Init()
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();

            // Microsoft is weird and uses version numbers of the form
            //   {major}.{minor}.{build}.{revision} while we use (the sane)
            //   {major}.{minor}.{revision}.{build}.
            Version ver = thisAssembly.GetName().Version;
            char[] build = ver.Revision.ToString().ToCharArray();
            build[0]++;
            build[1] = (char)(build[1] - '0' + 'A'); // second character is a letter; map [0,9] to [A,J]
            Version = String.Format(
                "{0}.{1:D2}.{2}.{3}",
                ver.Major, ver.Minor, ver.Build, new String(build));

            Version64 = (Environment.Is64BitProcess) ? " x64" : "";

#if DEBUG
            Debug = true;
#endif

            CompileTimestamp = GetLinkerTimestampUTC(thisAssembly);

            DecompressKeys();
        }

        internal static Stream GetStream(string resourceName)
        {
            Assembly assy = Assembly.GetExecutingAssembly();
            string[] resources = assy.GetManifestResourceNames();
            for (int i = 0; i < resources.Length; i++)
            {
                if (resources[i].ToLower().Contains(resourceName.ToLower()))
                    return assy.GetManifestResourceStream(resources[i]);
            }
            return Stream.Null;
        }

        private static DateTime GetLinkerTimestampUTC(Assembly assembly)
        {
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[512];

            Stream s = null;
            try {
                s = new FileStream(assembly.Location, FileMode.Open, FileAccess.Read);
                s.Read(b, 0, 512);
            } catch (Exception) {
                return DateTime.MinValue;
            } finally {
                if (s != null)
                    s.Dispose();
            }

            int i = BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(secondsSince1970);
        }

        private static void DecompressKeys()
        {
            GZipStream compressedKeys = new GZipStream(GetStream("keys.tar.gz"), CompressionMode.Decompress);
            MemoryStream decompressedKeys = new MemoryStream();

            compressedKeys.CopyTo(decompressedKeys);
            compressedKeys.Close();

            decompressedKeys.Seek(0, SeekOrigin.Begin);
            KeyArchive = new Firmware.TarFile(decompressedKeys);
        }
    }
}