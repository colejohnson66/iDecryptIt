/* =============================================================================
 * File:   KeyPageBundleWriter.cs
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
