/* =============================================================================
 * File:   KeyHelpers.cs
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
using Avalonia.Platform;
using Fizzler;
using iDecryptIt.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace iDecryptIt;

public static class KeyHelpers
{
    private static readonly IAssetLoader _loader = AvaloniaLocator.Current.GetService<IAssetLoader>()!;
    private static readonly ReadOnlyDictionary<Device, ReadOnlyCollection<HasKeysEntry>> _hasKeysDictionary;

    private static readonly object _readBundlesLock = new();
    private static readonly Dictionary<Device, KeyPageBundle> _readBundles = new();
    private static readonly LinkedList<Device> _readBundlesOrder = new(); // use a linked list to avoid array shifting
    private const int MAX_LOADED_BUNDLES = 8;

    static KeyHelpers()
    {
        // TODO: handle the loader failing
        using BinaryReader reader = new(_loader.Open(new("avares://iDecryptIt/Assets/Keys/HasKeys.bin")));
        if (IOHelpers.HEADER_HAS_KEYS.Any(c => reader.ReadByte() != (byte)c))
            throw new FormatException("Has keys file is bad."); // TODO: should this be handled?

        Dictionary<Device, ReadOnlyCollection<HasKeysEntry>> hasKeys = new();
        while (reader.BaseStream.Position != reader.BaseStream.Length)
        {
            List<HasKeysEntry> entries = new();

            Device device = Device.FromModelString(reader.ReadString());
            int entryCount = reader.ReadInt32();
            while (entryCount-- > 0)
                entries.Add(HasKeysEntry.Deserialize(reader));

            hasKeys.Add(device, new(entries));
        }

        _hasKeysDictionary = new(hasKeys);
    }

    // only exists to ensure the static constructor above runs
    public static void EnsureInit() { }

    public static ReadOnlyCollection<HasKeysEntry> GetHasKeysList(Device device) =>
        _hasKeysDictionary[device];

    public static bool HasKeys(Device device, string build) =>
        GetHasKeysList(device).Any(entry => entry.Build == build);

    public static void EnsureBundleIsLoaded(Device device)
    {
        lock (_readBundlesLock)
            EnsureBundleIsLoadedInternal(device);
    }

    private static void EnsureBundleIsLoadedInternal(Device device)
    {
        // this method does not have a lock as `ReadKeys` acquires it for us
        // however, due to the need to expose this as a public function,
        // `EnsureBundleIsLoaded` exists to wrap this one.

        if (_readBundles.ContainsKey(device))
            return;

        // if a file is missing, something is wrong; the key grabber writes all the devices (even if they are empty)
        // don't use a using block here; we want to keep the Stream open after returning
        BinaryReader reader = new(_loader.Open(new($"avares://iDecryptIt/Assets/Keys/{device.ModelString}.bin")));
        _readBundles.Add(device, KeyPageBundle.Open(reader));
        _readBundlesOrder.AddLast(device);

        // if too many are open, close the oldest
        if (_readBundlesOrder.Count > MAX_LOADED_BUNDLES)
        {
            Device oldestDevice = _readBundlesOrder.First();
            _readBundlesOrder.RemoveFirst();

            // then close it
            _readBundles.Remove(oldestDevice, out KeyPageBundle? bundleToClose);
            bundleToClose!.Dispose();
        }
    }

    public static KeyPage? ReadKeys(Device device, string build)
    {
        // prevent another thread from disposing this bundle while we're using it
        lock (_readBundlesLock)
        {
            EnsureBundleIsLoadedInternal(device);
            KeyPageBundle bundle = _readBundles[device];
            return bundle.HasBuild(build) ? bundle.Read(build) : null;
        }
    }
}
