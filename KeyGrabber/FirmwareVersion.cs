using System;

namespace KeyGrabber;

public struct FirmwareVersion
{
    public string Version { get; set; } = "";
    public string Build { get; set; } = "";
    public DateTime? ReleaseDate { get; set; } = null;
    public string? Url { get; set; } = null;
    public string? Hash { get; set; } = null;
    public string? FileSize { get; set; } = null;
    public bool HasKeys { get; set; } = false;

    public FirmwareVersion()
    { }
}
