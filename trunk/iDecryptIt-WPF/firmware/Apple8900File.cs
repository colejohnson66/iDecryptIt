/* =============================================================================
 * File:   Apple8900File.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2015-2016 Cole Johnson
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
    public class Apple8900File
    {
        /* Apple8900 {
         *    0  byte[4]  magic;   // "8900"
         *    4  byte[3]  version; // "1.0"
         *    7  byte     format;  // unencrypted == 0x4; encrypted == 0x3
         *    8  uint32;
         *    C  uint32   dataSize;
         *   10  uint32   footerSigOffset;  // ignoring header
         *   14  uint32   footerCertOffset; // ignoring header
         *   18  uint32   footerCertSize;
         *   1C  byte[32] salt;
         *   3C  uint16;
         *   3E  uint16   epoch;
         *   40  byte[16] headerSig; // aes128cbc(sha1(file[0:0x40])[0:0x10], Key0x837, ZeroIV)
         *   50  byte[0x7B0] padding;
         *  800  Img2File payload;   // sizeof(payload) == dataSize
         * ????  byte[?]  footer;    // padding, signature and certificate
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

        public Apple8900File(Stream stream)
        {
            _stream = stream;
        }

        public byte[] GetPayload()
        {
            return GetPayload(true);
        }

        public byte[] GetPayload(bool resetStreamPosition)
        {
            if (resetStreamPosition)
                _stream.Seek(0, SeekOrigin.Begin);

            // Read out the header
            byte[] header = new byte[0x800];
            if (_stream.Read(header, 0, 0x800) != 0x800)
                throw new FileFormatException("An 8900 file must be longer than 2048 bytes.");

            if (header[0] != Magic[0] || header[1] != Magic[1] ||
                header[2] != Magic[2] || header[3] != Magic[3])
                throw new FileFormatException("File not a valid 8900 file.");

            if (header[4] != '1' || header[5] != '.' || header[6] != '0')
                throw new FileFormatException("Unknown 8900 file version: " +
                    (char)header[4] + (char)header[5] + (char)header[6]);

            byte format = header[7];
            if (format != 0x3 && format != 0x4)
                throw new FileFormatException("Unknown 8900 file format: " + format);
            bool encrypted = (format == 0x3);

            uint dataSize = BitConverter.ToUInt32(header, 0xC);
            if (dataSize > Int32.MaxValue)
                throw new FileFormatException("8900 file contains an invalid payload size.");
            byte[] payload = new byte[dataSize];
            if (_stream.Read(payload, 0, (int)dataSize) != dataSize)
                throw new FileFormatException("8900 file contains an invalid payload size.");

            if (encrypted)
                DecryptPayload(ref payload);

            return payload;
        }

        private void DecryptPayload(ref byte[] payload)
        {
            using (AesManaged aes = new AesManaged()) {
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.IV = new byte[16];
                aes.Key = Key0x837;
                try
                {
                    MemoryStream ms = new MemoryStream();
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(payload, 0, payload.Length);
                        byte[] buf = ms.ToArray();
                        payload = buf;
                    }
                }
                catch (CryptographicException)
                {
                    // ignore padding issue
                }
            }
        }
    }
}
