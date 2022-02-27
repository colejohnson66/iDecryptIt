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
