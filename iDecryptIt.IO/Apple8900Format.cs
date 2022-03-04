namespace iDecryptIt.IO;

public enum Apple8900Format : byte
{
    BootPayloadEncryptedWithGid = 1,
    BootPayloadUnencrypted = 2,
    GenericPayloadEncryptedWithKey0x837 = 3,
    GenericPayloadUnencrypted = 4,
}
