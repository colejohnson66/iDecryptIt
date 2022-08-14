/* =============================================================================
 * File:   BiEndianBinaryReader.cs
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
using System.IO;
using System.Text;

namespace iDecryptIt.IO;

[PublicAPI]
public sealed class BiEndianBinaryReader : IDisposable
{
    private readonly Stream _stream;
    private readonly byte[] _buffer = new byte[8];
    private bool _disposed = false;

    public BiEndianBinaryReader(byte[] buffer)
        : this(new MemoryStream(buffer, false))
    { }

    public BiEndianBinaryReader(Stream stream)
    {
        _stream = stream;
    }

    // ReSharper disable once ConvertToAutoPropertyWhenPossible
    public Stream BaseStream => _stream;

    private Span<byte> BufferAsSpan(int length)
    {
        Debug.Assert(length is >= 0 and < 8);
        return _buffer.AsSpan()[..length];
    }

    private void ReadLE(Span<byte> buffer)
    {
        _stream.ReadExact(buffer);

        // if computer is BE, swap the buffer to that endianness
        if (!BitConverter.IsLittleEndian)
            buffer.Reverse();
    }

    private void ReadBE(Span<byte> buffer)
    {
        _stream.ReadExact(buffer);

        // if computer is LE, swap the buffer to that endianness
        if (BitConverter.IsLittleEndian)
            buffer.Reverse();
    }

    public void Skip(int count)
    {
        _stream.Position += count;
    }


    public void ReadBytes(Span<byte> buffer) =>
        _stream.ReadExact(buffer);

    public byte[] ReadBytes(int count)
    {
        byte[] buffer = new byte[count];
        ReadBytes(buffer);
        return buffer;
    }

    public string ReadAsciiChars(int length) =>
        Encoding.ASCII.GetString(ReadBytes(length));


    public sbyte ReadInt8() =>
        unchecked((sbyte)ReadUInt8());

    public byte ReadUInt8()
    {
        int c = _stream.ReadByte();
        if (c is -1)
            throw new EndOfStreamException($"Unexpected EOF in {nameof(ReadUInt8)}.");
        return (byte)c;
    }


    public short ReadInt16LE() =>
        unchecked((short)ReadUInt16LE());

    public ushort ReadUInt16LE()
    {
        Span<byte> span = BufferAsSpan(2);
        ReadLE(span);
        return BitConverter.ToUInt16(span);
    }

    public short ReadInt16BE() =>
        unchecked((short)ReadUInt16BE());

    public ushort ReadUInt16BE()
    {
        Span<byte> span = BufferAsSpan(2);
        ReadBE(span);
        return BitConverter.ToUInt16(span);
    }


    public int ReadInt32LE() =>
        unchecked((int)ReadUInt32LE());

    public uint ReadUInt32LE()
    {
        Span<byte> span = BufferAsSpan(4);
        ReadLE(span);
        return BitConverter.ToUInt32(span);
    }

    public int ReadInt32BE() =>
        unchecked((int)ReadUInt32BE());

    public uint ReadUInt32BE()
    {
        Span<byte> span = BufferAsSpan(4);
        ReadBE(span);
        return BitConverter.ToUInt32(span);
    }


    public long ReadInt64LE() =>
        unchecked((long)ReadUInt64LE());

    public ulong ReadUInt64LE()
    {
        Span<byte> span = BufferAsSpan(8);
        ReadLE(span);
        return BitConverter.ToUInt64(span);
    }

    public long ReadInt64BE() =>
        unchecked((long)ReadUInt64BE());

    public ulong ReadUInt64BE()
    {
        Span<byte> span = BufferAsSpan(4);
        ReadBE(span);
        return BitConverter.ToUInt64(span);
    }


    public Half ReadHalfLE() =>
        BitConverter.Int16BitsToHalf(ReadInt16LE());

    public Half ReadHalfBE() =>
        BitConverter.Int16BitsToHalf(ReadInt16BE());


    public float ReadSingleLE() =>
        BitConverter.Int32BitsToSingle(ReadInt32LE());

    public float ReadSingleBE() =>
        BitConverter.Int32BitsToSingle(ReadInt32BE());


    public double ReadDoubleLE() =>
        BitConverter.Int64BitsToDouble(ReadInt64LE());

    public double ReadDoubleBE() =>
        BitConverter.Int64BitsToDouble(ReadInt64BE());


    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            _stream.Dispose();
        }
    }
}
