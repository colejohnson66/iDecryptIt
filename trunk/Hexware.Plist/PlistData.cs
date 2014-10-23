﻿/* =============================================================================
 * File:   PlistData.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2012 Cole Johnson
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
        /// Hexware.Plist.PlistData constructor using a one dimensional <see cref="System.Byte"/> array and a path
        /// </summary>
        /// <param name="value">The value of this node</param>
        /// <param name="path">The path of this node in the hierarchy</param>
        /// <param name="parent">A reference to the parent element of this element</param>
        /// <exception cref="System.ArgumentNullException">A parameter is null</exception>
        public PlistData(byte[] value, string path, IPlistElement parent)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            _value = value;
            _path = path;
            _parent = parent;
        }

        /// <summary>
        /// Hexware.Plist.PlistData constructor using a base64 encoded <see cref="System.String"/> and a path
        /// </summary>
        /// <param name="value">The value of this node</param>
        /// <param name="path">The path of this node in the hierarchy</param>
        /// <param name="parent">A reference to the parent element of this element</param>
        /// <exception cref="System.ArgumentNullException">A parameter is null</exception>
        /// <exception cref="System.FormatException"><paramref name="value"/> is not a valid base64 encoded string</exception>
        public PlistData(string value, string path, IPlistElement parent)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            value = value
                .Replace("\n", "")
                .Replace("\r", "")
                .Replace("\t", "")
                .Replace(" ", "");

            try
            {
                _value = Convert.FromBase64String(value);
                _path = path;
                _parent = parent;
            }
            catch (FormatException)
            {
                Dispose();
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
        /// <summary>
        /// Read a binary Plist node into a .NET Plist element
        /// </summary>
        /// <param name="reader">The <see cref="System.IO.BinaryReader"/> that is responsible for deserializing the binary Plist</param>
        /// <param name="firstbyte">The first byte of this element as it was extracted from the reader stream</param>
        /// <param name="path">The path of this node</param>
        /// <param name="parent">A reference to the parent element of this element</param>
        /// <exception cref="Hexware.Plist.PlistException">Length of string exceeds length of signed integer</exception>
        /// <exception cref="Hexware.Plist.PlistFormatException">Length of element passes end of stream</exception>
        /// <returns>The <see cref="Hexware.Plist.PlistData"/> element from the binary stream</returns>
        internal static PlistData ReadBinary(BinaryReader reader, byte firstbyte, string path, IPlistElement parent)
        {
            byte uppernibble = (byte)((firstbyte & 0xF0) >> 4); // Get upper nibble
            byte lowernibble = (byte)(firstbyte & 0x0F); // Get lower nibble
            int length = lowernibble;
            if (lowernibble == 0x0F)
            {
                try
                {
                    length = (int)PlistInteger.ReadBinary(reader, reader.ReadByte(), "/", parent).Value;
                }
                catch (PlistFormatException ex)
                {
                    throw ex;
                }
                catch (Exception)
                {
                    throw new PlistException("Length of string exceeds length of signed integer");
                }
            }
            if (reader.BaseStream.Length < (reader.BaseStream.Position + length))
            {
                throw new PlistFormatException("Length of element passes end of stream");
            }
            return new PlistData(reader.ReadBytes(length), path, parent);
        }

        /// <summary>
        /// Write the Plist node to a <see cref="System.IO.BinaryWriter"/> for serializing the Plist as a Binary Plist
        /// </summary>
        /// <returns>An array of <see cref="System.Byte"/> that contains the data for this element</returns>
        internal byte[] WriteBinary()
        {
            byte[] tag = new byte[0];
            byte[] buf = _value;
            if (_value.Length > 0x0D)
            {
                tag = new byte[1]
                {
                    0x0F
                };
                byte[] temp = new PlistInteger(buf.Length, "/", _parent).WriteBinary();
                PlistInternal.Merge<byte>(ref tag, ref temp);
            }
            else
            {
                tag = new byte[2]
                {
                    0x0F,
                    (byte)buf.Length
                };
            }
            PlistInternal.Merge<byte>(ref tag, ref buf);
            return tag;
        }

        /// <summary>
        /// Read an Xml Plist node into a .NET Plist element
        /// </summary>
        /// <param name="reader">The <see cref="System.Xml.XmlDocument"/> that is responsible for deserialization</param>
        /// <param name="index">The index of the element number of the <paramref name="reader"/> for this node</param>
        /// <param name="path">The path of this node</param>
        /// <param name="parent">A reference to the parent element of this element</param>
        /// <exception cref="System.FormatException">Value is not a valid base64 encoded string</exception>
        /// <returns>The <see cref="Hexware.Plist.PlistData"/> element from the Xml stream</returns>
        internal static PlistData ReadXml(XmlDocument reader, int index, string path, IPlistElement parent)
        {
            string buf = reader.ChildNodes[index].InnerText
                .Replace("\n", "")
                .Replace("\r", "")
                .Replace("\t", "")
                .Replace(" ", "");

            try
            {
                return new PlistData(Convert.FromBase64String(buf), path, parent);
            }
            catch (FormatException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Write the Plist node to an <see cref="System.Xml.XmlDocument"/> for serializing the Plist as an Xml Plist
        /// </summary>
        /// <param name="tree">The current node that the new element will be added to</param>
        /// <param name="writer">A <see cref="System.Xml.XmlDocument"/> that is used to create <see cref="System.Xml.XmlNode"/></param>
        /// <returns>An <see cref="System.Xml.XmlElement"/> that contains the value of this element</returns>
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
        internal IPlistElement _parent;
        internal byte[] _value;
        internal string _path;

        /// <summary>
        /// Gets the Xml tag for this element
        /// </summary>
        public string Tag
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
                {
                    throw new ArgumentNullException("value");
                }
                _value = value;
            }
        }

        /// <summary>
        /// Gets the path of this element
        /// </summary>
        public string Path
        {
            get
            {
                return _path;
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
        /// Gets the parent of this element
        /// </summary>
        public IPlistElement Parent
        {
            get
            {
                return _parent;
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
    public partial class PlistData : IDisposable
    {
        internal bool _disposed;

        /// <summary>
        /// Free up resources used on the system for garbage collector
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Free up resources used on the system for garbage collector
        /// </summary>
        /// <param name="disposing"><c>true</c> if called from .Dispose() or else <c>false</c></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            // dispose all managed resources
            //if (disposing)
            //{
            //}

            _value = null;
            _path = null;

            _disposed = true;
        }

        /// <summary>
        /// Free up resources used on the system for garbage collector
        /// </summary>
        ~PlistData()
        {
            Dispose(false);
        }
    }
}
