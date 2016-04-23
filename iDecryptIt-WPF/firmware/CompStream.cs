/* =============================================================================
 * File:   CompStream.cs
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
    public class CompStream : Stream
    {
        /* Comp {
         *    0  byte[4]  magic;     // "comp"
         *    4  uint32   compressionType;
         *    8  uint32   checksum;
         *    C  uint32   decompressedLength;   // big endian
         *   10  uint32   compressedLength; // big endian
         *   14  byte[0x16C] padding;
         *  180  byte[]   payload;   // sizeof(payload) == compressedLength
         * }
         */

        private static readonly uint Magic = 0x706D6F63; // "comp" in big endian
        private static readonly uint LzssSignature = 0x73737A6C; // "lzss" in big endian

        private Stream _stream;
        private byte[] _payload;
        private int _seekPos;

        public CompStream(Stream stream)
            : this(stream, true)
        { }

        public CompStream(Stream stream, bool resetStreamPosition)
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

            byte[] buf = new byte[0x180];
            if (_stream.Read(buf, 0, 0x180) != 0x180)
                throw new FileFormatException("Stream too small to contain a Comp file.");

            if (BitConverter.ToUInt32(buf, 0) != Magic)
                throw new FileFormatException("Stream is not a Comp file.");

            if (BitConverter.ToUInt32(buf, 4) != LzssSignature)
                throw new FileFormatException("Unknown Comp compression type. Only LZSS is supported.");

            // the lengths are stored in big endian; reverse them
            byte[] tmp = new byte[4];
            Array.Copy(buf, 0xC, tmp, 0, 4);
            Array.Reverse(tmp);
            int decompLen = BitConverter.ToInt32(tmp, 0);
            Array.Copy(buf, 0x10, tmp, 0, 4);
            Array.Reverse(tmp);
            int compLen = BitConverter.ToInt32(tmp, 0);
            if (compLen <= 0 || decompLen <= 0)
                throw new FileFormatException("Payload cannot have a zero or negative size.");

            byte[] compPayload = new byte[compLen];
            _payload = new byte[decompLen];
            if (_stream.Read(compPayload, 0, compLen) != compLen)
                throw new FileFormatException("Stream to small to contain indicated payload.");

            int length = Lzss.Decompress(_payload, compPayload);
            if (length != decompLen)
                throw new FileFormatException("Decompressed stream not expected size.");
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
