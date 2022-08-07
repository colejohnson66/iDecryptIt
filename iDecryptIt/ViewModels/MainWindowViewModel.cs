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

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using DynamicData;
using iDecryptIt.Models;
using iDecryptIt.Shared;
using iDecryptIt.Views;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;

namespace iDecryptIt.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    // don't assume the window is ready when the VM is constructed; only get the object when we first need it
    private MainWindow? _mainWindowInstance;
    private MainWindow MainWindowInstance =>
        _mainWindowInstance ??= (MainWindow)((ClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!).MainWindow;

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
        FirmwareItemDecryptCommand = ReactiveCommand.Create<iDecryptIt.Controls.FirmwareItem>(OnFirmwareItemDecrypt);

        Subscribe();

        RxApp.TaskpoolScheduler.Schedule(() =>
        {
            try
            {
                KeyBundleHelper.Init();
            }
            catch (Exception ex)
            {
                RxApp.MainThreadScheduler.Schedule(() => Program.FatalException(ex));
            }
        });
    }

    private void Subscribe()
    {
        this.WhenAnyValue(vm => vm.VKGroupSelectedItem)
            .Subscribe(
                value =>
                {
                    // clear any old values
                    VKModelEnabled = false;
                    VKModelList.Clear();
                    VKModelSelectedItem = null;

                    VKBuildEnabled = false;
                    VKBuildList.Clear();
                    VKBuildSelectedItem = null;

                    ViewKeysCommandEnabled = false;

                    if (value is null || !Device.MappingGroupToDevices.ContainsKey(value.Value))
                        return;

                    VKModelEnabled = true;
                    VKModelList.AddRange(Device.MappingGroupToDevices[value.Value]);
                });

        this.WhenAnyValue(vm => vm.VKModelSelectedItem)
            .Subscribe(
                value =>
                {
                    // clear any old values
                    VKBuildEnabled = false;
                    VKBuildList.Clear();
                    VKBuildSelectedItem = null;

                    ViewKeysCommandEnabled = false;

                    if (value is null)
                        return;

                    VKBuildEnabled = true;
                    VKBuildList.AddRange(
                        KeyBundleHelper.GetHasKeysList(value)
                            .Select(entry => new VKBuildModel(entry))
                            .OrderBy(entry => entry));

                    // load the bundle up for faster key loading
                    RxApp.TaskpoolScheduler.Schedule(() => KeyBundleHelper.EnsureBundleIsLoaded(value));
                });

        this.WhenAnyValue(vm => vm.VKBuildSelectedItem)
            .Subscribe(
                value =>
                {
                    ViewKeysCommandEnabled = value?.HasKeys ?? false;
                });
    }

    [Reactive] public bool DecryptingRootFS { get; set; } = false;
    public ReactiveCommand<Unit, Unit> DecryptingRootFSSwitchCommand { get; }
    private void OnDecryptingRootFSSwitch() =>
        DecryptingRootFS = !DecryptingRootFS;

    [Reactive] public string RootFSInput { get; set; } = "";
    [Reactive] public string RootFSOutput { get; set; } = "";
    [Reactive] public string RootFSKey { get; set; } = "";
    public ReactiveCommand<string, Unit> RootFSOpenCommand { get; }
    private void OnRootFSOpen(string parameter)
    { }
    public ReactiveCommand<Unit, Unit> RootFSCopyKeyCommand { get; }
    private void OnRootFSCopyKey()
    { }

    [Reactive] public string DecryptInput { get; set; } = "";
    [Reactive] public string DecryptOutput { get; set; } = "";
    [Reactive] public string DecryptIV { get; set; } = "";
    [Reactive] public string DecryptKey { get; set; } = "";
    public ReactiveCommand<string, Unit> DecryptOpenCommand { get; }
    private void OnDecryptOpen(string parameter)
    { }
    public ReactiveCommand<Unit, Unit> DecryptCommand { get; }
    private void OnDecrypt()
    { }

    [Reactive] public string ExtractInput { get; set; } = "";
    [Reactive] public string ExtractOutput { get; set; } = "";
    public ReactiveCommand<string, Unit> ExtractOpenCommand { get; }
    private void OnExtractOpen(string parameter)
    { }
    public ReactiveCommand<Unit, Unit> ExtractCommand { get; }
    private void OnExtract()
    { }

    public ObservableCollection<DeviceGroup> VKGroupList { get; } = new(
        Enum.GetValues<DeviceGroup>().Where(group => group is not (DeviceGroup.AudioAccessory or DeviceGroup.IBridge)));
    // ReSharper disable once CommentTypo
    [Reactive] public DeviceGroup? VKGroupSelectedItem { get; set; } // = null; // see github:reactiveui/ReactiveUI#2688
    //
    [Reactive] public bool VKModelEnabled { get; set; } = false;
    [Reactive] public ObservableCollection<Device> VKModelList { get; set; } = new();
    [Reactive] public Device? VKModelSelectedItem { get; set; } = null;
    //
    [Reactive] public bool VKBuildEnabled { get; set; } = false;
    [Reactive] public ObservableCollection<VKBuildModel> VKBuildList { get; set; } = new();
    [Reactive] public VKBuildModel? VKBuildSelectedItem { get; set; } = null;

    [Reactive] public bool ViewKeysCommandEnabled { get; set; }
    public ReactiveCommand<Unit, Unit> ViewKeysCommand { get; }
    private void OnViewKeys()
    {
        Debug.Assert(VKBuildSelectedItem?.HasKeys is true);

        Device device = VKModelSelectedItem!; // SAFETY: button can't be clicked if this is null
        string build = VKBuildSelectedItem!.Build; // ditto

        KeysHeading = device.ModelString;

        KeyEntries.Clear();
        KeyPage? page = KeyBundleHelper.ReadKeys(device, build);
        Debug.Assert(page is not null); // SAFETY: never null if the key grabber worked correctly

        if (page.RootFS is not null)
            KeyEntries.Add(FirmwareItemModel.FromRootFS(FirmwareItemType.RootFS, page.RootFS, FirmwareItemDecryptCommand));
        if (page.RootFSBeta is not null)
            KeyEntries.Add(FirmwareItemModel.FromRootFS(FirmwareItemType.RootFSBeta, page.RootFSBeta, FirmwareItemDecryptCommand));

        foreach (KeyValuePair<FirmwareItemType, FirmwareItem> item in page.FirmwareItems)
            KeyEntries.Add(FirmwareItemModel.FromFirmwareItem(item.Key, item.Value, FirmwareItemDecryptCommand));
    }

    [Reactive] public string KeysHeading { get; set; } = "";
    [Reactive] public ObservableCollection<FirmwareItemModel> KeyEntries { get; set; } = new();

    private ReactiveCommand<iDecryptIt.Controls.FirmwareItem, Unit> FirmwareItemDecryptCommand { get; }
    private void OnFirmwareItemDecrypt(iDecryptIt.Controls.FirmwareItem model)
    { }
}
