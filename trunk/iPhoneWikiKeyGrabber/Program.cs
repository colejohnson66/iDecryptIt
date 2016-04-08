/* =============================================================================
 * File:   Program.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2012-2016, Cole Johnson
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
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

namespace Hexware.Programs.iDecryptIt.KeyGrabber
{
    public struct FirmwareVersion
    {
        public string Version;
        public string Build;
        public bool HasKeys;
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
            makeBinaryPlists = true;
#endif

            if (makeBinaryPlists) {
                if (!File.Exists(plutil))
                {
                    makeBinaryPlists = false;
                    Console.WriteLine("WARNING: plutil not found! Binary plists will NOT be generated.");
                }
            }

            // TODO: Parse the page using XPath (action=render) [id tags]
            EnumerateFirmwareList("Firmware/Apple_TV");
            //EnumerateFirmwareList("Firmware/Apple_Watch");
            EnumerateFirmwareList("Firmware/iPad");
            EnumerateFirmwareList("Firmware/iPad_mini");
            EnumerateFirmwareList("Firmware/iPhone");
            EnumerateFirmwareList("Firmware/iPod_touch");

            // Build version listing
            PlistDict plistRoot = new PlistDict();
            foreach (var deviceList in versionsList)
            {
                List<FirmwareVersion> versions = deviceList.Value;
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
            PlistDocument versionDoc = new PlistDocument(plistRoot);
            versionDoc.Save(Path.Combine(curDir, "KeyList.plist"), PlistDocumentType.Xml);
        }
        private static void EnumerateFirmwareList(string page)
        {
            foreach (string title in GetKeyPages(page))
            {
                Console.WriteLine(title);
                ParseAndSaveKeyPage(client.DownloadString(
                    "http://theiphonewiki.com/w/index.php?title=" + title + "&action=raw"));
            }
        }
        private static IEnumerable<string> GetKeyPages(string page)
        {
            string url = "https://www.theiphonewiki.com/w/index.php?title=" + page + "&action=render";

            // MediaWiki outputs valid [X]HTML...sortove
            XmlDocument doc = new XmlDocument();
            doc.InnerXml = "<doc>" + client.DownloadString(url) + "</doc>";
            XmlNodeList tableList = doc.SelectNodes("//table[@class='wikitable']");
            Debug.Assert(tableList.Count != 0, "Can't find device tables.");
            foreach (XmlNode table in tableList)
            {
                // What device is this?
                string device = null;
                foreach (XmlNode link in table.SelectNodes(".//a"))
                {
                    if (link.InnerText.Contains("ipsw"))
                    {
                        device = link.InnerText.Trim().Split('_')[0];
                        break;
                    }
                }
                if (device == null)
                    throw new Exception();

                // If we've already seen this device, append to its list,
                //   else, make a new list
                List<FirmwareVersion> versions;
                if (!versionsList.TryGetValue(device, out versions))
                {
                    versions = new List<FirmwareVersion>();
                    versionsList.Add(device, versions);
                }

                foreach (string fwPage in ParseTable(table, versions))
                    yield return fwPage;
            }
        }
        private static IEnumerable<string> ParseTable(XmlNode table, List<FirmwareVersion> list)
        {
            bool isSpecialATVFormat = false;
            int rowNum = -1;
            foreach (XmlNode row in table.ChildNodes)
            {
                rowNum++;
                
                // skip headers
                if (row.InnerText.Contains("Download URL"))
                    continue;
                if (row.InnerText.Contains("Marketing") && row.InnerText.Contains("Internal"))
                {
                    isSpecialATVFormat = true;
                    continue;
                }

                // Kludge to fix issues of rowspans
                // Should probably be a list we check against instead of a bunch of `row.InnerText.ToLower().Contains(...)'
                FirmwareVersion ver = new FirmwareVersion();
                XmlNode buildCell = null;
                if (row.InnerText.ToLower().Contains("6.1_11d257") ||
                    row.InnerText.ToLower().Contains("6.2_11d257") ||
                    row.InnerText.ToLower().Contains("6.2.1_11d258"))
                {
                    // Find build cell
                    foreach (XmlNode cell in row.ChildNodes)
                    {
                        if (cell.InnerText.Contains("11D25"))
                        {
                            buildCell = cell;
                            break;
                        }
                    }
                    // Get version
                    if (buildCell.InnerText.Contains("11D257a"))
                    {
                        ver.Build = "11D257a";
                        ver.Version = "6.1/7.1.2";
                    }
                    else if (buildCell.InnerText.Contains("11D257b"))
                    {
                        ver.Build = "11D257b";
                        ver.Version = "6.2/7.1.2";
                    }
                    else if (buildCell.InnerText.Contains("11D257c"))
                    {
                        ver.Build = "11D257c";
                        ver.Version = "6.2/7.1.2";
                    }
                    else if (buildCell.InnerText.Contains("11D258"))
                    {
                        ver.Build = "11D258";
                        ver.Version = "6.2.1/7.1.2";
                    }
                    goto kludgeJump;
                }

                // Fix colspans and rowspans
                // Possible fix for kludge: recursive rowspan fixer. Before copying, it scans the
                //   cells (l2r, t2b) between the original and the destination for more rowspans.
                int col = 0;
                foreach (XmlNode cell in row.ChildNodes)
                {
                    // Ignore the documentation column
                    if (cell.InnerText.Contains(".ipd"))
                        continue;

                    foreach (XmlAttribute attr in cell.Attributes)
                    {
                        if (attr.Name == "rowspan")
                        {
                            int val = Convert.ToInt32(attr.Value);
                            cell.Attributes.Remove(attr);

                            XmlNode newRow = row;
                            for (int i = 1; i < val; i++)
                            {
                                // Insert a new cell between [col-1] and [col]
                                // Use `InsertBefore' because `col' could be 0
                                newRow = newRow.NextSibling;
                                newRow.InsertBefore(cell.Clone(), newRow.ChildNodes[col]);
                            }
                        }
                        //else if (attr.Name == "colspan")
                        //{
                        //    int val = Convert.ToInt32(attr.Value);
                        //    cell.Attributes.Remove(attr);
                        //
                        //    for (int i = 1; i < val; i++)
                        //        row.InsertAfter(cell.Clone(), cell);
                        //}
                    }
                    col++;
                }

                // `ver' and `buildCell' are defined up at the kludge
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
                
            kludgeJump:
                XmlNodeList keyPageUrl = buildCell.SelectNodes(".//@href");
                if (keyPageUrl.Count == 0)
                {
                    // This build doesn't have an IPSW. For now, just add the
                    //   version to the list, but don't yield a URL. When adding
                    //   support for betas, this logic will need to be redone.
                    ver.HasKeys = false;
                    list.Add(ver);
                    continue;
                }
                else if (keyPageUrl.Count == 1)
                {
                    string url = keyPageUrl[0].Value;
                    if (url.Contains("redlink"))
                    {
                        ver.HasKeys = false;
                        list.Add(ver);
                        continue;
                    }

                    ver.HasKeys = true;
                    list.Add(ver);
                    url = url.Substring(url.IndexOf("/wiki/") + "/wiki/".Length);
                    yield return url;
                }
                else
                {
                    throw new Exception();
                }
            }
        }
        private static void ParseAndSaveKeyPage(string contents)
        {
            string[] lines = contents
                .Replace("{{keys", "")
                .Replace("}}", "")
                .Split(new char[] { '\n', '\r' }, 100, StringSplitOptions.RemoveEmptyEntries);

            string displayVersion = null;
            Dictionary<string, string> data = new Dictionary<string, string>();
            for (int i = 0; i < lines.Length; i++) {
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

            if (makeBinaryPlists) {
                Process proc = new Process();
                proc.StartInfo.FileName = plutil;
                proc.StartInfo.Arguments = $"-convert binary1 \"{filename}\"";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.OutputDataReceived += proc_OutputDataRecieved;
                proc.ErrorDataReceived += proc_OutputDataRecieved;
                proc.Start();
                proc.WaitForExit();
                // verify file converted correctly and can be loaded
                // (assertion prevents optimization out)
                PlistDocument doc2 = new PlistDocument(filename);
                Debug.Assert(doc2.RootNode != null);
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
                        if (temp.Contains("b") || temp.Contains("["))
                        {
                            // will need to be updated for beta support
                            Match match = new Regex(@"^([\d\.]+)[^(]+\(([\d\.]+)").Match(temp);
                            temp = $"{match.Groups[1].Value} ({match.Groups[2].Value})";
                            Console.WriteLine(" <<>> \"{0}\" --> \"{1}\"", data[key], temp);
                        }
                        dict.Add("Version", new PlistString(temp));
                        break;

                    case "Build":
                    case "Device":
                    case "Codename":
                    case "Download URL":
                    case "Baseband":
                        dict.Add(key, new PlistString(data[key]));
                        break;

                    case "RootFS":
                        elem = new PlistDict();
                        elem.Add("File Name", new PlistString(data["RootFS"] + ".dmg"));
                        elem.Add("Key", new PlistString(data["RootFSKey"]));
                        length = data["RootFSKey"].Length;
                        Debug.Assert(length == 72 || length == 4, $"{debug}data[\"RootFSKey\"].Length ({length}) != 72)");
                        dict.Add("Root FS", elem);
                        break;

                    /*case "GMRootFS":
                        elem = new PlistDict();
                        elem.Add("File Name", new PlistString(data["GMRootFS"] + ".dmg"));
                        elem.Add("Key", new PlistString(data["GMRootFSKey"]));
                        length = data["GMRootFSKey"].Length;
                        Debug.Assert(length == 72 || length == 4, $"{debug}data[\"GMRootFSKey\"].Length ({length}) != 72");
                        dict.Add("GM Root FS", elem);
                        break;*/

                    case "UpdateRamdisk":
                    case "RestoreRamdisk":
                        elem = new PlistDict();
                        elem.Add("File Name", new PlistString(data[key] + ".dmg"));
                        if (data[key + "IV"] == "Not Encrypted") {
                            elem.Add("Encryption", new PlistBool(false));
                        } else {
                            elem.Add("Encryption", new PlistBool(true));
                            elem.Add("IV", new PlistString(data[key + "IV"]));
                            elem.Add("Key", new PlistString(data[key + "Key"]));
                            length = data[key + "IV"].Length;
                            Debug.Assert(length == 32 || length == 4, $"{debug}data[\"{key}IV\"].Length ({length}) != 32");
                            length = data[key + "Key"].Length;
                            Debug.Assert(length == 32 || length == 64 || length == 4, $"{debug}data[\"{key}Key\"].Length ({length}) != (32 || 64)");
                        }
                        dict.Add(key.Replace("Ramdisk", " Ramdisk"), elem);
                        break;

                    case "AppleLogo":
                    case "BatteryCharging":
                    case "BatteryCharging0":
                    case "BatteryCharging1":
                    case "BatteryFull":
                    case "BatteryLow0":
                    case "BatteryLow1":
                    case "DeviceTree":
                    case "GlyphCharging":
                    case "GlyphPlugin":
                    case "iBEC":
                    case "iBoot":
                    case "iBSS":
                    case "Kernelcache":
                    case "LLB":
                    case "NeedService":
                    case "RecoveryMode":
                    case "SEP-Firmware":
                        elem = new PlistDict();
                        elem.Add("File Name", new PlistString(data[key]));
                        if (data[key + "IV"] == "Not Encrypted") {
                            elem.Add("Encryption", new PlistBool(false));
                        } else {
                            elem.Add("Encryption", new PlistBool(true));
                            elem.Add("IV", new PlistString(data[key + "IV"]));
                            elem.Add("Key", new PlistString(data[key + "Key"]));
                            length = data[key + "IV"].Length;
                            Debug.Assert(length == 32 || length == 4, $"{debug}data[\"{key}IV\"].Length ({length}) != 32");
                            length = data[key + "Key"].Length;
                            Debug.Assert(length == 32 || length == 64 || length == 4, $"{debug}data[\"{key}Key\"].Length ({length}) != (32 || 64)");
                        }
                        dict.Add(key, elem);
                        break;

                   default:
                        // Ignore GM keys for now
                        if (key.StartsWith("GM"))
                            break;
                        Debug.Assert(key.EndsWith("IV") || key.EndsWith("Key") || key.EndsWith("KBAG"), $"Unknown key: {key}");
                        break;
                }
            }
            return dict;
        }
        private static string HexStringToBase64(string hex)
        {
            if (hex == "TODO")
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

        private static void proc_OutputDataRecieved(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}