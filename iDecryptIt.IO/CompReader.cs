using JetBrains.Annotations;
using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace iDecryptIt.IO;

[PublicAPI]
public class CompReader : IDisposable
{
    private static byte[] MAGIC = { (byte)'c', (byte)'o', (byte)'m', (byte)'p' };
    private static byte[] MAGIC_LZSS = { (byte)'s', (byte)'s', (byte)'z', (byte)'l' };

    private readonly Stream _input;

    private uint _decompressedLength;
    private uint _compressedLength;
    private byte[] _payload = Array.Empty<byte>();

    private CompReader(Stream input)
    {
        if (!input.CanSeek)
            throw new ArgumentException("Input must be seekable.", nameof(input));

        _input = input;
        _input.Position = 0;

        ParseHeader();
        ExtractPayload();
    }

    public static CompReader Parse(Stream input) =>
        new(input);

    private void ParseHeader()
    {
        byte[] header = new byte[0x180];
        if (_input.Read(header) != 0x180)
            throw new EndOfStreamException("Unexpected EOF while reading header.");

        Span<byte> headerSpan = header.AsSpan();

        // magic
        if (!MAGIC.SequenceEqual(header[..MAGIC.Length]))
            throw new InvalidDataException("Input file is not a \"Comp\" file.");
        if (!MAGIC_LZSS.SequenceEqual(header[4..8]))
            throw new InvalidDataException($"Unknown compression format: \"{(char)header[0xB]}{(char)header[0xA]}{(char)header[9]}{(char)header[8]}\"");

        // TODO: checksum
        uint expectedChecksum = BitConverter.ToUInt32(headerSpan[8..0xC]);

        // lengths (stored in big endian)
        _decompressedLength = BitConverter.ToUInt32(header[0xC..0x10].Reverse().ToArray());
        _compressedLength = BitConverter.ToUInt32(header[0x10..0x14].Reverse().ToArray());

        // sanity check
        SpuriousDataInHeaderPadding = header.Skip(0x14).Any(b => b is not 0);
    }

    private void ExtractPayload()
    {
        Contract.Assert(_input.Position is 0x14);
        byte[] payload = new byte[_compressedLength];
        if (_input.Read(payload) != _compressedLength)
            throw new EndOfStreamException("Unexpected EOF while reading payload.");

        _payload = Helpers.DecompressLzss(payload);
        if (_payload.Length != _decompressedLength)
            throw new InvalidDataException($"Expected a decompressed length of {_decompressedLength}, but got a length of {_payload.Length}.");
    }
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
