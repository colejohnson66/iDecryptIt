using System.Diagnostics;
using System.Xml;

namespace KeyGrabber;

public static class Program
{
    private const string URL_START = "https://theiphonewiki.com/w/index.php?title=";
    private const string URL_END_HTML = "&action=render";
    private const string URL_END_RAW = "&action=raw";
    private const string WIKI_URL_START = "https://www.theiphonewiki.com/wiki/";

    private static readonly HttpClient Client = new();
    private static readonly List<string> Pages = new();
    private static readonly string CurrentDirectory = Directory.GetCurrentDirectory();
    private static readonly string KeysDir = Path.Combine(CurrentDirectory, "Keys");
    private static readonly Dictionary<string, List<FirmwareVersion>> Versions = new();

    public static Task Main(string[] args)
    {
        if (Directory.Exists(KeysDir))
            Directory.Delete(KeysDir, true);
        Directory.CreateDirectory(KeysDir);

        foreach (string id in Descriptors.DeviceIDs)
            Versions.Add(id, new());
        return Task.CompletedTask;
    }

    private static async Task GetKeyPagesFromMajorVersionPage(string page, params string[] tableProps)
    {
        XmlDocument doc = await GetPageAsXml(page);
        XmlNodeList tables = doc.SelectNodes("//table[@class='wikitable']/tbody")!;
        Debug.Assert(tables.Count is not 0);
        foreach (XmlNode table in tables)
        { }

        throw new NotImplementedException();
    }

    private static IEnumerable<string> ParseMajorVersionTable(XmlNode table)
    {
        FixColSpans(table);
        FixRowSpans(table);

        // TODO: won't work for watchOS
        bool specialAtvFormat = table.ChildNodes[1]!.InnerText.Contains("Marketing");

        int buildCellIdx = -1;
        int keysCellIdx = -1;
        int releaseDateCellIdx = -1;
        int urlCellIdx = -1;
        int hashCellIdx = -1;
        int fileSizeCellIdx = -1;
        XmlNode row0 = table.ChildNodes[0]!;
        for (int colIdx = 0; colIdx < row0.ChildNodes.Count; colIdx++)
        {
            XmlNode cell = row0.ChildNodes[colIdx]!;
            string cellName = cell.Value!.Trim().ToLower();
            if (cellName is "build")
                buildCellIdx = colIdx;
            if (cellName is "keys")
                keysCellIdx = colIdx;
            if (cellName.Contains("date"))
                releaseDateCellIdx = colIdx;
            if (cellName.Contains("ipsw"))
                urlCellIdx = colIdx;
            if (cellName.Contains("sha1"))
                hashCellIdx = colIdx;
            if (cellName.Contains("size"))
                fileSizeCellIdx = colIdx;
        }

        foreach (XmlNode row in table.ChildNodes)
        {
            if (row.InnerText.Contains("Version") || row.InnerText.Contains("Marketing"))
                continue;

            FirmwareVersion version = new();

            version.Version = row.ChildNodes[0]!.InnerText.Trim();
            if (specialAtvFormat)
            {
                string @internal = row.ChildNodes[1]!.InnerText.Trim();
                if (version.Version == @internal)
                {
                    // should only be true on ATV-4.3 (8F455.2557)
                    Debug.Assert(row.ChildNodes[2]!.InnerText.Trim() is "2557");
                    version.Version = "4.3";
                }
                else
                {
                    version.Version = $"{version.Version}/{@internal}";
                }
            }

            version.Build = row.ChildNodes[buildCellIdx]!.InnerText.Trim();

            XmlNodeList keyPageCell = row.ChildNodes[keysCellIdx]!.SelectNodes(".//@href")!;
            version.HasKeys = keyPageCell.Count is not 0;
            string device = "";
            string url = "";
            if (version.HasKeys)
            {
                Debug.Assert(keyPageCell.Count is 1);
                url = keyPageCell[0]!.Value!;
                int start = url.IndexOf('(');
                int end = url.IndexOf(')');
                device = url[(start + 1)..(end - start - 1)];
                if (url.Contains("redlink"))
                    version.HasKeys = false;
            }

            version.ReleaseDate = DateTime.Parse(row.ChildNodes[releaseDateCellIdx]!.InnerText.Trim());
            version.Url = row.ChildNodes[urlCellIdx]!.InnerText.Trim();
            version.Hash = row.ChildNodes[hashCellIdx]!.InnerText.Trim();
            version.FileSize = row.ChildNodes[fileSizeCellIdx]!.InnerText.Trim();

            Versions[device].Add(version);

            yield return url;
        }
    }

    private static KeyPage ParseKeyPage(string contents)
    {
        // convert the contents into a set of key-value pairs

        string[] lines = contents
            .Replace("{{keys", "")
            .Replace("}}", "")
            .Split(new[] { '\n', '\r' }, 200, StringSplitOptions.RemoveEmptyEntries);

        Dictionary<string, string> props = new();
        foreach (string line in lines)
        {
            Debug.Assert(line.StartsWith(" | "));
            string[] lineProps = line.Split('=', 2, StringSplitOptions.TrimEntries);
            props.Add(lineProps[0], lineProps[1]);
        }

        Debug.Assert(props["Device"].Contains(','));
        return BuildKeyPageFromDictionary(props);
    }

    private static KeyPage BuildKeyPageFromDictionary(Dictionary<string, string> props)
    {
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
            page.RootFS = new(s);
            s = props["RootFSKey"];
            if (s is "Not Encrypted")
                page.RootFS.Encrypted = false;
            else if (s is "Unknown")
                page.RootFS.Key = null;
            else
                page.RootFS.Key = s;
        }

        if (props.TryGetValue("RootFSBeta", out s))
        {
            page.RootFSBeta = new(s);
            s = props["RootFSBetaKey"];
            if (s is "Not Encrypted")
                page.RootFSBeta.Encrypted = false;
            else if (s is "Unknown")
                page.RootFSBeta.Key = null;
            else
                page.RootFSBeta.Key = s;
        }

        foreach (FirmwareItemType enumVal in Enum.GetValues<FirmwareItemType>())
        {
            string enumStr = enumVal.ToString();
            if (!props.TryGetValue(enumStr, out string? filename))
                continue;

            FirmwareItem item = new(filename);
            string iv = props[$"{enumStr}IV"];
            if (iv is "Not Encrypted")
                item.Encrypted = false;
            else if (iv is "Unknown")
                item.KBag = props[$"{enumStr}KBAG"];
            else
                item.IVKey = new(iv, props[$"{enumStr}Key"]);

            page.FirmwareItems.Add(enumVal, item);
        }

        return page;
    }

    private static async Task<XmlDocument> GetPageAsXml(string page)
    {
        string contents = await Client.GetStringAsync($"{URL_START}{page}{URL_END_HTML}");
        return new()
        {
            InnerXml = contents,
        };
    }

    private static async Task<string> GetPageAsRawWikiText(string page) =>
        await Client.GetStringAsync($"{URL_START}{page}{URL_END_RAW}");

    private static void FixRowSpans(XmlNode table)
    {
        // This method is a pretty inefficient way IMHO of fixing the problem...

        XmlNodeList rows = table.ChildNodes[0]!.ChildNodes;
        int rowCount = rows.Count;

        // Subtract 1 to ignore the release notes column (it causes
        //   problems when using XmlNode.InsertBefore(...) and we
        //   don't care about it)
        int colCount = rows[0]!.ChildNodes.Count - 1;
        int startRow = rows[1]!.InnerText.Contains("Marketing") ? 2 : 1;

        for (int col = 0; col < colCount; col++)
        {
            for (int row = startRow; row < rowCount; row++)
            {
                XmlNode cell = rows[row]!.ChildNodes[col]!;
                Debug.Assert(cell != null);
            restart:
                foreach (XmlAttribute attr in cell.Attributes!)
                {
                    if (attr.Name != "rowspan")
                        continue;
                    int val = Convert.ToInt32(attr.Value);
                    Debug.Assert(val >= 2); // check for `rowspan="1"`
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
        foreach (XmlNode row in table.ChildNodes[0]!.ChildNodes)
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
