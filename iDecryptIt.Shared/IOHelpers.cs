/* =============================================================================
 * File:   IOHelpers.cs
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
using System.Diagnostics.Contracts;
using System.Text;

namespace iDecryptIt.Shared;

[PublicAPI]
public static class IOHelpers
{
    public const string HEADER_BUNDLE = "iDecryptItBundle";
    public const string HEADER_KEY_FILE = "iDecryptItKeyFil";
    public const string HEADER_HAS_KEYS = "iDecryptItHasKey";

    public static byte[] DecodeFromHexString(string str)
    {
        Contract.Assert(str.Length % 2 is 0);
        byte[] ret = new byte[str.Length / 2];
        for (int i = 0; i < ret.Length; i++)
        {
            ret[i] = (byte)(
                (DecodeHexDigit(str[i * 2]) << 4) |
                DecodeHexDigit(str[i * 2 + 1]));
        }
        return ret;
    }

    private static int DecodeHexDigit(char c) =>
        c switch
        {
            >= '0' and <= '9' => c - '0',
            >= 'A' and <= 'F' => c - 'A' + 10,
            >= 'a' and <= 'f' => c - 'a' + 10,
            _ => throw new ArgumentException($"Character '{c}' is not a hex digit.", nameof(c)),
        };

    public static string EncodeToHexString(byte[] arr)
    {
        StringBuilder str = new(arr.Length * 2);
        foreach (byte b in arr)
            str.Append($"{b:x2}");
        return str.ToString();
    }
}
