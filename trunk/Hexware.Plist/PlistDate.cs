/* =============================================================================
 * File:   PlistDate.cs
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
using System.Globalization;
using System.Xml;

namespace Hexware.Plist
{
    public partial class PlistDate : IPlistElement
    {
        internal static DateTime AppleEpoch = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        internal DateTime _value;

        public PlistDate(DateTime value)
        {
            _value = value;
        }
        public PlistDate(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            try
            {
                _value = DateTime.Parse(value, null, DateTimeStyles.AdjustToUniversal);
            }
            catch (FormatException)
            {
                throw new FormatException("Provided date is not in valid ISO 8601 standard");
            }
        }
        public static implicit operator PlistDate(DateTime value)
        {
            return new PlistDate(value);
        }
        public static explicit operator PlistDate(string value)
        {
            return new PlistDate(value);
        }

        public DateTime Value
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
            return true;
        }
        public PlistElementType ElementType
        {
            get
            {
                return PlistElementType.Date;
            }
        }

        public override bool Equals(object obj)
        {
            PlistDate other = obj as PlistDate;
            if (other == null)
                return false;

            return (_value == other._value);
        }
    }
    public partial class PlistDate : IPlistElementInternal
    {
        internal static PlistDate ReadBinary(BinaryPlistReader reader, byte firstbyte)
        {
            byte[] buf = reader.ReadBytes(8);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buf);
            return new PlistDate(AppleEpoch.AddTicks((long)BitConverter.ToDouble(buf, 0)));
        }
        void IPlistElementInternal.WriteBinary(BinaryPlistWriter writer)
        {
            writer.Write((byte)0x33);

            // could be optimized by writing the array backwards instead of reversing first
            TimeSpan val = _value - AppleEpoch;
            byte[] buf = BitConverter.GetBytes(val.TotalSeconds);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buf);
            writer.Write(buf);
        }
        internal static PlistDate ReadXml(XmlNode node)
        {
            return new PlistDate(node.InnerText);
        }
        void IPlistElementInternal.WriteXml(XmlNode tree, XmlDocument writer)
        {
            XmlElement element = writer.CreateElement("date");
            element.InnerText = _value.ToString("s") + "Z";
            tree.AppendChild(element);
        }
    }
}