/* =============================================================================
 * File:   Common.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2014, Cole Johnson
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 *   along with this program. If not, see <http://www.gnu.org/licenses/>.
 * =============================================================================
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xpwn
{
    public class Common
    {
        public const long TimeOffsetFromUnix = 2082844800;
        public static bool IsLittleEndian = BitConverter.IsLittleEndian;

        public static long AppleToUnixTime(long time)
        {
            return time - TimeOffsetFromUnix;
        }

        public static long UnixToAppleTime(long time)
        {
            return time + TimeOffsetFromUnix;
        }

        public static void FlipEndianBig(byte[] array)
        {
            if (IsLittleEndian)
               Array.Reverse(array);
        }

        public static void FlipEndianLittle(byte[] array)
        {
            if (!IsLittleEndian)
                Array.Reverse(array);
        }

        public static byte[] HexStringToByteArray(string hexString)
        {
            int length = hexString.Length / 2;
            byte[] bytes = new byte[length];

            for (int i = 0; i < length; i++)
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

            return bytes;
        }

        public static int[] HexStringToIntArray(string hexString)
        {
            int length = hexString.Length / 2;
            int[] ints = new int[length];

            for (int i = 0; i < length; i++)
                ints[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

            return ints;
        }

        //unsigned char* decodeBase64(char* toDecode, size_t* dataLength);
        //void writeBase64(struct AbstractFile* file, unsigned char* data, size_t dataLength, int tabLength, int width);
        //char* convertBase64(unsigned char* data, size_t dataLength, int tabLength, int width);
    }

    public struct IOFunctionStruct
    {
        public byte[] Data;
        public delegate int Read(int offset, int size, byte[] buffer);
        public delegate int Write(int offset, int size, byte[] buffer);
        public delegate void Close();
    }
}