/* =============================================================================
 * File:   Lzss.cs
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

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;

namespace iDecryptIt.IO.Helpers;

[PublicAPI]
internal static class Lzss
{
    private const int N = 4096; // size of ring buffer - must be a power of 2
    private const int F = 18; // upper limit for match length
    private const int THRESHOLD = 2; // encode string into position and length if match_length is greater than this

    // ReSharper disable CommentTypo
    // Based off of Haruhiko Okumura's LZSS.C which is in the public domain
    // ReSharper enable CommentTypo

    internal static byte[] Decompress(byte[] input, int expectedLength)
    {
        List<byte> dest = new(expectedLength);
        using MemoryStream src = new(input);

        byte[] buf = new byte[N - 1 + F];
        Array.Fill(buf, (byte)0x20, 0, N - F); // space
        int bufPos = N - F; // "r" in the original C

        uint flags = 0;
        while (true)
        {
            if (((flags >>= 1) & 0x100) is 0)
            {
                int b = src.ReadByte();
                if (b is -1)
                    break;

                flags = (uint)b | 0xFF00;
            }

            if ((flags & 1) is not 0)
            {
                int b = src.ReadByte();
                if (b is -1)
                    break;

                dest.Add((byte)b);
                buf[bufPos++] = (byte)b;
                bufPos &= N - 1;
            }
            else
            {
                int i = src.ReadByte();
                int j = src.ReadByte();
                if (i is -1 || j is -1)
                    break;

                i |= (j & 0xF0) << 4;
                j = (j & 0x0F) + THRESHOLD;
                for (int k = 0; k <= j; k++)
                {
                    byte b = buf[(i + k) & (N - 1)];
                    dest.Add(b);
                    buf[bufPos++] = b;
                    bufPos &= N - 1;
                }
            }
        }

        return dest.ToArray();
    }
}
