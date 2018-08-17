﻿/* =============================================================================
 * File:   Program.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2012-2018, Cole Johnson
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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml;

namespace Hexware.Programs.iDecryptIt.KeyGrabber
{
    public struct FirmwareVersion
    {
        public string Version;
        public string Build;
        public bool HasKeys;

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(FirmwareVersion))
                return false;
            FirmwareVersion other = (FirmwareVersion)obj;
            return Build == other.Build;
        }

        public override int GetHashCode()
        {
            var hashCode = -1408460819;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Version);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Build);
            hashCode = hashCode * -1521134295 + HasKeys.GetHashCode();
            return hashCode;
        }
    }
    public class Program
    {
        static WebClient client = new WebClient();
        static List<string> pages = new List<string>();
        static string curDir = Directory.GetCurrentDirectory();
        static string keyDir = Path.Combine(curDir, "keys");
        static string plutil = "C:\\Program Files\\Common Files\\Apple\\Apple Application Support\\plutil.exe";
        static bool makeBinaryPlists = false;
        static Dictionary<string, List<FirmwareVersion>> versionsList = new Dictionary<string, List<FirmwareVersion>>();

        public static void Main(string[] args)
        {
            // CreateDirectory(...) sometimes fails unless we wait (race condition?)
            if (Directory.Exists(keyDir))
                Directory.Delete(keyDir, true);
            Thread.Sleep(100);
            Directory.CreateDirectory(keyDir);

#if !DEBUG
            // Currently, the .tar.gz file of binary Plists is bigger than the one for text Plists
            //makeBinaryPlists = true;
#endif

            if (makeBinaryPlists && !File.Exists(plutil))
            {
                makeBinaryPlists = false;
                Console.WriteLine("WARNING: plutil not found! Binary plists will NOT be generated.");
            }

            InitVersionsList();

            // TODO: Parse the key page using XPath (action=render) [id tags]
            EnumerateFirmwareListAndSaveKeys("Firmware/Apple_TV");
            //EnumerateFirmwareListAndSaveKeys("Firmware/Apple_Watch");
            EnumerateFirmwareListAndSaveKeys("Firmware/iPad");
            EnumerateFirmwareListAndSaveKeys("Firmware/iPad_mini");
            EnumerateFirmwareListAndSaveKeys("Firmware/iPhone");
            EnumerateFirmwareListAndSaveKeys("Firmware/iPod_touch");

            /*foreach (string title in GetKeyPages("https://www.theiphonewiki.com/wiki/Firmware/iPhone/11.x"))
            {
                Console.WriteLine(title);
                ParseAndSaveKeyPage(client.DownloadString(
                    "http://theiphonewiki.com/w/index.php?title=" + title + "&action=raw"));
            }*/

            // Build version listing
            PlistDict plistRoot = new PlistDict();
            foreach (var deviceList in versionsList)
            {
                List<FirmwareVersion> versions = deviceList.Value.Distinct().ToList();
                PlistArray deviceArr = new PlistArray(new IPlistElement[versions.Count]);
                for (int i = 0; i < versions.Count; i++)
                {
                    FirmwareVersion ver = versions[i];
                    PlistDict versionDict = new PlistDict();
                    versionDict.Add("Build", new PlistString(ver.Build));
                    versionDict.Add("Version", new PlistString(ver.Version));
                    versionDict.Add("Has Keys", new PlistBool(ver.HasKeys));
                    deviceArr.Set(i, versionDict);
                }
                plistRoot.Add(deviceList.Key, deviceArr);
            }

            // Save version listing
            PlistDocument versionDoc = new PlistDocument(plistRoot);
            string keyListPath = Path.Combine(keyDir, "KeyList.plist");
            versionDoc.Save(keyListPath, PlistDocumentType.Xml);
            ConvertPlist(keyListPath);
        }
        private static void InitVersionsList()
        {
            // Apple TV
            versionsList.Add("AppleTV2,1", new List<FirmwareVersion>());
            versionsList.Add("AppleTV3,1", new List<FirmwareVersion>());
            versionsList.Add("AppleTV3,2", new List<FirmwareVersion>());
            versionsList.Add("AppleTV5,3", new List<FirmwareVersion>());
            versionsList.Add("AppleTV6,2", new List<FirmwareVersion>());

            // iPad
            versionsList.Add("iPad1,1", new List<FirmwareVersion>());
            versionsList.Add("iPad2,1", new List<FirmwareVersion>());
            versionsList.Add("iPad2,2", new List<FirmwareVersion>());
            versionsList.Add("iPad2,3", new List<FirmwareVersion>());
            versionsList.Add("iPad2,4", new List<FirmwareVersion>());
            versionsList.Add("iPad3,1", new List<FirmwareVersion>());
            versionsList.Add("iPad3,2", new List<FirmwareVersion>());
            versionsList.Add("iPad3,3", new List<FirmwareVersion>());
            versionsList.Add("iPad3,4", new List<FirmwareVersion>());
            versionsList.Add("iPad3,5", new List<FirmwareVersion>());
            versionsList.Add("iPad3,6", new List<FirmwareVersion>());
            versionsList.Add("iPad4,1", new List<FirmwareVersion>());
            versionsList.Add("iPad4,2", new List<FirmwareVersion>());
            versionsList.Add("iPad4,3", new List<FirmwareVersion>());
            versionsList.Add("iPad5,3", new List<FirmwareVersion>());
            versionsList.Add("iPad5,4", new List<FirmwareVersion>());
            versionsList.Add("iPad6,3", new List<FirmwareVersion>());
            versionsList.Add("iPad6,4", new List<FirmwareVersion>());
            versionsList.Add("iPad6,7", new List<FirmwareVersion>());
            versionsList.Add("iPad6,8", new List<FirmwareVersion>());
            versionsList.Add("iPad6,11", new List<FirmwareVersion>());
            versionsList.Add("iPad6,12", new List<FirmwareVersion>());
            versionsList.Add("iPad7,1", new List<FirmwareVersion>());
            versionsList.Add("iPad7,2", new List<FirmwareVersion>());
            versionsList.Add("iPad7,3", new List<FirmwareVersion>());
            versionsList.Add("iPad7,4", new List<FirmwareVersion>());
            versionsList.Add("iPad7,5", new List<FirmwareVersion>());
            versionsList.Add("iPad7,6", new List<FirmwareVersion>());

            // iPad mini
            versionsList.Add("iPad2,5", new List<FirmwareVersion>());
            versionsList.Add("iPad2,6", new List<FirmwareVersion>());
            versionsList.Add("iPad2,7", new List<FirmwareVersion>());
            versionsList.Add("iPad4,4", new List<FirmwareVersion>());
            versionsList.Add("iPad4,5", new List<FirmwareVersion>());
            versionsList.Add("iPad4,6", new List<FirmwareVersion>());
            versionsList.Add("iPad4,7", new List<FirmwareVersion>());
            versionsList.Add("iPad4,8", new List<FirmwareVersion>());
            versionsList.Add("iPad4,9", new List<FirmwareVersion>());
            versionsList.Add("iPad5,1", new List<FirmwareVersion>());
            versionsList.Add("iPad5,2", new List<FirmwareVersion>());

            // iPhone
            versionsList.Add("iPhone1,1", new List<FirmwareVersion>());
            versionsList.Add("iPhone1,2", new List<FirmwareVersion>());
            versionsList.Add("iPhone2,1", new List<FirmwareVersion>());
            versionsList.Add("iPhone3,1", new List<FirmwareVersion>());
            versionsList.Add("iPhone3,2", new List<FirmwareVersion>());
            versionsList.Add("iPhone3,3", new List<FirmwareVersion>());
            versionsList.Add("iPhone4,1", new List<FirmwareVersion>());
            versionsList.Add("iPhone5,1", new List<FirmwareVersion>());
            versionsList.Add("iPhone5,2", new List<FirmwareVersion>());
            versionsList.Add("iPhone5,3", new List<FirmwareVersion>());
            versionsList.Add("iPhone5,4", new List<FirmwareVersion>());
            versionsList.Add("iPhone6,1", new List<FirmwareVersion>());
            versionsList.Add("iPhone6,2", new List<FirmwareVersion>());
            versionsList.Add("iPhone7,1", new List<FirmwareVersion>());
            versionsList.Add("iPhone7,2", new List<FirmwareVersion>());
            versionsList.Add("iPhone8,1", new List<FirmwareVersion>());
            versionsList.Add("iPhone8,2", new List<FirmwareVersion>());
            versionsList.Add("iPhone8,4", new List<FirmwareVersion>());
            versionsList.Add("iPhone9,1", new List<FirmwareVersion>());
            versionsList.Add("iPhone9,3", new List<FirmwareVersion>());
            versionsList.Add("iPhone9,2", new List<FirmwareVersion>());
            versionsList.Add("iPhone9,4", new List<FirmwareVersion>());
            versionsList.Add("iPhone10,1", new List<FirmwareVersion>());
            versionsList.Add("iPhone10,4", new List<FirmwareVersion>());
            versionsList.Add("iPhone10,2", new List<FirmwareVersion>());
            versionsList.Add("iPhone10,5", new List<FirmwareVersion>());
            versionsList.Add("iPhone10,3", new List<FirmwareVersion>());
            versionsList.Add("iPhone10,6", new List<FirmwareVersion>());

            // iPod touch
            versionsList.Add("iPod1,1", new List<FirmwareVersion>());
            versionsList.Add("iPod2,1", new List<FirmwareVersion>());
            versionsList.Add("iPod3,1", new List<FirmwareVersion>());
            versionsList.Add("iPod4,1", new List<FirmwareVersion>());
            versionsList.Add("iPod5,1", new List<FirmwareVersion>());
            versionsList.Add("iPod7,1", new List<FirmwareVersion>());
        }
        private static void EnumerateFirmwareListAndSaveKeys(string page)
        {
            XmlDocument doc = new XmlDocument
            {
                InnerXml = "<doc>" + client.DownloadString(
                    "http://theiphonewiki.com/w/index.php?title=" + page + "&action=render") + "</doc>"
            };

            // Parse the major version lists
            foreach (XmlNode majorVersion in doc.SelectNodes(".//a"))
            {
                foreach (XmlAttribute link in majorVersion.Attributes)
                {
                    if (link.Name == "href" && link.Value.Contains(page))
                    {
                        // Found a major version page (eg. Firmware/Apple_TV/4.x)
                        foreach (string title in GetKeyPages(link.Value))
                        {
                            Console.WriteLine(title);
                            ParseAndSaveKeyPage(client.DownloadString(
                                "http://theiphonewiki.com/w/index.php?title=" + title + "&action=raw"));
                        }
                    }
                }
            }
        }
        private static IEnumerable<string> GetKeyPages(string url)
        {
            url = "http://theiphonewiki.com/w/index.php?title=" + url.Substring(url.IndexOf("wiki/") + 5) + "&action=render";

            // MediaWiki outputs valid [X]HTML...sortove
            XmlDocument doc = new XmlDocument
            {
                InnerXml = "<doc>" + client.DownloadString(url) + "</doc>"
            };
            XmlNodeList tableList = doc.SelectNodes("//table[@class='wikitable']");
            Debug.Assert(tableList.Count != 0, "Can't find device tables.");
            foreach (XmlNode table in tableList)
            {
                foreach (string fwPage in ParseTable(table))
                    yield return fwPage;
            }
        }
        private static IEnumerable<string> ParseTable(XmlNode table)
        {
            FixColspans(table);
            FixRowspans(table);

            bool isSpecialATVFormat = false;
            int rowNum = -1;
            foreach (XmlNode row in table.ChildNodes[0].ChildNodes)
            {
                rowNum++;
                
                // skip headers
                if (row.InnerText.Contains("Version"))
                    continue;
                if (row.InnerText.Contains("Marketing") && row.InnerText.Contains("Internal"))
                {
                    isSpecialATVFormat = true;
                    continue;
                }
                
                FirmwareVersion ver = new FirmwareVersion();
                XmlNode buildCell = null;
                if (isSpecialATVFormat)
                {
                    string marketing = row.ChildNodes[0].InnerText.Trim();
                    string @internal = row.ChildNodes[1].InnerText.Trim();

                    if (marketing == @internal)
                    {
                        // Should only be true on ATV-4.3 (8F455 - 2557)
                        Debug.Assert(row.ChildNodes[2].InnerText.Trim() == "2557");
                        ver.Version = "4.3";
                    }
                    else
                    {
                        ver.Version = String.Format(
                            "{0}/{1}",
                            marketing,
                            @internal);
                    }
                    buildCell = row.ChildNodes[3];
                }
                else
                {
                    ver.Version = row.ChildNodes[0].InnerText.Trim();
                    buildCell = row.ChildNodes[1];
                }
                ver.Build = buildCell.InnerText.Trim();

                // Don't add a version we've already seen (FixRowspans(...) causes this)
                // Example: iPhone 2G 1.0.1 (1C25) and 1.0.2 (1C28)
                /*bool isDup = false;
                foreach (FirmwareVersion testVer in versionList)
                {
                    if (ver.Build == testVer.Build)
                    {
                        isDup = true;
                        break;
                    }
                }
                if (isDup)
                    continue;*/
                
                XmlNodeList keyPageUrl = buildCell.NextSibling.SelectNodes(".//@href");
                if (keyPageUrl.Count == 0)
                {
                    // This build doesn't have an IPSW. For now, just add the
                    //   version to the list, but don't yield a URL. When adding
                    //   support for betas, this logic will need to be redone.
                    ver.HasKeys = false;
                    //versionList.Add(ver);
                    continue;
                }
                foreach (XmlNode urlNode in keyPageUrl)
                {
                    string url = urlNode.Value;
                    string device = url.Substring(
                        url.IndexOf('(') + 1,
                        url.IndexOf(')') - url.IndexOf('(') - 1);

                    if (url.Contains("redlink"))
                    {
                        ver.HasKeys = false;
                        versionsList[device].Add(ver);
                        continue;
                    }

                    ver.HasKeys = true;
                    versionsList[device].Add(ver);
                    url = url.Substring(url.IndexOf("/wiki/") + "/wiki/".Length);
                    yield return url;
                }
            }
        }
        private static void FixRowspans(XmlNode table)
        {
            // This method is a pretty inefficient way IMHO of fixing the problem,
            //   but compared to the time the web request for the page takes, this
            //   is nothing.
            XmlNodeList rows = table.ChildNodes[0].ChildNodes;
            int rowCount = rows.Count;

            // Subtract 1 to ignore the documentation column (it causes
            //   problems when using XmlNode.InsertBefore(...) and we
            //   don't care about it)
            int colCount = rows[0].ChildNodes.Count - 1;
            int startRow = 1;
            if (rows[1].InnerText.Contains("Marketing"))
                startRow = 2;

            for (int col = 0; col < colCount; col++)
            {
                for (int row = startRow; row < rowCount; row++)
                {
                    XmlNode cell = rows[row].ChildNodes[col];
                    Debug.Assert(cell != null);
                restart:
                    foreach (XmlAttribute attr in cell.Attributes)
                    {
                        if (attr.Name != "rowspan")
                            continue;
                        int val = Convert.ToInt32(attr.Value);
                        Debug.Assert(val >= 2);
                        cell.Attributes.Remove(attr);
                        for (int i = 1; i < val; i++)
                        {
                            // Insert the new cell before the cell currently occupying the space we want
                            XmlNode rowToAddTo = rows[row + i];
                            if (rowToAddTo == null)
                            {
                                Console.WriteLine("ERROR");
                                break;
                            }
                            rowToAddTo.InsertBefore(cell.Clone(), rowToAddTo.ChildNodes[col]);
                        }
                        // We aren't allowed to modify the collection while enumerating,
                        //   so if we change it, we need to restart the enumeration
                        goto restart;
                    }
                }
            }
        }
        private static void FixColspans(XmlNode table)
        {
            foreach (XmlNode row in table.ChildNodes[0].ChildNodes)
            {
            restart:
                foreach (XmlNode cell in row.ChildNodes)
                {
                    foreach (XmlAttribute attr in cell.Attributes)
                    {
                        if (attr.Name != "colspan")
                            continue;
                        int val = Convert.ToInt32(attr.Value);
                        Debug.Assert(val >= 2);
                        cell.Attributes.Remove(attr);
                        for (int i = 1; i < val; i++)
                            row.InsertAfter(cell.Clone(), cell);
                        // We aren't allowed to modify the collection while enumerating,
                        //   so if we change it, we need to restart the enumeration
                        goto restart;
                    }
                }
            }
        }
        private static void ParseAndSaveKeyPage(string contents)
        {
            string[] lines = contents
                .Replace("{{keys", "")
                .Replace("}}", "")
                .Split(new char[] { '\n', '\r' }, 200, StringSplitOptions.RemoveEmptyEntries);

            string displayVersion = null;
            Dictionary<string, string> data = new Dictionary<string, string>();
            for (int i = 0; i < lines.Length; i++) {
                Debug.Assert(lines[i].StartsWith(" | "));
                lines[i] = lines[i].Substring(3); // Remove " | "
                string key = lines[i].Split(' ')[0];
                string value = lines[i].Split('=')[1];
                if (key == "DisplayVersion") {
                    displayVersion = value.Trim();
                    continue;
                } else if (key == "Device") {
                    Debug.Assert(value.Contains(","));
                } else if (key == "DownloadURL") {
                    key = "Download URL";
                    continue; // Ignore for now
                } else if (key == "RootFS" || key == "GMRootFS" || key == "UpdateRamdisk" || key == "RestoreRamdisk") {
                    if (String.IsNullOrWhiteSpace(value))
                        value = "XXX-XXXX-XXX";
                } else if (key.StartsWith("SEPFirmware")) {
                    key = key.Replace("SEPFirmware", "SEP-Firmware");
                }

                data.Add(key, value.Trim());
            }
            if (displayVersion != null && data["Device"].StartsWith("AppleTV"))
                data["Version"] = displayVersion; // Will need to be updated to handle betas

            // the rowspan fixer messes with these builds, so don't worry if it already exists, we'd be saving the same data
            string filename = Path.Combine(keyDir, data["Device"] + "_" + data["Build"] + ".plist");
            if (filename.EndsWith("iPhone1,1_1C25.plist") || filename.EndsWith("iPhone1,1_1C28.plist"))
                if (File.Exists(filename))
                    return;
            Debug.Assert(!File.Exists(filename), filename);

            PlistDocument doc = new PlistDocument(BuildPlist(data));
            doc.Save(filename, PlistDocumentType.Xml);
            ConvertPlist(filename);
        }
        private static void ConvertPlist(string path)
        {
            if (makeBinaryPlists)
            {
                Process proc = new Process();
                proc.StartInfo.FileName = plutil;
                proc.StartInfo.Arguments = $"-convert binary1 \"{path}\"";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.OutputDataReceived += Proc_OutputDataRecieved;
                proc.ErrorDataReceived += Proc_OutputDataRecieved;
                proc.Start();
                proc.WaitForExit();

                // verify file converted correctly and can be loaded
                PlistDocument doc = new PlistDocument(path);
                Debug.Assert(doc.RootNode != null);
            }
        }
        private static PlistDict BuildPlist(Dictionary<string, string> data)
        {
            PlistDict dict = new PlistDict();

            int length;
            string debug = data["Codename"] + " " + data["Build"] + " (" + data["Device"] + "): ";
            PlistDict elem;

            // We could save some space by saving the IVs and keys in Base64 (len*4/3)
            //   instead of a hex string (len*2) (use PlistData)
            foreach (string key in data.Keys) {
                switch (key) {
                    case "Version":
                        string temp = data["Version"];
                        if (temp.Contains("and"))
                        {
                            // will need to be updated for beta support
                            //Match match = new Regex(@"^([\d\.]+)[^(]+\(([\d\.]+)").Match(temp);
                            //temp = $"{match.Groups[1].Value} ({match.Groups[2].Value})";
                            temp = temp.Substring(temp.IndexOf("and") + 4);
                            Console.WriteLine(" <<>> \"{0}\" --> \"{1}\"", data[key], temp);
                        }
                        dict.Add("Version", new PlistString(temp));
                        break;

                    case "Build":
                    case "Device":
                    case "Codename":
                    case "Download URL":
                    case "Baseband":
                    case "Model":
                    case "Model2":
                        dict.Add(key, new PlistString(data[key]));
                        break;

                    case "RootFS":
                        elem = new PlistDict();
                        elem.Add("File Name", new PlistString(data["RootFS"] + ".dmg"));
                        if (data[key + "Key"] == "Not Encrypted") {
                            elem.Add("Encryption", new PlistBool(false));
                        } else {
                            elem.Add("Encryption", new PlistBool(true));
                            elem.Add("Key", new PlistString(data["RootFSKey"]));
                            length = data["RootFSKey"].Length;
                            Debug.Assert(length == 72 || length == 7, $"{debug}data[\"RootFSKey\"].Length ({length}) != 72)");
                        }
                        dict.Add("Root FS", elem);
                        break;

                    case "UpdateRamdisk":
                    case "UpdateRamdisk2":
                    case "RestoreRamdisk":
                    case "RestoreRamdisk2":
                        elem = new PlistDict();
                        elem.Add("File Name", new PlistString(data[key] + ".dmg"));
                        if (data[key + "IV"] == "Not Encrypted") {
                            elem.Add("Encryption", new PlistBool(false));
                        } else {
                            elem.Add("Encryption", new PlistBool(true));
                            elem.Add("IV", new PlistString(data[key + "IV"]));
                            elem.Add("Key", new PlistString(data[key + "Key"]));
                            length = data[key + "IV"].Length;
                            Debug.Assert(length == 32 || length == 7, $"{debug}data[\"{key}IV\"].Length ({length}) != 32");
                            length = data[key + "Key"].Length;
                            Debug.Assert(length == 32 || length == 64 || length == 7, $"{debug}data[\"{key}Key\"].Length ({length}) != (32 || 64)");
                        }
                        dict.Add(key.Replace("Ramdisk", " Ramdisk"), elem);
                        break;

                    case "AOPFirmware":
                    //case "AOPFirmware2":
                    case "AppleLogo":
                    case "AppleLogo2":
                    case "AppleMaggie":
                    //case "AppleMaggie2":
                    case "AudioDSP":
                    //case "AudioDSP2":
                    case "BatteryCharging":
                    //case "BatteryCharging2":
                    case "BatteryCharging0":
                    case "BatteryCharging02":
                    case "BatteryCharging1":
                    case "BatteryCharging12":
                    case "BatteryFull":
                    case "BatteryFull2":
                    case "BatteryLow0":
                    case "BatteryLow02":
                    case "BatteryLow1":
                    case "BatteryLow12":
                    case "Dali":
                    //case "Dali2":
                    case "DeviceTree":
                    case "DeviceTree2":
                    case "GlyphCharging":
                    //case "GlyphCharging2":
                    case "GlyphPlugin":
                    case "GlyphPlugin2":
                    case "Homer":
                    //case "Homer2":
                    case "iBEC":
                    case "iBEC2":
                    case "iBoot":
                    case "iBoot2":
                    case "iBSS":
                    case "iBSS2":
                    case "Kernelcache":
                    case "Kernelcache2":
                    case "LiquidDetect":
                    //case "LiquidDetect2":
                    case "LLB":
                    case "LLB2":
                    case "Multitouch":
                    //case "Multitouch2":
                    case "NeedService":
                    //case "NeedService2":
                    case "RecoveryMode":
                    case "RecoveryMode2":
                    case "SEP-Firmware":
                    case "SEP-Firmware2":
                        elem = new PlistDict();
                        elem.Add("File Name", new PlistString(data[key]));
                        if (data[key + "IV"] == "Not Encrypted") {
                            elem.Add("Encryption", new PlistBool(false));
                        } else {
                            elem.Add("Encryption", new PlistBool(true));
                            elem.Add("IV", new PlistString(data[key + "IV"]));
                            elem.Add("Key", new PlistString(data[key + "Key"]));
                            length = data[key + "IV"].Length;
                            Debug.Assert(length == 32 || length == 7, $"{debug}data[\"{key}IV\"].Length ({length}) != 32");
                            length = data[key + "Key"].Length;
                            Debug.Assert(length == 32 || length == 64 || length == 7, $"{debug}data[\"{key}Key\"].Length ({length}) != (32 || 64)");
                        }
                        dict.Add(key, elem);
                        break;

                   default:
                        // Ignore GM keys for now
                        if (key.StartsWith("GM") || key.Contains("Beta"))
                            break;
                        Debug.Assert(key.EndsWith("IV") || key.EndsWith("Key") || key.EndsWith("KBAG"), $"Unknown key: {key}");
                        break;
                }
            }
            return dict;
        }
        private static string HexStringToBase64(string hex)
        {
            if (hex == "Unknown")
                return "";
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return Convert.ToBase64String(bytes, Base64FormattingOptions.InsertLineBreaks);
        }
        private static bool IsImg2Firmware(string build)
        {
            if (build == "1A543a" || build == "1C25" || build == "1C28")
                return true;
            if (build[0] == '3' || build[0] == '4')
                return true;
            if (build == "5A147p" || build == "5A225c" || build == "5A240d")
                return true;

            return false;
        }

        private static void Proc_OutputDataRecieved(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}