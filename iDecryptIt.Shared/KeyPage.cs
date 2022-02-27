using JetBrains.Annotations;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

namespace iDecryptIt.Shared;

[PublicAPI]
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

    // `Model` and `Model2` from the {{keys}} template
    public (string, string)? Models { get; set; }
    public Dictionary<FirmwareItemType, FirmwareItem> FirmwareItems { get; } = new();

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(Version);
        writer.Write(Build);
        writer.Write(Device);
        writer.Write(Codename);

        writer.Write(Baseband is not null);
        if (Baseband is not null)
            writer.Write(Baseband is "" ? "?" : Baseband);

        writer.Write(DownloadUrl is not null);
        if (DownloadUrl is not null)
            writer.Write(DownloadUrl);

        if (Models is not null)
        {
            writer.Write("Models");
            writer.Write(Models.Value.Item1);
            writer.Write(Models.Value.Item2);
        }

        if (RootFS is not null)
        {
            writer.Write("RootFS");
            RootFS.Serialize(writer);
        }

        if (RootFSBeta is not null)
        {
            writer.Write("RootFSBeta");
            RootFSBeta.Serialize(writer);
        }

        foreach ((FirmwareItemType key, FirmwareItem value) in FirmwareItems)
        {
            writer.Write(key.ToString());
            value.Serialize(writer);
        }
    }
}

[PublicAPI]
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

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(Filename is not null);
        if (Filename is not null)
            writer.Write(Filename);

        writer.Write(Encrypted);
        if (!Encrypted)
            return;

        if (Key is not null)
        {
            foreach (char c in "key!")
                writer.Write((byte)c);
            writer.Write(Key);
        }
        else
        {
            foreach (char c in "unk?")
                writer.Write((byte)c);
        }
    }
}

[DebuggerDisplay("{DebuggerDisplay,nq}")]
[PublicAPI]
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

    public void Serialize(BinaryWriter writer)
    {
        // if not encrypted, they must both be null
        Contract.Assert(Encrypted || (IVKey is null && KBag is null));

        writer.Write(Filename is not null);
        if (Filename is not null)
            writer.Write(Filename);

        writer.Write(Encrypted);
        if (!Encrypted)
            return;

        if (IVKey is not null)
        {
            foreach (char c in "iv+k")
                writer.Write((byte)c);
            writer.Write(IVKey.IV);
            writer.Write(IVKey.Key);
        }
        else if (KBag is not null)
        {
            foreach (char c in "kbag")
                writer.Write((byte)c);
            writer.Write(KBag);
        }
        else
        {
            // not even the KBAG is known
            foreach (char c in "unk?")
                writer.Write((byte)c);
        }

        // `KBag` mustn't be null here
    }

    private string DebuggerDisplay
    {
        get
        {
            StringBuilder str = new($"FirmwareItem {{ {Filename}");
            if (!Encrypted)
                str.Append(" }");
            else if (IVKey is not null)
                str.Append($", IV={IVKey.IV}, Key={IVKey.Key} }}");
            else if (KBag is not null)
                str.Append($", KBAG={KBag} }}");
            else
                Debug.Assert(false);
            return str.ToString();
        }
    }

    public override string ToString() =>
        DebuggerDisplay;
}

[PublicAPI]
public record IVKeyPair(
    string IV,
    string Key);
