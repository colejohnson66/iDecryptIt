/* =============================================================================
 * File:   IBootImageStream.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2016 Cole Johnson
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

namespace Hexware.Programs.iDecryptIt.Firmware
{
    public enum IBootImageColorType
    {
        Argb,
        Gray
    }
    public class IBootImageStream : Stream
    {
        /* IBootImage {
         *    0  byte[8]  magic;   // "iBootIm\0"
         *    8  uint32;
         *    C  uint32   compressionType;
         *   10  uint32   colorType;
         *   14  uint16   width;
         *   16  uint16   height;
         *   18  byte[0x28] padding;
         *   40  byte[]   payload; // sizeof(payload) == sizeof(file) - offsetof(payload)
         * }
         */

        private static readonly ulong Magic = 0x006D49746F6F4269; // "iBootIm\0" in big endian
        private static readonly uint LzssSignature = 0x6C7A7373; // "lzss" in little endian
        private static readonly uint ColorArgb = 0x61726762; // "argb" in little endian
        private static readonly uint ColorGray = 0x67726579; // "grey" in little endian
        
        private Stream _stream;
        private byte[] _payload;
        private int _seekPos;
        private IBootImageColorType _colorType;
        private int _width;
        private int _height;

        public IBootImageStream(Stream stream)
            : this(stream, true)
        { }

        public IBootImageStream(Stream stream, bool resetStreamPosition)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("Stream must support reading.", "stream");
            if (!stream.CanSeek)
                throw new ArgumentException("Stream must support seeking.", "stream");

            _stream = stream;
            ParseStream(resetStreamPosition);
        }

        private void ParseStream(bool resetPos)
        {
            if (resetPos)
                _stream.Seek(0, SeekOrigin.Begin);

            byte[] buf = new byte[0x40];
            if (_stream.Read(buf, 0, 0x40) != 0x40)
                throw new FileFormatException("Stream too small to contain an iBootImage file.");

            if (BitConverter.ToUInt64(buf, 0) != Magic)
                throw new FileFormatException("Stream is not an iBootImage file.");

            if (BitConverter.ToUInt32(buf, 0xC) != LzssSignature)
                throw new FileFormatException("Unknown iBootImage compression type. Only LZSS is supported.");

            uint colorType = BitConverter.ToUInt32(buf, 0x10);
            if (colorType == ColorGray)
                _colorType = IBootImageColorType.Gray;
            else if (colorType == ColorArgb)
                _colorType = IBootImageColorType.Argb;
            else
                throw new FileFormatException("Unknown iBootImage color type. Only ARGB and grey are supported");

            _width = BitConverter.ToInt16(buf, 0x14);
            _height = BitConverter.ToInt16(buf, 0x16);
            if (_width < 0 || _height < 0)
                throw new FileFormatException("iBootImage cannot have a negative dimension.");

            int compLen = (int)_stream.Length - 0x40;
            int decompLen = 0;
            if (_colorType == IBootImageColorType.Gray)
                decompLen = 2 * _width * _height;
            else //if (_colorType == IBootImageColorType.Argb)
                decompLen = 4 * _width * _height;

            byte[] compPayload = new byte[compLen];
            _payload = new byte[decompLen];
            if (_stream.Read(compPayload, 0, compLen) != compLen)
                throw new FileFormatException("Stream to small to contain indicated payload.");

            int length = Lzss.Decompress(_payload, compPayload);
            System.Diagnostics.Debug.Assert(length == decompLen);
        }

        public IBootImageColorType ColorType
        {
            get
            {
                return _colorType;
            }
        }
        public int Width
        {
            get
            {
                return _width;
            }
        }
        public int Height
        {
            get
            {
                return _height;
            }
        }

        public override bool CanRead
        {
            get
            {
                return _stream.CanRead;
            }
        }
        public override bool CanSeek
        {
            get
            {
                return _stream.CanSeek;
            }
        }
        public override bool CanWrite
        {
            get
            {
                // FIXME: Implement writing
                return false;
            }
        }
        public override long Length
        {
            get
            {
                return _payload.Length;
            }
        }
        public override long Position
        {
            get
            {
                return _seekPos;
            }

            set
            {
                Seek(value, SeekOrigin.Begin);
            }
        }

        public override void Flush()
        {
            _stream.Flush();
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_seekPos + count > _payload.Length)
                count = _payload.Length - _seekPos;

            Array.Copy(_payload, _seekPos, buffer, offset, count);
            _seekPos += count;
            return count;
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
                _seekPos = (int)offset;
            else if (origin == SeekOrigin.Current)
                _seekPos += (int)offset;
            else
                _seekPos = _payload.Length - (int)offset;
            return _seekPos;
        }
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}
