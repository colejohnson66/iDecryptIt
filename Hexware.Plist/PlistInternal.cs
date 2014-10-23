/* =============================================================================
 * File:   PlistInternal.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2012 Cole Johnson
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
using System;

namespace Hexware.Plist
{
    /// <summary>
    /// Internal classes used by <see cref="Hexware.Plist"/>
    /// </summary>
    internal class PlistInternal
    {
        /// <summary>
        /// Merges two one-dimensional <typeparamref name="T"/> arrays into one and returns the result in <paramref name="left"/>
        /// </summary>
        /// <typeparam name="T">The type of array to merge</typeparam>
        /// <param name="left">The left side of the array in the end</param>
        /// <param name="right">The right side of the array in the end</param>
        internal static void Merge<T>(ref T[] left, ref T[] right)
        {
            int origlength = left.Length;

            // Make left the required size
            Array.Resize<T>(ref left, left.Length + right.Length);

            // Move right into tag at the end of left's original length
            Array.Copy(right, 0, left, origlength, right.Length);
        }
    }
}
