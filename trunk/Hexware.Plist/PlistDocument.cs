/* =============================================================================
 * File:   PlistDocument.cs
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
using System.Text;
using System.Net;
using System.Xml;

namespace Hexware.Plist
{
    /// <summary>
    /// Represents a Plist Document
    /// </summary>
    public partial class PlistDocument
    {
        private string _path;
        private PlistRoot _value;
        private PlistFormat _plisttype;

        /// <summary>
        /// Hexware.Plist.PlistDocument Constructor from a file
        /// </summary>
        /// <param name="plistPath">The File path</param>
        /// <exception cref="Hexware.Plist.PlistFormatException">Binary and Json Plists have not been implemented yet</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="plistPath"/> is null</exception>
        /// <exception cref="System.IO.FileNotFoundException"><paramref name="plistPath"/> does not exist</exception>
        /// <exception cref="System.IO.FileLoadException">Load Error</exception>
        /// <exception cref="System.IO.IOException">Unable to load Plist</exception>
        /// <exception cref="System.Xml.XmlException"><paramref name="plistPath"/> is not an Xml Plist</exception>
        public PlistDocument(string plistPath)
        {
            if (plistPath == null || plistPath == "")
            {
                throw new ArgumentNullException("plistPath", "The specified path is null or empty");
            }

            if (!plistPath.Contains(":"))
            {
                // Relative
                plistPath = Path.Combine(Directory.GetCurrentDirectory(), plistPath);
            }
            if (!File.Exists(plistPath))
            {
                Dispose();
                throw new FileNotFoundException("The specified file does not exist", plistPath);
            }
            if (Directory.Exists(plistPath))
            {
                Dispose();
                throw new ArgumentException("The specified file is a directory", "plistPath");
            }

            // Read file
            FileStream fileStream = null;
            byte[] bin;
            try
            {
                fileStream = new FileStream(plistPath, FileMode.Open, FileAccess.Read);
                long length = fileStream.Length;
                if (length < 8)
                {
                    throw new FileLoadException("The specified file is not bigger than 8 bytes", plistPath);
                }
                bin = new byte[length];
                int count;
                int sum = 0;

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(bin, sum, (int)(length - sum))) > 0)
                {
                    sum += count;
                }
            }
            catch (FileLoadException ex)
            {
                Dispose();
                throw ex;
            }
            catch (IOException ex)
            {
                Dispose();
                throw ex;
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
                Dispose();
                throw new PlistFormatException("Binary Plists have not been implemented yet");
                //_plisttype = PlistFormat.Binary;
            }
            else
            {
                _plisttype = PlistFormat.Xml;
                StreamReader stream = null;
                try
                {
                    stream = new StreamReader(plistPath);
                    //XmlReaderSettings settings = new XmlReaderSettings();
                    XmlDocument xml = new XmlDocument();
                    //settings.XmlResolver = null;
                    //settings.IgnoreComments = true;
                    //settings.ProhibitDtd = true;
                    //settings.ValidationType = ValidationType.None;
                    string streamtext = stream.ReadToEnd();
                    //xml.Load(XmlReader.Create(streamtext, settings));
                    xml.LoadXml(streamtext);
                    streamtext = null;
                    ReadXml(xml);
                }
                catch (IOException ex)
                {
                    Dispose();
                    throw new IOException("Unable to read Plist.", ex);
                }
                catch (PlistFormatException ex)
                {
                    Dispose();
                    throw ex;
                }
                catch (XmlException ex)
                {
                    Dispose();
                    throw new XmlException("Failed to extract Xml from document. Possible Json.", ex, ex.LineNumber, ex.LinePosition);
                }
                finally
                {
                    stream.Close();
                }
            }

            _path = plistPath;
        }

        /// <summary>
        /// Hexware.Plist.PlistDocument constructor from a one-dimensional <see cref="System.Byte"/> array of ASCII characters (slow)
        /// </summary>
        /// <param name="plistData">A one-dimensional <see cref="System.Byte"/> array of the Plist data</param>
        /// <exception cref="Hexware.Plist.PlistFormatException">Binary Plists have not been implemented yet</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="plistData"/> is null</exception>
        /// <exception cref="System.Xml.XmlException"><paramref name="plistData"/> is not an ASCII Xml Plist</exception>
        public PlistDocument(byte[] plistData)
        {
            if (plistData == null || plistData.Length == 0)
            {
                throw new ArgumentNullException("plistData");
            }

            if (plistData[0] == 'b' &&
                plistData[1] == 'p' &&
                plistData[2] == 'l' &&
                plistData[3] == 'i' &&
                plistData[4] == 's' &&
                plistData[5] == 't' &&
                plistData[6] == '0')
            {
                Dispose();
                throw new PlistFormatException("Binary Plists have not been implemented yet");
                //_plisttype = PlistFormat.Binary;
            }

            try
            {
                _plisttype = PlistFormat.Xml;
                XmlReaderSettings settings = new XmlReaderSettings();
                XmlDocument xml = new XmlDocument();
                settings.XmlResolver = null;
                settings.ProhibitDtd = false;
                settings.ValidationType = ValidationType.None;
                xml.Load(XmlReader.Create(Encoding.ASCII.GetString(plistData), settings));
                ReadXml(xml);
            }
            catch (PlistFormatException ex)
            {
                Dispose();
                throw ex;
            }
            catch (XmlException ex)
            {
                Dispose();
                throw new XmlException("Failed to extract Xml from array. Possible Json.", ex, ex.LineNumber, ex.LinePosition);
            }
        }

        /// <summary>
        /// Hexware.Plist.PlistDocument constructor from a <see cref="System.IO.Stream"/>
        /// </summary>
        /// <param name="plistStream"></param>
        /// <exception cref="Hexware.Plist.PlistFormatException">Binary Plists have not been implemented yet</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="plistStream"/> is null</exception>
        /// <exception cref="System.Xml.XmlException"><paramref name="plistStream"/> is not an Xml Plist</exception>
        public PlistDocument(Stream plistStream)
        {
            if (plistStream == null || plistStream == Stream.Null || plistStream.Length == 0)
            {
                throw new ArgumentNullException("plistStream");
            }

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
                Dispose();
                throw new PlistFormatException("Binary Plists have not been implemented yet");
                //_plisttype = PlistFormat.Binary;
            }

            // Reset stream
            plistStream.Position = 0;

            // Begin read
            try
            {
                _plisttype = PlistFormat.Xml;
                XmlReaderSettings settings = new XmlReaderSettings();
                XmlDocument xml = new XmlDocument();
                settings.XmlResolver = null;
                settings.ProhibitDtd = false;
                settings.ValidationType = ValidationType.None;
                xml.Load(XmlReader.Create(plistStream, settings));
                ReadXml(xml);
            }
            catch (PlistFormatException ex)
            {
                Dispose();
                throw ex;
            }
            catch (XmlException ex)
            {
                Dispose();
                throw new XmlException("Failed to extract Xml from array. Possible Json.", ex, ex.LineNumber, ex.LinePosition);
            }
        }

        /// <summary>
        /// Get the type of Plist the file is (Xml, Binary, or Json)
        /// </summary>
        public PlistFormat Type
        {
            get
            {
                return _plisttype;
            }
        }

        /// <summary>
        /// Get the path of the Plist file. Returns null if (<see cref="System.Byte"/>[]) constructor used used
        /// </summary>
        public string PlistPath
        {
            get
            {
                return _path;
            }
        }

        /// <summary>
        /// Get a <see cref="Hexware.Plist.PlistRoot"/> representation of the Plist data
        /// </summary>
        public PlistRoot Value
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

        internal byte[] WriteBinary()
        {
            throw new NotImplementedException();
        }

        internal void ReadXml(XmlDocument reader)
        {
            int length = reader.ChildNodes.Count;
            for (int i = 0; i < length; i++)
            {
                // Find the content node
                if (reader.ChildNodes[i].NodeType == XmlNodeType.Element)
                {
                    if (reader.ChildNodes[i].ChildNodes.Count == 1 &&
                        reader.ChildNodes[i].ChildNodes.Item(0).Name == "dict")
                    {
                        // Most should have a <dict> tag at the root under the <plist> tag
                        _value = new PlistRoot(
                            new PlistDict(
                                reader.ChildNodes[i]["dict"].ChildNodes,
                                "(Root)/",
                                null));
                    }
                    else
                    {
                        // But if it doesn't
                        _value = new PlistRoot(
                            new PlistDict(
                                reader.ChildNodes[i].ChildNodes,
                                "(Root)/",
                                null));
                    }
                    return;
                }
            }
            throw new PlistFormatException("Plist not in correct format!");
        }

        internal void WriteXml(XmlNode tree, XmlDocument writer)
        {
            _value.WriteXml(tree, writer);
        }
    }
    public partial class PlistDocument : IDisposable
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
                _value.Dispose();
            }

            _path = null;

            _disposed = true;
        }

        /// <summary>
        /// Free up resources used on the system for garbage collector
        /// </summary>
        ~PlistDocument()
        {
            Dispose(false);
        }
    }
}
