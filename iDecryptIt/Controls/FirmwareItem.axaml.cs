/* =============================================================================
 * File:   FirmwareItem.axaml.cs
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

using Avalonia;
using Avalonia.Controls.Primitives;
using iDecryptIt.Shared;

namespace iDecryptIt.Controls;

public class FirmwareItem : TemplatedControl
{
    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly StyledProperty<FirmwareItemType> ItemKindProperty =
        AvaloniaProperty.Register<FirmwareItem, FirmwareItemType>(nameof(ItemKind));
    public FirmwareItemType ItemKind
    {
        get => GetValue(ItemKindProperty);
        set => SetValue(ItemKindProperty, value);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly StyledProperty<string> FileNameProperty =
        AvaloniaProperty.Register<FirmwareItem, string>(nameof(FileName));
    public string FileName
    {
        get => GetValue(FileNameProperty);
        set => SetValue(FileNameProperty, value);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly StyledProperty<bool> EncryptedProperty =
        AvaloniaProperty.Register<FirmwareItem, bool>(nameof(Encrypted));
    public bool Encrypted
    {
        get => GetValue(EncryptedProperty);
        set => SetValue(EncryptedProperty, value);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly StyledProperty<string?> IVProperty =
        AvaloniaProperty.Register<FirmwareItem, string?>(nameof(IV));
    public string? IV
    {
        get => GetValue(IVProperty);
        set => SetValue(IVProperty, value);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly StyledProperty<string?> KeyProperty =
        AvaloniaProperty.Register<FirmwareItem, string?>(nameof(Key));
    public string? Key
    {
        get => GetValue(KeyProperty);
        set => SetValue(KeyProperty, value);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly StyledProperty<string?> KBagProperty =
        AvaloniaProperty.Register<FirmwareItem, string?>(nameof(KBag));
    public string? KBag
    {
        get => GetValue(KBagProperty);
        set => SetValue(KBagProperty, value);
    }
}
