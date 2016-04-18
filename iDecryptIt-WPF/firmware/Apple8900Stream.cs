/* =============================================================================
 * File:   Apple8900Stream.cs
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
using System.Security.Cryptography;

namespace Hexware.Programs.iDecryptIt.Firmware
{
    // This stream exposes the payload of the file as a stream, NOT the 8900 wrapper
    // A better name would be Apple8900PayloadStream, but in the future, this class
    // will allow changing the footer and other data.
    public class Apple8900Stream : Stream
    {
        /* Apple8900 {
         *    0  byte[4]  magic;   // "8900"
         *    4  byte[3]  version; // "1.0"
         *    7  byte     format;  // unencrypted == 0x4; encrypted == 0x3; types 0x1 and 0x2 exist, but are unsupported
         *    8  uint32;
         *    C  uint32   payloadLength;
         *   10  uint32   footerSigOffset;  // ignoring header
         *   14  uint32   footerCertOffset; // ignoring header
         *   18  uint32   footerCertSize;
         *   1C  byte[32] salt;
         *   3C  uint16;
         *   3E  uint16   epoch;
         *   40  byte[16] headerSig; // aes128cbc(sha1(file[0:0x40])[0:0x10], Key0x837, ZeroIV)
         *   50  byte[0x7B0] padding;
         *  800  byte[]   payload;   // sizeof(payload) == payloadLength (IMG2, DMG, or raw data)
         * ????  byte[?]  footer;    // padding, signature and certificate
         * }
         */
        
        // <https://www.theiphonewiki.com/wiki/AES_Keys#Key_0x837>
        // Used to encrypt/decrypt the contents of an 8900 file
        private static readonly byte[] Key0x837 = new byte[]
        {
            0x18, 0x84, 0x58, 0xA6, 0xD1, 0x50, 0x34, 0xDF,
            0xE3, 0x86, 0xF2, 0x3B, 0x61, 0xD4, 0x37, 0x74
        };
        private static readonly byte[] Magic = new byte[] { 0x38, 0x39, 0x30, 0x30 };

        private Stream _stream;
        private bool _encrypted = false;
        private byte[] _payload;
        private int _seekPos;

        public Apple8900Stream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("Stream must support reading.", "stream");
            if (!stream.CanSeek)
                throw new ArgumentException("Stream must support seeking.", "stream");

            _stream = stream;
            ParseStream();
        }

        private void ParseStream()
        {
            _stream.Seek(0, SeekOrigin.Begin);

            byte[] buf = new byte[0x800];
            if (_stream.Read(buf, 0, 0x800) != 0x800)
                throw new FileFormatException("Stream too small to contain an 8900 file.");

            if (buf[0] != Magic[0] || buf[1] != Magic[1] ||
                buf[2] != Magic[2] || buf[3] != Magic[3])
                throw new FileFormatException("Stream is not an Apple 8900 file.");

            if (buf[4] != '1' || buf[5] != '.' || buf[6] != '0')
                throw new FileFormatException("Unknown Apple 8900 file version. Only 1.0 is supported.");

            if (buf[7] != 3 && buf[7] != 4)
                throw new FileFormatException("Unknown Apple 8900 file type. Only types 3 and 4 are supported.");
            _encrypted = (buf[7] == 3);

            int payloadLength = BitConverter.ToInt32(buf, 0xC);
            if (payloadLength <= 0)
                throw new FileFormatException("Payload cannot have a zero or negative size.");

            _payload = new byte[payloadLength];
            _seekPos = 0;
            if (_stream.Read(_payload, 0, payloadLength) != payloadLength)
                throw new FileFormatException("Stream to small to contain indicated payload.");

            if (_encrypted)
                DecryptPayload();
        }
        private void DecryptPayload()
        {
            using (AesManaged aes = new AesManaged())
            {
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.IV = new byte[16];
                aes.Key = Key0x837;
                try
                {
                    MemoryStream ms = new MemoryStream();
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        // write the encrypted data to the crypto stream
                        cs.Write(_payload, 0, _payload.Length);
                        // then read back the decrypted data and discard the encrypted version
                        byte[] buf = ms.ToArray();
                        _payload = buf;
                    }
                }
                catch (CryptographicException)
                {
                    // ignore padding issue
                }
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
            // if `count' would put us past the end of the stream, fix it
            if (_seekPos + count > _payload.Length)
                count = _payload.Length - _seekPos;

            Array.Copy(_payload, _seekPos, buffer, offset, count);
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
