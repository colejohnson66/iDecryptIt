/* =============================================================================
 * File:   PlistDict.cs
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
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Hexware.Plist
{
    /// <summary>
    /// Represents a &lt;dict /&gt; tag using a <see cref="System.Collections.Generic.Dictionary{System.String, Hexware.Plist.IPlistElement}"/>
    /// </summary>
    public partial class PlistDict
    {
        /// <summary>
        /// Hexware.Plist.PlistDict constructor using a <see cref="System.Collections.Generic.Dictionary&lt;TKey, TValue&gt;"/>&lt;<see cref="System.String"/>, <see cref="Hexware.Plist.IPlistElement"/>&gt;
        /// </summary>
        /// <param name="value">The value of this node containing <see cref="Hexware.Plist.IPlistElement"/> objects</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is null</exception>
        public PlistDict(Dictionary<string, IPlistElement> value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            _value = value;
        }

        /// <summary>
        /// Hexware.Plist.PlistArray constructor using a <see cref="System.Xml.XmlNodeList"/>
        /// </summary>
        /// <param name="value">A <see cref="System.Xml.XmlNodeList"/> containing the current array</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is null</exception>
        /// <exception cref="Hexware.Plist.PlistFormatException">A node is invalid</exception>
        public PlistDict(XmlNodeList value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (value.Count % 2 == 1)
                throw new PlistFormatException("Plist dictionary is not valid");

            _value = new Dictionary<string, IPlistElement>(value.Count);
            for (int i = 0; i < value.Count; i = i + 2) {
                string key = value[i].InnerText;
                string valueType = value[i + 1].Name;

                if (value[i].Name != "key" ||
                    key.Contains("<") ||
                    key.Contains(">")) {
                    throw new PlistFormatException("\"" + value[i].InnerXml + "\" is not a valid Plist key");
                }

                if (valueType == "array")
                    _value.Add(key, PlistArray.ReadXml(value[i + 1]));
                else if (valueType == "true" || valueType == "false")
                    _value.Add(key, PlistBool.ReadXml(value[i + 1]));
                else if (valueType == "data")
                    _value.Add(key, PlistData.ReadXml(value[i + 1]));
                else if (valueType == "date")
                    _value.Add(key, PlistDate.ReadXml(value[i + 1]));
                else if (valueType == "dict")
                    _value.Add(key, PlistDict.ReadXml(value[i + 1]));
                else if (valueType == "fill")
                    _value.Add(key, PlistFill.ReadXml(value[i + 1]));
                else if (valueType == "integer")
                    _value.Add(key, PlistInteger.ReadXml(value[i + 1]));
                else if (valueType == "null")
                    _value.Add(key, PlistNull.ReadXml(value[i + 1]));
                else if (valueType == "real")
                    _value.Add(key, PlistReal.ReadXml(value[i + 1]));
                else if (valueType == "string")
                    _value.Add(key, PlistString.ReadXml(value[i + 1]));
                else if (valueType == "uid")
                    _value.Add(key, PlistUid.ReadXml(value[i + 1]));
                else
                    throw new PlistFormatException("Plist element is not a valid element");
            }
        }

        /// <summary>
        /// Adds an element to the array
        /// </summary>
        /// <param name="key">The key of the value to add</param>
        /// <param name="value">The value to add</param>
        /// <exception cref="System.ArgumentNullException">The provided key or value is null or empty</exception>
        /// <exception cref="System.StackOverflowException">The stack is too small</exception>
        /// <returns>The array after adding</returns>
        public PlistDict Add(string key, IPlistElement value)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "The specified key is null or empty");
            if (value == null)
                throw new ArgumentNullException("value", "The specified value is null");

            if (_value.ContainsKey(key))
                _value[key] = value;
            else
                _value.Add(key, value);

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
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "The specified key is null or empty");

            if (!_value.ContainsKey(key))
                throw new MissingFieldException("The specified key does not exist");

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
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "The specified key is null or empty");

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
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "The specified key is null or empty");

            if (!_value.ContainsKey(key))
                throw new IndexOutOfRangeException("The specified key does not exist in the array");

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
            return (T)Get(key);
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
                return Get(key);
            }
            set
            {
                Add(key, value);
            }
        }

        /// <summary>
        /// Gets the number of key-value pairs contained in this dictionary
        /// </summary>
        public int Length
        {
            get
            {
                return _value.Count;
            }
        }
    }
    public partial class PlistDict : IPlistElementInternal
    {
        // TODO
        internal static PlistDict ReadBinary(BinaryReader reader, byte firstbyte)
        {
            int length = firstbyte & 0x0F;
            if (length == 0x0F) {
                length = (int)PlistInteger.ReadBinary(reader, reader.ReadByte()).Value;
            }
            // TODO: keyref*
            // TODO: objref*
            throw new NotImplementedException();
        }

        // TODO
        void IPlistElementInternal.WriteBinary(BinaryWriter writer)
        {
            if (_value.Count < 0x0F) {
                writer.Write((byte)(0xD0 | _value.Count));
            } else {
                writer.Write((byte)0xDF);
                ((IPlistElementInternal)new PlistInteger(_value.Count)).WriteBinary(writer);
            }
            // TODO: keyref*
            // TODO: objref*
            throw new NotImplementedException();
        }

        internal static PlistDict ReadXml(XmlNode node)
        {
            return new PlistDict(node.ChildNodes);
        }

        void IPlistElementInternal.WriteXml(XmlNode tree, XmlDocument writer)
        {
            XmlElement element;
            element = writer.CreateElement("dict");

            foreach (KeyValuePair<string, IPlistElement> elem in _value) {
                XmlElement key = writer.CreateElement("key");
                key.InnerText = elem.Key;
                element.AppendChild(key);
                ((IPlistElementInternal)elem.Value).WriteXml(element, writer);
            }

            tree.AppendChild(element);
        }
    }
    public partial class PlistDict : IPlistElement<Dictionary<string, IPlistElement>>
    {
        internal Dictionary<string, IPlistElement> _value;

        /// <summary>
        /// Gets the Xml tag for this element
        /// </summary>
        public string XmlTag
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
                    throw new ArgumentNullException("value");

                _value = value;
            }
        }

        /// <summary>
        /// Gets the type of this element
        /// </summary>
        public PlistElementType ElementType
        {
            get
            {
                return PlistElementType.Dictionary;
            }
        }
    }
}