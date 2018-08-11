/* =============================================================================
 * File:   PlistBool.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2012, 2014-2016 Cole Johnson
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
using System.Xml;

namespace Hexware.Plist
{
    public partial class PlistBool : IPlistElement
    {
        internal bool _value;

        public PlistBool(bool value)
        {
            _value = value;
        }
        public static implicit operator PlistBool(bool value)
        {
            return new PlistBool(value);
        }

        public bool Value
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
                return PlistElementType.Boolean;
            }
        }

        public override bool Equals(object obj)
        {
            PlistBool other = obj as PlistBool;
            if (other == null)
                return false;

            return (_value == other._value);
        }
    }
    public partial class PlistBool : IPlistElementInternal
    {
        void IPlistElementInternal.WriteBinary(BinaryPlistWriter writer)
        {
            writer.Write((byte)(_value ? 0x09 : 0x08));
        }
        void IPlistElementInternal.WriteXml(XmlNode tree, XmlDocument writer)
        {
            XmlElement element = writer.CreateElement(_value ? "true" : "false");
            tree.AppendChild(element);
        }
    }
}