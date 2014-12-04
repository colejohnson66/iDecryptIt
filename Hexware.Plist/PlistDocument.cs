/* =============================================================================
 * File:   PlistDocument.cs
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
using System.Text;
using System.Xml;

namespace Hexware.Plist
{
    /// <summary>
    /// Represents a Plist document
    /// </summary>
    public partial class PlistDocument
    {
        private IPlistElement _value;

        /// <summary>
        /// Hexware.Plist.PlistDocument Constructor from a file
        /// </summary>
        /// <param name="plistPath">The file path</param>
        /// <exception cref="Hexware.Plist.PlistFormatException">An invalid Xml Plist was given</exception>
        /// <exception cref="Hexware.Plist.PlistFormatException">Binary and Json Plists have not been implemented yet</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="plistPath"/> is null</exception>
        /// <exception cref="System.IO.FileNotFoundException"><paramref name="plistPath"/> does not exist</exception>
        /// <exception cref="System.IO.FileLoadException">Load Error</exception>
        /// <exception cref="System.IO.IOException">Unable to load Plist</exception>
        /// <exception cref="System.Xml.XmlException"><paramref name="plistPath"/> is not an Xml Plist</exception>
        public PlistDocument(string plistPath)
        {
            if (String.IsNullOrEmpty(plistPath))
                throw new ArgumentNullException("plistPath", "The specified path is null or empty");

            if (!plistPath.Contains(":"))
                plistPath = Path.Combine(Directory.GetCurrentDirectory(), plistPath); // Relative
            if (!File.Exists(plistPath))
                throw new FileNotFoundException("The specified file does not exist", plistPath);
            if (Directory.Exists(plistPath))
                throw new ArgumentException("The specified file is a directory", "plistPath");

            // Read file
            FileStream fileStream = null;
            byte[] bin;
            try
            {
                fileStream = new FileStream(plistPath, FileMode.Open, FileAccess.Read);
                long length = fileStream.Length;
                if (length < 8)
                    throw new FileLoadException("The specified file is not bigger than 8 bytes", plistPath);
                bin = new byte[length];
                int count;
                int sum = 0;

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(bin, sum, (int)(length - sum))) > 0)
                    sum += count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                fileStream.Close();
            }

            // Is it binary or xml
            if (bin[0] == 'b' &&
                bin[1] == 'p' &&
                bin[2] == 'l' &&
                bin[3] == 'i' &&
                bin[4] == 's' &&
                bin[5] == 't' &&
                bin[6] == '0')
            {
                throw new PlistFormatException("Binary Plists have not been implemented yet");
            }
            else
            {
                StreamReader stream = null;
                try
                {
                    stream = new StreamReader(plistPath);
                    XmlReaderSettings settings = new XmlReaderSettings();
                    XmlDocument xml = new XmlDocument();
                    settings.XmlResolver = null;
                    settings.DtdProcessing = DtdProcessing.Ignore;
                    settings.ValidationType = ValidationType.None;
                    xml.Load(XmlReader.Create(stream, settings));
                    ReadXml(xml);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// Hexware.Plist.PlistDocument constructor from a one-dimensional <see cref="System.Byte"/> array of ASCII characters (slow)
        /// </summary>
        /// <param name="plistData">A one-dimensional <see cref="System.Byte"/> array of the Plist data</param>
        /// <exception cref="Hexware.Plist.PlistFormatException">An invalid Xml Plist was given</exception>
        /// <exception cref="Hexware.Plist.PlistFormatException">Binary and Json Plists have not been implemented yet</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="plistData"/> is null or empty</exception>
        /// <exception cref="System.Xml.XmlException"><paramref name="plistData"/> is not an ASCII Xml Plist</exception>
        public PlistDocument(byte[] plistData) : this(plistData, Encoding.ASCII)
        {
        }

        /// <summary>
        /// Hexware.Plist.PlistDocument constructor from a one-dimensional <see cref="System.Byte"/> array of bytes
        /// </summary>
        /// <param name="plistData">A one-dimensional <see cref="System.Byte"/> array of the Plist data</param>
        /// <param name="encoding">The encoding of <paramref name="plistData"/> if it is an Xml Plist; use null for binary Plists</param>
        /// <exception cref="Hexware.Plist.PlistFormatException">An invalid Xml Plist was given</exception>
        /// <exception cref="Hexware.Plist.PlistFormatException">Binary and Json Plists have not been implemented yet</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="plistData"/> is null or empty</exception>
        /// <exception cref="System.Xml.XmlException"><paramref name="plistData"/> is not an Xml Plist</exception>
        public PlistDocument(byte[] plistData, Encoding encoding)
        {
            if (plistData == null || plistData.Length == 0) {
                throw new ArgumentNullException("plistData");
            }

            if (plistData[0] == 'b' &&
                plistData[1] == 'p' &&
                plistData[2] == 'l' &&
                plistData[3] == 'i' &&
                plistData[4] == 's' &&
                plistData[5] == 't' &&
                plistData[6] == '0') {
                throw new PlistFormatException("Binary Plists have not been implemented yet");
            }

            try {
                XmlReaderSettings settings = new XmlReaderSettings();
                XmlDocument xml = new XmlDocument();
                settings.XmlResolver = null;
                settings.DtdProcessing = DtdProcessing.Ignore;
                settings.ValidationType = ValidationType.None;
                xml.Load(XmlReader.Create(encoding.GetString(plistData), settings));
                ReadXml(xml);
            } catch (Exception) {
                throw;
            }
        }

        /// <summary>
        /// Hexware.Plist.PlistDocument constructor from a <see cref="System.IO.Stream"/>
        /// </summary>
        /// <param name="plistStream"></param>
        /// <exception cref="Hexware.Plist.PlistFormatException">An invalid Xml Plist was given</exception>
        /// <exception cref="Hexware.Plist.PlistFormatException">Binary and Json Plists have not been implemented yet</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="plistStream"/> is null</exception>
        /// <exception cref="System.Xml.XmlException"><paramref name="plistStream"/> is not an Xml Plist</exception>
        public PlistDocument(Stream plistStream)
        {
            if (plistStream == null || plistStream == Stream.Null || plistStream.Length == 0)
                throw new ArgumentNullException("plistStream");

            // Reset stream
            plistStream.Position = 0;

            // Check for magic number
            byte[] buf = new byte[7];
            plistStream.Read(buf, 0, 7);
            if (buf[0] == 'b' &&
                buf[1] == 'p' &&
                buf[2] == 'l' &&
                buf[3] == 'i' &&
                buf[4] == 's' &&
                buf[5] == 't' &&
                buf[6] == '0')
            {
                throw new PlistFormatException("Binary Plists have not been implemented yet");
            }

            // Reset stream
            plistStream.Position = 0;

            // Begin read
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                XmlDocument xml = new XmlDocument();
                settings.XmlResolver = null;
                settings.DtdProcessing = DtdProcessing.Ignore;
                settings.ValidationType = ValidationType.None;
                xml.Load(XmlReader.Create(plistStream, settings));
                ReadXml(xml);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the root node of the Plist data
        /// </summary>
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
        internal void ReadBinary()
        {
            throw new NotImplementedException();
        }

        internal void WriteBinary(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        internal void ReadXml(XmlDocument reader)
        {
            for (int i = 0; i < reader.ChildNodes.Count; i++)
            {
                XmlNode current = reader.ChildNodes[i];
                if (current.Name == "plist")
                {
                    if (!current.HasChildNodes)
                        throw new PlistFormatException("Plist is not valid");

                    XmlNode root = current.ChildNodes.Item(0);
                    if (root.Name == "array")
                        _value = new PlistArray(root.ChildNodes);
                    else if (root.Name == "true")
                        _value = new PlistBool(true);
                    else if (root.Name == "false")
                        _value = new PlistBool(false);
                    else if (root.Name == "data")
                        _value = new PlistData(root.InnerText);
                    else if (root.Name == "date")
                        _value = new PlistDate(root.InnerText);
                    else if (root.Name == "dict")
                        _value = new PlistDict(root.ChildNodes);
                    else if (root.Name == "fill")
                        _value = new PlistFill();
                    else if (root.Name == "integer")
                        _value = new PlistInteger(root.InnerText);
                    else if (root.Name == "null")
                        _value = new PlistNull();
                    else if (root.Name == "real")
                        _value = new PlistReal(Convert.ToDouble(root.InnerText));
                    else if (root.Name == "string" || root.Name == "ustring")
                        _value = new PlistString(root.InnerText);
                    else if (root.Name == "uid")
                        _value = new PlistUid(Encoding.ASCII.GetBytes(root.InnerText));
                    else
                        throw new PlistFormatException("Plist is not valid");
                    return;
                }
            }
            throw new PlistFormatException("Plist not in correct format!");
        }

        internal void WriteXml(XmlNode tree, XmlDocument writer)
        {
            throw new NotImplementedException();
        }
    }
}