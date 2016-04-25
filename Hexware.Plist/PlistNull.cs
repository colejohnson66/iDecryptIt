/* =============================================================================
 * File:   PlistNull.cs
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
    public partial class PlistNull : IPlistElement
    {
        public PlistNull()
        {
        }

        public bool CanSerialize(PlistDocumentType type)
        {
            return true;
        }
        public PlistElementType ElementType
        {
            get
            {
                return PlistElementType.Null;
            }
        }

        public override bool Equals(object obj)
        {
            PlistNull other = obj as PlistNull;
            if (other == null)
                return false;

            return true;
        }
    }
    public partial class PlistNull : IPlistElementInternal
    {
        void IPlistElementInternal.WriteBinary(BinaryPlistWriter writer)
        {
            writer.Write((byte)0x00);
        }
        void IPlistElementInternal.WriteXml(XmlNode tree, XmlDocument writer)
        {
            XmlElement element = writer.CreateElement("string");
            element.InnerText = "CF$null";
            tree.AppendChild(element);
        }
    }
}