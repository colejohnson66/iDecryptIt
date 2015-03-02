/* =============================================================================
 * File:   Program.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2012-2015, Cole Johnson
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
    public class Program
    {
        static WebClient client = new WebClient();
        static List<string> pages = new List<string>();
        static string keyDir = Path.Combine(Directory.GetCurrentDirectory(), "keys");
        static string plutil = "C:\\Program Files (x86)\\Common Files\\Apple\\Apple Application Support\\plutil.exe";
        static bool plutilExists;
        static bool makeBinaryPlists;

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
                plutilExists = File.Exists(plutil);
                if (!plutilExists)
                    Console.WriteLine("WARNING: plutil not found! Binary plists will NOT be generated.");
            }

            IEnumerable<string> pages = GetKeyPages("Firmware");
            foreach (string title in pages) {
                if (!DoesPageExist(title))
                    continue;
                Console.WriteLine(title);
                // TODO: Maybe make this go faster by using client.DownloadStringAsync
                // TODO: Parse the page using XPath (action=render)
                ParseAndSaveKeyPage(client.DownloadString(
                    "http://theiphonewiki.com/w/index.php?title=" + title + "&action=raw"));
            }
        }

        private static IEnumerable<string> GetKeyPages(string page)
        {
            string baseUrl = "https://theiphonewiki.com/w/api.php?format=xml&action=query&prop=links&titles=" + page + "&pllimit=500";
            Regex regex = new Regex(@"(?:\w+ )+\d\w+ \(\D+\d,\d\)", RegexOptions.Compiled);
            XmlDocument doc = new XmlDocument();
            string plcontinue = null;
            while (true) {
                if (plcontinue == null)
                    doc.InnerXml = client.DownloadString(baseUrl);
                else
                    doc.InnerXml = client.DownloadString(baseUrl + "&plcontinue=" + plcontinue);
                Debug.Assert(doc.SelectNodes("//error").Count == 0);

                // get links
                XmlNodeList list = doc.SelectNodes("//pl[@ns='0']");
                foreach (XmlNode node in list) {
                    string title = node.Attributes.GetNamedItem("title").Value;
                    if (title == "Alpine 1A420 (iPhone1,1)")
                        continue;
                    if (regex.IsMatch(title))
                        yield return title;
                }

                // Are there more links to grab?
                list = doc.SelectNodes("//@plcontinue");
                if (list.Count == 0)
                    break;
                if (list.Count != 1)
                    throw new Exception();
                plcontinue = list[0].Value;
            }
        }
        private static bool DoesPageExist(string pageTitle)
        {
            XmlDocument doc = new XmlDocument();
            doc.InnerXml = client.DownloadString(
                "http://theiphonewiki.com/w/api.php?format=xml&action=query&prop=pageprops&titles=" + pageTitle.Replace(' ', '_'));
            Debug.Assert(doc.SelectNodes("//error").Count == 0);

            XmlNodeList list = doc.SelectNodes("//@pageid");
            if (list.Count == 0)
                return false;
            return true;
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
                    // TODO: Regex
                    value = value.Replace("appletv", "AppleTV");
                    value = value.Replace("ipad", "iPad");
                    value = value.Replace("iphone", "iPhone");
                    value = value.Replace("ipod", "iPod");

                    char[] numbers = new char[]
                    {
                        value[value.Length - 2],
                        ',',
                        value[value.Length - 1]
                    };

                    value = value.Substring(0, value.Length - 2) + new String(numbers);
                } else if (key == "DownloadURL") {
                    key = "Download URL";
                } else if (key.StartsWith("SEPFirmware")) {
                    key = key.Replace("SEPFirmware", "SEP-Firmware");
                }

                data.Add(key, value.Trim());
            }
            if (displayVersion != null && data["Device"].StartsWith("AppleTV"))
                data["Version"] = displayVersion; // Will need to be updated to handle betas

            string filename = Path.Combine(keyDir, data["Device"] + "_" + data["Build"] + ".plist");
            Debug.Assert(!File.Exists(filename), filename);
            PlistDocument doc = new PlistDocument(BuildPlist(data));
            doc.Save(filename, PlistDocumentType.Xml);

            if (plutilExists) {
                Process proc = new Process();
                proc.StartInfo.FileName = plutil;
                proc.StartInfo.Arguments = "-convert binary1 \"" + filename + "\"";
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
            PlistDict dict = new PlistDict(new Dictionary<string, IPlistElement>());

            int length;
            string debug = data["Codename"] + " " + data["Build"] + " (" + data["Device"] + "): ";
            PlistDict elem;

            // We could save some space by saving the IVs and keys in Base64 (len*4/3)
            //   instead of a hex string (len*2) (use PlistData)
            foreach (string key in data.Keys) {
                switch (key) {
                    case "Version":
                        // Remove everything past the "[[Golden Master|GM]]" on GM pages
                        // Both options work for public firmwares, but the first may not on betas
                        string temp = data[key].Split('[', 'b')[0];
                        //string[] temp = data[key].Split(new string[] { " and " }, StringSplitOptions.None)[1];
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
                        elem = new PlistDict(new Dictionary<string, IPlistElement>());
                        elem.Add("File Name", new PlistString(data["RootFS"] + ".dmg"));
                        elem.Add("Key", new PlistString(data["RootFSKey"]));
                        if (data.ContainsKey("GMRootFSKey")) {
                            // Only applicable to 4.0GM/4.0 8A293 (excluding iPhone3,1)
                            elem.Add("GM Key", new PlistString(data["GMRootFSKey"]));
                            length = data["GMRootFSKey"].Length;
                            Debug.Assert(length == 72 || length == 4, debug + "data[\"GMRootFSKey\"].Length (" + length + ") != 72");
                        }
                        length = data["RootFSKey"].Length;
                        Debug.Assert(length == 72 || length == 4, debug + "data[\"RootFSKey\"].Length (" + length + ") != 72");
                        dict.Add("Root FS", elem);
                        break;

                    case "UpdateRamdisk":
                    case "RestoreRamdisk":
                        elem = new PlistDict(new Dictionary<string, IPlistElement>());
                        elem.Add("File Name", new PlistString(data[key] + ".dmg"));
                        if (IsImg2Firmware(data["Build"])) {
                            elem.Add("Encryption", new PlistBool(false));
                        } else if (data[key + "IV"] == "Not Encrypted") {
                            elem.Add("Encryption", new PlistBool(false));
                        } else {
                            elem.Add("Encryption", new PlistBool(true));
                            elem.Add("IV", new PlistString(data[key + "IV"]));
                            elem.Add("Key", new PlistString(data[key + "Key"]));
                            length = data[key + "IV"].Length;
                            Debug.Assert(length == 32 || length == 4, debug + "data[\"" + key + "IV\"].Length (" + length + ") != 32");
                            length = data[key + "Key"].Length;
                            Debug.Assert(length == 32 || length == 64 || length == 4, debug + "data[\"" + key + "Key\"].Length (" + length + ") != (32 || 64)");
                        }
                        dict.Add(key.Replace("Ramdisk", " Ramdisk"), elem);
                        break;

                    case "AppleLogo":
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
                        elem = new PlistDict(new Dictionary<string, IPlistElement>());
                        elem.Add("File Name", new PlistString(data[key]));
                        if (data[key + "IV"] == "Not Encrypted") {
                            elem.Add("Encryption", new PlistBool(false));
                        } else {
                            elem.Add("Encryption", new PlistBool(true));
                            elem.Add("IV", new PlistString(data[key + "IV"]));
                            elem.Add("Key", new PlistString(data[key + "Key"]));
                            length = data[key + "IV"].Length;
                            Debug.Assert(length == 32 || length == 4, debug + "data[\"" + key + "IV\"].Length (" + length + ") != 32");
                            length = data[key + "Key"].Length;
                            Debug.Assert(length == 32 || length == 64 || length == 4, debug + "data[\"" + key + "Key\"].Length (" + length + ") != (32 || 64)");
                        }
                        dict.Add(key, elem);
                        break;

                    default:
                        Debug.Assert(key.EndsWith("IV") || key.EndsWith("Key"), key);
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