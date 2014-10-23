/* =============================================================================
 * File:   PlistArray.cs
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
        /// Hexware.Plist.PlistArray constructor using a one dimensional <see cref="Hexware.Plist.IPlistElement"/> array and a path
        /// </summary>
        /// <param name="value">The value of this node containing <see cref="Hexware.Plist.IPlistElement"/>-based objects</param>
        /// <param name="path">The path of this node in the hierarchy</param>
        /// <param name="parent">A reference to the parent element of this element</param>
        /// <exception cref="System.ArgumentNullException">A parameter is null</exception>
        public PlistArray(IPlistElement[] value, string path, IPlistElement parent)
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
        /// Hexware.Plist.PlistArray constructor using a <see cref="System.Xml.XmlNodeList"/> and a path
        /// </summary>
        /// <param name="value">A <see cref="System.Xml.XmlNodeList"/> containing the current array</param>
        /// <param name="path">The current path in the hierarchy</param>
        /// <param name="parent">A reference to the parent element of this element</param>
        /// <exception cref="Hexware.Plist.PlistFormatException">A node not a valid Plist Element</exception>
        /// <exception cref="System.ArgumentNullException">A parameter is null</exception>
        public PlistArray(XmlNodeList value, string path, IPlistElement parent)
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

            _path = path;
            _parent = parent;

            try
            {
                Parse(value);
            }
            catch (PlistFormatException ex)
            {
                Dispose();
                throw ex;
            }
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
            {
                throw new ArgumentNullException("value", "The provided value is null");
            }

            try
            {
                // Resize
                int length = _value.GetLength(0);
                Array.Resize<IPlistElement>(ref _value, length);
                _value[length] = value;
            }
            catch (OverflowException ex)
            {
                throw new OverflowException("The array is too big", ex);
            }
            catch (StackOverflowException ex)
            {
                throw new StackOverflowException("The stack is too small", ex);
            }
        }

        /// <summary>
        /// Deletes an element from the array based on the index
        /// </summary>
        /// <param name="index">A zero-based index of the element to delete</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="index"/> is negative</exception>
        /// <exception cref="System.IndexOutOfRangeException"><paramref name="index"/> is out of bounds of the array</exception>
        public void Delete(int index)
        {
            if (index < 0)
            {
                throw new ArgumentNullException("index", "The specified index is negative");
            }
            int length = _value.GetLength(0);
            if (length < index)
            {
                throw new IndexOutOfRangeException("Index is outside of bounds of the array");
            }

            if (length == index)
            {
                // Its the last index
                Array.Resize<IPlistElement>(ref _value, length - 1);
                return;
            }

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
            {
                throw new ArgumentNullException("key", "The specified index is negative");
            }

            if (_value.GetLength(0) < index)
            {
                throw new IndexOutOfRangeException("The specified index is out of the bounds of the array");
            }

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
            try
            {
                return (T)Get(index);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw ex;
            }
            catch (InvalidCastException ex)
            {
                throw ex;
            }
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
            {
                throw new ArgumentNullException("key", "The specified index is negative");
            }

            if (_value.GetLength(0) <= index)
            {
                throw new IndexOutOfRangeException("The specified index is out of the bounds of the array");
            }

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
                try
                {
                    return Get(index);
                }
                catch (IndexOutOfRangeException ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Parses a <see cref="System.Xml.XmlNodeList"/> into an array
        /// </summary>
        /// <param name="list">A list of nodes underneath this current node</param>
        internal void Parse(XmlNodeList list)
        {
            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                if (list[i].NodeType != XmlNodeType.Element)
                {
                    list[i].RemoveChild(list[i]);
                }
            }

            length = list.Count;
            _value = new IPlistElement[length];

            for (int i = 0; i < length; i++)
            {
                if (list[i].Name == "dict")
                {
                    _value[i] = new PlistDict(
                        list[i].ChildNodes,
                        _path + "/{ITEM_" + i + "}",
                        this);
                }
                else if (list[i].Name == "array")
                {
                    _value[i] = new PlistArray(
                        list[i].ChildNodes,
                        _path + "/{ITEM_" + i + "}",
                        this);
                }
                else if (list[i].Name == "true")
                {
                    _value[i] = new PlistBool(
                        true,
                        _path + "/{ITEM_" + i + "}",
                        this);
                }
                else if (list[i].Name == "false")
                {
                    _value[i] = new PlistBool(
                        false,
                        _path + "/{ITEM_" + i + "}",
                        this);
                }
                else if (list[i].Name == "data")
                {
                    _value[i] = new PlistData(
                        list[i].InnerText,
                        _path + "/{ITEM_" + i + "}",
                        this);
                }
                else if (list[i].Name == "date")
                {
                    _value[i] = new PlistDate(
                        list[i].InnerText,
                        _path + "/{ITEM_" + i + "}",
                        this);
                }
                else if (list[i].Name == "fill")
                {
                    _value[i] = new PlistFill(
                        _path + "/{ITEM_" + i + "}",
                        this);
                }
                else if (list[i].Name == "integer")
                {
                    _value[i] = new PlistInteger(
                        Convert.ToInt64(list[i].InnerText),
                        _path + "/{ITEM_" + i + "}",
                        this);
                }
                else if (list[i].Name == "null")
                {
                    _value[i] = new PlistNull(
                        _path + "/{ITEM_" + i + "}",
                        this);
                }
                else if (list[i].Name == "real")
                {
                    _value[i] = new PlistReal(
                        Convert.ToDouble(list[i].InnerText),
                        _path + "/{ITEM_" + i + "}",
                        this);
                }
                else if (list[i].Name == "string")
                {
                    _value[i] = new PlistString(
                        list[i].InnerText,
                        _path + "/{ITEM_" + i + "}",
                        this);
                }
                else if (list[i].Name == "uid")
                {
                    _value[i] = new PlistUid(
                        Encoding.ASCII.GetBytes(list[i].InnerText),
                        _path + "/{ITEM_" + i + "}",
                        this);
                }
                else if (list[i].Name == "ustring")
                {
                    _value[i] = new PlistString(
                        list[i].InnerText,
                        _path + "/{ITEM_" + i + "}",
                        this);
                }
                else
                {
                    Dispose();
                    throw new PlistFormatException("Plist element at \"" + _path + "/{ITEM_" + i + "}\" is not a valid element");
                }
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
        internal static PlistArray ReadBinary(BinaryReader reader, byte firstbyte, string path, IPlistElement parent)
        {
            throw new NotImplementedException();
        }

        internal byte[] WriteBinary()
        {
            throw new NotImplementedException();
        }

        internal static PlistArray ReadXml(XmlDocument reader, int index, string path, IPlistElement parent)
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
        private IPlistElement _parent;
        private IPlistElement[] _value;
        private string _path;

        /// <summary>
        /// Gets the Xml tag for this element
        /// </summary>
        public string Tag
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
        public Container ElementType
        {
            get
            {
                return Container.Dict;
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
            throw new NotImplementedException();
        }
    }
    public partial class PlistArray : IDisposable
    {
        private bool _disposed;

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
            if (disposing)
            {
                foreach (IPlistElement item in _value)
                {
                    item.Dispose();
                }
            }

            _path = null;

            _disposed = true;
        }

        /// <summary>
        /// Free up resources used on the system for garbage collector
        /// </summary>
        ~PlistArray()
        {
            Dispose(false);
        }
    }
}
