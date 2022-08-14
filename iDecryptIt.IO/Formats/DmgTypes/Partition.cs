/* =============================================================================
 * File:   Partition.cs
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

internal record Partition(
    ushort Signature, // 'PM'
    uint MapBlockCount,
    uint PartitionStart,
    uint PartitionBlockCount,
    string PartitionName,
    string PartitionType,
    uint DataStart,
    uint DataCount,
    uint PartitionStatus,
    uint BootStart,
    uint BootSize,
    uint[] BootAddress,
    uint[] BootEntry,
    uint BootChecksum,
    string Processor,
    uint BootCode)
{
    public static Partition Read(BiEndianBinaryReader reader)
    {
        ushort sig = reader.ReadUInt16BE();
        reader.Skip(2);
        uint mapBlockCount = reader.ReadUInt32BE();
        uint partitionStart = reader.ReadUInt32BE();
        uint partitionBlockCount = reader.ReadUInt32BE();
        string partitionName = reader.ReadBytes(32).ToStringNoTrailingNulls();
        string partitionType = reader.ReadBytes(32).ToStringNoTrailingNulls();
        uint dataStart = reader.ReadUInt32BE();
        uint dataCount = reader.ReadUInt32BE();
        uint partitionStatus = reader.ReadUInt32BE();
        uint bootStart = reader.ReadUInt32BE();
        uint bootSize = reader.ReadUInt32BE();
        uint bootAddress0 = reader.ReadUInt32BE();
        uint bootAddress1 = reader.ReadUInt32BE();
        uint bootEntry0 = reader.ReadUInt32BE();
        uint bootEntry1 = reader.ReadUInt32BE();
        uint bootChecksum = reader.ReadUInt32BE();
        string processor = reader.ReadBytes(16).ToStringNoTrailingNulls();
        uint bootCode = reader.ReadUInt32BE();
        reader.Skip(372); // pad to 0x200

        return new(
            sig, mapBlockCount, partitionStart, partitionBlockCount, partitionName, partitionType, dataStart, dataCount,
            partitionStatus, bootStart, bootSize, new[] { bootAddress0, bootAddress1 },
            new[] { bootEntry0, bootEntry1 }, bootChecksum, processor, bootCode);
    }
}
