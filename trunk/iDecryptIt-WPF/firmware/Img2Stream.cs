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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexware.Programs.iDecryptIt.Firmware
{
    public class Img2Stream
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
         *   1C  uint32   flags2;        // 0x0100'0000 has to be unset
         *   20  byte[64];
         *   60  uint32;                 // possibly a length field
         *   64  uint32  headerChecksum; // crc32(file[0:0x64])
         *   68  uint32  checksum2;
         *   6C  uint32;                 // always 0xFFFF'FFFF?
         *   70  VersionTag {
         *         70  byte[4]  magic;   // "srev" ("vers" in little endian)
         *         74  uint32;
         *         78  byte[24] version; // "EmbeddedImages-##" (terminated with a null and 0xFF)
         *       }
         *   90  byte[0x370];
         *  400  byte[]  payload; // sizeof(payload) == payloadLengthPadded
         */

        private static readonly uint Magic = 0x496D6732; // "Img2" in little endian
    }
}
