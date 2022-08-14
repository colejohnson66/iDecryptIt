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

using iDecryptIt.Controls;
using iDecryptIt.Shared;
using ReactiveUI;
using System.Diagnostics;
using System.Reactive;

namespace iDecryptIt.Models;

public sealed class FirmwareItemModel
{
    public FirmwareItemType ItemKind { get; }
    public string? Filename { get; }
    public bool Encrypted { get; }
    public string? IV { get; }
    public string? Key { get; }
    public string? KBag { get; }
    public ReactiveCommand<FirmwareItemKeyBlock, Unit> DecryptCommand { get; }

    public FirmwareItemModel(FirmwareItemType type, FirmwareItem item, ReactiveCommand<FirmwareItemKeyBlock, Unit> decryptCommand)
    {
        Debug.Assert(type is not (FirmwareItemType.RootFS or FirmwareItemType.RootFSBeta));

        ItemKind = type;
        Filename = item.Filename;
        Encrypted = item.Encrypted;
        IV = item.IVKey?.IV;
        Key = item.IVKey?.Key;
        KBag = item.KBag;
        DecryptCommand = decryptCommand;
    }

    public FirmwareItemModel(FirmwareItemType type, RootFS item, ReactiveCommand<FirmwareItemKeyBlock, Unit> decryptCommand)
    {
        Debug.Assert(type is FirmwareItemType.RootFS or FirmwareItemType.RootFSBeta);

        ItemKind = type;
        Filename = item.Filename;
        Encrypted = item.Encrypted;
        IV = null;
        Key = item.Key;
        KBag = null;
        DecryptCommand = decryptCommand;
    }
}
