/* =============================================================================
 * File:   HasKeysEntry.cs
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
using System.IO;
using System.Text.Json;

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

    public void Serialize(Utf8JsonWriter writer)
    {
        // these are serialized as a tuple to save whitespace
        writer.WriteStringValue(Version);
        writer.WriteStringValue(Build);
        writer.WriteBooleanValue(HasKeys);
    }

    public static HasKeysEntry Deserialize(BinaryReader reader)
    {
        string version = reader.ReadString();
        string build = reader.ReadString();
        bool hasKeys = reader.ReadBoolean();
        return new(version, build, hasKeys);
    }
}
