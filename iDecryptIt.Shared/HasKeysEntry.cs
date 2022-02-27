using JetBrains.Annotations;
using System.IO;

namespace iDecryptIt.Shared;

[PublicAPI]
public record HasKeysEntry(
    string Version,
    string Build,
    bool HasKeys)
{
    public void Serialize(BinaryWriter writer)
    {
        writer.Write(Version);
        writer.Write(Build);
        writer.Write(HasKeys);
    }

    public static HasKeysEntry Deserialize(BinaryReader reader)
    {
        string version = reader.ReadString();
        string build = reader.ReadString();
        bool hasKeys = reader.ReadBoolean();
        return new(version, build, hasKeys);
    }
}
