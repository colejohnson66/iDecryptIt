/* =============================================================================
 * File:   BinaryPlistTrailer.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2014-2015 Cole Johnson
 * 
 * This file is part of Hexware.Plist
 * 
 * Hexware.Plist is free software: you can redistribute it and/or modify it
 *   under the terms of the GNU Lesser General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or (at your
 *   option) any later version.
 * 
 * Hexware.Plist is distributed in the hope that it will be useful, but WITHOUT
 *   ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 *   FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public
 *   License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public License
 *   along with Hexware.Plist. If not, see <http://www.gnu.org/licenses/>.
 * =============================================================================
 */
namespace Hexware.Plist
{
    internal struct BinaryPlistTrailer
    {
        // Should use `long' for offsets, but if you have
        //   a Plist over 2 GiB, you've got other problems.
        internal int OffsetTableOffsetSize; // size of object refs in object table
        internal int ReferenceOffsetSize; // size of keyref[] and objref[] values
        internal int NumberOfObjects;
        internal int RootObjectNumber; // ObjectOffsets[RootObject] == root
        internal int OffsetTableOffset;

        internal BinaryPlistTrailer(byte[] trailer)
        {
            // Bytes 0-5 are ignored...
            OffsetTableOffsetSize = trailer[6];
            ReferenceOffsetSize = trailer[7];
            NumberOfObjects = (int)BinaryPlistReader.ParseUnsignedBigEndianNumber(trailer, 8, 8);
            RootObjectNumber = (int)BinaryPlistReader.ParseUnsignedBigEndianNumber(trailer, 16, 8);
            OffsetTableOffset = (int)BinaryPlistReader.ParseUnsignedBigEndianNumber(trailer, 24, 8);
        }
    }
}