/* =============================================================================
 * File:   IBootImageReader.cs
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

using iDecryptIt.IO.Helpers;
using JetBrains.Annotations;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace iDecryptIt.IO.Formats;

[PublicAPI]
public sealed class IBootImageReader : IDisposable
{
    private const string MAGIC = "iBootIm\0";
    private const string MAGIC_LZSS = "sszl";
    private const string MAGIC_ARGB = "bgra";
    private const string MAGIC_GRAY = "yerg";

    private readonly BiEndianBinaryReader _input;

    private byte[] _payload;

    private IBootImageReader(BiEndianBinaryReader input)
    {
        _input = input;

        ParseHeader();
        ExtractPayload();
    }

    public static IBootImageReader Parse(BiEndianBinaryReader input) =>
        new(input);

    private void ParseHeader()
    {
        byte[] header = _input.ReadBytes(0x40);
        using BiEndianBinaryReader reader = new(header);

        // magic
        if (reader.ReadAsciiChars(8) is not MAGIC)
            throw new InvalidDataException("Input file is not an \"iBootImage\" file.");
        if (reader.ReadAsciiChars(4) is not MAGIC_LZSS)
            throw new InvalidDataException($"Unknown compression format: \"{header[8..0xC].Reverse().ToStringNoTrailingNulls()}\".");

        // format
        Format = reader.ReadAsciiChars(4) switch
        {
            MAGIC_ARGB => IBootImageFormat.Color,
            MAGIC_GRAY => IBootImageFormat.Grey,
            _ => throw new InvalidDataException($"Unknown color format: \"{header[0xC..0x10].Reverse().ToStringNoTrailingNulls()}\"."),
        };

        // width + height
        Width = reader.ReadUInt16LE();
        Height = reader.ReadUInt16LE();

        // sanity check
        SpuriousDataInHeaderPadding = header.Skip(0x14).Any(b => b is not 0);
    }

    [MemberNotNull(nameof(_payload))]
    private void ExtractPayload()
    {
        Debug.Assert(_input.BaseStream.Position is 0x40);

        byte[] payload = _input.ReadBytes((int)_input.BaseStream.Length - 0x40);

        int expectedSize = Width * Height * Format.BytesPerPixel();
        _payload = Lzss.Decompress(payload, expectedSize);
        if (_payload.Length != expectedSize)
            throw new InvalidDataException($"Expected a decompressed length of {expectedSize}, but got a length of {_payload.Length}.");
    }

    public IBootImageFormat Format { get; private set; }
    public ushort Width { get; private set; }
    public ushort Height { get; private set; }
    public bool SpuriousDataInHeaderPadding { get; private set; }

    public void Read(out byte[] payload)
    {
        payload = new byte[Length];
        Array.Copy(_payload, payload, Length);
    }

    public int Length => _payload.Length;
    public byte this[int index] => _payload[index];

    public void Dispose()
    {
        _input.Dispose();
    }
}
