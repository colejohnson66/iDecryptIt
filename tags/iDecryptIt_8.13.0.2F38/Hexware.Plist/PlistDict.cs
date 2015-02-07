/* =============================================================================
 * File:   PlistDict.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2012, 2014-2015 Cole Johnson
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
using System.Xml;

namespace Hexware.Plist
{
    public partial class PlistDict : IPlistElement
    {
        internal Dictionary<string, IPlistElement> _value;

        public PlistDict(Dictionary<string, IPlistElement> value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            _value = new Dictionary<string, IPlistElement>(value);
        }
        internal PlistDict(Dictionary<string, IPlistElement> value, bool unused)
        {
            // Used by PlistDict.ReadBinary and PlistDict.ReadXml to avoid the shallow copy
            _value = value;
        }

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

        public bool CanSerialize(PlistDocumentType type)
        {
            foreach (KeyValuePair<string, IPlistElement> val in _value) {
                if (!val.Value.CanSerialize(type))
                    return false;
            }
            return true;
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
        internal static PlistDict ReadBinary(BinaryPlistReader reader, byte firstbyte)
        {
            int length = firstbyte & 0x0F;
            if (length == 0x0F)
                length = (int)PlistInteger.ReadBinary(reader, reader.ReadByte()).Value;

            int[] keyrefs = new int[length];
            int[] objrefs = new int[length];
            for (int i = 0; i < length; i++)
                keyrefs[i] = (int)BinaryPlistReader.ParseUnsignedBigEndianNumber(
                    reader.ReadBytes(reader.Trailer.ReferenceOffsetSize));
            for (int i = 0; i < length; i++)
                objrefs[i] = (int)BinaryPlistReader.ParseUnsignedBigEndianNumber(
                    reader.ReadBytes(reader.Trailer.ReferenceOffsetSize));

            Dictionary<string, IPlistElement> ret = new Dictionary<string, IPlistElement>();
            for (int i = 0; i < length; i++) {
                IPlistElement key = reader.ParseObject(keyrefs[i]);
                IPlistElement val = reader.ParseObject(objrefs[i]);
                if (key == null) {
                    if (val == null)
                        continue;
                    throw new PlistException(string.Format(
                        "Unknown Plist dictionary key type encountered at object {0}.",
                        keyrefs[i]));
                }
                if (key.ElementType != PlistElementType.String)
                    throw new PlistException(String.Format(
                        "Unknown Plist dictionary key type encountered at object {0}. Expected string, encountered {1}.",
                        keyrefs[i], key.ElementType));
                ret.Add(((PlistString)key).Value, val);
            }
            return new PlistDict(ret, false);
        }
        // TODO
        void IPlistElementInternal.WriteBinary(BinaryPlistWriter writer)
        {
            if (_value.Count < 0x0F) {
                writer.Write((byte)(0xD0 | _value.Count));
            } else {
                writer.Write((byte)0xDF);
                writer.WriteTypedInteger(_value.Count);
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
                else if (value.Name == "true")
                    ret.Add(key, new PlistBool(true));
                else if (value.Name == "false")
                    ret.Add(key, new PlistBool(false));
                else if (valueType == "data")
                    ret.Add(key, PlistData.ReadXml(value));
                else if (valueType == "date")
                    ret.Add(key, PlistDate.ReadXml(value));
                else if (valueType == "dict")
                    ret.Add(key, PlistDict.ReadXml(value));
                else if (valueType == "integer")
                    ret.Add(key, PlistInteger.ReadXml(value));
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