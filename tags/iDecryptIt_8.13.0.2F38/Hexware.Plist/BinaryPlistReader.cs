/* =============================================================================
 * File:   BinaryPlistReader.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2014-2015 Cole Johnson
 * 
 * This file is part of Hexware.Plist
 * 
 * Hexware.Plist is free software: you can redistribute it and/or modify it
 *   under the terms of the GNU Lesser General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or (at your
 *   option) any later version.
 * 
 * Hexware.Plist is distributed in the hope that it will be useful, but WITHOUT
 *   ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 *   FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public
 *   License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public License
 *   along with Hexware.Plist. If not, see <http://www.gnu.org/licenses/>.
 * =============================================================================
 */
using System;
using System.IO;

namespace Hexware.Plist
{
    internal class BinaryPlistReader : BinaryReader
    {
        internal BinaryPlistTrailer Trailer;

        internal int[] ObjectOffsets;

        internal BinaryPlistReader(Stream stream) : base(stream)
        {
        }

        internal IPlistElement ParseObject(int obj)
        {
            this.BaseStream.Seek(ObjectOffsets[obj], SeekOrigin.Begin);
            byte type = this.ReadByte();

            switch (type & 0xF0) {
                case 0x00:
                    switch (type) {
                        case 0x00:
                            return new PlistNull();
                        case 0x08:
                            return new PlistBool(false);
                        case 0x09:
                            return new PlistBool(true);
                        //case 0x0C:
                        //case 0x0D:
                        //    return new PlistUrl.ReadBinary(this, type);
                        case 0x0E:
                            return PlistUid.ReadBinary(this, type);
                        case 0x0F:
                            return null;
                    }
                    break;
                case 0x10:
                    return PlistInteger.ReadBinary(this, type);
                case 0x20:
                    return PlistReal.ReadBinary(this, type);
                case 0x30:
                    return PlistDate.ReadBinary(this, type);
                case 0x40:
                    return PlistData.ReadBinary(this, type);
                case 0x50:
                case 0x60:
                case 0x70:
                    return PlistString.ReadBinary(this, type);
                //case 0x80:
                //    return PlistUuid.ReadBinary(this, type);
                case 0xA0:
                    return PlistArray.ReadBinary(this, type);
                //case 0xB0:
                //case 0xC0:
                //    return PlistSet.ReadBinary(this, type);
                case 0xD0:
                    return PlistDict.ReadBinary(this, type);
            }

            throw new PlistException(String.Format(
                "Unknown binary Plist object encountered. Object {0} at offset 0x{1:x} has a tag of 0x{2:x}",
                obj,
                this.BaseStream.Position - 1,
                type));
        }

        internal static ulong ParseUnsignedBigEndianNumber(byte[] buf)
        {
            return ParseUnsignedBigEndianNumber(buf, 0, buf.Length);
        }
        internal static ulong ParseUnsignedBigEndianNumber(byte[] buf, int idx, int count)
        {
            ulong ret = 0;
            for (int i = 0; i < count; i++)
                ret = (ret << 8) | buf[idx + i];
            return ret;
        }
    }
}