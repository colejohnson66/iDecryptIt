using JetBrains.Annotations;
using System;
using System.Linq;

namespace KeyGrabber;

[PublicAPI]
public class FirmwareVersionEntry
{
    public string Version { get; set; } = "";
    public string Build { get; set; } = "";
    public DateTime? ReleaseDate { get; set; } = null;
    public string? Url { get; set; } = null;
    public string? Hash { get; set; } = null;
    public long? FileSize { get; set; } = null;
    public FirmwareVersionEntryUrl[]? KeyPages { get; set; } = null;

    public FirmwareVersionEntry CloneOneDevice(string deviceToKeep)
    {
        return new()
        {
            Version = Version,
            Build = Build,
            ReleaseDate = ReleaseDate,
            Url = Url,
            Hash = Hash,
            FileSize = FileSize,
            KeyPages = new[] { KeyPages!.First(entry => entry.Device == deviceToKeep) },
        };
    }
}

[PublicAPI]
public record FirmwareVersionEntryUrl(
    string Device,
    string? Url);
