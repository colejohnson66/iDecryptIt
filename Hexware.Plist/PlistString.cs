/* =============================================================================
 * File:   PlistString.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2012, 2014 Cole Johnson
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
using System.Text;
using System.Xml;

namespace Hexware.Plist
{
    /// <summary>
    /// Represents a &lt;string /&gt; tag using a <see cref="System.String"/>
    /// </summary>
    public partial class PlistString
    {
        /// <summary>
        /// Hexware.Plist.PlistString constructor using a <see cref="System.String"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is null</exception>
        public PlistString(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            _value = value;
        }
    }
    public partial class PlistString : IPlistElementInternal
    {
        internal static PlistString ReadBinary(BinaryReader reader, byte firstbyte)
        {
            int type = firstbyte & 0xF0;
            int length = firstbyte & 0x0F;
            if (length == 0x0F)
                length = (int)PlistInteger.ReadBinary(reader, reader.ReadByte()).Value;
            byte[] buf = reader.ReadBytes(length);

            Encoding encoding;
            if (type == 0x50)
                encoding = Encoding.ASCII;
            else if (type == 0x60)
                encoding = Encoding.BigEndianUnicode;
            else // type == 0x70
                encoding = Encoding.UTF8; // NOTE: version "bplist1?"+
            return new PlistString(encoding.GetString(buf));
        }

        void IPlistElementInternal.WriteBinary(BinaryWriter writer)
        {
            int length = _value.Length;
            if (length < 0x0F)
                writer.Write((byte)(0x70 | _value.Length));
            else
                ((IPlistElementInternal)new PlistInteger(_value.Length)).WriteBinary(writer);

            // Always save as UTF-8. It's the same size as ASCII and
            // doesn't waste bytes if one character was not ASCII.
            writer.Write(Encoding.UTF8.GetBytes(_value));
        }

        internal static PlistString ReadXml(XmlNode node)
        {
            return new PlistString(node.InnerText);
        }

        void IPlistElementInternal.WriteXml(XmlNode tree, XmlDocument writer)
        {
            XmlElement element = writer.CreateElement(XmlTag);
            element.InnerText = _value;
            tree.AppendChild(element);
        }
    }
    public partial class PlistString : IPlistElement<string>
    {
        internal string _value;

        /// <summary>
        /// Gets the Xml tag for this element
        /// </summary>
        public string XmlTag
        {
            get
            {
                return "string";
            }
        }

        /// <summary>
        /// Gets or sets the value of this element
        /// </summary>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is null</exception>
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

        /// <summary>
        /// Gets the type of this element
        /// </summary>
        public PlistElementType ElementType
        {
            get
            {
                return PlistElementType.String;
            }
        }
    }
}