/* =============================================================================
 * File:   FirmwareItem.cs
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

using JetBrains.Annotations;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Text.Json;

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
            return; // if not encrypted, nothing more to do

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

    public void Serialize(Utf8JsonWriter writer)
    {
        // if not encrypted, they must both be null
        Contract.Assert(Encrypted || (IVKey is null && KBag is null));

        if (Filename is not null)
            writer.WriteString(nameof(Filename), Filename);

        writer.WriteBoolean(nameof(Encrypted), Encrypted);
        if (!Encrypted)
            return; // if not encrypted, nothing more to do

        if (IVKey is not null)
        {
            writer.WriteStartObject(nameof(IVKey));
            writer.WriteString(nameof(IVKeyPair.IV), IVKey.IV);
            writer.WriteString(nameof(IVKeyPair.Key), IVKey.Key);
            writer.WriteEndObject();
        }
        else if (KBag is not null)
        {
            writer.WriteString(nameof(KBag), KBag);
        }

        // write nothing for unknown
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
                return $"{start}, Unencrypted }}";
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
