using JetBrains.Annotations;

namespace iDecryptIt.IO.Formats;

[PublicAPI]
public enum Apple8900Format
{
    BootPayloadEncryptedWithGid = 1,
    BootPayloadUnencrypted = 2,
    GenericPayloadEncryptedWithKey0x837 = 3,
    GenericPayloadUnencrypted = 4,
}
