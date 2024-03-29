﻿/* =============================================================================
 * File:   Program.cs
 * Author: Cole Tobin
 * =============================================================================
 * Copyright (c) 2022 Cole Tobin
 *
 * This file is part of iDecryptIt.
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

using iDecryptIt.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace KeyGrabber;

public static class Program
{
    private const string URL_START = "https://theiphonewiki.com/w/index.php?title=";
    private const string URL_END_HTML = "&action=render";
    private const string URL_END_RAW = "&action=raw";
    private const string WIKI_URL_START = "https://www.theiphonewiki.com/wiki/";

    private static readonly object _badPageFileLock = new();
    private const string BAD_PAGE_FILE = "BAD.txt";

    private static readonly HttpClient Client = new() { Timeout = TimeSpan.FromSeconds(5) };
    private static readonly string CurrentDirectory = Directory.GetCurrentDirectory();
    private static readonly string KeysDir = Path.Combine(CurrentDirectory, "Keys");
    private static readonly string iDecryptItOutputDir = Path.Combine(CurrentDirectory, "Output-iDecryptIt");
    private static readonly string iFirmwareOutputDir = Path.Combine(CurrentDirectory, "Output-iFirmware");
    private static readonly Dictionary<Device, List<FirmwareVersionEntry>> Versions = new();

    public static async Task Main()
    {
        Console.WriteLine("Deleting old artifacts...");
        if (Directory.Exists(KeysDir))
            Directory.Delete(KeysDir, true);
        if (Directory.Exists(iDecryptItOutputDir))
            Directory.Delete(iDecryptItOutputDir, true);
        if (Directory.Exists(iFirmwareOutputDir))
            Directory.Delete(iFirmwareOutputDir, true);
        if (File.Exists(BAD_PAGE_FILE))
            File.Delete(BAD_PAGE_FILE);

        Directory.CreateDirectory(KeysDir);
        Directory.CreateDirectory(iDecryptItOutputDir);
        Directory.CreateDirectory(iFirmwareOutputDir);

        foreach (Device id in Device.AllDevices)
            Versions.Add(id, new());


        Console.WriteLine("Parsing descriptor pages...");
        await ProcessDescriptors();
        Console.WriteLine("Descriptor parsing complete.");


        // all <ref> tags (`[..]`) should've been removed
        Console.WriteLine($"Checking for bad {nameof(FirmwareVersionEntry)} objects...");
        foreach ((Device device, List<FirmwareVersionEntry> entries) in Versions)
        {
            Debug.Assert(
                entries.All(entry => !entry.Build.Contains('[') && !entry.Version.Contains('[')),
                $"{device.ModelString} has bad {nameof(FirmwareVersionEntry)} object.");
        }
        Console.WriteLine("No issues found.");


        // use a "using block" using so the file is closed when we're done (not really needed, but why not cleanup?)
        Console.WriteLine("Generating 'HasKeys.bin'...");
        await using (BinaryWriter hasKeysWriter = new(File.OpenWrite(Path.Combine(iDecryptItOutputDir, "HasKeys.bin")), Encoding.UTF8))
        {
            hasKeysWriter.Write(Encoding.ASCII.GetBytes(IOHelpers.HEADER_HAS_KEYS)); // 16 byte header
            foreach ((Device device, List<FirmwareVersionEntry> entries) in Versions)
            {
                hasKeysWriter.Write(device.ModelString);
                hasKeysWriter.Write(entries.Count);
                foreach (FirmwareVersionEntry entry in entries)
                    new HasKeysEntry(entry.Version, entry.Build, entry.KeyPages![0].Url is not null).Serialize(hasKeysWriter);
            }
        }
        Console.WriteLine("'HasKeys.bin' complete.");

        // same here
        Console.WriteLine("Generating 'HasKeys.json'...");
        await using (Utf8JsonWriter hasKeysWriter = new(File.OpenWrite(Path.Combine(iFirmwareOutputDir, "HasKeys.json")), new() { Indented = true }))
        {
            hasKeysWriter.WriteStartObject();
            foreach ((Device device, List<FirmwareVersionEntry> entries) in Versions)
            {
                hasKeysWriter.WriteStartArray(device.ModelString);
                foreach (FirmwareVersionEntry entry in entries)
                {
                    hasKeysWriter.WriteStartArray();
                    new HasKeysEntry(entry.Version, entry.Build, entry.KeyPages![0].Url is not null).Serialize(hasKeysWriter);
                    hasKeysWriter.WriteEndArray();
                }
                hasKeysWriter.WriteEndArray();
            }
            hasKeysWriter.WriteEndObject();
        }
        Console.WriteLine("'HasKeys.json' complete.");


        // Unfortunately, the wiki export caps at 5000 pages, so we have to crawl one by one...
        // TODO: iterate by scanning [[Firmware Keys/##.x]] to avoid duplicates
        Console.WriteLine("Beginning crawl and save...");
        foreach ((_, List<FirmwareVersionEntry> entries) in Versions)
        {
            await Parallel.ForEachAsync(
                entries, async (entry, _) =>
                {
                    if (entry.KeyPages?[0].Url is null)
                        return;

                    KeyPage page = ParseKeyPage(await GetPageAsRawWikiText(entry.KeyPages[0].Url!));
                    string fileName = $"{KeysDir}/{page.Device}_{page.Build}.bin";
                    if (File.Exists(fileName))
                        return; // duplicate

                    try
                    {
                        await using BinaryWriter pageWriter = new(File.OpenWrite(fileName));
                        page.Serialize(pageWriter);
                    }
                    catch
                    {
                        // ignore; race condition may cause another thread to create the destination file between
                        // this one checking if it exists and opening
                    }
                });
        }
        Console.WriteLine("Crawl and save complete.");


        Console.WriteLine("Bundling for iDecryptIt...");
        foreach (Device device in Device.AllDevices)
        {
            Console.WriteLine($"Bundling {device}.");
            string idStr = device.ModelString;
            KeyPageBinaryBundleWriter writer = new();
            foreach (string file in Directory.GetFiles(KeysDir, $"{idStr}_*.bin"))
            {
                string build = new FileInfo(file).Name[(idStr.Length + 1)..^4];
                byte[] contents = await File.ReadAllBytesAsync(file);
                writer.AddFile(build, contents);
            }
            await using BinaryWriter bundleWriter = new(File.OpenWrite(Path.Combine(iDecryptItOutputDir, $"{idStr}.bin")));
            writer.WriteBundle(bundleWriter);
        }
        Console.WriteLine("Bundling for iDecryptIt complete.");


        Console.WriteLine("Bundling for iFirmware...");
        foreach (Device device in Device.AllDevices)
        {
            Console.WriteLine($"Bundling {device}.");
            string idStr = device.ModelString;
            KeyPageJsonBundleWriter writer = new();
            foreach (string file in Directory.GetFiles(KeysDir, $"{idStr}_*.bin"))
            {
                // deserialize the bin files
                using BinaryReader reader = new(File.OpenRead(file));
                string build = new FileInfo(file).Name[(idStr.Length + 1)..^4];
                writer.AddFile(build, KeyPage.Deserialize(reader));
            }
            await using Utf8JsonWriter bundleWriter = new(
                File.OpenWrite(Path.Combine(iFirmwareOutputDir, $"{idStr}.json")),
                new() { Indented = true });
            writer.WriteBundle(bundleWriter);
        }
        Console.WriteLine("Bundling for iFirmware complete.");


        Console.WriteLine("");
        Console.WriteLine("Done!");
    }

    #region Descriptors

    private static async Task ProcessDescriptors()
    {
        foreach (Descriptor.DeviceEntry device in Descriptor.ALL_DESCRIPTORS)
        {
            foreach (Descriptor.MajorVersionEntry version in device.Entries)
                await BuildVersionsDictionary($"{device.UrlPrefix}{version.MajorVersion}", version.DslForTables);
        }
    }

    private static async Task BuildVersionsDictionary(string page, string[] tableProps)
    {
        // TODO: handle builds with no key page link (eg. Watch listings)
        XmlDocument doc = await GetPageAsXml(page);
        XmlNodeList tables = doc.SelectNodes("//table[@class='wikitable']/tbody")!;
        Debug.Assert(tables.Count == tableProps.Length);
        for (int i = 0; i < tables.Count; i++)
        {
            XmlNode table = tables[i]!;
            FixColSpans(table);
            FixRowSpans(table);
            string[] descriptor = tableProps[i].Split(' ');
            Debug.Assert(table.ChildNodes[0]!.ChildNodes.Count == descriptor.Length);


            foreach (XmlNode row in table.ChildNodes)
            {
                // skip headers
                if (row.ChildNodes[0]!.Name is "th")
                    continue;

                FirmwareVersionEntry version = new();
                for (int cellIdx = 0; cellIdx < descriptor.Length; cellIdx++)
                {
                    XmlNode cell = row.ChildNodes[cellIdx]!;
                    string cellText = cell.InnerText.Trim();
                    int braceIndex = cellText.IndexOf('[');

                    string thisDescriptor = descriptor[cellIdx];
                    switch (thisDescriptor)
                    {
                        case "vm": // Marketing version (always comes before "v")
                            // remove <ref> tags
                            if (braceIndex is not -1)
                            {
                                int endBraceIndex = cellText.IndexOf(']');
                                Debug.Assert(endBraceIndex is not -1);
                                cellText = cellText.Remove(braceIndex, endBraceIndex - braceIndex + 1);
                                Debug.Assert(!cellText.Contains('[')); // ensure there aren't others
                            }

                            version.Version = cellText;
                            continue;

                        case "v": // iOS version
                            // remove <ref> tags
                            if (braceIndex is not -1)
                            {
                                int endBraceIndex = cellText.IndexOf(']');
                                Debug.Assert(endBraceIndex is not -1);
                                cellText = cellText.Remove(braceIndex, endBraceIndex - braceIndex + 1);
                                Debug.Assert(!cellText.Contains('[')); // ensure there aren't others
                            }

                            // if marketing version exists, put this cell in parenthesis
                            version.Version = version.Version is ""
                                ? cellText
                                : $"{version.Version} (iOS {cellText})";
                            continue;

                        case "bm": // Marketing build (always comes before "b")
                            version.Build = cellText;
                            continue;

                        case "b": // Build
                            // remove <ref> tags
                            if (braceIndex is not -1)
                            {
                                Debug.Assert(cellText.EndsWith(']'));
                                cellText = cellText[..braceIndex];
                                Debug.Assert(!cellText.Contains('[')); // ensure there aren't others
                            }

                            // if marketing version exists, put this cell in parenthesis
                            version.Build = version.Build is ""
                                ? cellText
                                : $"{version.Build} ({cellText})";
                            continue;

                        // cases "k..." and "bb..." handled below

                        case "r": // Release date; TODO: handle "preinstalled"
                            if (DateTime.TryParse(cellText, out DateTime dt))
                                version.ReleaseDate = dt;
                            continue;

                        case "u": // Download URL
                            if (cellText is "?" or "N/A")
                                continue;
                            version.Url = cell
                                .SelectNodes(".//@href")!
                                .Cast<XmlNode>()
                                .Select(link => link.Value!)
                                .FirstOrDefault(url => url[0] is not '#');
                            continue;

                        case "h": // Hash
                            if (cellText is "?" or "N/A")
                                continue;
                            // SHA-1 hashes are 20 bytes
                            Debug.Assert(cellText.Length is 40);
                            version.Hash = cellText;
                            continue;

                        case "s": // File size
                            if (long.TryParse(cellText.Replace(",", ""), out long l))
                                version.FileSize = l;
                            continue;

                        case "d": // Documentation
                        case "i": // Ignore
                            continue;

                        default:
                            if (thisDescriptor.StartsWith("bb"))
                                continue; // ignore baseband for now
                            Debug.Assert(thisDescriptor[0] is 'k');
                            break;
                    }

                    string[] devices = thisDescriptor[2..^1].Split(';');
                    List<FirmwareVersionEntryUrl> keyPages = new();
                    foreach (XmlNode link in cell.SelectNodes(".//a")!)
                    {
                        // skip cite links
                        string url = link.SelectSingleNode("@href")!.Value!;
                        if (url[0] is '#')
                            continue;

                        bool redlink = url.EndsWith("redlink=1");
                        string device = link.InnerText.Trim();
                        Debug.Assert(devices.Contains(device));
                        if (!redlink)
                            Debug.Assert(url.StartsWith(WIKI_URL_START));
                        keyPages.Add(new(device, redlink ? null : url[WIKI_URL_START.Length..]));
                    }
                    version.KeyPages = keyPages.ToArray();
                }
                foreach (FirmwareVersionEntryUrl url in version.KeyPages!)
                    Versions[Device.Parse(url.Device)].Add(version.CloneOneDevice(url.Device));
            }
        }
    }

    #endregion

    #region Key Page Parsing

    private static KeyPage ParseKeyPage(string contents)
    {
        // convert the contents into a set of key-value pairs

        // remove template header/footer
        string[] lines = contents
            .Replace("{{keys", "")
            .Replace("}}", "")
            .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        Dictionary<string, string> props = new();
        foreach (string line in lines)
        {
            Debug.Assert(line.StartsWith("| "));
            string[] lineProps = line[2..].Split('=', 2, StringSplitOptions.TrimEntries);
            props.Add(lineProps[0], lineProps[1]);
        }

        // all device IDs have a comma
        Debug.Assert(props["Device"].Contains(','));

        return BuildKeyPageFromDictionary(props);
    }

    private static KeyPage BuildKeyPageFromDictionary(Dictionary<string, string> props)
    {
        // pages with `Model` prop must have an accompanying `Model2` one
        if (props.ContainsKey("Model") && !props.ContainsKey("Model2"))
        {
            // if not, just ignore the `Model` prop
            MarkBadPage("No 'Model2'", props);
            props.Remove("Model");
        }

        // remove wikilinks
        string version = props["Version"]
            .Replace("[[RC]]", "RC")
            .Replace("[[Release Candidate|RC]]", "RC")
            .Replace("[[Golden Master|GM]]", "GM");
        if (version.Contains('['))
            MarkBadPage("Possible wikilink in 'Version'", props);

        KeyPage page = new()
        {
            Version = version,
            Build = props["Build"],
            Device = props["Device"],
            Codename = props["Codename"],
            Baseband = props.TryGetValue("Baseband", out string? s) ? s : null,
            DownloadUrl = props.TryGetValue("DownloadURL", out s) ? s : null,
            Models = props.ContainsKey("Model") ? (props["Model"], props["Model2"]) : null,
        };

        if (props.TryGetValue("RootFS", out s))
        {
            // root filesystems don't have the extension due to historical mistake by me
            // append '.dmg', but only if the filename is known (non-empty)
            page.RootFS = new(s is "" ? "" : $"{s}.dmg");
            s = props["RootFSKey"];
            if (s is "Not Encrypted")
                page.RootFS.Encrypted = false;
            else
                page.RootFS.Key = s is "Unknown" ? null : s;
        }

        if (props.TryGetValue("RootFSBeta", out s))
        {
            // ditto
            page.RootFSBeta = new(s is "" ? "" : $"{s}.dmg");
            s = props["RootFSBetaKey"];
            if (s is "Not Encrypted")
                page.RootFSBeta.Encrypted = false;
            else
                page.RootFSBeta.Key = s is "Unknown" ? null : s;
        }

        foreach (FirmwareItemType enumVal in Enum.GetValues<FirmwareItemType>())
        {
            if (enumVal is FirmwareItemType.RootFS or FirmwareItemType.RootFSBeta)
                continue;

            string enumStr = enumVal.ToString();
            if (!props.TryGetValue(enumStr, out string? filename))
                continue;

            // ramdisks ALSO don't have the extension
            // append '.dmg', but only if the filename is known (non-empty)
            if (enumStr.Contains("Ramdisk"))
                filename = filename is "" ? "" : $"{filename}.dmg";

            FirmwareItem item = new(filename);
            // ALL firmware items must have an `...IV` prop (even unencrypted ones)
            if (!props.TryGetValue($"{enumStr}IV", out string? iv))
            {
                // if not, ignore it for now
                MarkBadPage($"Missing '{enumStr}IV'", props);
                continue;
            }

            switch (iv)
            {
                case "Not Encrypted":
                    item.Encrypted = false;
                    break;

                case "Unknown":
                    // for now, don't error if the KBAG isn't known
                    // TODO: should we?
                    if (props.TryGetValue($"{enumStr}KBAG", out s))
                        item.KBag = s;
                    break;

                default:
                    // any encrypted item MUST have an accompanying `...Key` prop
                    if (props.TryGetValue($"{enumStr}Key", out string? key))
                        item.IVKey = new(iv, key);
                    else
                        MarkBadPage($"Missing '{enumStr}Key'", props);
                    break;
            }
            page.FirmwareItems.Add(enumVal, item);
        }

        return page;
    }

    private static void MarkBadPage(string error, Dictionary<string, string> props)
    {
        lock (_badPageFileLock)
        {
            File.AppendAllText(
                BAD_PAGE_FILE,
                $"{error.PadRight(15, ' ')}: \"{props["Codename"]} {props["Build"]} ({props["Device"]})\"\r\n",
                Encoding.UTF8);
        }
    }

    #endregion

    #region URL Downloading

    private static async Task<string> DownloadURL(string url)
    {
        Console.WriteLine($"Downloading: {url}");
        while (true)
        {
            try
            {
                return await Client.GetStringAsync(url);
            }
            catch
            {
                // network dip - hold off for a few seconds
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }
    }

    private static async Task<XmlDocument> GetPageAsXml(string page) =>
        new() { InnerXml = await DownloadURL($"{URL_START}{page}{URL_END_HTML}") };

    private static Task<string> GetPageAsRawWikiText(string page) =>
        DownloadURL($"{URL_START}{page}{URL_END_RAW}");

    #endregion

    #region Table Fixups

    private static void FixRowSpans(XmlNode table)
    {
        // This method is a pretty inefficient way IMHO of fixing the problem...

        XmlNodeList rows = table.ChildNodes;
        int colCount = rows[0]!.ChildNodes.Count;
        int rowCount = rows.Count;

        for (int col = 0; col < colCount; col++)
        {
            for (int row = 0; row < rowCount; row++)
            {
                XmlNode cell = rows[row]!.ChildNodes[col]!;
                Debug.Assert(cell is not null);
            restart:
                foreach (XmlAttribute attr in cell.Attributes!)
                {
                    if (attr.Name != "rowspan")
                        continue;
                    int val = Convert.ToInt32(attr.Value);
                    cell.Attributes.Remove(attr);
                    for (int i = 1; i < val; i++)
                    {
                        // Insert the new cell before the cell currently occupying the space we want
                        XmlNode? rowToAddTo = rows[row + i];
                        Debug.Assert(rowToAddTo is not null);
                        rowToAddTo.InsertBefore(cell.Clone(), rowToAddTo.ChildNodes[col]);
                    }

                    // We aren't allowed to modify the collection while enumerating, so if we change it, we need to
                    //   restart the enumeration
                    goto restart;
                }
            }
        }
    }

    private static void FixColSpans(XmlNode table)
    {
        foreach (XmlNode row in table.ChildNodes)
        {
        restart:
            foreach (XmlNode cell in row.ChildNodes)
            {
                foreach (XmlAttribute attr in cell.Attributes!)
                {
                    if (attr.Name != "colspan")
                        continue;

                    int val = Convert.ToInt32(attr.Value);
                    Debug.Assert(val >= 2);
                    cell.Attributes.Remove(attr);
                    for (int i = 1; i < val; i++)
                        row.InsertAfter(cell.Clone(), cell);

                    // We aren't allowed to modify the collection while enumerating, so if we change it, we need to
                    //   restart the enumeration
                    goto restart;
                }
            }
        }
    }

    #endregion
}
