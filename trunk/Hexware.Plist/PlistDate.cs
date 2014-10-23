/* =============================================================================
 * File:   PlistDate.cs
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
using System.Globalization;
using System.IO;
using System.Xml;

namespace Hexware.Plist
{
    /// <summary>
    /// Represents a &lt;date /&gt; tag using a <see cref="System.DateTime"/>
    /// </summary>
    public partial class PlistDate
    {
        /// <summary>
        /// Hexware.Plist.PlistDate constructor using a <see cref="System.DateTime"/> and a path
        /// </summary>
        /// <param name="value">The value of this node</param>
        /// <param name="path">The path of this node in the hierarchy</param>
        /// <param name="parent">A reference to the parent element of this element</param>
        /// <exception cref="System.ArgumentNullException">A parameter is null</exception>
        public PlistDate(DateTime value, string path, IPlistElement parent)
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
        /// Hexware.Plist.PlistDate constructor using a <see cref="System.String"/> and a path
        /// </summary>
        /// <param name="value">The value of this node</param>
        /// <param name="path">The path of this node in the hierarchy</param>
        /// <param name="parent">A reference to the parent element of this element</param>
        /// <exception cref="System.ArgumentNullException">A parameter is null</exception>
        /// <exception cref="System.FormatException">Provided date is not in valid ISO 8601 standard</exception>
        public PlistDate(string value, string path, IPlistElement parent)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            try
            {
                _value = DateTime.Parse(value, null, DateTimeStyles.AdjustToUniversal);
            }
            catch (FormatException ex)
            {
                Dispose();
                throw new FormatException("Provided date is not in valid ISO 8601 standard", ex);
            }
            _path = path;
            _parent = parent;
        }
    }
    public partial class PlistDate
    {
        /// <summary>
        /// Read a binary Plist node into a .NET Plist element
        /// </summary>
        /// <param name="reader">The <see cref="System.IO.BinaryReader"/> that is responsible for deserializing the binary Plist</param>
        /// <param name="firstbyte">The first byte of this element as it was extracted from the reader stream</param>
        /// <param name="path">The path of this node</param>
        /// <param name="parent">A reference to the parent element of this element</param>
        /// <exception cref="Hexware.Plist.PlistFormatException">Node is not a single (float) or double or length of element passes end of stream</exception>
        /// <returns>The <see cref="Hexware.Plist.PlistDate"/> element from the binary stream</returns>
        internal static PlistDate ReadBinary(BinaryReader reader, byte firstbyte, string path, IPlistElement parent)
        {
            firstbyte = (byte)(firstbyte & 0x0F); // get lower nibble
            int numofbytes = (1 << firstbyte); // how many bytes are contained in this date
            if (reader.BaseStream.Length < (reader.BaseStream.Position + numofbytes))
            {
                throw new PlistFormatException("Length of element passes end of stream");
            }
            byte[] buf = reader.ReadBytes(numofbytes);
            Array.Reverse(buf);
            DateTime epoch = new DateTime(2011, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            if (firstbyte == 0x02)
            {
                return new PlistDate(
                    epoch.AddTicks((long)BitConverter.ToSingle(buf, 0)),
                    path,
                    parent);
            }
            if (firstbyte == 0x03)
            {
                return new PlistDate(
                    epoch.AddTicks((long)BitConverter.ToDouble(buf, 0)),
                    path,
                    parent);
            }
            throw new PlistFormatException("Node is not a single (float) or double");
        }

        /// <summary>
        /// Write the Plist node to a <see cref="System.IO.BinaryWriter"/> for serializing the Plist as a Binary Plist
        /// </summary>
        /// <returns>An array of <see cref="System.Byte"/> that contains the data for this element</returns>
        internal byte[] WriteBinary()
        {
            // Epoch on Mac is 2011-01-01:00-00-00
            DateTime start = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan ts = _value - start;
            
            byte[] tag = new byte[1]
            {
                // System.TimeSpan equates to System.Double
                0x33
            };
            byte[] buf = BitConverter.GetBytes(ts.TotalSeconds);
            Array.Reverse(buf);
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
        /// <exception cref="System.FormatException">Provided date is not in valid ISO 8601 standard</exception>
        /// <returns>The <see cref="Hexware.Plist.PlistDate"/> element from the Xml stream</returns>
        internal static PlistDate ReadXml(XmlDocument reader, int index, string path, IPlistElement parent)
        {
            try
            {
                return new PlistDate(
                    DateTime.Parse(
                        reader.ChildNodes[index].InnerText,
                        null,
                        DateTimeStyles.AdjustToUniversal),
                    path,
                    parent);
            }
            catch (FormatException)
            {
                throw new FormatException("Provided date is not in valid ISO 8601 standard");
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
            XmlElement element = writer.CreateElement("date");
            element.InnerText = _value.ToString("s") + "Z";
            tree.AppendChild(element);
        }
    }
    public partial class PlistDate : IPlistElement<DateTime, Primitive>
    {
        internal IPlistElement _parent;
        internal DateTime _value;
        internal string _path;

        /// <summary>
        /// Gets the Xml tag for this element
        /// </summary>
        public string Tag
        {
            get
            {
                return "date";
            }
        }

        /// <summary>
        /// Gets or sets the value of this element
        /// </summary>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is null</exception>
        public DateTime Value
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
        /// Gets or sets the path of this element
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
                return Primitive.Date;
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
    public partial class PlistDate : IDisposable
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

            //_value = DateTime.;
            _path = null;

            _disposed = true;
        }

        /// <summary>
        /// Free up resources used on the system for garbage collector
        /// </summary>
        ~PlistDate()
        {
            Dispose(false);
        }
    }
}
