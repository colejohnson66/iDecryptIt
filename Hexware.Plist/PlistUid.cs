/* =============================================================================
 * File:   PlistUid.cs
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
    /// Represents a &lt;uid&gt; using a one-dimensional <see cref="System.Byte"/> array
    /// </summary>
    public partial class PlistUid
    {
        /// <summary>
        /// Hexware.Plist.PlistUid constructor using a one dimensional <see cref="System.Byte"/> array and a path
        /// </summary>
        /// <param name="value">The value of this node</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is null or empty</exception>
        /// <exception cref="Hexware.Plist.PlistFormatException"><paramref name="value"/> is bigger than the allowed 16 byte length</exception>
        public PlistUid(byte[] value)
        {
            if (value == null || value.Length == 0)
                throw new ArgumentNullException("value");
            if (value.Length > 16)
                throw new PlistFormatException("value array is bigger than the allowed 16 byte length");

            _value = value;
        }

        /// <summary>
        /// Gets the length of this element
        /// </summary>
        /// <returns>Amount of characters in decoded string</returns>
        public int GetPlistElementLength()
        {
            return _value.Length;
        }
    }
    public partial class PlistUid
    {
        internal static PlistUid ReadBinary(BinaryReader reader, byte firstbyte)
        {
            byte lowernibble = (byte)(firstbyte & 0x0F); // Get lower nibble
            int numofbytes = lowernibble + 1;
            byte[] buf = reader.ReadBytes(numofbytes);
            return new PlistUid(buf);
        }

        internal byte[] WriteBinary()
        {
            byte[] tag = new byte[]
            {
                (byte)(0xF0 | (_value.Length - 1))
            };
            byte[] buf = _value;
            PlistInternal.Merge(ref tag, ref buf);
            return tag;
        }

        internal static PlistUid ReadXml(XmlDocument reader, int index)
        {
            string val = reader.ChildNodes[index].InnerText.Substring(2); // trim off "0x" 
            int length = val.Length / 2;
            byte[] buf = new byte[length];
            for (int i = 0; i < length; i++)
            {
                buf[i] = Convert.ToByte(val.Substring(i * 2, 2), 16);
            }
            return new PlistUid(buf);
        }

        internal void WriteXml(XmlNode tree, XmlDocument writer)
        {
            XmlElement element = writer.CreateElement("string");
            int length = _value.Length;
            StringBuilder sb = new StringBuilder(length * 2 + 2);
            sb.Append("0x");
            for (int i = 0; i < length; i++)
            {
                sb.Append(String.Format("{0:X}", _value[i]));
            }
            element.InnerText = sb.ToString();
            tree.AppendChild(element);
        }
    }
    public partial class PlistUid : IPlistElement<byte[], Primitive>
    {
        internal byte[] _value;

        /// <summary>
        /// Gets the Xml tag for this element
        /// </summary>
        public string XmlTag
        {
            get
            {
                return "uid";
            }
        }

        /// <summary>
        /// Gets or sets the value of this element
        /// </summary>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is null</exception>
        public byte[] Value
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
        /// Gets the type of this element as one of <see cref="Hexware.Plist.Container"/> or <see cref="Hexware.Plist.Primitive"/>
        /// </summary>
        public Primitive ElementType
        {
            get
            {
                return Primitive.Uid;
            }
        }

        /// <summary>
        /// Gets the length of this element when written in binary mode
        /// </summary>
        /// <returns>Containers return the amount inside while Primitives return the binary length</returns>
        public int GetPlistElementBinaryLength()
        {
            // (tag length (1)) + (_value.Length - 1)
            return _value.Length;
        }
    }
}