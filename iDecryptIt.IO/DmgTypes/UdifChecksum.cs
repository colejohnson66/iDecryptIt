using iDecryptIt.IO.Helpers;

namespace iDecryptIt.IO.DmgTypes;

internal record UdifChecksum(
    uint Type,
    uint Size,
    uint[] Data)
{
    internal static UdifChecksum Read(BigEndianBinaryReader reader)
    {
        uint type = reader.ReadUInt32();
        uint size = reader.ReadUInt32();
        uint[] data = new uint[32];
        for (int i = 0; i < 32; i++)
            data[i] = reader.ReadUInt32();
        return new(type, size, data);
    }
}
