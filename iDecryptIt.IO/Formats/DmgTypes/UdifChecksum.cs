﻿/* =============================================================================
 * File:   UdifChecksum.cs
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

internal record UdifChecksum(
    uint Type,
    uint Size,
    uint[] Data)
{
    internal static UdifChecksum Read(BiEndianBinaryReader reader)
    {
        uint type = reader.ReadUInt32BE();
        uint size = reader.ReadUInt32BE();
        uint[] data = new uint[32];
        for (int i = 0; i < 32; i++)
            data[i] = reader.ReadUInt32BE();
        return new(type, size, data);
    }
}
