using iDecryptIt.Models;
using iDecryptIt.Shared;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Reactive;

namespace iDecryptIt.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
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
    public ReactiveCommand<Unit, Unit> RootFSCopyKeyCommand { get; set; }
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

    [Reactive] public ObservableCollection<string> ViewKeysDeviceList { get; set; } = new();
    [Reactive] public int ViewKeysDeviceListSelected { get; set; } = -1;
    [Reactive] public ObservableCollection<string> ViewKeysModelList { get; set; } = new();
    [Reactive] public int ViewKeysModelListSelected { get; set; } = -1;
    [Reactive] public bool ViewKeysModelListEnabled { get; set; } = false;
    [Reactive] public ObservableCollection<string> ViewKeysBuildList { get; set; } = new();
    [Reactive] public int ViewKeysBuildListSelected { get; set; } = -1;
    [Reactive] public bool ViewKeysBuildListEnabled { get; set; } = false;

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
