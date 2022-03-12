/* =============================================================================
 * File:   FirmwareVersion.cs
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
using System;
using System.Linq;

namespace KeyGrabber;

[PublicAPI]
public class FirmwareVersionEntry
{
    public string Version { get; set; } = "";
    public string Build { get; set; } = "";
    public DateTime? ReleaseDate { get; set; } = null;
    public string? Url { get; set; } = null;
    public string? Hash { get; set; } = null;
    public long? FileSize { get; set; } = null;
    public FirmwareVersionEntryUrl[]? KeyPages { get; set; } = null;

    public FirmwareVersionEntry CloneOneDevice(string deviceToKeep)
    {
        return new()
        {
            Version = Version,
            Build = Build,
            ReleaseDate = ReleaseDate,
            Url = Url,
            Hash = Hash,
            FileSize = FileSize,
            KeyPages = new[] { KeyPages!.First(entry => entry.Device == deviceToKeep) },
        };
    }
}

[PublicAPI]
public record FirmwareVersionEntryUrl(
    string Device,
    string? Url);
