using JetBrains.Annotations;
using System.Collections.Generic;
using System.IO;

namespace iDecryptIt.Shared;

[PublicAPI]
public sealed class KeyPageBundleWriter
{
    private MemoryStream _concatenatedFiles = new();
    // buildID -> (offset in `_concatenatedFiles`, length)
    private Dictionary<string, (int, int)> _offsets = new();

    public void AddFile(string buildID, byte[] file)
    {
        _offsets.Add(buildID, ((int)_concatenatedFiles.Length, file.Length));
        _concatenatedFiles.Write(file);
    }

    public void WriteBundle(BinaryWriter writer)
    {
        foreach (char c in IOHelpers.HEADER_BUNDLE)
            writer.Write((byte)c);

        // header
        writer.Write(_offsets.Count);
        foreach ((string name, (int offset, int length)) in _offsets)
        {
            writer.Write(name);
            writer.Write(offset);
            writer.Write(length);
        }

        writer.Write(_concatenatedFiles.ToArray());
        writer.Dispose();
    }
}
