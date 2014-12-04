/* =============================================================================
 * File:   PlistData.cs
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
    /// Represents a &lt;data /&gt; tag using a one-dimensional <see cref="System.Byte"/> array
    /// </summary>
    public partial class PlistData
    {
        /// <summary>
        /// Hexware.Plist.PlistData constructor using a one dimensional <see cref="System.Byte"/> array
        /// </summary>
        /// <param name="value">The value of this node</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is null</exception>
        public PlistData(byte[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            _value = value;
        }

        /// <summary>
        /// Hexware.Plist.PlistData constructor using a base64 encoded <see cref="System.String"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is null</exception>
        /// <exception cref="System.FormatException"><paramref name="value"/> is not a valid base64 encoded string</exception>
        public PlistData(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            value = value
                .Replace("\n", "")
                .Replace("\r", "")
                .Replace("\t", "")
                .Replace(" ", "");

            try
            {
                _value = Convert.FromBase64String(value);
            }
            catch (FormatException)
            {
                throw new FormatException("Not a valid base64 encoded string");
            }
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
    public partial class PlistData
    {
        internal static PlistData ReadBinary(BinaryReader reader, byte firstbyte)
        {
            int length = (byte)(firstbyte & 0x0F);
            if (length == 0x0F)
                length = (int)PlistInteger.ReadBinary(reader, reader.ReadByte()).Value;

            if (reader.BaseStream.Length < (reader.BaseStream.Position + length))
                throw new PlistFormatException("Length of element passes end of stream");

            return new PlistData(reader.ReadBytes(length));
        }

        internal byte[] WriteBinary()
        {
            byte[] tag;
            byte[] buf = _value;
            if (_value.Length < 0x0F)
            {
                tag = new byte[2]
                {
                    0x0F,
                    (byte)buf.Length
                };
            }
            else
            {
                tag = new byte[1]
                {
                    0x0F
                };
                byte[] temp = new PlistInteger(buf.Length).WriteBinary();
                PlistInternal.Merge(ref tag, ref temp);
            }
            PlistInternal.Merge(ref tag, ref buf);
            return tag;
        }

        internal static PlistData ReadXml(XmlDocument reader, int index)
        {
            return new PlistData(reader.ChildNodes[index].InnerText);
        }

        internal void WriteXml(XmlNode tree, XmlDocument writer)
        {
            int depth = 0;
            XmlNode clone = writer.Clone();
            while (clone.ParentNode != null)
            {
                clone = clone.ParentNode;
                depth++;
            }
            StringBuilder sb = new StringBuilder();
            while (depth != 0)
            {
                sb.Append("\t");
                depth--;
            }
            string indent = sb.ToString();

            XmlElement element = writer.CreateElement("data");
            string buf = Convert.ToBase64String(_value);
            int length = buf.Length;
            sb = new StringBuilder();
            sb.AppendLine();
            for (int i = 0; i < length; i++)
            {
                sb.Append(buf[i]);
                if (i % 64 == 0)
                {
                    sb.AppendLine();
                }
                else if (i % 65 == 1)
                {
                    sb.Append(indent);
                }
            }
            sb.AppendLine();
            element.InnerText = sb.ToString();
            tree.AppendChild(element);
        }
    }
    public partial class PlistData : IPlistElement<byte[], Primitive>
    {
        internal byte[] _value;

        /// <summary>
        /// Gets the Xml tag for this element
        /// </summary>
        public string XmlTag
        {
            get
            {
                return "data";
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
                return Primitive.Data;
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