using iDecryptIt.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace KeyGrabber;

public static class Program
{
    private const string URL_START = "https://theiphonewiki.com/w/index.php?title=";
    private const string URL_END_HTML = "&action=render";
    private const string URL_END_RAW = "&action=raw";
    private const string WIKI_URL_START = "https://www.theiphonewiki.com/wiki/";

    private static readonly HttpClient Client = new();
    private static readonly string CurrentDirectory = Directory.GetCurrentDirectory();
    private static readonly string KeysDir = Path.Combine(CurrentDirectory, "Keys");
    private static readonly Dictionary<DeviceID, List<FirmwareVersionEntry>> Versions = new();

    public static async Task Main()
    {
        if (Directory.Exists(KeysDir))
            Directory.Delete(KeysDir, true);
        Directory.CreateDirectory(KeysDir);

        foreach (DeviceID id in Enum.GetValues<DeviceID>())
            Versions.Add(id, new());

        await ProcessDescriptor("Firmware/Apple_TV/", Descriptors.AppleTVFirmwarePages);
        await ProcessDescriptor("Firmware/Apple_Watch/", Descriptors.AppleWatchFirmwarePages);
        await ProcessDescriptor("Firmware/HomePod/", Descriptors.HomePodFirmwarePages);
        // await ProcessDescriptor("Firmware/Mac/", Descriptors.AppleSiliconFirmwarePages);
        // await ProcessDescriptor("Firmware/iBridge/", Descriptors.IBridgeFirmwarePages);
        await ProcessDescriptor("Firmware/iPad/", Descriptors.IPadFirmwarePages);
        await ProcessDescriptor("Firmware/iPad_Air/", Descriptors.IPadAirFirmwarePages);
        await ProcessDescriptor("Firmware/iPad_Pro/", Descriptors.IPadProFirmwarePages);
        await ProcessDescriptor("Firmware/iPad_mini/", Descriptors.IPadMiniFirmwarePages);
        await ProcessDescriptor("Firmware/iPhone/", Descriptors.IPhoneFirmwarePages);
        await ProcessDescriptor("Firmware/iPod_touch/", Descriptors.IPodTouchFirmwarePages);
        await ProcessDescriptor("Beta_Firmware/Apple_TV/", Descriptors.AppleTVBetaFirmwarePages);
        await ProcessDescriptor("Beta_Firmware/Apple_Watch/", Descriptors.AppleWatchBetaFirmwarePages);
        await ProcessDescriptor("Beta_Firmware/HomePod/", Descriptors.HomePodBetaFirmwarePages);
        // await ProcessDescriptor("Beta_Firmware/Mac/", Descriptors.AppleSiliconBetaFirmwarePages);
        // await ProcessDescriptor("Beta_Firmware/iBridge/", Descriptors.IBridgeBetaFirmwarePages);
        await ProcessDescriptor("Beta_Firmware/iPad/", Descriptors.IPadBetaFirmwarePages);
        await ProcessDescriptor("Beta_Firmware/iPad_Air/", Descriptors.IPadAirBetaFirmwarePages);
        await ProcessDescriptor("Beta_Firmware/iPad_Pro/", Descriptors.IPadProBetaFirmwarePages);
        await ProcessDescriptor("Beta_Firmware/iPad_mini/", Descriptors.IPadMiniBetaFirmwarePages);
        await ProcessDescriptor("Beta_Firmware/iPhone/", Descriptors.IPhoneBetaFirmwarePages);
        await ProcessDescriptor("Beta_Firmware/iPod_touch/", Descriptors.IPodTouchBetaFirmwarePages);

        await using BinaryWriter hasKeysWriter = new(File.OpenWrite("HasKeys.bin"), Encoding.UTF8);
        foreach (char c in "iDecryptItHasKey") // 16 byte header
            hasKeysWriter.Write((byte)c);
        foreach ((DeviceID id, List<FirmwareVersionEntry> entries) in Versions)
        {
            hasKeysWriter.Write(id.ToStringID());
            hasKeysWriter.Write(entries.Count);
            foreach (FirmwareVersionEntry entry in entries)
                new HasKeysEntry(entry.Version, entry.Build, entry.KeyPages![0].Url is not null).Serialize(hasKeysWriter);
        }

        // Unfortunately, the wiki export caps at 5000 pages, so we have to crawl one by one...
        foreach ((_, List<FirmwareVersionEntry> entries) in Versions)
        {
            await Parallel.ForEachAsync(
                entries, async (entry, _) =>
                {
                    if (entry.KeyPages?[0].Url is null)
                        return;
                    string text;
                    while (true)
                    {
                        try
                        {
                            text = await GetPageAsRawWikiText(entry.KeyPages[0].Url!);
                            break;
                        }
                        catch (Exception)
                        {
                            Thread.Sleep(15000);
                        }
                    }
                    KeyPage page = ParseKeyPage(text);

                    string fileName = $"{KeysDir}/{page.Device}_{page.Build}.bin";
                    if (new FileInfo(fileName).Exists)
                        fileName = $"{KeysDir}/___{page.Device}_{page.Build}_{DateTime.Now.ToString("O").Replace(':', '_')}.bin";
                    await using BinaryWriter pageWriter = new(File.OpenWrite(fileName));
                    foreach (char c in "iDecryptItKeyFil") // 16 byte header
                        pageWriter.Write((byte)c);
                    page.Serialize(pageWriter);
                });
        }

        Debug.WriteLine("");
        Debug.WriteLine("DONE!");
    }

#region Descriptors
    private static async Task ProcessDescriptor(string basePageName, Dictionary<string, string[]> descriptor)
    {
        foreach ((string? majorVersion, string[]? tables) in descriptor)
            await BuildVersionsDictionary($"{basePageName}{majorVersion}", tables);
    }

    private static async Task BuildVersionsDictionary(string page, string[] tableProps)
    {
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
                    string thisDescriptor = descriptor[cellIdx];
                    switch (thisDescriptor)
                    {
                        case "vm": // Marketing version (always comes before "v")
                            version.Version = cellText;
                            continue;
                        case "v": // iOS version
                            version.Version = version.Version is ""
                                ? cell.InnerText.Trim()
                                : $"{version.Version} (iOS {cellText})";
                            continue;
                        case "bm": // Marketing build (always comes before "b")
                            version.Build = cellText;
                            continue;
                        case "b": // Build
                            version.Build = version.Build is ""
                                ? cell.InnerText.Trim()
                                : $"{version.Build} ({cellText})";
                            continue;

                        // case "k..." handled below
                        // case "bb", "bb..." ignored for now
                        case "r": // Release date
                            if (DateTime.TryParse(cellText, out DateTime dt))
                                version.ReleaseDate = dt;
                            continue;
                        case "u": // Download URL
                            // ReSharper disable once LoopCanBeConvertedToQuery
                            foreach (XmlNode link in cell.SelectNodes(".//@href")!)
                            {
                                // skip cite links
                                if (link.Value![0] is '#')
                                    continue;
                                version.Url = link.Value!;
                                break;
                            }
                            continue;
                        case "h": // Hash
                            if (cellText is "?" or "N/A")
                                continue;
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
                    }
                    if (thisDescriptor[..2] is "bb")
                        continue; // skip baseband
                    Debug.Assert(thisDescriptor[0] is 'k');
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
                    Versions[DeviceIDHelpers.FromStringID(url.Device)].Add(version.CloneOneDevice(url.Device));
            }
        }
    }
#endregion

    private static KeyPage ParseKeyPage(string contents)
    {
        // convert the contents into a set of key-value pairs

        string[] lines = contents
            .Replace("{{keys", "")
            .Replace("}}", "")
            .Split(new[] { '\n', '\r' }, 200, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        Dictionary<string, string> props = new();
        foreach (string line in lines)
        {
            Debug.Assert(line.StartsWith("| "));
            string[] lineProps = line[2..].Split('=', 2, StringSplitOptions.TrimEntries);
            props.Add(lineProps[0], lineProps[1]);
        }

        Debug.Assert(props["Device"].Contains(','));
        return BuildKeyPageFromDictionary(props);
    }

    private static KeyPage BuildKeyPageFromDictionary(Dictionary<string, string> props)
    {
        if (props.ContainsKey("Model") && !props.ContainsKey("Model2"))
            props.Remove("Model"); // TODO: fix pages with this issue

        KeyPage page = new()
        {
            Version = props["Version"],
            Build = props["Build"],
            Device = props["Device"],
            Codename = props["Codename"],
            Baseband = props.TryGetValue("Baseband", out string? s) ? s : null,
            DownloadUrl = props.TryGetValue("DownloadURL", out s) ? s : null,
            Models = props.ContainsKey("Model") ? (props["Model"], props["Model2"]) : null,
        };

        if (props.TryGetValue("RootFS", out s))
        {
            page.RootFS = new($"{s}.dmg");
            s = props["RootFSKey"];
            if (s is "Not Encrypted")
                page.RootFS.Encrypted = false;
            else
                page.RootFS.Key = s is "Unknown" ? null : s;
        }

        if (props.TryGetValue("RootFSBeta", out s))
        {
            page.RootFSBeta = new($"{s}.dmg");
            s = props["RootFSBetaKey"];
            if (s is "Not Encrypted")
                page.RootFSBeta.Encrypted = false;
            else
                page.RootFSBeta.Key = s is "Unknown" ? null : s;
        }

        foreach (FirmwareItemType enumVal in Enum.GetValues<FirmwareItemType>())
        {
            string enumStr = enumVal.ToString();
            if (!props.TryGetValue(enumStr, out string? filename))
                continue;

            if (enumStr.Contains("Ramdisk"))
                filename += ".dmg";

            FirmwareItem item = new(filename);
            try
            {
                string iv = props[$"{enumStr}IV"];

                switch (iv)
                {
                    case "Not Encrypted":
                        item.Encrypted = false;
                        break;

                    // ReSharper disable once StringLiteralTypo
                    case "Unknown":
                        if (props.TryGetValue($"{enumStr}KBAG", out s))
                            item.KBag = s;
                        break;

                    default:
                        item.IVKey = new(iv, props[$"{enumStr}Key"]);
                        break;
                }
                page.FirmwareItems.Add(enumVal, item);
            }
            catch
            {
                // page with bad syntax
                // could probably recover, but just fix the source and rerun this program
                ;
            }
        }

        return page;
    }

    private static async Task<XmlDocument> GetPageAsXml(string page)
    {
        string url = $"{URL_START}{page}{URL_END_HTML}";
        Debug.WriteLine($"Downloading: {url}");
        string contents = await Client.GetStringAsync(url);
        return new()
        {
            InnerXml = contents,
        };
    }

    private static async Task<string> GetPageAsRawWikiText(string page)
    {
        string url = $"{URL_START}{page}{URL_END_RAW}";
        Debug.WriteLine($"Downloading: {url}");
        return await Client.GetStringAsync(url);
    }

    private static void FixRowSpans(XmlNode table)
    {
        // This method is a pretty inefficient way IMHO of fixing the problem...

        XmlNodeList rows = table.ChildNodes;
        int rowCount = rows.Count;

        // Subtract 1 to ignore the release notes column (it causes
        //   problems when using XmlNode.InsertBefore(...) and we
        //   don't care about it)
        int colCount = rows[0]!.ChildNodes.Count;

        for (int col = 0; col < colCount; col++)
        {
            for (int row = 0; row < rowCount; row++)
            {
                XmlNode cell = rows[row]!.ChildNodes[col]!;
                Debug.Assert(cell != null);
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

                    // We aren't allowed to modify the collection while enumerating,
                    //   so if we change it, we need to restart the enumeration
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

                    // We aren't allowed to modify the collection while enumerating,
                    //   so if we change it, we need to restart the enumeration
                    goto restart;
                }
            }
        }
    }
}
