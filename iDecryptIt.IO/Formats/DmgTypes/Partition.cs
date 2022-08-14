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

using iDecryptIt.IO.Helpers;
using System.Linq;
using System.Text;

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
    public static Partition Read(BigEndianBinaryReader reader)
    {
        ushort sig = reader.ReadUInt16();
        reader.Skip(2);
        uint mapBlockCount = reader.ReadUInt32();
        uint partitionStart = reader.ReadUInt32();
        uint partitionBlockCount = reader.ReadUInt32();
        byte[] partitionName = reader.ReadBytes(32);
        byte[] partitionType = reader.ReadBytes(32);
        uint dataStart = reader.ReadUInt32();
        uint dataCount = reader.ReadUInt32();
        uint partitionStatus = reader.ReadUInt32();
        uint bootStart = reader.ReadUInt32();
        uint bootSize = reader.ReadUInt32();
        uint bootAddress0 = reader.ReadUInt32();
        uint bootAddress1 = reader.ReadUInt32();
        uint bootEntry0 = reader.ReadUInt32();
        uint bootEntry1 = reader.ReadUInt32();
        uint bootChecksum = reader.ReadUInt32();
        byte[] processor = reader.ReadBytes(16);
        uint bootCode = reader.ReadUInt32();
        reader.Skip(372); // pad to 0x200

        string partitionNameStr = Encoding.ASCII.GetString(partitionName.TakeWhile(b => b is not 0).ToArray());
        string partitionTypeStr = Encoding.ASCII.GetString(partitionType.TakeWhile(b => b is not 0).ToArray());
        string processorStr = Encoding.ASCII.GetString(processor.TakeWhile(b => b is not 0).ToArray());

        return new(
            sig, mapBlockCount, partitionStart, partitionBlockCount, partitionNameStr, partitionTypeStr, dataStart,
            dataCount, partitionStatus, bootStart, bootSize, new[] { bootAddress0, bootAddress1 },
            new[] { bootEntry0, bootEntry1 }, bootChecksum, processorStr, bootCode);
    }
}
