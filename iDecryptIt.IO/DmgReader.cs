/* =============================================================================
 * File:   DmgReader.cs
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

using iDecryptIt.IO.DmgTypes;
using iDecryptIt.IO.Helpers;
using PListLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace iDecryptIt.IO;

public class DmgReader : IDisposable
{
    private readonly Stream _input;

    private readonly UdifResourceFile _resourceFile;
    private readonly PListDictionary _resourceFork;
    private readonly List<DmgResource> _resources;

    private DmgReader(Stream input)
    {
        if (!input.CanSeek)
            throw new ArgumentException("Input must be seekable.", nameof(input));

        _input = input;
        _input.Position = 0;

        // UdifResourceFile structs are 0x200 bytes and located at the end of the file
        _input.Seek(-0x200, SeekOrigin.End);
        _resourceFile = UdifResourceFile.Read(new(_input));

        _resourceFork = ReadResourceFork();
        _resources = ParseResourceFork();

        // TODO: dmgfile.c[lines 171+]
    }

    public static DmgReader Parse(Stream input) =>
        new(input);

    private PListDictionary ReadResourceFork()
    {
        _input.Seek((long)_resourceFile.XmlOffset, SeekOrigin.Begin);
        byte[] fork = new byte[_resourceFile.XmlLength];
        _input.Read(fork);

        PListDocument doc = new(Encoding.ASCII.GetString(fork));

        Debug.Assert(doc.Value.Type is PListElementType.Dictionary);
        PListDictionary root = (PListDictionary)doc.Value;

        Debug.Assert(root.Value.Keys.Count is 1 && root.Value.Keys.First() is "resource-fork");
        Debug.Assert(root.Value.Values.First().Type is PListElementType.Dictionary);
        return (PListDictionary)root.Value["resource-fork"];
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

                using BigEndianBinaryReader dataReader = new(new MemoryStream(data.Value));
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

#region IDisposable

    public void Dispose()
    {
        _input.Dispose();
        GC.SuppressFinalize(this);
    }

#endregion
}
