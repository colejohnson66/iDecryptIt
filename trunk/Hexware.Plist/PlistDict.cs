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
    public partial class PlistDict
    {
        public PlistDict(Dictionary<string, IPlistElement> value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            _value = new Dictionary<string, IPlistElement>(value);
        }
        internal PlistDict(Dictionary<string, IPlistElement> value, bool unused)
        {
            // Used by PlistDict.ReadXml to avoid the shallow copy
            _value = value;
        }

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
        public PlistDict Delete(string key)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "The specified key is null or empty");

            if (!_value.ContainsKey(key))
                throw new MissingFieldException("The specified key does not exist");

            _value.Remove(key);

            return this;
        }
        public bool Exists(string key)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "The specified key is null or empty");

            return _value.ContainsKey(key);
        }
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
        public T Get<T>(string key) where T : IPlistElement
        {
            return (T)Get(key);
        }
        public ICollection<string> Keys
        {
            get
            {
                return _value.Keys;
            }
        }
        public ICollection<IPlistElement> Values
        {
            get
            {
                return _value.Values;
            }
        }
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
        public int Length
        {
            get
            {
                return _value.Count;
            }
        }
    }
    public partial class PlistDict : IPlistElement<Dictionary<string, IPlistElement>>
    {
        internal Dictionary<string, IPlistElement> _value;

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
        public PlistElementType ElementType
        {
            get
            {
                return PlistElementType.Dictionary;
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
            XmlNodeList children = node.ChildNodes;
            Dictionary<string, IPlistElement> ret = new Dictionary<string, IPlistElement>(children.Count);

            for (int i = 0; i < children.Count; i = i + 2) {
                string key = children[i].InnerText;
                XmlNode value = children[i + 1];
                string valueType = value.Name;

                if (children[i].Name != "key" ||
                    key.Contains("<") ||
                    key.Contains(">")) {
                    throw new PlistFormatException("\"" + children[i].InnerXml + "\" is not a valid Plist key");
                }

                if (valueType == "array")
                    ret.Add(key, PlistArray.ReadXml(value));
                else if (valueType == "true" || valueType == "false")
                    ret.Add(key, PlistBool.ReadXml(value));
                else if (valueType == "data")
                    ret.Add(key, PlistData.ReadXml(value));
                else if (valueType == "date")
                    ret.Add(key, PlistDate.ReadXml(value));
                else if (valueType == "dict")
                    ret.Add(key, PlistDict.ReadXml(value));
                else if (valueType == "integer")
                    ret.Add(key, PlistInteger.ReadXml(value));
                else if (valueType == "null")
                    ret.Add(key, PlistNull.ReadXml(value));
                else if (valueType == "real")
                    ret.Add(key, PlistReal.ReadXml(value));
                else if (valueType == "string")
                    ret.Add(key, PlistString.ReadXml(value));
                else
                    throw new PlistFormatException("Plist element is not a valid element");
            }
            return new PlistDict(ret, false);
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
}