/* =============================================================================
 * File:   CSumResource.cs
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

internal record CSumResource(
    ushort Version,
    uint Type,
    uint Checksum)
{
    public static CSumResource Read(BigEndianBinaryReader reader)
    {
        ushort version = reader.ReadUInt16();
        uint type = reader.ReadUInt32();
        uint checksum = reader.ReadUInt32();

        return new(version, type, checksum);
    }
}
