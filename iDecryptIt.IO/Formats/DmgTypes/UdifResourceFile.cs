/* =============================================================================
 * File:   UdifResourceFile.cs
 * Author: Cole Tobin
 * =============================================================================
 * Copyright (c) 2022 Cole Tobin
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

using System.IO;

namespace iDecryptIt.IO.Formats.DmgTypes;

internal record UdifResourceFile(
    uint Version,
    uint HeaderSize,
    uint Flags,

    //
    ulong RunningDataForkOffset,
    ulong DataForkOffset,
    ulong DataForkLength,
    ulong ResourceForkOffset,
    ulong ResourceForkLength,

    //
    uint SegmentNumber,
    uint SegmentCount,
    UdifID SegmentID,

    //
    UdifChecksum DataForkChecksum,

    //
    ulong XmlOffset,
    ulong XmlLength,

    //
    // byte[] Reserved,
    //
    UdifChecksum MasterChecksum,

    //
    uint ImageVariant,
    ulong SectorCount

    //
    // uint[] Reserved2,
)
{
    private const string MAGIC = "koly";

    public static UdifResourceFile Read(BiEndianBinaryReader reader)
    {
        // TODO: is this big endian?
        if (reader.ReadAsciiChars(4) is not MAGIC)
            throw new InvalidDataException("Stream does not contain a UDIF resource file at the expected location.");
        uint version = reader.ReadUInt32BE();
        uint headerSize = reader.ReadUInt32BE();
        uint flags = reader.ReadUInt32BE();

        ulong runningDataFork = reader.ReadUInt64BE();
        ulong dataForkOffset = reader.ReadUInt64BE();
        ulong dataForkLength = reader.ReadUInt64BE();
        ulong resourceForkOffset = reader.ReadUInt64BE();
        ulong resourceForkLength = reader.ReadUInt64BE();

        uint segmentNumber = reader.ReadUInt32BE();
        uint segmentCount = reader.ReadUInt32BE();
        UdifID segmentID = UdifID.Read(reader);

        UdifChecksum dataForkChecksum = UdifChecksum.Read(reader);

        ulong xmlOffset = reader.ReadUInt64BE();
        ulong xmlLength = reader.ReadUInt64BE();

        reader.Skip(0x78);

        UdifChecksum masterChecksum = UdifChecksum.Read(reader);

        uint imageVariant = reader.ReadUInt32BE();
        ulong sectorCount = reader.ReadUInt64BE();

        reader.Skip(12);

        return new(
            version, headerSize, flags,
            runningDataFork, dataForkOffset, dataForkLength, resourceForkOffset, resourceForkLength,
            segmentNumber, segmentCount, segmentID,
            dataForkChecksum,
            xmlOffset, xmlLength,
            masterChecksum,
            imageVariant, sectorCount);
    }
}
