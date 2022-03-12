/* =============================================================================
 * File:   Apple8900Reader.cs
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
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace iDecryptIt.IO;

[PublicAPI]
public class Apple8900Reader : IDisposable
{
    private static readonly byte[] KEY_0x837_ARRAY =
    {
        0x18, 0x84, 0x58, 0xA6, 0xD1, 0x50, 0x34, 0xDF,
        0xE3, 0x86, 0xF2, 0x3B, 0x61, 0xD4, 0x37, 0x74,
    };
    public static ReadOnlyCollection<byte> KEY_0x837 { get; } = Array.AsReadOnly(KEY_0x837_ARRAY);
    private static byte[] MAGIC = { (byte)'8', (byte)'9', (byte)'0', (byte)'0', (byte)'1', (byte)'.', (byte)'0' };

    private readonly Stream _input;

    private uint _sigOffset; // offset 10
    private uint _sigLength; // `_certOffset` - `_sigLength`
    private uint _certOffset; // offset 14
    private uint _certLength; // offset 18
    private byte[] _salt = new byte[32]; // offset 1C
    private byte[] _payload = Array.Empty<byte>();
    private byte[] _signature = new byte[0x80];
    private byte[] _certificate = Array.Empty<byte>();

    private Apple8900Reader(Stream input)
    {
        if (!input.CanSeek)
            throw new ArgumentException("Input must be seekable.", nameof(input));

        _input = input;
        _input.Position = 0;

        ParseHeader();
        ExtractPayload();
        ExtractSignature();
        ExtractCertificate();
    }

    public static Apple8900Reader Parse(Stream input) =>
        new(input);

    private void ParseHeader()
    {
        byte[] header = new byte[0x800];
        if (_input.Read(header) != 0x800)
            throw new EndOfStreamException("Unexpected EOF while reading header.");

        Span<byte> headerSpan = header.AsSpan();

        // magic
        if (!MAGIC.SequenceEqual(header[..4]))
            throw new InvalidDataException("Input file is not an \"8900\" file.");

        // format
        byte format = header[7];
        if (format is not (1 or 2 or 3 or 4))
            throw new InvalidDataException($"Unknown 8900 \"format\" {format}. Only values of 1, 2, 3, or 4 are supported.");
        Format = (Apple8900Format)format;

        // lengths + offsets
        Length = (int)BitConverter.ToUInt32(headerSpan[0xC..0x10]);
        _sigOffset = BitConverter.ToUInt32(headerSpan[0x10..0x14]);
        _certOffset = BitConverter.ToUInt32(headerSpan[0x14..0x18]);
        _certLength = BitConverter.ToUInt32(headerSpan[0x18..0x1C]);
        _sigLength = _certOffset - _sigOffset;

        // salt + epoch
        headerSpan[0x1C..0x3C].CopyTo(_salt);
        SecurityEpoch = BitConverter.ToUInt16(headerSpan[0x3E..0x40]);

        // header signature
        using SHA1 sha1 = SHA1.Create();
        using Aes aes = Aes.Create(); // by default, it's CBC
        aes.BlockSize = 128;
        aes.Mode = CipherMode.CBC;
        aes.IV = new byte[16];
        aes.Key = KEY_0x837_ARRAY;
        using MemoryStream ms = new(sha1.ComputeHash(header[..0x40])[..0x10]); // only the first 16 bytes of the hash are used
        using CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Read);
        byte[] computedHeaderSig = new byte[16];
        cs.Read(computedHeaderSig);
        HeaderSignatureCorrect = computedHeaderSig.SequenceEqual(header[0x40..0x50]);

        // sanity check
        SpuriousDataInHeaderPadding = header.Skip(0x50).Any(b => b is not 0);
    }

    private void ExtractPayload()
    {
        Contract.Assert(_input.Position is 0x800);
        byte[] payload = new byte[Length];
        if (_input.Read(payload) != Length)
            throw new EndOfStreamException("Unexpected EOF while reading payload.");

        // ReSharper disable once ConvertIfStatementToSwitchStatement
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
            cs.Read(_payload);
        }
        catch (CryptographicException)
        {
            // ignore padding issues
        }
    }

    private void ExtractSignature()
    {
        _input.Position = _sigOffset;
        _input.Read(_signature);
    }

    private void ExtractCertificate()
    {
        _input.Position = _certOffset;
        _certificate = new byte[_certLength];
        _input.Read(_certificate);
    }

    public Apple8900Format Format { get; private set; }
    public ReadOnlySpan<byte> Salt => _salt;
    public ushort SecurityEpoch { get; private set; }
    public bool HeaderSignatureCorrect { get; private set; }
    public bool SpuriousDataInHeaderPadding { get; private set; }

    public void Read(out byte[] payload)
    {
        payload = new byte[Length];
        Array.Copy(_payload, payload, Length);
    }

    public int Length { get; private set; }
    public byte this[int index] => _payload[index];

    #region IDisposable

    public void Dispose()
    {
        _input.Dispose();
        GC.SuppressFinalize(this);
    }

    #endregion
}
