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
