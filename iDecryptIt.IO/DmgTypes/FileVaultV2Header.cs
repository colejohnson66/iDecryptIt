/* =============================================================================
 * File:   FileVaultV2Header.cs
 * Author: Cole Tobin
 * =============================================================================
 * Copyright (c) 2022 Cole Tobin
 *
 * This file is part of iDecryptIt.
 *
 * iDecryptIt is free software: you can redistribute it and/or modify it under
 *   the terms of the GNU General Public License as published by the Free
 *   Software Foundation, either version 3 of the License, or (at your option)
 *   any later version.
 *
 * iDecryptIt is distributed in the hope that it will be useful, but WITHOUT
 *   ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 *   FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for
 *   more details.
 *
 * You should have received a copy of the GNU General Public License along with
 *   iDecryptIt. If not, see <http://www.gnu.org/licenses/>.
 * =============================================================================
 */

using iDecryptIt.IO.Helpers;
using System.Diagnostics;
using System.IO;

namespace iDecryptIt.IO.DmgTypes;

// AES key is the first 16 bytes (32 chars) of the Root FS key
// HMAC key is the next 20 bytes (40 chars)
internal record FileVaultV2Header(
    uint Version,
    uint EncryptedIVSize,
    UdifID Uuid,
    uint BlockSize,
    ulong DataSize,
    ulong DataOffset,
    uint KdfAlgorithm,
    uint KdfPRngAlgorithm,
    uint KdfIterationCount,
    byte[] KdfSalt,
    byte[] BlobEncryptedIV,
    uint BlobEncryptedKeyBits,
    uint BlobEncryptionAlgorithm,
    uint BlobEncryptionPadding,
    uint BlobEncryptionMode,
    byte[] EncryptedKeyBlob)
{
    private const string MAGIC = "encrcdsa";

    public static FileVaultV2Header Read(BigEndianBinaryReader reader)
    {
        if (reader.ReadAsciiChars(8) is not MAGIC)
            throw new InvalidDataException("Stream does not contain a V2 FileVault header.");

        uint version = reader.ReadUInt32();
        uint encryptedIVSize = reader.ReadUInt32();
        reader.Skip(20);
        UdifID uuid = UdifID.Read(reader);
        uint blockSize = reader.ReadUInt32();
        ulong dataSize = reader.ReadUInt64();
        ulong dataOffset = reader.ReadUInt64();
        reader.Skip(0x260);
        uint kdfAlgorithm = reader.ReadUInt32();
        uint kdfRngAlgo = reader.ReadUInt32();
        uint kdfIterationCount = reader.ReadUInt32();
        //
        uint kdfSaltLen = reader.ReadUInt32();
        Debug.Assert(kdfSaltLen <= 32);
        byte[] kdfSalt = reader.ReadBytes((int)kdfSaltLen);
        reader.Skip(32 - (int)kdfSaltLen);
        //
        uint blobEncIVSize = reader.ReadUInt32();
        Debug.Assert(blobEncIVSize <= 32);
        byte[] blobEncryptedIV = reader.ReadBytes((int)blobEncIVSize);
        reader.Skip(32 - (int)blobEncIVSize);
        //
        uint blobEncryptedKeyBits = reader.ReadUInt32();
        uint blobEncryptionAlgorithm = reader.ReadUInt32();
        uint blobEncryptionPadding = reader.ReadUInt32();
        uint blobEncryptionMode = reader.ReadUInt32();
        //
        uint encKeyBlobSize = reader.ReadUInt32();
        Debug.Assert(encKeyBlobSize <= 48);
        byte[] encryptedKeyBlob = reader.ReadBytes((int)encKeyBlobSize);
        reader.Skip(48 - (int)encKeyBlobSize);

        return new(version, encryptedIVSize, uuid, blockSize, dataSize, dataOffset, kdfAlgorithm, kdfRngAlgo,
            kdfIterationCount, kdfSalt, blobEncryptedIV, blobEncryptedKeyBits, blobEncryptionAlgorithm,
            blobEncryptionPadding, blobEncryptionMode, encryptedKeyBlob);
    }
}
