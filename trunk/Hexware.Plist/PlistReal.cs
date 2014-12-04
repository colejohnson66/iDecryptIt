/* =============================================================================
 * File:   PlistReal.cs
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
    /// Represents a &lt;real /&gt; tag using a <see cref="System.Double"/>
    /// </summary>
    public partial class PlistReal
    {
        /// <summary>
        /// Hexware.Plist.PlistReal constructor from an <see cref="System.String"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is null or empty</exception>
        /// <exception cref="System.FormatException"><paramref name="value"/> is not a real (<see cref="System.Double"/>)</exception>
        public PlistReal(string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");

            try
            {
                _value = Convert.ToDouble(value);
            }
            catch (FormatException)
            {
                throw new FormatException("\"" + value + "\" is not an real (double)");
            }
        }

        /// <summary>
        /// Hexware.Plist.PlistReal constructor from an <see cref="System.Single"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        public PlistReal(float value)
        {
            _value = value;
        }

        /// <summary>
        /// Hexware.Plist.PlistReal constructor from an <see cref="System.Double"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        public PlistReal(double value)
        {
            _value = value;
        }

        /// <summary>
        /// Hexware.Plist.PlistReal constructor from an <see cref="System.SByte"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        public PlistReal(sbyte value)
        {
            _value = value;
        }

        /// <summary>
        /// Hexware.Plist.PlistReal constructor from an <see cref="System.Byte"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        public PlistReal(byte value)
        {
            _value = value;
        }

        /// <summary>
        /// Hexware.Plist.PlistReal constructor from an <see cref="System.Int16"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        public PlistReal(short value)
        {
            _value = value;
        }

        /// <summary>
        /// Hexware.Plist.PlistReal constructor from an <see cref="System.UInt16"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        public PlistReal(ushort value)
        {
            _value = value;
        }

        /// <summary>
        /// Hexware.Plist.PlistReal constructor from an <see cref="System.Int32"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        public PlistReal(int value)
        {
            _value = value;
        }

        /// <summary>
        /// Hexware.Plist.PlistReal constructor from an <see cref="System.UInt32"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        public PlistReal(uint value)
        {
            _value = value;
        }

        /// <summary>
        /// Hexware.Plist.PlistReal constructor from an <see cref="System.Int64"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        public PlistReal(long value)
        {
            _value = value;
        }

        /// <summary>
        /// Hexware.Plist.PlistReal constructor from an <see cref="System.UInt64"/>
        /// </summary>
        /// <param name="value">The value of this node</param>
        public PlistReal(ulong value)
        {
            _value = value;
        }
    }
    public partial class PlistReal
    {
        internal static PlistReal ReadBinary(BinaryReader reader, byte firstbyte)
        {
            firstbyte = (byte)(firstbyte & 0x0F); // Get lower nibble
            int numofbytes = (1 << firstbyte); // how many bytes are contained in this integer
            if (reader.BaseStream.Length < (reader.BaseStream.Position + numofbytes))
            {
                throw new PlistFormatException("Length of element passes end of stream");
            }
            byte[] buf = reader.ReadBytes(numofbytes);
            Array.Reverse(buf);

            if (numofbytes == 4)
                return new PlistReal(BitConverter.ToSingle(buf, 0));
            if (numofbytes == 8)
                return new PlistReal(BitConverter.ToDouble(buf, 0));
            throw new PlistFormatException("Node is not a single (float) or double");
        }

        internal byte[] WriteBinary()
        {
            byte[] tag = new byte[1]
            {
                0x23
            };
            byte[] buf = BitConverter.GetBytes(_value);
            Array.Reverse(buf);
            PlistInternal.Merge(ref tag, ref buf);
            return tag;
        }

        internal static PlistReal ReadXml(XmlDocument reader, int index)
        {
            return new PlistReal(reader.ChildNodes[index].InnerText);
        }

        internal void WriteXml(XmlNode tree, XmlDocument writer)
        {
            XmlElement element = writer.CreateElement("real");
            writer.InnerText = _value.ToString();
            tree.AppendChild(element);
        }
    }
    public partial class PlistReal : IPlistElement<double, Primitive>
    {
        internal double _value;

        /// <summary>
        /// Gets the Xml tag for this element
        /// </summary>
        public string XmlTag
        {
            get
            {
                return "real";
            }
        }

        /// <summary>
        /// Gets or sets the value of this element
        /// </summary>
        public double Value
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
                return Primitive.Real;
            }
        }

        /// <summary>
        /// Gets the length of this element when written in binary mode
        /// </summary>
        /// <returns>Containers return the amount inside while Primitives return the binary length</returns>
        public int GetPlistElementBinaryLength()
        {
            return 9;
        }
    }
}