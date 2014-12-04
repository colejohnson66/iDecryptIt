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
using System.Text;

namespace Hexware.Plist
{
    /// <summary>
    /// Represents a &lt;array /&gt; tag using a one dimensional <see cref="Hexware.Plist.IPlistElement"/> array
    /// </summary>
    public partial class PlistArray
    {
        /// <summary>
        /// Hexware.Plist.PlistArray constructor using a one dimensional <see cref="Hexware.Plist.IPlistElement"/> array
        /// </summary>
        /// <param name="value">The value of this node containing <see cref="Hexware.Plist.IPlistElement"/>-based objects</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is null</exception>
        public PlistArray(IPlistElement[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            _value = value;
        }

        /// <summary>
        /// Hexware.Plist.PlistArray constructor using a <see cref="System.Xml.XmlNodeList"/>
        /// </summary>
        /// <param name="value">A <see cref="System.Xml.XmlNodeList"/> containing the current array</param>
        /// <exception cref="Hexware.Plist.PlistFormatException">A node not a valid Plist Element</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is null</exception>
        public PlistArray(XmlNodeList value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Parse(value);
        }

        /// <summary>
        /// Adds an element to the array
        /// </summary>
        /// <param name="value">The value to add</param>
        /// <exception cref="System.ArgumentNullException">The provided value is null</exception>
        /// <exception cref="System.OverflowException">The array is too big</exception>
        /// <exception cref="System.StackOverflowException">The stack is too small</exception>
        public void Add(IPlistElement value)
        {
            if (value == null)
                throw new ArgumentNullException("value", "The provided value is null");

            int length = _value.GetLength(0);
            Array.Resize(ref _value, length);
            _value[length] = value;
        }

        /// <summary>
        /// Deletes an element from the array based on the index
        /// </summary>
        /// <param name="index">A zero-based index of the element to delete</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="index"/> is negative</exception>
        /// <exception cref="System.IndexOutOfRangeException"><paramref name="index"/> is out of bounds of the array</exception>
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

        /// <summary>
        /// Retrieves an element from the array based on the index
        /// </summary>
        /// <param name="index">A zero-based index of the element to retrieve</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="index"/> is negative</exception>
        /// <exception cref="System.IndexOutOfRangeException"><paramref name="index"/> is out of bounds of the array</exception>
        /// <returns>The value from the array</returns>
        public IPlistElement Get(int index)
        {
            if (index < 0)
                throw new ArgumentNullException("key", "The specified index is negative");
            if (_value.GetLength(0) < index)
                throw new IndexOutOfRangeException("The specified index is out of the bounds of the array");

            return _value[index];
        }

        /// <summary>
        /// Retrieves an element from the array based on the index
        /// </summary>
        /// <typeparam name="T">The type to return of <see cref="Hexware.Plist.IPlistElement"/></typeparam>
        /// <param name="index">A zero-based index of the element to retrieve</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="index"/> is negative</exception>
        /// <exception cref="System.IndexOutOfRangeException"><paramref name="index"/> is out of bounds of the array</exception>
        /// <returns>The value from the array after casting</returns>
        public T Get<T>(int index) where T : IPlistElement
        {
            return (T)Get(index);
        }

        /// <summary>
        /// Sets an existing element in the array with a new value
        /// </summary>
        /// <param name="index">A zero-based index of the element to change</param>
        /// <param name="value">The value to add</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="index"/> is negative</exception>
        /// <exception cref="System.IndexOutOfRangeException"><paramref name="index"/> is out of bounds of the array</exception>
        public void Set(int index, IPlistElement value)
        {
            if (index < 0)
                throw new ArgumentNullException("key", "The specified index is negative");
            if (_value.GetLength(0) <= index)
                throw new IndexOutOfRangeException("The specified index is out of the bounds of the array");

            _value[index] = value;
        }

        /// <summary>
        /// Gets a value in the array
        /// </summary>
        /// <param name="index">A zero-based index of the element to change</param>
        /// <exception cref="System.IndexOutOfRangeException"><paramref name="index"/> is out of bounds of the array</exception>
        /// <returns>The array element at <paramref name="index"/></returns>
        public IPlistElement this[int index]
        {
            get
            {
                return Get(index);
            }
        }

        internal void Parse(XmlNodeList list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].NodeType != XmlNodeType.Element)
                    list[i].RemoveChild(list[i]);
            }

            _value = new IPlistElement[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Name == "array")
                    _value[i] = new PlistArray(list[i].ChildNodes);
                else if (list[i].Name == "true")
                    _value[i] = new PlistBool(true);
                else if (list[i].Name == "false")
                    _value[i] = new PlistBool(false);
                else if (list[i].Name == "data")
                    _value[i] = new PlistData(list[i].InnerText);
                else if (list[i].Name == "date")
                    _value[i] = new PlistDate(list[i].InnerText);
                else if (list[i].Name == "dict")
                    _value[i] = new PlistDict(list[i].ChildNodes);
                else if (list[i].Name == "fill")
                    _value[i] = new PlistFill();
                else if (list[i].Name == "integer")
                    _value[i] = new PlistInteger(list[i].InnerText);
                else if (list[i].Name == "null")
                    _value[i] = new PlistNull();
                else if (list[i].Name == "real")
                    _value[i] = new PlistReal(list[i].InnerText);
                else if (list[i].Name == "string" || list[i].Name == "ustring")
                    _value[i] = new PlistString(list[i].InnerText);
                else if (list[i].Name == "uid")
                    _value[i] = new PlistUid(Encoding.ASCII.GetBytes(list[i].InnerText));
                else
                    throw new PlistFormatException("Plist element is not a valid element");
            }
        }

        /// <summary>
        /// Gets the length of this element
        /// </summary>
        /// <returns>Amount of elements inside</returns>
        public int GetPlistElementLength()
        {
            return _value.Length;
        }
    }
    public partial class PlistArray
    {
        internal static PlistArray ReadBinary(BinaryReader reader, byte firstbyte)
        {
            throw new NotImplementedException();
        }

        internal byte[] WriteBinary()
        {
            throw new NotImplementedException();
        }

        internal static PlistArray ReadXml(XmlDocument reader, int index)
        {
            throw new NotImplementedException();
        }

        internal void WriteXml(XmlNode tree, XmlDocument writer)
        {
            throw new NotImplementedException();
        }
    }
    public partial class PlistArray : IPlistElement<IPlistElement[], Container>
    {
        private IPlistElement[] _value;

        /// <summary>
        /// Gets the Xml tag for this element
        /// </summary>
        public string XmlTag
        {
            get
            {
                return "array";
            }
        }

        /// <summary>
        /// Gets or sets the value of this element
        /// </summary>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is null</exception>
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

        /// <summary>
        /// Gets the type of this element as one of <see cref="Hexware.Plist.Container"/> or <see cref="Hexware.Plist.Primitive"/>
        /// </summary>
        public Container ElementType
        {
            get
            {
                return Container.Dict;
            }
        }

        /// <summary>
        /// Gets the length of this element when written in binary mode
        /// </summary>
        /// <returns>Containers return the amount inside while Primitives return the binary length</returns>
        public int GetPlistElementBinaryLength()
        {
            throw new NotImplementedException();
        }
    }
}