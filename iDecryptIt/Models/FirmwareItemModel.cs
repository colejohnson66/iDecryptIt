using iDecryptIt.Shared;

namespace iDecryptIt.Models;

public record FirmwareItemModel(
    FirmwareItemType ItemKind,
    string FileName,
    bool Encrypted,
    string? IV,
    string? Key);
