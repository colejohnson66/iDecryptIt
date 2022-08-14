/* =============================================================================
 * File:   DeviceDescriptorRecord.cs
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

namespace iDecryptIt.IO.Formats.DmgTypes;

internal record DeviceDescriptorRecord(
    ushort Signature,
    ushort BlockSize,
    uint BlockCount,
    ushort DeviceType,
    ushort DeviceID,
    uint Data,
    ushort DriverCount,
    uint DDBlock,
    ushort DDSize,
    ushort DDType)
{
    public static DeviceDescriptorRecord Read(BigEndianBinaryReader reader)
    {
        ushort sig = reader.ReadUInt16();
        ushort blockSize = reader.ReadUInt16();
        uint blockCount = reader.ReadUInt32();
        ushort deviceType = reader.ReadUInt16();
        ushort deviceID = reader.ReadUInt16();
        uint data = reader.ReadUInt32();
        ushort driverCount = reader.ReadUInt16();
        uint ddBlock = reader.ReadUInt32();
        ushort ddSize = reader.ReadUInt16();
        ushort ddType = reader.ReadUInt16();

        reader.Skip(DmgReader.SECTOR_SIZE - 0x1A); // pad out to SECTOR_SIZE
        return new(sig, blockSize, blockCount, deviceType, deviceID, data, driverCount, ddBlock, ddSize, ddType);
    }
}
