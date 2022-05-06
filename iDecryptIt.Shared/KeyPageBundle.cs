/* =============================================================================
 * File:   KeyPageBundle.cs
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace iDecryptIt.Shared;

[PublicAPI]
public class KeyPageBundle : IDisposable
{
    // buildID -> (offset, length)
    // offsets are the offset AFTER `_startOffset`
    private Dictionary<string, (int, int)> _offsets;
    private BinaryReader _reader;
    private int _startOffset;

    private KeyPageBundle(Dictionary<string, (int, int)> offsets, BinaryReader reader)
    {
        _offsets = offsets;
        _reader = reader;
        _startOffset = (int)reader.BaseStream.Position;
        Builds = new(_offsets.Keys.ToList());
    }

    public static KeyPageBundle Open(BinaryReader reader)
    {
        if (IOHelpers.HEADER_BUNDLE.Any(c => reader.ReadByte() != (byte)c))
            throw new InvalidDataException("Bad header.");

        // header
        Dictionary<string, (int, int)> offsets = new();
        int count = reader.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            string name = reader.ReadString();
            int offset = reader.ReadInt32();
            int length = reader.ReadInt32();
            offsets.Add(name, (offset, length));
        }

        return new(offsets, reader);
    }

    public ReadOnlyCollection<string> Builds { get; }

    public bool HasBuild(string buildID) =>
        _offsets.ContainsKey(buildID);

    public KeyPage Read(string buildID)
    {
        if (!_offsets.TryGetValue(buildID, out (int, int) value))
            throw new ArgumentException($"Build {buildID} does not exist.", nameof(buildID));

        _reader.BaseStream.Seek(_startOffset + value.Item1, SeekOrigin.Begin);
        byte[] file = _reader.ReadBytes(value.Item2);

        using BinaryReader reader = new(new MemoryStream(file), Encoding.UTF8);
        return KeyPage.Deserialize(reader);
    }

    public void Dispose()
    {
        _reader.Dispose();
        GC.SuppressFinalize(this);
    }
}
