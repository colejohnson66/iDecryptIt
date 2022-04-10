/* =============================================================================
 * File:   SizeResource.cs
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

internal record SizeResource(
    ushort Version,
    uint IsHfs,
    byte[] Data,
    bool VolumeModified,
    ushort VolumeSignature,
    bool SizePresent)
{
    public static SizeResource Read(BigEndianBinaryReader reader)
    {
        ushort version = reader.ReadUInt16();
        uint isHfs = reader.ReadUInt32();
        reader.Skip(4);
        int length = reader.ReadUInt8();
        byte[] data = reader.ReadBytes(length);
        reader.Skip(255 - length);
        reader.Skip(2 * 4);
        bool modified = reader.ReadUInt32() is not 0;
        reader.Skip(4);
        ushort signature = reader.ReadUInt16();
        bool sizePresent = reader.ReadUInt16() is not 0;

        return new(version, isHfs, data, modified, signature, sizePresent);
    }
}
