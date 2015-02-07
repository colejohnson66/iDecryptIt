/* =============================================================================
 * File:   PlistString.cs
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
using System.Text;
using System.Xml;

namespace Hexware.Plist
{
    public partial class PlistString : IPlistElement
    {
        internal string _value;

        public PlistString(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            _value = value;
        }

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _value = value;
            }
        }
        public int Length
        {
            get
            {
                return _value.Length;
            }
        }

        public bool CanSerialize(PlistDocumentType type)
        {
            return true;
        }
        public PlistElementType ElementType
        {
            get
            {
                return PlistElementType.String;
            }
        }
    }
    public partial class PlistString : IPlistElementInternal
    {
        internal static PlistString ReadBinary(BinaryPlistReader reader, byte firstbyte)
        {
            // length is number of characters, so we can't just read them right off
            int type = firstbyte & 0xF0;
            int length = firstbyte & 0x0F;
            if (length == 0x0F)
                length = (int)PlistInteger.ReadBinary(reader, reader.ReadByte()).Value;

            if (type == 0x50)
                return new PlistString(
                    Encoding.ASCII.GetString(reader.ReadBytes(length)));
            if (type == 0x60)
                return new PlistString(
                    Encoding.BigEndianUnicode.GetString(reader.ReadBytes(length)));

            // UTF-8; the binary reader should've been created with UTF-8 decoder
            return new PlistString(new String(reader.ReadChars(length)));
        }
        void IPlistElementInternal.WriteBinary(BinaryPlistWriter writer)
        {
            // Always save as UTF-8
            int length = _value.Length;
            if (length < 0x0F) {
                writer.Write((byte)(0x70 | _value.Length));
            } else {
                writer.Write((byte)0x7F);
                writer.WriteTypedInteger(_value.Length);
            }
            writer.Write(Encoding.UTF8.GetBytes(_value));
        }
        internal static PlistString ReadXml(XmlNode node)
        {
            return new PlistString(node.InnerText);
        }
        void IPlistElementInternal.WriteXml(XmlNode tree, XmlDocument writer)
        {
            XmlElement element = writer.CreateElement("string");
            element.InnerText = _value;
            tree.AppendChild(element);
        }
    }
}