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
using System.Net;

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
    public partial class PlistInteger
    {
        internal static PlistInteger ReadBinary(BinaryReader reader, byte firstbyte)
        {
            firstbyte = (byte)(firstbyte & 0x0F); // Get lower nibble
            int numofbytes = (1 << firstbyte); // how many bytes are contained in this integer
            if (reader.BaseStream.Length < (reader.BaseStream.Position + numofbytes))
                throw new PlistFormatException("Length of element passes end of stream");

            byte[] buf = reader.ReadBytes(numofbytes);

            // "cast" big-endian to native-endian if necessary, then return
            if (numofbytes == 1)
                return new PlistInteger(buf[0]);
            if (numofbytes == 2)
                return new PlistInteger(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buf, 0)));
            if (numofbytes == 4)
                return new PlistInteger(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buf, 0)));
            if (numofbytes == 8)
                return new PlistInteger(IPAddress.NetworkToHostOrder(BitConverter.ToInt64(buf, 0)));

            throw new PlistFormatException("The received value from the stream is bigger than long");
        }

        internal byte[] WriteBinary()
        {
            byte[] tag;
            byte[] buf;
            int length = GetPlistElementBinaryLength();
            if (length == 2)
            {
                tag = new byte[1]
                {
                    0x10
                };
                buf = new byte[1]
                {
                    (byte)_value
                };
            }
            else if (length == 3)
            {
                tag = new byte[1]
                {
                    0x11
                };
                buf = BitConverter.GetBytes((short)_value);
            }
            else if (length == 5)
            {
                tag = new byte[1]
                {
                    0x12
                };
                buf = BitConverter.GetBytes((int)_value);
            }
            else /*if (length == 9) */
            {
                tag = new byte[1]
                {
                    0x13
                };
                buf = BitConverter.GetBytes(_value);
            }
            Array.Reverse(buf);
            PlistInternal.Merge(ref tag, ref buf);
            return tag;
        }

        internal static PlistInteger ReadXml(XmlDocument reader, int index)
        {
            return new PlistInteger(reader.ChildNodes[index].InnerText);
        }

        internal void WriteXml(XmlNode tree, XmlDocument writer)
        {
            XmlElement element = writer.CreateElement("integer");
            element.InnerText = _value.ToString();
            tree.AppendChild(element);
        }
    }
    public partial class PlistInteger : IPlistElement<long, Primitive>
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
        /// Gets the type of this element as one of <see cref="Hexware.Plist.Container"/> or <see cref="Hexware.Plist.Primitive"/>
        /// </summary>
        public Primitive ElementType
        {
            get
            {
                return Primitive.Integer;
            }
        }

        /// <summary>
        /// Gets the length of this element when written in binary mode
        /// </summary>
        /// <returns>Containers return the amount inside while Primitives return the binary length</returns>
        public int GetPlistElementBinaryLength()
        {
            if (_value >= Byte.MinValue && _value <= Byte.MaxValue)
                return 2;
            if (_value >= Int16.MinValue && _value <= Int16.MaxValue)
                return 3;
            if (_value >= Int32.MinValue && _value <= Int32.MaxValue)
                return 5;
            return 9;
        }
    }
}