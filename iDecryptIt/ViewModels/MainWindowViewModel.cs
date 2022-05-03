/* =============================================================================
 * File:   MainWindowViewModel.cs
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

using DynamicData;
using iDecryptIt.Models;
using iDecryptIt.Shared;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;

// ReSharper disable MemberCanBePrivate.Global

namespace iDecryptIt.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly CompositeDisposable _disposables;

    public MainWindowViewModel()
    {
        DecryptingRootFSSwitchCommand = ReactiveCommand.Create(OnDecryptingRootFSSwitch);

        RootFSOpenCommand = ReactiveCommand.Create<string>(OnRootFSOpen);
        RootFSCopyKeyCommand = ReactiveCommand.Create(OnRootFSCopyKey);

        DecryptOpenCommand = ReactiveCommand.Create<string>(OnDecryptOpen);
        DecryptCommand = ReactiveCommand.Create(OnDecrypt);

        ExtractOpenCommand = ReactiveCommand.Create<string>(OnExtractOpen);
        ExtractCommand = ReactiveCommand.Create(OnExtract);

        ViewKeysCommand = ReactiveCommand.Create(OnViewKeys);

        _disposables = new();
        Subscribe(_disposables);
    }

    private void Subscribe(CompositeDisposable disposables)
    {
        this.WhenAnyValue(vm => vm.VKGroupSelectedItem)
            .Subscribe(
                value =>
                {
                    // clear any old values
                    VKModelEnabled = false;
                    VKModelList.Clear();
                    VKModelSelectedItem = null;
                    //
                    VKBuildEnabled = false;
                    VKBuildList.Clear();
                    VKBuildSelectedItem = null;
                    //
                    ViewKeysCommandEnabled = false;

                    if (value is null || !Device.MappingGroupToDevices.ContainsKey(value.Value))
                        return;

                    VKModelEnabled = true;
                    VKModelList.AddRange(Device.MappingGroupToDevices[value.Value]);
                })
            .DisposeWith(disposables);

        this.WhenAnyValue(vm => vm.VKModelSelectedItem)
            .Subscribe(
                value =>
                {
                    // clear any old values
                    VKBuildEnabled = false;
                    VKBuildList.Clear();
                    VKBuildSelectedItem = null;
                    //
                    ViewKeysCommandEnabled = false;

                    if (value is null)
                        return;
                })
            .DisposeWith(disposables);
    }

    [Reactive] public bool DecryptingRootFS { get; set; } = false;
    public ReactiveCommand<Unit, Unit> DecryptingRootFSSwitchCommand { get; set; }
    private void OnDecryptingRootFSSwitch() =>
        DecryptingRootFS = !DecryptingRootFS;

    [Reactive] public string RootFSInput { get; set; } = "";
    [Reactive] public string RootFSOutput { get; set; } = "";
    [Reactive] public string RootFSKey { get; set; } = "";
    public ReactiveCommand<string, Unit> RootFSOpenCommand { get; set; }
    private void OnRootFSOpen(string parameter)
    { }
    public ReactiveCommand<Unit, Unit> RootFSCopyKeyCommand { get; }
    private void OnRootFSCopyKey()
    { }

    [Reactive] public string DecryptInput { get; set; } = "";
    [Reactive] public string DecryptOutput { get; set; } = "";
    [Reactive] public string DecryptIV { get; set; } = "";
    [Reactive] public string DecryptKey { get; set; } = "";
    public ReactiveCommand<string, Unit> DecryptOpenCommand { get; set; }
    private void OnDecryptOpen(string parameter)
    { }
    public ReactiveCommand<Unit, Unit> DecryptCommand { get; set; }
    private void OnDecrypt()
    { }

    [Reactive] public string ExtractInput { get; set; } = "";
    [Reactive] public string ExtractOutput { get; set; } = "";
    public ReactiveCommand<string, Unit> ExtractOpenCommand { get; set; }
    private void OnExtractOpen(string parameter)
    { }

    public ReactiveCommand<Unit, Unit> ExtractCommand { get; set; }
    private void OnExtract()
    { }

    // not `Enum.GetValues<DeviceGroup>` because some don't exist in the map
    public ObservableCollection<DeviceGroup> VKGroupList { get; } = new(Device.MappingGroupToDevices.Keys);
    [Reactive] public DeviceGroup? VKGroupSelectedItem { get; set; } // = null; // see github:reactiveu/ReactiveUI#2688
    //
    [Reactive] public bool VKModelEnabled { get; set; } = false;
    [Reactive] public ObservableCollection<Device> VKModelList { get; set; } = new();
    [Reactive] public Device? VKModelSelectedItem { get; set; } = null;
    //
    [Reactive] public bool VKBuildEnabled { get; set; } = false;
    [Reactive] public ObservableCollection<string> VKBuildList { get; set; } = new();
    [Reactive] public string? VKBuildSelectedItem { get; set; } = null;

    [Reactive] public bool ViewKeysCommandEnabled { get; set; }
    public ReactiveCommand<Unit, Unit> ViewKeysCommand { get; set; }
    private void OnViewKeys()
    { }

    [Reactive] public string KeysHeading { get; set; } = "";
    [Reactive]
    public ObservableCollection<FirmwareItemModel> KeyEntries { get; set; } = new()
    {
        new(FirmwareItemType.RootFSBeta, "", true, null, ""),
    };
}
