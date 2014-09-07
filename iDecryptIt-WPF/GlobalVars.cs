/* =============================================================================
 * File:   GlobalVars.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2012-2014, Cole Johnson
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
using System.Reflection;

namespace Hexware.Programs.iDecryptIt
{
    internal static class GlobalVars
    {
        internal static string Version;
        internal static string Version64;
        internal static DateTime CompileTimestamp;
        internal static Dictionary<string, string> ExecutionArgs = new Dictionary<string, string>();

        internal static Dictionary<string, string> DeviceNames = new Dictionary<string, string>() {
            { "AppleTV2,1", "Apple TV 2G" },
            { "AppleTV3,1", "Apple TV 3G" },
            { "AppleTV3,2", "Apple TV 3G (Rev A)" },
            { "iPad1,1", "iPad 1G" },
            { "iPad2,1", "iPad 2 (Wi-Fi)" },
            { "iPad2,2", "iPad 2 (GSM)" },
            { "iPad2,3", "iPad 2 (CDMA)" },
            { "iPad2,4", "iPad 2 (Wi-Fi) [Rev A]" },
            { "iPad2,5", "iPad mini 1G (Wi-Fi)" },
            { "iPad2,6", "iPad mini 1G (GSM)" },
            { "iPad2,7", "iPad mini 1G (Global)" },
            { "iPad3,1", "iPad 3 (Wi-Fi)" },
            { "iPad3,2", "iPad 3 (CDMA)" },
            { "iPad3,3", "iPad 3 (Global)" },
            { "iPad3,4", "iPad 4 (Wi-Fi)" },
            { "iPad3,5", "iPad 4 (GSM)" },
            { "iPad3,6", "iPad 4 (Global)" },
            { "iPad4,1", "iPad Air (Wi-Fi)" },
            { "iPad4,2", "iPad Air (Cellular)" },
            { "iPad4,3", "iPad Air (Cellular) [Rev A]" },
            { "iPad4,4", "iPad mini 2G (Wi-Fi)" },
            { "iPad4,5", "iPad mini 2G (Cellular)" },
            { "iPad4,6", "iPad mini 2G (Cellular) [Rev A]" },
            { "iPhone1,1", "iPhone 2G" },
            { "iPhone1,2", "iPhone 3G" },
            { "iPhone2,1", "iPhone 3GS" },
            { "iPhone3,1", "iPhone 4 (GSM)" },
            { "iPhone3,2", "iPhone 4 (GSM) [Rev A]" },
            { "iPhone3,3", "iPhone 4 (CDMA)" },
            { "iPhone4,1", "iPhone 4S" },
            { "iPhone5,1", "iPhone 5 (GSM)" },
            { "iPhone5,2", "iPhone 5 (Global)" },
            { "iPhone5,3", "iPhone 5c (GSM)" },
            { "iPhone5,4", "iPhone 5c (Global)" },
            { "iPhone6,1", "iPhone 5s (GSM)" },
            { "iPhone6,2", "iPhone 5s (Global)" },
            { "iPod1,1", "iPod touch 1G" },
            { "iPod2,1", "iPod touch 2G" },
            { "iPod3,1", "iPod touch 3G" },
            { "iPod4,1", "iPod touch 4G" },
            { "iPod5,1", "iPod touch 5G" }
        };

        internal static void Init(string[] args)
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();

            Version ver = thisAssembly.GetName().Version;
            char[] build = ver.Revision.ToString().ToCharArray();
            build[0]++;
            build[1] = (char)(build[1] - '0' + 'A');
            Version = String.Format(
                "{0}.{1:D2}.{2}.{3}",
                ver.Major, ver.Minor, ver.Build, new String(build));

            Version64 = (Environment.Is64BitProcess) ? " x64" : "";

            CompileTimestamp = GetLinkerTimestampUTC(thisAssembly);
        }

        private static DateTime GetLinkerTimestampUTC(Assembly assembly)
        {
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            Stream s = null;

            try {
                s = new FileStream(assembly.Location, FileMode.Open, FileAccess.Read);
                s.Read(b, 0, 2048);
            } catch (Exception) {
                return new DateTime();
            } finally {
                if (s != null)
                    s.Close();
            }

            int i = BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds(secondsSince1970);
            return dt;
        }
    }
}