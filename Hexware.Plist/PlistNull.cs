/* =============================================================================
 * File:   PlistNull.cs
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
using System.IO;
using System.Xml;

namespace Hexware.Plist
{
    /// <summary>
    /// Represents a &lt;null /&gt; tag
    /// </summary>
    public partial class PlistNull
    {
        /// <summary>
        /// Hexware.Plist.PlistNull constructor
        /// </summary>
        public PlistNull()
        {
        }
    }
    public partial class PlistNull
    {
        internal static PlistNull ReadBinary(BinaryReader reader, byte firstbyte)
        {
            return new PlistNull();
        }

        internal byte[] WriteBinary()
        {
            return new byte[]
            {
                0x00
            };
        }

        internal static PlistNull ReadXml(XmlDocument reader, int index)
        {
            return new PlistNull();
        }

        internal void WriteXml(XmlNode tree, XmlDocument writer)
        {
            XmlElement element = writer.CreateElement("null");
            tree.AppendChild(element);
        }
    }
    public partial class PlistNull : IPlistElement
    {
        /// <summary>
        /// Gets the Xml tag for this element
        /// </summary>
        public string XmlTag
        {
            get
            {
                return "null";
            }
        }

        /// <summary>
        /// Gets the type of this element as one of <see cref="Hexware.Plist.Container"/> or <see cref="Hexware.Plist.Primitive"/>
        /// </summary>
        public Primitive ElementType
        {
            get
            {
                return Primitive.Null;
            }
        }

        /// <summary>
        /// Gets the length of this element when written in binary mode
        /// </summary>
        /// <returns>Containers return the amount inside while Primitives return the binary length</returns>
        public int GetPlistElementBinaryLength()
        {
            return 1;
        }
    }
}