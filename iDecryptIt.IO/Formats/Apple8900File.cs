/* =============================================================================
 * File:   Apple8900File.cs
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
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace iDecryptIt.IO.Formats;

[PublicAPI]
public sealed class Apple8900File : IDisposable
{
    private static readonly byte[] KEY_0x837_ARRAY =
    {
        0x18, 0x84, 0x58, 0xA6, 0xD1, 0x50, 0x34, 0xDF,
        0xE3, 0x86, 0xF2, 0x3B, 0x61, 0xD4, 0x37, 0x74,
    };
    public static ReadOnlySpan<byte> KEY_0x837 => KEY_0x837_ARRAY;
    private const string MAGIC_AND_VERSION = "89001.0";

    private readonly BiEndianBinaryReader _reader;

    private int _sigOffset; // offset 10
    private int _sigLength; // `_certOffset` - `_sigLength`
    private int _certOffset; // offset 14
    private int _certLength; // offset 18
    private byte[] _salt; // offset 1C
    private byte[] _payload;
    private byte[] _signature;
    private byte[] _certificate;

    private Apple8900File(BiEndianBinaryReader reader)
    {
        _reader = reader;

        ParseHeader();
        ExtractPayload();
        ExtractSignature();
        ExtractCertificate();
    }

    // TODO: throws ArgumentException (if can't seek), InvalidDataException (if not 8900), EndOfStreamException
    public static Apple8900File Parse(BiEndianBinaryReader input) =>
        new(input);

    [MemberNotNull(nameof(_salt))]
    private void ParseHeader()
    {
        byte[] header = _reader.ReadBytes(0x800);
        using BiEndianBinaryReader reader = new(header);

        // magic
        if (reader.ReadAsciiChars(7) is not MAGIC_AND_VERSION)
            throw new InvalidDataException("Input file is not an \"8900\" file.");

        // format
        byte format = reader.ReadUInt8();
        if (!Enum.GetValues<Apple8900Format>().Cast<int>().Contains(format))
            throw new InvalidDataException($"Unknown 8900 \"format\" {format}. Only values of 1, 2, 3, or 4 are supported.");
        Format = (Apple8900Format)format;

        reader.Skip(4);

        // lengths + offsets
        Length = reader.ReadInt32LE();
        _sigOffset = reader.ReadInt32LE();
        _certOffset = reader.ReadInt32LE();
        _certLength = reader.ReadInt32LE();
        _sigLength = _certOffset - _sigOffset;

        // salt + epoch
        _salt = reader.ReadBytes(32);
        reader.Skip(2);
        SecurityEpoch = reader.ReadUInt16LE();

        // header signature
        using (Aes aes = Aes.Create())
        {
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.IV = new byte[16];
            aes.Key = KEY_0x837_ARRAY;

            byte[] hash;
            using (SHA1 sha1 = SHA1.Create())
                hash = sha1.ComputeHash(header[..0x40])[..0x10]; // only the first 16 bytes of the hash are used

            using (MemoryStream ms = new(hash))
            {
                using (CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Read))
                {
                    byte[] computedHeaderSig = new byte[16];
                    cs.ReadExact(computedHeaderSig);
                    HeaderSignatureCorrect = computedHeaderSig.SequenceEqual(header[0x40..0x50]);
                }
            }
        }

        // sanity check
        SpuriousDataInHeaderPadding = header.Skip(0x50).Any(b => b is not 0);
    }

    [MemberNotNull(nameof(_payload))]
    private void ExtractPayload()
    {
        Debug.Assert(_reader.BaseStream.Position is 0x800);

        byte[] payload = _reader.ReadBytes(Length);

        if (Format is Apple8900Format.BootPayloadUnencrypted or
            Apple8900Format.GenericPayloadUnencrypted)
        {
            _payload = payload;
            return;
        }

        if (Format is Apple8900Format.BootPayloadEncryptedWithGid)
            throw new ArgumentException("Unable to decrypt boot payloads encrypted with the GID key.");

        _payload = new byte[Length];
        try
        {
            using Aes aes = Aes.Create();
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.IV = new byte[16];
            aes.Key = KEY_0x837_ARRAY;

            using CryptoStream cs = new(new MemoryStream(payload), aes.CreateDecryptor(), CryptoStreamMode.Read);
            cs.ReadExact(_payload);
        }
        catch (CryptographicException)
        {
            // ignore padding issues
        }
    }

    [MemberNotNull(nameof(_signature))]
    private void ExtractSignature()
    {
        _reader.Seek(_sigOffset);
        _signature = _reader.ReadBytes(0x80);
    }

    [MemberNotNull(nameof(_certificate))]
    private void ExtractCertificate()
    {
        _reader.Seek(_certOffset);
        _certificate = _reader.ReadBytes(_certLength);
    }

    public Apple8900Format Format { get; private set; }
    public ReadOnlySpan<byte> Salt => _salt;
    public ushort SecurityEpoch { get; private set; }
    public bool HeaderSignatureCorrect { get; private set; }
    public bool SpuriousDataInHeaderPadding { get; private set; }

    public byte[] Read()
    {
        byte[] buf = new byte[_payload.Length];
        Array.Copy(_payload, buf, _payload.Length);
        return buf;
    }

    public int Length { get; private set; }
    public byte this[int index] => _payload[index];

    public void Dispose()
    {
        _reader.Dispose();
    }
}
