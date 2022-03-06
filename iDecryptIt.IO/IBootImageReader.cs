using iDecryptIt.IO.Helpers;
using JetBrains.Annotations;
using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace iDecryptIt.IO;

[PublicAPI]
public class IBootImageReader : IDisposable
{
    private static byte[] MAGIC = { (byte)'i', (byte)'B', (byte)'o', (byte)'o', (byte)'t', (byte)'I', (byte)'m', 0 };
    private static byte[] MAGIC_LZSS = { (byte)'s', (byte)'s', (byte)'z', (byte)'l' };
    private static byte[] MAGIC_ARGB = { (byte)'b', (byte)'g', (byte)'r', (byte)'a' };
    private static byte[] MAGIC_GRAY = { (byte)'y', (byte)'e', (byte)'r', (byte)'g' };

    private readonly Stream _input;

    private byte[] _payload = Array.Empty<byte>();

    private IBootImageReader(Stream input)
    {
        if (!input.CanSeek)
            throw new ArgumentException("Input must be seekable.", nameof(input));

        _input = input;
        _input.Position = 0;

        ParseHeader();
        ExtractPayload();
    }

    public static IBootImageReader Parse(Stream input) =>
        new(input);

    private void ParseHeader()
    {
        byte[] header = new byte[0x40];
        if (_input.Read(header) != 0x40)
            throw new EndOfStreamException("Unexpected EOF while reading header.");

        Span<byte> headerSpan = header.AsSpan();

        // magic
        if (!MAGIC.SequenceEqual(header[..8]))
            throw new InvalidDataException("Input file is not an \"iBootImage\" file.");
        if (!MAGIC_LZSS.SequenceEqual(header[8..0xC]))
            throw new InvalidDataException($"Unknown compression format: \"{(char)header[0xB]}{(char)header[0xA]}{(char)header[9]}{(char)header[8]}\"");

        // format
        byte[] format = header[0xC..0x10];
        if (MAGIC_ARGB.SequenceEqual(format))
            Format = IBootImageFormat.Color;
        else if (MAGIC_GRAY.SequenceEqual(format))
            Format = IBootImageFormat.Grey;
        else
            throw new InvalidDataException($"Unknown color format: \"{(char)format[3]}{(char)format[2]}{(char)format[1]}{(char)format[0]}\"");

        // width + height
        Width = BitConverter.ToUInt16(headerSpan[0x10..0x12]);
        Height = BitConverter.ToUInt16(headerSpan[0x12..0x14]);

        // sanity check
        SpuriousDataInHeaderPadding = header.Skip(0x14).Any(b => b is not 0);
    }

    private void ExtractPayload()
    {
        Contract.Assert(_input.Position is 0x40);
        byte[] payload = new byte[(int)_input.Length - 0x40];
        if (_input.Read(payload) != payload.Length)
            throw new EndOfStreamException("Unexpected EOF while reading payload.");

        // gray images are two bytes per pixel; color are four
        int expectedSize = Width * Height * (Format is IBootImageFormat.Color ? 4 : 2);
        _payload = Lzss.Decompress(payload);
        if (_payload.Length != expectedSize)
            throw new InvalidDataException($"Expected a decompressed length of {expectedSize}, but got a length of {_payload.Length}.");
    }

    public IBootImageFormat Format { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public bool SpuriousDataInHeaderPadding { get; private set; }

    public void Read(out byte[] payload)
    {
        payload = new byte[Length];
        Array.Copy(_payload, payload, Length);
    }

    public int Length => _payload.Length;
    public byte this[int index] => _payload[index];

#region IDisposable

    public void Dispose()
    {
        _input.Dispose();
        GC.SuppressFinalize(this);
    }

#endregion
}
