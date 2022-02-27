using JetBrains.Annotations;
using System;
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
        foreach (char c in IOHelpers.HEADER_KEY_FILE) // 16 byte header
            writer.Write((byte)c);

        writer.Write(Version);
        writer.Write(Build);
        writer.Write(Device);
        writer.Write(Codename);

        writer.Write(Baseband is not null);
        if (Baseband is not null)
            writer.Write(Baseband is "" ? "?" : Baseband); // coerce empty values to '?'

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

    public static KeyPage Deserialize(BinaryReader reader)
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        KeyPage page = new();
        page.Version = reader.ReadString();
        page.Build = reader.ReadString();
        page.Device = reader.ReadString();
        page.Codename = reader.ReadString();
        page.Baseband = reader.ReadBoolean() ? reader.ReadString() : null;
        page.DownloadUrl = reader.ReadBoolean() ? reader.ReadString() : null;

        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            switch (reader.ReadString())
            {
                case "Models":
                    string model1 = reader.ReadString();
                    string model2 = reader.ReadString();
                    page.Models = (model1, model2);
                    break;
                case "RootFS":
                    page.RootFS = RootFS.Deserialize(reader);
                    break;
                case "RootFSBeta":
                    page.RootFSBeta = RootFS.Deserialize(reader);
                    break;
                default:
                    FirmwareItemType key = Enum.Parse<FirmwareItemType>(reader.ReadString());
                    page.FirmwareItems.Add(key, FirmwareItem.Deserialize(reader));
                    break;
            }
        }

        return page;
    }
}

[PublicAPI]
public class RootFS
{
    public const string KEY_MAGIC = "key!";
    public const string UNKNOWN_MAGIC = "unk?";

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
            return; // if not encrypted, nothing more to do

        if (Key is not null)
        {
            foreach (char c in KEY_MAGIC)
                writer.Write((byte)c);
            writer.Write(Key);
            return;
        }

        foreach (char c in UNKNOWN_MAGIC)
            writer.Write((byte)c);
    }

    public static RootFS Deserialize(BinaryReader reader)
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        RootFS dmg = new(reader.ReadBoolean() ? reader.ReadString() : null);

        dmg.Encrypted = reader.ReadBoolean();
        if (!dmg.Encrypted)
            return dmg; // if not encrypted, nothing more to do

        string magic = Encoding.ASCII.GetString(reader.ReadBytes(4));
        switch (magic)
        {
            case KEY_MAGIC:
                dmg.Key = reader.ReadString();
                return dmg;

            case UNKNOWN_MAGIC:
                return dmg;

            default:
                throw new InvalidDataException($"Unknown magic sequence: '{magic}'.");
        }
    }
}

[DebuggerDisplay("{DebuggerDisplay,nq}")]
[PublicAPI]
public class FirmwareItem
{
    public const string IV_KEY_MAGIC = "iv+k";
    public const string KBAG_MAGIC = "kbag";
    public const string UNKNOWN_MAGIC = "unk?";

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
            foreach (char c in IV_KEY_MAGIC)
                writer.Write((byte)c);
            writer.Write(IVKey.IV);
            writer.Write(IVKey.Key);
        }
        else if (KBag is not null)
        {
            foreach (char c in KBAG_MAGIC)
                writer.Write((byte)c);
            writer.Write(KBag);
        }
        else
        {
            // not even the KBAG is known
            foreach (char c in UNKNOWN_MAGIC)
                writer.Write((byte)c);
        }
    }

    public static FirmwareItem Deserialize(BinaryReader reader)
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        FirmwareItem item = new(reader.ReadBoolean() ? reader.ReadString() : null);

        item.Encrypted = reader.ReadBoolean();
        if (!item.Encrypted)
            return item; // if not encrypted, nothing more to do

        string magic = Encoding.ASCII.GetString(reader.ReadBytes(4));
        switch (magic)
        {
            case IV_KEY_MAGIC:
                string iv = reader.ReadString();
                item.IVKey = new(iv, reader.ReadString());
                return item;

            case KBAG_MAGIC:
                item.KBag = reader.ReadString();
                return item;

            case UNKNOWN_MAGIC:
                return item;

            default:
                throw new InvalidDataException($"Unknown magic sequence: '{magic}'.");
        }
    }

    private string DebuggerDisplay
    {
        get
        {
            string start = $"FirmwareItem {{ {Filename}";
            if (!Encrypted)
                return $"{start} }}";
            if (IVKey is not null)
                return $"{start}, IV={IVKey.IV}, Key={IVKey.Key} }}";
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (KBag is not null)
                return $"{start}, KBAG={KBag} }}";
            return $"{start} }}";
        }
    }

    public override string ToString() =>
        DebuggerDisplay;
}

[PublicAPI]
public record IVKeyPair(
    string IV,
    string Key);
