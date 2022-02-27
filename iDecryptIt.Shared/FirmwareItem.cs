using JetBrains.Annotations;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

namespace iDecryptIt.Shared;

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
