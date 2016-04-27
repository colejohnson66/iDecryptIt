/* =============================================================================
 * File:   Img2Stream.cs
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
    public class Img2Stream : Stream
    {
        /* Img2 {
         *    0  byte[4]  magic;     // "2gmI" ("Img2" in little endian)
         *    4  byte[4]  imageType; // eg. "logo" for AppleLogo
         *    8  uint16;
         *    A  uint16   epoch;
         *    C  uint32   flags1;
         *   10  uint32   payloadLengthPadded;
         *   14  uint32   payloadLength;
         *   18  uint32;
         *   1C  uint32   flags2;         // 0x0100'0000 has to be unset
         *   20  byte[64];
         *   60  uint32;                  // possibly a length field
         *   64  uint32   headerChecksum; // crc32(file[0:0x64])
         *   68  uint32   checksum2;
         *   6C  uint32;                  // always 0xFFFF'FFFF?
         *   70  VersionTag {
         *         70  byte[4]  magic;   // "srev" ("vers" in little endian)
         *         74  uint32;
         *         78  byte[24] version; // "EmbeddedImages-##" (terminated with a null and 0xFF)
         *       }
         *   90  byte[0x370];
         *  400  byte[]  payload; // sizeof(payload) == payloadLengthPadded
         */

        private static readonly uint Magic = 0x496D6732; // "Img2" in little endian

        private Stream _stream;
        private uint _imageType;
        private byte[] _payload;
        private int _seekPos;

        public Img2Stream(Stream stream)
            : this(stream, true)
        { }

        public Img2Stream(Stream stream, bool resetStreamPosition)
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

            byte[] buf = new byte[0x400];
            if (_stream.Read(buf, 0, 0x400) != 0x400)
                throw new FileFormatException("Stream too small to contain an IMG2 file.");

            if (BitConverter.ToUInt32(buf, 0) != Magic)
                throw new FileFormatException("Stream is not an IMG2 file.");

            _imageType = BitConverter.ToUInt32(buf, 4);

            int payloadLength = BitConverter.ToInt32(buf, 0x14);
            if (payloadLength <= 0)
                throw new FileFormatException("Payload cannot have a zero or negative size.");

            _payload = new byte[payloadLength];
            _seekPos = 0;
            if (_stream.Read(_payload, 0, payloadLength) != payloadLength)
                throw new FileFormatException("Stream to small to contain indicated payload.");
        }

        public uint ImageType
        {
            get
            {
                return _imageType;
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
