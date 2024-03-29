﻿/* =============================================================================
 * File:   DmgFile.cs
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

using iDecryptIt.IO.FileSystem;
using iDecryptIt.IO.Formats.DmgTypes;
using JetBrains.Annotations;
using PListLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace iDecryptIt.IO.Formats;

[PublicAPI]
public sealed class DmgFile : IDisposable
{
    internal const int SECTOR_SIZE = 512;

    private const uint BLOCK_ZLIB = 0x8000_0005u;
    private const uint BLOCK_RAW = 1;
    private const uint BLOCK_IGNORE = 2;
    private const uint BLOCK_COMMENT = 0x7FFF_FFFEu;
    private const uint BLOCK_TERMINATOR = 0xFFFF_FFFFu;

    private readonly BiEndianBinaryReader _input;

    private readonly UdifResourceFile _resourceFile;
    private readonly PListDictionary _resourceFork;
    private readonly List<DmgResource> _resources;
    private readonly List<BlkxResource> _blkxResources;
    private readonly DeviceDescriptorRecord _deviceDescriptor;
    private readonly Partition _primaryPartition;
    private readonly long _primaryPartitionStart;

    private byte[] _cachedSectors = Array.Empty<byte>();
    private long _cachedSectorsStart;
    private long _cachedSectorsCount;

    private DmgFile(BiEndianBinaryReader input)
    {
        _input = input;

        // UdifResourceFile structs are 0x200 bytes and located at the end of the file
        _input.Seek(-0x200, SeekOrigin.End);
        _resourceFile = UdifResourceFile.Read(_input);

        _resourceFork = ReadResourceFork();
        _resources = ParseResourceFork();
        _blkxResources = _resources.Where(res => res.Data is BlkxResource).Select(res => (BlkxResource)res.Data).ToList();

        (_primaryPartition, _deviceDescriptor) = FindPrimaryPartition();
        _primaryPartitionStart = _primaryPartition.PartitionStart * _deviceDescriptor.BlockSize;
    }

    public static DmgFile Parse(BiEndianBinaryReader input) =>
        new(input);

    private PListDictionary ReadResourceFork()
    {
        _input.Seek(_resourceFile.XmlOffset, SeekOrigin.Begin);

        byte[] fork = _input.ReadBytes(_resourceFile.XmlLength);
        PListDocument doc = new(Encoding.ASCII.GetString(fork));

        Debug.Assert(doc.Value.Type is PListElementType.Dictionary);
        Dictionary<string, IPListElement> root = ((PListDictionary)doc.Value).Value;

        Debug.Assert(root.Keys.Count is 1 && root.Keys.First() is "resource-fork");
        Debug.Assert(root.Values.First().Type is PListElementType.Dictionary);
        return (PListDictionary)root["resource-fork"];
    }

    private List<DmgResource> ParseResourceFork()
    {
        List<DmgResource> resources = new();

        // ignore anything else
        foreach (KeyValuePair<string, IPListElement> kvp in _resourceFork.Value.Where(kvp => kvp.Key is "blkx" or "size" or "cSum"))
        {
            // all `kvp.Value` are `PListArray<PListDictionary>`
            Debug.Assert(kvp.Value.Type is PListElementType.Array);
            PListArray value = (PListArray)kvp.Value;

            foreach (IPListElement blkx in value.Value)
            {
                PListDictionary blkx1 = (PListDictionary)blkx;
                PListString attributes = (PListString)blkx1.Value["Attributes"];
                PListData data = (PListData)blkx1.Value["Data"];
                PListString id = (PListString)blkx1.Value["ID"];
                PListString name = (PListString)blkx1.Value["Name"];

                using BiEndianBinaryReader dataReader = new(data.Value);
                object dataObj = kvp.Key switch
                {
                    "blkx" => BlkxResource.Read(dataReader),
                    "size" => SizeResource.Read(dataReader),
                    "cSum" => CSumResource.Read(dataReader),
                    _ => throw new(),
                };

                Debug.Assert(attributes.Value.StartsWith("0x"));
                resources.Add(new(
                    uint.Parse(attributes.Value[2..]),
                    dataObj,
                    int.Parse(id.Value),
                    name.Value));
            }
        }

        return resources;
    }

    private (Partition, DeviceDescriptorRecord) FindPrimaryPartition()
    {
        byte[] ddmSector = ReadSector(0);
        DeviceDescriptorRecord ddm;
        using (BiEndianBinaryReader reader = new(ddmSector))
            ddm = DeviceDescriptorRecord.Read(reader);
        Debug.Assert(ddm.Signature is 0x4552); // 'ER'

        int sectorsPerBlock = ddm.BlockSize / SECTOR_SIZE;

        // we need to read the first partition to know how many there are...
        // Why, Apple? Why not put it in the DeviceDescriptorRecord?
        Partition[] partitions = new Partition[1];
        byte[] partition0 = ReadSector(1);
        using (BiEndianBinaryReader reader = new(partition0))
            partitions[0] = Partition.Read(reader);
        Debug.Assert(partitions[0].Signature is 0x504D); // 'PM'

        Array.Resize(ref partitions, (int)partitions[0].MapBlockCount);
        byte[] allPartitions = ReadSectors(1, partitions.Length * sectorsPerBlock);
        using (BiEndianBinaryReader reader = new(allPartitions))
        {
            for (int i = 0; i < partitions.Length; i++)
            {
                Partition part = Partition.Read(reader);
                Debug.Assert(part.Signature is 0x504D); // 'PM'
                reader.Skip(ddm.BlockSize - SECTOR_SIZE);
                partitions[i] = part;
            }
        }

        foreach (Partition part in partitions)
        {
            if (part.PartitionType is "Apple_HFS" or "Apple_HFSX")
                return (part, ddm);
        }
        throw new InvalidDataException("Cannot find the Apple HFS/HFSX partition.");
    }

    private byte[] GetRun(BlkxResource blkx, BlkxRun run)
    {
        byte[] runData = new byte[SECTOR_SIZE * run.SectorCount];
        _input.Seek((long)(blkx.DataStart + run.CompressOffset), SeekOrigin.Begin);

        switch (run.Type)
        {
            case BLOCK_ZLIB:
                // use a Span so we can null pad the run data
                byte[] compressed = new byte[runData.Length];
                _input.ReadBytes(compressed.AsSpan(..(int)run.CompressLength));

                using (ZLibStream zlib = new(new MemoryStream(compressed), CompressionMode.Decompress, false))
                    zlib.ReadExact(runData);
                break;

            case BLOCK_RAW:
                // use a Span so we can null pad the run data
                _input.ReadBytes(runData.AsSpan(..(int)run.CompressLength));
                break;

            case BLOCK_IGNORE:
            case BLOCK_COMMENT:
            case BLOCK_TERMINATOR:
                break;

            default:
                throw new DataException("Unknown format.");
        }

        return runData;
    }

    private void CacheSector(long sector)
    {
        if (sector >= Sectors)
            throw new ArgumentException($"Attempt to read past the end of the data. Sector {sector} requested, but only {Sectors} exist.", nameof(sector));

        // nothing to do
        if (sector >= _cachedSectorsStart && sector < _cachedSectorsStart + _cachedSectorsCount)
            return;

        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (BlkxResource blkx in _blkxResources)
        {
            long firstSector = (long)blkx.FirstSectorNumber;
            if (sector < firstSector || sector >= firstSector + (long)blkx.SectorCount)
                continue;

            foreach (BlkxRun run in blkx.Runs)
            {
                long thisRunStart = firstSector + (long)run.SectorStart;
                if (sector < thisRunStart ||
                    sector >= thisRunStart + (long)run.SectorCount)
                    continue;

                _cachedSectors = GetRun(blkx, run);
                _cachedSectorsStart = thisRunStart;
                _cachedSectorsCount = (long)run.SectorCount;
                return;
            }
        }

        throw new InvalidOperationException($"Sector {sector} requested, but not found.");
    }

    internal byte[] ReadSector(long sector)
    {
        CacheSector(sector);
        int cacheOffset = SECTOR_SIZE * (int)(sector - _cachedSectorsStart);
        byte[] result = new byte[SECTOR_SIZE];
        Array.Copy(_cachedSectors, cacheOffset, result, 0, SECTOR_SIZE);
        return result;
    }

    internal byte[] ReadSectors(long startSector, int count)
    {
        byte[] res = new byte[SECTOR_SIZE * count];
        for (int i = 0; i < count; i++)
        {
            byte[] sector = ReadSector(startSector + i);
            Array.Copy(sector, 0, res, SECTOR_SIZE * i, SECTOR_SIZE);
        }
        return res;
    }

    public int RunCount => _blkxResources.Sum(res => res.Runs.Length);
    public long Sectors => _blkxResources.Sum(res => (long)res.SectorCount);
    public long Length => Sectors * SECTOR_SIZE;

    public FileSystemReaderBase OpenFileSystem()
    {
        if (_primaryPartition.PartitionType is "Apple_HFS" or "Apple_HFSX")
            return new HfsFileSystem(this, _primaryPartition.PartitionType is "Apple_HFSX");
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        _input.Dispose();
    }
}
