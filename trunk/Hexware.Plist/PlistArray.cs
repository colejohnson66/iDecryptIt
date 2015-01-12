/* =============================================================================
 * File:   PlistArray.cs
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
    public partial class PlistArray
    {
        public PlistArray(IPlistElement[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            _value = (IPlistElement[])value.Clone();
        }
        internal PlistArray(IPlistElement[] value, bool unused)
        {
            // Used by PlistArray.ReadXml to avoid shallow copy
            _value = value;
        }

        public void Add(IPlistElement value)
        {
            if (value == null)
                throw new ArgumentNullException("value", "The provided value is null");

            int length = _value.GetLength(0);
            Array.Resize(ref _value, length);
            _value[length] = value;
        }
        public void Delete(int index)
        {
            int length = _value.GetLength(0);
            if (index < 0)
                throw new ArgumentNullException("index", "The specified index is negative");
            if (length < index)
                throw new IndexOutOfRangeException("Index is outside of bounds of the array");

            if (length == index)
            {
                // It's the last index
                Array.Resize(ref _value, length - 1);
                return;
            }

            // TODO: Instead, pack down array ignoring _value[index], then resize

            // Resize
            bool reached = false;
            IPlistElement[] resize = new IPlistElement[length - 1];
            length--;

            // Remove unwanted
            for (int i = 0; i < length; i++)
            {
                if (i == index)
                {
                    reached = true;
                }
                if (reached)
                {
                    resize[i] = _value[i];
                }
                else
                {
                    resize[i] = _value[i + 1];
                }
            }

            _value = resize;
        }
        public IPlistElement Get(int index)
        {
            if (index < 0)
                throw new ArgumentNullException("key", "The specified index is negative");
            if (_value.GetLength(0) < index)
                throw new IndexOutOfRangeException("The specified index is out of the bounds of the array");

            return _value[index];
        }
        public T Get<T>(int index) where T : IPlistElement
        {
            return (T)Get(index);
        }
        public void Set(int index, IPlistElement value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (index < 0)
                throw new IndexOutOfRangeException("The specified index is negative");
            if (_value.GetLength(0) <= index)
                throw new IndexOutOfRangeException("The specified index is out of the bounds of the array");

            _value[index] = value;
        }
        public IPlistElement this[int index]
        {
            get
            {
                return Get(index);
            }
        }
        public int Length
        {
            get
            {
                return _value.Length;
            }
        }
    }
    public partial class PlistArray : IPlistElement<IPlistElement[]>
    {
        private IPlistElement[] _value;

        public IPlistElement[] Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _value = value;
            }
        }
        public PlistElementType ElementType
        {
            get
            {
                return PlistElementType.Dictionary;
            }
        }
    }
    public partial class PlistArray : IPlistElementInternal
    {
        // TODO
        internal static PlistArray ReadBinary(BinaryReader reader, byte firstbyte)
        {
            int length = firstbyte & 0x0F;
            if (length == 0x0F) {
                length = (int)PlistInteger.ReadBinary(reader, reader.ReadByte()).Value;
            }
            // TODO: objref*
            throw new NotImplementedException();
        }
        // TODO
        public void WriteBinary(BinaryWriter writer)
        {
            if (_value.Length < 0x0F) {
                writer.Write((byte)(0xA0 | _value.Length));
            } else {
                writer.Write((byte)0xAF);
                ((IPlistElementInternal)new PlistInteger(_value.Length)).WriteBinary(writer);
            }
            // TODO: objref*
            throw new NotImplementedException();
        }
        internal static PlistArray ReadXml(XmlNode node)
        {
            XmlNodeList children = node.ChildNodes;
            IPlistElement[] ret = new IPlistElement[children.Count];

            for (int i = 0; i < children.Count; i++) {
                XmlNode value = children[i];
                if (value.Name == "array")
                    ret[i] = PlistArray.ReadXml(value);
                else if (value.Name == "true" || value.Name == "false")
                    ret[i] = PlistBool.ReadXml(value);
                else if (value.Name == "data")
                    ret[i] = PlistData.ReadXml(value);
                else if (value.Name == "date")
                    ret[i] = PlistDate.ReadXml(value);
                else if (value.Name == "dict")
                    ret[i] = PlistDict.ReadXml(value);
                else if (value.Name == "integer")
                    ret[i] = PlistInteger.ReadXml(value);
                else if (value.Name == "null")
                    ret[i] = PlistNull.ReadXml(value);
                else if (value.Name == "real")
                    ret[i] = PlistReal.ReadXml(value);
                else if (value.Name == "string")
                    ret[i] = PlistString.ReadXml(value);
                else
                    throw new PlistFormatException("Plist element is not a valid element");
            }
            return new PlistArray(ret, false);
        }
        void IPlistElementInternal.WriteXml(XmlNode tree, XmlDocument writer)
        {
            XmlElement element;
            element = writer.CreateElement("array");

            for (int i = 0; i < _value.Length; i++) {
                ((IPlistElementInternal)_value[i]).WriteXml(element, writer);
            }

            tree.AppendChild(element);
        }
    }
}