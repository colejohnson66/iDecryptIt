/* =============================================================================
 * File:   Program.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2012-2014, Cole Johnson
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
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;

namespace Hexware.Programs.iDecryptIt.KeyGrabber
{
    public class Program
    {
        static List<string> urls = new List<string>();
        static string keyDir = Path.Combine(Directory.GetCurrentDirectory(), "keys");
        static XmlWriterSettings xmlWriterSettings;

        public static void Main(string[] args)
        {
            // For some reason, CreateDirectory(...) sometimes fails unless we wait (race condition?)
            if (Directory.Exists(keyDir))
                Directory.Delete(keyDir, true);
            Thread.Sleep(100);
            Directory.CreateDirectory(keyDir);

            Console.WriteLine("Grabbing list of key pages");
            WebClient client = new WebClient();
            string download = "<xml>" +
                client.DownloadString("http://theiphonewiki.com/w/index.php?title=Firmware&action=render") +
                "</xml>";

            // TODO: Replace with OpenCF#
            xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.IndentChars = "\t";
            xmlWriterSettings.NewLineChars = "\n";
            xmlWriterSettings.CloseOutput = true;
            xmlWriterSettings.Encoding = Encoding.UTF8;

            // MediaWiki outputs valid XHTML
            Console.WriteLine("Parsing page");
            XmlDocument document = new XmlDocument();
            document.InnerXml = download;
            XmlNodeList list = document.ChildNodes.Item(0).ChildNodes;
            int length = list.Count;
            for (int i = 1; i < length; i++) {
                if (list.Item(i).Name == "table")
                    ParseTableNode(list.Item(i).ChildNodes);
            }

            // Parse individual pages
            // TODO: We probably could make this go faster by
            //   using an async download in ParseTableDataNode
            Console.WriteLine("Parsing individual pages");
            foreach (string link in urls) {
                Console.WriteLine("    {0}", link.Substring(28));
                download = client.DownloadString(link.Replace("/wiki/", "/w/index.php?title=") + "&action=raw");
                ParseAndSaveKeyPage(download);
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }
        private static void ParseTableNode(XmlNodeList table)
        {
            for (int tr = 1; tr < table.Count; tr++) {
                XmlNodeList thisRow = table.Item(tr).ChildNodes;
                for (int td = 0; td < thisRow.Count; td++)
                    ParseTableDataNode(thisRow.Item(td).ChildNodes);
            }
        }
        private static void ParseTableDataNode(XmlNodeList nodes)
        {
            string url;
            int length = nodes.Count;
            for (int i = 0; i < length; i++) {
                if (nodes.Item(i).Name == "a") {
                    bool add = true; // Assume URL is good
                    url = nodes.Item(i).Attributes.Item(0).Value;

                    // Ignore download URLs
                    if (url.Contains("theiphonewiki.com")) {
                        // Is this a baseband link
                        for (int ii = 0; ii < 10; ii++) {
                            if (url.Contains("theiphonewiki.com/w/index.php?title=" + ii)) {
                                add = false;
                                break;
                            }
                        }

                        // Check if baseband test failed. Does page exist?
                        if (add && url.Contains("redlink=1"))
                            add = false;

                        // It must contain AppleTV, iPad, iPhone, or iPod
                        if (!url.Contains("AppleTV") && !url.Contains("iPad") &&
                            !url.Contains("iPhone") && !url.Contains("iPod")) {
                            add = false;
                        }

                        if (add)
                            urls.Add(url.Replace("http://the", "http://www.the"));
                    }
                }
            }
        }
        private static void ParseAndSaveKeyPage(string contents)
        {
            // Alpine 1A420 (iPhone)
            if (contents.Length > 2 && contents[0] == '[' && contents[1] == '[')
                return;

            // Split by lines
            string[] lines = contents
                .Replace("{{keys", "")
                .Replace("}}", "")
                .Split(new char[] { '\n', '\r' }, 100, StringSplitOptions.RemoveEmptyEntries);

            // Convert page to a dictionary
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

            // Handle usage of "DisplayVersion" on gold masters
            // Will need to be updated to handle betas
            if (displayVersion != null && data["Device"].StartsWith("AppleTV"))
                data["Version"] = displayVersion;

            // Set up XML
            XmlDocument xml = new XmlDocument();
            xml.AppendChild(xml.CreateDocumentType("plist", "-//Apple Computer//DTD PLIST 1.0//EN", "http://www.apple.com/DTDs/PropertyList-1.0.dtd", null));
            XmlElement plist = xml.CreateElement("plist");
            XmlElement dict = xml.CreateElement("dict");
            BuildXml(dict, data, xml);
            plist.AppendChild(dict);
            xml.AppendChild(plist);

            string filename = Path.Combine(keyDir, data["Device"] + "_" + data["Build"] + ".plist");
            if (File.Exists(filename))
                throw new Exception();
            XmlWriter writer = XmlWriter.Create(filename, xmlWriterSettings);
            xml.Save(writer);
            writer.Flush();
            writer.Close();
        }

        private static void BuildXml(XmlNode plist, Dictionary<string, string> data, XmlDocument xml)
        {
            string build = data["Build"];

            // We could save some space by saving the IVs and keys in Base64 (len*4/3)
            //   instead of a hex string (len*2)
            foreach (string key in data.Keys) {
                XmlElement temp;
                if (key == "Version") {
                    temp = xml.CreateElement("key");
                    temp.InnerText = key;
                    plist.AppendChild(temp);
                    temp = xml.CreateElement("string");
                    // Remove everything past the "[[Golden Master|GM]]" on GM pages
                    // Both options work for public firmwares, but the first may not on betas
                    temp.InnerText = data[key].Split('[', 'b')[0];
                    //string[] split = data[key].Split(new string[] { " and " }, StringSplitOptions.None)[1];
                    plist.AppendChild(temp);
                } else if (key == "Build" || key == "Device" || key == "Codename" ||
                           key == "Download URL" || key == "Baseband") {
                    temp = xml.CreateElement("key");
                    temp.InnerText = key;
                    plist.AppendChild(temp);
                    temp = xml.CreateElement("string");
                    temp.InnerText = data[key];
                    plist.AppendChild(temp);
                } else if (key == "RootFS") {
                    temp = xml.CreateElement("key");
                    temp.InnerText = "Root FS";
                    plist.AppendChild(temp);
                    temp = xml.CreateElement("dict");
                    temp.AppendChild(xml.CreateElement("key"));
                    temp.ChildNodes.Item(0).InnerText = "File Name";
                    temp.AppendChild(xml.CreateElement("string"));
                    temp.ChildNodes.Item(1).InnerText = data["RootFS"] + ".dmg";
                    temp.AppendChild(xml.CreateElement("key"));
                    temp.ChildNodes.Item(2).InnerText = "Key";
                    temp.AppendChild(xml.CreateElement("string"));
                    temp.ChildNodes.Item(3).InnerText = data["RootFSKey"];
                    if (data.ContainsKey("GMRootFSKey")) {
                        // Only applicable to 4.0GM/4.0 8A293 (excluding iPhone3,1)
                        temp.AppendChild(xml.CreateElement("key"));
                        temp.ChildNodes.Item(4).InnerText = "GM Key";
                        temp.AppendChild(xml.CreateElement("string"));
                        temp.ChildNodes.Item(5).InnerText = data["GMRootFSKey"];
                    }
                    plist.AppendChild(temp);
                } else if (key == "UpdateRamdisk" || key == "RestoreRamdisk") {
                    temp = xml.CreateElement("key");
                    temp.InnerText = key.Replace("Ramdisk", " Ramdisk");
                    plist.AppendChild(temp);
                    temp = xml.CreateElement("dict");
                    temp.AppendChild(xml.CreateElement("key"));
                    temp.ChildNodes.Item(0).InnerText = "File Name";
                    temp.AppendChild(xml.CreateElement("string"));
                    temp.ChildNodes.Item(1).InnerText = data[key] + ".dmg";
                    temp.AppendChild(xml.CreateElement("key"));
                    temp.ChildNodes.Item(2).InnerText = "Encryption";
                    if (build == "1A543a" || build == "1C25" || build == "1C28" ||
                        build[0] == '3' || build[0] == '4' ||
                        build == "5A147p" || build == "5A225c" || build == "5A240d" ||
                        data[key + "IV"] == "Not Encrypted") {
                        // Keep the "Not Encrypted" check last - an exception would be thrown on those builds
                        temp.AppendChild(xml.CreateElement("false"));
                    } else {
                        temp.AppendChild(xml.CreateElement("true"));
                        temp.AppendChild(xml.CreateElement("key"));
                        temp.ChildNodes.Item(4).InnerText = "IV";
                        temp.AppendChild(xml.CreateElement("string"));
                        temp.ChildNodes.Item(5).InnerText = data[key + "IV"];
                        temp.AppendChild(xml.CreateElement("key"));
                        temp.ChildNodes.Item(6).InnerText = "Key";
                        temp.AppendChild(xml.CreateElement("string"));
                        temp.ChildNodes.Item(7).InnerText = data[key + "Key"];
                    }
                    plist.AppendChild(temp);
                } else if (key == "AppleLogo" || key == "BatteryCharging0" || key == "BatteryCharging1" ||
                           key == "BatteryFull" || key == "BatteryLow0" || key == "BatteryLow1" ||
                           key == "DeviceTree" || key == "GlyphCharging" || key == "GlyphPlugin" ||
                           key == "iBEC" || key == "iBoot" || key == "iBSS" ||
                           key == "Kernelcache" || key == "LLB" || key == "NeedService" ||
                           key == "RecoveryMode" || key == "SEP-Firmware") {
                    temp = xml.CreateElement("key");
                    temp.InnerText = key;
                    plist.AppendChild(temp);
                    temp = xml.CreateElement("dict");
                    temp.AppendChild(xml.CreateElement("key"));
                    temp.ChildNodes.Item(0).InnerText = "File Name";
                    temp.AppendChild(xml.CreateElement("string"));
                    temp.ChildNodes.Item(1).InnerText = data[key];
                    temp.AppendChild(xml.CreateElement("key"));
                    temp.ChildNodes.Item(2).InnerText = "Encryption";
                    if (data[key + "IV"] == "Not Encrypted") {
                        temp.AppendChild(xml.CreateElement("false"));
                    } else {
                        temp.AppendChild(xml.CreateElement("true"));
                        temp.AppendChild(xml.CreateElement("key"));
                        temp.ChildNodes.Item(4).InnerText = "IV";
                        temp.AppendChild(xml.CreateElement("string"));
                        temp.ChildNodes.Item(5).InnerText = data[key + "IV"];
                        temp.AppendChild(xml.CreateElement("key"));
                        temp.ChildNodes.Item(6).InnerText = "Key";
                        temp.AppendChild(xml.CreateElement("string"));
                        temp.ChildNodes.Item(7).InnerText = data[key + "Key"];
                    }
                    plist.AppendChild(temp);
                } else if (key.EndsWith("IV") || key.EndsWith("Key")) {
                } else {
                    throw new Exception();
                }
            }
        }
    }
}