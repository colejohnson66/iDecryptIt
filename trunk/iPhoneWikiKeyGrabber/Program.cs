using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace Hexware.Programs.iDecryptIt.KeyGrabber
{
	internal class Program
	{
		// TODO: Replace XML with (WIP) OpenCF
		static string[] links = new string[256];
		static string[] blanks = new string[256];
		static short linksPosition = 0;
		static short blanksPosition = 0;
		static string keyPath = Path.Combine(Directory.GetCurrentDirectory(), "keys");
		static XmlWriterSettings settings = new XmlWriterSettings();
		static Dictionary<string, string> devices = new Dictionary<string, string>()
		{
			{ "appletv21", "Apple TV 2G" },
			{ "appletv31", "Apple TV 3G" },
			{ "appletv32", "Apple TV 3G (Rev A)" },
			{ "ipad11", "iPad 1G" },
			{ "ipad21", "iPad 2 (Wi-Fi)" },
			{ "ipad22", "iPad 2 (GSM)" },
			{ "ipad23", "iPad 2 (CDMA)" },
			{ "ipad24", "iPad 2 (Wi-Fi) [Rev A]" },
			{ "ipad25", "iPad mini 1G (Wi-Fi)" },
			{ "ipad26", "iPad mini 1G (GSM)" },
			{ "ipad27", "iPad mini 1G (Global)" },
			{ "ipad31", "iPad 3 (Wi-Fi)" },
			{ "ipad32", "iPad 3 (CDMA)" },
			{ "ipad33", "iPad 3 (Global)" },
			{ "ipad34", "iPad 4 (Wi-Fi)" },
			{ "ipad35", "iPad 4 (GSM)" },
			{ "ipad36", "iPad 4 (Global)" },
			{ "iphone11", "iPhone 2G" },
			{ "iphone12", "iPhone 3G" },
			{ "iphone21", "iPhone 3GS" },
			{ "iphone31", "iPhone 4 (GSM)" },
			{ "iphone32", "iPhone 4 (GSM) [Rev A]" },
			{ "iphone33", "iPhone 4 (CDMA)" },
			{ "iphone41", "iPhone 4S" },
			{ "iphone51", "iPhone 5 (GSM)" },
			{ "iphone52", "iPhone 5 (Global)" },
			{ "iphone53", "iPhone 5c (GSM)" },
			{ "iphone54", "iPhone 5c (Global)" },
			{ "iphone61", "iPhone 5s (GSM)" },
			{ "iphone62", "iPhone 5s (Global)" },
			{ "ipod11", "iPod touch 1G" },
			{ "ipod21", "iPod touch 2G" },
			{ "ipod31", "iPod touch 3G" },
			{ "ipod41", "iPod touch 4G" },
			{ "ipod51", "iPod touch 5G" }
		};

		private static void Main(string[] args)
		{
			if (Directory.Exists(keyPath))
			{
				Directory.Delete(keyPath, true);
			}
			Directory.CreateDirectory(keyPath);

			// Thankfully, MediaWiki outputs valid XHTML
			Console.WriteLine("Grabbing raw rendered HTML");
			WebClient client = new WebClient();
			string download = "<xml>" + client.DownloadString(new Uri("http://theiphonewiki.com/w/index.php?title=Firmware&action=render")) + "</xml>";

			settings.Encoding = Encoding.UTF8;
			settings.Indent = true;
			settings.IndentChars = "\t";
			settings.NewLineChars = "\r\n";

			// Parse XML
			Console.WriteLine("Parsing XML");
			XmlDocument document = new XmlDocument();
			document.InnerXml = download;
			XmlNodeList list = document.ChildNodes.Item(0).ChildNodes;
			int length = list.Count;
			for (int i = 3; i < length; i++)
			{
				if (list.Item(i).Name == "table")
				{
					ParseTableNode(list.Item(i).ChildNodes);
				}
			}

			// Parse individual pages
			Console.WriteLine("Parsing individual pages");
			for (int i = 0; i < linksPosition; i++)
			{
				Console.WriteLine("    {0}", links[i].Substring(28));
				download = client.DownloadString(new Uri(links[i].Replace("/wiki/", "/w/index.php?title=") + "&action=raw"));
				ParseKeyPage(download);
			}
			Console.ReadLine();
		}
		private static void ParseTableNode(XmlNodeList table)
		{
			for (int tr = 1; tr < table.Count; tr++)
			{
				XmlNodeList thisRow = table.Item(tr).ChildNodes;
				for (int td = 1; td < thisRow.Count; td++)
				{
					ParseTableDataNode(thisRow.Item(td).ChildNodes);
				}
			}
		}
		private static void ParseTableDataNode(XmlNodeList nodes)
		{
			string url;
			bool add;
			int length = nodes.Count;
			for (int i = 0; i < length; i++)
			{
				// Assume URL is good
				add = true;
				if (nodes.Item(i).Name == "a")
				{
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

						// Check if baseband test failed. If so, does page exist?
						if (add && url.Contains("redlink=1"))
						{
							add = false;
							blanks[blanksPosition] = url;
							blanksPosition++;
						}

						// It must contain Apple_TV, iPad, iPhone, or iPod_touch
						if (!url.Contains("Apple_TV") && !url.Contains("iPad") &&
							!url.Contains("iPhone") && !url.Contains("iPod_touch"))
						{
							add = false;
						}

						if (add)
						{
							links[linksPosition] = url.Replace("http://the", "http://www.the");
							linksPosition++;
						}
					}
				}
			}
		}
		private static void ParseKeyPage(string contents)
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
                .Replace(" | ", "")
				.Split(new char[] { '\n', '\r' }, 100, StringSplitOptions.RemoveEmptyEntries);

			// Prepare
			string key;
            string value;
            int length = lines.Length;
			Dictionary<string, string> data = new Dictionary<string, string>();
			for (int i = 0; i < length; i++)
			{
				key = lines[i].Split(' ')[0];
				value = lines[i].Split('=')[1];
				if (value[value.Length - 1] == ' ' || value[value.Length - 1] == '\t')
				{
					// Some pages have keys ending in ' ' or '\t'
					// Halt because OCD
					throw new Exception();
				}
                if (key == "version")
                {
                    // Old format pages
                    throw new Exception();
                }
                if (key == "DownloadURL")
                {
                    key = "Download URL";
                }

				// Convert template to a dictionary
				/*if (value.Trim() == "Not Encrypted")
				{
					data.Add(key.Replace("IV", "") + "NotEncrypted", "true");
				}
				else
				{*/
					data.Add(key, value.Trim());
				//}
			}

			// Build XML
			int num = -1; // what outer element we are on
			string device;
			string build;
			string filename;
            Dictionary<string, string>.Enumerator dict = data.GetEnumerator();
            while (dict.MoveNext())
			{
                KeyValuePair<string, string> elem = dict.Current;
                if (elem.Key == "RootFS")
                {
                    plist.AppendChild(xml.CreateElement("key"));
                    num++;
                    plist.ChildNodes.Item(num).InnerText = "Root FS";
                    plist.AppendChild(xml.CreateElement("dict"));
                    num++;
                    
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("key"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(0).InnerText = "File Name";
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("string"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(1).InnerText = elem.Value + ".dmg";

                    dict.MoveNext();
                    elem = dict.Current;
                    Assert(elem.Key == "RootFSKey");
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("key"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(2).InnerText = "Key";
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("string"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(3).InnerText = elem.Value + ".dmg";

                    if (!data.ContainsKey("GMRootFSKey"))
                    {
                        continue;
                    }
                    dict.MoveNext();
                    elem = dict.Current;
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("key"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(4).InnerText = "Key";
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("string"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(5).InnerText = elem.Value + ".dmg";
                }
                else if (elem.Key == "NoUpdateRamdisk")
                {
                    plist.AppendChild(xml.CreateElement("key"));
                    num++;
                    plist.ChildNodes.Item(num).InnerText = "No Update Ramdisk";
                    plist.AppendChild(xml.CreateElement("true"));
                    num++;
                }
                else if (elem.Key == "RamdiskNotEncrypted")
                {
                    plist.AppendChild(xml.CreateElement("key"));
                    plist.ChildNodes.Item(num).InnerText = "Ramdisk Not Encrypted";
                    num++;
                    plist.AppendChild(xml.CreateElement("true"));
                    num++;
                }
                else if (elem.Key == "UpdateRamdisk" || elem.Key == "RestoreRamdisk")
                {
				    string elemKey = elem.Key;
                    plist.AppendChild(xml.CreateElement("key"));
                    num++;
                    plist.ChildNodes.Item(num).InnerText = elemKey.Replace("Ram", " Ram");
                    plist.AppendChild(xml.CreateElement("dict"));
                    num++;

                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("key"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(0).InnerText = "File Name";
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("string"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(1).InnerText = elem.Value + ".dmg";

                    if (data.ContainsKey("RamdiskNotEncrypted"))
                    {
                        continue;
                    }

                    dict.MoveNext();
                    elem = dict.Current;
                    Assert(elem.Key == elemKey + "IV");
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("key"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(2).InnerText = "IV";
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("string"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(3).InnerText = elem.Value;

                    dict.MoveNext();
                    elem = dict.Current;
                    Assert(elem.Key == elemKey + "Key");
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("key"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(4).InnerText = "Key";
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("string"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(5).InnerText = elem.Value;
                }
                else if (elem.Key != "Version" && elem.Key != "DisplayVersion" &&
                    elem.Key != "Build" && elem.Key != "Device" && elem.Key != "Codename" &&
                    elem.Key != "Baseband" && elem.Key != "Download URL")
                {
                    // An IMG3 file
                    plist.AppendChild(xml.CreateElement("key"));
                    num++;
                    plist.ChildNodes.Item(num).InnerText = elem.Key;
                    plist.AppendChild(xml.CreateElement("dict"));
                    num++;

                    dict.MoveNext();
                    elem = dict.Current;
                    if (elem.Value == "Not Encrypted")
                    {
                    }
                    else
                    {
                    }
                }
                else if (elem.Key.Contains("IV"))
                {
                    plist.AppendChild(xml.CreateElement("key"));
                    plist.ChildNodes.Item(num).InnerText = elem.Key.Replace("IV", "");
                    num++;
                    plist.AppendChild(xml.CreateElement("dict"));
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("key"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(0).InnerText = "Encryption";
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("true"));
                    // 
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("key"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(2).InnerText = "IV";
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("string"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(3).InnerText = elem.Value;
                }
                else if (elem.Key.Contains("Key"))
                {
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("key"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(4).InnerText = "Key";
                    plist.ChildNodes.Item(num).AppendChild(xml.CreateElement("string"));
                    plist.ChildNodes.Item(num).ChildNodes.Item(5).InnerText = elem.Value;
                    num++;
                }
                else if (elem.Key == "Version")
                {
                    plist.AppendChild(xml.CreateElement("key"));
                    num++;
                    plist.ChildNodes.Item(num).InnerText = "Version";
                    plist.AppendChild(xml.CreateElement("string"));
                    num++;
                    // Remove duplicate data from Golden Masters ([[Golden Master|GM]])
                    plist.ChildNodes.Item(num).InnerText = elem.Value.Split('[')[0];
                }
                else if (elem.Key == "DisplayVersion")
                {
                }
                else if (elem.Key == "Device")
                {
                    plist.AppendChild(xml.CreateElement("key"));
                    num++;
                    plist.ChildNodes.Item(num).InnerText = "Device";
                    plist.AppendChild(xml.CreateElement("string"));
                    num++;
                    plist.ChildNodes.Item(num).InnerText = devices[elem.Value];
                }
                else // Just something else
                {
                    plist.AppendChild(xml.CreateElement("key"));
                    num++;
                    plist.ChildNodes.Item(num).InnerText = elem.Key;
                    plist.AppendChild(xml.CreateElement("string"));
                    num++;
                    plist.ChildNodes.Item(num).InnerText = elem.Value;
                }
			}

			// Prepare to save data
			build = data["Build"];
			device = data["Device"];

			// Fix capitalization
			device = device
				.Replace("appletv", "AppleTV")
				.Replace("ipad", "iPad")
				.Replace("iphone", "iPhone")
				.Replace("ipod", "iPod");
			// Add missing comma
			char[] deviceNumbers = device.Substring(device.Length - 2).ToCharArray(); // Get two numbers
			Array.Resize<char>(ref deviceNumbers, 3); // Add room for comma
			deviceNumbers[2] = deviceNumbers[1]; // Move second number over
			deviceNumbers[1] = ',';
			device = device.Substring(0, device.Length - 2) + new String(deviceNumbers);

			// Save data
			filename = Path.Combine(keyPath, device + "_" + build + ".plist");
			if (File.Exists(filename))
			{
                // Something is wrong
                throw new Exception();
				//File.Delete(filename);
			}
			XmlWriter writer = XmlWriter.Create(filename, settings);
			xml.Save(writer);
			writer.Close();
		}

        static void Assert(bool cond)
        {
            if (!cond)
            {
                throw new Exception();
            }
        }
	}
}