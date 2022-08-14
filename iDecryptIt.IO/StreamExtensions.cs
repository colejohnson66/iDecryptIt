/* =============================================================================
 * File:   StreamExtensions.cs
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

using System;
using System.IO;

namespace iDecryptIt.IO;

internal static class StreamExtensions
{
    /// <summary>
    /// Read an exact number of bytes from a stream into a specified buffer.
    /// It does so by repeatedly asking the stream for as many bytes as needed until
    ///   <paramref name="buf" />.<see cref="Span{T}.Length" /> bytes total have been read.
    /// </summary>
    /// <param name="s">The <see cref="Stream" /> to read from.</param>
    /// <param name="buf">The buffer to place the read bytes into.</param>
    /// <exception cref="EndOfStreamException">If any one read attempt reads zero bytes.</exception>
    public static void ReadExact(this Stream s, Span<byte> buf)
    {
        int i = 0;
        while (i != buf.Length)
        {
            int thisRead = s.Read(buf[i..]);
            i += thisRead;
            if (thisRead is 0)
                throw new EndOfStreamException($"Unexpected EOF in {nameof(ReadExact)}.");
        }
    }
}
