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
using System.Buffers;
using System.Buffers.Binary;
using System.IO;
using System.Text;
using System.Threading;

namespace iDecryptIt.IO;

[PublicAPI]
public sealed class BiEndianBinaryReader : IDisposable
{
    private readonly bool _keepOpen;
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

        BaseStream = stream;
        _keepOpen = keepOpen;
    }


    /// <summary>
    /// Get the underlying stream.
    /// </summary>
    public Stream BaseStream { get; }


    /// <summary>
    /// Skip a specified number of bytes forwards.
    /// </summary>
    /// <param name="count">The number of bytes to skip.</param>
    public void Skip(int count)
    {
        BaseStream.Position += count;
    }

    /// <summary>
    /// Seek to a specific position.
    /// </summary>
    /// <param name="offset">The position, relative to the start of the stream, to seek to.</param>
    public void Seek(long offset)
    {
        BaseStream.Seek(offset, SeekOrigin.Begin);
    }

    /// <summary>
    /// Seek to a specific position.
    /// </summary>
    /// <param name="offset">The position, relative to <paramref name="origin" />, in the stream to seek to.</param>
    /// <param name="origin">Where <paramref name="offset" /> is relative to.</param>
    public void Seek(long offset, SeekOrigin origin)
    {
        BaseStream.Seek(offset, origin);
    }


    /// <summary>
    /// Read bytes from the input stream, and place them in a provided buffer.
    /// </summary>
    /// <param name="buffer">The buffer to place the read bytes into.</param>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
    public void ReadBytes(Span<byte> buffer) =>
        BaseStream.ReadExact(buffer);

    /// <summary>
    /// Read bytes from the input stream, and get an array containing them.
    /// </summary>
    /// <param name="count">The number of bytes to read.</param>
    /// <returns>An array of bytes with <paramref name="count" /> elements.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
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
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
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
    /// If <c>true</c>, anything from the first null character on will be removed from the returned string.
    /// </param>
    /// <returns>A string containing, at most, <paramref name="length" /> bytes, when interpreted as ASCII.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
    public string ReadAsciiChars(int length, bool trimNulls = false)
    {
        byte[]? pool = null;
        try
        {
            pool = ArrayPool<byte>.Shared.Rent(length);
            Span<byte> buf = pool.AsSpan()[..length];
            BaseStream.ReadExact(buf);

            if (trimNulls)
            {
                int lastNull = buf.IndexOf((byte)0);
                if (lastNull is not -1)
                    buf = buf[..lastNull];
            }

            return Encoding.ASCII.GetString(buf);
        }
        finally
        {
            if (pool is not null)
                ArrayPool<byte>.Shared.Return(pool);
        }
    }


    /// <summary>
    /// Read a single byte from the stream, and interpret it as a signed 8 bit integer.
    /// </summary>
    /// <returns>The next signed 8 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
    public sbyte ReadInt8() =>
        unchecked((sbyte)ReadUInt8());

    /// <summary>
    /// Read a single byte from the stream.
    /// </summary>
    /// <returns>The next unsigned 8 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
    public byte ReadUInt8()
    {
        int c = BaseStream.ReadByte();
        if (c is -1)
            throw new EndOfStreamException($"Unexpected EOF in {nameof(ReadUInt8)}.");
        return (byte)c;
    }


    /// <summary>
    /// Read two bytes from the stream, and interpret them as a signed 16 bit little endian integer.
    /// </summary>
    /// <returns>The next signed 16 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
    public short ReadInt16LE()
    {
        Span<byte> buf = stackalloc byte[2];
        BaseStream.ReadExact(buf);
        return BinaryPrimitives.ReadInt16LittleEndian(buf);
    }

    /// <summary>
    /// Read two bytes from the stream, and interpret them as an unsigned 16 bit little endian integer.
    /// </summary>
    /// <returns>The next unsigned 16 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
    public ushort ReadUInt16LE()
    {
        Span<byte> buf = stackalloc byte[2];
        BaseStream.ReadExact(buf);
        return BinaryPrimitives.ReadUInt16LittleEndian(buf);
    }

    /// <summary>
    /// Read two bytes from the stream, and interpret them as a signed 16 bit big endian integer.
    /// </summary>
    /// <returns>The next signed 16 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
    public short ReadInt16BE()
    {
        Span<byte> buf = stackalloc byte[2];
        BaseStream.ReadExact(buf);
        return BinaryPrimitives.ReadInt16BigEndian(buf);
    }

    /// <summary>
    /// Read two bytes from the stream, and interpret them as an unsigned 16 bit big endian integer.
    /// </summary>
    /// <returns>The next unsigned 16 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
    public ushort ReadUInt16BE()
    {
        Span<byte> buf = stackalloc byte[2];
        BaseStream.ReadExact(buf);
        return BinaryPrimitives.ReadUInt16BigEndian(buf);
    }


    /// <summary>
    /// Read four bytes from the stream, and interpret them as a signed 32 bit little endian integer.
    /// </summary>
    /// <returns>The next signed 32 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
    public int ReadInt32LE()
    {
        Span<byte> buf = stackalloc byte[4];
        BaseStream.ReadExact(buf);
        return BinaryPrimitives.ReadInt32LittleEndian(buf);
    }

    /// <summary>
    /// Read four bytes from the stream, and interpret them as an unsigned 32 bit little endian integer.
    /// </summary>
    /// <returns>The next unsigned 32 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
    public uint ReadUInt32LE()
    {
        Span<byte> buf = stackalloc byte[4];
        BaseStream.ReadExact(buf);
        return BinaryPrimitives.ReadUInt32LittleEndian(buf);
    }

    /// <summary>
    /// Read four bytes from the stream, and interpret them as a signed 32 bit big endian integer.
    /// </summary>
    /// <returns>The next signed 32 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
    public int ReadInt32BE()
    {
        Span<byte> buf = stackalloc byte[4];
        BaseStream.ReadExact(buf);
        return BinaryPrimitives.ReadInt32BigEndian(buf);
    }

    /// <summary>
    /// Read four bytes from the stream, and interpret them as an unsigned 32 bit big endian integer.
    /// </summary>
    /// <returns>The next unsigned 32 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
    public uint ReadUInt32BE()
    {
        Span<byte> buf = stackalloc byte[4];
        BaseStream.ReadExact(buf);
        return BinaryPrimitives.ReadUInt32BigEndian(buf);
    }


    /// <summary>
    /// Read eight bytes from the stream, and interpret them as a signed 64 bit little endian integer.
    /// </summary>
    /// <returns>The next signed 64 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
    public long ReadInt64LE()
    {
        Span<byte> buf = stackalloc byte[8];
        BaseStream.ReadExact(buf);
        return BinaryPrimitives.ReadInt64LittleEndian(buf);
    }

    /// <summary>
    /// Read eight bytes from the stream, and interpret them as an unsigned 64 bit little endian integer.
    /// </summary>
    /// <returns>The next unsigned 64 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
    public ulong ReadUInt64LE()
    {
        Span<byte> buf = stackalloc byte[8];
        BaseStream.ReadExact(buf);
        return BinaryPrimitives.ReadUInt64LittleEndian(buf);
    }

    /// <summary>
    /// Read eight bytes from the stream, and interpret them as a signed 64 bit big endian integer.
    /// </summary>
    /// <returns>The next signed 64 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
    public long ReadInt64BE()
    {
        Span<byte> buf = stackalloc byte[8];
        BaseStream.ReadExact(buf);
        return BinaryPrimitives.ReadInt64BigEndian(buf);
    }

    /// <summary>
    /// Read eight bytes from the stream, and interpret them as an unsigned 64 bit big endian integer.
    /// </summary>
    /// <returns>The next unsigned 64 bit integer from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
    public ulong ReadUInt64BE()
    {
        Span<byte> buf = stackalloc byte[8];
        BaseStream.ReadExact(buf);
        return BinaryPrimitives.ReadUInt64BigEndian(buf);
    }


    /// <summary>
    /// Read two bytes from the stream, and interpret them as a half precision little endian floating point number.
    /// </summary>
    /// <returns>The next half precision floating point number from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
    public Half ReadHalfLE()
    {
        Span<byte> buf = stackalloc byte[2];
        BaseStream.ReadExact(buf);
        return BinaryPrimitives.ReadHalfLittleEndian(buf);
    }

    /// <summary>
    /// Read two bytes from the stream, and interpret them as a half precision big endian floating point number.
    /// </summary>
    /// <returns>The next half precision floating point number from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
    public Half ReadHalfBE()
    {
        Span<byte> buf = stackalloc byte[2];
        BaseStream.ReadExact(buf);
        return BinaryPrimitives.ReadHalfBigEndian(buf);
    }


    /// <summary>
    /// Read four bytes from the stream, and interpret them as a single precision little endian floating point number.
    /// </summary>
    /// <returns>The next single precision floating point number from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
    public float ReadSingleLE()
    {
        Span<byte> buf = stackalloc byte[4];
        BaseStream.ReadExact(buf);
        return BinaryPrimitives.ReadSingleLittleEndian(buf);
    }

    /// <summary>
    /// Read four bytes from the stream, and interpret them as a single precision big endian floating point number.
    /// </summary>
    /// <returns>The next single precision floating point number from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
    public float ReadSingleBE()
    {
        Span<byte> buf = stackalloc byte[4];
        BaseStream.ReadExact(buf);
        return BinaryPrimitives.ReadSingleBigEndian(buf);
    }


    /// <summary>
    /// Read eight bytes from the stream, and interpret them as a double precision little endian floating point number.
    /// </summary>
    /// <returns>The next double precision floating point number from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
    public double ReadDoubleLE()
    {
        Span<byte> buf = stackalloc byte[8];
        BaseStream.ReadExact(buf);
        return BinaryPrimitives.ReadSingleLittleEndian(buf);
    }

    /// <summary>
    /// Read eight bytes from the stream, and interpret them as a double precision big endian floating point number.
    /// </summary>
    /// <returns>The next double precision floating point number from the stream.</returns>
    /// <exception cref="EndOfStreamException">If the EOF is reached in the middle of the read.</exception>
    public double ReadDoubleBE()
    {
        Span<byte> buf = stackalloc byte[8];
        BaseStream.ReadExact(buf);
        return BinaryPrimitives.ReadDoubleBigEndian(buf);
    }


    /// <inheritdoc />
    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            if (!_keepOpen)
                BaseStream.Dispose();
        }
    }
}
