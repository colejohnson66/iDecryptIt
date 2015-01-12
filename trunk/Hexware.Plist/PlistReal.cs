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
    public partial class PlistReal
    {
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
        public PlistReal(float value)
        {
            _value = value;
        }
        public PlistReal(double value)
        {
            _value = value;
        }
        public PlistReal(sbyte value)
        {
            _value = value;
        }
        public PlistReal(byte value)
        {
            _value = value;
        }
        public PlistReal(short value)
        {
            _value = value;
        }
        public PlistReal(ushort value)
        {
            _value = value;
        }
        public PlistReal(int value)
        {
            _value = value;
        }
        public PlistReal(uint value)
        {
            _value = value;
        }
        public PlistReal(long value)
        {
            _value = value;
        }
        public PlistReal(ulong value)
        {
            _value = value;
        }
    }
    public partial class PlistReal : IPlistElement<double>
    {
        internal double _value;

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
        public PlistElementType ElementType
        {
            get
            {
                return PlistElementType.Real;
            }
        }
    }
    public partial class PlistReal : IPlistElementInternal
    {
        internal static PlistReal ReadBinary(BinaryReader reader, byte firstbyte)
        {
            int numofbytes = 1 << (firstbyte & 0x08);
            byte[] buf = reader.ReadBytes(numofbytes);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buf);

            // The specification uses 3 bits to store the size,
            // but CoreFoundation only supports 32 and 64 bit reals.
            if (numofbytes == 4) // 010
                return new PlistReal(BitConverter.ToSingle(buf, 0));
            if (numofbytes == 8) // 011
                return new PlistReal(BitConverter.ToDouble(buf, 0));

            throw new PlistFormatException("Support does not exist for reals that aren't 32 or 64 bits long");
        }
        void IPlistElementInternal.WriteBinary(BinaryWriter writer)
        {
            // To avoid unintentional loss of precision, save as
            // a 64 bit real. CoreFoundation uses some function
            // called CFNumberGetByteSize to determine what format
            // to use.
            writer.Write((byte)0x23);

            byte[] buf = BitConverter.GetBytes(_value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buf);
            writer.Write(buf);
        }
        internal static PlistReal ReadXml(XmlNode node)
        {
            string val = node.InnerText;
            if (String.Compare(val, "nan", true) == 0)
                return new PlistReal(Double.NaN);
            else if (String.Compare(val, "infinity", true) == 0)
                return new PlistReal(Double.PositiveInfinity);
            else if (String.Compare(val, "+infinity", true) == 0)
                return new PlistReal(Double.PositiveInfinity);
            else if (String.Compare(val, "-infinity", true) == 0)
                return new PlistReal(Double.NegativeInfinity);
            else if (String.Compare(val, "inf", true) == 0)
                return new PlistReal(Double.PositiveInfinity);
            else if (String.Compare(val, "+inf", true) == 0)
                return new PlistReal(Double.PositiveInfinity);
            else if (String.Compare(val, "-inf", true) == 0)
                return new PlistReal(Double.NegativeInfinity);

            return new PlistReal(node.InnerText);
        }
        void IPlistElementInternal.WriteXml(XmlNode tree, XmlDocument writer)
        {
            XmlElement element = writer.CreateElement("real");

            if (Double.IsNaN(_value))
                writer.InnerText = "nan";
            else if (Double.IsPositiveInfinity(_value))
                writer.InnerText = "+infinity";
            else if (Double.IsNegativeInfinity(_value))
                writer.InnerText = "-infinity";
            else
                writer.InnerText = _value.ToString();

            tree.AppendChild(element);
        }
    }
}