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

namespace iDecryptIt.IO.Formats.DmgTypes;

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
    public static BlkxResource Read(BiEndianBinaryReader reader)
    {
        uint blockSignature = reader.ReadUInt32BE();
        uint infoVersion = reader.ReadUInt32BE();
        ulong firstSector = reader.ReadUInt64BE();
        ulong sectorCount = reader.ReadUInt64BE();
        ulong dataStart = reader.ReadUInt64BE();
        uint decompressBufferReq = reader.ReadUInt32BE();
        uint blockDescriptor = reader.ReadUInt32BE();
        reader.Skip(4 * 6);
        UdifChecksum checksum = UdifChecksum.Read(reader);
        BlkxRun[] runs = new BlkxRun[reader.ReadUInt32BE()];
        for (int i = 0; i < runs.Length; i++)
            runs[i] = BlkxRun.Read(reader);

        return new(blockSignature, infoVersion, firstSector, sectorCount, dataStart, decompressBufferReq,
            blockDescriptor, checksum, runs);
    }
}
