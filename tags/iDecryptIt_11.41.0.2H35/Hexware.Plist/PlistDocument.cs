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
using System.Text;
using System.Xml;

namespace Hexware.Plist
{
    public partial class PlistDocument
    {
        // 'bplist' in ASCII
        private static byte[] magic = { 98, 112, 108, 105, 115, 116 };
        private static XmlWriterSettings xmlSettings = XmlSettings();

        private IPlistElement _value;

        public PlistDocument(IPlistElement rootNode)
        {
            if (rootNode == null)
                throw new ArgumentNullException("rootNode");

            _value = rootNode;
        }
        public PlistDocument(string filePath)
        {
            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Init(stream);
            stream.Close();
        }
        public PlistDocument(Stream stream)
        {
            Init(stream);
        }

        private void Init(Stream stream)
        {
            if (stream == null || stream == Stream.Null || stream.Length == 0)
                throw new ArgumentNullException("stream");

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

        private static XmlWriterSettings XmlSettings()
        {
            XmlWriterSettings ret = new XmlWriterSettings();
            ret.Indent = true;
            ret.IndentChars = "\t";
            ret.NewLineChars = "\n";
            ret.CloseOutput = true;
            ret.Encoding = Encoding.UTF8;
            return ret;
        }

        public void Save(string filePath, PlistDocumentType format)
        {
            FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            Save(stream, format);
            stream.Close();
        }
        public void Save(Stream stream, PlistDocumentType format)
        {
            if (format == PlistDocumentType.Binary)
                SaveBinary(stream);
            else if (format == PlistDocumentType.Xml)
                SaveXml(stream);
        }
        // TODO
        private void SaveBinary(Stream stream)
        {
            throw new NotImplementedException();
        }
        private void SaveXml(Stream stream)
        {
            XmlWriter writer = XmlWriter.Create(stream, xmlSettings);
            XmlDocument document = new XmlDocument();

            document.AppendChild(document.CreateDocumentType("plist", "-//Apple Computer//DTD PLIST 1.0//EN", "http://www.apple.com/DTDs/PropertyList-1.0.dtd", null));

            XmlElement plist = document.CreateElement("plist");
            ((IPlistElementInternal)_value).WriteXml(plist, document);
            document.AppendChild(plist);

            document.Save(writer);
        }

        public IPlistElement RootNode
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
    }
    public partial class PlistDocument
    {
        internal void ReadBinary(BinaryPlistReader reader)
        {
            reader.BaseStream.Seek(-32, SeekOrigin.End);
            reader.Trailer = new BinaryPlistTrailer(reader.ReadBytes(32));

            reader.BaseStream.Seek(reader.Trailer.OffsetTableOffset, SeekOrigin.Begin);
            reader.ObjectOffsets = new long[reader.Trailer.NumberOfObjects];
            for (int i = 0; i < reader.Trailer.NumberOfObjects; i++) {
                byte[] buf = reader.ReadBytes(reader.Trailer.OffsetTableOffsetSize);
                reader.ObjectOffsets[i] = (long)BinaryPlistReader.ParseUnsignedBigEndianNumber(buf);
            }

            _value = reader.ParseObject(reader.Trailer.RootObjectNumber);
        }
        // TODO
        internal void WriteBinary(BinaryPlistWriter writer)
        {
            // https://github.com/songkick/plist/blob/master/src/com/dd/plist/BinaryPropertyListParser.java
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
                        throw new PlistException("Plist is not valid.");

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
                        throw new PlistException("Plist is not valid.");
                    return;
                }
            }
            throw new PlistException("Plist not in correct format.");
        }
    }
}