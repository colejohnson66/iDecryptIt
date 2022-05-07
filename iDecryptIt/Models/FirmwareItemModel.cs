/* =============================================================================
 * File:   FirmwareItemModel.cs
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

using iDecryptIt.Shared;
using System.Diagnostics;

namespace iDecryptIt.Models;

public record FirmwareItemModel(
    FirmwareItemType ItemKind,
    string? FileName,
    bool Encrypted,
    string? IV,
    string? Key)
{
    public static FirmwareItemModel FromFirmwareItem(FirmwareItemType type, FirmwareItem item)
    {
        Debug.Assert(type is not (FirmwareItemType.RootFS or FirmwareItemType.RootFSBeta));
        return new(type, item.Filename, item.Encrypted, item.IVKey?.IV, item.IVKey?.Key);
    }

    public static FirmwareItemModel FromRootFS(FirmwareItemType type, RootFS item)
    {
        Debug.Assert(type is FirmwareItemType.RootFS or FirmwareItemType.RootFSBeta);
        return new(type, item.Filename, item.Encrypted, null, item.Key);
    }
}
