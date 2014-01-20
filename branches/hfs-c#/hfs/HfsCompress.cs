/* =============================================================================
 * File:   HfsCompress.cs
 * Author: Hexware
 * =============================================================================
 * Copyright (c) 2008-2010, planetbeing
 * Copyright (c) 2014, Hexware
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

namespace Xpwn.Hfs
{
    public struct HfsPlusDecmpfs
    {
        public uint Magic;
        public uint Flags;
        public ulong Size;
        public byte[] Data;
    }

    public struct HfsPlusCmpfRsrcHead
    {
        public uint HeaderSize;
        public uint TotalSize;
        public uint DataSize;
        public uint Flags;
    }

    public struct HfsPlusCmpfRsrcBlock
    {
        public uint Offset;
        public uint Size;
    }

    public struct HfsPlusCmpfRsrcBlockHead
    {
        public uint DataSize;
        public uint NumberOfBlocks;
        public HfsPlusCmpfRsrcBlock[] Blocks;
    }
    
    public struct HfsPlusCmpfEnd
    {
        //public fixed uint Pad[6];
        public ushort Unknown1;
        public ushort Unknown2;
        public ushort Unknown3;
        public uint Magic;
        public uint Flags;
        public ulong Size;
        public uint Unknown4;

        public HfsPlusCmpfEnd Read(BinaryReader reader)
        {
            HfsPlusCmpfEnd ret = new HfsPlusCmpfEnd();
            reader.BaseStream.Position = reader.BaseStream.Position + 24; // padding
            ret.Unknown1 = reader.ReadUInt16();
            ret.Unknown2 = reader.ReadUInt16();
            ret.Unknown3 = reader.ReadUInt16();
            ret.Magic = reader.ReadUInt32();
            ret.Flags = reader.ReadUInt32();
            ret.Size = reader.ReadUInt64();
            ret.Unknown4 = reader.ReadUInt32();
            return ret;
        }
    }

    public struct HfsPlusCompressed
    {
        //Volume* volume;
        //HFSPlusCatalogFile* file;
        //io_func* io;
        //size_t decmpfsSize;
        //HFSPlusDecmpfs* decmpfs;

        //HFSPlusCmpfRsrcHead rsrcHead;
        //HFSPlusCmpfRsrcBlockHead* blocks;

        //int dirty;

        //uint8_t* cached;
        //uint32_t cachedStart;
        //uint32_t cachedEnd;

    }
}
