/* =============================================================================
 * File:   PlistDocument.cs
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
using System.IO;
using System.Linq;
using System.Xml;

namespace Hexware.Plist
{
    public partial class PlistDocument
    {
        // 'bplist' in ASCII
        private static byte[] magic = { 98, 112, 108, 105, 115, 116 };

        private IPlistElement _value;

        public PlistDocument(string filePath)
        {
            Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Init(stream, true);
            stream.Close();
        }
        public PlistDocument(Stream stream)
        {
            Init(stream, false);
        }

        private void Init(Stream stream, bool fromFile)
        {
            if (stream == null || stream == Stream.Null || stream.Length == 0)
                throw new ArgumentNullException(fromFile ? "filePath" : "stream");

            byte[] buf = new byte[6];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(buf, 0, 6);
            if (Enumerable.SequenceEqual(buf, magic)) {
                ReadBinary(new BinaryPlistReader(stream));
                return;
            }

            stream.Seek(0, SeekOrigin.Begin);
            XmlReaderSettings settings = new XmlReaderSettings();
            XmlDocument xml = new XmlDocument();
            settings.XmlResolver = null;
            settings.DtdProcessing = DtdProcessing.Ignore;
            settings.ValidationType = ValidationType.None;
            xml.Load(XmlReader.Create(stream, settings));
            ReadXml(xml.ChildNodes);
        }

        public IPlistElement RootNode
        {
            get
            {
                return _value;
            }
        }
    }
    public partial class PlistDocument
    {
        internal void ReadBinary(BinaryPlistReader reader)
        {
            // Magic already parsed
            //reader.BaseStream.Seek(0, SeekOrigin.Begin);
            //if (!Enumerable.SequenceEqual(reader.ReadBytes(6), magic))
            //    throw new PlistFormatException("Not a valid binary Plist");

            reader.BaseStream.Seek(-32, SeekOrigin.End);
            reader.Trailer = new BinaryPlistTrailer(reader.ReadBytes(32));

            reader.BaseStream.Seek(reader.Trailer.OffsetTableOffset, SeekOrigin.Begin);
            reader.ObjectOffsets = new int[reader.Trailer.NumberOfObjects];
            for (int i = 0; i < reader.Trailer.NumberOfObjects; i++) {
                byte[] buf = reader.ReadBytes(reader.Trailer.OffsetTableOffsetSize);
                reader.ObjectOffsets[i] = (int)BinaryPlistReader.ParseUnsignedBigEndianNumber(buf);
            }

            _value = reader.ParseObject(reader.Trailer.RootObjectNumber);
        }
        // TODO
        internal void WriteBinary(BinaryPlistWriter writer)
        {
            // HELP: https://github.com/songkick/plist/blob/master/src/com/dd/plist/BinaryPropertyListParser.java
            writer.Write(magic);
            writer.Write((byte)'0');
            writer.Write((byte)'0');
            throw new NotImplementedException();
        }
        internal void ReadXml(XmlNodeList reader)
        {
            // Don't ensure there's only one element under <plist>
            // as CoreFoundation ignores anything after the first.
            for (int i = 0; i < reader.Count; i++)
            {
                XmlNode current = reader[i];
                if (current.Name == "plist")
                {
                    if (!current.HasChildNodes)
                        throw new PlistFormatException("Plist is not valid");

                    XmlNode root = current.ChildNodes.Item(0);
                    if (root.Name == "array")
                        _value = PlistArray.ReadXml(root);
                    else if (root.Name == "true" || root.Name == "false")
                        _value = PlistArray.ReadXml(root);
                    else if (root.Name == "data")
                        _value = PlistData.ReadXml(root);
                    else if (root.Name == "date")
                        _value = PlistDate.ReadXml(root);
                    else if (root.Name == "dict")
                        _value = PlistDict.ReadXml(root);
                    else if (root.Name == "integer")
                        _value = PlistInteger.ReadXml(root);
                    else if (root.Name == "real")
                        _value = PlistReal.ReadXml(root);
                    else if (root.Name == "string")
                        _value = PlistString.ReadXml(root);
                    else
                        throw new PlistFormatException("Plist is not valid");
                    return;
                }
            }
            throw new PlistFormatException("Plist not in correct format!");
        }
        internal void WriteXml(XmlNode tree, XmlDocument writer)
        {
            XmlNode element = writer.CreateElement("plist");
            ((IPlistElementInternal)_value).WriteXml(element, writer);
            tree.AppendChild(element);
        }
    }
}