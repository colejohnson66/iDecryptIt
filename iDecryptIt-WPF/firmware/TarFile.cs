/* =============================================================================
 * File:   TarFile.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2016 Cole Johnson
 * 
 * This file is part of iDecryptIt.
 * 
 * iDecryptIt is free software: you can redistribute it and/or modify it under
 *   the terms of the GNU General Public License as published by the Free
 *   Software Foundation, either version 3 of the License, or (at your option)
 *   any later version.
 * 
 * iDecryptIt is distributed in the hope that it will be useful, but WITHOUT
 *   ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 *   FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for
 *   more details.
 * 
 * You should have received a copy of the GNU General Public License along with
 *   iDecryptIt. If not, see <http://www.gnu.org/licenses/>.
 * =============================================================================
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hexware.Programs.iDecryptIt.Firmware
{
    // Implements just the "ustar" format as that's all that's needed
    public class TarFile : IDisposable
    {
        /* NOTE: `cStr[x]' denotes an null terminated, ASCII encoded string of length `x'.
         *       `octStr[x]' denotes an null terminated string containing an octal encoded
         *       number. Also, all offsets are in decimal.
         * 
         * TarEntry {
         *    0  cStr[100]  fileName;
         *  100  octStr[8]  fileMode;
         *  108  octStr[8]  userId;
         *  116  octStr[8]  groupId;
         *  124  octStr[12] fileSize;
         *  136  octStr[12] modifyTime; // "Unix" time
         *  148  octStr[8]  checksum;
         *  156  byte       fileType; // all we care about is '0' and NULL which
         *  157  cStr[100]  linkedFileName;            // indicate a normal file
         *  257  byte[6]    magic;    // "ustar\0"
         *  263  byte[2]    version;  // "00"
         *  265  cStr[32]   userName;
         *  297  cStr[32]   groupName;
         *  329  octStr[8]  deviceMajNum;
         *  337  octStr[8]  deviceMinNum;
         *  345  cStr[155]  fileNamePrefix;
         *  500  byte[12];                // pad out to 0x200
         *  512  byte[]     fileContents; // sizeof(fileContents) == fileSize
         *  ???  byte[];                  // optional padding out to a multiple of 512
         * }
         * TarArchive {
         *    0  TarEntry[]  entries;
         *  ???  TarEntry[2] footer; // 0x400 NULL bytes
         * }
         */

        private static readonly byte[] Magic = new byte[] { (byte)'u', (byte)'s', (byte)'t', (byte)'a', (byte)'r' };
        
        private Stream _stream;
        private Dictionary<string, TarEntry> _entries;

        public TarFile(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("Stream must support reading.", "stream");
            if (!stream.CanSeek)
                throw new ArgumentException("Stream must support seeking.", "stream");

            _stream = stream;
            ParseStream();
        }

        private void ParseStream()
        {
            _entries = new Dictionary<string, TarEntry>();
            _stream.Seek(0, SeekOrigin.Begin);
            
            while (true)
            {
                byte[] header = new byte[512];
                if (_stream.Read(header, 0, 512) != 512)
                    throw new FileFormatException("Stream contains incomplete Tape Archive entry header.");

                // Is this a null entry? If so, we've reached the supposed end of the
                //   stream. On real tape archives using the TAR format, there may be
                //   data after the null entry, but it is ignored. In addition, there
                //   should be two null entries to signify the end of the archive,
                //   but 7-Zip handles just fine with only one null entry.
                if (header.All(x => (x == 0)))
                    return;

                // Is this a ustar file?
                if (!Magic.SequenceEqual(header.Skip(257).Take(5)))
                    throw new FileFormatException("Tape Archive entry is not a valid ustar entry.");
                
                // Get file name and file size. Trim null characters while doing so.
                char[] arrfileName = Encoding.ASCII.GetChars(header, 0, 100);
                if (arrfileName[0] == 0)
                    throw new FileFormatException("Invalid file name.");
                string strFileName = new String(arrfileName.TakeWhile(x => x != 0).ToArray());
                char[] arrfileSize = Encoding.ASCII.GetChars(header, 124, 12);
                string strFileSize = new String(arrfileSize.TakeWhile(x => x != 0).ToArray());

                // Before adding the entry to the list, ensure the stream contains that many bytes.
                // Interpret the file size as unsigned to ensure it's not read as a signed number.
                //   TODO: Are numbers interpreted as signed /at all/?
                ulong fileSize = Convert.ToUInt64(strFileSize, 8);
                TarEntry entry = new TarEntry(_stream.Position, (long)fileSize);

                // Round file size up to multiple of 512
                // Hacker's Delight 2nd Ed., pg. 59
                ulong paddedFileLength = (fileSize + 511UL) & ~511UL;

                // Skip over file
                try
                {
                    _stream.Seek((long)paddedFileLength, SeekOrigin.Current);
                }
                catch (IOException ex)
                {
                    throw new FileFormatException("File length extends past end of stream.", ex);
                }

                // Add the entry
                if (_entries.ContainsKey(strFileName))
                    throw new FileFormatException("Encountered a file with an already encountered file path.");

                _entries.Add(strFileName, entry);
            }
        }

        public MemoryStream GetFile(string fileName)
        {
            TarEntry entry;
            if (!_entries.TryGetValue(fileName, out entry))
                throw new FileNotFoundException("Specified file does not exist in archive.");

            if (entry.Size > Int32.MaxValue)
                throw new InsufficientMemoryException("Specified file too large to allocate.");

            byte[] buf = new byte[(int)entry.Size];
            _stream.Seek(entry.Offset, SeekOrigin.Begin);
            _stream.Read(buf, 0, buf.Length);
            return new MemoryStream(buf, false);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _stream.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~TarFile() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }
        
        public void Dispose()
        {
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        private struct TarEntry
        {
            internal long Offset;
            internal long Size;

            public TarEntry(long offset, long size)
            {
                Offset = offset;
                Size = size;
            }
        }
    }
}
