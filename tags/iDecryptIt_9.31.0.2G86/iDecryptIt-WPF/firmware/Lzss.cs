/* =============================================================================
 * File:   LzssStream.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2016 Cole Johnson
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
namespace Hexware.Programs.iDecryptIt.Firmware
{
    // Based off of Haruhiko Okumura's LZSS.C which is in the public domain
    public static class Lzss
    {
        private static readonly int N = 4096; // size of ring buffer - must be a power of 2
        private static readonly int F = 18; // upper limit for match length
        private static readonly int THRESHOLD = 2; // encode string into position and length if match_length is greater than this

        // return number of bytes decompressed
        public static int Decompress(byte[] dest, byte[] src)
        {
            byte[] textBuf = new byte[N + F - 1];
            int srcPos = 0;
            int dstPos = 0;

            for (int i = 0; i < N - F; i++)
                textBuf[i] = 0x20; // space
            
            int r = N - F;
            byte c; // current byte
            uint flags = 0;

            while (true)
            {
                if (((flags >>= 1) & 0x100) == 0)
                {
                    if (srcPos < src.Length)
                        c = src[srcPos++];
                    else
                        break;
                    flags = (uint)c | 0xFF00;
                }

                if ((flags & 1) != 0)
                {
                    if (srcPos < src.Length)
                        c = src[srcPos++];
                    else
                        break;
                    dest[dstPos++] = c;
                    textBuf[r++] = c;
                    r &= (N - 1);
                }
                else
                {
                    int i, j;
                    if (srcPos < src.Length)
                        i = src[srcPos++];
                    else
                        break;
                    if (srcPos < src.Length)
                        j = src[srcPos++];
                    else
                        break;
                    i |= ((j & 0xF0) << 4);
                    j = (j & 0x0F) + THRESHOLD;
                    for (int k = 0; k <= j; k++)
                    {
                        c = textBuf[(i + k) & (N - 1)];
                        dest[dstPos++] = c;
                        textBuf[r++] = c;
                        r &= (N - 1);
                    }
                }
            }

            return dstPos;
        }
    }
}
