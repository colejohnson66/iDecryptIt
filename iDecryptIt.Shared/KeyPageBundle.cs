using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace iDecryptIt.Shared;

[PublicAPI]
public class KeyPageBundle : IDisposable
{
    // buildID -> (offset, length)
    // offsets are the offset AFTER `_startOffset`
    private Dictionary<string, (int, int)> _offsets;
    private BinaryReader _reader;
    private int _startOffset;

    private KeyPageBundle(Dictionary<string, (int, int)> offsets, BinaryReader reader)
    {
        _offsets = offsets;
        _reader = reader;
        _startOffset = (int)reader.BaseStream.Position;
        Builds = _offsets.Keys.ToList().AsReadOnly();
    }

    public static KeyPageBundle Read(BinaryReader reader)
    {
        if (IOHelpers.HEADER_BUNDLE.Any(c => reader.ReadByte() != (byte)c))
            throw new InvalidDataException("Bad header.");

        // header
        Dictionary<string, (int, int)> offsets = new();
        int count = reader.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            string name = reader.ReadString();
            int offset = reader.ReadInt32();
            int length = reader.ReadInt32();
            offsets.Add(name, (offset, length));
        }

        return new(offsets, reader);
    }

    public ReadOnlyCollection<string> Builds { get; }

    public bool HasBuild(string buildID) =>
        _offsets.ContainsKey(buildID);

    public KeyPage Read(string buildID)
    {
        if (!_offsets.TryGetValue(buildID, out (int, int) value))
            throw new ArgumentException($"Build {buildID} does not exist.", nameof(buildID));

        _reader.BaseStream.Seek(_startOffset + value.Item1, SeekOrigin.Begin);
        byte[] file = _reader.ReadBytes(value.Item2);

        using BinaryReader reader = new(new MemoryStream(file), Encoding.UTF8);
        return KeyPage.Deserialize(reader);
    }

    public void Dispose()
    {
        _reader.Dispose();
        GC.SuppressFinalize(this);
    }
}
