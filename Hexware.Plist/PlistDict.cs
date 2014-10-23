/* =============================================================================
 * File:   PlistDict.cs
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
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Hexware.Plist
{
    /// <summary>
    /// Represents a &lt;dict /&gt; tag using a <see cref="System.Collections.Generic.Dictionary&lt;TKey, TValue&gt;"/>&lt;<see cref="System.String"/>, <see cref="Hexware.Plist.IPlistElement"/>&gt;
    /// </summary>
    public partial class PlistDict
    {
        /// <summary>
        /// Hexware.Plist.PlistDict constructor using a <see cref="System.Collections.Generic.Dictionary&lt;TKey, TValue&gt;"/>&lt;<see cref="System.String"/>, <see cref="Hexware.Plist.IPlistElement"/>&gt;
        /// </summary>
        /// <param name="value">The value of this node containing <see cref="Hexware.Plist.IPlistElement"/>-based objects</param>
        /// <param name="path">The path of this node in the hierarchy</param>
        /// <param name="parent">A reference to the parent element of this element</param>
        /// <exception cref="System.ArgumentNullException">A parameter is null</exception>
        public PlistDict(Dictionary<string, IPlistElement> value, string path, IPlistElement parent)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (path == null)
            {
                throw new ArgumentNullException("path");
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
        /// <exception cref="System.ArgumentNullException">A parameter is null</exception>
        /// <exception cref="Hexware.Plist.PlistFormatException">A node is invalid</exception>
        public PlistDict(XmlNodeList value, string path, IPlistElement parent)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (path == null)
            {
                throw new ArgumentNullException("path");
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
        /// <param name="key">The key of the value to add</param>
        /// <param name="value">The value to add</param>
        /// <exception cref="System.ArgumentNullException">The provided key or value is null or empty</exception>
        /// <exception cref="System.OverflowException">The array is too big</exception>
        /// <exception cref="System.StackOverflowException">The stack is too small</exception>
        /// <returns>The array after adding</returns>
        public PlistDict Add(string key, IPlistElement value)
        {
            if (key == null || key == "")
            {
                throw new ArgumentNullException("key", "The specified key is null or empty");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value", "The specified value is null");
            }

            if (_value.ContainsKey(key))
            {
                _value[key] = value;
            }
            else
            {
                _value.Add(key, value);
            }

            return this;
        }

        /// <summary>
        /// Deletes an element from the array based on the index
        /// </summary>
        /// <param name="key">A zero-based index of the element to delete</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> is null or empty</exception>
        /// <exception cref="System.MissingFieldException"><paramref name="key"/> does not exist</exception>
        /// <returns>The array after deletion</returns>
        public PlistDict Delete(string key)
        {
            if (key == null || key == "")
            {
                throw new ArgumentNullException("key", "The specified key is null or empty");
            }

            // It no exist...
            if (!_value.ContainsKey(key))
            {
                throw new MissingFieldException("The specified key does not exist");
            }

            _value.Remove(key);

            return this;
        }

        /// <summary>
        /// Determines if a provided key exists in the current dictionary
        /// </summary>
        /// <param name="key">The key to check for</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> is null or empty</exception>
        /// <returns><c>true</c> if the specified key exists; otherwise <c>false</c></returns>
        public bool Exists(string key)
        {
            if (key == null || key == "")
            {
                throw new ArgumentNullException("key", "The specified key is null or empty");
            }

            return _value.ContainsKey(key);
        }

        /// <summary>
        /// Retrieves an element from the array based on a key
        /// </summary>
        /// <param name="key">The key of the element to retrieve</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> is null or empty</exception>
        /// <exception cref="System.IndexOutOfRangeException"><paramref name="key"/> does not exist in the array</exception>
        /// <returns>The value from the array</returns>
        public IPlistElement Get(string key)
        {
            if (key == null || key == "")
            {
                throw new ArgumentNullException("key", "The specified key is null or empty");
            }

            if (!_value.ContainsKey(key))
            {
                throw new IndexOutOfRangeException("The specified key \"" + key + "\" does not exist in the array");
            }

            IPlistElement temp;
            _value.TryGetValue(key, out temp);
            return temp;
        }

        /// <summary>
        /// Retrieves an element from the array based on a key
        /// </summary>
        /// <param name="key">The key of the element to retrieve</param>
        /// <param name="throw">Whether to throw an exception of missing field or not</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> is null or empty</exception>
        /// <exception cref="System.IndexOutOfRangeException"><paramref name="key"/> does not exist in the array</exception>
        /// <returns>The value from the array or <c>null</c> if <paramref name="key"/> does not exist</returns>
        public IPlistElement Get(string key, bool @throw)
        {
            if (key == null || key == "")
            {
                throw new ArgumentNullException("key", "The specified key is null or empty");
            }

            if (!_value.ContainsKey(key))
            {
                if (@throw)
                {
                    throw new IndexOutOfRangeException("The specified key \"" + key + "\" does not exist in the array");
                }
                return null;
            }

            IPlistElement temp;
            _value.TryGetValue(key, out temp);
            return temp;
        }

        /// <summary>
        /// Retrieves an element from the array based on a key
        /// </summary>
        /// <typeparam name="T">The type to return of <see cref="Hexware.Plist.IPlistElement"/></typeparam>
        /// <param name="key">The key of the element to retrieve</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> is null or empty</exception>
        /// <exception cref="System.IndexOutOfRangeException"><paramref name="key"/> does not exist in the array</exception>
        /// <returns>The value from the array after casting</returns>
        public T Get<T>(string key) where T : IPlistElement
        {
            try
            {
                return (T)Get(key);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new IndexOutOfRangeException(ex.Message, ex);
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidCastException("Unable to cast array element", ex);
            }
        }

        /// <summary>
        /// Retrieves an element from the array based on a key
        /// </summary>
        /// <typeparam name="T">The type to return of <see cref="Hexware.Plist.IPlistElement"/></typeparam>
        /// <param name="key">The key of the element to retrieve</param>
        /// <param name="throw">Whether to throw an exception of missing field or not</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> is null or empty</exception>
        /// <exception cref="System.IndexOutOfRangeException"><paramref name="key"/> does not exist in the array</exception>
        /// <returns>The value from the array after casting or <c>null</c> if <paramref name="key"/> does not exist</returns>
        public T Get<T>(string key, bool @throw) where T : IPlistElement
        {
            try
            {
                return (T)Get(key, @throw);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new IndexOutOfRangeException(ex.Message, ex);
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidCastException("Unable to cast array element", ex);
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
            if (length % 2 == 1)
            {
                throw new PlistFormatException("Plist dictionary at \"" + _path + "\" is not valid");
            }

            string key = "";
            _value = new Dictionary<string, IPlistElement>(length);

            for (int i = 0; i < length; i++)
            {
                if (i % 2 == 0)
                {
                    if (list[i].Name != "key" ||
                        list[i].InnerXml.Contains("<") ||
                        list[i].InnerXml.Contains(">"))
                    {
                        throw new PlistFormatException("Plist element at \"" + _path + "{ITEM_" + i + "}\" is not a key");
                    }
                    key = list[i].InnerText;
                }
                else
                {
                    if (list[i].Name == "dict")
                    {
                        _value.Add(key, new PlistDict(
                            list[i].ChildNodes,
                            _path + "/" + key,
                            this));
                    }
                    else if (list[i].Name == "array")
                    {
                        _value.Add(key, new PlistArray(
                           list[i].ChildNodes,
                           _path + "/" + key,
                            this));
                    }
                    else if (list[i].Name == "true")
                    {
                        _value.Add(key, new PlistBool(
                            true,
                            _path + "/" + key,
                            this));
                    }
                    else if (list[i].Name == "false")
                    {
                        _value.Add(key, new PlistBool(
                            false,
                            _path + "/" + key,
                            this));
                    }
                    else if (list[i].Name == "data")
                    {
                        _value.Add(key, new PlistData(
                            list[i].InnerText,
                            _path + "/" + key,
                            this));
                    }
                    else if (list[i].Name == "date")
                    {
                        _value.Add(key, new PlistDate(
                            list[i].InnerText,
                            _path + "/" + key,
                            this));
                    }
                    else if (list[i].Name == "fill")
                    {
                        _value.Add(key, new PlistFill(
                            _path + "/" + key,
                            this));
                    }
                    else if (list[i].Name == "integer")
                    {
                        _value.Add(key, new PlistInteger(
                            Convert.ToInt64(list[i].InnerText),
                            _path + "/" + key,
                            this));
                    }
                    else if (list[i].Name == "null")
                    {
                        _value.Add(key, new PlistNull(
                            _path + "/" + key,
                            this));
                    }
                    else if (list[i].Name == "real")
                    {
                        _value.Add(key, new PlistReal(
                            Convert.ToDouble(list[i].InnerText),
                            _path + "/" + key,
                            this));
                    }
                    else if (list[i].Name == "string")
                    {
                        _value.Add(key, new PlistString(
                            list[i].InnerText,
                            _path + "/" + key,
                            this));
                    }
                    else if (list[i].Name == "ustring")
                    {
                        _value.Add(key, new PlistString(
                            list[i].InnerText,
                            _path + "/" + key,
                            this));
                    }
                    else
                    {
                        throw new PlistFormatException("Plist element at \"" + _path + "{ITEM_" + i + "}\" is not a valid element");
                    }
                }
            }
        }

        /// <summary>
        /// Gets a <see cref="System.Collections.Generic.ICollection&lt;T&gt;"/>&lt;<see cref="System.String"/>&gt; collection of the keys
        /// </summary>
        public ICollection<string> Keys
        {
            get
            {
                return _value.Keys;
            }
        }

        /// <summary>
        /// Gets a <see cref="System.Collections.Generic.ICollection&lt;T&gt;"/>&lt;<see cref="Hexware.Plist.IPlistElement"/>&gt; collection of the values
        /// </summary>
        public ICollection<IPlistElement> Values
        {
            get
            {
                return _value.Values;
            }
        }

        /// <summary>
        /// Gets or sets a value in this element
        /// </summary>
        /// <param name="key">The key for the element to retrieve or set</param>
        /// <exception cref="System.ArgumentNullException">A value is null</exception>
        /// <exception cref="System.IndexOutOfRangeException"><paramref name="key"/> does not exist in the array</exception>
        /// <returns>The value at the specified key</returns>
        public IPlistElement this[string key]
        {
            get
            {
                if (key == null || key == "")
                {
                    throw new ArgumentNullException("key", "The specified key is null or empty");
                }

                if (!_value.ContainsKey(key))
                {
                    throw new IndexOutOfRangeException("The specified key \"" + key + "\" does not exist in the array");
                }

                IPlistElement temp;
                _value.TryGetValue(key, out temp);
                return temp;
            }
            set
            {
                if (key == null || key == "")
                {
                    throw new ArgumentNullException("key", "The specified key is null or empty");
                }

                if (value == null)
                {
                    throw new ArgumentNullException("value", "The specified value is null");
                }

                if (_value.ContainsKey(key))
                {
                    _value[key] = value;
                }
                else
                {
                    _value.Add(key, value);
                }
            }
        }

        /// <summary>
        /// Gets the number of nodes underneath this element
        /// </summary>
        /// <returns>Amount of elements inside</returns>
        public int GetPlistElementLength()
        {
            return _value.Count;
        }
    }
    public partial class PlistDict
    {
        /// <summary>
        /// Read a binary Plist node into a .NET Plist element
        /// </summary>
        /// <param name="reader">The <see cref="System.IO.BinaryReader"/> that is responsible for deserializing the binary Plist</param>
        /// <param name="firstbyte">The first byte of this element as it was extracted from the reader stream</param>
        /// <param name="path">The path of this node</param>
        /// <param name="parent">A reference to the parent element of this element</param>
        /// <returns>The <see cref="Hexware.Plist.PlistDict"/> element from the binary stream</returns>
        internal static PlistDict ReadBinary(BinaryReader reader, byte firstbyte, string path, IPlistElement parent)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Write the Plist node to a <see cref="System.IO.BinaryWriter"/> for serializing the Plist as a Binary Plist
        /// </summary>
        /// <returns>An array of <see cref="System.Byte"/> that contains the data for this element</returns>
        internal byte[] WriteBinary()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Read an Xml Plist node into a .NET Plist element
        /// </summary>
        /// <param name="reader">The <see cref="System.Xml.XmlDocument"/> that is responsible for deserialization</param>
        /// <param name="index">The index of the element number of the <paramref name="reader"/> for this node</param>
        /// <param name="path">The path of this node</param>
        /// <param name="parent">A reference to the parent element of this element</param>
        /// <returns>The <see cref="Hexware.Plist.PlistDict"/> element from the Xml stream</returns>
        internal static PlistDict ReadXml(XmlDocument reader, int index, string path, IPlistElement parent)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Write the Plist node to an <see cref="System.Xml.XmlDocument"/> for serializing the Plist as an Xml Plist
        /// </summary>
        /// <param name="tree">The current node that the new element will be added to</param>
        /// <param name="writer">A <see cref="System.Xml.XmlDocument"/> that is used to create <see cref="System.Xml.XmlNode"/></param>
        /// <returns>An <see cref="System.Xml.XmlElement"/> that contains the value of this element</returns>
        internal void WriteXml(XmlNode tree, XmlDocument writer)
        {
            throw new NotImplementedException();
        }
    }
    public partial class PlistDict : IPlistElement<Dictionary<string, IPlistElement>, Container>
    {
        internal IPlistElement _parent;
        internal Dictionary<string, IPlistElement> _value;
        internal string _path;

        /// <summary>
        /// Gets the Xml tag for this element
        /// </summary>
        public string Tag
        {
            get
            {
                return "dict";
            }
        }

        /// <summary>
        /// Gets or sets the value of this element
        /// </summary>
        /// <param name="value">The value of this node</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is null</exception>
        public Dictionary<string, IPlistElement> Value
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
    public partial class PlistDict : IDisposable
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
            if (disposing)
            {
                foreach (IPlistElement item in _value.Values)
                {
                    item.Dispose();
                }
            }

            _value.Clear();
            _value = null;
            _path = null;

            _disposed = true;
        }

        /// <summary>
        /// Free up resources used on the system for garbage collector
        /// </summary>
        ~PlistDict()
        {
            Dispose(false);
        }
    }
}
