/* =============================================================================
 * File:   PlistUid.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2012, 2014-2015 Cole Johnson
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
using System.Xml;

namespace Hexware.Plist
{
    // http://www.cclgroupltd.com/geek-post-nskeyedarchiver-files-what-are-they-and-how-can-i-use-them/
    public partial class PlistUid : IPlistElement
    {
        internal ulong _value;

        public PlistUid(ulong value)
        {
            _value = value;
        }
        public static implicit operator PlistUid(ulong value)
        {
            return new PlistUid(value);
        }

        public ulong Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public bool CanSerialize(PlistDocumentType type)
        {
            if (type == PlistDocumentType.Binary)
                return true;
            return false;
        }
        public PlistElementType ElementType
        {
            get
            {
                return PlistElementType.Uid;
            }
        }
    }
    public partial class PlistUid : IPlistElementInternal
    {
        internal static PlistUid ReadBinary(BinaryPlistReader reader, byte firstbyte)
        {
            int count = firstbyte & 0x0f + 1;

            ulong ret = 0;
            for (int i = 0; i < count; i++)
                ret = (ret << 8) + reader.ReadByte();

            return new PlistUid(ret);
        }
        void IPlistElementInternal.WriteBinary(BinaryPlistWriter writer)
        {
            byte[] buf;
            if (_value <= Byte.MaxValue) {
                writer.Write((byte)0x80);
                writer.Write((byte)_value);
                return;
            } else if (_value <= UInt16.MaxValue) {
                writer.Write((byte)0x81);
                buf = BitConverter.GetBytes((ushort)_value);
            } else if (_value <= UInt32.MaxValue) {
                writer.Write((byte)0x83);
                buf = BitConverter.GetBytes((uint)_value);
            } else {
                writer.Write((byte)0x87);
                buf = BitConverter.GetBytes(_value);
            }

            if (BitConverter.IsLittleEndian)
                Array.Reverse(buf);

            writer.Write(buf);
        }
        void IPlistElementInternal.WriteXml(XmlNode tree, XmlDocument writer)
        {
            throw new PlistException("UIDs can only be serialized in binary");
        }
    }
}