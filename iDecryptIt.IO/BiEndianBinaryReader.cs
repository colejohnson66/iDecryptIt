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
    private readonly bool _keepOpen;
    private readonly byte[] _buffer = new byte[8];
    private bool _disposed = false;

    public BiEndianBinaryReader(byte[] buffer)
        : this(new MemoryStream(buffer, false), false)
    { }

    public BiEndianBinaryReader(Stream stream, bool resetPosition = true, bool keepOpen = false)
    {
        if (!stream.CanSeek)
            throw new ArgumentException("Input stream must be seekable.", nameof(stream));

        if (resetPosition)
            stream.Position = 0;

        _stream = stream;
        _keepOpen = keepOpen;
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

    /// <summary>
    /// Skip a specified number of bytes forwards.
    /// </summary>
    /// <param name="count">The number of bytes to skip.</param>
    public void Skip(int count)
    {
        _stream.Position += count;
    }

    /// <summary>
    /// Seek to a specific position.
    /// </summary>
    /// <param name="offset">The position, relative to the start of the stream, to seek to.</param>
    public void Seek(long offset)
    {
        _stream.Seek(offset, SeekOrigin.Begin);
    }

    /// <summary>
    /// Seek to a specific position.
    /// </summary>
    /// <param name="offset">The position, relative to <paramref name="origin" />, in the stream to seek to.</param>
    /// <param name="origin">Where <paramref name="offset" /> is relative to.</param>
    public void Seek(long offset, SeekOrigin origin)
    {
        _stream.Seek(offset, origin);
    }


    /// <summary>
    /// Read bytes from the input stream, and place them in a provided buffer.
    /// </summary>
    /// <param name="buffer">The buffer to place the read bytes into.</param>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public void ReadBytes(Span<byte> buffer) =>
        _stream.ReadExact(buffer);

    /// <summary>
    /// Read bytes from the input stream, and get an array containing them.
    /// </summary>
    /// <param name="count">The number of bytes to read.</param>
    /// <returns>An array of bytes with <paramref name="count" /> elements.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public byte[] ReadBytes(int count)
    {
        byte[] buffer = new byte[count];
        ReadBytes(buffer);
        return buffer;
    }

    /// <summary>
    /// Read bytes from the input stream, and get an array containing them.
    /// </summary>
    /// <param name="count">The number of bytes to read.</param>
    /// <returns>An array of bytes with <paramref name="count" /> elements.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public byte[] ReadBytes(long count)
    {
        byte[] buffer = new byte[count];
        ReadBytes(buffer);
        return buffer;
    }

    /// <summary>
    /// Read bytes from the input stream, and interpret them as an ASCII string.
    /// </summary>
    /// <param name="length">The number of bytes to read.</param>
    /// <param name="trimNulls">
    /// If <c>true</c>, null characters will be trimmed from the end of the returned string.
    /// </param>
    /// <returns>A string containing the next <paramref name="length" /> bytes, if they were encoded in ASCII.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public string ReadAsciiChars(int length, bool trimNulls = false)
    {
        byte[] buffer = ReadBytes(length);
        return trimNulls
            ? buffer.ToStringNoTrailingNulls()
            : Encoding.ASCII.GetString(buffer);
    }


    /// <summary>
    /// Read a single byte from the stream, and interpret it as a signed 8 bit integer.
    /// </summary>
    /// <returns>The next signed 8 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public sbyte ReadInt8() =>
        unchecked((sbyte)ReadUInt8());

    /// <summary>
    /// Read a single byte from the stream.
    /// </summary>
    /// <returns>The next unsigned 8 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public byte ReadUInt8()
    {
        int c = _stream.ReadByte();
        if (c is -1)
            throw new EndOfStreamException($"Unexpected EOF in {nameof(ReadUInt8)}.");
        return (byte)c;
    }


    /// <summary>
    /// Read two bytes from the stream, and interpret them as a signed 16 bit little endian integer.
    /// </summary>
    /// <returns>The next signed 16 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public short ReadInt16LE() =>
        unchecked((short)ReadUInt16LE());

    /// <summary>
    /// Read two bytes from the stream, and interpret them as an unsigned 16 bit little endian integer.
    /// </summary>
    /// <returns>The next unsigned 16 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public ushort ReadUInt16LE()
    {
        Span<byte> span = BufferAsSpan(2);
        ReadLE(span);
        return BitConverter.ToUInt16(span);
    }

    /// <summary>
    /// Read two bytes from the stream, and interpret them as a signed 16 bit big endian integer.
    /// </summary>
    /// <returns>The next signed 16 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public short ReadInt16BE() =>
        unchecked((short)ReadUInt16BE());

    /// <summary>
    /// Read two bytes from the stream, and interpret them as an unsigned 16 bit big endian integer.
    /// </summary>
    /// <returns>The next unsigned 16 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public ushort ReadUInt16BE()
    {
        Span<byte> span = BufferAsSpan(2);
        ReadBE(span);
        return BitConverter.ToUInt16(span);
    }


    /// <summary>
    /// Read four bytes from the stream, and interpret them as a signed 32 bit little endian integer.
    /// </summary>
    /// <returns>The next signed 32 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public int ReadInt32LE() =>
        unchecked((int)ReadUInt32LE());

    /// <summary>
    /// Read four bytes from the stream, and interpret them as an unsigned 32 bit little endian integer.
    /// </summary>
    /// <returns>The next unsigned 32 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public uint ReadUInt32LE()
    {
        Span<byte> span = BufferAsSpan(4);
        ReadLE(span);
        return BitConverter.ToUInt32(span);
    }

    /// <summary>
    /// Read four bytes from the stream, and interpret them as a signed 32 bit big endian integer.
    /// </summary>
    /// <returns>The next signed 32 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public int ReadInt32BE() =>
        unchecked((int)ReadUInt32BE());

    /// <summary>
    /// Read four bytes from the stream, and interpret them as an unsigned 32 bit big endian integer.
    /// </summary>
    /// <returns>The next unsigned 32 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public uint ReadUInt32BE()
    {
        Span<byte> span = BufferAsSpan(4);
        ReadBE(span);
        return BitConverter.ToUInt32(span);
    }


    /// <summary>
    /// Read eight bytes from the stream, and interpret them as a signed 64 bit little endian integer.
    /// </summary>
    /// <returns>The next signed 64 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public long ReadInt64LE() =>
        unchecked((long)ReadUInt64LE());

    /// <summary>
    /// Read eight bytes from the stream, and interpret them as an unsigned 64 bit little endian integer.
    /// </summary>
    /// <returns>The next unsigned 64 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public ulong ReadUInt64LE()
    {
        Span<byte> span = BufferAsSpan(8);
        ReadLE(span);
        return BitConverter.ToUInt64(span);
    }

    /// <summary>
    /// Read eight bytes from the stream, and interpret them as a signed 64 bit big endian integer.
    /// </summary>
    /// <returns>The next signed 64 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public long ReadInt64BE() =>
        unchecked((long)ReadUInt64BE());

    /// <summary>
    /// Read eight bytes from the stream, and interpret them as an unsigned 64 bit big endian integer.
    /// </summary>
    /// <returns>The next unsigned 64 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public ulong ReadUInt64BE()
    {
        Span<byte> span = BufferAsSpan(4);
        ReadBE(span);
        return BitConverter.ToUInt64(span);
    }


    /// <summary>
    /// Read two bytes from the stream, and interpret them as a little endian half precision floating point number.
    /// </summary>
    /// <returns>The next half precision floating point number from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public Half ReadHalfLE() =>
        BitConverter.Int16BitsToHalf(ReadInt16LE());

    /// <summary>
    /// Read two bytes from the stream, and interpret them as a big endian half precision floating point number.
    /// </summary>
    /// <returns>The next half precision floating point number from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public Half ReadHalfBE() =>
        BitConverter.Int16BitsToHalf(ReadInt16BE());


    /// <summary>
    /// Read four bytes from the stream, and interpret them as a little endian single precision floating point number.
    /// </summary>
    /// <returns>The next single precision floating point number from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public float ReadSingleLE() =>
        BitConverter.Int32BitsToSingle(ReadInt32LE());

    /// <summary>
    /// Read four bytes from the stream, and interpret them as a big endian single precision floating point number.
    /// </summary>
    /// <returns>The next single precision floating point number from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public float ReadSingleBE() =>
        BitConverter.Int32BitsToSingle(ReadInt32BE());


    /// <summary>
    /// Read eight bytes from the stream, and interpret them as a little endian double precision floating point number.
    /// </summary>
    /// <returns>The next double precision floating point number from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public double ReadDoubleLE() =>
        BitConverter.Int64BitsToDouble(ReadInt64LE());

    /// <summary>
    /// Read eight bytes from the stream, and interpret them as a big endian double precision floating point number.
    /// </summary>
    /// <returns>The next double precision floating point number from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle or the read.</exception>
    public double ReadDoubleBE() =>
        BitConverter.Int64BitsToDouble(ReadInt64BE());


    /// <inheritdoc />
    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            if (!_keepOpen)
                _stream.Dispose();
        }
    }
}
