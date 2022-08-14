/* =============================================================================
 * File:   FileVaultFile.cs
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

using iDecryptIt.IO.Formats.DmgTypes;
using JetBrains.Annotations;
using System;
using System.IO;
using System.Security.Cryptography;

namespace iDecryptIt.IO.Formats;

[PublicAPI]
public sealed class FileVaultFile : IDisposable
{
    private readonly BiEndianBinaryReader _input;
    private readonly Aes _aes;
    private readonly HMACSHA1 _hmac;

    private readonly FileVaultV2Header _header;
    private readonly int _lastBlockSize;

    private FileVaultFile(BiEndianBinaryReader input, byte[] fullKey)
    {
        if (fullKey.Length is not 36)
            throw new ArgumentException("Key must be 36 bytes.", nameof(fullKey));

        _input = input;

        // The 36 byte (72 char) "Root FS key" is:
        byte[] aesKey = fullKey[..16]; // a 16 byte AES-128 key
        byte[] hmacKey = fullKey[16..]; // followed by a 20 byte HMAC key

        _aes = Aes.Create();
        _aes.BlockSize = 128;
        _aes.Mode = CipherMode.CBC;
        _aes.Padding = PaddingMode.Zeros;
        _aes.IV = new byte[16]; // the HMAC of each block's index is the IV
        _aes.Key = aesKey;

        _hmac = new(hmacKey);

        _header = FileVaultV2Header.Read(_input);
        BlockSize = (int)_header.BlockSize;
        TotalBlocks = (int)((_header.DataSize + (BlockSize - 1)) / BlockSize); // round up
        _lastBlockSize = (int)(_header.DataSize - (TotalBlocks - 1) * BlockSize);
    }

    public static FileVaultFile Parse(BiEndianBinaryReader input, byte[] fullKey) =>
        new(input, fullKey);

    public int TotalBlocks { get; }
    public int BlockSize { get; }

    public byte[] ReadBlock(int blockNumber)
    {
        if (blockNumber >= TotalBlocks)
            throw new ArgumentException($"Attempt to read past the end of the data. Block {blockNumber} requested, but only {TotalBlocks} exist.", nameof(blockNumber));

        _input.Seek(_header.DataOffset + blockNumber * BlockSize);
        byte[] block = _input.ReadBytes(blockNumber == TotalBlocks - 1 ? _lastBlockSize : BlockSize);

        // compute the digest (the IV for decryption)
        byte[] hmacData = BitConverter.GetBytes(blockNumber);
        if (!BitConverter.IsLittleEndian)
            Array.Reverse(hmacData);
        Array.Reverse(hmacData);
        _hmac.Initialize();
        byte[] hmac = _hmac.ComputeHash(hmacData);

        // set to as the IV
        _aes.IV = hmac[..16];

        // decrypt
        byte[] decrypted = new byte[block.Length];
        using CryptoStream cs = new(new MemoryStream(block), _aes.CreateDecryptor(), CryptoStreamMode.Read);
        cs.ReadExact(decrypted);
        return decrypted;
    }

    public void Dispose()
    {
        _input.Dispose();
        _aes.Dispose();
        _hmac.Dispose();
    }
}
