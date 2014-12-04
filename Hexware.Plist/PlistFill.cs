/* =============================================================================
 * File:   PlistFill.cs
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
    /// Represents a &lt;fill /&gt; tag
    /// </summary>
    public partial class PlistFill
    {
        /// <summary>
        /// Hexware.Plist.PlistFill constructor
        /// </summary>
        public PlistFill()
        {
        }
    }
    public partial class PlistFill : IPlistElementInternal
    {
        internal static PlistFill ReadBinary(BinaryReader reader, byte firstbyte)
        {
            return new PlistFill();
        }

        void IPlistElementInternal.WriteBinary(BinaryWriter writer)
        {
            writer.Write((byte)0x0F);
        }

        internal static PlistFill ReadXml(XmlNode node)
        {
            return new PlistFill();
        }

        void IPlistElementInternal.WriteXml(XmlNode tree, XmlDocument writer)
        {
            XmlElement element = writer.CreateElement("fill");
            tree.AppendChild(element);
        }
    }
    public partial class PlistFill : IPlistElement
    {
        /// <summary>
        /// Gets the Xml tag for this element
        /// </summary>
        public string XmlTag
        {
            get
            {
                return "fill";
            }
        }

        /// <summary>
        /// Gets the type of this element
        /// </summary>
        public PlistElementType ElementType
        {
            get
            {
                return PlistElementType.Fill;
            }
        }
    }
}