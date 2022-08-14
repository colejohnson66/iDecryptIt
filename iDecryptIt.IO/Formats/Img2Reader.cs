/* =============================================================================
 * File:   Img2Reader.cs
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

namespace iDecryptIt.IO.Formats;

[PublicAPI]
public sealed class Img2Reader : IDisposable
{
    private const string MAGIC = "2gmI";
    private const string MAGIC_VERSION_TAG = "srev";

    private readonly BiEndianBinaryReader _input;

    private int _paddedLength = 0; // offset 10
    private byte[] _payload;

    private Img2Reader(BiEndianBinaryReader input)
    {
        _input = input;

        ParseHeader();
        ExtractPayload();
    }

    public static Img2Reader Parse(BiEndianBinaryReader input) =>
        new(input);

    [MemberNotNull(nameof(ImageType))]
    [MemberNotNull(nameof(VersionTagValue))]
    private void ParseHeader()
    {
        byte[] header = _input.ReadBytes(0x400);
        using BiEndianBinaryReader reader = new(header);

        // magic
        if (reader.ReadAsciiChars(4) is not MAGIC)
            throw new InvalidDataException("Input file is not an \"IMG2\" file.");

        // image type + epoch
        byte[] imageType = reader.ReadBytes(4);
        imageType.AsSpan().Reverse();
        ImageType = imageType.ToStringNoTrailingNulls();
        reader.Skip(2);
        SecurityEpoch = reader.ReadUInt16LE();

        // flags
        reader.Skip(4);

        // length
        _paddedLength = reader.ReadInt32LE();
        Length = reader.ReadInt32LE();
        if (_paddedLength < Length)
            throw new InvalidDataException("Payload's padded length cannot less than unpadded length.");

        reader.Seek(0x70);

        // version tag
        // ReSharper disable once StringLiteralTypo
        if (reader.ReadAsciiChars(4) is not MAGIC_VERSION_TAG)
            throw new InvalidDataException("Input file's version ('vers') tag is missing.");
        reader.Skip(4);
        VersionTagValue = reader.ReadAsciiChars(24, true);

        // sanity check
        SpuriousDataInHeaderPadding = header.Skip(0x90).Any(b => b is not 0);
    }

    [MemberNotNull(nameof(_payload))]
    private void ExtractPayload()
    {
        Debug.Assert(_input.BaseStream.Position is 0x400);

        _payload = _input.ReadBytes(Length);

        byte[] padding = _input.ReadBytes(_paddedLength - Length);
        SpuriousDataInPayloadPadding = padding.Any(b => b is not 0);
    }

    public string ImageType { get; private set; }
    public ushort SecurityEpoch { get; private set; }
    public string VersionTagValue { get; private set; }
    public bool SpuriousDataInHeaderPadding { get; private set; }
    public bool SpuriousDataInPayloadPadding { get; private set; }

    public void Read(out byte[] payload)
    {
        payload = new byte[Length];
        Array.Copy(_payload, payload, Length);
    }

    public int Length { get; private set; }
    public byte this[int index] => _payload[index];

    public void Dispose()
    {
        _input.Dispose();
    }
}
