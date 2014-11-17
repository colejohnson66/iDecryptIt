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
using System.Threading;
using System.Xml;

namespace Hexware.Programs.iDecryptIt.KeyGrabber
{
    public class Program
    {
        // TODO: Replace with OpenCF#
        static List<string> links = new List<string>();
        static string keyPath = Path.Combine(Directory.GetCurrentDirectory(), "keys");
        static XmlWriterSettings settings = new XmlWriterSettings();

        public static void Main(string[] args)
        {
            if (Directory.Exists(keyPath))
            {
                // I don't know why, but not waiting prevents the
                //   CreateDirectory(...) call below not working
                Directory.Delete(keyPath, true);
                Thread.Sleep(100);
            }
            Directory.CreateDirectory(keyPath);

            Console.WriteLine("Grabbing list of key pages");
            WebClient client = new WebClient();
            string download = "<xml>" + client.DownloadString(new Uri("http://theiphonewiki.com/w/index.php?title=Firmware&action=render")) + "</xml>";

            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.NewLineChars = "\n";
            settings.CloseOutput = true;

            // Thankfully, MediaWiki outputs valid XHTML
            Console.WriteLine("Parsing page");
            XmlDocument document = new XmlDocument();
            document.InnerXml = download;
            XmlNodeList list = document.ChildNodes.Item(0).ChildNodes;
            int length = list.Count;
            for (int i = 1; i < length; i++)
            {
                if (list.Item(i).Name == "table")
                    ParseTableNode(list.Item(i).ChildNodes);
            }

            // Parse individual pages
            // TODO: We probably could make this go faster by
            //   using an async download in ParseTableDataNode
            Console.WriteLine("Parsing individual pages");
            foreach (string link in links)
            {
                Console.WriteLine("    {0}", link.Substring(28));
                download = client.DownloadString(new Uri(link.Replace("/wiki/", "/w/index.php?title=") + "&action=raw"));
                ParseAndSaveKeyPage(download);
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }
        private static void ParseTableNode(XmlNodeList table)
        {
            for (int tr = 1; tr < table.Count; tr++)
            {
                XmlNodeList thisRow = table.Item(tr).ChildNodes;
                for (int td = 0; td < thisRow.Count; td++)
                {
                    ParseTableDataNode(thisRow.Item(td).ChildNodes);
                }
            }
        }
        private static void ParseTableDataNode(XmlNodeList nodes)
        {
            string url;
            int length = nodes.Count;
            for (int i = 0; i < length; i++)
            {
                if (nodes.Item(i).Name == "a")
                {
                    bool add = true; // Assume URL is good
                    url = nodes.Item(i).Attributes.Item(0).Value;

                    // Ignore download URLs
                    if (url.Contains("theiphonewiki.com"))
                    {
                        // Is this a baseband link
                        for (int ii = 0; ii < 10; ii++)
                        {
                            if (url.Contains("theiphonewiki.com/w/index.php?title=" + ii))
                            {
                                add = false;
                                break;
                            }
                        }

                        // Check if baseband test failed. Does page exist?
                        if (add && url.Contains("redlink=1"))
                            add = false;

                        // It must contain AppleTV, iPad, iPhone, or iPod
                        if (!url.Contains("AppleTV") && !url.Contains("iPad") &&
                            !url.Contains("iPhone") && !url.Contains("iPod"))
                        {
                            add = false;
                        }

                        if (add)
                            links.Add(url.Replace("http://the", "http://www.the"));
                    }
                }
            }
        }
        private static void ParseAndSaveKeyPage(string contents)
        {
            // If not correct format, ignore
            if (contents.Length > 2)
            {
                if (contents[0] == '[' && contents[1] == '[')
                {
                    // Alpine 1A420 (iPhone)
                    return;
                }
                if (contents[0] != '{' && contents[1] != '{')
                {
                    // Page isn't in template format
                    throw new Exception();
                }
            }

            // Set up XML
            XmlDocument xml = new XmlDocument();
            xml.AppendChild(xml.CreateDocumentType("plist", "-//Apple Computer//DTD PLIST 1.0//EN", "http://www.apple.com/DTDs/PropertyList-1.0.dtd", null));
            xml.AppendChild(xml.CreateElement("plist"));
            XmlNode plist = xml.ChildNodes.Item(1);
            plist.AppendChild(xml.CreateElement("dict"));
            plist = plist.ChildNodes.Item(0);

            // Split by lines
            string[] lines = contents
                .Replace("{{keys", "")
                .Replace("}}", "")
                .Split(new char[] { '\n', '\r' }, 100, StringSplitOptions.RemoveEmptyEntries);

            // Remove " | " from lines
            int length = lines.Length;
            for (int i = 0; i < length; i++)
                lines[i] = lines[i].Substring(3);

            // Convert page to a dictionary
            string displayVersion = null;
            Dictionary<string, string> data = new Dictionary<string, string>();
            for (int i = 0; i < length; i++)
            {
                string key = lines[i].Split(' ')[0];
                string value = lines[i].Split('=')[1];
                if (key == "DisplayVersion")
                {
                    displayVersion = value.Trim();
                    continue;
                }
                else if (key == "Device")
                {
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
                }
                else if (key == "DownloadURL")
                {
                    key = "Download URL";
                }
                else if (key.StartsWith("SEPFirmware"))
                {
                    key = key.Replace("SEPFirmware", "SEP-Firmware");
                }

                data.Add(key, value.Trim());
            }

            // Handle usage of "DisplayVersion" on gold masters
            // Will need to be updated to handle betas
            if (displayVersion != null && data["Device"].Contains("AppleTV"))
                data["Version"] = displayVersion;


            // TODO: Move this into the `data` builder block above
            // Convert data.Keys to a string array
            string[] keys = new string[data.Count];
            int num = 0;
            foreach (string thiskey in data.Keys)
            {
                keys[num] = thiskey;
                num++;
            }
            
            BuildXml(plist, keys, data, xml);

            string filename = Path.Combine(keyPath, data["Device"] + "_" + data["Build"] + ".plist");
            if (File.Exists(filename))
            {
                // Something is wrong
                throw new Exception();
            }
            XmlWriter writer = XmlWriter.Create(filename, settings);
            xml.Save(writer);
            writer.Close();
            writer.Dispose();
        }
        
        private static void BuildXml(XmlNode plist, string[] keys, Dictionary<string, string> data, XmlDocument xml)
        {
            // What outer-most element we are on; incremented AFTER element is used
            int num = 0; // what outer-most element we are on - in
            
            for (int i = 0; i < data.Count; i++)
            {
                string thiskey = keys[i];
                
                if (thiskey == "Version" || thiskey == "Build" ||
                    thiskey == "Device" || thiskey == "Codename" ||
                    thiskey == "Download URL" || thiskey == "Baseband")
                {
                    plist.AppendChild(xml.CreateElement("key"));
                    plist.ChildNodes.Item(num).InnerText = thiskey;
                    num++;
                    plist.AppendChild(xml.CreateElement("string"));
                    if (thiskey == "Version")
                    {
                        // Remove everything past the "[[Golden Master|GM]]" on GM pages
                        // Both options work for public firmwares, but the first may not on betas
                        plist.ChildNodes.Item(num).InnerText = data[thiskey].Split('[', 'b')[0];
                        //string[] split = data[thiskey].Split(new string[] { " and " }, StringSplitOptions.None)[1];
                    }
                    else
                    {
                        plist.ChildNodes.Item(num).InnerText = data[thiskey];
                    }
                    num++;
                }
                else if (thiskey == "RootFS")
                {
                    plist.AppendChild(xml.CreateElement("key"));
                    plist.ChildNodes.Item(num).InnerText = "Root FS";
                    num++;
                    plist.AppendChild(xml.CreateElement("dict"));
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("key"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(0).InnerText = "File Name";
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("string"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(1).InnerText = data["RootFS"] + ".dmg";
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("key"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(2).InnerText = "Key";
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("string"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(3).InnerText = data["RootFSKey"];
                    if (data.ContainsKey("GMRootFSKey"))
                    {
                        // Only applicable to 4.0GM/4.0 8A293 (excluding iPhone3,1)
                        plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("key"));
                        plist.ChildNodes.Item(num).ChildNodes.Item(4).InnerText = "GM Key";
                        plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("string"));
                        plist.ChildNodes.Item(num).ChildNodes.Item(5).InnerText = data["GMRootFSKey"];
                    }
                    num++;
                }
                else if (thiskey == "NoUpdateRamdisk")
                {
                    throw new Exception();
                }
                else if (thiskey == "UpdateRamdisk" || thiskey == "RestoreRamdisk") {
                    plist.AppendChild(xml.CreateElement("key"));
                    plist.ChildNodes.Item(num).InnerText = thiskey.Replace("Ramdisk", " Ramdisk");
                    num++;
                    plist.AppendChild(xml.CreateElement("dict"));
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("key"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(0).InnerText = "File Name";
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("string"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(1).InnerText = data[thiskey] + ".dmg";
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("key"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(2).InnerText = "Encryption";
                    string build = data["Build"];
                    if (build == "1A543a" || build == "1C25" || build == "1C28" ||
                        build[0] == '3' || build[0] == '4' ||
                        build == "5A147p" || build == "5A225c" || build == "5A240d" ||
                        data[thiskey + "IV"] == "Not Encrypted")
                    {
                        // Keep the "Not Encrypted" check last - an exception is thrown on those builds
                        plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("false"));
                    }
                    else
                    {
                        plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("true"));
                        plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("key"));
                        plist.ChildNodes.Item(num).ChildNodes.Item(4).InnerText = "IV";
                        plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("string"));
                        plist.ChildNodes.Item(num).ChildNodes.Item(5).InnerText = data[thiskey + "IV"];
                        plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("key"));
                        plist.ChildNodes.Item(num).ChildNodes.Item(6).InnerText = "Key";
                        plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("string"));
                        plist.ChildNodes.Item(num).ChildNodes.Item(7).InnerText = data[thiskey + "Key"];
                    }
                    num++;
                }
                else if (thiskey == "AppleLogo" || thiskey == "BatteryCharging0" ||
                    thiskey == "BatteryCharging1" || thiskey == "BatteryFull" ||
                    thiskey == "BatteryLow0" || thiskey == "BatteryLow1" ||
                    thiskey == "DeviceTree" || thiskey == "GlyphCharging" ||
                    thiskey == "GlyphPlugin" || thiskey == "iBEC" ||
                    thiskey == "iBoot" || thiskey == "iBSS" ||
                    thiskey == "Kernelcache" || thiskey == "LLB" ||
                    thiskey == "NeedService" || thiskey == "RecoveryMode" ||
                    thiskey == "SEP-Firmware")
                {
                    plist.AppendChild(xml.CreateElement("key"));
                    plist.ChildNodes.Item(num).InnerText = thiskey;
                    num++;
                    plist.AppendChild(xml.CreateElement("dict"));
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("key"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(0).InnerText = "File Name";
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("string"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(1).InnerText = data[thiskey];
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("key"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(2).InnerText = "Encryption";
                    if (data[thiskey + "IV"] == "Not Encrypted")
                    {
                        plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("false"));
                    }
                    else
                    {
                        plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("true"));
                        plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("key"));
                        plist.ChildNodes.Item(num).ChildNodes.Item(4).InnerText = "IV";
                        plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("string"));
                        plist.ChildNodes.Item(num).ChildNodes.Item(5).InnerText = data[thiskey + "IV"];
                        plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("key"));
                        plist.ChildNodes.Item(num).ChildNodes.Item(6).InnerText = "Key";
                        plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("string"));
                        plist.ChildNodes.Item(num).ChildNodes.Item(7).InnerText = data[thiskey + "Key"];
                    }
                    num++;
                }
                else if (thiskey.EndsWith("IV") || thiskey.EndsWith("Key"))
                {
                }
                else // Just something else
                {
                    plist.AppendChild(xml.CreateElement("key"));
                    plist.ChildNodes.Item(num).InnerText = thiskey;
                    num++;
                    plist.AppendChild(xml.CreateElement("string"));
                    plist.ChildNodes.Item(num).InnerText = data[thiskey];
                    num++;
                }
            }
        }
    }
}