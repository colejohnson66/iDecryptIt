/* =============================================================================
 * File:   KeyBundleHelper.cs
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
using iDecryptIt.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace iDecryptIt;

public static class KeyBundleHelper
{
    private static readonly object _lock = new();
    private static bool _initialized = false;

    private static readonly IAssetLoader _loader = AvaloniaLocator.Current.GetService<IAssetLoader>()!;
    private static ReadOnlyDictionary<Device, ReadOnlyCollection<HasKeysEntry>>? _hasKeysDictionary;

    private static readonly Dictionary<Device, KeyPageBundle> _readBundles = new();
    private static readonly List<Device> _readBundlesOrder = new(MAX_LOADED_BUNDLES);
    private const int MAX_LOADED_BUNDLES = 8;

    public static void Init()
    {
        lock (_lock)
        {
            if (_initialized)
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
                return;
            }

            using BinaryReader reader = new(_loader.Open(new("avares://iDecryptIt/Assets/Keys/HasKeys.bin")));
            if (reader.BaseStream.Length < IOHelpers.HEADER_HAS_KEYS.Length ||
                IOHelpers.HEADER_HAS_KEYS.Any(c => reader.ReadByte() != (byte)c))
                throw new FormatException("\"Has keys\" file is corrupt. Please redownload iDecryptIt.");

            try
            {
                Dictionary<Device, ReadOnlyCollection<HasKeysEntry>> hasKeys = new();
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    List<HasKeysEntry> entries = new();

                    Device device = Device.Parse(reader.ReadString());
                    int entryCount = reader.ReadInt32();
                    while (entryCount-- > 0)
                        entries.Add(HasKeysEntry.Deserialize(reader));

                    hasKeys.Add(device, new(entries));
                }

                _hasKeysDictionary = new(hasKeys);
                _initialized = true;
            }
            catch (Exception ex)
            {
                throw new FormatException("\"Has keys\" file is corrupt. Please redownload iDecryptIt.", ex);
            }
        }
    }

    public static ReadOnlyCollection<HasKeysEntry> GetHasKeysList(Device device)
    {
        lock (_lock)
        {
            if (!_initialized)
                throw new InvalidOperationException($"{nameof(KeyBundleHelper)}.{nameof(Init)} has not been called yet.");

            return _hasKeysDictionary![device];
        }
    }

    public static void EnsureBundleIsLoaded(Device device)
    {
        lock (_lock)
        {
            if (!_initialized)
                throw new InvalidOperationException($"{nameof(KeyBundleHelper)}.{nameof(Init)} has not been called yet.");

            EnsureBundleIsLoadedCore(device);
        }
    }

    private static void EnsureBundleIsLoadedCore(Device device)
    {
        // this method does not have a lock as `ReadKeys` acquires it for us
        // however, due to the need to expose this as a public function, `EnsureBundleIsLoaded` exists to wrap this one

        if (_readBundles.ContainsKey(device))
            return;

        // if a file is missing, something is wrong; the key grabber writes all the devices (even if they are empty)
        // don't use a using block here; we want to keep the Stream open after returning
        BinaryReader reader = new(_loader.Open(new($"avares://iDecryptIt/Assets/Keys/{device.ModelString}.bin")));
        _readBundles.Add(device, KeyPageBundle.Open(reader));
        _readBundlesOrder.Add(device);

        // if too many are open, close the oldest
        if (_readBundlesOrder.Count > MAX_LOADED_BUNDLES)
        {
            Device oldestDevice = _readBundlesOrder.First();
            _readBundlesOrder.RemoveAt(0);

            // then close it
            _readBundles.Remove(oldestDevice, out KeyPageBundle? bundleToClose);
            bundleToClose!.Dispose();
        }
    }

    public static KeyPage ReadKeys(Device device, string build)
    {
        lock (_lock)
        {
            if (!_initialized)
                throw new InvalidOperationException($"{nameof(KeyBundleHelper)}.{nameof(Init)} has not been called yet.");

            EnsureBundleIsLoadedCore(device);
            KeyPageBundle bundle = _readBundles[device];
            Debug.Assert(bundle.HasBuild(build));
            return bundle.Read(build);
        }
    }
}
