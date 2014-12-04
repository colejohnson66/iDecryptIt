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
    /// <remarks>&lt;ustring /&gt; doesn't exist in Xml Plists, only binary; the encoding of the Xml value is the same as the file</remarks>
    public partial class PlistString
    {
        internal bool _UTF16;

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

            // UTF-16?
            int length = value.Length;
            for (int i = 0; i < length; i++)
            {
                if (value[i] > 0x7F)
                {
                    _UTF16 = true;
                    return;
                }
            }
            //_UTF16 = false; // default(bool) == false
        }

        /// <summary>
        /// Gets a <see cref="System.Boolean"/> indicating if the string contains UTF-16 characters
        /// </summary>
        public bool UTF16
        {
            get
            {
                return _UTF16;
            }
        }

        /// <summary>
        /// Gets the length of this element
        /// </summary>
        /// <returns>Amount of characters in the string</returns>
        public int GetPlistElementLength()
        {
            return _value.Length;
        }
    }
    public partial class PlistString
    {
        internal static PlistString ReadBinary(BinaryReader reader, byte firstbyte)
        {
            bool ustring = ((firstbyte & 0xF0) >> 4 == 0x06);
            int length = firstbyte & 0x0F;
            if (length == 0x0F)
            {
                length = (int)PlistInteger.ReadBinary(reader, reader.ReadByte()).Value;
            }
            if (ustring)
            {
                // UTF-16 uses two bytes per character
                length = length * 2;
            }

            byte[] buf;
            if (reader.BaseStream.Length < (reader.BaseStream.Position + length))
                throw new PlistFormatException("Length of element passes end of stream");

            buf = reader.ReadBytes(length);
            if (ustring)
                return new PlistString(Encoding.BigEndianUnicode.GetString(buf));
            return new PlistString(Encoding.ASCII.GetString(buf));
        }

        internal byte[] WriteBinary()
        {
            byte[] tag;
            byte[] buf;            
            if (GetPlistElementLength() > 0x0D)
            {
                tag = new PlistInteger(GetPlistElementLength()).WriteBinary();
            }
            else
            {
                if (_UTF16)
                {
                    tag = new byte[1]
                    {
                        (byte)(0x60 | _value.Length)
                    };
                }
                else
                {
                    tag = new byte[1]
                    {
                        (byte)(0x50 | _value.Length)
                    };
                }
            }
            Encoding enc = (_UTF16) ? Encoding.BigEndianUnicode : Encoding.ASCII;
            buf = enc.GetBytes(_value);
            PlistInternal.Merge(ref tag, ref buf);
            return tag;
        }

        internal static PlistString ReadXml(XmlDocument reader, int index)
        {
            return new PlistString(reader.ChildNodes[index].InnerText);
        }

        internal void WriteXml(XmlNode tree, XmlDocument writer)
        {
            XmlElement element = writer.CreateElement(XmlTag);
            element.InnerText = _value;
            tree.AppendChild(element);
        }
    }
    public partial class PlistString : IPlistElement<string, Primitive>
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
                //return (_UTF16 ? "ustring" : "string");
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

                // UTF-16?
                int length = value.Length;
                for (int i = 0; i < length; i++)
                {
                    if (value[i] > 0x7F)
                    {
                        _UTF16 = true;
                        return;
                    }
                }
                _UTF16 = false;
            }
        }

        /// <summary>
        /// Gets the type of this element as one of <see cref="Hexware.Plist.Container"/> or <see cref="Hexware.Plist.Primitive"/>
        /// </summary>
        public Primitive ElementType
        {
            get
            {
                return (_UTF16 ? Primitive.UString : Primitive.String);
            }
        }

        /// <summary>
        /// Gets the length of this element when written in binary mode
        /// </summary>
        /// <returns>Containers return the amount inside while Primitives return the binary length</returns>
        public int GetPlistElementBinaryLength()
        {
            return WriteBinary().Length;
        }
    }
}