/* =============================================================================
 * File:   BlkxResource.cs
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

using iDecryptIt.IO.Helpers;

namespace iDecryptIt.IO.DmgTypes;

internal record BlkxResource(
    uint BlockSignature,
    uint InfoVersion,
    ulong FirstSectorNumber,
    ulong SectorCount,
    ulong DataStart,
    uint DecompressBufferRequested,
    uint BlockDescriptor,
    UdifChecksum Checksum,
    BlkxRun[] Runs)
{
    public static BlkxResource Read(BigEndianBinaryReader reader)
    {
        uint blockSignature = reader.ReadUInt32();
        uint infoVersion = reader.ReadUInt32();
        ulong firstSector = reader.ReadUInt64();
        ulong sectorCount = reader.ReadUInt64();
        ulong dataStart = reader.ReadUInt64();
        uint decompressBufferReq = reader.ReadUInt32();
        uint blockDescriptor = reader.ReadUInt32();
        reader.Skip(4 * 6);
        UdifChecksum checksum = UdifChecksum.Read(reader);
        BlkxRun[] runs = new BlkxRun[reader.ReadUInt32()];
        for (int i = 0; i < runs.Length; i++)
            runs[i] = BlkxRun.Read(reader);

        return new(blockSignature, infoVersion, firstSector, sectorCount, dataStart, decompressBufferReq,
            blockDescriptor, checksum, runs);
    }
}
