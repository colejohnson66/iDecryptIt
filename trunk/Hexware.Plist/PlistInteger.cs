/* =============================================================================
 * File:   PlistInteger.cs
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
using System.Xml;

namespace Hexware.Plist
{
    /// <summary>
    /// Represents a &lt;integer /&gt; tag using an <see cref="System.Int64"/>
    /// </summary>
    public partial class PlistInteger
    {
        /// <summary>
        /// Hexware.Plist.PlistInteger constructor from an <see cref="System.String"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is null or empty</exception>
        /// <exception cref="System.FormatException"><paramref name="value"/> is not an integer</exception>
        public PlistInteger(string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");

            try
            {
                _value = Convert.ToInt64(value);
            }
            catch (FormatException)
            {
                throw new FormatException("\"" + value + "\" is not an integer");
            }
        }

        /// <summary>
        /// Hexware.Plist.PlistInteger constructor from an <see cref="System.Single"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        /// <exception cref="System.FormatException"><paramref name="value"/> is out of the range of a <see cref="System.Int64"/></exception>
        public PlistInteger(float value)
        {
            try
            {
                _value = Convert.ToInt64(value);
            }
            catch (FormatException)
            {
                throw new FormatException("\"" + value + "\" cannot be converted to an long");
            }
        }

        /// <summary>
        /// Hexware.Plist.PlistInteger constructor from an <see cref="System.Double"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        /// <exception cref="System.FormatException"><paramref name="value"/> is out of the range of a <see cref="System.Int64"/></exception>
        public PlistInteger(double value)
        {
            try
            {
                _value = Convert.ToInt64(value);
            }
            catch (FormatException)
            {
                throw new FormatException("\"" + value + "\" cannot be converted to an long");
            }
        }

        /// <summary>
        /// Hexware.Plist.PlistInteger constructor from an <see cref="System.SByte"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        public PlistInteger(sbyte value)
        {
            _value = value;
        }

        /// <summary>
        /// Hexware.Plist.PlistInteger constructor from an <see cref="System.Byte"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        public PlistInteger(byte value)
        {
            _value = value;
        }

        /// <summary>
        /// Hexware.Plist.PlistInteger constructor from an <see cref="System.Int16"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        public PlistInteger(short value)
        {
            _value = value;
        }

        /// <summary>
        /// Hexware.Plist.PlistInteger constructor from an <see cref="System.UInt16"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        public PlistInteger(ushort value)
        {
            _value = value;
        }

        /// <summary>
        /// Hexware.Plist.PlistInteger constructor from an <see cref="System.Int32"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        public PlistInteger(int value)
        {
            _value = value;
        }

        /// <summary>
        /// Hexware.Plist.PlistInteger constructor from an <see cref="System.UInt32"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        public PlistInteger(uint value)
        {
            _value = value;
        }

        /// <summary>
        /// Hexware.Plist.PlistInteger constructor from an <see cref="System.Int64"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        public PlistInteger(long value)
        {
            _value = value;
        }

        /// <summary>
        /// Hexware.Plist.PlistInteger constructor from an <see cref="System.UInt64"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        /// <exception cref="System.FormatException"><paramref name="value"/> is out of the range of a <see cref="System.Int64"/></exception>
        public PlistInteger(ulong value)
        {
            try
            {
                _value = Convert.ToInt64(value);
            }
            catch (FormatException)
            {
                throw new FormatException("\"" + value + "\" cannot be converted to an long");
            }
        }
    }
    public partial class PlistInteger : IPlistElementInternal
    {
        internal static PlistInteger ReadBinary(BinaryReader reader, byte firstbyte)
        {
            int numofbytes = 1 << (firstbyte & 0x08);
            byte[] buf = reader.ReadBytes(numofbytes);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buf);

            // TODO: Add support for 128 bit integers
            if (numofbytes == 1) // 000
                return new PlistInteger(buf[0]);
            if (numofbytes == 2) // 001
                return new PlistInteger(BitConverter.ToUInt16(buf, 0));
            if (numofbytes == 4) // 010
                return new PlistInteger(BitConverter.ToUInt32(buf, 0));
            if (numofbytes == 8) // 011
                return new PlistInteger(BitConverter.ToInt64(buf, 0));
            //if (numofbytes == 16) // 100
            //    return new PlistInteger(...)

            // The specification uses the 3 bits to store the size,
            // allowing for integers up to 512 bits long. However,
            // CoreFoundation only implements support up to 128 bits.
            throw new PlistFormatException("Support does not exist for integers greater than 64 bits");
        }

        void IPlistElementInternal.WriteBinary(BinaryWriter writer)
        {
            byte[] buf;

            // 8, 16, and 32 bit integers have to be interpreted as unsigned,
            // whereas 64 bit integers are signed (and 16-byte when available).
            // Negative 8, 16, and 32 bit integers are always emitted as 8 bytes.
            // Integers are not required to be in the most compact possible
            // representation, but only the last 64 bits are significant.
            // integers are not required to be in the most compact possible representation, but only the last 64 bits are significant currently
            // Negative values are written as 8 bytes. The 1, 2,
            // and 4 byte size options are unsigned values.
            if (_value > 0) {
                if (_value <= Byte.MaxValue) {
                    writer.Write((byte)0x10);
                    writer.Write((byte)_value);
                    return;
                }

                if (_value <= UInt16.MaxValue) {
                    writer.Write((byte)0x11);
                    buf = BitConverter.GetBytes((ushort)_value);
                } else if (_value <= UInt32.MaxValue) {
                    writer.Write((byte)0x12);
                    buf = BitConverter.GetBytes((uint)_value);
                } else {
                    writer.Write((byte)0x13);
                    buf = BitConverter.GetBytes(_value);
                }
            } else {
                writer.Write((byte)0x13);
                buf = BitConverter.GetBytes(_value);
            }

            if (BitConverter.IsLittleEndian)
                Array.Reverse(buf);
            writer.Write(buf);
        }

        internal static PlistInteger ReadXml(XmlNode node)
        {
            return new PlistInteger(node.InnerText);
        }

        void IPlistElementInternal.WriteXml(XmlNode tree, XmlDocument writer)
        {
            XmlElement element = writer.CreateElement("integer");
            element.InnerText = _value.ToString();
            tree.AppendChild(element);
        }
    }
    public partial class PlistInteger : IPlistElement<long>
    {
        internal long _value;

        /// <summary>
        /// Gets the Xml tag for this element
        /// </summary>
        public string XmlTag
        {
            get
            {
                return "integer";
            }
        }

        /// <summary>
        /// Gets or sets the value of this element
        /// </summary>
        public long Value
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

        /// <summary>
        /// Gets the type of this element
        /// </summary>
        public PlistElementType ElementType
        {
            get
            {
                return PlistElementType.Integer;
            }
        }
    }
}