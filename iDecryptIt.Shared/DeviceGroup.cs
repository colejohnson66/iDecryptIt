using JetBrains.Annotations;
using System;

namespace iDecryptIt.Shared;

[PublicAPI]
public enum DeviceGroup
{
    AppleTV,
    AppleWatch,
    IBridge,
    AudioAccessory, // HomePod,
    IPad,
    IPadAir,
    IPadMini,
    IPadPro,
    IPhone,
    IPodTouch,
}

[PublicAPI]
public static class DeviceGroupExtensions
{
    public static string ModelNumberPrefix(this DeviceGroup group) =>
        group switch
        {
            DeviceGroup.AppleTV => "AppleTV",
            DeviceGroup.AppleWatch => "Watch",
            DeviceGroup.IBridge => "iBridge",
            DeviceGroup.AudioAccessory => "AudioAccessory",
            DeviceGroup.IPad or DeviceGroup.IPadAir or DeviceGroup.IPadMini or DeviceGroup.IPadPro => "iPad",
            DeviceGroup.IPhone => "iPhone",
            DeviceGroup.IPodTouch => "iPod",
            _ => throw new ArgumentException($"Unknown {nameof(DeviceGroup)}: {group}.", nameof(group)),
        };
}
