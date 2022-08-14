/* =============================================================================
 * File:   CompReader.cs
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
public sealed class CompReader : IDisposable
{
    private const string MAGIC = "comp";
    private const string MAGIC_LZSS = "sszl";

    private readonly BiEndianBinaryReader _input;

    private int _decompressedLength;
    private int _compressedLength;
    private byte[] _payload;

    private CompReader(BiEndianBinaryReader input)
    {
        _input = input;

        ParseHeader();
        ExtractPayload();
    }

    public static CompReader Parse(BiEndianBinaryReader input) =>
        new(input);

    private void ParseHeader()
    {
        byte[] header = _input.ReadBytes(0x180);
        using BiEndianBinaryReader reader = new(header);

        // magic
        if (reader.ReadAsciiChars(4) is not MAGIC)
            throw new InvalidDataException("Input file is not a \"Comp\" file.");
        if (reader.ReadAsciiChars(4) is not MAGIC_LZSS)
            throw new InvalidDataException($"Unknown compression format: \"{header[4..8].Reverse().ToStringNoTrailingNulls()}\".");

        // TODO: checksum
        // ReSharper disable once UnusedVariable
        uint expectedChecksum = reader.ReadUInt32LE();

        // lengths (big endian)
        _decompressedLength = reader.ReadInt32BE();
        _compressedLength = reader.ReadInt32BE();

        // sanity check
        SpuriousDataInHeaderPadding = header.Skip(0x14).Any(b => b is not 0);
    }

    [MemberNotNull(nameof(_payload))]
    private void ExtractPayload()
    {
        Debug.Assert(_input.BaseStream.Position is 0x14);

        byte[] compressedPayload = _input.ReadBytes(_compressedLength);
        _payload = Lzss.Decompress(compressedPayload, _decompressedLength);
        if (_payload.Length != _decompressedLength)
            throw new InvalidDataException($"Expected a decompressed length of {_decompressedLength}, but got a length of {_payload.Length}.");
    }
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
