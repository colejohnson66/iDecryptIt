using System.Collections.Generic;

namespace KeyGrabber;

public class KeyPage
{
    public string Version { get; set; } = "";
    public string Build { get; set; } = "";
    public string Device { get; set; } = "";
    public string Codename { get; set; } = "";
    public string? Baseband { get; set; }
    public string? DownloadUrl { get; set; }

    public RootFS? RootFS { get; set; }
    public RootFS? RootFSBeta { get; set; }

    // `Model` and `Model2` template keys
    public (string, string)? Models { get; set; }
    public Dictionary<FirmwareItemType, FirmwareItem> FirmwareItems { get; } = new();
}

public class RootFS
{
    /// <summary>The filename; <c>null</c> for unknown.</summary>
    public string? Filename { get; }

    /// <summary>If the root FS is encrypted.</summary>
    public bool Encrypted { get; set; }

    /// <summary>The key, if it exists; <c>null</c> if unknown.</summary>
    public string? Key { get; set; }

    public RootFS(string? filename)
    {
        Filename = filename;
        Encrypted = true;
        Key = null;
    }
}

public class FirmwareItem
{
    /// <summary>The filename; <c>null</c> for unknown.</summary>
    public string? Filename { get; }

    /// <summary>If the item is encrypted.</summary>
    public bool Encrypted { get; set; }

    /// <summary>The IV/key pair; <c>null</c> if unknown or not encrypted.</summary>
    public IVKeyPair? IVKey { get; set; }

    /// <summary>The KBAG; <c>null</c> if unknown, IV/key pair found, or not encrypted.</summary>
    public string? KBag { get; set; }

    public FirmwareItem(string? filename)
    {
        Filename = filename;
        Encrypted = true;
        IVKey = null;
        KBag = null;
    }
}

public record IVKeyPair(
    string IV,
    string Key);
