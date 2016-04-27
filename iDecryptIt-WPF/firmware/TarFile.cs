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

namespace Hexware.Programs.iDecryptIt.firmware
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
        private Stream _stream;
        private string[] _entryNames; // a hashmap might be a good idea
        private long[] _entryOffsets;
        private long[] _entrySizes;

        public TarFile(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("Stream must support reading.", "stream");
            if (!stream.CanSeek)
                throw new ArgumentException("Stream must support seeking.", "stream");

            _stream = stream;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
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

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
