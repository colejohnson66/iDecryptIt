using System;
using System.IO;
using System.Text;

namespace iDecryptIt.IO.Helpers;

internal class BigEndianBinaryReader
{
    private readonly Stream _stream;

    public BigEndianBinaryReader(Stream input)
    {
        _stream = input;
    }

    public void Skip(int count)
    {
        _stream.Position += count;
    }

    public string ReadAsciiChars(int count)
    {
        byte[] bytes = new byte[count];
        if (_stream.Read(bytes) != count)
            throw new EndOfStreamException("Unexpected EOF.");
        return Encoding.ASCII.GetString(bytes);
    }

    public uint ReadUInt32()
    {
        byte[] buffer = new byte[4];
        if (_stream.Read(buffer) != 4)
            throw new EndOfStreamException("Unexpected EOF.");
        Array.Reverse(buffer);
        return BitConverter.ToUInt32(buffer);
    }

    public ulong ReadUInt64()
    {
        byte[] buffer = new byte[8];
        if (_stream.Read(buffer) != 8)
            throw new EndOfStreamException("Unexpected EOF.");
        Array.Reverse(buffer);
        return BitConverter.ToUInt64(buffer);
    }
}
