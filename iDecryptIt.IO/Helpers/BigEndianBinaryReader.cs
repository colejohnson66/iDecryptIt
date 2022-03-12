/* =============================================================================
 * File:   BigEndianBinaryReader.cs
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

using System;
using System.IO;
using System.Text;

namespace iDecryptIt.IO.Helpers;

internal class BigEndianBinaryReader
{
    private readonly Stream _stream;

    public BigEndianBinaryReader(Stream input)
    {
        _stream = input;
    }

    public void Skip(int count)
    {
        _stream.Position += count;
    }

    public string ReadAsciiChars(int count)
    {
        byte[] bytes = new byte[count];
        if (_stream.Read(bytes) != count)
            throw new EndOfStreamException("Unexpected EOF.");
        return Encoding.ASCII.GetString(bytes);
    }

    public byte[] ReadBytes(int count)
    {
        byte[] buffer = new byte[count];
        if (_stream.Read(buffer) != count)
            throw new EndOfStreamException("Unexpected EOF.");
        return buffer;
    }

    public uint ReadUInt32()
    {
        byte[] buffer = new byte[4];
        if (_stream.Read(buffer) != 4)
            throw new EndOfStreamException("Unexpected EOF.");
        Array.Reverse(buffer);
        return BitConverter.ToUInt32(buffer);
    }

    public ulong ReadUInt64()
    {
        byte[] buffer = new byte[8];
        if (_stream.Read(buffer) != 8)
            throw new EndOfStreamException("Unexpected EOF.");
        Array.Reverse(buffer);
        return BitConverter.ToUInt64(buffer);
    }
}
