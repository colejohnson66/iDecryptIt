using iDecryptIt.IO.Helpers;

namespace iDecryptIt.IO.DmgTypes;

internal record UdifID(uint[] Data)
{
    public static UdifID Read(BigEndianBinaryReader reader)
    {
        uint[] data = new uint[4];
        for (int i = 0; i < 4; i++)
            data[i] = reader.ReadUInt32();

        return new(data);
    }
}
