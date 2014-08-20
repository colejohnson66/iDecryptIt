/* =============================================================================
 * File:   AbstractFile.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2014, Cole Johnson
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 *   along with this program. If not, see <http://www.gnu.org/licenses/>.
 * =============================================================================
 */
using System.IO;

namespace Xpwn.Shared
{
    public enum AbstractFileType
    {
        File,
        S5L8900,
        Img2,
        Img3,
        Lzss,
        IBootIM,
        Memory,
        MemoryFile,
        Dummy
    }

    public struct AbstractFile
    {
        public byte[] Data;
        public delegate int Write(byte[] data);
        public delegate int Read(byte[] data, int length);
        public delegate int Seek(int offset);
        public delegate int Tell();
        public delegate void Close();
        public delegate int GetLength();
        public AbstractFileType Type;

        /*public AbstractFile(FileStream file)
        {
            // createAbstractFileFromFile
        }

        public AbstractFile()
        {
            // createAbstractFileFromDummy
        }

        public AbstractFile(byte[] buffer, int size)
        {
            // createAbstractFileFromMemory
        }

        public AbstractFile(byte[] buffer int[] size)
        {
            // createAbstractFileFromMemoryFile
        }

        public AbstractFile(byte[] buffer, int[] size, int actualSize)
        {
            // createAbstractFileFromMemoryFileBuffer
        }*/

        public void Print(string format, params object[] args)
        {

        }

        //io_func* IOFuncFromAbstractFile(AbstractFile* file);
    }

    public struct AbstractFile2
    {
        public AbstractFile Super;
        public delegate void SetKey(uint[] key, uint[] iv);
    }

    public struct MemoryWrapperInfo
    {
        int Offset;
        byte[] Buffer;
    }
    
    public struct MemoryFileWrapperInfo
    {
        int offset;
        byte[] Buffer;
        int BufferSize;
        int ActualBufferSize;
    }
}